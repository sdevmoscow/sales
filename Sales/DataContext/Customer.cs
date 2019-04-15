using System;
using System.Collections.Generic;

namespace Sales.DataContext
{
    public partial class Customer
    {
        public Customer()
        {
            SalesOrder = new HashSet<SalesOrder>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<SalesOrder> SalesOrder { get; set; }
    }
}
