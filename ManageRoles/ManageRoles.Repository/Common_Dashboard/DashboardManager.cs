using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManageRoles.Models;
using System.Data.Entity;
using System.Web.Mvc;

namespace ManageRoles.Repository
{

    public class FinalLineInspectionManager : CommonRepository<VW_FinalLineInspectionList>
    {
        public FinalLineInspectionManager(DbContext context) : base(context) { }

        public List<VW_FinalLineInspectionList> GetList()
        {
            var lst = this.GetAll().ToList();
            if (lst == null)
            {
                return new List<VW_FinalLineInspectionList>();
            }
            else
            {
                return lst.Where(c => c.RemainingDays != null && c.RemainingDays > 0 && c.RemainingDays <= 20).ToList();
            }
        }
    }

    public class InLineInspectionManager : CommonRepository<VW_InLineInspectionList>
    {
        public InLineInspectionManager(DbContext context) : base(context) { }

        public List<VW_InLineInspectionList> GetList()
        {
            var lst = this.GetAll(c => c.RemainingDays != null && c.RemainingDays > 0 && c.RemainingDays <= 20).ToList();
            return lst;
        }
    }

    public class MidLineInspectionManager : CommonRepository<VW_MidLineInspectionList>
    {
        public MidLineInspectionManager(DbContext context) : base(context) { }

        public List<VW_MidLineInspectionList> GetList()
        {
            var lst = this.GetAll(c => c.RemainingDays != null && c.RemainingDays > 0 && c.RemainingDays <= 20).ToList();
            return lst;
        }
    }

    public class OrderPackingManager : CommonRepository<VW_OrderPackingList>
    {
        public OrderPackingManager(DbContext context) : base(context) { }

        public List<VW_OrderPackingList> GetList()
        {
            var lst = this.GetAll(c => c.RemainingDays != null && c.RemainingDays > 0 && c.RemainingDays <= 20 ).ToList();
            return lst;
        }
    }

    public class PKGKDManager : CommonRepository<VW_BuyerOrderPackingList>
    {
        public PKGKDManager(DbContext context) : base(context) { }

        public List<VW_BuyerOrderPackingList> GetList()
        {
            var lst = this.GetAll(c => c.RemainingDays != null && c.RemainingDays > 0 && c.RemainingDays <= 30).ToList();
            return lst;           
        }

    }

    public class PKGKDDespatchManager : CommonRepository<VW_BuyerOrderDespatchList>
    {
        public PKGKDDespatchManager(DbContext context) : base(context) { }

        public List<VW_BuyerOrderDespatchList> GetList()
        {
            var lst = this.GetAll(c => c.RemainingDays != null && c.RemainingDays > 0 && c.RemainingDays <= 30).ToList();
            return lst;
        }

    }
    public class OrdersDespatcheManager : CommonRepository<VW_OrdersDespatcheList>
    {
        public OrdersDespatcheManager(DbContext context) : base(context) { }

        public List<VW_OrdersDespatcheList> GetList()

        {
            var lst = this.GetAll(c => c.RemainingDays != null && c.RemainingDays > 0 && c.RemainingDays <= 20).ToList();
            return lst;
        }
    }

    public class UserManager : CommonRepository<Usermaster>
    {
        public UserManager(DbContext context) : base(context) { }

        public int GetTotalUserCount()
        {
            return this.GetAll().Count();
        }
    }

    public class DSProductUpdateGrid_Model : CommonModel<VW_DSProductUpdateGrid>
    {
        public SelectList ProcessList { get; set; }

        public SelectList BuyerList { get; set; }
        public SelectList BuyerOrderNumberList { get; set; }
        public string SortNameType { get; set; }
        public List<VW_DSProductUpdateGrid> ProductsList { get; set; }
    }

    public class DSProductUpdateGridManager : CommonRepository<VW_DSProductUpdateGrid>
    {
        public DSProductUpdateGridManager(DbContext context) : base(context) { }

        //public List<VW_DSProductUpdateGrid> GetList()
        //{
        //    var lst = this.GetAll(c => c.FinishedDays > 0 && c.FinishedDays <= 20).ToList();
        //    return lst;
        //}
        public List<VW_DSProductUpdateGrid> GetList(string buyername = null, string orderNo = null, string processName = null)
        {
            if (string.IsNullOrEmpty(buyername) && string.IsNullOrEmpty(orderNo) && string.IsNullOrEmpty(processName))
            {
                var lst = this.GetAll().ToList();
                return lst;
            }
            else if (!string.IsNullOrEmpty(buyername) && string.IsNullOrEmpty(orderNo) && string.IsNullOrEmpty(processName))
            {
                var lst = this.GetAll(c => c.Buyername == buyername).ToList();
                return lst;
            }
            else if (string.IsNullOrEmpty(buyername) && string.IsNullOrEmpty(orderNo) && string.IsNullOrEmpty(processName))
            {
                var lst = this.GetAll(c => c.BuyerOrderNumberName == orderNo).ToList();
                return lst;
            }
            else if (!string.IsNullOrEmpty(orderNo) && string.IsNullOrEmpty(processName))
            {
                var lst = this.GetAll(c => c.BuyerOrderNumberName == orderNo).ToList();
                return lst;
            }
            else if (string.IsNullOrEmpty(buyername) && string.IsNullOrEmpty(orderNo) && !string.IsNullOrEmpty(processName))
            {
                var lst = this.GetAll(c => c.Processname == processName).ToList();
                return lst;
            }
            else
            {
                var lst = this.GetAll(c => c.Buyername == buyername && c.BuyerOrderNumberName == orderNo && c.Processname == processName).ToList();
                return lst;
            }
        }

    }
    public class DashboardModel
    {
        public List<VW_OrdersDespatcheList> LstOrdersDespatche { get; set; }
        public List<VW_OrderPackingList> LstOrderPacking { get; set; }
        public List<VW_MidLineInspectionList> LstMidLineInspection { get; set; }
        public List<VW_InLineInspectionList> LstInLineInspection { get; set; }
        public List<VW_FinalLineInspectionList> LstFinalLineInspection { get; set; }
        public List<VW_DSProductUpdateGrid> LstDSProductUpdateGrid { get; set; }
        public List<VW_BuyerOrderPackingList> LstBuyerOrderPackingList { get; set; }
        public List<VW_BuyerOrderDespatchList> LstBuyerOrderDespatchList { get; set; }
        public List<Tuple<string, string>> LstUser { get; set; }
        public string labels { get; set; }
        public string series { get; set; }
        public SelectList BuyerOrderNumberList { get; set; }
        public SelectList ProcessList { get; set; }
        public SelectList BuyerList { get; set; }
    }
}
