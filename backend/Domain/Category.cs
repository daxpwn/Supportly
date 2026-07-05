using System.Collections.Generic;

namespace Domain
{
    public class Category
    {
        public short Id { get; set; }          // SMALLINT
        public string Name { get; set; }
        public short? ParentId { get; set; }     // podkategorija (self-reference)
        public short? DepartmentId { get; set; }

        public virtual Category Parent { get; set; }
        public virtual Department Department { get; set; }

        public virtual HashSet<Category> Children { get; set; } = new HashSet<Category>();
        public virtual HashSet<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
    }
}
