using CTESign.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTESign.MVVM.ViewModel
{
    public class AFKViewModel : Core.ViewModel
    {
        private INavigationService _navigation;

        public INavigationService Navigation
        {
            get { return _navigation; }
            set { _navigation = value; OnPropertyChanged(); }
        }

        public AFKViewModel(INavigationService navigationService) { 
            Navigation = navigationService;
        }

        public void GoToSignIn() => Navigation.NavigateTo<SignInViewModel>();
    }
}
