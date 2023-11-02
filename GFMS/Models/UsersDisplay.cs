using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFMS.Models
{
    public class UsersDisplay
    {
        public Users? User { get; set; }
        public string? UsersName
        {
            get
            {
                if (User != null)
                {
                    return $"{User.FirstName} {User.LastName}";
                }
                return "";
            }
        }
        public string? BanIcon
        {
            get
            {
                if (User != null)
                {
                    if (User.Status != null)
                    {
                        if (User.Status == 1)
                        {
                            return "Block";
                        }
                        else
                        {
                            return "Check";
                        }
                    }
                }
                return "";
            }
        }

        public string? BanColor
        {
            get
            {
                if (User != null)
                {
                    if (User.Status != null)
                    {
                        if (User.Status == 1)
                        {
                            return "#fe3839";
                        }
                        else
                        {
                            return "LimeGreen";
                        }
                    }
                }
                return "";
            }
        }

        public string? Status
        {
            get
            {
                if (User != null)
                {
                    if (User.Status != null)
                    {
                        if (User.Status == 1)
                        {
                            return "Activated";
                        }
                        else
                        {
                            return "Deactivated";
                        }
                    }
                }
                return "";
            }
        }

        public string? StatusColor
        {
            get
            {
                if (User != null)
                {
                    if (User.Status != null)
                    {
                        if (User.Status == 1)
                        {
                            return "LimeGreen";
                        }
                        else
                        {
                            return "#fe3839";
                        }
                    }
                }
                return "";
            }
        }
    }
}
