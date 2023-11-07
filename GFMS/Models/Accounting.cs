using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFMS.Models
{
    public class Accounting
    {
        public int Id { get; set; }
        public string?  Registration_Id { get; set; }
        public string? Payment { get; set; }
        public DateTime Created_At { get; set; }
    }
}
