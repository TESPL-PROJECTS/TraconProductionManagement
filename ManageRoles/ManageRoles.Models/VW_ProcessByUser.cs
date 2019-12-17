using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageRoles.Models
{
    [Table("VW_ProcessByUser")]
    public partial class VW_ProcessByUser
    {
        [Key]
        public int UserID { get; set; }

        public string UserName { get; set; }
        public string ProcessID { get; set; }
        public string ProcessName { get; set; }
    }

    [Table("Productname")]
    public partial class Productname
    {
        [Key]
        public int PrdID { get; set; }
        [Column("Productname")]
        public string ProductName { get; set; }
    }

    


}
