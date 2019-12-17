
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageRoles.Repository
{
    public class ProductManager : CommonRepository<ProductUpdateGrid>
    {
        public ProductManager(DbContext context) : base(context) { }

        public List<ProductUpdateGrid> GetProductMasterList()
        {
            var lst = this.GetAll().ToList();
            return lst;
        }

        public List<ProductUpdateGrid> GetProductList(int OPM_ID)
        {
            var lst = this.GetAll(c=>c.OPM_ID == OPM_ID).OrderByDescending(c => c.ProductID).ToList();
            return lst;
        }

        public void AddProductMaster(ProductUpdateGrid obj)
        {
            this.Add(obj);
        }

        public void UpdateProductMaster(ProductUpdateGrid obj)
        {
            this.Update(obj);
        }

        public ProductUpdateGrid GetProductMasterById(int Id)
        {
            return this.GetById(Id);
        }

        public void DeleteProductMaster(int Id)
        {
            this.Delete(Id);
        }

        public string SaveProductMaster(ProductUpdateGrid objProductMaster)
        {
            using (var context = new CommonRepository<ProductUpdateGrid>(this.DbContext))
            {
                if (objProductMaster.ProductID == 0)
                {
                    //var res = context.FirstOrDefault(d => d.Productname == objProductMaster.Productname);
                    //if (res != null) { return "exists"; }
                    //else
                    //{
                    //objProductMaster.CreatedDate = DateTime.Now;
                    context.Add(objProductMaster);
                    return "success";
                    //}
                }
                else
                {
                    //var res = context.FirstOrDefault(d => d.Productname == objProductMaster.Productname && d.ProductID != objProductMaster.ProductID);
                    //if (res != null) { return "exists"; }
                    //else
                    //{
                    context.Update(objProductMaster);
                    return "success";
                    //}
                }

            }
        }

        public void UpdateQty(int Id, decimal PlannedQty, decimal FinishedQty)
        {
            using (var context = new CommonRepository<ProductUpdateGrid>(this.DbContext))
            {
                var obj = context.FirstOrDefault(d => d.ProductID == Id);
                obj.PlannedQty = PlannedQty;
                obj.FinishedQty = FinishedQty;
                obj.BalanceQty = obj.ProductQty - FinishedQty;
                context.Update(obj);
            }
        }
        public Tuple<decimal, decimal, decimal> GetTotalProductGrid(int Id)
        {
            using (DataContext context = new DataContext())
            {
                var res = context.ProductUpdateGrid.GroupBy(d => true).Select(d =>
                  new
                  {
                      tot_PlannedQty = d.Sum(x => x.PlannedQty),
                      tot_FinishedQty = d.Sum(x => x.FinishedQty),
                      BalanceQty = d.Sum(x => x.BalanceQty)
                  }).ToList();

                if (res.Count > 0)
                    return new Tuple<decimal, decimal, decimal>(res[0].tot_PlannedQty, res[0].tot_FinishedQty, res[0].BalanceQty);
                else
                    return new Tuple<decimal, decimal, decimal>(0, 0, 0);
            }
        }
        public Tuple<decimal, decimal, decimal> GetRowProductGrid(int Id)
        {
            using (DataContext context = new DataContext())
            {
                var res = context.ProductUpdateGrid.Where(d => d.ProductID == Id).GroupBy(d => true).Select(d =>
                    new
                    {
                        tot_PlannedQty = d.Sum(x => x.PlannedQty),
                        tot_FinishedQty = d.Sum(x => x.FinishedQty),
                        tot_BalanceQty = d.Sum(x => x.BalanceQty)
                    }).ToList();
                if (res.Count > 0)
                    return new Tuple<decimal, decimal, decimal>(res[0].tot_PlannedQty, res[0].tot_FinishedQty, res[0].tot_BalanceQty);
                else
                    return new Tuple<decimal, decimal, decimal>(0, 0, 0);
            }
        }

    }

    public class ProductUpdateGrid_Model : CommonModel<ProductUpdateGrid>
    {
        public SelectList BuyerList { get; set; }
        public SelectList BuyerOrderNumberList { get; set; }       
        public SelectList ProcessList { get; set; }        
        public SelectList SupplierList { get; set; }
        public SelectList ProductList { get; set; }
        public SelectList UnitList { get; set; }
        public SelectList SetNoteList { get; set; }
        public string SortNameType { get; set; }
        public int OPM_ID { get; set; }
        public List<ProductUpdateGrid> ProductsList { get; set; }
        public List<PlannerUpdateGrid> PlannerUpdateGridList { get; set; }
        public List<FinishingUpdateGrid> FinishingUpdateGridList { get; set; }
    }

    public class PlannerManager : CommonRepository<PlannerUpdateGrid>
    {
        public PlannerManager(DbContext context) : base(context) { }
        public List<PlannerUpdateGrid> GetPlannerUpdateGridList()
        {
            var lst = this.GetAll().ToList();
            return lst;
        }
        public PlannerUpdateGrid GetPlannerMasterById(int Id)
        {
            using (DataContext context = new DataContext())
            {
                return context.PlannerUpdateGrid.Where(d => d.PlannerID == Id).FirstOrDefault();
                //return this.GetById(Id);
            }
        }
        public string SavePlannerMaster(PlannerUpdateGrid objPlannerMaster)
        {
            using (var context = new CommonRepository<PlannerUpdateGrid>(this.DbContext))
            {
                if (objPlannerMaster.PlannerID == 0)
                {
                    context.Add(objPlannerMaster);
                    return "success";
                }
                else
                {
                    context.Update(objPlannerMaster);
                    return "success";
                }
            }
        }
        public void DeletePlannerMaster(int Id)
        {
            this.Delete(Id);
        }

        public Tuple<decimal, decimal> GetTotalPlannerGrid(int Id)
        {
            using (DataContext context = new DataContext())
            {
                var res = context.PlannerUpdateGrid.Where(d => d.ProductID == Id).GroupBy(d => d.ProductID).Select(d =>
                   new
                   {
                       tot_PlannedQty = d.Sum (x => x.PlannedQty),
                       tot_FinishedQty = d.Sum(x => x.FinishedQty)
                   }).ToList();
                if (res.Count > 0)
                    return new Tuple<decimal, decimal>(res[0].tot_PlannedQty, res[0].tot_FinishedQty);
                else
                    return new Tuple<decimal, decimal>(0, 0);

            }
        }
        public void UpdateQty(int Id, decimal FinishedQty)
        {
            using (var context = new CommonRepository<PlannerUpdateGrid>(this.DbContext))
            {
                var obj = context.FirstOrDefault(d => d.PlannerID == Id);
                obj.FinishedQty = FinishedQty;
                context.Update(obj);
            }
        }

        public decimal GetRowPlannerGrid(int Id)
        {
            using (DataContext context = new DataContext())
            {
                var res = context.PlannerUpdateGrid.Where(d => d.PlannerID == Id).Sum(d => d.FinishedQty);
                return res;
            }
        }
    }


    public class FinishingManager : CommonRepository<FinishingUpdateGrid>
    {
        public FinishingManager(DbContext context) : base(context) { }
        public List<FinishingUpdateGrid> GetFinishingUpdateGridList()
        {
            var lst = this.GetAll().ToList();
            return lst;
        }
        public FinishingUpdateGrid GetFinishingMasterById(int Id)
        {
            using (DataContext context = new DataContext())
            {
                return context.FinishingUpdateGrid.Where(d => d.FinishedID == Id).FirstOrDefault();
                //return this.GetById(Id);
            }
        }
        public string SaveFinishingMaster(FinishingUpdateGrid objFinishingMaster)
        {
            using (var context = new CommonRepository<FinishingUpdateGrid>(this.DbContext))
            {
                if (objFinishingMaster.FinishedID == 0)
                {
                    context.Add(objFinishingMaster);
                    return "success";
                }
                else
                {
                    context.Update(objFinishingMaster);
                    return "success";
                }
            }
        }
        public void DeleteFinishingMaster(int Id)
        {
            this.Delete(Id);
        }


        public decimal GetTotalFinishingGrid(int Id)
        {
            using (DataContext context = new DataContext())
            {
                var res = context.FinishingUpdateGrid.Where(d => d.PlannerID == Id);
                return res.Any() ? res.Sum(s => s.FinishedQty) : 0;
            }
        }
    }



    public class ProductListManager : CommonRepository<Product>
    {
        public ProductListManager(DbContext context) : base(context) { }
        public DataTable GetDtProduct()
        {
            return GetDataTable("select PrdID,Productname from Productname order by Productname");
        }
    }
    public class ProcessListManager : CommonRepository<Processname>
    {
        public ProcessListManager(DbContext context) : base(context) { }
        public DataTable GetDtProcess()
        {
            return GetDataTable("select ProcID,Processname,UserID from Processname order by Processname");
        }
    }
    public class BuyerOrderNumberListManager : CommonRepository<BuyerOrderNumber>
    {
        public BuyerOrderNumberListManager(DbContext context) : base(context) { }
        public DataTable GetDtBuyerOrderNumber()
        {
            return GetDataTable("select BuyerOrderNumberID,BuyerOrderNumberName from BuyerOrderNumber order by BuyerOrderNumberName");
        }
        public string GetPINumberByBuyerOrderNumber(string BONName)
        {
            using (DataContext context = new DataContext())
            {
                string PINumber = "";
                var res = context.BuyerOrderNumber.FirstOrDefault(d => d.BuyerOrderNumberName == BONName);
                if(res != null)
                {
                    PINumber = res.PINumber;
                }
                return PINumber;

            }
        }
    }
    public class SupplierListManager : CommonRepository<Supplier>
    {
        public SupplierListManager(DbContext context) : base(context) { }
        public DataTable GetDtSupplier()
        {
            return GetDataTable("select SupID,Suppliername from Suppliername order by Suppliername");
        }
    }

    public class BuyerListManager : CommonRepository<Buyername>
    {
        public BuyerListManager(DbContext context) : base(context) { }
        public DataTable GetDtBuyer()
        {
            return GetDataTable("select ByrID,Buyername from Buyername order by Buyername");
        }
    }
    public class UnitListManager : CommonRepository<Unit>
    {
        public UnitListManager(DbContext context) : base(context) { }
        public DataTable GetDtUnit()
        {
            return GetDataTable("select UnitID,Unitname from Unit order by Unitname");
        }
    }
    public class SetNoteListManager : CommonRepository<SetNote>
    {
        public SetNoteListManager(DbContext context) : base(context) { }
        public DataTable GetDtSetNote()
        {
            return GetDataTable("select SetID,Setnotename from SetNote order by Setnotename");
        }
    }


    public class DynamicList_Model
    {
        public List<SqlParameter> SqlParameterList { get; set; }
        public string[] ColumnList { get; set; }
        public string[] ActionList { get; set; }
        public List<dynamic> dynamicList { get; set; }
        public IPagedList dynamicListMetaData { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
        public int totalCount { get; set; }
        public string sortOrder { get; set; }
        public string fieldName { get; set; }

        public string Controller { get; set; }
        public string ControllerAction { get; set; }
        public string UpdateTargetId { get; set; }
    }
}