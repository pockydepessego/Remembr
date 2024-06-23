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
    /// Interaction logic for HVLembretes.xaml
    /// </summary>
    public partial class HVLembretes : UserControl
    {
        public int prio;
        public HVLembretes()
        {
            InitializeComponent();
            MessageBox.Show("inicializado sem prio");

        }

        public HVLembretes(int priov)
        {
            InitializeComponent();
            prio = priov;

            if (priov >= 400)
            {
                LembreteAntecipacao.IsChecked = true;
                LembreteExecucao.IsChecked = true;
                LembreteAntecipacao.IsEnabled = false;
                LembreteExecucao.IsEnabled = false;
            } else
            {
                LembreteAntecipacao.IsChecked = false;
                LembreteExecucao.IsChecked = false;
                tsAntec.IsEnabled = false;
                tsExec.IsEnabled = false;

            }
        }

        private void LembreteAntecipacao_Checked(object sender, RoutedEventArgs e)
        {
            if (LembreteAntecipacao.IsChecked == true)
            {
                tsAntec.IsEnabled = true;
            }
            else
            {
                tsAntec.IsEnabled = false;
            }
        }

        private void LembreteExecucao_Checked(object sender, RoutedEventArgs e)
        {
            if (LembreteExecucao.IsChecked == true)
            {
                tsExec.IsEnabled = true;
            }
            else
            {
                tsExec.IsEnabled = false;
            }
        }
    }
}
