using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Remembr.ViewModels;
using System.IO;
using System.Windows.Media;
using Remembr.Models;

namespace Remembr.Views
{
    /// <summary>
    /// Interaction logic for DefinicoesUtilizador.xaml
    /// </summary>
    public partial class DefinicoesUtilizador : UserControl
    {
        MainVM MVM;
        bool removerPW;

        SolidColorBrush? red = new BrushConverter().ConvertFromString("#eb9d9d") as SolidColorBrush;
        SolidColorBrush? gray = new BrushConverter().ConvertFromString("#FFEBEBEB") as SolidColorBrush;


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

            if (MVM.GPerfil.Password == null)
            {
                RemoverPassword.Visibility = Visibility.Hidden;
                RemoverPassword.IsEnabled = false;
            }


        }

        private void NomedaTarefa_TextChanged(object sender, TextChangedEventArgs e)
        {

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

            if (removerPW)
            {
                MVM.GPerfil.Password = null;
            } else if (txtPassword.Password != "")
            {
                
                if (txtPassword.Password != txtPasswordconfirm.Password)
                {
                    MessageBox.Show("As passwords não coincidem.");
                    pass1.Fill = red;
                    pass2.Fill = red;
                    return;
                }
                
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

            MVM.SavePerfil();

            MVM.ChangeView("BACK");

        }

        private void PasswordChanged_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtPassword.Focus();
        }


        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {

            if (MVM.GPerfil == null)
            {
                MessageBox.Show("Erro de perfil");
                App.Current.Shutdown();
                return;
            }

            if (!string.IsNullOrEmpty(txtPassword.Password) && txtPassword.Password.Length > 0)
            {
                PasswordChanged.Visibility = Visibility.Hidden;
                removerPW = false;
                RemoverPassword.IsEnabled = true;
                RemoverPassword.Visibility = Visibility.Visible;
            }
            else
            {
                PasswordChanged.Visibility = Visibility.Visible;
                if (string.IsNullOrEmpty(txtPasswordconfirm.Password))
                {
                    if (MVM.GPerfil.Password == null)
                    {
                        RemoverPassword.Visibility = Visibility.Hidden;
                        RemoverPassword.IsEnabled = false;
                    }
                } 

            }


            
            if (txtPassword.Password != txtPasswordconfirm.Password && !string.IsNullOrEmpty(txtPasswordconfirm.Password))
            {
                pass1.Fill = red;
                pass2.Fill = red;
            }
            else
            {
                pass1.Fill = gray;
                pass2.Fill = gray;
            }


        }
        private void txtPasswordconfirm_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (MVM.GPerfil == null)
            {
                MessageBox.Show("Erro de perfil");
                App.Current.Shutdown();
                return;
            }

            if (!string.IsNullOrEmpty(txtPasswordconfirm.Password) && txtPasswordconfirm.Password.Length > 0)
            {
                PasswordConfirmChanged.Visibility = Visibility.Hidden;
                removerPW = false;
                RemoverPassword.IsEnabled = true;
                RemoverPassword.Visibility = Visibility.Visible;
            }
            else
            {
                PasswordConfirmChanged.Visibility = Visibility.Visible;
                if (string.IsNullOrEmpty(txtPassword.Password))
                {
                    if (MVM.GPerfil.Password == null)
                    {
                        RemoverPassword.Visibility = Visibility.Hidden;
                        RemoverPassword.IsEnabled = false;
                    }
                }
            }

            if (txtPassword.Password != txtPasswordconfirm.Password && !string.IsNullOrEmpty(txtPassword.Password))
            {
                pass1.Fill = red;
                pass2.Fill = red;
            }
            else
            {
                pass1.Fill = gray;
                pass2.Fill = gray;
            }



        }

        private void RemoverPassword_Click(object sender, RoutedEventArgs e)
        {
            removerPW = true;
            RemoverPassword.IsEnabled = false;
            RemoverPassword.Visibility = Visibility.Hidden;
            txtPassword.Password = "";
            txtPasswordconfirm.Password = "";
        }

        private void PasswordConfirmChanged_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtPassword.Focus();
        }
    }
}
