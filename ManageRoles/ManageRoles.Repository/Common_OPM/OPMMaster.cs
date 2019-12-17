using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageRoles.Repository
{
    [Table("OPM")]
    public partial class OPM
    {
        [Key]
        public int OPM_ID { get; set; }
        [StringLength(100)]
        public string BuyerName { get; set; }

        public string BuyerReferenceName { get; set; }

        [StringLength(100)]
        public string BuyerStoryName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? OID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartupLetterIssued { get; set; }

        public string Participants { get; set; }

        [Column(TypeName = "date")]
        public DateTime? KrrMeetingHeldOn { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PackingList { get; set; }

        [Column(TypeName = "date")]
        public DateTime? KarurDespatch { get; set; }
    }


    public class BuyerReference
    {
        [Key]
        public int BuyerID { get; set; }
        [StringLength(100)]
        public string BuyerReferenceName { get; set; }
        [StringLength(100)]
        public string StoryName { get; set; }
    }

    public class Buyername
    {
        [Key]
        public int ByrID { get; set; }
        [Column("Buyername")]
        [StringLength(100)]
        public string BuyerName { get; set; }       
    }

    [Table("Vendor")]
    public partial class Vendor
    {
        [Key]
        public int VP_ID { get; set; }

        public int? OPM_ID { get; set; }

        [StringLength(100)]
        public string BuyerOrderNumber { get; set; }

        [StringLength(100)]
        public string VendorName { get; set; }

        [StringLength(100)]
        public string Process { get; set; }

        [Column(TypeName = "date")]
        public DateTime? SPC1 { get; set; }

        [Column(TypeName = "date")]
        public DateTime? SPC2 { get; set; }

        [Column(TypeName = "date")]
        public DateTime? SPC3 { get; set; }

        [StringLength(100)]
        public string InChargeName { get; set; }
    }

    [Table("StyleInfo")]
    public partial class StyleInfo
    {
        [Key]
        public int Style_ID { get; set; }

        public int? OPM_ID { get; set; }
        public DateTime? AllPoIssuedDate { get; set; }

        [StringLength(255)]
        public string MasterSwatches { get; set; }

        [StringLength(255)]
        public string EngineeredDesignWeaving { get; set; }

        [StringLength(255)]
        public string EngineeredDesignPrint { get; set; }

        [StringLength(255)]
        public string EngineeredDesignSurface { get; set; }

        [StringLength(255)]
        public string EngineeredDesignStitching { get; set; }

        [StringLength(255)]
        public string SpecSheet { get; set; }
        public DateTime? QADate { get; set; }
        [StringLength(255)]
        public string StyleSample { get; set; }
        [StringLength(255)]
        public string Comments { get; set; }
    }
    [Table("Takka")]
    public partial class Takka
    {
        [Key]
        public int Takka_ID { get; set; }

        public int? OPM_ID { get; set; }

        [StringLength(100)]
        public string FirstTakka { get; set; }

        [StringLength(100)]
        public string TotalNo { get; set; }

        public DateTime? Date { get; set; }

        [StringLength(100)]
        public string TotalSKU { get; set; }

        public DateTime? SampleSubmission { get; set; }

        public DateTime? ReportReceipt { get; set; }

        public DateTime? ProductionSample { get; set; }

        public DateTime? CartonableLot { get; set; }
    }

    [Table("Target")]
    public partial class Target
    {
        [Key]
        public int Target_ID { get; set; }

        public int? OPM_ID { get; set; }

        public DateTime? WarpIssuance { get; set; }

        public DateTime? WeavingCompletion { get; set; }

        public DateTime? SKUSheet { get; set; }

        public DateTime? OrderAccessories { get; set; }

        public DateTime? DesignApproval { get; set; }

        public DateTime? FinishingCompletion { get; set; }

        public DateTime? ReceiptAccessories { get; set; }
    }

    [Table("QAInfo")]
    public partial class QAInfo
    {
        [Key]
        public int QAInfo_ID { get; set; }

        public int? OPM_ID { get; set; }

        public bool? ExternalAgencyInspection { get; set; }

        [StringLength(50)]
        public string SendRequestBeforeDay { get; set; }

        public DateTime? SendSampleForApproval { get; set; }

        public DateTime? SendSealerSample { get; set; }
    }

    [Table("QAInspection")]
    public partial class QAInspection
    {
        [Key]
        public int QAIns_ID { get; set; }

        public int? OPM_ID { get; set; }

        [StringLength(100)]
        public string TypeOfInspection { get; set; }

        public DateTime? RequestDate1 { get; set; }

        public DateTime? InspectionDate1 { get; set; }

        public DateTime? RequestDate2 { get; set; }

        public DateTime? InspectionDate2 { get; set; }

        public DateTime? InspectionDate3 { get; set; }

        public DateTime? RequestDate3 { get; set; }
    }

    [Table("PKGKDList")]
    public partial class PKGKDList
    {
        [Key]
        public int PkgDesID { get; set; }

        public int? OPM_ID { get; set; }

        [StringLength(100)]
        public string BuyerOrderName { get; set; }

        public string PINumber { get; set; }
        public string Productname { get; set; }

        [Column(TypeName = "date")]
        public DateTime? OPMPKGListDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? BuyerPKGListDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? OPMKDListDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? BuyerKDListDate { get; set; }

        public string Remarks { get; set; }
        
    }

    [Table("TypeOfInspection")]
    public partial class TypeOfInspection
    {
        [Key]
        public int InsID { get; set; }

        [StringLength(100)]
        public string InspectionType { get; set; }
    }
    #region Gantt
    public class Series
    {
        public string name { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string color { get; set; }
    }

    public class ProductGantt
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<Series> series { get; set; }
    }
    
    #endregion
}
