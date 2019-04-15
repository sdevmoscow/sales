using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.Models
{
    public class ViewModelStatus
    {
        [DisplayName("Id")]
        public int Id { get; set; }
        [DisplayName("Статус")]
        public string Name { get; set; }
    }
}
