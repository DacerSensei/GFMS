using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFMS.Models
{
    public class Requirement
    {
        public int Id { get; set; }
        public int Student_ID { get; set; }
        public string? Path { get; set; }
        public string? Description { get; set; }
    }
}
