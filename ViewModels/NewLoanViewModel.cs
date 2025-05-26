using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CreditCalculator.ViewModels;
using p4_projektwpf.Models;

namespace p4_projektwpf.ViewModels
{
    class NewLoanViewModel : BaseViewModel
    {
        private readonly LibraryDbContext _context = new();

        public ObservableCollection<Book> AvailableBooks { get; set; } = new();
        public ObservableCollection<Client> Clients { get; set; } = new();

        private Book _selectedBook;
        public Book SelectedBook
        {
            get => _selectedBook;
            set { _selectedBook = value; OnPropertyChanged(); }
        }

        private Client _selectedClient;
        public Client SelectedClient
        {
            get => _selectedClient;
            set { _selectedClient = value; OnPropertyChanged(); }
        }

        public ICommand CreateLoanCommand { get; }

        public NewLoanViewModel()
        {
            LoadData();
            CreateLoanCommand = new RelayCommand(CreateLoan, CanCreateLoan);
        }

        private void LoadData()
        {
            AvailableBooks.Clear();
            foreach (var book in _context.Books.Where(b => b.Status == "Dostępna"))
                AvailableBooks.Add(book);

            Clients.Clear();
            foreach (var client in _context.Clients)
                Clients.Add(client);
        }

        private bool CanCreateLoan()
        {
            return SelectedBook != null && SelectedClient != null;
        }

        private void CreateLoan()
        {
            if (!CanCreateLoan()) return;

            var loan = new Loan
            {
                BookId = SelectedBook.Id,
                ClientId = SelectedClient.Id
            };

            SelectedBook.Status = "Wypożyczona";

            _context.Loans.Add(loan);
            _context.Books.Update(SelectedBook);
            _context.SaveChanges();

            LoadData();
            SelectedBook = null;
            SelectedClient = null;
        }
    }
}
