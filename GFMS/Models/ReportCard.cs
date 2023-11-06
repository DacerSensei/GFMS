using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFMS.Models
{
    public class ReportCard
    {
        public string? Id { get; set; }
        public string? Registration_Id { get; set; }
        public string? Student_Level { get; set; }
        public string? Subjects { get; set; }
        public string? Behavior { get; set; }
        public string? Attendance { get; set; }
        public string? Narrative { get; set; }
    }
}
