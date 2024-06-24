using Remembr.Models;
using Remembr.ViewModels;
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
    /// Interaction logic for EditarTarefa.xaml
    /// </summary>
    public partial class EditarTarefa : UserControl
    {
        MainVM MVM;
        HVLembretes? hvLembV;
        HVData? hVDataV;
        HVPeriodicidade? hvPerV;
        HVPrioridade? hvPriV;
        Tarefa? task;

        public EditarTarefa()
        {
            InitializeComponent();
            MVM = (MainVM)Application.Current.MainWindow.DataContext;
            var idTarefa = MVM.CurrentEditarTarefa;
            if (idTarefa == null)
            {
                MessageBox.Show("Erro tarefas");
                return;
            }
            if (MVM.GTarefas == null)
            {
                MessageBox.Show("Erro tarefas");
                return;
            }
            var tarefa = MVM.GTarefas.Where(t => t.ID == idTarefa).FirstOrDefault();
            if (tarefa == null)
            {
                MessageBox.Show("Erro tarefas");
                return;
            }

            task = tarefa;
            NomedaTarefa.Text = tarefa.Titulo;
            DescricaodaTarefa.Text = tarefa.Descricao;

            if (tarefa.Estado != -1)
            {
                estado.SelectedIndex = tarefa.Estado;
            } else
            {
                estado.SelectedIndex = 3;
                editar.Content = "Tarefa Apagada (mudar estado para recuperar)";
            }


            if (hVDataV == null)
            {
                hVDataV = new HVData(tarefa);
                hVDataV.DataContext = new HVDataVM();

                cc.Content = hVDataV;
            }

        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void cancelartarefa_Click(object sender, RoutedEventArgs e)
        {
            MVM.ChangeView("BACK");
            // MessageBox.Show(hvlView.LembreteAntecipacao.IsChecked.Value.ToString());

        }

        private void Data_Click(object sender, RoutedEventArgs e)
        {
            if (hVDataV == null)
            {
                if (task == null) return;
                hVDataV = new HVData(task);
                hVDataV.DataContext = new HVDataVM();
            }
            cc.Content = hVDataV;
        }


        private void Prioridade_Click(object sender, RoutedEventArgs e)
        {
            if (hvPriV == null)
            {
                if (task == null) return;
                hvPriV = new HVPrioridade(task);
                hvPriV.DataContext = new HVPrioridadeVM();
            }
            cc.Content = hvPriV;
        }

        int? priov = null;
        private void Lembretes_Click(object sender, RoutedEventArgs e)
        {
            if (hvPriV == null)
            {
                MessageBox.Show("É obrigatório escolher a prioridade antes de alterar as notificações");
                Prioridade.IsChecked = true;
                Notificacoes.IsChecked = false;
                return;
            }

            if (hvPriV.selectedPrio == null)
            {
                MessageBox.Show("É obrigatório escolher a prioridade antes de alterar as notificações");
                Prioridade.IsChecked = true;
                Notificacoes.IsChecked = false;
                return;
            }


            if (hvPriV.selectedPrio == -100)
            {
                if (hvPriV.HVNovaPrioridade == null)
                {
                    MessageBox.Show("É obrigatório escolher a prioridade antes de alterar as notificações");
                    Prioridade.IsChecked = true;
                    Notificacoes.IsChecked = false;
                    return;
                }
                else if (hvPriV.HVNovaPrioridade.valorp.Value == null && hvPriV.HVNovaPrioridade.combo.SelectedIndex == 0)
                {
                    MessageBox.Show("É obrigatório escolher a prioridade antes de alterar as notificações");
                    Prioridade.IsChecked = true;
                    Notificacoes.IsChecked = false;
                    return;
                }
                else if (hvPriV.HVNovaPrioridade.combo.SelectedIndex != 0)
                {
                    var s = hvPriV.HVNovaPrioridade.combo.SelectedItem.ToString();
                    if (s == null)
                    {
                        return;
                    }
                    priov = int.Parse(s);
                }
                else if (hvPriV.HVNovaPrioridade.valorp.Value != null)
                {
                    priov = (int)hvPriV.HVNovaPrioridade.valorp.Value;
                }
            }
            else
            {
                priov = (int)hvPriV.selectedPrio;
            }


            if (hvLembV == null || hvLembV.prio != priov || priov == null)
            {

                if (hvLembV != null)
                {
                    MessageBox.Show("A importância foi alterada, a configuração de notificações será resetada");
                }


                if (priov == null)
                {
                    MessageBox.Show("É obrigatório escolher a prioridade antes de alterar as notificações");
                    Prioridade.IsChecked = true;
                    Notificacoes.IsChecked = false;
                    return;
                }

                if (task == null) return;

                hvLembV = new HVLembretes((int)priov, task);
                hvLembV.DataContext = new HVLembretesVM();
            }
            cc.Content = hvLembV;
        }


        private void CriarTarefa_Click(object sender, RoutedEventArgs e)
        {

            Models.Tarefa? tTarefa = null;

            string? tNome = null;
            string? tDesc = null;

            // NOME
            if (string.IsNullOrEmpty(NomedaTarefa.Text))
            {
                MessageBox.Show("É obrigatório preencher o nome da tarefa");
                NomedaTarefa.Focus();
                return;
            }

            tNome = NomedaTarefa.Text;

            if (!string.IsNullOrEmpty(DescricaodaTarefa.Text))
            {
                tDesc = DescricaodaTarefa.Text;
            }

            // DATA
            if (hVDataV == null || hVDataV.calendario.SelectedDate == null)
            {
                MessageBox.Show("É obrigatório preencher a data da tarefa");
                Data.IsChecked = true;
                return;
            }

            // PERIODICIDADE 
            bool[]? diasSemana = null;
            DateTime? dataLimite = null;
            int? opM = null;
            int? opA = null;
            int? intervaloRep = null;

            if (hvPerV != null && hvPerV.perSelecionada != 0)
            {
                switch (hvPerV.perSelecionada)
                {
                    case 1:
                        if (hvPerV.hvPDiarioV == null)
                        {
                            MessageBox.Show("É obrigatório preencher a periodicidade da tarefa");
                            Periodicidade.IsChecked = true;
                            return;
                        }

                        if (hvPerV.hvPDiarioV.dataate.SelectedDate == null)
                        {
                            MessageBox.Show("É obrigatório preencher a data final da periodicidade");
                            Periodicidade.IsChecked = true;
                            return;
                        }

                        if (hvPerV.hvPDiarioV.dataate.SelectedDate <= hVDataV.calendario.SelectedDate)
                        {
                            MessageBox.Show("A data final da periodicidade deve ser depois da data da tarefa (" + hVDataV.calendario.SelectedDate.Value.ToString("dd/MM/yyyy") + ").");
                            Periodicidade.IsChecked = true;
                            return;
                        }

                        if (hvPerV.hvPDiarioV.nDias.Value == null)
                        {
                            MessageBox.Show("É obrigatório preencher o intervalo de dias da periodicidade");
                            Periodicidade.IsChecked = true;
                            return;
                        }
                        intervaloRep = (int)hvPerV.hvPDiarioV.nDias.Value;
                        dataLimite = hvPerV.hvPDiarioV.dataate.SelectedDate;

                        break;

                    case 2:
                        if (hvPerV.hvPSemanalV == null)
                        {
                            MessageBox.Show("É obrigatório preencher a periodicidade da tarefa");
                            Periodicidade.IsChecked = true;
                            return;
                        }

                        if (hvPerV.hvPSemanalV.dataate.SelectedDate == null)
                        {
                            MessageBox.Show("É obrigatório preencher a data final da periodicidade");
                            Periodicidade.IsChecked = true;
                            return;
                        }

                        if (hvPerV.hvPSemanalV.dataate.SelectedDate <= hVDataV.calendario.SelectedDate)
                        {
                            MessageBox.Show("A data final da periodicidade deve ser depois da data da tarefa (" + hVDataV.calendario.SelectedDate.Value.ToString("dd/MM/yyyy") + ").");
                            Periodicidade.IsChecked = true;
                            return;
                        }

                        if (hvPerV.hvPSemanalV.nSemanas.Value == null)
                        {
                            MessageBox.Show("É obrigatório preencher o intervalo de dias da periodicidade");
                            Periodicidade.IsChecked = true;
                            return;
                        }

                        if (hvPerV.hvPSemanalV.lDias.All(x => !x))
                        {
                            MessageBox.Show("É obrigatório preencher os dias da semana da periodicidade");
                            Periodicidade.IsChecked = true;
                            return;
                        }


                        intervaloRep = (int)hvPerV.hvPSemanalV.nSemanas.Value;
                        dataLimite = hvPerV.hvPSemanalV.dataate.SelectedDate;
                        diasSemana = hvPerV.hvPSemanalV.lDias;
                        break;

                    case 3:
                        if (hvPerV.hvPMensalV == null)
                        {
                            MessageBox.Show("É obrigatório preencher a periodicidade da tarefa");
                            Periodicidade.IsChecked = true;
                            return;
                        }

                        if (hvPerV.hvPMensalV.dataate.SelectedDate == null)
                        {
                            MessageBox.Show("É obrigatório preencher a data final da periodicidade");
                            Periodicidade.IsChecked = true;
                            return;
                        }

                        if (hvPerV.hvPMensalV.dataate.SelectedDate <= hVDataV.calendario.SelectedDate)
                        {
                            MessageBox.Show("A data final da periodicidade deve ser depois da data da tarefa (" + hVDataV.calendario.SelectedDate.Value.ToString("dd/MM/yyyy") + ").");
                            Periodicidade.IsChecked = true;
                            return;
                        }

                        if (hvPerV.hvPMensalV.nMes.Value == null)
                        {
                            MessageBox.Show("É obrigatório preencher o intervalo de dias da periodicidade");
                            Periodicidade.IsChecked = true;
                            return;
                        }
                        intervaloRep = (int)hvPerV.hvPMensalV.nMes.Value;
                        dataLimite = hvPerV.hvPMensalV.dataate.SelectedDate;

                        if (hvPerV.hvPMensalV.diax.IsChecked == true)
                        {
                            opM = 1;
                        }
                        else if (hvPerV.hvPMensalV.zdia.IsChecked == true)
                        {
                            opM = 2;
                        }
                        else
                        {
                            MessageBox.Show("É obrigatório preencher o tipo de periodicidade mensal");
                            Periodicidade.IsChecked = true;
                            return;
                        }
                        break;

                    case 4:
                        if (hvPerV.hvPAnualV == null)
                        {
                            MessageBox.Show("É obrigatório preencher a periodicidade da tarefa");
                            Periodicidade.IsChecked = true;
                            return;
                        }

                        if (hvPerV.hvPAnualV.dataate.SelectedDate == null)
                        {
                            MessageBox.Show("É obrigatório preencher a data final da periodicidade");
                            Periodicidade.IsChecked = true;
                            return;
                        }

                        if (hvPerV.hvPAnualV.dataate.SelectedDate <= hVDataV.calendario.SelectedDate)
                        {
                            MessageBox.Show("A data final da periodicidade deve ser depois da data da tarefa (" + hVDataV.calendario.SelectedDate.Value.ToString("dd/MM/yyyy") + ").");
                            Periodicidade.IsChecked = true;
                            return;
                        }

                        if (hvPerV.hvPAnualV.nAnos.Value == null)
                        {
                            MessageBox.Show("É obrigatório preencher o intervalo de dias da periodicidade");
                            Periodicidade.IsChecked = true;
                            return;
                        }
                        intervaloRep = (int)hvPerV.hvPAnualV.nAnos.Value;
                        dataLimite = hvPerV.hvPAnualV.dataate.SelectedDate;

                        if (hvPerV.hvPAnualV.diaxdey.IsChecked == true)
                        {
                            opA = 1;
                        }
                        else if (hvPerV.hvPAnualV.zdiadey.IsChecked == true)
                        {
                            opA = 2;
                        }
                        else
                        {
                            MessageBox.Show("É obrigatório preencher o tipo de periodicidade anual");
                            Periodicidade.IsChecked = true;
                            return;
                        }
                        break;

                    default:
                        MessageBox.Show("Erro periodicidade selecionada");
                        return;

                }

            }

            // PRIORIDADE
            if (hvPriV == null || hvPriV.selectedPrio == null)
            {
                MessageBox.Show("É obrigatório escolher a prioridade antes de guardar a tarefa");
                Prioridade.IsChecked = true;
                return;
            }

            if (hvPriV.selectedPrio == -100)
            {
                if (hvPriV.HVNovaPrioridade == null)
                {
                    MessageBox.Show("É obrigatório escolher a prioridade antes de guardar a tarefa");
                    Prioridade.IsChecked = true;
                    return;
                }
                else if (hvPriV.HVNovaPrioridade.valorp.Value == null && hvPriV.HVNovaPrioridade.combo.SelectedIndex == 0)
                {
                    MessageBox.Show("É obrigatório escolher a prioridade antes de guardar a tarefa");
                    Prioridade.IsChecked = true;
                    return;
                }
                else if (hvPriV.HVNovaPrioridade.combo.SelectedIndex != 0)
                {
                    var s = hvPriV.HVNovaPrioridade.combo.SelectedItem.ToString();
                    if (s == null)
                    {
                        return;
                    }
                    priov = int.Parse(s);
                }
                else if (hvPriV.HVNovaPrioridade.valorp.Value != null)
                {
                    priov = (int)hvPriV.HVNovaPrioridade.valorp.Value;

                    var cor = hvPriV.HVNovaPrioridade.colorp.SelectedColor;
                    if (cor == null)
                    {
                        MessageBox.Show("É obrigatório escolher uma cor para a nova prioridade");
                        Prioridade.IsChecked = true;
                        return;
                    }
                    var corS = cor.ToString();
                    if (corS == null)
                    {
                        return;
                    }

                    Models.Prioridade tempP = new Models.Prioridade()
                    {
                        Valor = (int)hvPriV.HVNovaPrioridade.valorp.Value,
                        Cor = corS

                    };

                    if (MVM.GPrioridades == null)
                    {
                        MessageBox.Show("Erro prioridades");
                        return;
                    }
                    MVM.GPrioridades.Add(tempP);

                }
            }
            else
            {
                priov = (int)hvPriV.selectedPrio;
            }

            // NOTIFICACOES
            if ((priov >= 400) && (hvLembV == null || hvLembV.tsAntec == null || hvLembV.tsExec == null || hvLembV.tsExec.Value == null || hvLembV.tsAntec.Value == null))
            {
                MessageBox.Show("É obrigatório configurar as notificações em tarefas com nível igual ou superior a 400");
                Notificacoes.IsChecked = true;
                return;
            }

            if (priov == null)
            {
                MessageBox.Show("É obrigatório configurar a prioridade da tarefa.");
                Prioridade.IsChecked = true;
                return;
            }

            if (hVDataV.CheckTodoDia.IsChecked == null)
            {
                return;
            }

            DateTime tDataInicio;
            DateTime? tDataFim = null;

            if (hVDataV.CheckTodoDia.IsChecked.Value)
            {
                tDataInicio = hVDataV.calendario.SelectedDate.Value;
            }
            else
            {
                if (hVDataV.tpInicio == null || hVDataV.tpInicio.Value == null)
                {
                    MessageBox.Show("É obrigatório preencher a hora de início da tarefa se a mesma não estiver marcada como todo o dia.");
                    Data.IsChecked = true;
                    return;
                }
                tDataInicio = hVDataV.calendario.SelectedDate.Value.Add(hVDataV.tpInicio.Value.Value.TimeOfDay);
            }

            if (hVDataV.tpFim != null && hVDataV.tpFim.Value != null)
            {
                tDataFim = hVDataV.calendario.SelectedDate.Value.Add(hVDataV.tpFim.Value.Value.TimeOfDay);

                if (tDataFim < tDataInicio)
                {
                    MessageBox.Show("A hora de fim da tarefa deve ser depois da hora de início ou nula.");
                    hVDataV.tpFim.Value = null;
                    Data.IsChecked = true;
                    return;
                }
            }


            if (task == null) return;

            tTarefa = new Models.Tarefa()
            {
                ID = task.ID,
                Titulo = tNome,
                Descricao = tDesc,
                CreationTime = DateTime.Now,
                DataInicio = tDataInicio,
                DataFim = tDataFim,
                valorPrio = (int)priov,
                Estado = estado.SelectedIndex,
                FullDia = hVDataV.CheckTodoDia.IsChecked.Value,
                IsTarefaOriginal = true
            };

            // ALERTAS

            if (hvLembV != null && hvLembV.LembreteAntecipacao.IsChecked != null && hvLembV.LembreteAntecipacao.IsChecked.Value)
            {
                if (hvLembV.tsAntec == null || hvLembV.tsAntec.Value == null)
                {
                    MessageBox.Show("É obrigatório preencher o tempo de antecipação do alerta de antecipação.");
                    Notificacoes.IsChecked = true;
                    return;
                }


                var talertaAntecipacao = new Models.Alerta()
                {
                    Email = false,
                    Windows = true,
                    Tempo = (TimeSpan)hvLembV.tsAntec.Value,
                };

                tTarefa.AlertaAntecipacao = talertaAntecipacao;
            }

            if (hvLembV != null && hvLembV.LembreteExecucao.IsChecked != null && hvLembV.LembreteExecucao.IsChecked.Value)
            {
                if (hvLembV.tsExec == null || hvLembV.tsExec.Value == null)
                {
                    MessageBox.Show("É obrigatório preencher o tempo de antecipação do alerta de execução.");
                    Notificacoes.IsChecked = true;
                    return;
                }

                var talertaExecucao = new Models.Alerta()
                {
                    Email = false,
                    Windows = true,
                    Tempo = (TimeSpan)hvLembV.tsExec.Value,
                };

                tTarefa.AlertaAtraso = talertaExecucao;
            }

            if (hvPerV != null && hvPerV.perSelecionada != 0 && hvPerV.perSelecionada != null &&
                dataLimite != null && intervaloRep != null)
            {
                Models.Periodicidade tPeriodicidade = new Models.Periodicidade()
                    {
                        IDTarefaOriginal = tTarefa.ID,
                        Tipo = (int)hvPerV.perSelecionada,
                        DataOriginal = tTarefa.DataInicio,
                        DataLimite = dataLimite.Value,
                        intervaloRepeticao = (int)intervaloRep,
                        DiasSemana = diasSemana,
                        tipoMensal = opM,
                        tipoAnual = opA
                };

                if (MVM.GPeriodicidades == null)
                    {
                        MessageBox.Show("erro periodicidades");
                        return;
                    }

                var pp = MVM.GPeriodicidades.FirstOrDefault(p => p.IDTarefaOriginal == tTarefa.ID);
                    if (pp == null)
                    {
                        MVM.GPeriodicidades.Add(tPeriodicidade);
                        tTarefa.idPeriodicidade = tPeriodicidade.ID;
                    } else
                    {
                        pp.copy(tPeriodicidade);
                        if (pp != null)
                        tTarefa.idPeriodicidade = pp.ID;

                }
            }

            if (MVM.GTarefas == null)
            {
                MessageBox.Show("erro tarefas");
                return;
            }

            if (MVM.GTarefas == null)
            {
                return;
            }

            Tarefa? tt = MVM.GTarefas.FirstOrDefault(t => t.ID == tTarefa.ID);
            if (tt == null)
            {
                MessageBox.Show("erro tarefas");
                return;
            }
            tt.copy(tTarefa);

            MVM.SavePrioridades();
            MVM.SavePeriodicidades();
            MVM.SaveTarefas();

            MVM.ChangeView("BACK");

        }



        private void Periodicidade_Click(object sender, RoutedEventArgs e)
        {

            if (hVDataV == null)
            {
                MessageBox.Show("É obrigatório preencher a data antes de alterar a periodicidade");
                Data.IsChecked = true;
                Periodicidade.IsChecked = false;
                return;
            }


            if (!hVDataV.calendario.SelectedDate.HasValue)
            {
                MessageBox.Show("É obrigatório preencher a data antes de alterar a periodicidade");
                Data.IsChecked = true;
                Periodicidade.IsChecked = false;
                return;
            }

            if (!((hVDataV.tpInicio != null && hVDataV.tpInicio.Value == null && hVDataV.CheckTodoDia.IsChecked != null && hVDataV.CheckTodoDia.IsChecked.Value == true) ^
                (hVDataV.CheckTodoDia.IsChecked != null && hVDataV.CheckTodoDia.IsChecked.Value == false && hVDataV.tpInicio != null && hVDataV.tpInicio.Value != null)))
            {
                MessageBox.Show("É obrigatório preencher a data antes de alterar a periodicidade");
                Data.IsChecked = true;
                Periodicidade.IsChecked = false;
                return;
            }

            if (hVDataV.tpInicio != null && hVDataV.tpFim != null && hVDataV.tpInicio.Value >= hVDataV.tpFim.Value)
            {
                MessageBox.Show("A hora de fim deve ser maior ou igual que a hora de início.");
                Data.IsChecked = true;
                Periodicidade.IsChecked = false;
                return;
            }


            if (hvPerV == null || hVDataV.calendario.SelectedDate.Value != hvPerV.timeInicial)
            {

                if (hvPerV != null && hvPerV.perSelecionada != 0)
                {
                    MessageBox.Show("A data foi alterada, a configuração de periodicidade será resetada");
                }


                if (task == null) return;
                hvPerV = new HVPeriodicidade(hVDataV.calendario.SelectedDate.Value, task);
                hvPerV.DataContext = new HVPeriodicidadeVM();


            }
            cc.Content = hvPerV;
        }

        private void textNomedaTarefa_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NomedaTarefa.Focus();
        }

        private void NomedaTarefa_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(NomedaTarefa.Text) && NomedaTarefa.Text.Length > 0)
            {
                textNomedaTarefa.Visibility = Visibility.Hidden;
            }
            else
            {
                textNomedaTarefa.Visibility = Visibility.Visible;
            }
        }

        private void textDescricaodaTarefa_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DescricaodaTarefa.Focus();
        }

        private void DescricaodaTarefa_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(DescricaodaTarefa.Text) && DescricaodaTarefa.Text.Length > 0)
            {
                textDescricaodaTarefa.Visibility = Visibility.Hidden;
            }
            else
            {
                textDescricaodaTarefa.Visibility = Visibility.Visible;
            }
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
