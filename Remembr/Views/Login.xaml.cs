using Remembr.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
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

namespace Remembr.Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        private MainVM MVM;
        public Login()
        {
            InitializeComponent();
            MVM = (MainVM)Application.Current.MainWindow.DataContext;

            if (MVM.gPerfil == null)
            {
                MessageBox.Show("Erro de perfil");
                App.Current.Shutdown();
                return;
            }

            username.Text = MVM.gPerfil.Nome;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var salt = File.ReadAllBytes(Path.Combine(MVM.AppData, "rmbrs"));

            if (salt == null || MVM.gPerfil == null || MVM.gPerfil.Password == null) {
                MessageBox.Show("Erro de perfil");
                return;
            }

            if (MVM.VerifyPassword(txtPassword.Password, MVM.gPerfil.Password, salt))
            {
                MVM.ChangeView("TarefasPorIniciar");
            }
            else
            {
                MessageBox.Show("Password errada");
            }

        }

        private void textPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtPassword.Focus();
        }
        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPassword.Password) && txtPassword.Password.Length > 0)
            {
                textPassword.Visibility = Visibility.Hidden;
            }
            else
            {
                textPassword.Visibility = Visibility.Visible;
            }
        }
    }
}

