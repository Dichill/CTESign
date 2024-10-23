using CTESign.Services;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CTESign.MVVM.ViewModel
{
    public class AFKViewModel : Core.ViewModel
    {
        public GlobalViewModel GlobalViewModel { get; } = GlobalViewModel.Instance;

        private INavigationService _navigation;

        public INavigationService Navigation
        {
            get { return _navigation; }
            set { _navigation = value; OnPropertyChanged(); }
        }

        public AFKViewModel(INavigationService navigationService) { 
            Navigation = navigationService;
        }

        public void GoToSignIn()
        {
            if (GlobalViewModel.Department == null)
            {
                HandyControl.Controls.MessageBox.Show("Department is not currently set. Contact admin.", "Dash Software", MessageBoxButton.OK, MessageBoxImage.Hand);
                Navigation.NavigateTo<ErrorPageViewModel>();
                return;
            }

            switch(GlobalViewModel.Department)
            {
                case "CTE":
                    Navigation.NavigateTo<SignInViewModel>();
                    break;
                case "MESA":
                    Navigation.NavigateTo<MesaSignInViewModel>();
                    break;
            }

        }
    }
}
