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
using System.Windows.Input.Manipulations;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Remembr.Views
{
    /// <summary>
    /// Interaction logic for HVHTPorIniciar.xaml
    /// </summary>
    public partial class HVHTPorIniciar : UserControl
    {
        MainVM MVM;
        public HVHTPorIniciar()
        {
            InitializeComponent();
            MVM = (MainVM)Application.Current.MainWindow.DataContext;


            var listaCC = new List<UserControl>();

            if (MVM.GTarefas == null) {
                MessageBox.Show("Erro tarefas");
                return;
            }

            foreach (var tarefa in MVM.GTarefas)
            {
                var cc = new HVTarefa();
                cc.DataContext = tarefa;
                listaCC.Add(cc);
            }
            
            listinha.ItemsSource = listaCC;


        }
    }
}
