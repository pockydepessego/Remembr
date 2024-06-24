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
using Syncfusion.UI.Xaml.Gauges;
using System.Diagnostics;
using System.Threading;
using Remembr.Views;


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

        public void editarTarefa(string idTarefa)
        {
            if (CurrentViewModel != null)
                lastVM = CurrentViewModel;
            CurrentEditarTarefa = idTarefa;
            CurrentViewModel = new EditarTarefaVM();
        }
        public string? CurrentEditarTarefa { get; set; } = null;
        public void ChangeView(string viewName)
        {
            CurrentEditarTarefa = null;
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

                case "Avancado":
                    CurrentViewModel = new AvancadoVM();
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
        public Perfil? GPerfil { get; set; }
        public List<Tarefa>? GTarefas { get; set; } = [];

        public List<Prioridade>? GPrioridades { get; set; } = [];

        public List<Periodicidade>? GPeriodicidades { get; set; } = [];

        public List<Notificacao>? GNotificacoes { get; set; } = [];
        public MainVM()
        {

            Process proc = Process.GetCurrentProcess();
            int count = Process.GetProcesses().Where(p =>
                p.ProcessName == proc.ProcessName).Count();

            if (count > 1)
            {
                MessageBox.Show("Só é permitido rodar uma instância desta aplicação. Se outra janela existe mas não está a responder, encerre-a e tente novamente");
                App.Current.Shutdown();
            }


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
                    if (GPerfil == null)
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

                    if (!LoadPeroidicidades())
                    {
                        MessageBox.Show("Erro de periodicidades.");
                        App.Current.Shutdown();
                        return;
                    }


                    if (!LoadTarefas())
                    {
                        MessageBox.Show("Erro de tarefas.");
                        App.Current.Shutdown();
                        return;
                    }

                    if (!LoadNotifs())
                    {
                        MessageBox.Show("Erro de notificações.");
                        App.Current.Shutdown();
                        return;
                    }


                    if (GPerfil.Password != null)
                    {
                        ChangeView("Login");

                    } else
                    {
                        StartPeriodicCheck();
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

            if (GPerfil == null)
            {
                MessageBox.Show("Erro: Perfil não encontrado.");
                return false;
            }

            try
            {
                Directory.CreateDirectory(basePath);


                XDocument doc = new XDocument(
                        new XElement("perfil",
                        new XAttribute("nome", GPerfil.Nome),
                        new XAttribute("email", GPerfil.Email)
                        )
                    );

                XElement? docPerf = doc.Element("perfil");

                if ((GPerfil.Password != null) && (docPerf != null))
                {
                    docPerf.Add(new XAttribute("password", GPerfil.Password));
                }

                doc.Save(System.IO.Path.Combine(basePath, "perfil.xml"));

                string photoPath = System.IO.Path.Combine(basePath, "pfp.png");

                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(GPerfil.Fotografia));
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

                    GPerfil = perf;
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
            if (GTarefas == null)
            {
                MessageBox.Show("Erro: Tarefas não encontradas.");
                return false;
            }

            if (GTarefas.Count == 0)
            {
                try
                {
                    if (File.Exists(System.IO.Path.Combine(basePath, "tarefas.xml")))
                    {
                        File.Delete(System.IO.Path.Combine(basePath, "tarefas.xml"));
                        return true;
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao apagar ficheiro de tarefas:\n\n" + ex.Message + "\n" + ex.StackTrace);
                    return false;
                }
            }

            try
            {
                Directory.CreateDirectory(basePath);

                XDocument doc = new XDocument(
                    new XElement("tarefas")
                    );

                XElement? docTarefas = doc.Element("tarefas");

                foreach (Tarefa t in GTarefas)
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
                MkPeriodicidades();

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

                if (GPrioridades == null)
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

                    if (GTarefas == null)
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


                        Prioridade? tPrio = GPrioridades.FirstOrDefault(p => p.Valor == int.Parse(docPrio.Value));

                        if (tPrio == null)
                        {
                            tPrio = new Prioridade()
                            {
                                Valor = int.Parse(docPrio.Value),
                                Cor = "#000000"
                            };
                            GPrioridades.Add(tPrio);
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

                        GTarefas.Add(tempTarefa);

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
                if (GPrioridades == null)
                {
                    MessageBox.Show("Erro: Prioridades não encontradas.");
                    return false;
                }

                if (GPrioridades.Count == 4)
                {
                    return true;
                }

                Directory.CreateDirectory(basePath);

                XDocument doc = new XDocument(
                    new XElement("prioridades")
                    );

                XElement? docPrioridades = doc.Element("prioridades");

                foreach (Prioridade p in GPrioridades)
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

                GPrioridades = tempPrioridades;
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
                if (GPeriodicidades == null)
                {
                    return false;
                }

                if (GPeriodicidades.Count == 0)
                {
                    return true;
                }

                Directory.CreateDirectory(basePath);

                XDocument doc = new XDocument(
                    new XElement("periodicidades")
                    );

                XElement? docPeriodicidades = doc.Element("periodicidades");

                foreach (Periodicidade pe in GPeriodicidades)
                {
                    if (pe == null || docPeriodicidades == null)
                    {
                        throw new Exception("Erro: periodicidade não inicializada ao guardar periodicidades");
                    }

                    docPeriodicidades.Add(
                        new XElement("periodicidade",
                            new XAttribute("ID", pe.ID),
                            new XAttribute("IDTarefaOriginal", pe.IDTarefaOriginal),
                            new XAttribute("DataOriginal", pe.DataOriginal),
                            new XAttribute("DataLimite", pe.DataLimite),
                            new XAttribute("Tipo", pe.Tipo),
                            new XAttribute("IntervaloRepeticao", pe.intervaloRepeticao)
                        )
                    );

                    XElement? docPeriodicidade = docPeriodicidades.Elements().Last() ?? throw new Exception("Erro ao guardar tarefa.");

                    if (pe.DiasSemana != null)
                    {
                        docPeriodicidade.Add(
                                new XAttribute("DiasSemana", string.Join(",", pe.DiasSemana))
                        );
                    }

                    if (pe.tipoMensal != null)
                    {
                        docPeriodicidade.Add(
                            new XAttribute("TipoMensal", pe.tipoMensal)
                        );
                    }

                    if (pe.tipoAnual != null)
                    {
                        docPeriodicidade.Add(
                            new XAttribute("TipoAnual", pe.tipoAnual)
                        );
                    }

                    if(pe.IDChildTarefas != null)
                    {
                        docPeriodicidade.Add(
                            new XAttribute("IDChildTarefas", string.Join(",", pe.IDChildTarefas))
                        );
                    }



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


        public bool LoadPeroidicidades()
        {
            string periodicidadesPath = System.IO.Path.Combine(basePath, "periodicidades.xml");
            try
            {

                if (GPeriodicidades == null)
                {
                    return false;
                }

                if (Path.Exists(periodicidadesPath))
                {

                    XDocument doc = XDocument.Load(periodicidadesPath);
                    XElement? docPeriodicidades = doc.Element("periodicidades");

                    if (docPeriodicidades == null)
                    {
                        MessageBox.Show("Ficheiro de periodicidades inválido.");
                        return false;
                    }

                    foreach (XElement pe in docPeriodicidades.Elements("periodicidade"))
                    {
                        if (pe == null)
                        {
                            MessageBox.Show("Erro: periodicidade não inicializada ao carregar periodicidades");
                            return false;
                        }

                        XAttribute? docID = pe.Attribute("ID");
                        XAttribute? docIDTarefaOriginal = pe.Attribute("IDTarefaOriginal");
                        XAttribute? docDataOriginal = pe.Attribute("DataOriginal");
                        XAttribute? docDataLimite = pe.Attribute("DataLimite");
                        XAttribute? docTipo = pe.Attribute("Tipo");
                        XAttribute? docIntervaloRepeticao = pe.Attribute("IntervaloRepeticao");

                        if (docID == null || docIDTarefaOriginal == null || docDataOriginal == null || docDataLimite == null || docTipo == null || docIntervaloRepeticao == null)
                        {
                            MessageBox.Show("Ficheiro de periodicidades inválido.");
                            return false;
                        }

                        Periodicidade tempPeriodicidade = new Periodicidade()
                        {
                            ID = docID.Value,
                            IDTarefaOriginal = docIDTarefaOriginal.Value,
                            DataOriginal = DateTime.Parse(docDataOriginal.Value),
                            DataLimite = DateTime.Parse(docDataLimite.Value),
                            Tipo = int.Parse(docTipo.Value),
                            intervaloRepeticao = int.Parse(docIntervaloRepeticao.Value)
                        };

                        XAttribute? docDiasSemana = pe.Attribute("DiasSemana");
                        XAttribute? docTipoMensal = pe.Attribute("TipoMensal");
                        XAttribute? docTipoAnual = pe.Attribute("TipoAnual");

                        if (docDiasSemana != null)
                        {
                            tempPeriodicidade.DiasSemana = docDiasSemana.Value.Split(",").Select(s => bool.Parse(s)).ToArray();
                        }

                        if (docTipoMensal != null)
                        {
                            tempPeriodicidade.tipoMensal = int.Parse(docTipoMensal.Value);
                        }

                        if (docTipoAnual != null)
                        {
                            tempPeriodicidade.tipoAnual = int.Parse(docTipoAnual.Value);
                        }

                        XAttribute? docIDChildTarefas = pe.Attribute("IDChildTarefas");

                        if (docIDChildTarefas != null)
                        {
                            tempPeriodicidade.IDChildTarefas = docIDChildTarefas.Value.Split(",").ToList();
                        }

                        GPeriodicidades.Add(tempPeriodicidade);

                    }
                    return true;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar periodicidades:\n\n" + ex.Message + "\n" + ex.StackTrace);
                return false;
            }
        }

        public bool SaveNotifs()
        {

            if (GNotificacoes == null)
            {
                MessageBox.Show("GNotificacoes não inicializado");
                return false;
            }



            if (GNotificacoes.Count == 0)
            {
                try
                {
                    if (File.Exists(System.IO.Path.Combine(basePath, "notifs.xml")))
                    {
                        File.Delete(System.IO.Path.Combine(basePath, "notifs.xml"));
                        return true;
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao apagar ficheiro de notificações:\n\n" + ex.Message + "\n" + ex.StackTrace);
                    return false;
                }
            }

            XDocument doc = new();
            doc.Add(new XElement("notifs"));
            XElement? docNotifs = doc.Element("notifs");

            foreach (Notificacao Notif in GNotificacoes)
            {
                if (Notif == null || docNotifs == null)
                {
                    throw new Exception("Erro: Notificação não inicializada.");
                }

                docNotifs.Add(new XElement("notif",
                        new XAttribute("mensagem", Notif.Mensagem),
                        new XAttribute("data", Notif.Data),
                        new XAttribute("lida", Notif.Lida),
                        new XAttribute("IDOriginal", Notif.IDOriginal),
                        new XAttribute("Tipo", Notif.Tipo)
                        ));
            }

            doc.Save(System.IO.Path.Combine(basePath, "notifs.xml"));
            return true;
        }



        public bool LoadNotifs()
        {
            string notifsPath = System.IO.Path.Combine(basePath, "notifs.xml");
            try
            {
                if (GNotificacoes == null)
                {
                    MessageBox.Show("GNotificacoes não inicializado");
                    return false;
                }

                if (Path.Exists(notifsPath))
                {
                    XDocument doc = XDocument.Load(notifsPath);
                    XElement? docNotifs = doc.Element("notifs");

                    if (docNotifs == null)
                    {
                        MessageBox.Show("Ficheiro de notificações inválido.");
                        return false;
                    }

                    foreach (XElement n in docNotifs.Elements("notif"))
                    {
                        if (n == null)
                        {
                            MessageBox.Show("Erro: notificação não inicializada ao carregar notificações");
                            return false;
                        }

                        XAttribute? docMensagem = n.Attribute("mensagem");
                        XAttribute? docData = n.Attribute("data");
                        XAttribute? docLida = n.Attribute("lida");
                        XAttribute? docIDOriginal = n.Attribute("IDOriginal");
                        XAttribute? docTipo = n.Attribute("Tipo");

                        if (docMensagem == null || docData == null || docLida == null || docIDOriginal == null || docTipo == null)
                        {
                            MessageBox.Show("Ficheiro de notificações inválido.");
                            return false;
                        }

                        Notificacao tempNotif = new Notificacao()
                        {
                            Mensagem = docMensagem.Value,
                            Data = DateTime.Parse(docData.Value),
                            Lida = bool.Parse(docLida.Value),
                            IDOriginal = docIDOriginal.Value,
                            Tipo = int.Parse(docTipo.Value)
                        };

                        GNotificacoes.Add(tempNotif);
                    }
                    return true;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar notificações:\n\n" + ex.Message + "\n" + ex.StackTrace);
                return false;
            }
        }





        private bool _isRunning;
        private SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        private async void StartPeriodicCheck()
        {
            _isRunning = true;
            while (_isRunning)
            {
                await Task.Delay(TimeSpan.FromMinutes(1));
                await CheckAsync();
                SaveTarefas();
            }
        }

        private async Task CheckAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                DateTime agora = DateTime.Now;

                if (GTarefas == null || GNotificacoes == null)
                {
                    return;
                }


                for (int i = 0; i < GTarefas.Count; i++)
                {
                    Tarefa t = GTarefas[i];
                    TimeSpan diff = t.DataInicio - agora;
                    TimeSpan pdiff = agora - t.DataInicio;

                    if (t.AlertaAntecipacao != null)
                    {
                        if (diff <= t.AlertaAntecipacao.Tempo && diff.TotalSeconds > 0 && t.Estado != 2 && t.Estado != -1)
                        {
                            
                            if (GNotificacoes.Any(n => n.IDOriginal == t.ID && n.Tipo == 1))
                            {
                                continue;
                            }

                            Notificacao notA = new Notificacao()
                            {
                                Mensagem = "A hora prevista para a tarefa '" + t.Titulo + "' está quase a chegar.",
                                Data = DateTime.Now,
                                Tipo = 1,
                                Lida = false,
                                IDOriginal = t.ID
                            };

                            GNotificacoes.Add(notA);

                            var WA = new WAlerta(notA);
                            WA.Show();
                            
                        }
                    }

                    if (t.AlertaAtraso != null)
                    {
                        if (pdiff >= t.AlertaAtraso.Tempo && pdiff.TotalSeconds > 0 && t.Estado != 2 && t.Estado != -1)
                        {
                            
                            if (GNotificacoes.Any(n => n.IDOriginal == t.ID && n.Tipo == 2))
                            {
                                continue;
                            }

                            Notificacao notE = new Notificacao()
                            {
                                Mensagem = "A hora prevista para a tarefa '" + t.Titulo + "' já passou.",
                                Data = DateTime.Now,
                                Tipo = 2,
                                Lida = false,
                                IDOriginal = t.ID
                            };

                            GNotificacoes.Add(notE);

                            var WA = new WAlerta(notE);
                            WA.Show();

                        }
                    }

                    SaveNotifs();

                }
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public void StopPeriodicCheck()
        {
            _isRunning = false;
        }

        public bool existeRepetida(string idOriginal, DateTime date)
        {
            if (GPeriodicidades == null || GTarefas == null)
            {
                return false;
            }

            var per = GPeriodicidades.FirstOrDefault(p => p.IDTarefaOriginal == idOriginal);
            if (per != null && per.IDChildTarefas != null)
            {
                foreach (string rid in per.IDChildTarefas)
                {
                    if (GTarefas.Any(t => t.ID == rid && t.DataInicio == date))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void MkPeriodicidades()
        {
            if (GTarefas == null)
            {
                return;
            }

            for (int i = 0; i < GTarefas.Count; i++)
            {
                Tarefa t = GTarefas[i];

                if (t.idPeriodicidade != null && t.IsTarefaOriginal)
                {

                    if (GPeriodicidades == null)
                    {
                        return;
                    }

                    var per = GPeriodicidades.FirstOrDefault(p => p.IDTarefaOriginal == t.ID && p.ID == t.idPeriodicidade);

                    if (per == null)
                    {
                        continue;
                    }

                    switch(per.Tipo) {

                        case 1: // diária
                            var interv = per.intervaloRepeticao;
                            DateTime dat = t.DataInicio.AddDays(interv);
                            while (dat <= per.DataLimite)
                            {
                                if (existeRepetida(t.ID, dat))
                                {
                                    dat = dat.AddDays(per.intervaloRepeticao);
                                    continue;
                                }

                                Tarefa nt = new Tarefa()
                                {
                                    Titulo = t.Titulo,
                                    CreationTime = t.CreationTime,
                                    DataInicio = dat,
                                    FullDia = t.FullDia,
                                    valorPrio = t.valorPrio,
                                    Estado = t.Estado,
                                    Descricao = t.Descricao,
                                    DataFim = t.DataFim,
                                    idPeriodicidade = t.idPeriodicidade,
                                    AlertaAntecipacao = t.AlertaAntecipacao,
                                    AlertaAtraso = t.AlertaAtraso,
                                    IsTarefaOriginal = false,
                                };

                                if (per.IDChildTarefas == null)
                                {
                                    return;
                                }
                                per.IDChildTarefas.Add(nt.ID);
                                GTarefas.Add(nt);

                                dat = dat.AddDays(interv);
                            }
                            break;
                        case 2: // semanal
                            var interv2 = per.intervaloRepeticao;
                            int nSemana = 0;
                            DateTime dat2 = t.DataInicio.AddDays(1);
                            while (dat2 <= per.DataLimite)
                            {
                                if (existeRepetida(t.ID, dat2))
                                {
                                    dat2 = dat2.AddDays(1);
                                    continue;
                                }

                                if (per.DiasSemana == null)
                                    return;


                                int dayOfWeek = (int)dat2.DayOfWeek;
                                    dayOfWeek = (dayOfWeek == 0) ? 6 : dayOfWeek - 1;

                                if (dayOfWeek == 0)
                                {
                                    nSemana++;
                                }

                                if (nSemana % interv2 != 0 || !per.DiasSemana[dayOfWeek])
                                {
                                    dat2 = dat2.AddDays(1);
                                    continue;
                                }

                                Tarefa nt2 = new Tarefa()
                                {
                                    Titulo = t.Titulo,
                                    CreationTime = t.CreationTime,
                                    DataInicio = dat2,
                                    FullDia = t.FullDia,
                                    valorPrio = t.valorPrio,
                                    Estado = t.Estado,
                                    Descricao = t.Descricao,
                                    DataFim = t.DataFim,
                                    idPeriodicidade = t.idPeriodicidade,
                                    AlertaAntecipacao = t.AlertaAntecipacao,
                                    AlertaAtraso = t.AlertaAtraso,
                                    IsTarefaOriginal = false,
                                };

                                if (per.IDChildTarefas == null)
                                {
                                    return;
                                }
                                per.IDChildTarefas.Add(nt2.ID);
                                GTarefas.Add(nt2);

                                dat2 = dat2.AddDays(1);
                            }
                            break;

                        case 3: // mensal
                            switch (per.tipoMensal)
                            {
                                case 1: // dia do mês
                                    var interv3 = per.intervaloRepeticao;
                                    DateTime dat3 = t.DataInicio.AddDays(1);
                                    int nMes = 0;
                                    while (dat3 <= per.DataLimite)
                                    {

                                        if (dat3.Day == 1)
                                        {
                                            nMes++;
                                        }

                                        if (dat3.Day != t.DataInicio.Day)
                                        {
                                            dat3 = dat3.AddDays(1);
                                            continue;
                                        }


                                        if (existeRepetida(t.ID, dat3))
                                        {
                                            dat3 = dat3.AddDays(1);
                                            continue;
                                        }

                                        if (nMes % interv3 != 0 )
                                        {
                                            dat3 = dat3.AddDays(1);
                                            continue;
                                        }


                                        Tarefa nt3 = new Tarefa()
                                        {
                                            Titulo = t.Titulo,
                                            CreationTime = t.CreationTime,
                                            DataInicio = dat3,
                                            FullDia = t.FullDia,
                                            valorPrio = t.valorPrio,
                                            Estado = t.Estado,
                                            Descricao = t.Descricao,
                                            DataFim = t.DataFim,
                                            idPeriodicidade = t.idPeriodicidade,
                                            AlertaAntecipacao = t.AlertaAntecipacao,
                                            AlertaAtraso = t.AlertaAtraso,
                                            IsTarefaOriginal = false,
                                        };

                                        if (per.IDChildTarefas == null)
                                        {
                                            return;
                                        }
                                        per.IDChildTarefas.Add(nt3.ID);
                                        GTarefas.Add(nt3);

                                        dat3 = dat3.AddDays(1);
                                    }
                                    break;

                                case 2: // dia da semana
                                    var interv4 = per.intervaloRepeticao;
                                    DateTime dat4 = t.DataInicio.AddDays(1);
                                    int nMes2 = 0;
                                    int WOM = HVPeriodicidade.WOM(t.DataInicio);
                                    while (dat4 <= per.DataLimite)
                                    {

                                        if (dat4.Day == 1)
                                        {
                                            nMes2++;
                                        }


                                        if (dat4.DayOfWeek != t.DataInicio.DayOfWeek)
                                        {
                                            dat4 = dat4.AddDays(1);
                                            continue;
                                        }

                                        if (existeRepetida(t.ID, dat4))
                                        {
                                            dat4 = dat4.AddDays(1);
                                            continue;
                                        }

                                        if (nMes2 % interv4 != 0)
                                        {
                                            dat4 = dat4.AddDays(1);
                                            continue;
                                        }

                                        if (WOM != HVPeriodicidade.WOM(dat4))
                                        {
                                            dat4 = dat4.AddDays(1);
                                            continue;
                                        }

                                        Tarefa nt4 = new Tarefa()
                                        {
                                            Titulo = t.Titulo,
                                            CreationTime = t.CreationTime,
                                            DataInicio = dat4,
                                            FullDia = t.FullDia,
                                            valorPrio = t.valorPrio,
                                            Estado = t.Estado,
                                            Descricao = t.Descricao,
                                            DataFim = t.DataFim,
                                            idPeriodicidade = t.idPeriodicidade,
                                            AlertaAntecipacao = t.AlertaAntecipacao,
                                            AlertaAtraso = t.AlertaAtraso,
                                            IsTarefaOriginal = false,
                                        };

                                        if (per.IDChildTarefas == null)
                                        {
                                            return;
                                        }
                                        per.IDChildTarefas.Add(nt4.ID);
                                        GTarefas.Add(nt4);

                                        dat4 = dat4.AddDays(1);
                                    }
                                    break;
                            }
                            break;

                        case 4:
                            switch (per.tipoAnual)
                            {
                                case 1: // dia do mês
                                    var interv5 = per.intervaloRepeticao;
                                    DateTime dat5 = t.DataInicio.AddDays(1);
                                    int nAno = 0;
                                    while (dat5 <= per.DataLimite)
                                    {

                                        if (dat5.Day == 1 && dat5.Month == 1)
                                        {
                                            nAno++;
                                        }

                                        if (dat5.Day != t.DataInicio.Day || dat5.Month != t.DataInicio.Month)
                                        {
                                            dat5 = dat5.AddDays(1);
                                            continue;
                                        }

                                        if (existeRepetida(t.ID, dat5))
                                        {
                                            dat5 = dat5.AddDays(1);
                                            continue;
                                        }

                                        if (nAno % interv5 != 0)
                                        {
                                            dat5 = dat5.AddDays(1);
                                            continue;
                                        }

                                        Tarefa nt5 = new Tarefa()
                                        {
                                            Titulo = t.Titulo,
                                            CreationTime = t.CreationTime,
                                            DataInicio = dat5,
                                            FullDia = t.FullDia,
                                            valorPrio = t.valorPrio,
                                            Estado = t.Estado,
                                            Descricao = t.Descricao,
                                            DataFim = t.DataFim,
                                            idPeriodicidade = t.idPeriodicidade,
                                            AlertaAntecipacao = t.AlertaAntecipacao,
                                            AlertaAtraso = t.AlertaAtraso,
                                            IsTarefaOriginal = false,
                                        };

                                        if (per.IDChildTarefas == null)
                                        {
                                            return;
                                        }
                                        per.IDChildTarefas.Add(nt5.ID);
                                        GTarefas.Add(nt5);

                                        dat5 = dat5.AddDays(1);
                                    }
                                    break;

                                case 2: // dia da semana
                                    var interv6 = per.intervaloRepeticao;
                                    DateTime dat6 = t.DataInicio.AddDays(1);
                                    int nAno2 = 0;
                                    int WOM = HVPeriodicidade.WOM(t.DataInicio);
                                    while (dat6 <= per.DataLimite)
                                    {

                                        if (dat6.Day == 1 && dat6.Month == 1)
                                        {
                                            nAno2++;
                                        }

                                        if (dat6.DayOfWeek != t.DataInicio.DayOfWeek || dat6.Month != t.DataInicio.Month)
                                        {
                                            dat6 = dat6.AddDays(1);
                                            continue;
                                        }

                                        if (existeRepetida(t.ID, dat6))
                                        {
                                            dat6 = dat6.AddDays(1);
                                            continue;
                                        }

                                        if (nAno2 % interv6 != 0)
                                        {
                                            dat6 = dat6.AddDays(1);
                                            continue;
                                        }

                                        if (WOM != HVPeriodicidade.WOM(dat6))
                                        {
                                            dat6 = dat6.AddDays(1);
                                            continue;
                                        }

                                        Tarefa nt6 = new Tarefa()
                                        {
                                            Titulo = t.Titulo,
                                            CreationTime = t.CreationTime,
                                            DataInicio = dat6,
                                            FullDia = t.FullDia,
                                            valorPrio = t.valorPrio,
                                            Estado = t.Estado,
                                            Descricao = t.Descricao,
                                            DataFim = t.DataFim,
                                            idPeriodicidade = t.idPeriodicidade,
                                            AlertaAntecipacao = t.AlertaAntecipacao,
                                            AlertaAtraso = t.AlertaAtraso,
                                            IsTarefaOriginal = false,
                                        };

                                        if (per.IDChildTarefas == null)
                                        {
                                            return;
                                        }
                                        per.IDChildTarefas.Add(nt6.ID);
                                        GTarefas.Add(nt6);

                                        dat6 = dat6.AddDays(1);
                                    }
                                    break;
                            }
                            break;

                    }

                }

            }
        }


    }
}
