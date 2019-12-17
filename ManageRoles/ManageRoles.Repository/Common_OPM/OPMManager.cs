using ManageRoles.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ManageRoles.Repository{
    
    #region OPM
    public class OPM_Model : CommonModel<OPM>
    {
        public SelectList BuyerReferenceNameList { get; set; }
        public SelectList BuyerStoryNameList { get; set; }
        public SelectList BuyerNameList { get; set; }
        public SelectList ParticipantList { get; set; }
        public string[] Participant { get; set; }
        public string[] BuyerReferenceName { get; set; }

    }
    public class OPMManager : CommonRepository<OPM>
    {
        public OPMManager(DbContext context) : base(context) { }

        public List<OPM> GetOPMList()
        {
            var lst = this.GetAll().ToList();
            return lst;
        }

        public void AddOPM(OPM obj)
        {
            this.Add(obj);
        }

        public void UpdateOPM(OPM obj)
        {
            this.Update(obj);
        }

        public OPM GetOPMById(int Id)
        {
            return this.GetById(Id);
        }

        public void DeleteOPM(int Id)
        {
            this.Delete(Id);
        }

        public string SaveOPM(OPM objOPM)
        {
            using (var context = new CommonRepository<OPM>(this.DbContext))
            {
                if (objOPM.OPM_ID == 0)
                {
                    context.Add(objOPM);
                    return "success";
                }
                else
                {
                    context.Update(objOPM);
                    return "success";
                }
            }
        }
    }
    #endregion OPM

    #region BuyerReferenceManager
    public class BuyerReferenceManager : CommonRepository<BuyerReference>
    {
        public BuyerReferenceManager(DbContext context) : base(context) { }
        public DataTable GetDtBuyerReferenceName()
        {
            return GetDataTable("SELECT BuyerID,BuyerReferenceName FROM BuyerReference ORDER BY BuyerReferenceName");
        }

        public DataTable GetDtBuyerStoryName()
        {
            return GetDataTable("SELECT BuyerID,StoryName FROM BuyerReference ORDER BY StoryName");
        }
    }
    #endregion

    #region BuyerReferenceManager
    public class BuyerNameManager : CommonRepository<Buyername>
    {
        public BuyerNameManager(DbContext context) : base(context) { }
        public DataTable GetDtBuyerName()
        {
            return GetDataTable("SELECT ByrID,Buyername FROM Buyername ORDER BY Buyername");
        }
    }
    #endregion
        

    #region Vendor
    public class Vendor_Model : CommonModel<Vendor>
    {
        public int OPM_ID { get; set; }
        public SelectList BuyerOrderNumberList { get; set; }

        public SelectList SupplierNameList { get; set; }//Vendor
        public SelectList ParticipiantNameList { get; set; }

        public List<Vendor> VendorList { get; set; }
    }
    public class VendorManager : CommonRepository<Vendor>
    {
        public VendorManager(DbContext context) : base(context) { }

        public List<Vendor> GetVendorList()
        {
            var lst = this.GetAll().ToList();
            return lst;
        }

        public List<Vendor> GetVendorList(int OPM_ID)
        {
            if (OPM_ID == 0)
            {
                return new List<Vendor>();
            }
            var lst = this.GetAll(c => c.OPM_ID == OPM_ID).OrderByDescending(c=>c.VP_ID).ToList();
            return lst;
        }

        public void AddVendor(Vendor obj)
        {
            this.Add(obj);
        }

        public void UpdateVendor(Vendor obj)
        {
            this.Update(obj);
        }

        public Vendor GetVendorById(int Id)
        {
            return this.GetById(Id);
        }

        public void DeleteVendor(int Id)
        {
            this.Delete(Id);
        }

        public string SaveVendor(Vendor objVendor)
        {
            using (var context = new CommonRepository<Vendor>(this.DbContext))
            {
                if (objVendor.VP_ID == 0)
                {
                    context.Add(objVendor);
                    return "success";
                }
                else
                {
                    context.Update(objVendor);
                    return "success";
                }
            }
        }
    }
    #endregion Vendor

    #region PKGKDList
    public class PKGKDList_Model : CommonModel<PKGKDList>
    {
        public int OPM_ID { get; set; }
        public SelectList BuyerOrderNumberList { get; set; }      

        public List<PKGKDList> PKGKDListList { get; set; }
    }
    public class PKGKDListManager : CommonRepository<PKGKDList>
    {
        public PKGKDListManager(DbContext context) : base(context) { }

        public List<PKGKDList> GetPKGKDListList()
        {
            var lst = this.GetAll().ToList();
            return lst;
        }

        public List<PKGKDList> GetPKGKDListList(int OPM_ID)
        {
            if (OPM_ID == 0)
            {
                return new List<PKGKDList>();
            }
            var lst = this.GetAll(c => c.OPM_ID == OPM_ID).OrderByDescending(c => c.PkgDesID).ToList();
            return lst;
        }

        public void AddPKGKDList(PKGKDList obj)
        {
            this.Add(obj);
        }

        public void UpdatePKGKDList(PKGKDList obj)
        {
            this.Update(obj);
        }

        public PKGKDList GetPKGKDListById(int Id)
        {
            return this.GetById(Id);
        }

        public void DeletePKGKDList(int Id)
        {
            this.Delete(Id);
        }

        public string SavePKGKDList(PKGKDList objPKGKDList)
        {
            using (var context = new CommonRepository<PKGKDList>(this.DbContext))
            {
                if (objPKGKDList.PkgDesID == 0)
                {
                    context.Add(objPKGKDList);
                    return "success";
                }
                else
                {
                    context.Update(objPKGKDList);
                    return "success";
                }
            }
        }
    }
    #endregion PKGKDList

    #region Style
    public class Style_Model : CommonModel<StyleInfo>
    {
        public int OPM_ID { get; set; }
        public List<StyleInfo> StyleList { get; set; }
    }
    public class StyleManager : CommonRepository<StyleInfo>
    {
        public StyleManager(DbContext context) : base(context) { }

        public List<StyleInfo> GetStyleList()
        {
            var lst = this.GetAll().ToList();
            return lst;
        }

        public List<StyleInfo> GetStyleList(int OPM_ID)
        {
            if (OPM_ID == 0)
            {
                return new List<StyleInfo>();
            }
            var lst = this.GetAll(c => c.OPM_ID == OPM_ID).OrderByDescending(c => c.Style_ID).ToList();
            return lst;
        }

        public void AddStyle(StyleInfo obj)
        {
            this.Add(obj);
        }

        public void UpdateStyle(StyleInfo obj)
        {
            this.Update(obj);
        }

        public StyleInfo GetStyleById(int Id)
        {
            return this.GetById(Id);
        }

        public void DeleteStyle(int Id)
        {
            this.Delete(Id);
        }

        public string SaveStyle(StyleInfo objStyle)
        {
            using (var context = new CommonRepository<StyleInfo>(this.DbContext))
            {
                if (objStyle.Style_ID == 0)
                {
                    context.Add(objStyle);
                    return "success";
                }
                else
                {
                    context.Update(objStyle);
                    return "success";
                }
            }
        }
    }
    #endregion Style

    #region Takka
    public class Takka_Model : CommonModel<Takka>
    {
        public int OPM_ID { get; set; }
        public List<Takka> TakkaList { get; set; }
    }
    public class TakkaManager : CommonRepository<Takka>
    {
        public TakkaManager(DbContext context) : base(context) { }

        public List<Takka> GetTakkaList()
        {
            var lst = this.GetAll().ToList();
            return lst;
        }

        public List<Takka> GetTakkaList(int OPM_ID)
        {
            if (OPM_ID == 0)
            {
                return new List<Takka>();
            }
            var lst = this.GetAll(c => c.OPM_ID == OPM_ID).OrderByDescending(c => c.Takka_ID).ToList();
            return lst;
        }

        public void AddTakka(Takka obj)
        {
            this.Add(obj);
        }

        public void UpdateTakka(Takka obj)
        {
            this.Update(obj);
        }

        public Takka GetTakkaById(int Id)
        {
            return this.GetById(Id);
        }

        public void DeleteTakka(int Id)
        {
            this.Delete(Id);
        }

        public string SaveTakka(Takka objTakka)
        {
            using (var context = new CommonRepository<Takka>(this.DbContext))
            {
                if (objTakka.Takka_ID == 0)
                {
                    context.Add(objTakka);
                    return "success";
                }
                else
                {
                    context.Update(objTakka);
                    return "success";
                }
            }
        }
    }
    #endregion Takka

    #region Target
    public class Target_Model : CommonModel<Target>
    {
        public int OPM_ID { get; set; }
        public List<Target> TargetList { get; set; }
    }
    public class TargetManager : CommonRepository<Target>
    {
        public TargetManager(DbContext context) : base(context) { }

        public List<Target> GetTargetList()
        {
            var lst = this.GetAll().ToList();
            return lst;
        }

        public List<Target> GetTargetList(int OPM_ID)
        {
            if (OPM_ID == 0)
            {
                return new List<Target>();
            }
            var lst = this.GetAll(c => c.OPM_ID == OPM_ID).OrderByDescending(c => c.Target_ID).ToList();
            return lst;
        }

        public void AddTarget(Target obj)
        {
            this.Add(obj);
        }

        public void UpdateTarget(Target obj)
        {
            this.Update(obj);
        }

        public Target GetTargetById(int Id)
        {
            return this.GetById(Id);
        }

        public void DeleteTarget(int Id)
        {
            this.Delete(Id);
        }

        public string SaveTarget(Target objTarget)
        {
            using (var context = new CommonRepository<Target>(this.DbContext))
            {
                if (objTarget.Target_ID == 0)
                {
                    context.Add(objTarget);
                    return "success";
                }
                else
                {
                    context.Update(objTarget);
                    return "success";
                }
            }
        }
    }
    #endregion Target

    #region QAInfo
    public class QAInfo_Model : CommonModel<QAInfo>
    {
        public int OPM_ID { get; set; }
        public List<QAInfo> QAInfoList { get; set; }
    }
    public class QAInfoManager : CommonRepository<QAInfo>
    {
        public QAInfoManager(DbContext context) : base(context) { }

        public List<QAInfo> GetQAInfoList()
        {
            var lst = this.GetAll().ToList();
            return lst;
        }

        public List<QAInfo> GetQAInfoList(int OPM_ID)
        {
            if (OPM_ID == 0)
            {
                return new List<QAInfo>();
            }
            var lst = this.GetAll(c => c.OPM_ID == OPM_ID).OrderByDescending(c => c.QAInfo_ID).ToList();
            return lst;
        }

        public void AddQAInfo(QAInfo obj)
        {
            this.Add(obj);
        }

        public void UpdateQAInfo(QAInfo obj)
        {
            this.Update(obj);
        }

        public QAInfo GetQAInfoById(int Id)
        {
            return this.GetById(Id);
        }

        public void DeleteQAInfo(int Id)
        {
            this.Delete(Id);
        }

        public string SaveQAInfo(QAInfo objQAInfo)
        {
            using (var context = new CommonRepository<QAInfo>(this.DbContext))
            {
                if (objQAInfo.QAInfo_ID == 0)
                {
                    context.Add(objQAInfo);
                    return "success";
                }
                else
                {
                    context.Update(objQAInfo);
                    return "success";
                }
            }
        }
    }
    #endregion QAInfo

    #region QAInspection
    public class QAInspection_Model : CommonModel<QAInspection>
    {
        public int OPM_ID { get; set; }
        public SelectList InspectionTypeList { get; set; }
        public List<QAInspection> QAInspectionList { get; set; }
    }
    public class QAInspectionManager : CommonRepository<QAInspection>
    {
        public QAInspectionManager(DbContext context) : base(context) { }

        public List<QAInspection> GetQAInspectionList()
        {
            var lst = this.GetAll().ToList();
            return lst;
        }

        public List<QAInspection> GetQAInspectionList(int OPM_ID)
        {
            if (OPM_ID == 0)
            {
                return new List<QAInspection>();
            }
            var lst = this.GetAll(c => c.OPM_ID == OPM_ID).OrderByDescending(c => c.QAIns_ID).ToList();
            return lst;
        }

        public void AddQAInspection(QAInspection obj)
        {
            this.Add(obj);
        }

        public void UpdateQAInspection(QAInspection obj)
        {
            this.Update(obj);
        }

        public QAInspection GetQAInspectionById(int Id)
        {
            return this.GetById(Id);
        }

        public void DeleteQAInspection(int Id)
        {
            this.Delete(Id);
        }

        public string SaveQAInspection(QAInspection objQAInspection)
        {
            using (var context = new CommonRepository<QAInspection>(this.DbContext))
            {
                if (objQAInspection.QAIns_ID == 0)
                {
                    context.Add(objQAInspection);
                    return "success";
                }
                else
                {
                    context.Update(objQAInspection);
                    return "success";
                }
            }
        }
    }
    #endregion QAInspection    

    #region TypeOfInspection
    public class TypeOfInspectionManager : CommonRepository<TypeOfInspection>
    {
        public TypeOfInspectionManager(DbContext context) : base(context) { }
        public DataTable GetTypeOfInspection()
        {
            return GetDataTable("SELECT InsID,InspectionType FROM TypeOfInspection ORDER BY InspectionType");
        }
    }
    #endregion

    #region ParticipiantManager
    public class ParticipiantManager : CommonRepository<Participiant>
    {
        public ParticipiantManager(DbContext context) : base(context) { }
        public DataTable GetDtParticipiantName()
        {
            return GetDataTable("SELECT ID,Name FROM Participiant ORDER BY Name");
        }
    }
    #endregion

    #region ProductSummary
    public class SeriesData
    {
        public string name { get; set; }
        public List<decimal> data { get; set; }
    }
    public class VW_ProductSummary_Model
    {
        public SelectList BuyerList { get; set; }
        public SelectList BuyerOrderNumberList { get; set; }
        public SelectList ProcessList { get; set; }
        public List<SeriesData> SeriesList { get; set; }
        public string Json { get; set; }
        public string categories { get; set; }
        public string ProcessName { get; set; }
        public string BuyerOrderNumberName { get; set; }
       
    }
    public class VW_ProductSummaryManager : CommonRepository<VW_ProductSummary>
    {
        public VW_ProductSummaryManager(DbContext context) : base(context) { }
        //public List<VW_ProductSummary> GetProductSummaryList()
        //{
        //    var lst = this.GetAll().ToList();
        //    return lst;
        //}

        public List<VW_ProductSummary> GetProductSummaryList(string Buyername =null, string BuyerOrderNumberName = null, string Processname = null)
        {
            List<VW_ProductSummary> lst = new List<VW_ProductSummary>();
            if (!string.IsNullOrEmpty(Buyername) && !String.IsNullOrEmpty(BuyerOrderNumberName) && !string.IsNullOrEmpty(Processname))
            {
                lst = this.GetAll(c => c.Buyername == Buyername && c.BuyerOrderNumberName == BuyerOrderNumberName && c.Processname == Processname).ToList();
            }
            else
            {
                lst = this.GetAll().ToList();
            }
            return lst;
        }
    }
    #endregion

    #region Gantt
    public class ProductGantt_Model
    {
        public SelectList BuyerList { get; set; }
        public SelectList BuyerOrderNumberList { get; set; }
        public SelectList ProcessList { get; set; }
        public List<ProductGantt> SeriesList { get; set; }
        public string ProcessName { get; set; }
        public string BuyerOrderNumberName { get; set; }
        public string Json { get; set; }
       
    }
    public class ProdcutGroupGanttManager : CommonRepository<VW_ProdcutGroupGantt>
    {
        public ProdcutGroupGanttManager(DbContext context) : base(context) { }
        
    }
    #endregion

    #region GroupBar
    public class GroupBarModel
    {
        public SelectList BuyerOrderNumberList { get; set; }
        public string categories { get; set; }
        public string ProductQty { get; set; }
        public string FinishedQty { get; set; }
        public string BalanceQty { get; set; }
    }
    public class ProductGroupChartManager : CommonRepository<VW_ProductGroupChart>
    {
        public ProductGroupChartManager(DbContext context) : base(context) { }
        public List<VW_ProductGroupChart> GetList()
        {
            var lst = this.GetAll().ToList();
            return lst;
        }
    }
    #endregion

    public class TaskViewModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int? ParentID { get; set; }
        public int OrderID { get; set; }
        public int ChildId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public double PercentComplete { get; set; }
        public bool Summary { get; set; }
        public bool Expanded { get; set; }
        public int NoDays { get; set; }

        public string Buyername { get; set;}
        public string BuyerOrderNumberName { get; set; }
        public string Processname { get; set; }
        public int DelayedDays { get; set; }
        public decimal Done { get; set; }
        public DateTime? SKUStart { get; set; }
        public DateTime? SKUEnd { get; set; }
    }
}
