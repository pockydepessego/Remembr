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
    /// Interaction logic for HVHTExec.xaml
    /// </summary>
    public partial class HVHTExec : UserControl
    {
        MainVM MVM;
        public HVHTExec()
        {
            MVM = (MainVM)Application.Current.MainWindow.DataContext;
            InitializeComponent();

            var listaCC = new List<UserControl>();

            if (MVM.GTarefas == null)
            {
                MessageBox.Show("Erro tarefas");
                return;
            }

            var listaOrdenada = MVM.GTarefas.Where(t => t.Estado == 1).OrderBy(t => t.DataInicio).ThenByDescending(t => t.valorPrio).ThenBy(t => t.CreationTime).ToList();

            foreach (var tarefa in listaOrdenada)
            {
                var cc = new HVTarefa(tarefa);
                cc.DataContext = tarefa;
                listaCC.Add(cc);
                if (tarefa.Descricao == null)
                {
                    cc.Height = 100;
                }
                else
                {
                    cc.Height = 150;
                }
            }

            listinha.ItemsSource = listaCC;

        }

        private void reload()
        {
            var listaCC = new List<UserControl>();

            if (MVM.GTarefas == null)
            {
                MessageBox.Show("Erro tarefas");
                return;
            }

            var listaOrdenada = MVM.GTarefas.Where(t => t.Estado == 1).OrderBy(t => t.DataInicio).ThenByDescending(t => t.valorPrio).ThenBy(t => t.CreationTime).ToList();

            foreach (var tarefa in listaOrdenada)
            {
                var cc = new HVTarefa(tarefa);
                cc.DataContext = tarefa;
                listaCC.Add(cc);
                if (tarefa.Descricao == null)
                {
                    cc.Height = 100;
                }
                else
                {
                    cc.Height = 150;
                }
            }

            listinha.ItemsSource = listaCC;
        }

        private async void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = false;
            await Task.Delay(500);
            reload();
        }
    }
}

