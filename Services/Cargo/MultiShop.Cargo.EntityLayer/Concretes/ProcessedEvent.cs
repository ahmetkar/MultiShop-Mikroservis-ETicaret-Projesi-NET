using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.Cargo.EntityLayer.Concretes
{
    public class ProcessedEvent
    {
        public int Id { get; set; } 
        public Guid EventId { get; set; }

        public string HandlerName { get; set; } = string.Empty;

        public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;
    }
}
