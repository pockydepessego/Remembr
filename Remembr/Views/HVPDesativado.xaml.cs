using Remembr.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interaction logic for HVPDesativado.xaml
    /// </summary>
    public partial class HVPDesativado : UserControl
    {
        public DateTime? dataInicial;
        public HVPDesativado()
        {
            InitializeComponent();
            MessageBox.Show("view periodicidade desativada iniciada sem datetime");
            
        }

        public HVPDesativado(DateTime dataInicial)
        {
            InitializeComponent();
            desativado.Text = "Ocorre apenas no dia " + dataInicial.ToString("dd/MM/yyyy");

        }
    }
}
