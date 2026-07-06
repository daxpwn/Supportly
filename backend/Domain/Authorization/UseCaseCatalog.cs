using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Domain.Authorization
{
    /// <summary>
    /// Katalog svih poznatih use case id-jeva (čita konstante iz <see cref="UseCaseIds"/>).
    /// Služi UI-ju da ponudi koje dozvole se mogu dodeliti roli i validaciji pri dodeli.
    /// </summary>
    public static class UseCaseCatalog
    {
        public static IReadOnlyList<string> All { get; } =
            typeof(UseCaseIds)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(f => f.IsLiteral && !f.IsInitOnly && f.FieldType == typeof(string))
                .Select(f => (string)f.GetRawConstantValue())
                .OrderBy(x => x)
                .ToList();
    }
}
