import { Component, OnInit } from '@angular/core';
import { ReceiptService } from '../../../shared/services/receipt.service';
import { Receipt } from '../../../shared/models/receipt';
import { Router } from '@angular/router';
import {CurrencyPipe, NgForOf, NgIf} from '@angular/common';
import {Customer} from '../../../shared/models/customer';
import {FormatDatePipe} from '../../../shared/pipes/format-date.pipe';
import {SortPipe} from '../../../shared/pipes/sort.pipe';
import { AuthService } from '../../../shared/services/auth.service';
import { CustomerService } from '../../../shared/services/customer.service';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-receipt-list',
  templateUrl: './receipt-list.component.html',
  imports: [
    NgForOf,
    FormatDatePipe,
    SortPipe,
    NgIf,
    ReactiveFormsModule,
    CurrencyPipe
  ],
  styleUrls: ['./receipt-list.component.css']
})
export class ReceiptListComponent implements OnInit  {
  paginatedReceipts: Receipt[] = [];
  receipts: (Receipt & { customerName?: string })[] = [];
  Math = Math;

  sortField: string = '';
  sortOrder: 'asc' | 'desc' = 'asc';

  showAddModal: boolean = false;
  addReceiptForm: FormGroup;

  currentPage: number = 1;
  itemsPerPage: number = 5;
  totalPages: number = 1;

  constructor(
    private receiptService: ReceiptService,
    private authService: AuthService,
    private customerService: CustomerService,
    private router: Router,
    private fb: FormBuilder
  ) {
    this.addReceiptForm = this.fb.group({
      operationDate: [new Date().toISOString().split('T')[0], Validators.required],
      isCheckedOut: [false]
    });
  }


  ngOnInit(): void {
    this.loadReceipts();
  }

  toggleSort(field: string): void {
    if (this.sortField === field) {
      this.sortOrder = this.sortOrder === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortField = field;
      this.sortOrder = 'asc';
    }
  }

  getSortIcon(field: string): string {
    if (this.sortField === field) {
      return this.sortOrder === 'asc'
        ? '<i class="bi bi-sort-alpha-up"></i>'
        : '<i class="bi bi-sort-alpha-down"></i>';
    }
    return '<i class="bi bi-sort"></i>';
  }

  updatePagination(): void {
    this.totalPages = Math.ceil(this.receipts.length / this.itemsPerPage);
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedReceipts = this.receipts.slice(startIndex, endIndex);
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.updatePagination();
    }
  }

  prevPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.updatePagination();
    }
  }

  loadReceipts(): void {
    const customerId = this.authService.getCustomerIdFromToken();

    if (!customerId) {
      console.error('Customer ID not found in token');
      this.router.navigate(['/login']);
      return;
    }

    this.receiptService.getReceiptsByCustomerId(customerId).subscribe(
      (data) => {
        this.customerService.getAllCustomers().subscribe(
          (customers) => {
            this.receipts = data.map(receipt => {
              const customer = customers.find(c => c.id === receipt.customerId);
              return {
                ...receipt,
                customerName: customer ? `${customer.name} ${customer.surname}` : 'Unknown Customer',
                totalPrice: 0
              };
            });

            this.loadReceiptPrices();
          },
          (error) => {
            console.error('Failed to load customers:', error);
            this.receipts = data.map(receipt => ({
              ...receipt,
              customerName: 'Unknown Customer',
              totalPrice: 0
            }));
            this.loadReceiptPrices();
          }
        );
      },
      (error) => {
        console.error('Failed to load receipts:', error);
      }
    );
  }

  private loadReceiptPrices(): void {
    this.receipts.forEach((receipt, index) => {
      this.receiptService.getReceiptById(receipt.id).subscribe(
        (detailedReceipt) => {
          // Отримуємо суму чека
          this.receiptService.getReceiptDetails(receipt.id).subscribe(
            (details) => {
              const totalPrice = details.reduce((sum, detail) =>
                sum + (detail.discountUnitPrice * detail.quantity), 0
              );
              this.receipts[index] = { ...this.receipts[index], totalPrice };
              this.updatePagination();
            },
            (error) => {
              console.error(`Failed to load details for receipt ${receipt.id}:`, error);
              this.receipts[index] = { ...this.receipts[index], totalPrice: 0 };
              this.updatePagination();
            }
          );
        },
        (error) => {
          console.error(`Failed to load receipt ${receipt.id}:`, error);
        }
      );
    });
  }

  editReceipt(id: number): void {
    this.router.navigate([`/receipts/${id}`]);
  }

  deleteReceipt(id: number): void {
    if (confirm('Are you sure you want to delete this receipt?')) {
      this.receiptService.deleteReceipt(id).subscribe(
        () => {
          console.log('Receipt deleted successfully');
          this.loadReceipts();
          this.updatePagination();
        },
        (error) => {
          console.error('Failed to delete receipt:', error);
        }
      );
    }
  }

  addReceipt(): void {
    this.showAddModal = true;
  }

// Додайте нові методи
  closeAddModal(): void {
    this.showAddModal = false;
    this.addReceiptForm.reset({
      operationDate: new Date().toISOString().split('T')[0],
      isCheckedOut: false
    });
  }

  submitNewReceipt(): void {
    if (this.addReceiptForm.valid) {
      const customerId = this.authService.getCustomerIdFromToken();

      if (!customerId) {
        console.error('Customer ID not found in token');
        return;
      }

      const formValue = this.addReceiptForm.value;
      const newReceipt = new Receipt(
        0,
        customerId,
        new Date(formValue.operationDate),
        formValue.isCheckedOut
      );

      this.receiptService.addReceipt(newReceipt).subscribe(
        (createdReceipt) => {
          console.log('Receipt created successfully:', createdReceipt);
          this.loadReceipts();
          this.closeAddModal();
          this.showSuccessMessage('Order created successfully!');
        },
        (error) => {
          console.error('Failed to create receipt:', error);
          this.showErrorMessage('Failed to create order. Please try again.');
        }
      );
    }
  }

// Додайте допоміжні методи для показу повідомлень
  private showSuccessMessage(message: string): void {
    alert(message); // Можна замінити на toast notification
  }

  private showErrorMessage(message: string): void {
    alert(message); // Можна замінити на toast notification
  }
}
