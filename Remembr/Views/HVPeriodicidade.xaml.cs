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
using Remembr.ViewModels;

namespace Remembr.Views
{
    /// <summary>
    /// Interaction logic for HVPeriodicidade.xaml
    /// </summary>
    public partial class HVPeriodicidade : UserControl
    {

        public DateTime? timeInicial;
        HVPDiario? hvPDiarioV;
        HVPSemanal? hvPSemanalV;
        HVPMensal? hvPMensalV;
        HVPAnual? hvPAnualV;
        HVPDesativado? hvPDesativadoV;
        public HVPeriodicidade()
        {
            InitializeComponent();
            MessageBox.Show("view periodicidade iniciada sem datetime");

        }
        public HVPeriodicidade(DateTime dataInicial)
        {
            InitializeComponent();

            timeInicial = dataInicial;

            if (hvPDesativadoV == null)
            {
                hvPDesativadoV = new HVPDesativado(dataInicial);
                hvPDesativadoV.DataContext = new HVPDesativadoVM();
            }
            cc.Content = hvPDesativadoV;

        }


        private void BotaoDiario_Checked(object sender, RoutedEventArgs e)
        {
            if (hvPDiarioV == null)
            {
                hvPDiarioV = new HVPDiario();
                hvPDiarioV.DataContext = new HPVDiarioVM();
            }


            cc.Content = hvPDiarioV;
        }

        private void BotaoSemanal_Checked(object sender, RoutedEventArgs e)
        {
            if (hvPSemanalV == null)
            {
                hvPSemanalV = new HVPSemanal();
                hvPSemanalV.DataContext = new HVPSemanalVM();
            }

            cc.Content = hvPSemanalV;
        }

        private void BotaoMensal_Checked(object sender, RoutedEventArgs e)
        {
            if (hvPMensalV == null)
            {
                hvPMensalV = new HVPMensal();
                hvPMensalV.DataContext = new HVPMensalVM();
            }
            cc.Content = hvPMensalV;
        }

        private void BotaoAnual_Checked(object sender, RoutedEventArgs e)
        {
            if (hvPAnualV == null)
            {
                hvPAnualV = new HVPAnual();
                hvPAnualV.DataContext = new HVPAnualVM();
            }
            cc.Content = hvPAnualV;
        }

        private void BotaoDesativado_Checked(object sender, RoutedEventArgs e)
        {
            if (cc == null) return;

            var dataInicial = timeInicial;
            if ( dataInicial == null)
                return;

            if (hvPDesativadoV == null)
            {
                hvPDesativadoV = new HVPDesativado((DateTime)dataInicial);
                hvPDesativadoV.DataContext = new HVPDesativadoVM();
            }
            cc.Content = hvPDesativadoV;
        }
    }
}
