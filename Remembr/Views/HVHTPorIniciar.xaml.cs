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
using System.Windows.Input.Manipulations;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Remembr.Views
{
    /// <summary>
    /// Interaction logic for HVHTPorIniciar.xaml
    /// </summary>
    public partial class HVHTPorIniciar : UserControl
    {
        MainVM MVM;
        public HVHTPorIniciar()
        {
            InitializeComponent();
            MVM = (MainVM)Application.Current.MainWindow.DataContext;


            var listaCC = new List<UserControl>();

            if (MVM.GTarefas == null) {
                MessageBox.Show("Erro tarefas");
                return;
            }

            var listaOrdenada = MVM.GTarefas.Where(t => t.Estado == 0).OrderBy(t => t.DataInicio).ThenBy(t => t.CreationTime).ToList();

            foreach (var tarefa in listaOrdenada)
            {
                var cc = new HVTarefa(tarefa);
                cc.DataContext = tarefa;
                listaCC.Add(cc);
                if (tarefa.Descricao == null)
                {
                    cc.Height = 100;
                } else
                {
                    cc.Height = 150;
                }
            }
            
            listinha.ItemsSource = listaCC;

        }

        private void listinha_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("clicou");
            var listaCC = new List<UserControl>();

            if (MVM.GTarefas == null)
            {
                MessageBox.Show("Erro tarefas");
                return;
            }

            var listaOrdenada = MVM.GTarefas.Where(t => t.Estado == 0).OrderBy(t => t.DataInicio).ThenBy(t => t.CreationTime).ToList();

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
    }
}
