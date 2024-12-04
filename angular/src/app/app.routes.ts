import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { CustomerListComponent } from './components/customer/customer-list/customer-list.component';
import { CustomerDetailComponent } from './components/customer/customer-detail/customer-detail.component';
import { ReceiptListComponent } from './components/receipt/receipt-list/receipt-list.component';
import { ReceiptDetailComponent } from './components/receipt/receipt-detail/receipt-detail.component';

export const routes: Routes = [
  {
    path: 'customers',
    component: CustomerListComponent,
  },
  {
    path: 'customers/:id',
    component: CustomerDetailComponent,
  },
  {
    path: 'receipts',
    component: ReceiptListComponent,
  },
  {
    path: 'receipts/:id',
    component: ReceiptDetailComponent,
  },
  {
    path: '', // Головна сторінка
    redirectTo: '/receipts',
    pathMatch: 'full',
  },
  {
    path: '**', // Невідомий маршрут
    redirectTo: '/customers',
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
