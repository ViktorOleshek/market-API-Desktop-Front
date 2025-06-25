import { BaseModel } from './base.model';

export class ReceiptDetail extends BaseModel {
  receiptId: number;
  productId: number;
  productName?: string;
  discountUnitPrice: number;
  unitPrice: number;
  quantity: number;

  constructor(
    id: number = 0,
    receiptId: number = 0,
    productId: number = 0,
    discountUnitPrice: number = 0,
    unitPrice: number = 0,
    quantity: number = 0,
    productName?: string
  ) {
    super(id);
    this.receiptId = receiptId;
    this.productId = productId;
    this.discountUnitPrice = discountUnitPrice;
    this.unitPrice = unitPrice;
    this.quantity = quantity;
    this.productName = productName;
  }
}
