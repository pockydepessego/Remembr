using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Remembr.ViewModels;
using Syncfusion.UI.Xaml.Schedule;

namespace Remembr.Views
{
    /// <summary>
    /// Interaction logic for HVHAExportar.xaml
    /// </summary>
    public partial class HVHAExportar : UserControl
    {
        MainVM MVM;
        public HVHAExportar()
        {
            InitializeComponent();
            MVM = (MainVM)Application.Current.MainWindow.DataContext;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (MVM.GTarefas == null)
            {
                return;
            }

            if (MVM.GPrioridades == null)
            {
                return;
            }

            SfSchedule scheduler = new SfSchedule();
            ScheduleAppointmentCollection appointmentCollection = new ScheduleAppointmentCollection();

            foreach (var tarefa in MVM.GTarefas)
            {
                if (tarefa.Estado == -1)
                {
                    continue;
                }

                if (tarefa.DataInicio < DateTime.Now)
                {
                    continue;
                }

                var appointment = new ScheduleAppointment();
                appointment.StartTime = (DateTime)tarefa.DataInicio;
                appointment.AllDay = tarefa.FullDia;
                appointment.Subject = tarefa.Titulo;
                appointment.Notes = tarefa.Descricao;

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

            scheduler.Appointments = appointmentCollection;
            scheduler.ExportICS();

        }

        private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
