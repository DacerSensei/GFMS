using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace GFMS.Models
{
    public class StudentRequirements
    {
        public Student? Student { get; set; }
        public Registration? Registration { get; set; }
        public List<Requirement>? Requirement { get; set; }
        public string? StudentName
        {
            get
            {
                return $"{Student!.LastName} {Student!.FirstName}";
            }
        }
        public string? Status { get; set; }
        public string? StatusColor { get; set; }
    }
}
