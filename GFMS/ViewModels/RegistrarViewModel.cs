using GFMS.Commands;
using GFMS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GFMS.ViewModels
{
    public class RegistrarViewModel : ViewModelBase
    {
        public RegistrarViewModel()
        {

        }

        public ICommand LogoutCommand { get; }
    }
}
