using Remembr.Models;
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
    /// Interaction logic for HVPrioridade.xaml
    /// </summary>
    /// 
    public partial class HVPrioridade : UserControl
    {
        public HVNovaPrioridade? HVNovaPrioridade;
        public int? selectedPrio;

        public HVPrioridade()
        {
            InitializeComponent();
            if (HVNovaPrioridade == null)
            {
                HVNovaPrioridade = new HVNovaPrioridade();
                HVNovaPrioridade.DataContext = new HVNovaPrioridadeVM();
            }


        }

        public HVPrioridade(Tarefa tarefa)
        {
            InitializeComponent();
            if (HVNovaPrioridade == null)
            {
                HVNovaPrioridade = new HVNovaPrioridade(tarefa.valorPrio);
                HVNovaPrioridade.DataContext = new HVNovaPrioridadeVM();
            }

            if ((new List<int> { 100, 200, 300, 400 }.Contains(tarefa.valorPrio)))
            {
                switch (tarefa.valorPrio)
                {
                    case 100:
                        Prioridade100.IsChecked = true;
                        selectedPrio = 100;
                        break;
                    case 200:
                        Prioridade200.IsChecked = true;
                        selectedPrio = 200;
                        break;
                    case 300:
                        Prioridade300.IsChecked = true;
                        selectedPrio = 300;
                        break;
                    case 400:
                        Prioridade400.IsChecked = true;
                        selectedPrio = 400;
                        break;
                }
            }
            else
            {
                Prioridadex.IsChecked = true;
                selectedPrio = -100;

                pcc.Content = HVNovaPrioridade;
            }
        }

        private void Prioridadex_Click(object sender, RoutedEventArgs e)
        {
            pcc.Content = HVNovaPrioridade;
            selectedPrio = -100;
        }

        private void Prioridade400_Click(object sender, RoutedEventArgs e)
        {
            pcc.Content = new HVPrioritaria() {
                DataContext = new HVPrioritariaVM()
            };
            selectedPrio = 400;
        }

        private void Prioridade300_Click(object sender, RoutedEventArgs e)
        {
            pcc.Content = null;
            selectedPrio = 300;
        }

        private void Prioridade200_Click(object sender, RoutedEventArgs e)
        {
            pcc.Content = null;
            selectedPrio = 200;
        }

        private void Prioridade100_Click(object sender, RoutedEventArgs e)
        {
            pcc.Content = null;
            selectedPrio = 100;
        }

    }
}
