using System.Collections.Generic;

namespace Application.Queries
{
    public interface IGetUseCaseCatalogQuery : IQuery<object, IEnumerable<string>>
    {
    }
}
