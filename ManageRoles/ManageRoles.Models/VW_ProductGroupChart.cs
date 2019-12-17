using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageRoles.Models
{
    public class VW_ProductGroupChart
    {
        public long ID { get; set; }

        public string Buyername { get; set; }
        public string BuyerOrderNumberName { get; set; }
        public string Processname { get; set; }
        public string Productname { get; set; }
        public decimal ProductQtyfrom { get; set; }
        public decimal FinishedQty { get; set; }
        public decimal BalanceQty { get; set; }
    }
}
