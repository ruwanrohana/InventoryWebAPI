using System;
using System.Collections.Generic;

namespace InventoryWebAPI.Models
{
    public partial class InvoiceItem
    {
        public int InvoiceItemId { get; set; }
        public int InvoiceId { get; set; }
        public int ItemId { get; set; }
        public string Note { get; set; }
        public decimal Price { get; set; }
        public decimal Tax { get; set; }
        public int Quantity { get; set; }
 
        public decimal ExclAmount
        {
            get
            {
                return (Quantity * Price);
            }

            private set { }


        }

        public decimal TaxAmount
        {
            get
            {
                return (ExclAmount * Tax) / 100;
            }

            private set { }
        }
     
        public decimal InclAmount
        {
            get
            {
                return (ExclAmount + TaxAmount);
            }

            private set { }
        }

        public virtual Invoice Invoice { get; set; }
        public virtual Item Item { get; set; }
    }
}
