﻿using CTESign.MVVM.View.Admin;
using CTESign.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CTESign.MVVM.View
{
    /// <summary>
    /// Interaction logic for AFKView.xaml
    /// </summary>
    public partial class AFKView : UserControl
    {
        private AFKViewModel viewModel;
        public GlobalViewModel GlobalViewModel { get; } = GlobalViewModel.Instance;

        public AFKView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
            var window = Window.GetWindow(this);

            if (window != null && !GlobalViewModel.InAdminMode)
            {
                viewModel = (AFKViewModel)this.DataContext;
                window.KeyDown += HandleKeyPress;
            }
            else
                viewModel = null;

        }

        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            Storyboard myAnimation = (Storyboard)FindResource("FadeOutStoryboard");

            
            myAnimation.Completed += AnimCompleted;
            myAnimation.Begin();

            ((Control) sender).KeyDown -= HandleKeyPress;
        }

        private void AnimCompleted(object? sender, EventArgs e)
        {
            viewModel.GoToSignIn();
        }

        private void adminBttn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Disable key presses
            Window.GetWindow(this).KeyDown -= HandleKeyPress;
            ((App)Application.Current).ShowAdminWindow();
        }
    }
}
