using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CreditCalculator.ViewModels;
using Microsoft.EntityFrameworkCore;
using p4_projektwpf.Models;

namespace p4_projektwpf.ViewModels
{
    public class LoansViewModel : BaseViewModel
    {
        private readonly LibraryDbContext _context = new();

        public ObservableCollection<Loan> Loans { get; set; } = new();
        public ObservableCollection<Book> Books { get; set; } = new();
        public ObservableCollection<Client> Clients { get; set; } = new();

        private Loan _selectedLoan;
        public Loan SelectedLoan
        {
            get => _selectedLoan;
            set
            {
                _selectedLoan = value;
                OnPropertyChanged();

                if (value != null)
                {
                    SelectedBook = Books.FirstOrDefault(b => b.Id == value.BookId);
                    SelectedClient = Clients.FirstOrDefault(c => c.Id == value.ClientId);
                }
            }
        }

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

        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }

        public LoansViewModel()
        {
            Books = new ObservableCollection<Book>();
            Clients = new ObservableCollection<Client>();

            LoadStaticData();  
            LoadData();        

            UpdateCommand = new RelayCommand(UpdateLoan, () => SelectedLoan != null && SelectedBook != null && SelectedClient != null);
            DeleteCommand = new RelayCommand(DeleteLoan, () => SelectedLoan != null);
        }
        private void LoadStaticData()
        {
            Books.Clear();
            foreach (var book in _context.Books.ToList())
                Books.Add(book);

            Clients.Clear();
            foreach (var client in _context.Clients.ToList())
                Clients.Add(client);

            OnPropertyChanged(nameof(Books));
            OnPropertyChanged(nameof(Clients));
        }

        private void LoadData()
        {
            Loans.Clear();
            foreach (var loan in _context.Loans.Include(l => l.Book).Include(l => l.Client).ToList())
                Loans.Add(loan);
        }

        private void UpdateLoan()
        {
            if (SelectedLoan == null || SelectedBook == null || SelectedClient == null)
                return;

            var loanInDb = _context.Loans.Find(SelectedLoan.Id);
            if (loanInDb != null)
            {
                if (loanInDb.BookId != SelectedBook.Id)
                {
                    var oldBook = _context.Books.Find(loanInDb.BookId);
                    if (oldBook != null)
                    {
                        oldBook.Status = "Dostępna";
                        _context.Books.Update(oldBook);
                    }

                    var newBook = _context.Books.Find(SelectedBook.Id);
                    if (newBook != null)
                    {
                        newBook.Status = "Wypożyczona";
                        _context.Books.Update(newBook);
                    }

                    loanInDb.BookId = SelectedBook.Id;
                }

                loanInDb.ClientId = SelectedClient.Id;

                _context.Loans.Update(loanInDb);
                _context.SaveChanges();
                LoadData();
            }
        }


        private void DeleteLoan()
        {
            if (SelectedLoan == null) return;

            var book = _context.Books.Find(SelectedLoan.BookId);
            if (book != null)
            {
                book.Status = "Dostępna";
                _context.Books.Update(book);
            }

            _context.Loans.Remove(SelectedLoan);
            _context.SaveChanges();
            LoadData();
        }

    }
}
