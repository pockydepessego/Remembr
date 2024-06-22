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
    /// Interaction logic for HomeTarefas.xaml
    /// </summary>
    public partial class HomeTarefas : UserControl
    {
        MainVM MVM;
        HVHTPorIniciar? hvHTPorIniciarV;
        HVHTExec? hvHTExecV;
        HVHTTerminadas? hvHTTerminadasV;
        HVHTApagadas? hVHTApagadasV;

        public HomeTarefas()
        {
            InitializeComponent();
            MVM = (MainVM)Application.Current.MainWindow.DataContext;
            if (MVM.GPerfil == null)
            {
                MessageBox.Show("Erro de perfil");
                App.Current.Shutdown();
                return;
            }
            pfp.ImageSource = MVM.GPerfil.Fotografia;


            if (hvHTPorIniciarV == null)
            {
                hvHTPorIniciarV = new HVHTPorIniciar();
                hvHTPorIniciarV.DataContext = new HVHTPorIniciarVM();

                hcc.Content = hvHTPorIniciarV;
            }


        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void CloseApp_button(object sender, MouseButtonEventArgs e)
        {

        }

        private void BigApp_button(object sender, MouseButtonEventArgs e)
        {

        }

        private void HideApp_button(object sender, MouseButtonEventArgs e)
        {

        }

        private void CalendarPage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TarefaPage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Ellipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MVM.ChangeView("DefinicoesUtilizador");
        }

        private void BotaotarefastIV_Click(object sender, RoutedEventArgs e)
        {
            if (hVHTApagadasV == null)
            {
                hVHTApagadasV = new HVHTApagadas();
                hVHTApagadasV.DataContext = new HVHTApagadasVM();
            }
            hcc.Content = hVHTApagadasV;
        }

        private void BotaotarefastIII_Click(object sender, RoutedEventArgs e)
        {
            if (hvHTTerminadasV == null)
            {
                hvHTTerminadasV = new HVHTTerminadas();
                hvHTTerminadasV.DataContext = new HVHTTerminadasVM();
            }
            hcc.Content = hvHTTerminadasV;
        }

        private void BotaotarefastII_Click(object sender, RoutedEventArgs e)
        {
            if (hvHTExecV == null)
            {
                hvHTExecV = new HVHTExec();
                hvHTExecV.DataContext = new HVHTExecVM();
            }
            hcc.Content = hvHTExecV;
        }

        private void BotaotarefastI_Click(object sender, RoutedEventArgs e)
        {
            if (hvHTPorIniciarV == null)
            {
                hvHTPorIniciarV = new HVHTPorIniciar();
                hvHTPorIniciarV.DataContext = new HVHTPorIniciarVM();
            }
            hcc.Content = hvHTPorIniciarV;
        }
    }

}
