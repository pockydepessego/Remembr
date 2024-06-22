using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
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
    /// Interaction logic for HVPSemanal.xaml
    /// </summary>
    public partial class HVPSemanal : UserControl
    {
        public HVPSemanal()
        {
            InitializeComponent();

            if (nSemanas.Value == 1)
            {
                plural.Text = "Semana";

                ate.Text = "Ocorre todas as semanas " + diasDaSemana() + "até";
            }
            else
            {
                plural.Text = "Semanas";
                ate.Text = "Ocorre de " + nSemanas.Value + " em " + nSemanas.Value + " semanas " + diasDaSemana() + "até";
            }

        }

        private bool[] lDias = [false, false, false, false, false, false, false];

        private void BotaoSemanal(object sender, RoutedEventArgs e)
        {
            if (SEGUNDA.IsChecked == true)
            {
                lDias[0] = true;
            }
            else
            {
                lDias[0] = false;
            }

            if (TERCA.IsChecked == true)
            {
                lDias[1] = true;
            }
            else
            {
                lDias[1] = false;
            }

            if (QUARTA.IsChecked == true)
            {
                lDias[2] = true;
            }
            else
            {
                lDias[2] = false;
            }

            if (QUINTA.IsChecked == true)
            {
                lDias[3] = true;
            }
            else
            {
                lDias[3] = false;
            }

            if (SEXTA.IsChecked == true)
            {
                lDias[4] = true;
            }
            else
            {
                lDias[4] = false;
            }

            if (SABADO.IsChecked == true)
            {
                lDias[5] = true;
            }
            else
            {
                lDias[5] = false;
            }

            if (DOMINGO.IsChecked == true)
            {
                lDias[6] = true;
            }
            else
            {
                lDias[6] = false;
            }

            if (nSemanas.Value == null || plural == null || ate == null)
            {
                return;
            }
            if (nSemanas.Value == 1)
            {
                plural.Text = "Semana";

                ate.Text = "Ocorre todas as semanas " + diasDaSemana() + "até";
            }
            else
            {
                plural.Text = "Semanas";
                ate.Text = "Ocorre de " + nSemanas.Value + " em " + nSemanas.Value + " semanas " + diasDaSemana() + "até";
            }


        }


        private string diasDaSemana()
        {
            string dias = "";
            for (int i = 0; i < lDias.Length; i++)
            {
                if (lDias[i] == true)
                {
                    switch (i)
                    {
                        case 0:
                            dias += "segundas";
                            break;
                        case 1:
                            if (dias != "")
                            {
                                dias += ", ";
                            }
                            dias += "terças";
                            break;
                        case 2:
                            if (dias != "")
                            {
                                dias += ", ";
                            }
                            dias += "quartas";
                            break;
                        case 3:
                            if (dias != "")
                            {
                                dias += ", ";
                            }
                            dias += "quintas";
                            break;
                        case 4:
                            if (dias != "")
                            {
                                dias += ", ";
                            }
                            dias += "sextas";
                            break;
                        case 5:
                            if (dias != "")
                            {
                                dias += ", ";
                            }
                            dias += "sábados";
                            break;
                        case 6:
                            if (dias != "")
                            {
                                dias += ", ";
                            }
                            dias += "domingos";
                            break;
                    }

                    
                }
            }

            if (dias != "")
            {
                dias = "("+dias+") ";
            } else if (dias == "")
            {
                dias = "(SELECIONAR DIAS) ";
            }

            return dias;
        }

        private void nSemanas_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (nSemanas.Value == null || plural == null || ate == null)
            {
                return;
            }
            if (nSemanas.Value == 1)
            {
                plural.Text = "Semana";

                ate.Text = "Ocorre todas as semanas " + diasDaSemana() + "até";
            }
            else
            {
                plural.Text = "Semanas";
                ate.Text = "Ocorre de " + nSemanas.Value + " em " + nSemanas.Value + " semanas " + diasDaSemana() +  "até";
            }

        }

    }
}
