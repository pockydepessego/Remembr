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
using System.Windows.Shapes;

namespace Remembr.Views
{
    /// <summary>
    /// Interaction logic for Alerta.xaml
    /// </summary>
    /// 

    public partial class WAlerta : Window
    {
        public WAlerta()
        {
            InitializeComponent();
        }

        public WAlerta(Models.Notificacao n)
        {
            InitializeComponent();
            
            if (n == null)
            {
                MessageBox.Show("Erro ao carregar notificação");
                return;
            }

            switch (n.Tipo)
            {
                case 0:
                    titulo.Text = "Aviso";
                    break;
                case 1:
                    titulo.Text = "Alerta de Antecipação";
                    break;
                case 2:
                    titulo.Text = "Alerta de Execução";
                    break;
            }
            mensagem.Text = n.Mensagem;


        }

        private void AlertaOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void CloseApp_button(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void HideApp_button(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Minimized;
            }
        }
    }
}
