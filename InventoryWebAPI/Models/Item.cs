using System;
using System.Collections.Generic;

namespace InventoryWebAPI.Models
{
    public partial class Item
    {
        public Item()
        {
            InvoiceItems = new HashSet<InvoiceItem>();
        }

        public int ItemId { get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal Tax { get; set; }

        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
    }
}
