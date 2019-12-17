using ManageRoles.Filters;
using ManageRoles.Models;
using ManageRoles.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ManageRoles.Controllers
{
    [AuthorizeSuperAdminandAdmin]
    [SessionExpireFilter]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            FinalLineInspectionManager context1 = new FinalLineInspectionManager(new DataContext());
            InLineInspectionManager context2 = new InLineInspectionManager(new DataContext());
            MidLineInspectionManager context3 = new MidLineInspectionManager(new DataContext());            
            OrderPackingManager context4 = new OrderPackingManager(new DataContext());
            OrdersDespatcheManager context5 = new OrdersDespatcheManager(new DataContext());
            UserManager context6 = new UserManager(new DataContext());
            DSProductUpdateGridManager context7 = new DSProductUpdateGridManager(new DataContext());
            PKGKDManager context8 = new PKGKDManager(new DataContext());
            PKGKDDespatchManager context9 = new PKGKDDespatchManager (new DataContext());
            DashboardModel model = new DashboardModel();
            model.LstFinalLineInspection = context1.GetList();
            model.LstInLineInspection = context2.GetList();
            model.LstMidLineInspection = context3.GetList();
            model.LstOrderPacking = context4.GetList();
            model.LstOrdersDespatche = context5.GetList();
            model.LstDSProductUpdateGrid = context7.GetList();
            model.LstBuyerOrderPackingList = context8.GetList();
            model.LstBuyerOrderDespatchList = context9.GetList();
            List<string> labels = new List<string>();
            labels.Add("Online User");
            labels.Add("Offline User");
            List<int> series = new List<int>();
            int onlineUsers = 0;
            int totUsers = context6.GetTotalUserCount();
            var allUser = context6.GetAll().ToList();
            if (HttpContext.Application["TotalOnlineUsers"] != null)
            {
                onlineUsers = Convert.ToInt32(HttpContext.Application["TotalOnlineUsers"]);
            }

            if (HttpContext.Application["OnlineUsers"] != null)
            {
                string user = (string)HttpContext.Application["OnlineUsers"];
                List<string> lstUser = user.Split(',').ToList();
                model.LstUser = allUser.Where(c => lstUser.Contains(c.UserId.ToString())).Select(c => new Tuple<string, string>(c.UserName, c.ImageName)).ToList();
            }
            BuyerListManager objbuyerNameManager = new BuyerListManager(new DataContext());
            BuyerOrderNumberListManager objBuyerOrderNumberNameManager = new BuyerOrderNumberListManager(new DataContext());
            ProcessListManager objProcessListManager = new ProcessListManager(new DataContext());
            model.BuyerList = Extens.ToSelectList(objbuyerNameManager.GetDtBuyer(), "Buyername", "Buyername");
            model.BuyerOrderNumberList = Extens.ToSelectList(objBuyerOrderNumberNameManager.GetDtBuyerOrderNumber(), "BuyerOrderNumberName", "BuyerOrderNumberName");
            model.ProcessList = Extens.ToSelectList(objProcessListManager.GetDtProcess(), "Processname", "Processname");
            int offlineUsers = totUsers - onlineUsers;
            series.Add(onlineUsers);
            series.Add(offlineUsers);
            model.labels = JsonConvert.SerializeObject(labels);
            model.series = JsonConvert.SerializeObject(series);
            ViewBag.OnlineUser = model.LstUser;
            return View(model);
        }
        public ActionResult DSProductFilterSearch(string buyername = null, string orderNo = null, string processName = null)
        {
            DSProductUpdateGridManager context7 = new DSProductUpdateGridManager(new DataContext());
            List<VW_DSProductUpdateGrid> lst = context7.GetList(buyername, orderNo, processName);
            return PartialView("DSProductList", lst);
        }

        //public ActionResult DSProductFilterSearch(DSProductUpdateGrid_Model objModel, int page = 1, int pageSize = 5)
        //{
        //    objModel.StaticPageSize = pageSize;
        //    BindDSProductGrid(objModel, page, pageSize);
        //    return PartialView("DSProductList", objModel);
        //}
        //CommonFunction common = new CommonFunction();
        //public void BindDSProductGrid(DSProductUpdateGrid_Model objModel, int page, int pageSize)
        //{
        //    BuyerReferenceManager objBuyerReferenceManager = new BuyerReferenceManager(new DataContext());
        //    ProcessListManager objProcessListManager = new ProcessListManager(new DataContext());
        //    DSProductUpdateGridManager context = new DSProductUpdateGridManager(new DataContext());
        //    StringBuilder query = new StringBuilder();
        //    var colName = common.GetColumns(CommonFunction.module.DSProduct.ToString());
        //    query = common.GetSqlTableQuery(CommonFunction.module.DSProduct.ToString());
        //    if (objModel != null)
        //        objModel.StaticPageSize = pageSize;

        //    objModel.BuyerOrderNumberList = Extens.ToSelectList(objBuyerReferenceManager.GetDtBuyerReferenceName(), "BuyerReferenceName", "BuyerReferenceName");
        //    objModel.ProcessList = Extens.ToSelectList(objProcessListManager.GetDtProcess(), "Processname", "Processname");
        //    context.setModel(query, objModel, colName, "ProductID", page, pageSize);
        //}

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}