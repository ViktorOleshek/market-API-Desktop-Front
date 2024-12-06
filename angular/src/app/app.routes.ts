import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { CustomerListComponent } from './components/customer/customer-list/customer-list.component';
import { CustomerDetailComponent } from './components/customer/customer-detail/customer-detail.component';
import { ReceiptListComponent } from './components/receipt/receipt-list/receipt-list.component';
import { ReceiptDetailComponent } from './components/receipt/receipt-detail/receipt-detail.component';
import { LoginComponent } from './components/login/login.component';
import { AuthGuard } from './shared/guard/auth.guard';

export const routes: Routes = [
  { path: 'customers', component: CustomerListComponent, canActivate: [AuthGuard] },
  { path: 'customers/:id', component: CustomerDetailComponent, canActivate: [AuthGuard] },
  { path: 'receipts', component: ReceiptListComponent, canActivate: [AuthGuard] },
  { path: 'receipts/:id', component: ReceiptDetailComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent },
  { path: '', redirectTo: '/login', pathMatch: 'full' }, // Головна сторінка
  { path: '**', redirectTo: '/login', pathMatch: 'full' }, // Невідомий маршрут
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
