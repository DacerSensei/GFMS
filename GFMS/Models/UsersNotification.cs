using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFMS.Models
{
    public class UsersNotification
    {
        public Users? User { get; set; }
        public Notification? Notification { get; set; }

        public string FullName
        {
            get
            {
                return User == null ? "" : User.FirstName + " " + User.LastName;
            }
        }

        public string? StatusColor
        {
            get
            {
                if (Notification != null)
                {
                    if (Notification.Status != null)
                    {
                        if (Notification.Status.ToLower() == "pending")
                        {
                            return "#ffb302";
                        }
                        else if(Notification.Status.ToLower() == "approved")
                        {
                            return "LimeGreen";
                        }else
                        {
                            return "#fe3839";
                        }
                    }
                }
                return "#000";
            }
        }
    }
}
