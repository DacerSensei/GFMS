using GFMS.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFMS.Models
{
    public class Subject : ViewModelBase
    {
        private int? fourthGrading = null;
        private int? thirdGrading = null;
        private int? secondGrading = null;
        private int? firstGrading = null;

        public string? SubjectName { get; set; }
        public int? FirstGrading
        {
            get
            {
                return firstGrading;
            }
            set
            {
                if (value == null || value.ToString() == string.Empty)
                {
                    firstGrading = null;
                }

                firstGrading = value;
                OnPropertyChanged(nameof(FirstGrading));
                OnPropertyChanged(nameof(FinalRating));
                OnPropertyChanged(nameof(Remarks));
                OnPropertyChanged(nameof(RemarksColor));
            }
        }
        public int? SecondGrading
        {
            get
            {
                return secondGrading;
            }
            set
            {
                if (value == null || value.ToString() == string.Empty)
                {
                    secondGrading = null;
                }
                secondGrading = value;
                OnPropertyChanged(nameof(SecondGrading));
                OnPropertyChanged(nameof(FinalRating));
                OnPropertyChanged(nameof(Remarks));
                OnPropertyChanged(nameof(RemarksColor));
            }
        }
        public int? ThirdGrading
        {
            get
            {
                return thirdGrading;
            }
            set
            {
                if (value == null || value.ToString() == string.Empty)
                {
                    thirdGrading = null;
                }
                thirdGrading = value;
                OnPropertyChanged(nameof(ThirdGrading));
                OnPropertyChanged(nameof(FinalRating));
                OnPropertyChanged(nameof(Remarks));
                OnPropertyChanged(nameof(RemarksColor));
            }
        }
        public int? FourthGrading
        {
            get
            {
                return fourthGrading;
            }
            set
            {
                if (value == null || value.ToString() == string.Empty)
                {
                    fourthGrading = null;
                }
                fourthGrading = value;
                OnPropertyChanged(nameof(FourthGrading));
                OnPropertyChanged(nameof(FinalRating));
                OnPropertyChanged(nameof(Remarks));
                OnPropertyChanged(nameof(RemarksColor));

            }
        }

        public string? FinalRating
        {
            get
            {
                if (FirstGrading == null || SecondGrading == null || ThirdGrading == null || FourthGrading == null)
                {
                    return string.Empty;
                }
                return $"{(FirstGrading + SecondGrading + ThirdGrading + FourthGrading) / 4}";
            }
        }

        public string? Remarks
        {
            get
            {
                if (FirstGrading == null || SecondGrading == null || ThirdGrading == null || FourthGrading == null)
                {
                    return string.Empty;
                }
                if ((FirstGrading + SecondGrading + ThirdGrading + FourthGrading) / 4 > 74)
                {
                    return "PASSED";
                }
                return "FAILED";
            }
        }

        public string? RemarksColor
        {
            get
            {
                if (FirstGrading == null || SecondGrading == null || ThirdGrading == null || FourthGrading == null)
                {
                    return "#c12d2b";
                }
                if ((FirstGrading + SecondGrading + ThirdGrading + FourthGrading) / 4 > 74)
                {
                    return "LimeGreen";
                }
                return "#c12d2b";
            }
        }
    }
}
