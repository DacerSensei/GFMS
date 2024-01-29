using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GFMS.Models
{
    public class UsersNotification
    {
        public Users? User { get; set; }
        public Notification? Notification { get; set; }
        public Registration? Registration { get; set; }
        public Student? Student { get; set; }

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

        public string Message
        {
            get
            {
                if (Registration != null && Notification != null)
                {
                    if (Notification.Type != null)
                    {
                        return $"Request print for {Notification.Type}";
                    }
                }
                return "Unknown";
            }
        }

        public string StudentName
        {
            get
            {
                if (Student != null)
                {
                    return $"{Student.LastName} {Student.FirstName}";
                }
                return "Unknown";
            }
        }
    }
}
