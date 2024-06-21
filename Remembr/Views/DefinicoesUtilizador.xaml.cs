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
            if (MVM.gPerfil == null)
            {
                MessageBox.Show("Erro de perfil");
                App.Current.Shutdown();
                return;
            }
            pfp.ImageSource = MVM.gPerfil.Fotografia;
            pfp2.ImageSource = MVM.gPerfil.Fotografia;
            Nomeutilizadorchanged.Text = MVM.gPerfil.Nome;
            EmailChanged.Text = MVM.gPerfil.Email;


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
            if (MVM.gPerfil == null)
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
                MVM.gPerfil.Password = MVM.HashPassword(txtPassword.Password, out var salt);
                using var writer = new BinaryWriter(File.OpenWrite(System.IO.Path.Combine(MVM.AppData, "rmbrs")));
                writer.Write(salt);
            }

            MVM.gPerfil.Nome = Nomeutilizadorchanged.Text;
            MVM.gPerfil.Email = EmailChanged.Text;
            MVM.gPerfil.Fotografia = (BitmapImage)pfp2.ImageSource;

            if (!MVM.SavePerfil())
            {
                return;
            }

            MVM.ChangeView("BACK");

        }
    }
}
