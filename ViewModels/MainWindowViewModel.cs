using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CreditCalculator.ViewModels;

namespace p4_projektwpf.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public ICommand ShowBooksViewCommand { get; }
        public ICommand ShowClientsViewCommand { get; }
        public ICommand ShowLoansViewCommand { get; }
        public ICommand ShowNewLoanViewCommand { get; }

        public object CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }
        private object _currentView;

        private readonly BooksViewModel _booksVM = new();
        private readonly ClientsViewModel _clientsVM = new();
        private readonly LoansViewModel _loansVM = new();
        private NewLoanViewModel _newLoanVM;

        public MainWindowViewModel()
        {
            ShowBooksViewCommand = new RelayCommand(() => CurrentView = _booksVM);
            ShowClientsViewCommand = new RelayCommand(() => CurrentView = _clientsVM);
            ShowLoansViewCommand = new RelayCommand(() => CurrentView = _loansVM);
            ShowNewLoanViewCommand = new RelayCommand(() =>
            {
                _newLoanVM = new NewLoanViewModel(); 
                CurrentView = _newLoanVM;
            });

            CurrentView = _booksVM; 
        }
    }
}
