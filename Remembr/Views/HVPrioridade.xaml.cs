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
using Remembr.ViewModels;

namespace Remembr.Views
{
    /// <summary>
    /// Interaction logic for HVPrioridade.xaml
    /// </summary>
    /// 
    public partial class HVPrioridade : UserControl
    {
        MainVM MVM;
        HVNovaPrioridade? HVNovaPrioridade;

        public HVPrioridade()
        {
            InitializeComponent();
            MVM = (MainVM)Application.Current.MainWindow.DataContext;
            if (HVNovaPrioridade == null)
            {
                HVNovaPrioridade = new HVNovaPrioridade();
                HVNovaPrioridade.DataContext = new HVNovaPrioridadeVM();
            }


        }

        private void Prioridadex_Click(object sender, RoutedEventArgs e)
        {
            pcc.Content = HVNovaPrioridade;
        }

        private void Prioridade400_Click(object sender, RoutedEventArgs e)
        {
            pcc.Content = null;
        }

        private void Prioridade300_Click(object sender, RoutedEventArgs e)
        {
            pcc.Content = null;
        }

        private void Prioridade200_Click(object sender, RoutedEventArgs e)
        {
            pcc.Content = null;
        }

        private void Prioridade100_Click(object sender, RoutedEventArgs e)
        {
            pcc.Content = null;
        }
    }
}
