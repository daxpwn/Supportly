using Application.Queries;
using Domain.Authorization;
using Supportly.DataAccess;
using System.Collections.Generic;
using System.Linq;

namespace Implementation.UseCases.Queries
{
    public class EfGetUseCaseCatalogQuery : EfUseCase, IGetUseCaseCatalogQuery
    {
        public EfGetUseCaseCatalogQuery(LabDbContext context) : base(context)
        {
        }

        public string Name => "Get use case catalog";

        public string Id => UseCaseIds.ManageRoles;

        // Katalog ne zavisi od baze — vraća sve poznate use case id-jeve.
        public IEnumerable<string> Execute(object request) => UseCaseCatalog.All.ToList();
    }
}
