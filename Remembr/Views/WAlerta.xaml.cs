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
using System.Windows.Shapes;

namespace Remembr.Views
{
    /// <summary>
    /// Interaction logic for Alerta.xaml
    /// </summary>
    /// 

    public partial class WAlerta : Window
    {
        public WAlerta()
        {
            InitializeComponent();
        }

        public WAlerta(Models.Notificacao n)
        {
            InitializeComponent();
            
            if (n == null)
            {
                MessageBox.Show("Erro ao carregar notificação");
                return;
            }
            mensagem.Text = n.Mensagem;


        }
    }
}
