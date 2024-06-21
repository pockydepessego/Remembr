using Remembr.Views;
using Remembr.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Remembr.ViewModels;


namespace Remembr
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainVM MVM;
        public MainWindow()
        {
            InitializeComponent();
            MVM = (MainVM)Application.Current.MainWindow.DataContext;
        }


        private void CloseApp_button(object sender, MouseButtonEventArgs e)
        {
            MVM.SavePerfil();
            Close();
        }

        private void HideApp_button(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }


        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void BigApp_button(object sender, MouseButtonEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
                BorderThickness = new Thickness(6);
                MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
                MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            }
            else
            {
                WindowState = WindowState.Normal;
                BorderThickness = new Thickness(2);
            }
        }


    }
}