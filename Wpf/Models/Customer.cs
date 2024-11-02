using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Wpf.Models
{
	public class Customer : INotifyPropertyChanged
	{
		private int _customerId;
		private string _name = null!;
		private string _surname = null!;
		private DateTime _birthDate;
		private int _discountValue;
		private ICollection<int> _receiptsIds = new List<int>();

		public int CustomerId
		{
			get { return _customerId; }
			set
			{
				_customerId = value;
				OnPropertyChanged(nameof(CustomerId));
			}
		}

		public string Name
		{
			get { return _name; }
			set
			{
				_name = value;
				OnPropertyChanged(nameof(Name));
			}
		}

		public string Surname
		{
			get { return _surname; }
			set
			{
				_surname = value;
				OnPropertyChanged(nameof(Surname));
			}
		}

		public DateTime BirthDate
		{
			get { return _birthDate; }
			set
			{
				_birthDate = value;
				OnPropertyChanged(nameof(BirthDate));
			}
		}

		public int DiscountValue
		{
			get { return _discountValue; }
			set
			{
				_discountValue = value;
				OnPropertyChanged(nameof(DiscountValue));
			}
		}

		public ICollection<int> ReceiptsIds
		{
			get { return _receiptsIds; }
			set
			{
				_receiptsIds = value;
				OnPropertyChanged(nameof(ReceiptsIds));
			}
		}

		public event PropertyChangedEventHandler? PropertyChanged;
		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
