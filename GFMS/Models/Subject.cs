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
                OnPropertyChanged(nameof(ThirdGrading));
                OnPropertyChanged(nameof(FinalRating));
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
                    FourthGrading = null;
                }
                fourthGrading = value;
                OnPropertyChanged(nameof(FourthGrading));
                OnPropertyChanged(nameof(FinalRating));

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
    }
}
