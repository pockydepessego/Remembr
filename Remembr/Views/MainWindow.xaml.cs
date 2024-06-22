using Remembr.Views;
using Remembr.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Remembr.ViewModels;
using System.ComponentModel.DataAnnotations;


namespace Remembr
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainVM MVM;
        public MainWindow()
        {
            InitializeComponent();
            MVM = (MainVM)Application.Current.MainWindow.DataContext;
        }


        private void CloseApp_button(object sender, MouseButtonEventArgs e)
        {
            MVM.SavePerfil();
            MVM.SavePrioridades();


            Models.Alerta aa = new Models.Alerta() { 
                Email = true,
                Windows = true,
                Tempo = TimeSpan.FromSeconds(60)
            };

            Models.Alerta ba = new Models.Alerta()
            {
                Email = true,
                Windows = true,
                Tempo = TimeSpan.FromSeconds(120)
            };


            MVM.gTarefas.Add(new Tarefa()
            {

                Titulo = "Teste",
                Descricao = "Teste",
                CreationTime = System.DateTime.Now,
                DataInicio = System.DateTime.Now.AddDays(2),
                DataFim = System.DateTime.Now.AddDays(3),
                valorPrio = 460,
                FullDia = false,
                Estado = 0,
                IsTarefaOriginal = true,
                AlertaAntecipacao = aa,
                AlertaAtraso = ba,
                idPeriodicidade = "testeper"
            }); ;

            var pila = MVM.gTarefas;
            var pilinha = MVM.gPrioridades;
            MVM.SaveTarefas();


            Close();
        }

        private void HideApp_button(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }


        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void BigApp_button(object sender, MouseButtonEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
                BorderThickness = new Thickness(6);
                MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
                MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            }
            else
            {
                WindowState = WindowState.Normal;
                BorderThickness = new Thickness(2);
            }
        }


    }
}