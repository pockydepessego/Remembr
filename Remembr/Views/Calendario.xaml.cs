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
using Syncfusion.UI.Xaml.Scheduler;
using System.Security.Cryptography;

namespace Remembr.Views
{
    /// <summary>
    /// Interaction logic for Calendario.xaml
    /// </summary>
    public partial class Calendario : UserControl
    {
        private MainVM MVM;
        public Calendario()
        {
            InitializeComponent();
            MVM = (MainVM)Application.Current.MainWindow.DataContext;
            if (MVM.GPerfil == null)
            {
                MessageBox.Show("Erro de perfil");
                App.Current.Shutdown();
                return;
            }
            pfp.ImageSource = MVM.GPerfil.Fotografia;

            if (MVM.GTarefas == null)
            {
                MessageBox.Show("Erro tarefas");
                return;
            }

            if (MVM.GPrioridades == null)
            {
                MessageBox.Show("Erro prioridades");
                return;
            }

            ScheduleAppointmentCollection appointmentCollection = new ScheduleAppointmentCollection();

            foreach (var tarefa in MVM.GTarefas)
            {
                var appointment = new ScheduleAppointment();
                appointment.StartTime = (DateTime)tarefa.DataInicio;
                appointment.IsAllDay = tarefa.FullDia;
                appointment.Subject = tarefa.Titulo;
                appointment.Notes = tarefa.Descricao;
                appointment.Id = tarefa.ID;
                appointment.Location = tarefa.valorPrio.ToString(); // TEM DE SER TEM DE SER

                if (tarefa.DataFim != null)
                {
                    appointment.EndTime = (DateTime)tarefa.DataFim;
                }

                Models.Prioridade? prio = MVM.GPrioridades.Where(p => p.Valor == tarefa.valorPrio).FirstOrDefault();
                if (prio != null)
                {
                    appointment.AppointmentBackground = new BrushConverter().ConvertFromString(prio.Cor) as SolidColorBrush;
                }

                appointmentCollection.Add(appointment);
            }

            Schedule.ItemsSource = appointmentCollection;
            Schedule.DaysViewSettings.TimeRulerFormat = "HH:mm";
            Schedule.MonthViewSettings.ShowWeekNumber = true;
            



        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }

        private void HideApp_button(object sender, MouseButtonEventArgs e)
        {

        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void BigApp_button(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void CloseApp_button(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_MouseEnter_1(object sender, MouseEventArgs e)
        {

        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void CalendarPage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TarefaPage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Ellipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MVM.ChangeView("DefinicoesUtilizador");
        }

        private void Schedule_AppointmentDragOver(object sender, AppointmentDragOverEventArgs e)
        {

        }

        private void Schedule_AppointmentTapped(object sender, AppointmentTappedArgs e)
        {
            if (e.Appointment == null)
            {
                return;
            }

            var tid = e.Appointment.Id.ToString();

            if (tid == null)
            {
                return;
            }

            MVM.editarTarefa(tid);
        }

        private void DayViewButton_Click(object sender, RoutedEventArgs e)
        {
            Schedule.ViewType = SchedulerViewType.Day;
        }

        private void MonthViewButton_Click(object sender, RoutedEventArgs e)
        {
            Schedule.ViewType = SchedulerViewType.Month;
        }
    }

}
