using ManageRoles.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;


namespace ManageRoles.Repository
{
    public class DataContext : DbContext
    {
        public DataContext() : base(CommonFunction.GetConnectionStringname())
        {
        }
        public DbSet<ProductUpdateGrid> ProductUpdateGrid { get; set; }
        public DbSet<PlannerUpdateGrid> PlannerUpdateGrid { get; set; }
        public DbSet<FinishingUpdateGrid> FinishingUpdateGrid { get; set; }
        public DbSet<Buyername> Buyer { get; set; }
        public DbSet<BuyerReference> BuyerReference { get; set; }
        public DbSet<BuyerOrderNumber> BuyerOrderNumber { get; set; }
        public DbSet<Supplier> Supplier { get; set; }
        public DbSet<Processname> Process { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<PKGKDList> PKGKDList { get; set; }



        public DbSet<Unit> Unit { get; set; }
        public DbSet<SetNote> SetNote { get; set; }

        public virtual DbSet<Vendor> Vendor { get; set; }
        public virtual DbSet<StyleInfo> StyleInfo { get; set; }
        public virtual DbSet<OPM> OPM { get; set; }
        public virtual DbSet<QAInfo> QAInfo { get; set; }
        public virtual DbSet<QAInspection> QAInspection { get; set; }
        public virtual DbSet<Takka> Takka { get; set; }
        public virtual DbSet<Target> Target { get; set; }
        public virtual DbSet<TypeOfInspection> TypeOfInspection { get; set; }
        public virtual DbSet<ProcessByUser> ProcessByUser { get; set; }
        public virtual DbSet<VW_ProcessByUser> VW_ProcessByUser { get; set; }
        public virtual DbSet<Participiant> Participiant { get; set; }
        public virtual DbSet<VW_ProductSummary> VW_ProductSummary { get; set; }
        public virtual DbSet<Productname> Productname { get; set; }
      
        public virtual DbSet<VW_FinalLineInspectionList> VW_FinalLineInspectionList { get; set; }
        public virtual DbSet<VW_InLineInspectionList> VW_InLineInspectionList { get; set; }
        public virtual DbSet<VW_MidLineInspectionList> VW_MidLineInspectionList { get; set; }
        public virtual DbSet<VW_OrderPackingList> VW_OrderPackingList { get; set; }
        public virtual DbSet<VW_OrdersDespatcheList> VW_OrdersDespatcheList { get; set; }
        public virtual DbSet<Usermaster> Usermaster { get; set; }
        public virtual DbSet<VW_ProductGroupChart> VW_ProductGroupChart { get; set; }
        public virtual DbSet<VW_ProdcutGroupGantt> VW_ProdcutGroupGantt { get; set; }
        public virtual DbSet<VW_DSProductUpdateGrid> VW_DSProductUpdateGrid { get; set; }
        public virtual DbSet<VW_BuyerOrderPackingList> VW_BuyerOrderPackingList { get; set; }
        public virtual DbSet<VW_BuyerOrderDespatchList> VW_BuyerOrderDespatchList { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public override int SaveChanges()
        {
            foreach (var changeSet in ChangeTracker.Entries())
            {
                var obj = changeSet.Entity;
            }
            return base.SaveChanges();
        }

    }
}