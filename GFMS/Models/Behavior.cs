using GFMS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFMS.Models
{
    public class Behavior : ViewModelBase
    {

        private string? fourthGrading = string.Empty;
        private string? thirdGrading = string.Empty;
        private string? secondGrading = string.Empty;
        private string? firstGrading = string.Empty;

        public string? BehaviorDescription { get; set; }
        public string? FirstGrading
        {
            get
            {
                return firstGrading;
            }
            set
            {
                if (value == firstGrading)
                {
                    return;
                }

                firstGrading = value;
                OnPropertyChanged(nameof(FirstGrading));
            }
        }
        public string? SecondGrading
        {
            get
            {
                return secondGrading;
            }
            set
            {
                if (value == secondGrading)
                {
                    return;
                }
                secondGrading = value;
                OnPropertyChanged(nameof(SecondGrading));
            }
        }
        public string? ThirdGrading
        {
            get
            {
                return thirdGrading;
            }
            set
            {
                if (value == thirdGrading)
                {
                    return;
                }
                thirdGrading = value;
                OnPropertyChanged(nameof(ThirdGrading));
            }
        }
        public string? FourthGrading
        {
            get
            {
                return fourthGrading;
            }
            set
            {
                if (value == fourthGrading)
                {
                    return;
                }
                fourthGrading = value;
                OnPropertyChanged(nameof(FourthGrading));
            }
        }
    }
}
