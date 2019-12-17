using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageRoles.Models
{
    public class VW_ProductSummary
    {
        [Key, Column(Order = 0)]
        public string Processname { get; set; }
        [Key, Column(Order = 1)]
        public string Buyername { get; set; }
        public string BuyerOrderNumberName { get; set; }
        public decimal ProductQty { get; set; }
        public decimal FinishedQty { get; set; }
        public decimal BalanceQty { get; set; }
    }
}
