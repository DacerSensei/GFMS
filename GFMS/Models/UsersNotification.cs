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

        public string FullNamee
        {
            get
            {
                return Notification.Message.Split("for ", StringSplitOptions.RemoveEmptyEntries)[1];
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
                        else if (Notification.Status.ToLower() == "approved")
                        {
                            return "LimeGreen";
                        }
                        else
                        {
                            return "#ff2147";
                        }
                    }
                }
                return "#000";
            }
        }
        public string? PrintVisible
        {
            get
            {
                if (Notification != null)
                {
                    if (Notification.Status != null)
                    {
                        if (Notification.Status.ToLower() == "pending")
                        {
                            return "Hidden";
                        }
                        else if (Notification.Status.ToLower() == "approved")
                        {
                            return "Visible";
                        }
                        else
                        {
                            return "Hidden";
                        }
                    }
                }
                return "#000";
            }
        }

        public string? CanModify
        {
            get
            {
                if (Notification != null)
                {
                    if (Notification.Status != null)
                    {
                        if (Notification.Status.ToLower() == "pending")
                        {
                            return "Visible";
                        }
                    }
                }
                return "Hidden";
            }
        }
    }
}
