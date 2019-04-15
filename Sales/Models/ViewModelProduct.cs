using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.Models
{
    public class ViewModelProduct
    {
        public bool Selected { get; set; }

        [DisplayName("Id товара")]
        [Key]
        public int Id { get; set; }
        [DisplayName("Наименование товара")]
        public string Name { get; set; }
        [DisplayName("Цена")]
        public decimal ListPrice { get; set; }
        [DisplayName("Описание")]
        public string Comment { get; set; }

    }
}
