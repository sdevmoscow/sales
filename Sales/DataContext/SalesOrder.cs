using System;
using System.Collections.Generic;

namespace Sales.DataContext
{
    public partial class SalesOrder
    {
        public SalesOrder()
        {
            SalesOrderDetail = new HashSet<SalesOrderDetail>();
        }

        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int StatusId { get; set; }
        public int CustomerId { get; set; }
        public string Comment { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual SalesStatus Status { get; set; }
        public virtual ICollection<SalesOrderDetail> SalesOrderDetail { get; set; }
    }
}
