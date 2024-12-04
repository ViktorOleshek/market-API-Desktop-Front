import { Component, Input, OnInit } from '@angular/core';
import { CustomerService } from '../../../shared/services/customer.service';
import { Customer } from '../../../shared/models/customer';
import { ActivatedRoute, Router } from '@angular/router';
import {FormsModule} from '@angular/forms';

@Component({
  selector: 'app-customer-detail',
  templateUrl: './customer-detail.component.html',
  imports: [
    FormsModule
  ],
  styleUrls: ['./customer-detail.component.css']
})
export class CustomerDetailComponent implements OnInit {
  @Input() customer: Customer = new Customer(0, '', '', new Date(), 0, []);

  constructor(
    private customerService: CustomerService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loadCustomer(Number(id));
    }
  }

  loadCustomer(id: number): void {
    this.customerService.getCustomerById(id).subscribe(
      (data) => {
        this.customer = data;
      },
      (error) => {
        console.error('Error loading customer:', error);
      }
    );
  }

  saveCustomer(): void {
    if (this.customer.id === 0) {
      this.addCustomer();
    } else {
      this.updateCustomer();
    }
  }

  addCustomer(): void {
    this.customerService.addCustomer(this.customer).subscribe(
      (data) => {
        console.log('Customer added successfully:', data);
        this.router.navigate(['/customers']);
      },
      (error) => {
        console.error('Failed to add customer:', error);
      }
    );
  }

  updateCustomer(): void {
    this.customerService.updateCustomer(this.customer.id, this.customer).subscribe(
      () => {
        console.log('Customer updated successfully');
        this.router.navigate(['/customers']);
      },
      (error) => {
        console.error('Failed to update customer:', error);
      }
    );
  }

  cancel(): void {
    this.router.navigate(['/customers']);
  }
}
