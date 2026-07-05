using Application;
using Application.Exceptions;
using System.Diagnostics;

namespace Implementation.UseCases
{
    public class UseCaseHandler
    {

        private IApplicationUser _user;

        public UseCaseHandler(IApplicationUser user)
        {
            _user = user;
        }
        private void HandleAuthorization(IUseCase useCase) 
        {
            if (!_user.AllowedUseCases.Contains(useCase.Id))
            {
                throw new UnauthorizedUseCaseException(_user.Username, useCase.Name);
            }
        } 
        public void ExecuteCommand<TRequest>(ICommand<TRequest> command, TRequest request)   
        {
            HandleAuthorization(command);

            Stopwatch stopwatch = Stopwatch.StartNew();

            stopwatch.Start();

            command.Execute(request);

            stopwatch.Stop();

            Console.WriteLine($"{_user.Username} has executed use case: {command.Name}" + stopwatch.ElapsedMilliseconds + " ms.");
        }

        public TResult ExecuteQuery<TParam,TResult>(IQuery<TParam, TResult> query, TParam request)
            where TResult : class
        {
            HandleAuthorization(query);
            Stopwatch stopwatch = Stopwatch.StartNew();
            
            stopwatch.Start();
            
            var result = query.Execute(request);

            stopwatch.Stop();

            Console.WriteLine($"{_user.Username} has executed use case: {query.Name}" + stopwatch.ElapsedMilliseconds + " ms.");

            return result;
        }
    }
}
