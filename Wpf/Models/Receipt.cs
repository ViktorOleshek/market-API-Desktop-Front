using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Wpf.Models
{
	public class Receipt : INotifyPropertyChanged
	{
		private int _receiptId;
		private int _customerId;
		private DateTime _operationDate;
		private bool _isCheckedOut;
		private ICollection<int> _receiptDetailsIds = new List<int>();

		public int ReceiptId
		{
			get { return _receiptId; }
			set
			{
				_receiptId = value;
				OnPropertyChanged(nameof(ReceiptId));
			}
		}

		public int CustomerId
		{
			get { return _customerId; }
			set
			{
				_customerId = value;
				OnPropertyChanged(nameof(CustomerId));
			}
		}

		public DateTime OperationDate
		{
			get { return _operationDate; }
			set
			{
				_operationDate = value;
				OnPropertyChanged(nameof(OperationDate));
			}
		}

		public bool IsCheckedOut
		{
			get { return _isCheckedOut; }
			set
			{
				_isCheckedOut = value;
				OnPropertyChanged(nameof(IsCheckedOut));
			}
		}

		public ICollection<int> ReceiptDetailsIds
		{
			get { return _receiptDetailsIds; }
			set
			{
				_receiptDetailsIds = value;
				OnPropertyChanged(nameof(ReceiptDetailsIds));
			}
		}

		public event PropertyChangedEventHandler? PropertyChanged;
		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
