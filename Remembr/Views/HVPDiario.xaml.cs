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
    /// Interaction logic for HVPDiario.xaml
    /// </summary>
    public partial class HVPDiario : UserControl
    {
        public DateTime? dataInicial;
        SolidColorBrush? blackOutline = new BrushConverter().ConvertFromString("#3F3F3F") as SolidColorBrush;
        SolidColorBrush redOutline = Brushes.Red;

        public HVPDiario()
        {
            InitializeComponent();
            MessageBox.Show("iniciado sem data inicial");
        }

        public HVPDiario(DateTime dataInicial)
        {
            InitializeComponent();
            this.dataInicial = dataInicial;
        }

        public HVPDiario(DateTime dataInicial, Models.Periodicidade p)
        {
            InitializeComponent();
            this.dataInicial = dataInicial;
            nDias.Value = p.intervaloRepeticao;
            dataate.SelectedDate = p.DataLimite;
        }

        private void nDias_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (nDias.Value == null || plural == null || ate == null)
            {
                return;
            }
            if (nDias.Value == 1)
            {
                plural.Text = "Dia";
                ate.Text = "Ocorre todos os dias até";
            }
            else
            {
                plural.Text = "Dias";
                ate.Text = "Ocorre de " + nDias.Value + " em " + nDias.Value  + " dias até";
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
            } else
            {
                datafinal.Stroke = blackOutline;
            }
        }
    }
}
