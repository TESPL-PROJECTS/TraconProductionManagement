using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageRoles.Repository
{
    public class ProductUpdateGrid
    {
        [Key]
        public int ProductID { get; set; }
        public string Buyername { get; set; }
        public string BuyerOrderNumberName { get; set; }
        public string Processname { get; set; }
        public string Suppliername { get; set; }
        public string Productname { get; set; }
        public string ArticleNumber { get; set; }
        public string DesignNumber { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? SPCDate { get; set; }
        public string Size { get; set; }
        public string Colour { get; set; }
        public string CountConstruction { get; set; }
        public string Unit { get; set; }
        public string SetNote { get; set; }
        public decimal ProductQty { get; set; }
        public decimal FinishedQty { get; set; }
        public decimal BalanceQty { get; set; }
        public decimal PlannedQty { get; set; }
        public string Remarks { get; set; }       
        public string StoryName { get; set; }
        public string Status { get; set; }
        public int? OPM_ID { get; set; }
        public string PINumber { get; set; }
        public string ProcessMethod { get; set; }
        public DateTime? SKUStartDate { get; set; }
        public DateTime? SKUEndDate { get; set; }
    }
    public class PlannerUpdateGrid
    {
        [Key]
        public int PlannerID { get; set; }
        public int ProductID { get; set; }
        public DateTime? PlanningDate { get; set; }
        [NotMapped]
        public string PlanningDate_Display { get; set; }
        public decimal PlannedQty { get; set; }
        public decimal FinishedQty { get; set; }
        public string Remarks { get; set; }
    }
    public class FinishingUpdateGrid
    {
        [Key]
        public int FinishedID { get; set; }
        public int PlannerID { get; set; }
        public DateTime? FinishingDate { get; set; }
        [NotMapped]
        public string FinishingDate_Display { get; set; }
        public decimal FinishedQty { get; set; }
        public string Remarks { get; set; }
        [NotMapped]
        public int ProductID { get; set; }
    }

    public class Processname
    {
        [Key]
        public int ProcID { get; set; }

        [Column("Processname")]
        public string ProcessName { get; set; }
        public int? UserID { get; set;}
    }
     public class Product
    {
        [Key]
        public int PrdID { get; set; }
        public string Productname { get; set; }
    }
    public class SetNote
    {
        [Key]
        public int SetID { get; set; }
        public string Setnotename { get; set; }
    }
    public class Supplier
    {
        [Key]
        public int SupID { get; set; }

        [Column("Suppliername")]
        public string Suppliername { get; set; }
    }
    public class Unit
    {
        [Key]
        public int UnitID { get; set; }
        public string Unitname { get; set; }
    }
    public class BuyerOrderNumber
    {
        [Key]
        public int BuyerOrderNumberID { get; set; }
        public string BuyerOrderNumberName { get; set; }
        public string PINumber { get; set; }
    }
    

}
