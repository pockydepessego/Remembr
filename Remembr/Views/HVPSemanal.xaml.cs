using Syncfusion.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
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
    /// Interaction logic for HVPSemanal.xaml
    /// </summary>
    public partial class HVPSemanal : UserControl
    {

        public DateTime? dataInicial;
        SolidColorBrush? blackOutline = new BrushConverter().ConvertFromString("#3F3F3F") as SolidColorBrush;
        SolidColorBrush redOutline = Brushes.Red;

        public HVPSemanal()
        {
            InitializeComponent();
            MessageBox.Show("inicializado sem data");
        }

        public HVPSemanal(DateTime dataR)
        {
            InitializeComponent();
            dataInicial = dataR;

            if (nSemanas.Value == 1)
            {
                plural.Text = "Semana";

                ate.Text = "Ocorre todas as semanas até";
            }
            else
            {
                plural.Text = "Semanas";
                ate.Text = "Ocorre de " + nSemanas.Value + " em " + nSemanas.Value + " semanas até";
            }

        }

        private bool[] lDias = [false, false, false, false, false, false, false];

        private void BotaoSemanal(object sender, RoutedEventArgs e)
        {
            if (SEGUNDA.IsChecked != null && SEGUNDA.IsChecked.Value)
            {
                lDias[0] = true;
            }
            else
            {
                lDias[0] = false;
            }

            if (TERCA.IsChecked != null && TERCA.IsChecked.Value)
            {
                lDias[1] = true;
            }
            else
            {
                lDias[1] = false;
            }

            if (QUARTA.IsChecked != null && QUARTA.IsChecked.Value)
            {
                lDias[2] = true;
            }
            else
            {
                lDias[2] = false;
            }

            if (QUINTA.IsChecked != null && QUINTA.IsChecked.Value)
            {
                lDias[3] = true;
            }
            else
            {
                lDias[3] = false;
            }

            if (SEXTA.IsChecked != null && SEXTA.IsChecked.Value)
            {
                lDias[4] = true;
            }
            else
            {
                lDias[4] = false;
            }

            if (SABADO.IsChecked != null && SABADO.IsChecked.Value)
            {
                lDias[5] = true;
            }
            else
            {
                lDias[5] = false;
            }

            if (DOMINGO.IsChecked != null && DOMINGO.IsChecked.Value)
            {
                lDias[6] = true;
            }
            else
            {
                lDias[6] = false;
            }

        }


        private void nSemanas_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (nSemanas.Value == null || plural == null || ate == null)
            {
                return;
            }
            if (nSemanas.Value == 1)
            {
                plural.Text = "Semana";

                ate.Text = "Ocorre todas as semanas até";
            }
            else
            {
                plural.Text = "Semanas";
                ate.Text = "Ocorre de " + nSemanas.Value + " em " + nSemanas.Value + " semanas até";
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
