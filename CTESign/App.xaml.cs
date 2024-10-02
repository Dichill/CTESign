using System.Configuration;
using System.Data;
using System.Windows;
using CTESign.Core;
using CTESign.MVVM.ViewModel;
using CTESign.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CTESign
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;
        public App()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<MainWindow>(provider => new MainWindow
            {
                DataContext = provider.GetRequiredService<MainViewModel>()
            });

            services.AddSingleton<MainViewModel>();
            services.AddSingleton<AFKViewModel>();
            services.AddSingleton<SignInViewModel>();
            services.AddSingleton<SubmittedViewModel>();
            services.AddSingleton<GlobalViewModel>();
            services.AddSingleton<INavigationService, NavigationService>();
            InstantiateGlobal();

            services.AddSingleton<Func<Type, ViewModel>>(serviceProvider =>
                viewModelType => (ViewModel)serviceProvider.GetRequiredService(viewModelType));

            _serviceProvider = services.BuildServiceProvider();
        }

        void InstantiateGlobal()
        {

        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }

}
