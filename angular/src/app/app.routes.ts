import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HomeComponent } from './components/home/home.component';
import { CustomerListComponent } from './components/customer/customer-list/customer-list.component';
import { CustomerDetailComponent } from './components/customer/customer-detail/customer-detail.component';
import { ReceiptListComponent } from './components/receipt/receipt-list/receipt-list.component';
import { ReceiptDetailComponent } from './components/receipt/receipt-detail/receipt-detail.component';
import { LoginComponent } from './components/login/login.component';
import { AuthGuard } from './shared/guard/auth.guard';
import {NotFoundComponent} from './components/not-found/not-found.component';

export const routes: Routes = [
  // Public routes
  { path: 'login', component: LoginComponent },
  { path: 'home', component: HomeComponent },

  // Protected routes - only for authenticated users
  { path: 'customers', component: CustomerListComponent, canActivate: [AuthGuard] },
  { path: 'customers/:id', component: CustomerDetailComponent, canActivate: [AuthGuard] },
  { path: 'receipts', component: ReceiptListComponent, canActivate: [AuthGuard] },
  { path: 'receipts/:id', component: ReceiptDetailComponent, canActivate: [AuthGuard] },

  { path: '', redirectTo: '/home', pathMatch: 'full' }, // Головна сторінка
  { path: '**', component: NotFoundComponent }, // Невідомий маршрут
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
