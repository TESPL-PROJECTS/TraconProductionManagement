using ManageRoles.Filters;
using ManageRoles.Helpers;
using ManageRoles.Repository;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ManageRoles.Controllers
{
    [AuthorizeSuperAdminandAdmin]
    [SessionExpireFilter]
    public class OrderProcessMeetingController : Controller
    {
        // GET: OrderProcessMeeting
        public ActionResult Index()
        {
            OPM_Model objModel = new OPM_Model();
            objModel.page = 1;
            objModel.StaticPageSize = 5;
            BindOPMGrid(objModel, Convert.ToInt32(objModel.page), objModel.StaticPageSize);
            return View(objModel);
        }
        CommonFunction common = new CommonFunction();
        #region Start OPMGrid   
        public ActionResult OPMFilterSearch(OPM_Model objModel, int page = 1, int pageSize = 5)
        {
            BindOPMGrid(objModel, page, pageSize);
            return PartialView("OPMList", objModel);
        }
        public void BindOPMGrid(OPM_Model objModel, int page, int pageSize)
        {
            BuyerReferenceManager objBuyerReferenceManager = new BuyerReferenceManager(new DataContext());
            StringBuilder query = new StringBuilder();
            var colName = common.GetColumns(CommonFunction.module.OPM.ToString());
            query = common.GetSqlTableQuery(CommonFunction.module.OPM.ToString());
            if (objModel != null)
                objModel.StaticPageSize = pageSize;

            objModel.BuyerReferenceNameList = Extens.ToSelectList(objBuyerReferenceManager.GetDtBuyerReferenceName(), "BuyerReferenceName", "BuyerReferenceName");
            //objModel.BuyerStoryNameList = Extens.ToSelectList(objBuyerReferenceManager.GetDtBuyerReferenceName(), "StoryName", "StoryName");
            OPMManager context = new OPMManager(new DataContext());
            objModel.sortOrder = "desc";
            objModel.fieldName = "OPM_ID";
            context.setModel(query, objModel, colName, "OPM_ID", page, pageSize);
        }


        //Start Product Master
        [HttpPost]
        public ActionResult AddEditOPM(int OPM_ID = 0)
        {
            BuyerReferenceManager objBuyerReferenceManager = new BuyerReferenceManager(new DataContext());
            ParticipiantManager objParticipiantManager = new ParticipiantManager(new DataContext());
            BuyerNameManager objBuyerNameManager = new BuyerNameManager(new DataContext());
            OPMManager context = new OPMManager(new DataContext());
            OPM_Model objModel = new OPM_Model();
             
            if (OPM_ID != 0)
            {
                objModel.Table = context.GetOPMById(OPM_ID);
                if (objModel.Table.Participants != null)
                {
                    objModel.Participant = objModel.Table.Participants.Split(',');
                }
                if (objModel.Table.BuyerReferenceName != null)
                {
                    objModel.BuyerReferenceName = objModel.Table.BuyerReferenceName.Split(',');
                }
            }
            else
            {
                objModel.Table = new OPM();
                objModel.Participant = null;
                objModel.BuyerReferenceName = null;
            }
            objModel.BuyerReferenceNameList = Extens.ToSelectList(objBuyerReferenceManager.GetDtBuyerReferenceName(), "BuyerReferenceName", "BuyerReferenceName");
            objModel.BuyerStoryNameList = Extens.ToSelectList(objBuyerReferenceManager.GetDtBuyerStoryName(), "StoryName", "StoryName");
            objModel.BuyerNameList = Extens.ToSelectList(objBuyerNameManager.GetDtBuyerName(), "Buyername", "Buyername");
            objModel.ParticipantList = Extens.ToSelectList(objParticipiantManager.GetDtParticipiantName(), "Name", "Name");

            return PartialView("OPMCRUD", objModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOPM(OPM_Model objModel, int page = 1, int pageSize = 5)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join("|", ModelState.Values.SelectMany(e => e.Errors).Select(em => em.ErrorMessage));
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, message);
            }

            //Save     
            OPMManager context = new OPMManager(new DataContext());
            if(objModel.Participant != null)
            {
                objModel.Table.Participants = string.Join(",", objModel.Participant);
            }
            if (objModel.BuyerReferenceName != null)
            {
                objModel.Table.BuyerReferenceName = string.Join(",", objModel.BuyerReferenceName);
            }
            var msg = context.SaveOPM(objModel.Table);
            if (msg.Contains("exists"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "exists");
            }
            else
            {
                if (objModel.StaticPageSize != 0) { pageSize = objModel.StaticPageSize; }
                objModel.StaticPageSize = pageSize;

                BindOPMGrid(objModel, page, pageSize);
                return PartialView("OPMList", objModel);
            }
        }

        [HttpPost]
        public ActionResult DeleteOPM(string OPM_ID, OPM_Model objModel, int page = 1, int pageSize = 5)
        {
            OPMManager context = new OPMManager(new DataContext());

            if (!string.IsNullOrEmpty(OPM_ID))
            {
                context.DeleteOPM(Convert.ToInt32(OPM_ID));
            }

            if (objModel.StaticPageSize != 0) { pageSize = objModel.StaticPageSize; }
            objModel.StaticPageSize = pageSize;

            BindOPMGrid(objModel, page, pageSize);
            return PartialView("OPMList", objModel);
        }
       
        public List<SelectListItem> GetParticipantList()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "A", Value = "A" });
            items.Add(new SelectListItem { Text = "B", Value = "B" });
            items.Add(new SelectListItem { Text = "C", Value = "C" });
            items.Add(new SelectListItem { Text = "D", Value = "D" });
            return items;
        }
        #endregion

        #region Start VendorGrid   
        public ActionResult VendorFilterSearch(int OPM_ID, int page = 1, int pageSize = 5)
        {
            Vendor_Model objModel = new Vendor_Model();
            objModel.OPM_ID = OPM_ID;
            objModel.StaticPageSize = pageSize;
            BindVendorGrid(objModel, page, pageSize);
            return PartialView("VendorList", objModel);
        }
        public void BindVendorGrid(Vendor_Model objModel, int page, int PageSize)
        {
            VendorManager context = new VendorManager(new DataContext());
            var VendorList = context.GetVendorList(objModel.OPM_ID);
            objModel.VendorList = VendorList.ToPagedList(page, PageSize).ToList();
            objModel.pageList = new PagedList<Vendor>(VendorList, page, PageSize);
        }


        //Start Product Master
        [HttpPost]
        public ActionResult AddEditVendor(int OPM_ID, int VP_ID = 0)
        {
            BuyerOrderNumberListManager objBuyerOrderNumber = new BuyerOrderNumberListManager(new DataContext());
            ParticipiantManager objParticipiantManager = new ParticipiantManager(new DataContext());
            SupplierListManager objSupplierListManager = new SupplierListManager(new DataContext());
            VendorManager context = new VendorManager(new DataContext());
            Vendor_Model objModel = new Vendor_Model();
            if (VP_ID != 0)
            {
                objModel.Table = context.GetVendorById(VP_ID);
            }
            else
            {
                objModel.Table = new Vendor();
                objModel.Table.OPM_ID = OPM_ID;
            }
            objModel.BuyerOrderNumberList = Extens.ToSelectList(objBuyerOrderNumber.GetDtBuyerOrderNumber(), "BuyerOrderNumberName", "BuyerOrderNumberName");
            objModel.ParticipiantNameList = Extens.ToSelectList(objParticipiantManager.GetDtParticipiantName(), "Name", "Name");
            objModel.SupplierNameList = Extens.ToSelectList(objSupplierListManager.GetDtSupplier(), "Suppliername", "Suppliername");
            return PartialView("VendorCRUD", objModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveVendor(Vendor_Model objModel, int page = 1, int pageSize = 5)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join("|", ModelState.Values.SelectMany(e => e.Errors).Select(em => em.ErrorMessage));
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, message);
            }
            int OPM_ID = objModel.Table.OPM_ID != null ? objModel.Table.OPM_ID.Value : 0;
            //Save     
            VendorManager context = new VendorManager(new DataContext());
            var msg = context.SaveVendor(objModel.Table);
            if (msg.Contains("exists"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "exists");
            }
            else
            {
                objModel.OPM_ID = OPM_ID;
                BindVendorGrid(objModel, page, pageSize);
                //return PartialView("VendorList", objModel);
                string vwString = HtmlHelpers.RenderViewToString(this.ControllerContext, "VendorList", objModel);
                return Json(new { OPM_ID = OPM_ID, viewData = vwString });
            }
        }

        [HttpPost]
        public ActionResult DeleteVendor(string VP_ID, int OPM_ID, int page = 1, int pageSize = 5)
        {
            VendorManager context = new VendorManager(new DataContext());

            if (!string.IsNullOrEmpty(VP_ID))
            {
                context.DeleteVendor(Convert.ToInt32(VP_ID));
            }
            Vendor_Model objModel = new Vendor_Model();
            objModel.OPM_ID = OPM_ID;
            BindVendorGrid(objModel, page, pageSize);
            return PartialView("VendorList", objModel);
        }
        #endregion

        #region Start PKGKDListGrid   
        public ActionResult PKGKDListFilterSearch(int OPM_ID, int page = 1, int pageSize = 5)
        {
            PKGKDList_Model objModel = new PKGKDList_Model();
            objModel.OPM_ID = OPM_ID;
            objModel.StaticPageSize = pageSize;
            BindPKGKDListGrid(objModel, page, pageSize);
            return PartialView("PKGKDListList", objModel);
        }
        public void BindPKGKDListGrid(PKGKDList_Model objModel, int page, int PageSize)
        {
            PKGKDListManager context = new PKGKDListManager(new DataContext());
            var PKGKDListList = context.GetPKGKDListList(objModel.OPM_ID);
            objModel.PKGKDListList = PKGKDListList.ToPagedList(page, PageSize).ToList();
            objModel.pageList = new PagedList<PKGKDList>(PKGKDListList, page, PageSize);
        }


        //Start Product Master
        [HttpPost]
        public ActionResult AddEditPKGKDList(int OPM_ID, int PkgDesID = 0)
        {
            BuyerOrderNumberListManager objBuyerOrderNumber = new BuyerOrderNumberListManager(new DataContext());
            PKGKDListManager context = new PKGKDListManager(new DataContext());
            PKGKDList_Model objModel = new PKGKDList_Model();
            if (PkgDesID != 0)
            {
                objModel.Table = context.GetPKGKDListById(PkgDesID);
            }
            else
            {
                objModel.Table = new PKGKDList();
                objModel.Table.OPM_ID = OPM_ID;
            }
            objModel.BuyerOrderNumberList = Extens.ToSelectList(objBuyerOrderNumber.GetDtBuyerOrderNumber(), "BuyerOrderNumberName", "BuyerOrderNumberName");            
            return PartialView("PKGKDListCRUD", objModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SavePKGKDList(PKGKDList_Model objModel, int page = 1, int pageSize = 5)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join("|", ModelState.Values.SelectMany(e => e.Errors).Select(em => em.ErrorMessage));
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, message);
            }
            int OPM_ID = objModel.Table.OPM_ID != null ? objModel.Table.OPM_ID.Value : 0;
            //Save     
            PKGKDListManager context = new PKGKDListManager(new DataContext());
            var msg = context.SavePKGKDList(objModel.Table);
            if (msg.Contains("exists"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "exists");
            }
            else
            {
                objModel.OPM_ID = OPM_ID;
                BindPKGKDListGrid(objModel, page, pageSize);
                //return PartialView("VendorList", objModel);
                string vwString = HtmlHelpers.RenderViewToString(this.ControllerContext, "PKGKDListList", objModel);
                return Json(new { OPM_ID = OPM_ID, viewData = vwString });
            }
        }

        [HttpPost]
        public ActionResult DeletePKGKDList(string PkgDesID, int OPM_ID, int page = 1, int pageSize = 5)
        {
            PKGKDListManager context = new PKGKDListManager(new DataContext());

            if (!string.IsNullOrEmpty(PkgDesID))
            {
                context.DeletePKGKDList(Convert.ToInt32(PkgDesID));
            }
            PKGKDList_Model objModel = new PKGKDList_Model();
            objModel.OPM_ID = OPM_ID;
            BindPKGKDListGrid(objModel, page, pageSize);
            return PartialView("PKGKDListList", objModel);
        }
        [HttpPost]
        public string GetPINames(string BuyerOrderNumber)
        {
            BuyerOrderNumberListManager objM = new BuyerOrderNumberListManager(new DataContext());
            return objM.GetPINumberByBuyerOrderNumber(BuyerOrderNumber);
        }
        #endregion

        #region Start StyleGrid   
        public ActionResult StyleFilterSearch(int OPM_ID, int page = 1, int pageSize = 5)
        {
            Style_Model objModel = new Style_Model();
            objModel.OPM_ID = OPM_ID;
            objModel.StaticPageSize = pageSize;
            BindStyleGrid(objModel, page, pageSize);
            return PartialView("StyleList", objModel);
        }
        public void BindStyleGrid(Style_Model objModel, int page, int pageSize)
        {
            StyleManager context = new StyleManager(new DataContext());
            objModel.StyleList = context.GetStyleList(objModel.OPM_ID);
            var StyleList = context.GetStyleList(objModel.OPM_ID);
            objModel.StyleList = StyleList.ToPagedList(page, pageSize).ToList();
            objModel.pageList = new PagedList<StyleInfo>(StyleList, page, pageSize);
        }
        
        [HttpPost]
        public ActionResult AddEditStyle(int OPM_ID, int Style_ID = 0)
        {
            StyleManager context = new StyleManager(new DataContext());
            Style_Model objModel = new Style_Model();

            if (Style_ID != 0)
            {
                objModel.Table = context.GetStyleById(Style_ID);
            }
            else
            {
                objModel.Table = new StyleInfo();
                objModel.Table.OPM_ID = OPM_ID;
            }
            return PartialView("StyleCRUD", objModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveStyle(Style_Model objModel, int page = 1, int pageSize = 5)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join("|", ModelState.Values.SelectMany(e => e.Errors).Select(em => em.ErrorMessage));
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, message);
            }
            int OPM_ID = objModel.Table.OPM_ID != null ? objModel.Table.OPM_ID.Value : 0;
            //Save     
            StyleManager context = new StyleManager(new DataContext());
            var msg = context.SaveStyle(objModel.Table);
            if (msg.Contains("exists"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "exists");
            }
            else
            {
                objModel.OPM_ID = OPM_ID;
                BindStyleGrid(objModel, page, pageSize);
                string vwString = HtmlHelpers.RenderViewToString(this.ControllerContext, "StyleList", objModel);
                return Json(new { OPM_ID = OPM_ID, viewData = vwString });
            }
        }

        [HttpPost]
        public ActionResult DeleteStyle(string Style_ID, int OPM_ID, int page = 1, int pageSize = 5)
        {
            StyleManager context = new StyleManager(new DataContext());

            if (!string.IsNullOrEmpty(Style_ID))
            {
                context.DeleteStyle(Convert.ToInt32(Style_ID));
            }
            Style_Model objModel = new Style_Model();
            objModel.OPM_ID = OPM_ID;
            BindStyleGrid(objModel, page, pageSize);
            return PartialView("StyleList", objModel);
        }
        #endregion

        #region Start TakkaGrid   
        public ActionResult TakkaFilterSearch(int OPM_ID, int page = 1, int pageSize = 5)
        {
            Takka_Model objModel = new Takka_Model();
            objModel.OPM_ID = OPM_ID;
            objModel.StaticPageSize = pageSize;
            BindTakkaGrid(objModel, page, pageSize);
            return PartialView("TakkaList", objModel);
        }
        public void BindTakkaGrid(Takka_Model objModel, int page, int pageSize)
        {
            TakkaManager context = new TakkaManager(new DataContext());
            var TakkaList = context.GetTakkaList(objModel.OPM_ID);
            objModel.TakkaList = TakkaList.ToPagedList(page, pageSize).ToList();
            objModel.pageList = new PagedList<Takka>(TakkaList, page, pageSize);
        }

        [HttpPost]
        public ActionResult AddEditTakka(int OPM_ID, int Takka_ID = 0)
        {
            TakkaManager context = new TakkaManager(new DataContext());
            Takka_Model objModel = new Takka_Model();

            if (Takka_ID != 0)
            {
                objModel.Table = context.GetTakkaById(Takka_ID);
            }
            else
            {
                objModel.Table = new Takka();
                objModel.Table.OPM_ID = OPM_ID;
            }
            return PartialView("TakkaCRUD", objModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveTakka(Takka_Model objModel, int page = 1, int pageSize = 5)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join("|", ModelState.Values.SelectMany(e => e.Errors).Select(em => em.ErrorMessage));
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, message);
            }
            int OPM_ID = objModel.Table.OPM_ID != null ? objModel.Table.OPM_ID.Value : 0;
            //Save     
            TakkaManager context = new TakkaManager(new DataContext());
            var msg = context.SaveTakka(objModel.Table);
            if (msg.Contains("exists"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "exists");
            }
            else
            {
                objModel.OPM_ID = OPM_ID;
                BindTakkaGrid(objModel, page, pageSize);
                string vwString = HtmlHelpers.RenderViewToString(this.ControllerContext, "TakkaList", objModel);
                return Json(new { OPM_ID = OPM_ID, viewData = vwString });
            }
        }

        [HttpPost]
        public ActionResult DeleteTakka(string Takka_ID, int OPM_ID, int page = 1, int pageSize = 5)
        {
            TakkaManager context = new TakkaManager(new DataContext());

            if (!string.IsNullOrEmpty(Takka_ID))
            {
                context.DeleteTakka(Convert.ToInt32(Takka_ID));
            }
            Takka_Model objModel = new Takka_Model();
            objModel.OPM_ID = OPM_ID;
            BindTakkaGrid(objModel, page, pageSize);
            return PartialView("TakkaList", objModel);
        }
        #endregion

        #region Start TargetGrid   
        public ActionResult TargetFilterSearch(int OPM_ID, int page = 1, int pageSize = 5)
        {
            Target_Model objModel = new Target_Model();
            objModel.OPM_ID = OPM_ID;
            objModel.StaticPageSize = pageSize;
            BindTargetGrid(objModel, page, pageSize);
            return PartialView("TargetList", objModel);
        }
        public void BindTargetGrid(Target_Model objModel, int page, int pageSize)
        {
            TargetManager context = new TargetManager(new DataContext());
            var TargetList = context.GetTargetList(objModel.OPM_ID);
            objModel.TargetList = TargetList.ToPagedList(page, pageSize).ToList();
            objModel.pageList = new PagedList<Target>(TargetList, page, pageSize);
        }
        
        [HttpPost]
        public ActionResult AddEditTarget(int OPM_ID, int Target_ID = 0)
        {
            TargetManager context = new TargetManager(new DataContext());
            Target_Model objModel = new Target_Model();

            if (Target_ID != 0)
            {
                objModel.Table = context.GetTargetById(Target_ID);
            }
            else
            {
                objModel.Table = new Target();
                objModel.Table.OPM_ID = OPM_ID;
            }
            return PartialView("TargetCRUD", objModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveTarget(Target_Model objModel, int page = 1, int pageSize = 5)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join("|", ModelState.Values.SelectMany(e => e.Errors).Select(em => em.ErrorMessage));
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, message);
            }
            int OPM_ID = objModel.Table.OPM_ID != null ? objModel.Table.OPM_ID.Value : 0;
            //Save     
            TargetManager context = new TargetManager(new DataContext());
            var msg = context.SaveTarget(objModel.Table);
            if (msg.Contains("exists"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "exists");
            }
            else
            {
                objModel.OPM_ID = OPM_ID;
                BindTargetGrid(objModel, page, pageSize);
                string vwString = HtmlHelpers.RenderViewToString(this.ControllerContext, "TargetList", objModel);
                return Json(new { OPM_ID = OPM_ID, viewData = vwString });
            }
        }

        [HttpPost]
        public ActionResult DeleteTarget(string Target_ID, int OPM_ID, int page = 1, int pageSize = 5)
        {
            TargetManager context = new TargetManager(new DataContext());

            if (!string.IsNullOrEmpty(Target_ID))
            {
                context.DeleteTarget(Convert.ToInt32(Target_ID));
            }
            Target_Model objModel = new Target_Model();
            objModel.OPM_ID = OPM_ID;
            BindTargetGrid(objModel, page, pageSize);
            return PartialView("TargetList", objModel);
        }
        #endregion

        #region Start QAInfoGrid   
        public ActionResult QAInfoFilterSearch(int OPM_ID, int page = 1, int pageSize = 5)
        {
            QAInfo_Model objModel = new QAInfo_Model();
            objModel.OPM_ID = OPM_ID;
            objModel.StaticPageSize = pageSize;
            BindQAInfoGrid(objModel, page, pageSize);
            return PartialView("QAInfoList", objModel);
        }
        public void BindQAInfoGrid(QAInfo_Model objModel, int page, int pageSize)
        {
            QAInfoManager context = new QAInfoManager(new DataContext());
            var QAInfoList = context.GetQAInfoList(objModel.OPM_ID);
            objModel.QAInfoList = QAInfoList.ToPagedList(page, pageSize).ToList();
            objModel.pageList = new PagedList<QAInfo>(QAInfoList, page, pageSize);
        }

        [HttpPost]
        public ActionResult AddEditQAInfo(int OPM_ID, int QAInfo_ID = 0)
        {
            QAInfoManager context = new QAInfoManager(new DataContext());
            QAInfo_Model objModel = new QAInfo_Model();

            if (QAInfo_ID != 0)
            {
                objModel.Table = context.GetQAInfoById(QAInfo_ID);
            }
            else
            {
                objModel.Table = new QAInfo();
                objModel.Table.OPM_ID = OPM_ID;
            }
            return PartialView("QAInfoCRUD", objModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveQAInfo(QAInfo_Model objModel, int page = 1, int pageSize = 5)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join("|", ModelState.Values.SelectMany(e => e.Errors).Select(em => em.ErrorMessage));
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, message);
            }
            int OPM_ID = objModel.Table.OPM_ID != null ? objModel.Table.OPM_ID.Value : 0;
            //Save     
            QAInfoManager context = new QAInfoManager(new DataContext());
            var msg = context.SaveQAInfo(objModel.Table);
            if (msg.Contains("exists"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "exists");
            }
            else
            {
                objModel.OPM_ID = OPM_ID;
                BindQAInfoGrid(objModel, page, pageSize);
                string vwString = HtmlHelpers.RenderViewToString(this.ControllerContext, "QAInfoList", objModel);
                return Json(new { OPM_ID = OPM_ID, viewData = vwString });
            }
        }

        [HttpPost]
        public ActionResult DeleteQAInfo(string QAInfo_ID, int OPM_ID, int page = 1, int pageSize = 5)
        {
            QAInfoManager context = new QAInfoManager(new DataContext());

            if (!string.IsNullOrEmpty(QAInfo_ID))
            {
                context.DeleteQAInfo(Convert.ToInt32(QAInfo_ID));
            }
            QAInfo_Model objModel = new QAInfo_Model();
            objModel.OPM_ID = OPM_ID;
            BindQAInfoGrid(objModel, page, pageSize);
            return PartialView("QAInfoList", objModel);
        }
        #endregion

        #region Start QAInspectionGrid   
        public ActionResult QAInspectionFilterSearch(int OPM_ID, int page = 1, int pageSize = 5)
        {
            QAInspection_Model objModel = new QAInspection_Model();
            objModel.OPM_ID = OPM_ID;
            objModel.StaticPageSize = pageSize;
            BindQAInspectionGrid(objModel, page, pageSize);
            return PartialView("QAInspectionList", objModel);
        }
        public void BindQAInspectionGrid(QAInspection_Model objModel, int page, int pageSize)
        {
            QAInspectionManager context = new QAInspectionManager(new DataContext());
            //objModel.QAInspectionList = context.GetQAInspectionList(objModel.OPM_ID);
            var QAInspectionList = context.GetQAInspectionList(objModel.OPM_ID);
            objModel.QAInspectionList = QAInspectionList.ToPagedList(page, pageSize).ToList();
            objModel.pageList = new PagedList<QAInspection>(QAInspectionList, page, pageSize);
        }

        [HttpPost]
        public ActionResult AddEditQAInspection(int OPM_ID, int QAIns_ID = 0)
        {
            TypeOfInspectionManager objTypeOfInspection = new TypeOfInspectionManager(new DataContext());
            QAInspectionManager context = new QAInspectionManager(new DataContext());
            QAInspection_Model objModel = new QAInspection_Model();

            if (QAIns_ID != 0)
            {
                objModel.Table = context.GetQAInspectionById(QAIns_ID);
            }
            else
            {
                objModel.Table = new QAInspection();
                objModel.Table.OPM_ID = OPM_ID;
            }
            objModel.InspectionTypeList = Extens.ToSelectList(objTypeOfInspection.GetTypeOfInspection(), "InspectionType", "InspectionType");
            return PartialView("QAInspectionCRUD", objModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveQAInspection(QAInspection_Model objModel, int page = 1, int pageSize = 5)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join("|", ModelState.Values.SelectMany(e => e.Errors).Select(em => em.ErrorMessage));
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, message);
            }
            int OPM_ID = objModel.Table.OPM_ID != null ? objModel.Table.OPM_ID.Value : 0;
            //Save     
            QAInspectionManager context = new QAInspectionManager(new DataContext());
            var msg = context.SaveQAInspection(objModel.Table);
            if (msg.Contains("exists"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "exists");
            }
            else
            {
                objModel.OPM_ID = OPM_ID;
                BindQAInspectionGrid(objModel, page, pageSize);
                string vwString = HtmlHelpers.RenderViewToString(this.ControllerContext, "QAInspectionList", objModel);
                return Json(new { OPM_ID = OPM_ID, viewData = vwString });
            }
        }

        [HttpPost]
        public ActionResult DeleteQAInspection(string QAIns_ID, int OPM_ID, int page = 1, int pageSize = 5)
        {
            QAInspectionManager context = new QAInspectionManager(new DataContext());

            if (!string.IsNullOrEmpty(QAIns_ID))
            {
                context.DeleteQAInspection(Convert.ToInt32(QAIns_ID));
            }
            QAInspection_Model objModel = new QAInspection_Model();
            objModel.OPM_ID = OPM_ID;
            BindQAInspectionGrid(objModel, page, pageSize);
            return PartialView("QAInspectionList", objModel);
        }
        #endregion



        #region Start ProductGrid   
        public ActionResult ProductFilterSearch(int OPM_ID, int page = 1, int pageSize = 5)
        {
            ProductUpdateGrid_Model objModel = new ProductUpdateGrid_Model();
            objModel.OPM_ID = OPM_ID;
            objModel.StaticPageSize = pageSize;
            BindProductGrid(objModel, page, pageSize);
            return PartialView("ProductList", objModel);
        }
        public void BindProductGrid(ProductUpdateGrid_Model objModel, int page, int pageSize)
        {
            ProductManager context = new ProductManager(new DataContext());
            //objModel.ProductsList = context.GetProductList(objModel.OPM_ID);
            var ProductsList = context.GetProductList(objModel.OPM_ID);
            objModel.ProductsList = ProductsList.ToPagedList(page, pageSize).ToList();
            objModel.pageList = new PagedList<ProductUpdateGrid>(ProductsList, page, pageSize);
        }

        [HttpPost]
        public ActionResult AddEditProduct(int OPM_ID, int ProductID = 0)
        {
            ProductManager context = new ProductManager(new DataContext());
            ProductUpdateGrid_Model objModel = new ProductUpdateGrid_Model();

            BuyerListManager objbuyerListManager = new BuyerListManager(new DataContext());
            ProductListManager objProductListManager = new ProductListManager(new DataContext());
            ProcessListManager objProcessListManager = new ProcessListManager(new DataContext());
            BuyerOrderNumberListManager objBuyerOrderNumberListManager = new BuyerOrderNumberListManager(new DataContext());
            SupplierListManager objSupplierListManager = new SupplierListManager(new DataContext());
            UnitListManager objUnitListManager = new UnitListManager(new DataContext());
            SetNoteListManager objSetNoteListManager = new SetNoteListManager(new DataContext());


            if (ProductID != 0)
            {
                objModel.Table = context.GetProductMasterById(ProductID);
            }
            else
            {
                objModel.Table = new ProductUpdateGrid();
                objModel.Table.OPM_ID = OPM_ID;
            }
            objModel.BuyerList = Extens.ToSelectList(objbuyerListManager.GetDtBuyer(), "Buyername", "Buyername");
            objModel.ProductList = Extens.ToSelectList(objProductListManager.GetDtProduct(), "Productname", "Productname");
            objModel.ProcessList = Extens.ToSelectList(objProcessListManager.GetDtProcess(), "Processname", "Processname");
            objModel.BuyerOrderNumberList = Extens.ToSelectList(objBuyerOrderNumberListManager.GetDtBuyerOrderNumber(), "BuyerOrderNumberName", "BuyerOrderNumberName");
            objModel.SupplierList = Extens.ToSelectList(objSupplierListManager.GetDtSupplier(), "Suppliername", "Suppliername");
            objModel.UnitList = Extens.ToSelectList(objUnitListManager.GetDtUnit(), "Unitname", "Unitname");
            objModel.SetNoteList = Extens.ToSelectList(objSetNoteListManager.GetDtSetNote(), "Setnotename", "Setnotename");

            return PartialView("ProductCRUD", objModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveProduct(ProductUpdateGrid_Model objModel, int page = 1, int pageSize = 5)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join("|", ModelState.Values.SelectMany(e => e.Errors).Select(em => em.ErrorMessage));
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, message);
            }
            int OPM_ID = objModel.Table.OPM_ID != null ? objModel.Table.OPM_ID.Value : 0;
            //Save     
            ProductManager context = new ProductManager(new DataContext());
            var msg = context.SaveProductMaster(objModel.Table);
            if (msg.Contains("exists"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "exists");
            }
            else
            {
                objModel.OPM_ID = OPM_ID;
                BindProductGrid(objModel, page, pageSize);
                string vwString = HtmlHelpers.RenderViewToString(this.ControllerContext, "ProductList", objModel);
                return Json(new { OPM_ID = OPM_ID, viewData = vwString });
            }
        }

        [HttpPost]
        public ActionResult DeleteProduct(string ProductID, int OPM_ID, int page = 1, int pageSize = 5)
        {
            ProductManager context = new ProductManager(new DataContext());

            if (!string.IsNullOrEmpty(ProductID))
            {
                context.DeleteProductMaster(Convert.ToInt32(ProductID));
            }
            ProductUpdateGrid_Model objModel = new ProductUpdateGrid_Model();
            objModel.OPM_ID = OPM_ID;
            BindProductGrid(objModel, page, pageSize);
            return PartialView("ProductList", objModel);
        }

        [HttpPost]
        public string GetPIName(string BuyerOrderNumber)
        {
            BuyerOrderNumberListManager objM = new BuyerOrderNumberListManager(new DataContext());
            return objM.GetPINumberByBuyerOrderNumber(BuyerOrderNumber);
        }
        #endregion
        public ActionResult DownloadImportSample()
        {
            string path = Server.MapPath("~/Content/Excel/ProductImportSample.xlsx");
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            string fileName = "ProductImportSample.xlsx";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        public ActionResult ProductImport(int Import_OPM_ID, HttpPostedFileBase FileUpload)
        {
            HttpPostedFileBase file = Request.Files[0];
            if (FileUpload != null)
            {
                if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    string filename = FileUpload.FileName;

                    if (filename.EndsWith(".xlsx"))
                    {
                        string targetpath = Server.MapPath("~/Content/Excel/");
                        if (!System.IO.Directory.Exists(targetpath))
                        {
                            System.IO.Directory.CreateDirectory(targetpath);
                        }
                        FileUpload.SaveAs(targetpath + filename);
                        string pathToExcelFile = targetpath + filename;
                        DataTable dtExcel = Excel_To_DataTable(pathToExcelFile, 0);
                        List<ProductUpdateGrid> lstProductUpdateGrid = dtExcel.DataTableToList<ProductUpdateGrid>();

                        SaveImportProduct(lstProductUpdateGrid, Import_OPM_ID);
                        ProductUpdateGrid_Model objModel = new ProductUpdateGrid_Model();
                        objModel.OPM_ID = Import_OPM_ID;
                        BindProductGrid(objModel, 1, 10);
                        return PartialView("ProductList", objModel);
                    }
                    else
                    {
                        return Json(new { msg = "Select Excel File" });
                    }
                }
                else
                {
                    return Json(new { msg = "Select Excel File" });
                }
            }
            else
            {
                return Json(new { msg = "Select Excel File" });
            }
        }

        public string SaveImportProduct(List<ProductUpdateGrid> lstProductUpdateGrid, int Import_OPM_ID)
        {
            StringBuilder str = new StringBuilder();
            foreach (ProductUpdateGrid obj in lstProductUpdateGrid)
            {
                try
                {
                    ProductManager context = new ProductManager(new DataContext());
                    obj.OPM_ID = Import_OPM_ID;
                    var msg = context.SaveProductMaster(obj);
                }
                catch (Exception ex)
                {
                    str.AppendLine(obj.Processname + " " + ex.Message);
                }
            }
            if (str.Length != 0)
            {
                return CommonFunction.WriteErrorLog(str.ToString());
            }
            return "";
        }
        private DataTable Excel_To_DataTable(string file, int index)
        {
            DataTable Tabla = null;
            try
            {
                if (System.IO.File.Exists(file))
                {
                    IWorkbook workbook = null;
                    ISheet worksheet = null;
                    string first_sheet_name = "";

                    using (FileStream FS = new FileStream(file, FileMode.Open, FileAccess.Read))
                    {
                        workbook = WorkbookFactory.Create(FS);
                        worksheet = workbook.GetSheetAt(index);
                        first_sheet_name = worksheet.SheetName;

                        Tabla = new DataTable(first_sheet_name);
                        Tabla.Rows.Clear();
                        Tabla.Columns.Clear();

                        for (int rowIndex = 0; rowIndex <= worksheet.LastRowNum; rowIndex++)
                        {
                            DataRow NewReg = null;
                            IRow row = worksheet.GetRow(rowIndex);
                            IRow row2 = null;

                            if (row != null)
                            {
                                if (rowIndex > 0) NewReg = Tabla.NewRow();

                                foreach (ICell cell in row.Cells)
                                {
                                    object valorCell = null;
                                    string cellType = "";

                                    if (rowIndex == 0)
                                    {
                                        row2 = worksheet.GetRow(rowIndex + 1);
                                        ICell cell2 = row2.GetCell(cell.ColumnIndex, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                        switch (cell2.CellType)
                                        {
                                            case CellType.Boolean: cellType = "System.Boolean"; break;
                                            case CellType.String: cellType = "System.String"; break;
                                            case CellType.Numeric:
                                                if (HSSFDateUtil.IsCellDateFormatted(cell2)) { cellType = "System.DateTime"; }
                                                else { cellType = "System.Double"; }
                                                break;
                                            case CellType.Formula:
                                                switch (cell2.CachedFormulaResultType)
                                                {
                                                    case CellType.Boolean: cellType = "System.Boolean"; break;
                                                    case CellType.String: cellType = "System.String"; break;
                                                    case CellType.Numeric:
                                                        if (HSSFDateUtil.IsCellDateFormatted(cell2)) { cellType = "System.DateTime"; }
                                                        else { cellType = "System.Double"; }
                                                        break;
                                                }
                                                break;
                                            default:
                                                cellType = "System.String"; break;
                                        }

                                        DataColumn codigo = new DataColumn(cell.StringCellValue, System.Type.GetType(cellType));
                                        Tabla.Columns.Add(codigo);
                                    }
                                    else
                                    {
                                        switch (cell.CellType)
                                        {
                                            case CellType.Blank: valorCell = DBNull.Value; break;
                                            case CellType.Boolean: valorCell = cell.BooleanCellValue; break;
                                            case CellType.String: valorCell = cell.StringCellValue; break;
                                            case CellType.Numeric:
                                                if (HSSFDateUtil.IsCellDateFormatted(cell)) { valorCell = cell.DateCellValue; }
                                                else { valorCell = cell.NumericCellValue; }
                                                break;
                                            case CellType.Formula:
                                                switch (cell.CachedFormulaResultType)
                                                {
                                                    case CellType.Blank: valorCell = DBNull.Value; break;
                                                    case CellType.String: valorCell = cell.StringCellValue; break;
                                                    case CellType.Boolean: valorCell = cell.BooleanCellValue; break;
                                                    case CellType.Numeric:
                                                        if (HSSFDateUtil.IsCellDateFormatted(cell)) { valorCell = cell.DateCellValue; }
                                                        else { valorCell = cell.NumericCellValue; }
                                                        break;
                                                }
                                                break;
                                            default: valorCell = cell.StringCellValue; break;
                                        }
                                        NewReg[cell.ColumnIndex] = valorCell;
                                    }
                                }
                            }
                            if (rowIndex > 0) Tabla.Rows.Add(NewReg);
                        }
                        Tabla.AcceptChanges();
                    }
                }
                else
                {
                    throw new Exception("ERROR 404:");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Tabla;
        }
    }
}