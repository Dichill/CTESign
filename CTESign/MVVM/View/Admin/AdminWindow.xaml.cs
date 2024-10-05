using CTESign.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CTESign.MVVM.View.Admin
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public GlobalViewModel GlobalViewModel { get; } = GlobalViewModel.Instance;

        public AdminWindow()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GlobalViewModel.InAdminMode = false;
            // Save Admin Configuration and Open the App Again


            var currentExecutablePath = Process.GetCurrentProcess().MainModule.FileName;
            Process.Start(currentExecutablePath);
            Application.Current.Shutdown();
        }
    }
}
