using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using ManageRoles.Models;
using System.Web;
using System.Web.Mvc;
using ManageRoles.Filters;
using ManageRoles.Repository;

namespace ManageRoles.Controllers
{
    [AuthorizeSuperAdminandAdmin]
    [SessionExpireFilter]
    public class SuperDashboardController : Controller
    {
        private readonly IMenu _iMenu;
        private readonly ISubMenu _ISubMenu;

        public SuperDashboardController(IMenu menu, ISubMenu subMenu)
        {
            _iMenu = menu;
            _ISubMenu = subMenu;
        }

        // GET: SuperDashboard
        public ActionResult Dashboard()
        {
            FinalLineInspectionManager context1 = new FinalLineInspectionManager(new DataContext());
            InLineInspectionManager context2 = new InLineInspectionManager(new DataContext());
            MidLineInspectionManager context3 = new MidLineInspectionManager(new DataContext());
            OrderPackingManager context4 = new OrderPackingManager(new DataContext());
            OrdersDespatcheManager context5 = new OrdersDespatcheManager(new DataContext());
            UserManager context6 = new UserManager(new DataContext());
            DSProductUpdateGridManager context7 = new DSProductUpdateGridManager(new DataContext());
            PKGKDManager context8 = new PKGKDManager(new DataContext());
            PKGKDDespatchManager context9 = new PKGKDDespatchManager(new DataContext());
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


        public ActionResult ShowMenus()
        {
            try
            {
                var menuList = _iMenu.GetAllActiveMenuSuperAdmin();
                return PartialView("ShowMenu", menuList);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}