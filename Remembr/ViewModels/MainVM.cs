using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.Input;
using Remembr.Models;
using System.Xml.Linq;
using System.Windows.Media.Imaging;
using System.Security.Cryptography;


namespace Remembr.ViewModels
{
    public class MainVM : BaseVM
    {

        private BaseVM? _currentViewModel;
        public BaseVM? CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand<string> ChangeViewCommand { get; }

        private BaseVM? lastVM;
        public void ChangeView(string viewName)
        {
            if (viewName == "BACK")
            {
                if (lastVM == null)
                {
                    MessageBox.Show("Erro: Não é possível voltar atrás.");
                    return;
                }
                CurrentViewModel = lastVM;
                return;
            }
            if (CurrentViewModel != null)
                lastVM = CurrentViewModel;
            
            switch (viewName)
            {
                case "Registo":
                    CurrentViewModel = new RegistoVM();
                    break;
                case "Login":
                    CurrentViewModel = new LoginVM();
                    break;
                case "Calendario":
                    CurrentViewModel = new CalendarioVM();
                    break;
                case "TarefasApagadas":
                    CurrentViewModel = new TarefasApagadasVM();
                    break;
                case "TarefasExec":
                    CurrentViewModel = new TarefasExecVM();
                    break;
                case "TarefasPorIniciar":
                    CurrentViewModel = new TarefasPorIniciarVM();
                    break;
                case "TarefasTerminadas":
                    CurrentViewModel = new TarefasTerminadasVM();
                    break;
                case "NovaTarefa":
                    CurrentViewModel = new NovaTarefaVM();
                    break;
                case "DefinicoesUtilizador":
                    CurrentViewModel = new DefinicoesUtilizadorVM();
                    break;


                default:
                    MessageBox.Show("Erro: View não encontrada.");
                    return;
            }
        }


        const int keySize = 64;
        const int iterations = 350000;
        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
        public string HashPassword(string password, out byte[] salt)
        {
            salt = RandomNumberGenerator.GetBytes(keySize);

            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations,
                hashAlgorithm,
                keySize);

            return Convert.ToHexString(hash);
        }


        public bool VerifyPassword(string password, string hash, byte[] salt)
        {
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, hashAlgorithm, keySize);

            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
        }

        public string AppData;
        public string basePath;
        public Perfil? gPerfil { get; set; }

        public MainVM()
        {

#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
            ChangeViewCommand = new RelayCommand<string>(ChangeView);
#pragma warning restore CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).


            AppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            basePath = Path.Combine(AppData, "Remembr");


            if (!Directory.Exists(basePath))
            {
                ChangeView("Registo");
            }
            else
            {
                if (LoadPerfil())
                {
                    if (gPerfil == null)
                    {
                        MessageBox.Show("Erro de perfil.");
                        App.Current.Shutdown();
                        return;
                    } 

                    if (gPerfil.Password != null)
                    {
                        ChangeView("Login");

                    } else
                    {
                        ChangeView("TarefasPorIniciar");
                    }
                
                
                }
                else
                {
                    App.Current.Shutdown();
                }

            }







        }


        public bool SavePerfil()
        {

            if (gPerfil == null)
            {
                MessageBox.Show("Erro: Perfil não encontrado.");
                return false;
            }

            try
            {
                Directory.CreateDirectory(basePath);


                XDocument doc = new XDocument(
                        new XElement("perfil",
                        new XAttribute("nome", gPerfil.Nome),
                        new XAttribute("email", gPerfil.Email)
                        )
                    );

                XElement? docPerf = doc.Element("perfil");

                if ((gPerfil.Password != null) && (docPerf != null))
                {
                    docPerf.Add(new XAttribute("password", gPerfil.Password));
                }

                doc.Save(System.IO.Path.Combine(basePath, "perfil.xml"));

                string photoPath = System.IO.Path.Combine(basePath, "pfp.png");

                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(gPerfil.Fotografia));
                using (var fileStream = new System.IO.FileStream(photoPath, System.IO.FileMode.Create))
                {
                    encoder.Save(fileStream);
                }

                return true;


            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao guardar perfil:\n\n" + ex.Message + "\n" + ex.StackTrace);
                return false;
            }


        }

        public bool LoadPerfil()
        {

            string photoPath = System.IO.Path.Combine(basePath, "pfp.png");
            string perfilPath = System.IO.Path.Combine(basePath, "perfil.xml");
            try
            {
                if (Path.Exists(perfilPath))
                {

                    XDocument doc = XDocument.Load(perfilPath);
                    XElement? docPerf = doc.Element("perfil");

                    if (docPerf == null)
                    {
                        MessageBox.Show("Ficheiro de perfil inválido.");
                        return false;
                    }

                    XAttribute? docNome = docPerf.Attribute("nome");
                    XAttribute? docEmail = docPerf.Attribute("email");
                    XAttribute? docPassword = docPerf.Attribute("password");

                    if (docNome == null || docEmail == null)
                    {
                        MessageBox.Show("Ficheiro de perfil inválido.");
                        return false;
                    }

                    string? docPasswordV = null;
                    if (docPassword != null)
                    {
                        docPasswordV = docPassword.Value;
                    } 

                    if (!File.Exists(photoPath))
                    {
                        MessageBox.Show("Fotografia inválida.");
                        return false;
                    }

                    MemoryStream ms = new MemoryStream();
                    BitmapImage bi = new BitmapImage();
                    byte[] bytArray = File.ReadAllBytes(Path.Combine(basePath, "pfp.png"));
                    ms.Write(bytArray, 0, bytArray.Length); ms.Position = 0;
                    bi.BeginInit();
                    bi.StreamSource = ms;
                    bi.EndInit();

                    BitmapImage foto = bi;

                    Perfil perf = new Perfil()
                    {
                        Nome = docNome.Value,
                        Email = docEmail.Value,
                        Password = docPasswordV,
                        Fotografia = foto
                    };

                    gPerfil = perf;
                    return true;

                }
                else
                {
                    MessageBox.Show("Erro ao carregar perfil: o ficheiro não existe");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar perfil:\n\n" + ex.Message + "\n" + ex.StackTrace);
                return false;
            }
        }

    }
}
