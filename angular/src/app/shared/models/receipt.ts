import {BaseModel} from './base.model';

export class Receipt extends BaseModel {
  customerId: number;
  operationDate: Date;
  isCheckedOut: boolean;
  receiptDetailsIds: number[];
  totalPrice?: number;

  constructor(
    id: number = 0,
    customerId: number = 0,
    operationDate: Date = new Date(),
    isCheckedOut: boolean = false,
    receiptDetailsIds: number[] = [],
    totalPrice?: number
  ) {
    super(id);
    this.customerId = customerId;
    this.operationDate = operationDate;
    this.isCheckedOut = isCheckedOut;
    this.receiptDetailsIds = receiptDetailsIds;
    this.totalPrice = totalPrice;
  }
}
