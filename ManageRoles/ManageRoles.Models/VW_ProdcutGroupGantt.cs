using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageRoles.Models
{
    public class VW_ProdcutGroupGantt
    {
        public long ID { get; set; }
        public string Buyername { get; set; }
        public string BuyerOrderNumberName { get; set; }
        public string Processname { get; set; }
        public string Productname { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? SPCDate { get; set; }
        public int NoDays { get; set; }
        public int DelayedDays { get; set; }
        public decimal Done { get; set; }
        public DateTime? SKUStartDate { get; set; }
        public DateTime? SKUEndDate { get; set; }
    }
}
