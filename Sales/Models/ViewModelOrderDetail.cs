using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.Models
{
    public class ViewModelOrderDetail
    {
        [DisplayName("Id")]
        public int Id { get; set; }
        [DisplayName("Id заказа")]
        public int SalesOrderId { get; set; }
        [DisplayName("Id товара")]
        public int ProductId { get; set; }
        [DisplayName("Товар")]
        public string Product { get; set; }
        [DisplayName("Количество")]
        public int OrderQty { get; set; }
        [DisplayName("Цена")]
        public decimal UnitPrice { get; set; }
        [DisplayName("Дата изменения")]
        public DateTime ModifyDate { get; set; }
    }
}
