using GFMS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFMS.Models
{
    public class Narrative : ViewModelBase
    {
        private string? message = string.Empty;
        private string? signature = string.Empty;
        private string? date = string.Empty;

        public string? Title { get; set; }

        public string? Message
        {
            get
            {
                return message;
            }
            set
            {
                if (value == message)
                {
                    return;
                }

                message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        public string? Signature
        {
            get
            {
                return signature;
            }
            set
            {
                if (value == signature)
                {
                    return;
                }

                signature = value;
                OnPropertyChanged(nameof(Signature));
            }
        }

        public string? Date
        {
            get
            {
                return date;
            }
            set
            {
                if (value == date)
                {
                    return;
                }

                date = value;
                OnPropertyChanged(nameof(Date));
            }
        }
    }
}
