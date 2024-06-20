using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.Input;


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

        public void ChangeView(string viewName)
        {
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


                default:
                    MessageBox.Show("Erro: View não encontrada.");
                    return;
            }
        }


        public MainVM()
        {

#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
            ChangeViewCommand = new RelayCommand<string>(ChangeView);
#pragma warning restore CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).


            ChangeView("NovaTarefa");


        }

    }
}
