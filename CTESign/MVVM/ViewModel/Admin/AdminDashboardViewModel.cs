using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CTESign.Core;

namespace CTESign.MVVM.ViewModel
{
    public class AdminDashboardViewModel : Core.ViewModel
    {
        public GlobalViewModel GlobalViewModel { get; } = GlobalViewModel.Instance;

        private string? _afkSignageText;

        public string? AFKSignageText
        {
            get { return _afkSignageText; }
            set { _afkSignageText = value; OnPropertyChanged(); }
        }

        public List<string> SoftwareImages
        {
            get
            {
                return new List<string>
                {
                    "/Assets/logo.png",
                    "/Assets/mesa.png",
                };
            }
        }

        private string? _selectedAfkLogo;
        public string? SelectedAFKLogo
        {
            get { return _selectedAfkLogo; }
            set { _selectedAfkLogo = value; OnPropertyChanged(); }
        }

        private int _afkLogoSize;

        public int AfkLogoSize
        {
            get { return _afkLogoSize; }
            set { _afkLogoSize = value; OnPropertyChanged(); }
        }


        public RelayCommand SaveChangesCommand { get; set; }

        public AdminDashboardViewModel()
        {
            SelectedAFKLogo = GlobalViewModel.LogoImage;
            AFKSignageText = GlobalViewModel.AfkTitle;
            AfkLogoSize = GlobalViewModel.LogoSize;

            SaveChangesCommand = new RelayCommand(async o =>
            {
                await GlobalViewModel.ConfigService.UpdateConfigAsync(afkLogoPath: SelectedAFKLogo, afkSignageText: AFKSignageText, afkLogoSize: AfkLogoSize);

                // Change this to be changed via the ConfigService
                GlobalViewModel.LogoImage = SelectedAFKLogo;
                GlobalViewModel.AfkTitle = AFKSignageText;
                GlobalViewModel.LogoSize = AfkLogoSize;

                HandyControl.Controls.MessageBox.Show("Changes has been saved", "Dash Software", MessageBoxButton.OK, MessageBoxImage.Information);
            }, canExecute => true);
        }
    }
}
