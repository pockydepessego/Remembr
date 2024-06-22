using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Remembr.ViewModels;
using System.IO;

namespace Remembr.Views
{
    /// <summary>
    /// Interaction logic for DefinicoesUtilizador.xaml
    /// </summary>
    public partial class DefinicoesUtilizador : UserControl
    {
        MainVM MVM;
        public DefinicoesUtilizador()
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
            pfp2.ImageSource = MVM.GPerfil.Fotografia;
            Nomeutilizadorchanged.Text = MVM.GPerfil.Nome;
            EmailChanged.Text = MVM.GPerfil.Email;


        }

        private void NomedaTarefa_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void PasswordChanged_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtPassword.Focus();
        }
        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPassword.Password) && txtPassword.Password.Length > 0)
            {
                PasswordChanged.Visibility = Visibility.Hidden;
            }
            else
            {
                PasswordChanged.Visibility = Visibility.Visible;
            }
        }

        private void cancelartarefa_Click(object sender, RoutedEventArgs e)
        {

            /* não vamos usar este aqui mas o código fica para referência futura
            Alerta av = new Alerta();
            av.mensagem.Text = "Ao voltar para trás, as modificações serão descartadas.";
            av.Show();
            */

            MVM.ChangeView("BACK");
        }

        private void Alterarpfp_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Imagens|*.jpg;*.jpeg;*.png;*.bmp|Todos os ficheiros|*.*";

            if (dlg.ShowDialog() == true)
            {
                pfp2.ImageSource = new BitmapImage(new Uri(dlg.FileName));
            }

        }

        private void Removerpfp_Click(object sender, RoutedEventArgs e)
        {
            pfp2.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/NoPhoto.png"));
        }

        private void GuardarDefiniçoes_Click(object sender, RoutedEventArgs e)
        {
            if (MVM.GPerfil == null)
            {
                MessageBox.Show("Erro de perfil");
                return;
            }

            if (txtPassword.Password != "")
            {
                /*
                if (txtPassword.Password != txtPasswordConfirmar.Password)
                {
                    MessageBox.Show("As passwords não coincidem.");
                    passwordBorder.BorderBrush = redOutline;
                    passwordConfirmarBorder.BorderBrush = redOutline;
                    return;
                }
                */
                MVM.GPerfil.Password = MVM.HashPassword(txtPassword.Password, out var salt);
                using var writer = new BinaryWriter(File.OpenWrite(System.IO.Path.Combine(MVM.AppData, "rmbrs")));
                writer.Write(salt);
            }

            MVM.GPerfil.Nome = Nomeutilizadorchanged.Text;
            MVM.GPerfil.Email = EmailChanged.Text;
            MVM.GPerfil.Fotografia = (BitmapImage)pfp2.ImageSource;

            if (!MVM.SavePerfil())
            {
                return;
            }

            MVM.ChangeView("BACK");

        }
    }
}
