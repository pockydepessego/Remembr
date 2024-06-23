using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
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
using Syncfusion.UI.Xaml.Schedule;

namespace Remembr.Views
{
    /// <summary>
    /// Interaction logic for HVPeriodicidade.xaml
    /// </summary>
    public partial class HVPeriodicidade : UserControl
    {

        public DateTime? timeInicial;
        public int? perSelecionada;

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
            perSelecionada = 0;

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
            perSelecionada = 1;
        }

        private void BotaoSemanal_Checked(object sender, RoutedEventArgs e)
        {
            if (hvPSemanalV == null)
            {
                hvPSemanalV = new HVPSemanal();
                hvPSemanalV.DataContext = new HVPSemanalVM();
            }

            cc.Content = hvPSemanalV;
            perSelecionada = 2;
        }

        private void BotaoMensal_Checked(object sender, RoutedEventArgs e)
        {
            if (hvPMensalV == null || hvPMensalV.dataInicial != timeInicial)
            {
                if (cc == null) return;

                var dataInicial = timeInicial;
                if (dataInicial == null)
                    return;

                hvPMensalV = new HVPMensal((DateTime)dataInicial);
                hvPMensalV.DataContext = new HVPMensalVM();
            }
            cc.Content = hvPMensalV;
            perSelecionada = 3;
        }

        private void BotaoAnual_Checked(object sender, RoutedEventArgs e)
        {
            if (hvPAnualV == null || hvPAnualV.dataInicial != timeInicial)
            {
                if (cc == null) return;

                var dataInicial = timeInicial;
                if (dataInicial == null)
                    return;

                hvPAnualV = new HVPAnual((DateTime)dataInicial);
                hvPAnualV.DataContext = new HVPAnualVM();
            }
            cc.Content = hvPAnualV;
            perSelecionada = 4;
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
            perSelecionada = 0;
        }

        public static string MonthPT(int month)
        {
            switch (month)
            {
                case 1:
                    return "Janeiro";
                case 2:
                    return "Fevereiro";
                case 3:
                    return "Março";
                case 4:
                    return "Abril";
                case 5:
                    return "Maio";
                case 6:
                    return "Junho";
                case 7:
                    return "Julho";
                case 8:
                    return "Agosto";
                case 9:
                    return "Setembro";
                case 10:
                    return "Outubro";
                case 11:
                    return "Novembro";
                case 12:
                    return "Dezembro";
                default:
                    return "??";
            }
        }

        public static string DiaSemana(DayOfWeek dia)
        {
            switch (dia)
            {
                case DayOfWeek.Sunday:
                    return "domingo";
                case DayOfWeek.Monday:
                    return "segunda-feira";
                case DayOfWeek.Tuesday:
                    return "terça-feira";
                case DayOfWeek.Wednesday:
                    return "quarta-feira";
                case DayOfWeek.Thursday:
                    return "quinta-feira";
                case DayOfWeek.Friday:
                    return "sexta-feira";
                case DayOfWeek.Saturday:
                    return "sábado";
                default:
                    return "??";
            }
        }

        public static string Gender(int diasemana)
        {
            if (diasemana == 6 || diasemana == 0)
            {
                return "o";
            }
            else
            {
                return "a";
            }
        }
        public static string CardinalSemana(int dia, int diasemana)
        {
            string retur = dia.ToString();

            if (diasemana == 6 || diasemana == 0)
            {
                retur += "º";
            }
            else
            {
                retur += "ª";
            }

            return retur;
        }

        public static int WOM(DateTime date)
        {
            DateTime firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            int i, c = 0;

            for (i = 0; i <= DateTime.DaysInMonth(date.Year, date.Month) ; i++)
            {
                if (firstDayOfMonth.AddDays(i).DayOfWeek == date.DayOfWeek)
                {
                    c++;
                    if (firstDayOfMonth.AddDays(i).Day == date.Day)
                    {
                        return c;
                    }
                }
            }
            return -1;
        }

    }
}
