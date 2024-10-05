using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTESign.Core;

namespace CTESign.MVVM.ViewModel
{
    public class AdminDashboardViewModel : Core.ViewModel
    {
        public GlobalViewModel GlobalViewModel { get; } = GlobalViewModel.Instance;
        public AdminDashboardViewModel()
        {
        }
    }
}
