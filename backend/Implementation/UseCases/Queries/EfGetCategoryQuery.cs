using Application.DTO;
using Application.DTO.Search;
using Application.Queries;
using Supportly.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace Implementation.UseCases.Queries
{
    public class EfGetCategoryQuery : EfUseCase, IGetCategoriesQuery
    {
        public EfGetCategoryQuery(LabDbContext context) : base(context)
        {
        }

        public string Name => "Vraca sve kategorije";

        public string Id => "get-categories";

        public IEnumerable<CategoryDTO> Execute(CategorySearch search)
        {
            // Projekcija u DTO -> EF pravi JOIN ka Statuses/Priorities (bez lazy loading-a).
            return ctx.Categories
                      .Select(t => new CategoryDTO
                      {
                           Id = t.Id,
                           Name = t.Name,
                           ParentId = t.ParentId
                      })
                      .ToList();
        }
    }
}
