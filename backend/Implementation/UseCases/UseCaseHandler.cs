using Application;
using Application.Exceptions;
using Domain;
using Supportly.DataAccess;
using System;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Implementation.UseCases
{
    public class UseCaseHandler
    {
        private const int MaxPayloadLength = 2000;

        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new ByteArrayLengthConverter() }
        };

        private readonly IApplicationUser _user;
        private readonly LabDbContext _auditContext;

        // _auditContext je zasebna (transient) instanca konteksta — audit se upisuje
        // nezavisno od konteksta koji koristi sam use case.
        public UseCaseHandler(IApplicationUser user, LabDbContext auditContext)
        {
            _user = user;
            _auditContext = auditContext;
        }

        private void HandleAuthorization(IUseCase useCase)
        {
            if (!_user.AllowedUseCases.Contains(useCase.Id))
            {
                throw new UnauthorizedUseCaseException(_user.Username, useCase.Name);
            }
        }

        public void ExecuteCommand<TRequest>(ICommand<TRequest> command, TRequest request)
            => Run(command, request, () => command.Execute(request));

        public TResult ExecuteQuery<TParam, TResult>(IQuery<TParam, TResult> query, TParam request)
            where TResult : class
        {
            TResult result = null;
            Run(query, request, () => result = query.Execute(request));
            return result;
        }

        // Zajednička putanja: meri vreme, izvršava, i UVEK upiše audit red (i za pao pokušaj).
        private void Run(IUseCase useCase, object payload, Action action)
        {
            var executedAt = DateTime.UtcNow;
            var stopwatch = Stopwatch.StartNew();
            bool succeeded = false;

            try
            {
                HandleAuthorization(useCase);
                action();
                succeeded = true;
            }
            finally
            {
                stopwatch.Stop();
                Console.WriteLine($"{_user.Username} has executed use case: {useCase.Name} {stopwatch.ElapsedMilliseconds} ms.");
                WriteAudit(useCase, payload, executedAt, stopwatch.ElapsedMilliseconds, succeeded);
            }
        }

        private void WriteAudit(IUseCase useCase, object payload, DateTime executedAt, long durationMs, bool succeeded)
        {
            try
            {
                _auditContext.UseCaseLogs.Add(new UseCaseLog
                {
                    UserId = _user.Id > 0 ? _user.Id : (int?)null,   // gost -> null
                    Username = _user.Username,
                    UseCaseId = useCase.Id,
                    UseCaseName = useCase.Name,
                    ExecutedAt = executedAt,
                    DurationMs = durationMs,
                    Succeeded = succeeded,
                    Payload = SerializePayload(payload)
                });
                _auditContext.SaveChanges();
            }
            catch
            {
                // Audit ne sme da obori glavni tok izvršavanja use case-a.
            }
        }

        // Serijalizuje prosleđene vrednosti u JSON (skraćeno na MaxPayloadLength).
        private static string SerializePayload(object payload)
        {
            if (payload == null) return null;

            try
            {
                var json = JsonSerializer.Serialize(payload, payload.GetType(), _jsonOptions);
                if (string.IsNullOrEmpty(json) || json == "{}" || json == "null")
                    return null;

                return json.Length > MaxPayloadLength
                    ? json.Substring(0, MaxPayloadLength)
                    : json;
            }
            catch
            {
                return null;
            }
        }

        // Ne upisuj sadržaj fajla (byte[]) kao base64 blob — samo njegovu veličinu.
        private sealed class ByteArrayLengthConverter : JsonConverter<byte[]>
        {
            public override byte[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
                => null;

            public override void Write(Utf8JsonWriter writer, byte[] value, JsonSerializerOptions options)
                => writer.WriteStringValue($"<{value.Length} bytes>");
        }
    }
}
