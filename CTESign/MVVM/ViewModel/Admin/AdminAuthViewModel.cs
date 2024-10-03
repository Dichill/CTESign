using CTESign.Core;
using CTESign.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CTESign.MVVM.ViewModel
{
    public class AdminAuthViewModel : Core.ViewModel
    {
		// this will be hardcoded for now
		private string? _usernameTxt;

		public string? UsernameTxt
		{
			get { return _usernameTxt; }
			set { _usernameTxt = value; OnPropertyChanged(); }
		}

		private string? _passwordTxt;

		public string? Passwordtxt
		{
			get { return _passwordTxt; }
			set { _passwordTxt = value; OnPropertyChanged(); }
		}

        private INavigationService _navigation;

        public INavigationService Navigation
        {
            get { return _navigation; }
            set { _navigation = value; OnPropertyChanged(); }
        }

        public GlobalViewModel GlobalViewModel { get; } = GlobalViewModel.Instance;
        public RelayCommand LoginCommand { get; set; }

		public AdminAuthViewModel(INavigationService navService)
		{
			Navigation = navService;

			LoginCommand = new RelayCommand(o =>
			{

				if (UsernameTxt == "mesa" && Passwordtxt == "fahatgae")
				{
					GlobalViewModel.CurrentAdmin = "MESA";

				} else if (UsernameTxt == "cte" && Passwordtxt == "123")
				{
					GlobalViewModel.CurrentAdmin = "CTE";
				} else
				{
					MessageBox.Show("Invalid username and password. Only admins can login!", "Dash Software", MessageBoxButton.OK, MessageBoxImage.Warning);
					return;
				}

				Navigation.NavigateTo<AdminDashboardViewModel>();
			}, canExecute => true);
		}

    }
}
