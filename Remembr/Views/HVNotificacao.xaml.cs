using Remembr.Models;
using Remembr.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interaction logic for HVNotificacao.xaml
    /// </summary>
    public partial class HVNotificacao : UserControl
    {
        MainVM? MVM;
        Notificacao? notif;
        Tarefa? Tarefa;
        public HVNotificacao()
        {
            InitializeComponent();
            MessageBox.Show("iniciado sem notif");
        }

        public HVNotificacao(Models.Notificacao notif)
        {
            InitializeComponent();
            MVM = (MainVM)Application.Current.MainWindow.DataContext;
            this.notif = notif;

            if (MVM.GTarefas == null)
            {
                MessageBox.Show("Erro tarefas");
                return;
            }

            string? tipoalerta = null;

            if (notif.Tipo == 1)
            {
                tipoalerta = "antecipação";
            }
            else if (notif.Tipo == 2)
            {
                tipoalerta = "execução";
            }
            else 
            {
                tipoalerta = "aplicação";
            }

            if (notif.Lida)
            {
                marcarVisto.IsChecked = true;
            }
            else
            {
                marcarVisto.IsChecked = false;
            }

            nomeAlerta.Text = "Alerta de " + tipoalerta;

            Tarefa? t;
            try
            {
                t = MVM.GTarefas.Where(t => t.ID == notif.IDOriginal).First();
            } catch (Exception e)
            {
                // TAREFA JA NAO EXISTE
                griddesc.Children.Clear();
                grid3.Children.Clear();
                gr.RowDefinitions.Clear();
                gr.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                EditarTarefa.IsEnabled = false;
                EditarTarefa.Visibility = Visibility.Hidden;
                return;
            }

            Tarefa = t;

            if (t == null)
            {
                MessageBox.Show("Erro tarefa");
                return;
            }

            nomeAlerta.Text = "Alerta de "+ tipoalerta + ": " + t.Titulo;
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

            }
            else
            {
                prioridade.Text = "Prioridade " + t.valorPrio.ToString();
            }

            data.Text = notif.Data.ToString("dd/MM/yyyy");
            var prio = MVM.GPrioridades.FirstOrDefault(n => n.Valor == t.valorPrio);

            if (prio == null)
            {
                MessageBox.Show("erro prioridade");
                return;
            }
            border.BorderBrush = new BrushConverter().ConvertFromString(prio.Cor) as SolidColorBrush;


        }

        private void LembreteAntecipacao_Checked(object sender, RoutedEventArgs e)
        {
            if (MVM == null) return;

            if (MVM.GNotificacoes == null)
            {
                MessageBox.Show("Erro notificações");
                return;
            }

            if (notif == null)
            {
                MessageBox.Show("Erro notificação");
                return;
            }

            if (marcarVisto.IsChecked != null && marcarVisto.IsChecked.Value)
            {
                notif.Lida = true;
            }
            else
            {
                notif.Lida = false;
            }

        }

        private void EditarTarefa_Click(object sender, RoutedEventArgs e)
        {
            if (MVM == null) return;

            if (Tarefa == null)
            {
                MessageBox.Show("Erro tarefa");
                return;
            }

            MVM.editarTarefa(Tarefa.ID);

        }

        private void ApagarTarefa_Click(object sender, RoutedEventArgs e)
        {
            if (MVM == null) return;

            if (MVM.GNotificacoes == null)
            {
                MessageBox.Show("Erro notificações");
                return;
            }

            if (notif == null)
            {
                MessageBox.Show("Erro notificação");
                return;
            }

            MVM.GNotificacoes.Remove(notif);
            var pila = MVM.GNotificacoes;
            gr.Visibility = Visibility.Hidden;
            apagada.Visibility = Visibility.Visible;
        }
    }
}
