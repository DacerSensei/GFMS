using GFMS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFMS.Models
{
    public class Attendance : ViewModelBase
    {
        private string? fourthGrading = string.Empty;
        private string? thirdGrading = string.Empty;
        private string? secondGrading = string.Empty;
        private string? firstGrading = string.Empty;

        public string? AttendanceDescription { get; set; }
    }
}
