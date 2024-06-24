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
    /// Interaction logic for Avançado.xaml
    /// </summary>
    public partial class Avançado : UserControl
    {
        MainVM MVM;
        HVHAPesquisa? hVHAPesquisaV;
        HVHANotificacoes? hVHANotificacoesV;
        HVHAExportar? hVHAExportarV;
        public Avançado()
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

            hVHAPesquisaV = new HVHAPesquisa();
            hVHAPesquisaV.DataContext = new HVHAPesquisaVM();
            hcc.Content = hVHAPesquisaV;


        }


        private void Ellipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MVM.ChangeView("DefinicoesUtilizador");
        }


        private void BotaotarefastI_Click(object sender, RoutedEventArgs e)
        {
            hVHAPesquisaV = new HVHAPesquisa();
            hVHAPesquisaV.DataContext = new HVHAPesquisaVM();
            hcc.Content = hVHAPesquisaV;
        }

        private void BotaotarefastII_Click(object sender, RoutedEventArgs e)
        {
            HVHANotificacoes hVHANotificacoesV = new HVHANotificacoes();
            hVHANotificacoesV.DataContext = new HVHANotificacoesVM();
            hcc.Content = hVHANotificacoesV;
        }

        private void BotaotarefastIII_Click(object sender, RoutedEventArgs e)
        {
            hVHAExportarV = new HVHAExportar();
            hVHAExportarV.DataContext = new HVHAExportarVM();
            hcc.Content = hVHAExportarV;
        }
    }
}
