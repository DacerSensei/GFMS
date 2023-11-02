using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFMS.Models
{
    public class Registration
    {
        public int Id { get; set; }
        public string? Student_Id { get; set; }
        public string? Registration_Date { get; set; }
        public string? Year { get; set; }
        public string? Level { get; set; }
        public string? Grade { get; set; }
        public string? Pic { get; set; }
        public string? Status { get; set; }
    }
}
