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
        private int? _aug = null;
        private int? _sept = null;
        private int? _oct = null;
        private int? _nov = null;
        private int? _dec = null;
        private int? _jan = null;
        private int? _feb = null;
        private int? _mar = null;
        private int? _apr = null;
        private int? _may = null;
        private int? _jun = null;

        public string? AttendanceDescription { get; set; }

        public int? Aug
        {
            get => _aug;
            set
            {
                if (value == null || value.ToString() == string.Empty)
                {
                    _aug = null;
                }
                _aug = value;
                OnPropertyChanged(nameof(Aug));
                OnPropertyChanged(nameof(Total));
            }
        }
        public int? Sept
        {
            get => _sept;
            set
            {
                if (value == null || value.ToString() == string.Empty)
                {
                    _sept = null;
                }
                _sept = value;
                OnPropertyChanged(nameof(Sept));
                OnPropertyChanged(nameof(Total));
            }
        }
        public int? Oct
        {
            get => _oct;
            set
            {
                if (value == null || value.ToString() == string.Empty)
                {
                    _oct = null;
                }
                _oct = value;
                OnPropertyChanged(nameof(Oct));
                OnPropertyChanged(nameof(Total));
            }
        }
        public int? Nov
        {
            get => _nov;
            set
            {
                if (value == null || value.ToString() == string.Empty)
                {
                    _nov = null;
                }
                _nov = value;
                OnPropertyChanged(nameof(Nov));
                OnPropertyChanged(nameof(Total));
            }
        }
        public int? Dec
        {
            get => _dec;
            set
            {
                if (value == null || value.ToString() == string.Empty)
                {
                    _dec = null;
                }
                _dec = value;
                OnPropertyChanged(nameof(Dec));
                OnPropertyChanged(nameof(Total));
            }
        }
        public int? Jan
        {
            get => _jan;
            set
            {
                if (value == null || value.ToString() == string.Empty)
                {
                    _jan = null;
                }
                _jan = value;
                OnPropertyChanged(nameof(Jan));
                OnPropertyChanged(nameof(Total));
            }
        }
        public int? Feb
        {
            get => _feb;
            set
            {
                if (value == null || value.ToString() == string.Empty)
                {
                    _feb = null;
                }
                _feb = value;
                OnPropertyChanged(nameof(Feb));
                OnPropertyChanged(nameof(Total));
            }
        }
        public int? Mar
        {
            get => _mar;
            set
            {
                if (value == null || value.ToString() == string.Empty)
                {
                    _may = null;
                }
                _mar = value;
                OnPropertyChanged(nameof(Mar));
                OnPropertyChanged(nameof(Total));
            }
        }
        public int? Apr
        {
            get => _apr;
            set
            {
                if (value == null || value.ToString() == string.Empty)
                {
                    _apr = null;
                }
                _apr = value;
                OnPropertyChanged(nameof(Apr));
                OnPropertyChanged(nameof(Total));
            }
        }
        public int? May
        {
            get => _may;
            set
            {
                if (value == null || value.ToString() == string.Empty)
                {
                    _may = null;
                }
                _may = value;
                OnPropertyChanged(nameof(May));
                OnPropertyChanged(nameof(Total));
            }
        }
        public int? Jun
        {
            get => _jun;
            set
            {
                if (value == null || value.ToString() == string.Empty)
                {
                    _jun = null;
                }
                _jun = value;
                OnPropertyChanged(nameof(Jun));
                OnPropertyChanged(nameof(Total));
            }
        }

        public string? Total
        {
            get
            {
                if (Aug == null || Sept == null || Oct == null || Nov == null || Dec == null || Jan == null || Feb == null || Mar == null || Apr == null || May == null || Jun == null)
                {
                    return string.Empty;
                }
                return $"{Aug + Sept + Oct + Nov + Dec + Jan + Feb + Mar + Apr + May + Jun}";
            }
        }
    }
}
