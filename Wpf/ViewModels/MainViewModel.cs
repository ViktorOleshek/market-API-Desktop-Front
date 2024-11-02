using System;
using System.Net.Http;
using Wpf.ViewModels;

namespace WPF.ViewModels
{
	public class MainViewModel
	{
		public CustomerViewModel CustomerViewModel { get; set; }
		public ReceiptViewModel ReceiptViewModel { get; set; }

		public MainViewModel()
		{
			var httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:5001/") };

			CustomerViewModel = new CustomerViewModel();
			ReceiptViewModel = new ReceiptViewModel();
		}
	}
}