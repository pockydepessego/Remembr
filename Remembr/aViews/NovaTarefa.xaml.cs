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

        private App _app;

        HVLembretes? hvlView;

        public NovaTarefa()
        {
            InitializeComponent();
            _app = (App)App.Current;


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

        private void NomedaTarefa_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void DescricaoodaTarefa_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void DescricaodaTarefa_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void DaraInicio_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Lembretes_Click(object sender, RoutedEventArgs e)
        {
            if (hvlView == null) {
                hvlView = new HVLembretes();
                hvlView.DataContext = new HVLembretesVM();

                cc.Content = hvlView;
            }
        }

        private void cancelartarefa_Click(object sender, RoutedEventArgs e)
        {
            // MessageBox.Show(hvlView.LembreteAntecipacao.IsChecked.Value.ToString());

        }
    }
}
