using ManageRoles.Filters;
using ManageRoles.Models;
using ManageRoles.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageRoles.Controllers
{
    [AuthorizeSuperAdminandAdmin]
    [SessionExpireFilter]
    public class ChartController : Controller
    {
        // GET: Chart
        public ActionResult Index()
        {
            ProcessListManager objProcessListManager = new ProcessListManager(new DataContext());
            BuyerListManager objBuyerListManager = new BuyerListManager(new DataContext());
            BuyerOrderNumberListManager objBuyerOrderNumberListManager = new BuyerOrderNumberListManager(new DataContext());
            VW_ProductSummaryManager objVW_ProductSummaryManager = new VW_ProductSummaryManager(new DataContext());

            List<VW_ProductSummary> lst = objVW_ProductSummaryManager.GetProductSummaryList();

            SeriesData obj1 = new SeriesData();
            obj1.name = "ProductQty";
            obj1.data = lst.Select(c => c.ProductQty).ToList();

            SeriesData obj2 = new SeriesData();
            obj2.name = "FinishedQty";
            obj2.data = lst.Select(c => c.FinishedQty).ToList();

            SeriesData obj3 = new SeriesData();
            obj3.name = "BalanceQty";
            obj3.data = lst.Select(c => c.BalanceQty).ToList();

            List<SeriesData> SeriesData = new List<SeriesData>();
            SeriesData.Add(obj1);
            SeriesData.Add(obj2);
            SeriesData.Add(obj3);

            VW_ProductSummary_Model objModel = new VW_ProductSummary_Model();
            objModel.BuyerList = Extens.ToSelectList(objBuyerListManager.GetDtBuyer(), "Buyername", "Buyername");
            objModel.BuyerOrderNumberList = Extens.ToSelectList(objBuyerOrderNumberListManager.GetDtBuyerOrderNumber(), "BuyerOrderNumberName", "BuyerOrderNumberName");
            objModel.ProcessList = Extens.ToSelectList(objProcessListManager.GetDtProcess(), "Processname", "Processname");         
            objModel.SeriesList = SeriesData;
            objModel.Json = JsonConvert.SerializeObject(SeriesData);
            objModel.categories = JsonConvert.SerializeObject(lst.Select(c => c.Buyername + " / " + c.BuyerOrderNumberName + " / " + c.Processname).ToList());
            return View(objModel);
        }

        public ActionResult ResetChart(string pName, string oNumber)
        {
            VW_ProductSummaryManager objVW_ProductSummaryManager = new VW_ProductSummaryManager(new DataContext());
            List<VW_ProductSummary> lst = objVW_ProductSummaryManager.GetProductSummaryList(oNumber, pName);
            if (lst.Count() > 0)
            {
                SeriesData obj1 = new SeriesData();
                obj1.name = "ProductQty";
                obj1.data = lst.Select(c => c.ProductQty).ToList();

                SeriesData obj2 = new SeriesData();
                obj2.name = "FinishedQty";
                obj2.data = lst.Select(c => c.FinishedQty).ToList();

                SeriesData obj3 = new SeriesData();
                obj3.name = "BalanceQty";
                obj3.data = lst.Select(c => c.BalanceQty).ToList();

                List<SeriesData> SeriesData = new List<SeriesData>();
                SeriesData.Add(obj1);
                SeriesData.Add(obj2);
                SeriesData.Add(obj3);
                var lstCat = lst.Select(c => c.Buyername + " / " + c.BuyerOrderNumberName + " / " + c.Processname).ToList();
                return Json(new { Error = false, Series = SeriesData, Categories = lstCat });
            }
            else
            {
                return Json(new { Error = true });
            }
        }

        public ActionResult Gantt()
        {
            ProductManager objProductManager = new ProductManager(new DataContext());
            BuyerListManager objBuyername = new BuyerListManager(new DataContext());
            BuyerOrderNumberListManager objBuyerOrderNumber = new BuyerOrderNumberListManager(new DataContext());
            ProcessListManager objProcessListManager = new ProcessListManager(new DataContext());

            List<BuyerOrderNumber> lstBuyerOrderNumber = objBuyerOrderNumber.GetAll();
            List<ProductUpdateGrid> lstProductUpdateGrid = objProductManager.GetProductMasterList();
            int i = 1;
            List<ProductGantt> lst = new List<ProductGantt>();
            foreach (BuyerOrderNumber obj in lstBuyerOrderNumber)
            {
                ProductGantt obj1 = new ProductGantt();
                obj1.id = i;
                obj1.name = obj.BuyerOrderNumberName;
                List<Series> s1 = lstProductUpdateGrid.Where(c => c.BuyerOrderNumberName == obj.BuyerOrderNumberName).Select(c => new Series { start = c.StartDate.Value, end = c.SPCDate.Value, name = c.Processname, color = null }).ToList();
                obj1.series = s1;
                lst.Add(obj1);
                i++;
            }
            ProductGantt_Model model = new ProductGantt_Model();
            model.ProcessList = Extens.ToSelectList(objProcessListManager.GetDtProcess(), "Processname", "Processname");
            model.BuyerOrderNumberList = Extens.ToSelectList(objBuyerOrderNumber.GetDtBuyerOrderNumber(), "BuyerOrderNumberName", "BuyerOrderNumberName");
            model.SeriesList = lst;
            model.Json = JsonConvert.SerializeObject(lst);
            return View(model);
        }

        [HttpPost]
        public ActionResult ResetGantt(string ProcessName, string BuyerOrderNumberName)
        {
            ProductManager objProductManager = new ProductManager(new DataContext());
            BuyerOrderNumberListManager objBuyerOrderNumber = new BuyerOrderNumberListManager(new DataContext());
            ProcessListManager objProcessListManager = new ProcessListManager(new DataContext());
            List<BuyerOrderNumber> lstBuyerOrderNumber = new List<BuyerOrderNumber>();
            List<ProductUpdateGrid> lstProductUpdateGrid = new List<ProductUpdateGrid>();

            if (!string.IsNullOrEmpty(BuyerOrderNumberName) && !string.IsNullOrEmpty(ProcessName))
            {
                lstBuyerOrderNumber = objBuyerOrderNumber.GetAll(c => c.BuyerOrderNumberName == BuyerOrderNumberName).ToList();
                lstProductUpdateGrid = objProductManager.GetAll(c => c.BuyerOrderNumberName == BuyerOrderNumberName && c.Processname == ProcessName).ToList();
            }
            else if (string.IsNullOrEmpty(BuyerOrderNumberName) && !string.IsNullOrEmpty(ProcessName))
            {
                lstBuyerOrderNumber = objBuyerOrderNumber.GetAll().ToList();
                lstProductUpdateGrid = objProductManager.GetAll(c => c.Processname == ProcessName).ToList();
            }
            else if (!string.IsNullOrEmpty(BuyerOrderNumberName) && string.IsNullOrEmpty(ProcessName))
            {
                lstBuyerOrderNumber = objBuyerOrderNumber.GetAll(c => c.BuyerOrderNumberName == BuyerOrderNumberName).ToList();
                lstProductUpdateGrid = objProductManager.GetAll(c => c.BuyerOrderNumberName == BuyerOrderNumberName).ToList();
            }
            else
            {
                lstBuyerOrderNumber = objBuyerOrderNumber.GetAll().ToList();
                lstProductUpdateGrid = objProductManager.GetAll().ToList();
            }

            ProductGantt_Model model = new ProductGantt_Model();
            model.ProcessList = Extens.ToSelectList(objProcessListManager.GetDtProcess(), "Processname", "Processname");
            model.BuyerOrderNumberList = Extens.ToSelectList(objBuyerOrderNumber.GetDtBuyerOrderNumber(), "BuyerOrderNumberName", "BuyerOrderNumberName");
            if (lstProductUpdateGrid.Count() > 0)
            {
                int i = 1;
                List<ProductGantt> lst = new List<ProductGantt>();
                foreach (BuyerOrderNumber obj in lstBuyerOrderNumber)
                {
                    ProductGantt obj1 = new ProductGantt();
                    obj1.id = i;
                    obj1.name = obj.BuyerOrderNumberName;
                    List<Series> s1 = lstProductUpdateGrid.Where(c => c.BuyerOrderNumberName == obj.BuyerOrderNumberName).Select(c => new Series { start = c.StartDate.Value, end = c.SPCDate.Value, name = c.Processname, color = null }).ToList();
                    obj1.series = s1;
                    lst.Add(obj1);
                    i++;
                }
                model.SeriesList = lst;
                model.Json = JsonConvert.SerializeObject(lst);
            }
            else
            {
                model.SeriesList = new List<ProductGantt>();
                model.Json = "";
            }
            return View("Gantt", model);
        }

        public ActionResult Bar()
        {
            ProductGroupChartManager objPGChartManager = new ProductGroupChartManager(new DataContext());
            BuyerOrderNumberListManager objBuyerOrderNumberListManager = new BuyerOrderNumberListManager(new DataContext());
            var lstData = objPGChartManager.GetAll().ToList();
            List<string> categories = new List<string>();
            List<decimal> lstProductQty = new List<decimal>();
            List<decimal> lstFinishedQty = new List<decimal>();
            List<decimal> lstBalanceQty = new List<decimal>();
            GroupBarModel model = new GroupBarModel();
            model.BuyerOrderNumberList = Extens.ToSelectList(objBuyerOrderNumberListManager.GetDtBuyerOrderNumber(), "BuyerOrderNumberName", "BuyerOrderNumberName");

            foreach (VW_ProductGroupChart obj in lstData)
            {
                categories.Add(obj.Productname + "-" + obj.Processname);
                lstProductQty.Add(obj.ProductQtyfrom);
                lstFinishedQty.Add(obj.FinishedQty);
                lstBalanceQty.Add(obj.BalanceQty);
            }
            model.categories = JsonConvert.SerializeObject(categories);
            model.ProductQty = JsonConvert.SerializeObject(lstProductQty);
            model.FinishedQty = JsonConvert.SerializeObject(lstFinishedQty);
            model.BalanceQty = JsonConvert.SerializeObject(lstBalanceQty);
            return View(model);
        }

        [HttpPost]
        public ActionResult ResetBar(string BuyerOrderNumberName)
        {
            ProductGroupChartManager objPGChartManager = new ProductGroupChartManager(new DataContext());
            BuyerOrderNumberListManager objBuyerOrderNumberListManager = new BuyerOrderNumberListManager(new DataContext());
            var lstData = new List<VW_ProductGroupChart>();
            if (!string.IsNullOrEmpty(BuyerOrderNumberName))
            {
                lstData = objPGChartManager.GetAll(c => c.BuyerOrderNumberName == BuyerOrderNumberName).ToList();
            }
            else
            {
                lstData = objPGChartManager.GetAll().ToList();
            }
            List<string> categories = new List<string>();
            List<decimal> lstProductQty = new List<decimal>();
            List<decimal> lstFinishedQty = new List<decimal>();
            List<decimal> lstBalanceQty = new List<decimal>();
            GroupBarModel model = new GroupBarModel();
            model.BuyerOrderNumberList = Extens.ToSelectList(objBuyerOrderNumberListManager.GetDtBuyerOrderNumber(), "BuyerOrderNumberName", "BuyerOrderNumberName");
            if (lstData != null && lstData.Count() > 0)
            {
                foreach (VW_ProductGroupChart obj in lstData)
                {
                    categories.Add(obj.Productname + "-" + obj.Processname);
                    lstProductQty.Add(obj.ProductQtyfrom);
                    lstFinishedQty.Add(obj.FinishedQty);
                    lstBalanceQty.Add(obj.BalanceQty);
                }
                return Json(new { Error = false, Categories = categories, ProductQty = lstProductQty, FinishedQty = lstFinishedQty, BalanceQty = lstBalanceQty });
            }
            else
            {
                return Json(new { Error = true });
            }
        }

        public ActionResult GanttChart()
        {
            BuyerListManager objBuyername = new BuyerListManager(new DataContext());
            var lstBuyername = Extens.ToSelectList(objBuyername.GetDtBuyer(), "Buyername", "Buyername");
            ViewBag.LstBuyername = lstBuyername;
            return View();
        }

        [HttpPost]
        public ActionResult SetGanttChart(string BON = "")
        {
            BuyerListManager objBuyername = new BuyerListManager(new DataContext());
            ProdcutGroupGanttManager objPGGantttManager = new ProdcutGroupGanttManager(new DataContext());
            BuyerOrderNumberListManager objBuyerOrderNumberListManager = new BuyerOrderNumberListManager(new DataContext());
            List<VW_ProdcutGroupGantt> lstData = new List<VW_ProdcutGroupGantt>();
            if (string.IsNullOrEmpty(BON))
            {
                lstData = objPGGantttManager.GetAll().ToList();
            }
            else
            {
                lstData = objPGGantttManager.GetAll(c => c.Buyername == BON).ToList();
            }
            List<TaskViewModel> lstTData = new List<TaskViewModel>();
            List<string> lstBDN = new List<string>();
            int ord = 1;           
            foreach (var objData in lstData)
            {
                TaskViewModel obj = new TaskViewModel();
                obj.ID = Convert.ToInt32(objData.ID);
                obj.Title = objData.Productname;
                obj.Start = DateTime.SpecifyKind(objData.StartDate.Value, DateTimeKind.Utc);
                obj.End = DateTime.SpecifyKind(objData.SPCDate.Value, DateTimeKind.Utc);
                obj.ParentID = null;
                obj.PercentComplete = Convert.ToDouble(objData.Done) / 100.00;
                obj.OrderID = ord;
                obj.ChildId = ord;
                obj.Expanded = true;
                obj.Summary = false;
                obj.NoDays = objData.NoDays;
                obj.Buyername = objData.Buyername;
                obj.BuyerOrderNumberName = objData.BuyerOrderNumberName;
                obj.Processname = objData.Processname;
                obj.Done = objData.Done;
                obj.DelayedDays = objData.DelayedDays;
                obj.SKUStart = objData.SKUStartDate;
                obj.SKUEnd = objData.SKUEndDate;
                lstTData.Add(obj);
                ord++;
                            }
            return Json(new { data = lstTData });
        }

        [HttpPost]
        public ActionResult SetGanttChart1(string BON = "")
        {
            ProdcutGroupGanttManager objPGGantttManager = new ProdcutGroupGanttManager(new DataContext());
            BuyerOrderNumberListManager objBuyerOrderNumberListManager = new BuyerOrderNumberListManager(new DataContext());
            BuyerListManager objBuyerListManager = new BuyerListManager(new DataContext());
            ProcessListManager objProcessListManager = new ProcessListManager(new DataContext());
            ProductNameManager objProductNameManager = new ProductNameManager(new DataContext());
            List<VW_ProdcutGroupGantt> lstData = new List<VW_ProdcutGroupGantt>();
            if (string.IsNullOrEmpty(BON))
            {
                lstData = objPGGantttManager.GetAll().ToList();
            }
            else
            {
                lstData = objPGGantttManager.GetAll(c => c.Buyername == BON).ToList();
            }
            List<TaskViewModel> lstTData = new List<TaskViewModel>();
            List<string> lstBDN = objBuyerListManager.GetAll().ToList().Select(c => c.BuyerName).ToList();
            List<string> lstODN = objBuyerOrderNumberListManager.GetAll().ToList().Select(c => c.BuyerOrderNumberName).ToList();
            List<string> lstPSN = objProcessListManager.GetAll().ToList().Select(c => c.ProcessName).ToList();
            List<string> lstPDN = objProductNameManager.GetAll().ToList().Select(c => c.ProductName).ToList();
            int ord = 1;
            foreach (string bo in lstBDN)
            {
                var lstByBON = lstData.Where(c => c.Buyername == bo);
                if (lstByBON.Count() > 0)
                {
                    DateTime sDateBON = lstByBON.Min(c => c.StartDate).Value;
                    DateTime eDateBON = lstByBON.Max(c => c.SPCDate).Value;

                    DateTime? skusDate = lstByBON.Min(c => c.SKUStartDate);
                    DateTime? skueeDate = lstByBON.Max(c => c.SKUEndDate);
                    if (skusDate == null)
                    {

                    }
                    TaskViewModel obj = new TaskViewModel();
                    obj.ID = ord;
                    obj.Title = bo;
                    obj.ParentID = null;
                    obj.PercentComplete = Convert.ToDouble(lstByBON.FirstOrDefault().Done) / 100.00;
                    obj.OrderID = ord;
                    obj.ChildId = ord;
                    obj.Start = DateTime.SpecifyKind(sDateBON, DateTimeKind.Utc);
                    obj.End = DateTime.SpecifyKind(eDateBON, DateTimeKind.Utc);
                    obj.Expanded = true;
                    obj.Summary = true;
                    obj.Done = lstByBON.Max(c => c.Done);
                    obj.DelayedDays = lstByBON.Max(c => c.DelayedDays);
                    obj.SKUStart = skusDate;
                    obj.SKUEnd = skueeDate;
                    lstTData.Add(obj);
                    ord++;
                    foreach (string ps in lstODN)
                    {
                        var lstByPS = lstData.Where(c => c.Buyername == bo && c.BuyerOrderNumberName == ps);
                        if (lstByPS.Count() > 0)
                        {
                            DateTime sDatePS = lstByPS.Min(c => c.StartDate).Value;
                            DateTime eDatePS = lstByPS.Max(c => c.SPCDate).Value;
                            DateTime? skusDate1 = lstByBON.Min(c => c.SKUStartDate);
                            DateTime? skueeDate1 = lstByBON.Max(c => c.SKUEndDate);
                            if (skusDate1 == null)
                            {

                            }
                            TaskViewModel obj1 = new TaskViewModel();
                            obj1.ID = ord;
                            obj1.Title = ps;
                            obj1.ParentID = obj.ID;
                            obj1.Start = DateTime.SpecifyKind(sDatePS, DateTimeKind.Utc);
                            obj1.End = DateTime.SpecifyKind(eDatePS, DateTimeKind.Utc);
                            obj1.PercentComplete = Convert.ToDouble(lstByPS.FirstOrDefault().Done) / 100.00;
                            obj1.OrderID = ord;
                            obj1.ChildId = ord;
                            obj1.Expanded = true;
                            obj1.Summary = true;
                            obj1.Done = lstByPS.Max(c => c.Done);
                            obj1.DelayedDays = lstByPS.Max(c => c.DelayedDays);
                            obj1.SKUStart = skusDate1;
                            obj1.SKUEnd = skueeDate1;
                            lstTData.Add(obj1);
                            ord++;
                            foreach (string vs in lstPSN)
                            {
                                var lstByVS = lstData.Where(c => c.Buyername == bo && c.BuyerOrderNumberName == ps && c.Processname == vs);
                                if (lstByVS.Count() > 0)
                                {
                                    DateTime sDateVS = lstByVS.Min(c => c.StartDate).Value;
                                    DateTime eDateVS = lstByVS.Max(c => c.SPCDate).Value;
                                    DateTime? skusDate2 = lstByBON.Min(c => c.SKUStartDate);
                                    DateTime? skueeDate2 = lstByBON.Max(c => c.SKUEndDate);
                                    if (skusDate1 == null)
                                    {

                                    }
                                    TaskViewModel obj2 = new TaskViewModel();
                                    obj2.ID = ord;
                                    obj2.Title = vs;
                                    obj2.ParentID = obj1.ID;
                                    obj2.Start = DateTime.SpecifyKind(sDatePS, DateTimeKind.Utc);
                                    obj2.End = DateTime.SpecifyKind(eDatePS, DateTimeKind.Utc);
                                    obj2.PercentComplete = Convert.ToDouble(lstByVS.FirstOrDefault().Done) / 100.00;
                                    obj2.OrderID = ord;
                                    obj2.ChildId = ord;
                                    obj2.Expanded = true;
                                    obj2.Summary = true;
                                    obj2.Done = lstByVS.Max(c => c.Done);
                                    obj2.DelayedDays = lstByVS.Max(c => c.DelayedDays);
                                    obj2.SKUStart = skusDate2;
                                    obj2.SKUEnd = skueeDate2;
                                    lstTData.Add(obj2);
                                    ord++;
                                    foreach (string pd in lstPDN)
                                    {
                                        var lstByPD = lstData.Where(c => c.Buyername == bo && c.BuyerOrderNumberName == ps && c.Processname == vs && c.Productname == pd);
                                        if (lstByPD.Count() > 0)
                                        {
                                            foreach (var objData in lstByPD)
                                            {
                                                DateTime? skusDate3 = objData.SKUStartDate;
                                                DateTime? skueeDate3 = objData.SKUEndDate;
                                                if (skusDate3 == null)
                                                {

                                                }
                                                TaskViewModel obj3 = new TaskViewModel();
                                                obj3.Title = objData.Productname;
                                                obj3.ID = ord;
                                                obj3.Start = DateTime.SpecifyKind(objData.StartDate.Value, DateTimeKind.Utc);
                                                obj3.End = DateTime.SpecifyKind(objData.SPCDate.Value, DateTimeKind.Utc);
                                                obj3.ParentID = obj2.ID;
                                                obj3.PercentComplete = Convert.ToDouble(objData.Done) / 100.00;
                                                obj3.OrderID = ord;
                                                obj3.ChildId = ord;
                                                obj3.Expanded = true;
                                                obj3.Summary = false;
                                                obj3.NoDays = objData.NoDays;
                                                obj3.Done = objData.Done;
                                                obj3.DelayedDays = objData.DelayedDays;
                                                obj3.SKUStart = skusDate3;
                                                obj3.SKUEnd = skueeDate3;
                                                lstTData.Add(obj3);
                                                ord++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
                    return Json(new { data = lstTData });
         }
     }
 }
    
