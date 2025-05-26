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
    public class ClientsViewModel : BaseViewModel
    {
        private readonly LibraryDbContext _context;

        public ObservableCollection<Client> Clients { get; set; } = new();
        private Client _selectedClient = new();
        public Client SelectedClient
        {
            get => _selectedClient;
            set { _selectedClient = value; OnPropertyChanged(); }
        }

        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }

        public ClientsViewModel()
        {
            _context = new LibraryDbContext();
            LoadClients();

            AddCommand = new RelayCommand(AddClient);
            UpdateCommand = new RelayCommand(UpdateClient, () => SelectedClient != null);
            DeleteCommand = new RelayCommand(DeleteClient, () => SelectedClient != null);
        }

        private void LoadClients()
        {
            Clients.Clear();
            foreach (var client in _context.Clients.ToList())
                Clients.Add(client);
        }

        private void AddClient()
        {
            _context.Clients.Add(SelectedClient);
            _context.SaveChanges();
            LoadClients();
            SelectedClient = new Client();
        }

        private void UpdateClient()
        {
            var clientInDb = _context.Clients.Find(SelectedClient.Id);
            if (clientInDb != null)
            {
                clientInDb.Name = SelectedClient.Name;
                clientInDb.Surname = SelectedClient.Surname;
                clientInDb.Email = SelectedClient.Email;

                _context.SaveChanges();
                LoadClients();
            }
        }


        private void DeleteClient()
        {
            _context.Clients.Remove(SelectedClient);
            _context.SaveChanges();
            LoadClients();
            SelectedClient = new Client();
        }
    }
}
