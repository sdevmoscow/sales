using System;
using System.Collections.Generic;

namespace Sales.DataContext
{
    public partial class SalesOrderDetail
    {
        public int Id { get; set; }
        public int SalesOrderId { get; set; }
        public int ProductId { get; set; }
        public int OrderQty { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime ModifyDate { get; set; }

        public virtual Product Product { get; set; }
        public virtual SalesOrder SalesOrder { get; set; }
    }
}
