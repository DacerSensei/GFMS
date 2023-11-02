using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFMS.Models
{
    public class StudentReport
    {
        public Student? Student { get; set; }
        public Registration? Registration { get; set; }
        public ReportCard? ReportCard { get; set; }
        public string? StudentName
        {
            get
            {
                return $"{Student!.LastName} {Student!.FirstName}";
            }
        }
        public string? StatusColor
        {
            get
            {
                if (Registration != null && Registration.Level != null)
                {
                    if (Registration.Level.ToUpper() == "PRE SCHOOL")
                    {
                        return "#ff6773";
                    }
                    else if (Registration.Level.ToUpper() == "ELEMENTARY")
                    {
                        return "#6f68d2";
                    }
                    else if (Registration.Level.ToUpper() == "JUNIOR HIGH SCHOOL")
                    {
                        return "#0f84b9";
                    }
                    else if (Registration.Level.ToUpper() == "SENIOR HIGH SCHOOL")
                    {
                        return "#1abf32";
                    }
                }

                return "#000";
            }
        }
    }
}
