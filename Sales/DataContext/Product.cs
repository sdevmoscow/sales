using System;
using System.Collections.Generic;

namespace Sales.DataContext
{
    public partial class Product
    {
        public Product()
        {
            SalesOrderDetail = new HashSet<SalesOrderDetail>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal ListPrice { get; set; }
        public string Comment { get; set; }

        public virtual ICollection<SalesOrderDetail> SalesOrderDetail { get; set; }
    }
}
