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
using Remembr.Views;

namespace Remembr.Views
{
    /// <summary>
    /// Interaction logic for HVPAnual.xaml
    /// </summary>
    public partial class HVPAnual : UserControl
    {
        public DateTime dataInicial;
        public HVPAnual()
        {
            InitializeComponent();
            MessageBox.Show("hvpanual iniciado sem data inical");
        }

        public HVPAnual(DateTime dataInicial)
        {
            InitializeComponent();
            this.dataInicial = dataInicial;
            
            diaxdey.Content = "No dia " + dataInicial.Day.ToString() + " de " + HVPeriodicidade.MonthPT(dataInicial.Month);
            zdiadey.Content = "N" + HVPeriodicidade.Gender((int)dataInicial.DayOfWeek) + " " + HVPeriodicidade.CardinalSemana(HVPeriodicidade.WOM(dataInicial), (int)dataInicial.DayOfWeek) + " " + HVPeriodicidade.DiaSemana(dataInicial.DayOfWeek) + " de " + HVPeriodicidade.MonthPT(dataInicial.Month);

            if (nAnos.Value == 1)
            {
                plural.Text = "Ano";

                ate.Text = "Ocorre todas os anos até";
            }
            else
            {
                plural.Text = "Anos";
                ate.Text = "Ocorre de " + nAnos.Value + " em " + nAnos.Value + " anos até";
            }

        }


        private void nAnos_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (nAnos.Value == null || plural == null || ate == null)
            {
                return;
            }
            if (nAnos.Value == 1)
            {
                plural.Text = "Ano";

                ate.Text = "Ocorre todos os anos até";
            }
            else
            {
                plural.Text = "Anos";
                ate.Text = "Ocorre de " + nAnos.Value + " em " + nAnos.Value + " anos até";
            }
        }
    }
}
