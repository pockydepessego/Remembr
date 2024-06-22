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
        public HVPDiario()
        {
            InitializeComponent();
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
    }
}
