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
    /// Interaction logic for HVHAPesquisa.xaml
    /// </summary>
    public partial class HVHAPesquisa : UserControl
    {
        MainVM MVM;
        public HVHAPesquisa()
        {
            InitializeComponent();
            MVM = (MainVM)Application.Current.MainWindow.DataContext;


            var listaCC = new List<UserControl>();

            if (MVM.GTarefas == null)
            {
                MessageBox.Show("Erro tarefas");
                return;
            }

            var listaOrdenada = MVM.GTarefas.OrderBy(t => t.DataInicio).ThenByDescending(t => t.valorPrio).ThenBy(t => t.CreationTime).ToList();
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

        private void textNomedaTarefa_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pesquisa.Focus();
        }

        private void NomedaTarefa_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (!string.IsNullOrEmpty(pesquisa.Text) && pesquisa.Text.Length > 0)
            {
                textNomedaTarefa.Visibility = Visibility.Hidden;

            }
            else
            {
                textNomedaTarefa.Visibility = Visibility.Visible;
            }

            var listaCC = new List<UserControl>();

            if (MVM.GTarefas == null)
            {
                MessageBox.Show("Erro tarefas");
                return;
            }


            List<Models.Tarefa>? listaOrdenada = null;

            if (datainicio.SelectedDate == null && dataate.SelectedDate == null)
            {
                listaOrdenada = MVM.GTarefas.Where(t => t.Titulo.ToLower().Contains(pesquisa.Text.ToLower()) || (t.Descricao != null && t.Descricao.ToLower().Contains(pesquisa.Text.ToLower()))).OrderBy(t => t.DataInicio).ThenByDescending(t => t.valorPrio).ThenBy(t => t.CreationTime).ToList();
            } else if (datainicio.SelectedDate != null && dataate.SelectedDate == null)
            {
                listaOrdenada = MVM.GTarefas.Where(t => (t.Titulo.ToLower().Contains(pesquisa.Text.ToLower()) || 
                (t.Descricao != null && t.Descricao.ToLower().Contains(pesquisa.Text.ToLower()))) && t.DataInicio >= datainicio.SelectedDate).OrderBy(t => t.DataInicio).ThenByDescending(t => t.valorPrio).ThenBy(t => t.CreationTime).ToList();
            } else if (datainicio.SelectedDate == null && dataate.SelectedDate != null)
            {
                listaOrdenada = MVM.GTarefas.Where(t => (t.Titulo.ToLower().Contains(pesquisa.Text.ToLower()) || (t.Descricao != null && t.Descricao.ToLower().Contains(pesquisa.Text.ToLower()))) && t.DataInicio <= dataate.SelectedDate).OrderBy(t => t.DataInicio).ThenByDescending(t => t.valorPrio).ThenBy(t => t.CreationTime).ToList();
            } else if (datainicio.SelectedDate != null && dataate.SelectedDate != null)
            {
                listaOrdenada = MVM.GTarefas.Where(t => (t.Titulo.ToLower().Contains(pesquisa.Text.ToLower()) || (t.Descricao != null && t.Descricao.ToLower().Contains(pesquisa.Text.ToLower()))) && t.DataInicio >= datainicio.SelectedDate && t.DataInicio <= dataate.SelectedDate).OrderBy(t => t.DataInicio).ThenByDescending(t => t.valorPrio).ThenBy(t => t.CreationTime).ToList();
            }

            if (listaOrdenada == null)
            {
                return;
            }


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

        private void datainicio_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var listaCC = new List<UserControl>();

            if (MVM.GTarefas == null)
            {
                MessageBox.Show("Erro tarefas");
                return;
            }


            List<Models.Tarefa>? listaOrdenada = null;

            if (datainicio.SelectedDate == null && dataate.SelectedDate == null)
            {
                listaOrdenada = MVM.GTarefas.Where(t => t.Titulo.ToLower().Contains(pesquisa.Text.ToLower()) || (t.Descricao != null && t.Descricao.ToLower().Contains(pesquisa.Text.ToLower()))).OrderBy(t => t.DataInicio).ThenByDescending(t => t.valorPrio).ThenBy(t => t.CreationTime).ToList();
            }
            else if (datainicio.SelectedDate != null && dataate.SelectedDate == null)
            {
                listaOrdenada = MVM.GTarefas.Where(t => (t.Titulo.ToLower().Contains(pesquisa.Text.ToLower()) ||
                (t.Descricao != null && t.Descricao.ToLower().Contains(pesquisa.Text.ToLower()))) && t.DataInicio >= datainicio.SelectedDate).OrderBy(t => t.DataInicio).ThenByDescending(t => t.valorPrio).ThenBy(t => t.CreationTime).ToList();
            }
            else if (datainicio.SelectedDate == null && dataate.SelectedDate != null)
            {
                listaOrdenada = MVM.GTarefas.Where(t => (t.Titulo.ToLower().Contains(pesquisa.Text.ToLower()) || (t.Descricao != null && t.Descricao.ToLower().Contains(pesquisa.Text.ToLower()))) && t.DataInicio <= dataate.SelectedDate).OrderBy(t => t.DataInicio).ThenByDescending(t => t.valorPrio).ThenBy(t => t.CreationTime).ToList();
            }
            else if (datainicio.SelectedDate != null && dataate.SelectedDate != null)
            {
                listaOrdenada = MVM.GTarefas.Where(t => (t.Titulo.ToLower().Contains(pesquisa.Text.ToLower()) || (t.Descricao != null && t.Descricao.ToLower().Contains(pesquisa.Text.ToLower()))) && t.DataInicio >= datainicio.SelectedDate && t.DataInicio <= dataate.SelectedDate).OrderBy(t => t.DataInicio).ThenByDescending(t => t.valorPrio).ThenBy(t => t.CreationTime).ToList();
            }

            if (listaOrdenada == null)
            {
                return;
            }


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

        private void Reload()
        {
            var listaCC = new List<UserControl>();

            if (MVM.GTarefas == null)
            {
                MessageBox.Show("Erro tarefas");
                return;
            }


            List<Models.Tarefa>? listaOrdenada = null;

            if (datainicio.SelectedDate == null && dataate.SelectedDate == null)
            {
                listaOrdenada = MVM.GTarefas.Where(t => t.Titulo.ToLower().Contains(pesquisa.Text.ToLower()) || (t.Descricao != null && t.Descricao.ToLower().Contains(pesquisa.Text.ToLower()))).OrderBy(t => t.DataInicio).ThenByDescending(t => t.valorPrio).ThenBy(t => t.CreationTime).ToList();
            }
            else if (datainicio.SelectedDate != null && dataate.SelectedDate == null)
            {
                listaOrdenada = MVM.GTarefas.Where(t => (t.Titulo.ToLower().Contains(pesquisa.Text.ToLower()) ||
                (t.Descricao != null && t.Descricao.ToLower().Contains(pesquisa.Text.ToLower()))) && t.DataInicio >= datainicio.SelectedDate).OrderBy(t => t.DataInicio).ThenByDescending(t => t.valorPrio).ThenBy(t => t.CreationTime).ToList();
            }
            else if (datainicio.SelectedDate == null && dataate.SelectedDate != null)
            {
                listaOrdenada = MVM.GTarefas.Where(t => (t.Titulo.ToLower().Contains(pesquisa.Text.ToLower()) || (t.Descricao != null && t.Descricao.ToLower().Contains(pesquisa.Text.ToLower()))) && t.DataInicio <= dataate.SelectedDate).OrderBy(t => t.DataInicio).ThenByDescending(t => t.valorPrio).ThenBy(t => t.CreationTime).ToList();
            }
            else if (datainicio.SelectedDate != null && dataate.SelectedDate != null)
            {
                listaOrdenada = MVM.GTarefas.Where(t => (t.Titulo.ToLower().Contains(pesquisa.Text.ToLower()) || (t.Descricao != null && t.Descricao.ToLower().Contains(pesquisa.Text.ToLower()))) && t.DataInicio >= datainicio.SelectedDate && t.DataInicio <= dataate.SelectedDate).OrderBy(t => t.DataInicio).ThenByDescending(t => t.valorPrio).ThenBy(t => t.CreationTime).ToList();
            }

            if (listaOrdenada == null)
            {
                return;
            }


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
            Reload();
        }
    }
}
