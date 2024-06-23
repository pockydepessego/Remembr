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
    /// Interaction logic for NovaTarefa.xaml
    /// </summary>
    /// 

    public partial class NovaTarefa : UserControl
    {


        HVLembretes? hvLembV;
        HVData? hVDataV;
        HVPeriodicidade? hvPerV;
        HVPrioridade? hvPriV;
        MainVM MVM;

        public NovaTarefa()
        {
            InitializeComponent();
            MVM = (MainVM)Application.Current.MainWindow.DataContext;

            if (hVDataV == null)
            {
                hVDataV = new HVData();
                hVDataV.DataContext = new HVDataVM();

                cc.Content = hVDataV;
            }

        }


        private void BigApp_button(object sender, MouseButtonEventArgs e)
        {

        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void CloseApp_button(object sender, MouseButtonEventArgs e)
        {

        }

        private void HideApp_button(object sender, MouseButtonEventArgs e)
        {

        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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

        private void DaraInicio_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Lembretes_Click(object sender, RoutedEventArgs e)
        {
            if (hvLembV == null) {
                hvLembV = new HVLembretes();
                hvLembV.DataContext = new HVLembretesVM();
            }
            cc.Content = hvLembV;
        }

        private void Data_Click(object sender, RoutedEventArgs e)
        {
            if (hVDataV == null)
            {
                hVDataV = new HVData();
                hVDataV.DataContext = new HVDataVM();
            }
            cc.Content = hVDataV;
        }

        private void Prioridade_Click(object sender, RoutedEventArgs e)
        {
            if (hvPriV == null)
            {
                hvPriV = new HVPrioridade();
                hvPriV.DataContext = new HVPrioridadeVM();
            }
            cc.Content = hvPriV;
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

            if (!((hVDataV.tpInicio != null && hVDataV.tpInicio.Value == null && hVDataV.CheckTodoDia.IsChecked != null &&  hVDataV.CheckTodoDia.IsChecked.Value==true)^
                (hVDataV.CheckTodoDia.IsChecked != null && hVDataV.CheckTodoDia.IsChecked.Value==false && hVDataV.tpInicio != null && hVDataV.tpInicio.Value != null))) {
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

            if (hvPerV == null)
            {

                if (hVDataV.CheckTodoDia.IsChecked != null && hVDataV.CheckTodoDia.IsChecked.Value)
                {

                    hvPerV = new HVPeriodicidade(hVDataV.calendario.SelectedDate.Value);
                    hvPerV.DataContext = new HVPeriodicidadeVM();


                }



            }
            cc.Content = hvPerV;
        }
        private void cancelartarefa_Click(object sender, RoutedEventArgs e)
        {
            MVM.ChangeView("BACK");
            // MessageBox.Show(hvlView.LembreteAntecipacao.IsChecked.Value.ToString());

        }


    }
}
