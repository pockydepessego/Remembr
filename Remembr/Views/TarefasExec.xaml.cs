using Remembr.ViewModels;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Remembr.Views
{
    /// <summary>
    /// Interaction logic for TarefasExec.xaml
    /// </summary>
    public partial class TarefasExec : UserControl
    {
        MainVM MVM;
        public TarefasExec()
        {
            InitializeComponent();
            MVM = (MainVM)Application.Current.MainWindow.DataContext;
            if (MVM.gPerfil == null)
            {
                MessageBox.Show("Erro de perfil");
                App.Current.Shutdown();
                return;
            }
            pfp.ImageSource = MVM.gPerfil.Fotografia;

        }

        private void BigApp_button(object sender, MouseButtonEventArgs e)
        {

        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void CalendarPage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CloseApp_button(object sender, MouseButtonEventArgs e)
        {

        }

        private void HideApp_button(object sender, MouseButtonEventArgs e)
        {

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TarefaPage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Ellipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MVM.ChangeView("DefinicoesUtilizador");
        }
    }
}
