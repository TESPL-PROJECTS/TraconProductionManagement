using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageRoles.Repository;
using System.Text;
using System.Net;
using ManageRoles.Filters;

namespace ManageRoles.Controllers
{
    [AuthorizeSuperAdminandAdmin]
    [SessionExpireFilter]
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            ProductUpdateGrid_Model objModel = new ProductUpdateGrid_Model();
            objModel.page = 1;
            objModel.StaticPageSize = 5;
            BindProductGrid(objModel, Convert.ToInt32(objModel.page), objModel.StaticPageSize);
            return View(objModel);
        }

        #region Start ProductGrid   
        CommonFunction common = new CommonFunction();
        public ActionResult ProductGridFilterSearch(ProductUpdateGrid_Model objModel, int page = 1, int pageSize = 5)
        {
            BindProductGrid(objModel, page, pageSize);
            return PartialView("ProductGridList", objModel);
        }
        public void BindProductGrid(ProductUpdateGrid_Model objModel, int page, int pageSize)
        {
            BuyerListManager objBuyerListManager = new BuyerListManager(new DataContext());
            BuyerOrderNumberListManager objBuyerOrderNumberListManager = new BuyerOrderNumberListManager(new DataContext());
            ProcessListManager objProcessListManager = new ProcessListManager(new DataContext());
            SupplierListManager objSupplierListManager = new SupplierListManager(new DataContext());
            ProductListManager objProductListManager = new ProductListManager(new DataContext());           
            
            UnitListManager objUnitListManager = new UnitListManager(new DataContext());
            SetNoteListManager objSetNoteListManager = new SetNoteListManager(new DataContext());

            ProductManager context = new ProductManager(new DataContext());

            PlannerManager pln_context = new PlannerManager(new DataContext());
            FinishingManager fis_context = new FinishingManager(new DataContext());
            objModel.PlannerUpdateGridList = pln_context.GetPlannerUpdateGridList();
            objModel.FinishingUpdateGridList = fis_context.GetFinishingUpdateGridList();


            StringBuilder query = new StringBuilder();
            var colName = common.GetColumns(CommonFunction.module.ProductMaster.ToString());
            query = common.GetSqlTableQuery(CommonFunction.module.ProductMaster.ToString());
            string uProcess = "";
            if (Session["UserProcess"] != null)
            {
                uProcess = Session["UserProcess"].ToString();
            }
            uProcess = "'" + uProcess.Replace(",", "','") + "'";
            query.Append(" AND Processname IN (" + uProcess + ") AND ");
            if (objModel != null)
                objModel.StaticPageSize = pageSize;

            objModel.BuyerList = Extens.ToSelectList(objBuyerListManager.GetDtBuyer(), "ByrID", "Buyername");
            objModel.BuyerOrderNumberList = Extens.ToSelectList(objBuyerOrderNumberListManager.GetDtBuyerOrderNumber(), "BuyerOrderNumberID", "BuyerOrderNumberName");
            objModel.ProcessList = Extens.ToSelectList(objProcessListManager.GetDtProcess(), "ProcID", "Processname");
            objModel.SupplierList = Extens.ToSelectList(objSupplierListManager.GetDtSupplier(), "SupID", "Suppliername");
            objModel.ProductList = Extens.ToSelectList(objProductListManager.GetDtProduct(), "PrdID", "Productname");        
           
            objModel.UnitList = Extens.ToSelectList(objUnitListManager.GetDtUnit(), "UnitID", "Unitname");
            objModel.SetNoteList = Extens.ToSelectList(objSetNoteListManager.GetDtSetNote(), "SetID", "Setnotename");
            objModel.sortOrder = "desc";
            objModel.fieldName = "ProductID";
            context.setModel(query, objModel, colName, "ProductID", page, pageSize);
        }


        //Start Product Master
        [HttpPost]
        public ActionResult AddEditProductMaster(int ProductID = 0)
        {
            BuyerListManager objBuyerListManager = new BuyerListManager(new DataContext());
            BuyerOrderNumberListManager objBuyerOrderNumberListManager = new BuyerOrderNumberListManager(new DataContext());
            ProcessListManager objProcessListManager = new ProcessListManager(new DataContext());
            SupplierListManager objSupplierListManager = new SupplierListManager(new DataContext());
            ProductListManager objProductListManager = new ProductListManager(new DataContext());          
           
            UnitListManager objUnitListManager = new UnitListManager(new DataContext());
            SetNoteListManager objSetNoteListManager = new SetNoteListManager(new DataContext());

            ProductManager context = new ProductManager(new DataContext());
            ProductUpdateGrid_Model objModel = new ProductUpdateGrid_Model();

            if (ProductID != 0)
            {
                objModel.Table = context.GetProductMasterById(ProductID);
            }
            else { objModel.Table = new ProductUpdateGrid(); }
            objModel.BuyerList = Extens.ToSelectList(objBuyerListManager.GetDtBuyer(), "Buyername", "Buyername");
            objModel.BuyerOrderNumberList = Extens.ToSelectList(objBuyerOrderNumberListManager.GetDtBuyerOrderNumber(), "BuyerOrderNumberName", "BuyerOrderNumberName");
            objModel.ProcessList = Extens.ToSelectList(objProcessListManager.GetDtProcess(), "Processname", "Processname");
            objModel.SupplierList = Extens.ToSelectList(objSupplierListManager.GetDtSupplier(), "Suppliername", "Suppliername");
            objModel.ProductList = Extens.ToSelectList(objProductListManager.GetDtProduct(), "Productname", "Productname");            
            
            objModel.UnitList = Extens.ToSelectList(objUnitListManager.GetDtUnit(), "Unitname", "Unitname");
            objModel.SetNoteList = Extens.ToSelectList(objSetNoteListManager.GetDtSetNote(), "Setnotename", "Setnotename");

            return PartialView("ProductMasterCRUD", objModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveProductMaster(ProductUpdateGrid_Model objModel, int page = 1, int pageSize = 5)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join("|", ModelState.Values.SelectMany(e => e.Errors).Select(em => em.ErrorMessage));
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, message);
            }

            //Save     
            ProductManager context = new ProductManager(new DataContext());
            objModel.Table.BalanceQty = objModel.Table.ProductQty - objModel.Table.FinishedQty;
            var msg = context.SaveProductMaster(objModel.Table);
            if (msg.Contains("exists"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "exists");
            }
            else
            {
                if (objModel.StaticPageSize != 0) { pageSize = objModel.StaticPageSize; }
                objModel.StaticPageSize = pageSize;//objModel.StaticPageSize = 10;

                BindProductGrid(objModel, page, pageSize);
                return PartialView("ProductGridList", objModel);
            }
        }

        [HttpPost]
        public ActionResult DeleteProductMaster(string ProductID, ProductUpdateGrid_Model objModel, int page = 1, int pageSize = 5)
        {
            ProductManager context = new ProductManager(new DataContext());

            if (!string.IsNullOrEmpty(ProductID))
            {
                context.DeleteProductMaster(Convert.ToInt32(ProductID));
            }

            if (objModel.StaticPageSize != 0) { pageSize = objModel.StaticPageSize; }
            objModel.StaticPageSize = pageSize;//objModel.StaticPageSize = 10;

            BindProductGrid(objModel, page, pageSize);
            return PartialView("ProductGridList", objModel);
        }
        [HttpPost]
        public string GetPINumberNames(string BuyerOrderNumber)
        {
            BuyerOrderNumberListManager objM = new BuyerOrderNumberListManager(new DataContext());
            return objM.GetPINumberByBuyerOrderNumber(BuyerOrderNumber);
        }


        //Start Planner Master
        [HttpPost]
        public ActionResult AddEditPlannedQty(int PlannerID = 0)
        {
            PlannerManager context = new PlannerManager(new DataContext());
            PlannerUpdateGrid objModel = new PlannerUpdateGrid();

            if (PlannerID != 0)
            {
                objModel = context.GetPlannerMasterById(PlannerID);
            }
            else { objModel = new PlannerUpdateGrid(); }

            return PartialView("PlannerMasterCRUD", objModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SavePlannedMaster(PlannerUpdateGrid objModel)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join("|", ModelState.Values.SelectMany(e => e.Errors).Select(em => em.ErrorMessage));
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, message);
            }
            var PlannerID = objModel.PlannerID;
            //Save    
            var msg = "";
            PlannerManager context = new PlannerManager(new DataContext());
            msg = context.SavePlannerMaster(objModel);

            //var SubTotal_Planned = new Tuple<int, int>(0, 0);
            //var Total_Product = new Tuple<int, int>(0, 0);
            //var Row_Product = new Tuple<int, int>(0, 0);
            //if (msg == "success")
            //{
            //    //Get Total PlannedQty & FinishedQty
            //    SubTotal_Planned = context.GetTotalPlannerGrid(objModel.ProductID);//PlannedQty,FinishedQty       

            //    //Update Product Grid PlannedQty & FinishedQty 
            //    ProductManager cont = new ProductManager(new DataContext());
            //    cont.UpdateQty(objModel.ProductID, SubTotal_Planned.Item1, SubTotal_Planned.Item2);
            //    Total_Product = cont.GetTotalProductGrid(objModel.ProductID);//PlannedQty,FinishedQty       
            //    Row_Product = cont.GetRowProductGrid(objModel.ProductID);//PlannedQty,FinishedQty       
            //}

            //objModel.PlanningDate_Display = objModel.PlanningDate == null ? "" : objModel.PlanningDate.Value.ToString("dd-MMM-yy");
            //var obj = new
            //{
            //    obj = objModel,
            //    sub_tot_plannedqty = SubTotal_Planned.Item1,//PlannedQty of PlannedQty Grid
            //    sub_tot_finishedqty = SubTotal_Planned.Item2,//FinishedQty of PlannedQty Grid
            //    tot_plannedqty = Total_Product.Item1,//PlannedQty of Product Grid
            //    tot_finishedqty = Total_Product.Item2,//FinishedQty of Product Grid
            //    row_plannedqty = Row_Product.Item1,//PlannedQty of selected Product row Grid
            //    row_finishedqty = Row_Product.Item2,//FinishedQty of selected Product row Product Grid
            //    is_Edit = PlannerID > 0 ? true : false,
            //};

            objModel.PlanningDate_Display = objModel.PlanningDate == null ? "" : objModel.PlanningDate.Value.ToString("dd-MMM-yy");
            var model = GetUpdated_PlannedMaster(objModel.PlannerID, objModel.ProductID, PlannerID > 0 ? true : false, objModel);

            return Json(new { msg = msg, model = model }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DeletePlannerMaster(int PlannerID, int ProductID)
        {
            PlannerManager context = new PlannerManager(new DataContext());

            if (PlannerID > 0)
            {
                context.DeletePlannerMaster(PlannerID);

            }
            var model = GetUpdated_PlannedMaster(PlannerID, ProductID, PlannerID > 0 ? true : false);
            return Json(new { msg = "success", model = model }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetUpdated_PlannedMaster(int PlannerID, int ProductID, bool is_Edit, PlannerUpdateGrid objModel = null)
        {
            var SubTotal_Planned = new Tuple<decimal, decimal>(0, 0);
            var Total_Product = new Tuple<decimal, decimal, decimal>(0, 0, 0);
            var Row_Product = new Tuple<decimal, decimal, decimal>(0, 0, 0);

            PlannerManager context = new PlannerManager(new DataContext());
            FinishingManager context1 = new FinishingManager(new DataContext());

            //Get Total PlannedQty & FinishedQty
            SubTotal_Planned = context.GetTotalPlannerGrid(ProductID);//PlannedQty,FinishedQty    

            //Get Total Planned days
            var lstPlanner = context.GetAll(c => c.ProductID == ProductID).ToList();
            int plannedDays = lstPlanner.Count() * 7;

            ////Get Total Finished days
            //int finishedDays = 0;
            //var lstPlannerID = lstPlanner.Select(c => c.PlannerID).ToList();
            //var distinctFinishGridCount = context1.GetAll(c => lstPlannerID.Contains(c.PlannerID)).ToList().GroupBy(c=>c.PlannerID).ToList();
            //finishedDays = distinctFinishGridCount.Count() * 7;

            //Update Product Grid PlannedQty & FinishedQty 
            ProductManager cont = new ProductManager(new DataContext());
            cont.UpdateQty(ProductID, SubTotal_Planned.Item1, SubTotal_Planned.Item2);
            Total_Product = cont.GetTotalProductGrid(ProductID);//PlannedQty,FinishedQty       
            Row_Product = cont.GetRowProductGrid(ProductID);//PlannedQty,FinishedQty       

            if (objModel == null)
            {
                objModel = new PlannerUpdateGrid()
                {
                    PlannerID = PlannerID,
                    ProductID = ProductID
                };
            }

            var obj = new
            {
                obj = objModel,
                sub_tot_plannedqty = SubTotal_Planned.Item1,//PlannedQty of PlannedQty Grid
                sub_tot_finishedqty = SubTotal_Planned.Item2,//FinishedQty of PlannedQty Grid
                tot_plannedqty = Total_Product.Item1,//PlannedQty of Product Grid
                tot_finishedqty = Total_Product.Item2,//FinishedQty of Product Grid
                row_plannedqty = Row_Product.Item1,//PlannedQty of selected Product row Grid
                row_finishedqty = Row_Product.Item2,//FinishedQty of selected Product row Product Grid
                is_Edit = is_Edit,
                tot_plannedDays = plannedDays
                //,tot_finishedDays = finishedDays
            };

            return obj;
        }



        //Start Finished Master
        [HttpPost]
        public ActionResult AddEditFinishedQty(int FinishedID = 0)
        {
            FinishingManager context = new FinishingManager(new DataContext());
            FinishingUpdateGrid objModel = new FinishingUpdateGrid();

            if (FinishedID != 0)
            {
                objModel = context.GetFinishingMasterById(FinishedID);
            }
            else { objModel = new FinishingUpdateGrid(); }

            return PartialView("FinishedMasterCRUD", objModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveFinishedMaster(FinishingUpdateGrid objModel)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join("|", ModelState.Values.SelectMany(e => e.Errors).Select(em => em.ErrorMessage));
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, message);
            }
            var FinishedID = objModel.FinishedID;
            //Save    
            var msg = "";
            FinishingManager context = new FinishingManager(new DataContext());
            msg = context.SaveFinishingMaster(objModel);

            //var SubTotal_Finished = 0;
            //var SubTotal_Planned = new Tuple<int, int>(0, 0);
            //var Total_Product = new Tuple<int, int>(0, 0);
            //var Row_Product = new Tuple<int, int>(0, 0);
            //var Row_Planner_FinishedQty = 0;
            //if (msg == "success")
            //{
            //    //Get Total FinishedQty of FinishedGrid
            //    SubTotal_Finished = context.GetTotalFinishingGrid(objModel.PlannerID);//FinishedQty       
            //    //Update Planner Grid FinishedQty 
            //    PlannerManager cont_plan = new PlannerManager(new DataContext());
            //    cont_plan.UpdateQty(objModel.PlannerID, SubTotal_Finished);

            //    //Get Total PlannedQty & FinishedQty
            //    SubTotal_Planned = cont_plan.GetTotalPlannerGrid(objModel.ProductID);//PlannedQty,FinishedQty       
            //    Row_Planner_FinishedQty = cont_plan.GetRowPlannerGrid(objModel.PlannerID);//PlannedQty,FinishedQty       

            //    //Update Product Grid PlannedQty & FinishedQty 
            //    ProductManager cont_prod = new ProductManager(new DataContext());
            //    cont_prod.UpdateQty(objModel.ProductID, SubTotal_Planned.Item1, SubTotal_Planned.Item2);
            //    Total_Product = cont_prod.GetTotalProductGrid(objModel.ProductID);//PlannedQty,FinishedQty       
            //    Row_Product = cont_prod.GetRowProductGrid(objModel.ProductID);//PlannedQty,FinishedQty       
            //}

            //objModel.FinishingDate_Display = objModel.FinishingDate == null ? "" : objModel.FinishingDate.Value.ToString("dd-MMM-yy");
            //var obj = new
            //{
            //    obj = objModel,
            //    sub_tot_finishedqty2 = SubTotal_Finished,//FinishedQty of FinishedQty Grid
            //    sub_tot_plannedqty = SubTotal_Planned.Item1,//PlannedQty of PlannedQty Grid
            //    sub_tot_finishedqty = SubTotal_Planned.Item2,//FinishedQty of PlannedQty Grid
            //    tot_plannedqty = Total_Product.Item1,//PlannedQty of Product Grid
            //    tot_finishedqty = Total_Product.Item2,//FinishedQty of Product Grid
            //    row_product_plannedqty = Row_Product.Item1,//PlannedQty of selected Product row Grid
            //    row_product_finishedqty = Row_Product.Item2,//FinishedQty of selected Product row of Product Grid
            //    row_plan_finishedqty = Row_Planner_FinishedQty,//FinishedQty of selected planner row of planner Grid

            //    is_Edit = FinishedID > 0 ? true : false,
            //};
            objModel.FinishingDate_Display = objModel.FinishingDate == null ? "" : objModel.FinishingDate.Value.ToString("dd-MMM-yy");

            var model = GetUpdated_FinishedMaster(objModel.FinishedID, objModel.PlannerID, objModel.ProductID, FinishedID > 0 ? true : false, objModel);
            return Json(new { msg = msg, model = model }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DeleteFinishedMaster(int FinishedID, int PlannerID, int ProductID)
        {
            FinishingManager context = new FinishingManager(new DataContext());

            if (FinishedID > 0)
            {
                context.DeleteFinishingMaster(Convert.ToInt32(FinishedID));
            }
            var model = GetUpdated_FinishedMaster(FinishedID, PlannerID, ProductID, FinishedID > 0 ? true : false);
            return Json(new { msg = "success", model = model }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetUpdated_FinishedMaster(int FinishedID, int PlannerID, int ProductID, bool is_Edit, FinishingUpdateGrid objModel = null)
        {
            var SubTotal_Finished = Convert.ToDecimal(0);
            var SubTotal_Planned = new Tuple<decimal, decimal>(0, 0);
            var Total_Product = new Tuple<decimal, decimal, decimal>(0, 0, 0);
            var Row_Product = new Tuple<decimal, decimal, decimal>(0, 0, 0);
            var Row_Planner_FinishedQty = Convert.ToDecimal(0);

            FinishingManager context = new FinishingManager(new DataContext());
            //Get Total FinishedQty of FinishedGrid
            SubTotal_Finished = context.GetTotalFinishingGrid(PlannerID);//FinishedQty       
                                                                         //Update Planner Grid FinishedQty 
            PlannerManager cont_plan = new PlannerManager(new DataContext());
            cont_plan.UpdateQty(PlannerID, SubTotal_Finished);

            //Get Total PlannedQty & FinishedQty
            SubTotal_Planned = cont_plan.GetTotalPlannerGrid(ProductID);//PlannedQty,FinishedQty       
            Row_Planner_FinishedQty = cont_plan.GetRowPlannerGrid(PlannerID);//PlannedQty,FinishedQty       

            //Update Product Grid PlannedQty & FinishedQty 
            ProductManager cont_prod = new ProductManager(new DataContext());
            cont_prod.UpdateQty(ProductID, SubTotal_Planned.Item1, SubTotal_Planned.Item2);
            Total_Product = cont_prod.GetTotalProductGrid(ProductID);//PlannedQty,FinishedQty       
            Row_Product = cont_prod.GetRowProductGrid(ProductID);//PlannedQty,FinishedQty

            //Get Total Finished days
            //int finishedDays = 0;
            //cont_plan = new PlannerManager(new DataContext());
            //var lstPlannerID = cont_plan.GetAll(c => c.ProductID == ProductID).Select(c => c.PlannerID).ToList();
            //context = new FinishingManager(new DataContext());
            //var distinctFinishGridCount = context.GetAll(c => lstPlannerID.Contains(c.PlannerID)).ToList().GroupBy(c => c.PlannerID).ToList();
            //finishedDays = distinctFinishGridCount.Count() * 7;

            if (objModel == null)
            {
                objModel = new FinishingUpdateGrid()
                {
                    FinishedID = FinishedID,
                    PlannerID = PlannerID,
                    ProductID = ProductID
                };
            }
            var obj = new
            {
                obj = objModel,
                sub_tot_finishedqty2 = SubTotal_Finished,//FinishedQty of FinishedQty Grid
                sub_tot_plannedqty = SubTotal_Planned.Item1,//PlannedQty of PlannedQty Grid
                sub_tot_finishedqty = SubTotal_Planned.Item2,//FinishedQty of PlannedQty Grid
                tot_plannedqty = Total_Product.Item1,//PlannedQty of Product Grid
                tot_finishedqty = Total_Product.Item2,//FinishedQty of Product Grid
                tot_balanceqty = Total_Product.Item3,//BalanceQty of Product Grid
                row_product_plannedqty = Row_Product.Item1,//PlannedQty of selected Product row Grid
                row_product_finishedqty = Row_Product.Item2,//FinishedQty of selected Product row of Product Grid
                row_product_balanceqty = Row_Product.Item3,//BalanceQty of selected Product row of Product Grid
                row_plan_finishedqty = Row_Planner_FinishedQty,//FinishedQty of selected planner row of planner Grid
                is_Edit = is_Edit
                //,tot_finishedDays = finishedDays
            };

            return obj;
        }


        #endregion

    }
}