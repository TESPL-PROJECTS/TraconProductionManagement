using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageRoles.Models
{
    public class VW_BuyerOrderDespatchList
    {
        public long ID { get; set; }
        public string BuyerOrderName { get; set; }
        public DateTime? OPMKDListDate { get; set; }
        public DateTime? TodayDate { get; set; }
        public int? RemainingDays { get; set; }
        public DateTime? BuyerKDListDate { get; set; }
        public DateTime? TodaysDate { get; set; }
        public int? BuyerRemainingDays { get; set; }
    }
}
