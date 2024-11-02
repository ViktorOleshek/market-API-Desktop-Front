using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Wpf.Command;
using Wpf.Models;
using Wpf.Views;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Text;

namespace Wpf.ViewModels
{
	public class CustomerViewModel : INotifyPropertyChanged
	{
		private readonly HttpClient _httpClient;
		public ObservableCollection<Customer> Customers { get; set; }
		private Customer? _selectedCustomer;

		public Customer? SelectedCustomer
		{
			get => _selectedCustomer;
			set
			{
				_selectedCustomer = value;
				OnPropertyChanged(nameof(SelectedCustomer));
			}
		}

		public ICommand AddCommand { get; }
		public ICommand DeleteCommand { get; }
		public ICommand UpdateCommand { get; }
		public ICommand SaveCommand { get; }
		public ICommand CancelCommand { get; }

		public CustomerViewModel()
		{
			_httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:5001/") };
			Customers = new ObservableCollection<Customer>();
			LoadCustomersAsync();

			AddCommand = new RelayCommand(OpenAddCustomerWindow);
			DeleteCommand = new RelayCommand(DeleteCustomer, () => SelectedCustomer != null);
			UpdateCommand = new RelayCommand(OpenUpdateCustomerWindow, () => SelectedCustomer != null);
			SaveCommand = new RelayCommand(SaveCustomer);
			CancelCommand = new RelayCommand(Cancel);
		}

		private async void LoadCustomersAsync()
		{
			var response = await _httpClient.GetStringAsync("https://localhost:5001/api/customers");
			var customers = JsonConvert.DeserializeObject<List<Customer>>(response);
			foreach (var customer in customers)
			{
				Customers.Add(customer);
			}
		}

		private void Cancel()
		{
			CloseCustomerDetailsWindow();
		}

		private async void SaveCustomer()
		{
			if (SelectedCustomer != null)
			{
				var json = JsonConvert.SerializeObject(SelectedCustomer);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				if (SelectedCustomer.CustomerId == 0) // New customer
				{
					await _httpClient.PostAsync("api/customers", content);
					Customers.Add(SelectedCustomer);
				}
				else // Update existing customer
				{
					await _httpClient.PutAsync($"api/customers/{SelectedCustomer.CustomerId}", content);
				}
			}
			CloseCustomerDetailsWindow();
			SelectedCustomer = null;
		}

		private void CloseCustomerDetailsWindow()
		{
			var window = Application.Current.Windows.OfType<CustomerDetailsView>().FirstOrDefault();
			window?.Close();
		}

		private void OpenUpdateCustomerWindow()
		{
			OpenCustomerDetailsWindow();
		}

		private void OpenAddCustomerWindow()
		{
			SelectedCustomer = new Customer();
			OpenCustomerDetailsWindow();
		}

		private void OpenCustomerDetailsWindow()
		{
			var customerDetailsWindow = new CustomerDetailsView
			{
				DataContext = this
			};
			customerDetailsWindow.ShowDialog();
		}

		private async void DeleteCustomer()
		{
			if (SelectedCustomer != null)
			{
				await _httpClient.DeleteAsync($"api/customers/{SelectedCustomer.CustomerId}");
				Customers.Remove(SelectedCustomer);
			}
		}

		public event PropertyChangedEventHandler? PropertyChanged;
		protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
