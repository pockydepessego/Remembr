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
        public EditarTarefa()
        {
            InitializeComponent();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void cancelartarefa_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Data_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Prioridade_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Lembretes_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CriarTarefa_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Periodicidade_Click(object sender, RoutedEventArgs e)
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

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
