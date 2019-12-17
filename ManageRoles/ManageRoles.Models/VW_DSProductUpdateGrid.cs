using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageRoles.Models
{
    public class VW_DSProductUpdateGrid
    {
        [Key]
        public int ProductID { get; set; }
        public string Buyername { get; set; }
        public string BuyerOrderNumberName { get; set; }
        public string Processname { get; set; }
        public string Productname { get; set; }
        public DateTime SPCDate { get; set; }
        public DateTime TodayDate { get; set; }
        public int FinishedDays { get; set; }
        public decimal ProductQty { get; set; }
        public decimal FinishedQty { get; set; }
        public string Unit { get; set; }
        public int RemainingDays { get; set; }
        public decimal Done { get; set; }
    }
}
