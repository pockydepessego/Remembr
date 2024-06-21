using Remembr.Views;
using Remembr.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace Remembr
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void CloseApp_button(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void HideApp_button(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }


        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void BigApp_button(object sender, MouseButtonEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
                this.BorderThickness = new System.Windows.Thickness(6);
                MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
                MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            }
            else
            {
                WindowState = WindowState.Normal;
                this.BorderThickness = new System.Windows.Thickness(2);
            }
        }


    }
}