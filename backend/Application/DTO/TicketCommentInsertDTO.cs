using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO
{
    public class TicketCommentInsertDTO
    {
        public long TicketId { get; set; }
        public string Body { get; set; }
        public bool IsInternal { get; set; }

        // Izlaz: popunjava ga komanda posle upisa (id kreiranog komentara).
        public long Id { get; set; }
    }
}
