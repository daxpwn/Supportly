using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public short? ParentId { get; set; }
    }
}
