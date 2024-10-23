using CTESign.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTESign.Services;
using System.Reflection;
using System.Net.Http;
using System.Diagnostics;
using System.Windows;
using System.IO;
using CTESign.MVVM.Model;

namespace CTESign.MVVM.ViewModel
{
    public class MainViewModel : Core.ViewModel
    {
        private INavigationService _navigation;
        public INavigationService Navigation
        {
            get { return _navigation; }
            set { _navigation = value; OnPropertyChanged(); }
        }

        private bool _hasUpdate;

        public bool HasUpdate
        {
            get { return _hasUpdate; }
            set { _hasUpdate = value; OnPropertyChanged(); }
        }
        public GlobalViewModel GlobalViewModel { get; } = GlobalViewModel.Instance;

        public RelayCommand UpdateSoftwareCommand { get; set; }
        public RelayCommand SkipUpdateCommand { get; set; }
        public MainViewModel(INavigationService navService)
        {
            HasUpdate = false;
            Navigation = navService;
            var filePath = "config.json";
            GlobalViewModel.ConfigService = new JsonConfigService(filePath);

            CreateVersionFile();

            Navigation.NavigateTo<AFKViewModel>();
            UpdateSoftwareCommand = new RelayCommand(o =>
            {
                var updaterPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CTEUpdater.exe");

                if (System.IO.File.Exists(updaterPath))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = updaterPath,
                        UseShellExecute = true
                    });

                    Application.Current.Shutdown();
                }
                else
                {
                    MessageBox.Show("Updater not found!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }, canExecute => true);

            SkipUpdateCommand = new RelayCommand(o =>
            {
                HasUpdate = false;
            }, canExecute => true);

            CheckUpdate();
            FetchConfig();
        }


        async void GetConfigGlobal()
        {
            var config = await GlobalViewModel.ConfigService.LoadConfigAsync();

            GlobalViewModel.LogoImage = config.AFKLogoPath;
            GlobalViewModel.LogoSize = config.AFKLogoSize;
            GlobalViewModel.AfkTitle = config.AFKSignageText;

            if (config.Department == "")
            {
                GlobalViewModel.Department = null;
            } else
            {
                GlobalViewModel.Department = config.Department;
            }

        }
        async void FetchConfig()
        {
            if (GlobalViewModel.ConfigService.ConfigExists())
            {
                GetConfigGlobal();
            } else
            {
                var config = new ConfigModel
                {
                    Department = "",
                    IssueDate = DateTime.Now,
                    AFKLogoPath = "/Assets/logo.png",
                    AFKLogoSize = 400,
                    AFKSignageText = "CONFIGURE AT ADMIN PANEL",
                };

                await GlobalViewModel.ConfigService.SaveConfigAsync(config);

                // Load data once saved
                GetConfigGlobal();
            }
        }

        async void CheckUpdate()
        {
            string remoteVersion = await GetVersionFromGithubAsync("https://raw.githubusercontent.com/Dichill/CTESign/refs/heads/main/Dash/Version");

            if (IsUpdateAvailable(GetAssemblyVersion(), remoteVersion))
            {
                HasUpdate = true;
            }
        }

        public bool IsUpdateAvailable(string localVersion, string remoteVersion)
        {
            Version local = new Version(localVersion);
            Version remote = new Version(remoteVersion);

            // Compare the versions
            return remote > local;
        }

        public async Task<string> GetVersionFromGithubAsync(string versionUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string versionFromGithub = await client.GetStringAsync(versionUrl);
                    return versionFromGithub.Trim(); 
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Request error: {e.Message}");
                    return null;
                }
            }
        }

        public string GetAssemblyVersion()
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            return version.ToString();
        }

        public void CreateVersionFile()
        {
            string versionString = GetAssemblyVersion();

            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "VERSION");

            try
            {
                File.WriteAllText(filePath, versionString);
                Console.WriteLine($"Version file created successfully with version: {versionString}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating version file: {ex.Message}");
            }
        }
    }
}
