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
using System.Windows.Documents.DocumentStructures;


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
                case "HomeTarefas":
                    CurrentViewModel = new HomeTarefasVM();
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
        public List<Tarefa>? gTarefas { get; set; } = [];

        public List<Prioridade>? gPrioridades { get; set; } = [];

        public List<Periodicidade>? gPeriodicidades { get; set; } = [];
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


                    if (!LoadPrioridades())
                    {
                        MessageBox.Show("Erro de prioridades.");
                        App.Current.Shutdown();
                        return;
                    }

                    if (!LoadTarefas())
                    {
                        MessageBox.Show("Erro de tarefas.");
                        App.Current.Shutdown();
                        return;
                    }


                    if (gPerfil.Password != null)
                    {
                        ChangeView("Login");

                    } else
                    {
                        ChangeView("HomeTarefas");
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

        public bool SaveTarefas()
        {
            if (gTarefas == null)
            {
                MessageBox.Show("Erro: Tarefas não encontradas.");
                return false;
            }

            if (gTarefas.Count == 0)
            {
                return true;
            }

            try
            {
                Directory.CreateDirectory(basePath);

                XDocument doc = new XDocument(
                    new XElement("tarefas")
                    );

                XElement? docTarefas = doc.Element("tarefas");

                foreach (Tarefa t in gTarefas)
                {
                    if (t == null || docTarefas == null)
                    {
                        throw new Exception("Erro: tarefa não inicializada ao guardar tarefas");
                    }

                    docTarefas.Add(
                        new XElement("tarefa",
                            new XAttribute("ID", t.ID),
                            new XAttribute("Titulo", t.Titulo),
                            new XAttribute("CreationTime", t.CreationTime),
                            new XAttribute("DataInicio", t.DataInicio),
                            new XAttribute("FullDia", t.FullDia),
                            new XAttribute("Estado", t.Estado),
                            new XAttribute("Prio", t.valorPrio),
                            new XAttribute("IsTarefaOriginal", t.IsTarefaOriginal)
                        )
                    );


                    XElement? docTarefa = docTarefas.Elements().Last() ?? throw new Exception("Erro ao guardar tarefa.");

                    if (t.Descricao != null)
                    {
                        docTarefa.Add(
                            new XAttribute("Descricao", t.Descricao)
                        );
                    }

                    if (t.DataFim != null)
                    {
                        docTarefa.Add(
                            new XAttribute("DataFim", t.DataFim)
                        );
                    }

                    if (t.idPeriodicidade != null)
                    {
                        docTarefa.Add(
                            new XAttribute("Periodicidade", t.idPeriodicidade)
                        );
                    }

                    
                    if (t.AlertaAtraso != null)
                    {
                        docTarefa.Add(
                            new XElement("AlertaAtraso",
                                new XAttribute("Email", t.AlertaAtraso.Email),
                                new XAttribute("Windows", t.AlertaAtraso.Windows),
                                new XAttribute("Tempo", t.AlertaAtraso.Tempo.TotalSeconds)
                            )
                        );
                    }


                    if (t.AlertaAntecipacao != null)
                    {
                        docTarefa.Add(
                            new XElement("AlertaAntecipacao",
                                new XAttribute("Email", t.AlertaAntecipacao.Email),
                                new XAttribute("Windows", t.AlertaAntecipacao.Windows),
                                new XAttribute("Tempo", t.AlertaAntecipacao.Tempo.TotalSeconds)
                            )
                        );
                    }

                }

                doc.Save(System.IO.Path.Combine(basePath, "tarefas.xml"));

                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao guardar tarefas:\n\n" + ex.Message + "\n" + ex.StackTrace);
                return false;
            }
        }

        public bool LoadTarefas()
        {
            string tarefasPath = System.IO.Path.Combine(basePath, "tarefas.xml");
            try
            {

                if (gPrioridades == null)
                {
                    MessageBox.Show("Erro: Prioridades não encontradas.");
                    return false;
                }


                if (Path.Exists(tarefasPath))
                {

                    XDocument doc = XDocument.Load(tarefasPath);
                    XElement? docTarefas = doc.Element("tarefas");

                    if (docTarefas == null)
                    {
                        MessageBox.Show("Ficheiro de tarefas inválido.");
                        return false;
                    }

                    if (gTarefas == null)
                    {
                        MessageBox.Show("Lista de tarefas inválida.");
                        return false;
                    } 

                    foreach (XElement t in docTarefas.Elements("tarefa"))
                    {
                        if (t == null)
                        {
                            MessageBox.Show("Erro: tarefa não inicializada ao carregar tarefas");
                            return false;
                        }

                        XAttribute? docID = t.Attribute("ID");
                        XAttribute? docTitulo = t.Attribute("Titulo");
                        XAttribute? docCreationTime = t.Attribute("CreationTime");
                        XAttribute? docDataInicio = t.Attribute("DataInicio");
                        XAttribute? docFullDia = t.Attribute("FullDia");
                        XAttribute? docEstado = t.Attribute("Estado");
                        XAttribute? docPrio = t.Attribute("Prio");
                        XAttribute? docIsTarefaOriginal = t.Attribute("IsTarefaOriginal");

                        if (docID == null || docTitulo == null || docCreationTime == null || docDataInicio == null || docFullDia == null || docEstado == null || docPrio == null || docIsTarefaOriginal == null)
                        {
                            MessageBox.Show("Ficheiro de tarefas inválido.");
                            return false;
                        }


                        Prioridade? tPrio = gPrioridades.FirstOrDefault(p => p.Valor == int.Parse(docPrio.Value));

                        if (tPrio == null)
                        {
                            tPrio = new Prioridade()
                            {
                                Valor = int.Parse(docPrio.Value),
                                Cor = "#000000"
                            };
                            gPrioridades.Add(tPrio);
                        }



                        var tempTarefa = new Tarefa()
                        {
                            ID = docID.Value,
                            Titulo = docTitulo.Value,
                            CreationTime = DateTime.Parse(docCreationTime.Value),
                            DataInicio = DateTime.Parse(docDataInicio.Value),
                            FullDia = bool.Parse(docFullDia.Value),
                            Estado = int.Parse(docEstado.Value),
                            IsTarefaOriginal = bool.Parse(docIsTarefaOriginal.Value),
                            valorPrio = int.Parse(docPrio.Value)
                        };

                        XAttribute? docDescricao = t.Attribute("Descricao");
                        XAttribute? docDataFim = t.Attribute("DataFim");
                        XAttribute? docIDPeriodicidade = t.Attribute("Periodicidade");
                        XElement? docAlertaAtraso = t.Element("AlertaAtraso");
                        XElement? docAlertaAntecipacao = t.Element("AlertaAntecipacao");

                        if (docDescricao != null)
                        {
                            tempTarefa.Descricao = docDescricao.Value;
                        }

                        if (docDataFim != null)
                        {
                            tempTarefa.DataFim = DateTime.Parse(docDataFim.Value);
                        }

                        if (docIDPeriodicidade != null)
                        {
                            tempTarefa.idPeriodicidade = docIDPeriodicidade.Value;
                        }

                        if (docAlertaAtraso != null)
                        {
                            XAttribute? docEmail = docAlertaAtraso.Attribute("Email");
                            XAttribute? docWindows = docAlertaAtraso.Attribute("Windows");
                            XAttribute? docTempo = docAlertaAtraso.Attribute("Tempo");

                            if (docEmail == null || docWindows == null || docTempo == null)
                            {
                                MessageBox.Show("Ficheiro de tarefas inválido.");
                                return false;
                            }

                            tempTarefa.AlertaAtraso = new Alerta()
                            {
                                Email = bool.Parse(docEmail.Value),
                                Windows = bool.Parse(docWindows.Value),
                                Tempo = TimeSpan.FromSeconds(int.Parse(docTempo.Value))
                            };

                        }

                        if (docAlertaAntecipacao != null)
                        {
                            XAttribute? docEmail = docAlertaAntecipacao.Attribute("Email");
                            XAttribute? docWindows = docAlertaAntecipacao.Attribute("Windows");
                            XAttribute? docTempo = docAlertaAntecipacao.Attribute("Tempo");

                            if (docEmail == null || docWindows == null || docTempo == null)
                            {
                                MessageBox.Show("Ficheiro de tarefas inválido.");
                                return false;
                            }

                            tempTarefa.AlertaAntecipacao = new Alerta()
                            {
                                Email = bool.Parse(docEmail.Value),
                                Windows = bool.Parse(docWindows.Value),
                                Tempo = TimeSpan.FromSeconds(int.Parse(docTempo.Value))
                            };

                        }

                        gTarefas.Add(tempTarefa);

                    }
                    return true;
                }
                else
                {
                    return true;
                }
            } catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar tarefas:\n\n" + ex.Message + "\n" + ex.StackTrace);
                return false;
            }
        }

        
        public bool SavePrioridades()
        {
            try
            {
                if (gPrioridades == null)
                {
                    MessageBox.Show("Erro: Prioridades não encontradas.");
                    return false;
                }

                List<Prioridade> basePrioridades =
                    [
                        new Prioridade() { Valor = 100, Cor = "#76d074" },
                        new Prioridade() { Valor = 200, Cor = "#feff00" },
                        new Prioridade() { Valor = 300, Cor = "#feb022" },
                        new Prioridade() { Valor = 400, Cor = "#fe3f3f" },
                    ];


                if (gPrioridades == basePrioridades)
                {
                    return true;
                }

                Directory.CreateDirectory(basePath);

                XDocument doc = new XDocument(
                    new XElement("prioridades")
                    );

                XElement? docPrioridades = doc.Element("prioridades");

                foreach (Prioridade p in gPrioridades)
                {
                    if (new List<int> { 100, 200, 300, 400 }.Contains(p.Valor ))
                    {
                        continue;
                    }

                    if (p == null || docPrioridades == null)
                    {
                        throw new Exception("Erro: prioridade não inicializada ao guardar prioridades");
                    }

                    docPrioridades?.Add(
                        new XElement("prioridade",
                        new XAttribute("Valor", p.Valor),
                        new XAttribute("Cor", p.Cor)
                        )
                    );
                }


                doc.Save(System.IO.Path.Combine(basePath, "prioridades.xml"));

                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao guardar prioridades:\n\n" + ex.Message + "\n" + ex.StackTrace);
                return false;
            }
        }
        
        public bool LoadPrioridades()
        {
            string prioridadesPath = System.IO.Path.Combine(basePath, "prioridades.xml");
            List<Prioridade> tempPrioridades =
            [
                new Prioridade() { Valor = 100, Cor = "#76d074" },
                new Prioridade() { Valor = 200, Cor = "#feff00" },
                new Prioridade() { Valor = 300, Cor = "#feb022" },
                new Prioridade() { Valor = 400, Cor = "#fe3f3f" },
            ];

            try
            {
                if (Path.Exists(prioridadesPath))
                {

                    XDocument doc = XDocument.Load(prioridadesPath);
                    XElement? docPrioridades = doc.Element("prioridades");

                    if (docPrioridades == null)
                    {
                        MessageBox.Show("Ficheiro de prioridades inválido.");
                        return false;
                    }

                    foreach (XElement p in docPrioridades.Elements("prioridade"))
                    {
                        if (p == null)
                        {
                            MessageBox.Show("Erro: prioridade não inicializada ao carregar prioridades");
                            return false;
                        }

                        XAttribute? docValor = p.Attribute("Valor");
                        XAttribute? docCor = p.Attribute("Cor");


                        if (docValor == null || docCor == null)
                        {
                            MessageBox.Show("Ficheiro de prioridades inválido.");
                            return false;
                        }

                        if (tempPrioridades.Any(p => p.Valor == int.Parse(docValor.Value)))
                        {
                            continue;
                        }

                        tempPrioridades.Add(new Prioridade()
                        {
                            Valor = int.Parse(docValor.Value),
                            Cor = docCor.Value
                        });

                    }
                }

                gPrioridades = tempPrioridades;
                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar prioridades:\n\n" + ex.Message + "\n" + ex.StackTrace);
                return false;
            }
        }

        public bool SavePeriodicidades()
        {
            try
            {
                if (gTarefas == null)
                {
                    MessageBox.Show("Erro: Tarefas não encontradas.");
                    return false;
                }

                if (gTarefas.Count == 0)
                {
                    return true;
                }

                Directory.CreateDirectory(basePath);

                XDocument doc = new XDocument(
                                       new XElement("periodicidades")
                                                          );

                XElement? docPeriodicidades = doc.Element("periodicidades");

                foreach (Tarefa t in gTarefas)
                {
                    if (t == null || docPeriodicidades == null)
                    {
                        throw new Exception("Erro: tarefa não inicializada ao guardar periodicidades");
                    }

                    if (t.idPeriodicidade == null)
                    {
                        continue;
                    }

                    docPeriodicidades.Add(
                                               new XElement("periodicidade",
                                                                          new XAttribute("ID", t.idPeriodicidade),
                                                                                                     new XAttribute("Titulo", t.Titulo)
                                                                                                                            )
                                                                  );

                }

                doc.Save(System.IO.Path.Combine(basePath, "periodicidades.xml"));

                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao guardar periodicidades:\n\n" + ex.Message + "\n" + ex.StackTrace);
                return false;
            }
        }












    }
}
