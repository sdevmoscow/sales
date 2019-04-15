using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.Models
{
    public class ViewModelAddProducts
    {
        public int OrderId { get; set; }
        public List<Orders.Models.ViewModelProduct> Products { get; set; }
    }
}
