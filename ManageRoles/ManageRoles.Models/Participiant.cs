using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageRoles.Models
{
    [Table("Participiant")]
    public partial class Participiant
    {
        public int ID { get; set; }
        
        public string Name { get; set; }
    }
}
