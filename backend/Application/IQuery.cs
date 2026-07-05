using System;
using System.Collections.Generic;
using System.Text;

namespace Application
{
    public interface IQuery<TParam, TResponse> : IUseCase
        where TResponse : class //Generic type constraint
    {
        TResponse Execute(TParam request);
    }
}
