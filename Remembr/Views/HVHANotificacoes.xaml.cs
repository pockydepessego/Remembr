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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Remembr.Views
{
    /// <summary>
    /// Interaction logic for HVHANotificacoes.xaml
    /// </summary>
    public partial class HVHANotificacoes : UserControl
    {
        MainVM MVM;
        public HVHANotificacoes()
        {
            
            InitializeComponent();
            MVM = (MainVM)Application.Current.MainWindow.DataContext;


            var listaCC = new List<UserControl>();

            if (MVM.GNotificacoes == null)
            {
                MessageBox.Show("Erro tarefas");
                return;
            }

            var listaOrdenada = MVM.GNotificacoes.OrderByDescending(t => t.Data).ToList();

            foreach (var notificacao in listaOrdenada)
            {
                var cc = new HVNotificacao(notificacao);
                cc.DataContext = notificacao;
                listaCC.Add(cc);
            }

            listinha.ItemsSource = listaCC;

        }

        private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
