import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Receipt } from '../models/receipt';
import {ReceiptDetail} from '../models/receipt-detail';

@Injectable({
  providedIn: 'root',
})
export class ReceiptService {
  private readonly apiUrl = `${environment.apiUrl}/receipts`;

  constructor(private http: HttpClient) {}

  getAllReceipts(): Observable<Receipt[]> {
    return this.http.get<Receipt[]>(this.apiUrl);
  }

  // Додайте цей метод до класу ReceiptService
  getReceiptsByCustomerId(customerId: number): Observable<Receipt[]> {
    return this.http.get<Receipt[]>(`${this.apiUrl}/customer/${customerId}`);
  }

  getReceiptById(id: number): Observable<Receipt> {
    return this.http.get<Receipt>(`${this.apiUrl}/${id}`);
  }

  addReceipt(receipt: Receipt): Observable<Receipt> {
    return this.http.post<Receipt>(this.apiUrl, receipt);
  }

  getReceiptDetails(receiptId: number): Observable<ReceiptDetail[]> {
    return this.http.get<ReceiptDetail[]>(`${this.apiUrl}/${receiptId}/details`);
  }

  addProductToReceipt(receiptId: number, productId: number, quantity: number): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${receiptId}/products/add/${productId}/${quantity}`, {});
  }

  removeProductFromReceipt(receiptId: number, productId: number, quantity: number): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${receiptId}/products/remove/${productId}/${quantity}`, {});
  }

  checkOutReceipt(id: number): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}/checkout`, {});
  }

  deleteReceipt(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
