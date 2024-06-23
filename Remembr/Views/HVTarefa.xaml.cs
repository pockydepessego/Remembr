using Remembr.Models;
using Remembr.ViewModels;
using Syncfusion.UI.Xaml.CellGrid.Styles;
using Syncfusion.UI.Xaml.Diagram;
using Syncfusion.Windows.Controls;
using Syncfusion.Windows.Tools.Controls;
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
    /// Interaction logic for HVTarefa.xaml
    /// </summary>
    public partial class HVTarefa : UserControl
    {
        MainVM? MVM;
        public HVTarefa()
        {
            InitializeComponent();
            MessageBox.Show("hvtarefa chamado sem tarefa");
        }

        Tarefa? task;
        public HVTarefa(Tarefa t)
        {
            InitializeComponent();
            MVM = (MainVM)Application.Current.MainWindow.DataContext;
            task = t;

            nomeTarefa.Text = t.Titulo;

            if (t.FullDia)
            {
                data.Text = t.DataInicio.ToString("dd/MM/yyyy");
            } else if (t.DataFim == null)
            {
                data.Text = t.DataInicio.ToString("dd/MM/yyyy") + " às " + t.DataInicio.ToString("HH:mm");
            } else
            {
                data.Text = t.DataInicio.ToString("dd/MM/yyyy") + " das " + t.DataInicio.ToString("HH:mm") + " até às " + ((DateTime)t.DataFim).ToString("HH:mm");
            }


            if (new List<int> { 100, 200, 300, 400 }.Contains(t.valorPrio))
            {
                switch (t.valorPrio)
                {
                    case 100:
                        prioridade.Text = "Sem Importância";
                        break;
                    case 200:
                        prioridade.Text = "Pouco Importante";
                        break;
                    case 300:
                        prioridade.Text = "Importante";
                        break;
                    case 400:
                        prioridade.Text = "Prioritária";
                        break;
                }

            } else
            {
                prioridade.Text = "Prioridade " + t.valorPrio.ToString();
            }


            if (t.Descricao == null)
            {
                griddesc.Children.Clear();
                gr.RowDefinitions.Clear();
                gr.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                gr.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(10, GridUnitType.Star) });
                gr.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(10, GridUnitType.Star) });
                gr.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                Grid.SetRow(grid3, 2);
            }
            else
            {
                desc.Text = t.Descricao;
            }



            if (MVM.GPrioridades == null)
            {
                MessageBox.Show("erro prioridades");
                return;
            }
            var prio = MVM.GPrioridades.FirstOrDefault(n => n.Valor == t.valorPrio);

            if (prio == null)
            {
                MessageBox.Show("erro prioridade");
                return;
            }
            border.BorderBrush = new BrushConverter().ConvertFromString(prio.Cor) as SolidColorBrush;


        }

        private void ApagarTarefa_Click(object sender, RoutedEventArgs e)
        {
            if (MVM == null)
            {
                MessageBox.Show("erro MVM");
                return;
            }
            if (MVM.GTarefas == null)
            {
                MessageBox.Show("erro tarefas");
                return;
            }

            if (task == null)
            {
                MessageBox.Show("erro tarefa");
                return;
            }

            if (task.Estado != -1)
            {
                task.Estado = -1;
            } else {
                MVM.GTarefas.Remove(task);
            }
            MVM.SaveTarefas();
            gr.Visibility = Visibility.Hidden;
            apagada.Visibility = Visibility.Visible;
        }

        private void EditarTarefa_Click(object sender, RoutedEventArgs e)
        {
            if (MVM == null)
            {
                MessageBox.Show("erro MVM");
                return;
            }
            if (MVM.GTarefas == null)
            {
                MessageBox.Show("erro tarefas");
                return;
            }
            if (task == null)
            {
                MessageBox.Show("erro tarefa");
                return;
            }

            MVM.editarTarefa(task.ID);
        }
    }
}
