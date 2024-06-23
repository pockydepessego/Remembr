using Syncfusion.Windows.Controls;
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
    /// Interaction logic for HVPMensal.xaml
    /// </summary>
    public partial class HVPMensal : UserControl
    {
        public DateTime? dataInicial;
        SolidColorBrush? blackOutline = new BrushConverter().ConvertFromString("#3F3F3F") as SolidColorBrush;
        SolidColorBrush redOutline = Brushes.Red;

        public HVPMensal()
        {
            InitializeComponent();
            MessageBox.Show("iniciado sem data inicial");
        }

        public HVPMensal(DateTime dataInicial)
        {
            InitializeComponent();
            this.dataInicial = dataInicial;

            diax.Content = "No dia " + dataInicial.Day.ToString();
            zdia.Content = "N" + HVPeriodicidade.Gender((int)dataInicial.DayOfWeek) + " " + HVPeriodicidade.CardinalSemana(HVPeriodicidade.WOM(dataInicial), (int)dataInicial.DayOfWeek) + " " + HVPeriodicidade.DiaSemana(dataInicial.DayOfWeek);

            if (nMes.Value == 1)
            {
                plural.Text = "Mes";

                ate.Text = "Ocorre todas os meses até";
            }
            else
            {
                plural.Text = "Meses";
                ate.Text = "Ocorre de " + nMes.Value + " em " + nMes.Value + " meses até";
            }

        }

        private void nMes_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (nMes.Value == null || plural == null || ate == null)
            {
                return;
            }
            if (nMes.Value == 1)
            {
                plural.Text = "Mes";

                ate.Text = "Ocorre todos os meses até";
            }
            else
            {
                plural.Text = "Meses";
                ate.Text = "Ocorre de " + nMes.Value + " em " + nMes.Value + " meses até";
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

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
