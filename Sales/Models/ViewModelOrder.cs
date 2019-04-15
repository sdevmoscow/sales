using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.Models
{
    public class ViewModelOrder
    {
        [DisplayName("Номер")]
        public int Id { get; set; }

        [DisplayName("Дата")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime OrderDate { get; set; }

        public int StatusId { get; set; }

        [DisplayName("Статус")]
        public string Status { get; set; }

        [DisplayName("Клиент")]
        public string Customer { get; set; }

        [DisplayName("Комментарий")]
        public string Comment { get; set; }

        [DisplayName("Сумма")]
        public decimal Total { get; set; }

        public List<ViewModelOrderDetail> Details { get; set; }

        public List<ViewModelStatus> Statuses { get; set; }

    }
}
