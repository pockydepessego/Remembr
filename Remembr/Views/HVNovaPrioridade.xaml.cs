using Remembr.Models;
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
    /// Interaction logic for HVNovaPrioridade.xaml
    /// </summary>
    public partial class HVNovaPrioridade : UserControl
    {

        public int? selectedPrio;
        public int? valor;
        SolidColorBrush? blackOutline = new BrushConverter().ConvertFromString("#3F3F3F") as SolidColorBrush;
        SolidColorBrush redOutline = Brushes.Red;
        MainVM MVM;
        public HVNovaPrioridade()
        {
            InitializeComponent();
            MVM = (MainVM)Application.Current.MainWindow.DataContext;
            
            combo.Items.Add("Nova prioridade");

            if (MVM.GPrioridades == null) return;

            var sortedPrioridades = MVM.GPrioridades.OrderBy(x => x.Valor).ToList();
            foreach (var item in sortedPrioridades)
            {
                if (new List<int> { 100, 200, 300, 400 }.Contains(item.Valor))
                {
                    continue;
                }
                    
                combo.Items.Add(item.Valor);
            }

            combo.SelectedIndex = 0;
            gridnovaprio.IsEnabled = true;


        }

        public HVNovaPrioridade(int valorPrio)
        {
            InitializeComponent();
            MVM = (MainVM)Application.Current.MainWindow.DataContext;

            combo.Items.Add("Nova prioridade");

            if (MVM.GPrioridades == null) return;

            var sortedPrioridades = MVM.GPrioridades.OrderBy(x => x.Valor).ToList();
            foreach (var item in sortedPrioridades)
            {
                if (new List<int> { 100, 200, 300, 400 }.Contains(item.Valor))
                {
                    continue;
                }

                combo.Items.Add(item.Valor);
            }

            combo.SelectedIndex = 0;
            gridnovaprio.IsEnabled = true;




            if (new List<int> { 100, 200, 300, 400 }.Contains(valorPrio))
            {
                combo.SelectedIndex = 0;
                gridnovaprio.IsEnabled = true;
            } else
            {
                combo.SelectedItem = valorPrio;
                gridnovaprio.IsEnabled = false;
            }

        }


        private void combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (combo.SelectedItem == null || MVM.GPrioridades == null)
            {
                return;
            }

            if (combo.SelectedIndex == 0)
            {
                selectedPrio = -100;
                gridnovaprio.IsEnabled = true;

                if (valorp.Value == null || MVM.GPrioridades == null)
                {
                    return;
                }
                if (MVM.GPrioridades.Any(item => item.Valor == valorp.Value))
                {
                    rectPrio.Stroke = redOutline;
                }
                else
                {
                    rectPrio.Stroke = blackOutline;
                }

            }
            else
            {
                selectedPrio = (int)combo.SelectedItem;
                gridnovaprio.IsEnabled = false;

                valorp.Value = (int)combo.SelectedItem;
                var prio = MVM.GPrioridades.FirstOrDefault(x => x.Valor == (int)combo.SelectedItem);
                if (prio != null)
                colorp.SelectedColor = (Color)ColorConverter.ConvertFromString(prio.Cor);

                
            }
        }

        private void valorp_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (valorp.Value == null || MVM.GPrioridades == null)
            {
                return;
            }
            if (MVM.GPrioridades.Any(item => item.Valor == valorp.Value) && gridnovaprio.IsEnabled == true)
            {
                rectPrio.Stroke = redOutline;
            } else
            {
                rectPrio.Stroke = blackOutline;
            }

            valor = valorp.Value;
        }
    }
}
