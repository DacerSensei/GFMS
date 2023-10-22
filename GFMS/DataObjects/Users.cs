using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFMS.DataObjects
{
    public class Users
    {
        
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Username { get; set; }
        public int Id { get; set; }
        public string? Password { get; set; }
        public string? Usertype { get; set; }
        public int? Status { get; set; }
    }
}
