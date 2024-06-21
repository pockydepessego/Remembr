using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Remembr.ViewModels;
using System.IO;
using Remembr.Models;


namespace Remembr.Views
{
    /// <summary>
    /// Interaction logic for Registo.xaml
    /// </summary>
    public partial class Registo : UserControl
    {
        private BitmapImage foto;
        private MainVM MVM;

        public Registo()
        {
            InitializeComponent();
            MVM = (MainVM)Application.Current.MainWindow.DataContext;


            foto = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/NoPhoto.png"));
            pfp.ImageSource = foto;

        }


        // CORES
        SolidColorBrush? blackOutline = new BrushConverter().ConvertFromString("#3F3F3F") as SolidColorBrush;
        SolidColorBrush redOutline = Brushes.Red;


        // NOME
        private void textNome_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtNome.Focus();
        }
        private void txtNome_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNome.Text) && txtNome.Text.Length > 0)
            {
                textNome.Visibility = Visibility.Hidden;

            }
            else
            {
                textNome.Visibility = Visibility.Visible;
            }


            if (txtNome.Text.Length > 35)
            {
                nomeBorder.BorderBrush = redOutline;
            }
            else
            {
                nomeBorder.BorderBrush = blackOutline;
            }

        }




        // EMAIL
        private void textEmail_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtEmail.Focus();
        }
        private void txtemail_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtEmail.Text) && txtEmail.Text.Length > 0)
            {
                textEmail.Visibility = Visibility.Hidden;

            }
            else
            {
                textEmail.Visibility = Visibility.Visible;
            }

            if (!(Regex.IsMatch(txtEmail.Text, "^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,5}$")) && txtEmail.Text.Length > 0)
            {
                emailBorder.BorderBrush = redOutline;
            }
            else
            {
                emailBorder.BorderBrush = blackOutline;
            }

        }




        // PASSWORD
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


            if (txtPassword.Password != txtPasswordConfirmar.Password && !string.IsNullOrEmpty(txtPasswordConfirmar.Password))
            {
                passwordBorder.BorderBrush = redOutline;
                passwordConfirmarBorder.BorderBrush = redOutline;
            }
            else
            {
                passwordBorder.BorderBrush = blackOutline;
                passwordConfirmarBorder.BorderBrush = blackOutline;
            }


        }



        // PASSWORD CONFIRMAR
        private void textPasswordConfirmar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtPasswordConfirmar.Focus();
        }
        private void txtPasswordConfirmar_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPasswordConfirmar.Password) && txtPasswordConfirmar.Password.Length > 0)
            {
                textPasswordConfirmar.Visibility = Visibility.Hidden;
            }
            else
            {
                textPasswordConfirmar.Visibility = Visibility.Visible;
            }


            if (txtPassword.Password != txtPasswordConfirmar.Password && !string.IsNullOrEmpty(txtPassword.Password))
            {
                passwordBorder.BorderBrush = redOutline;
                passwordConfirmarBorder.BorderBrush = redOutline;
            }
            else
            {
                passwordBorder.BorderBrush = blackOutline;
                passwordConfirmarBorder.BorderBrush = blackOutline;
            }

        }

        // BOTÃO REGISTAR
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNome.Text))
            {
                MessageBox.Show("O nome não pode estar vazio.");
                nomeBorder.BorderBrush = redOutline;
                return;
            }

            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                MessageBox.Show("O email não pode estar vazio.");
                emailBorder.BorderBrush = redOutline;
                return;
            }

            if (txtNome.Text.Length > 35)
            {
                MessageBox.Show("O nome não pode ter mais de 35 caracteres.");
                nomeBorder.BorderBrush = redOutline;
                return;
            }

            if (!(Regex.IsMatch(txtEmail.Text, "^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,5}$")))
            {
                MessageBox.Show("O email não é válido.");
                emailBorder.BorderBrush = redOutline;
                return;
            }

            if (txtPassword.Password != txtPasswordConfirmar.Password)
            {
                MessageBox.Show("As passwords não coincidem.");
                passwordBorder.BorderBrush = redOutline;
                passwordConfirmarBorder.BorderBrush = redOutline;
                return;
            }


            var basePath = MVM.basePath;
            string? passwd = null;

            if (txtPassword.Password != "")
            {
                passwd = MVM.HashPassword(txtPassword.Password, out var salt);
                using var writer = new BinaryWriter(File.OpenWrite(System.IO.Path.Combine(MVM.AppData, "rmbrs")));
                writer.Write(salt);
            }


            Perfil perf = new Perfil()
            {
                Nome = txtNome.Text,
                Email = txtEmail.Text,
                Password = passwd,
                Fotografia = foto
            };

            MVM.gPerfil = perf;


            if (MVM.SavePerfil())
            {
                MVM.ChangeView("TarefasPorIniciar");
            }
            else
            {
                return;
            }

        }



        // MUDAR FOTO
        private void Save_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Imagens|*.jpg;*.jpeg;*.png;*.bmp|Todos os ficheiros|*.*";

            if (dlg.ShowDialog() == true)
            {
                foto = new BitmapImage(new Uri(dlg.FileName));
                pfp.ImageSource = foto;

            }


        }
    }
}
