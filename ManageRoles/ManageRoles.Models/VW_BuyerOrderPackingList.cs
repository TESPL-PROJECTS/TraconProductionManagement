using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageRoles.Models
{
   public class VW_BuyerOrderPackingList
    {
        public long ID { get; set; }
        public string BuyerOrderName { get; set; }
        public DateTime? OPMPKGListDate { get; set; }
        public DateTime? TodayDate { get; set; }
        public int? RemainingDays { get; set; }
        public DateTime? BuyerPKGListDate { get; set; }
        public DateTime? TodaysDate { get; set; }
        public int? BuyerRemainingDays { get; set; }


    }
}
