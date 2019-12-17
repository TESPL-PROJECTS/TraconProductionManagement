using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageRoles.Models
{
    [Table("ProcessByUser")]
    public partial class ProcessByUser
    {
        public int ID { get; set; }

        public int? UserID { get; set; }

        public int? ProcessID { get; set; }
    }
}
