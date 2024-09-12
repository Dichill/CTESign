using CTESign.Core;
using CTESign.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTESign.MVVM.ViewModel
{
    public class SubmittedViewModel : Core.ViewModel
    {
        private INavigationService _navigation;

        public INavigationService Navigation
        {
            get { return _navigation; }
            set { _navigation = value; OnPropertyChanged(); }
        }

        public RelayCommand SubmitOnLoaded { get; set; }

        public SubmittedViewModel(INavigationService navService) {
            Navigation = navService;

            SubmitOnLoaded = new RelayCommand(async o =>
            {
                await NavigateToAnotherViewModelAsync();
            }, canExecute => true);

        }

        public async Task NavigateToAnotherViewModelAsync()
        {
            await Task.Delay(TimeSpan.FromSeconds(2.5));
            Navigation.NavigateTo<AFKViewModel>();
        }
    }
}
