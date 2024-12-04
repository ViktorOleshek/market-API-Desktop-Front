import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ReceiptService } from '../../../shared/services/receipt.service';
import { Receipt } from '../../../shared/models/receipt';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-receipt-detail',
  templateUrl: './receipt-detail.component.html',
  styleUrls: ['./receipt-detail.component.css'],
  imports: [
    FormsModule
  ]
})
export class ReceiptDetailComponent implements OnInit {
  receipt: Receipt = new Receipt();

  constructor(
    private receiptService: ReceiptService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');

    if (id && id !== '0') {
      this.loadReceipt(Number(id));
    }
  }

  loadReceipt(id: number): void {
    this.receiptService.getReceiptById(id).subscribe(
      (data) => {
        this.receipt = data;
      },
      (error) => {
        console.error('Error loading receipt:', error);
      }
    );
  }

  saveReceipt(): void {
    if (this.receipt.id === 0) {
      this.addReceipt();
    } else {
      this.updateReceipt();
    }
  }

  addReceipt(): void {
    this.receiptService.addReceipt(this.receipt).subscribe(
      (data) => {
        console.log('Receipt added successfully:', data);
        this.router.navigate(['/receipts']);
      },
      (error) => {
        console.error('Failed to add receipt:', error);
      }
    );
  }

  updateReceipt(): void {
    this.receiptService.addReceipt(this.receipt).subscribe(
      (data) => {
        console.log('Receipt updated successfully:', data);
        this.router.navigate(['/receipts']);
      },
      (error) => {
        console.error('Failed to update receipt:', error);
      }
    );
  }

  cancel(): void {
    this.router.navigate(['/receipts']);
  }
}
