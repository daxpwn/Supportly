using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO
{

    public class PagedResponse<TDto> where TDto : class
    {
        public int TotalCount { get; set; }
        public int PagesCount => (int)Math.Ceiling((decimal)TotalCount / PerPage);
        public IEnumerable<TDto> Items { get; set; }
        public int CurrentPage { get; set; }
        public int PerPage { get; set; }
    }

    //broj strana, niz


    public class PagedSearch
    {
        public int? Page { get; set; } = 1;
        public int? PerPage { get; set; } = 10;
    }

}
