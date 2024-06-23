using Remembr.Models;
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
    /// Interaction logic for HVData.xaml
    /// </summary>
    public partial class HVData : UserControl
    {
        public HVData()
        {
            InitializeComponent();
            tpInicio.IsEnabled = false;
            tpFim.IsEnabled = false;

        }

        public HVData(Tarefa t)
        {
            InitializeComponent();

            if (t.FullDia)
            {
                CheckTodoDia.IsChecked = true;
                calendario.SelectedDate = t.DataInicio;
            }
            else
            {
                CheckTodoDia.IsChecked = false;
                calendario.SelectedDate = t.DataInicio;
                tpInicio.Value = t.DataInicio;
                tpFim.Value = t.DataFim;
            }

        }

        private void CheckTodoDia_Checked(object sender, RoutedEventArgs e)
        {

            if (CheckTodoDia.IsChecked == null || tpInicio == null || tpFim == null)
                return;


            if (CheckTodoDia.IsChecked.Value == true)
            {
                tpInicio.IsEnabled = false;
                tpInicio.Value = null;
                tpFim.IsEnabled = false;
                tpFim.Value = null;
            }
            else 
            {
                tpInicio.IsEnabled = true;
                tpFim.IsEnabled = true;
            }
        }
    }
}
