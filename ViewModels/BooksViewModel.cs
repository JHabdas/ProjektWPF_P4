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
    public class BooksViewModel : BaseViewModel
    {
        private readonly LibraryDbContext _context;

        public ObservableCollection<Book> Books { get; set; } = new();
        private Book _selectedBook = new();
        public Book SelectedBook
        {
            get => _selectedBook;
            set { _selectedBook = value; OnPropertyChanged(); }
        }

        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }

        public BooksViewModel()
        {
            _context = new LibraryDbContext();
            LoadBooks();

            AddCommand = new RelayCommand(AddBook);
            UpdateCommand = new RelayCommand(UpdateBook, () => SelectedBook != null);
            DeleteCommand = new RelayCommand(DeleteBook, () => SelectedBook != null);
        }

        private void LoadBooks()
        {
            Books.Clear();
            foreach (var book in _context.Books.ToList())
                Books.Add(book);
        }

        private void AddBook()
        {
            _context.Books.Add(SelectedBook);
            _context.SaveChanges();
            LoadBooks();
            SelectedBook = new Book();
        }

        private void UpdateBook()
        {
            var bookInDb = _context.Books.Find(SelectedBook.Id);
            if (bookInDb != null)
            {
                bookInDb.Title = SelectedBook.Title;
                bookInDb.Author = SelectedBook.Author;
                bookInDb.Description = SelectedBook.Description;
                bookInDb.Year = SelectedBook.Year;
                bookInDb.Status = SelectedBook.Status;

                _context.SaveChanges();
                LoadBooks();
            }
        }


        private void DeleteBook()
        {
            _context.Books.Remove(SelectedBook);
            _context.SaveChanges();
            LoadBooks();
            SelectedBook = new Book();
        }
    }
}
