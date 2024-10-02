using CTESign.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTESign.MVVM.ViewModel
{
    public class AdminViewModel : Core.ViewModel
    {
        private INavigationService _navigation;

        public INavigationService Navigation
        {
            get { return _navigation; }
            set { _navigation = value; OnPropertyChanged(); }
        }
        public AdminViewModel(INavigationService navService)
        {
            Navigation = navService;
            Navigation.NavigateTo<AdminAuthViewModel>();
        }
    }
}
