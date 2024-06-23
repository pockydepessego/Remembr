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
using Syncfusion.Windows.Controls;

namespace Remembr.Views
{
    /// <summary>
    /// Interaction logic for HVPAnual.xaml
    /// </summary>
    public partial class HVPAnual : UserControl
    {
        public DateTime? dataInicial;
        SolidColorBrush? blackOutline = new BrushConverter().ConvertFromString("#3F3F3F") as SolidColorBrush;
        SolidColorBrush redOutline = Brushes.Red;

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

        public HVPAnual(DateTime dataInicial, Models.Periodicidade p)
        {
            InitializeComponent();
            this.dataInicial = dataInicial;

            diaxdey.Content = "No dia " + dataInicial.Day.ToString() + " de " + HVPeriodicidade.MonthPT(dataInicial.Month);
            zdiadey.Content = "N" + HVPeriodicidade.Gender((int)dataInicial.DayOfWeek) + " " + HVPeriodicidade.CardinalSemana(HVPeriodicidade.WOM(dataInicial), (int)dataInicial.DayOfWeek) + " " + HVPeriodicidade.DiaSemana(dataInicial.DayOfWeek) + " de " + HVPeriodicidade.MonthPT(dataInicial.Month);

            nAnos.Value = p.intervaloRepeticao;
            dataate.SelectedDate = p.DataLimite;

            switch (p.tipoAnual)
            {
                case 1:
                    diaxdey.IsChecked = true;
                    break;
                case 2:
                    zdiadey.IsChecked = true;
                    break;
                default:
                    break;
            }


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

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataInicial == null || dataate.SelectedDate == null)
            {
                return;
            }

            if (dataate.SelectedDate <= dataInicial)
            {
                MessageBox.Show("A data final da periodicidade deve ser depois da data da tarefa (" + dataInicial.ToDateTime().ToString("dd/MM/yyyy") + ").");
                datafinal.Stroke = redOutline;
            }
            else
            {
                datafinal.Stroke = blackOutline;
            }
        }
    }
}
