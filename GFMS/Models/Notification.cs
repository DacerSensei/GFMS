using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFMS.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string? User_Id { get; set; }
        public string? Message { get; set; }
        public string? Status { get; set; }
    }
}
