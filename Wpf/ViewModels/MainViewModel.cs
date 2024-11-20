using System;
using System.Net.Http;
using Wpf.ViewModels;

namespace Wpf.ViewModels
{
	public class MainViewModel
	{
		public CustomerViewModel CustomerViewModel { get; set; }
		public ReceiptViewModel ReceiptViewModel { get; set; }

		public MainViewModel()
		{
			CustomerViewModel = new CustomerViewModel();
			ReceiptViewModel = new ReceiptViewModel();
		}
	}
}