import { Component, OnInit } from '@angular/core';
import { CustomerService } from '../../../shared/services/customer.service';
import { Customer } from '../../../shared/models/customer';
import { Router } from '@angular/router';
import {NgForOf} from '@angular/common';
import {FormatDatePipe} from '../../../shared/pipes/format-date.pipe';
import {SortPipe} from '../../../shared/pipes/sort.pipe';

@Component({
  selector: 'app-customer-list',
  templateUrl: './customer-list.component.html',
  imports: [
    NgForOf,
    FormatDatePipe,
    SortPipe,
  ],
  styleUrls: ['./customer-list.component.css']
})
export class CustomerListComponent implements OnInit {
  customers: Customer[] = [];
  paginatedCustomers: Customer[] = [];

  sortField: string = '';
  sortOrder: 'asc' | 'desc' = 'asc';

  currentPage: number = 1;
  itemsPerPage: number = 5;
  totalPages: number = 1;

  constructor(
    private customerService: CustomerService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.customerService.getAllCustomers().subscribe(
      (data) => {
        this.customers = data;
        this.updatePagination();
      },
      (error) => {
        console.error('Failed to load customers:', error);
      }
    );
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
    this.totalPages = Math.ceil(this.customers.length / this.itemsPerPage);
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedCustomers = this.customers.slice(startIndex, endIndex);
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

  editCustomer(id: number): void {
    this.router.navigate([`/customers/${id}`]);
  }

  deleteCustomer(id: number): void {
    this.customerService.deleteCustomer(id).subscribe(
      () => {
        this.customers = this.customers.filter(customer => customer.id !== id);
        this.updatePagination();
      },
      (error) => {
        console.error('Failed to delete customer:', error);
      }
    );
  }

  addCustomer(): void {
    this.router.navigate(['/customers/add']);
  }
}
