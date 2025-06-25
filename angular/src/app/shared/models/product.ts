import { BaseModel } from './base.model';

export class Product extends BaseModel {
  productCategoryId: number;
  categoryName: string;
  productName: string;
  price: number;
  receiptDetailIds: number[];

  constructor(
    id: number = 0,
    productCategoryId: number = 0,
    categoryName: string = '',
    productName: string = '',
    price: number = 0,
    receiptDetailIds: number[] = []
  ) {
    super(id);
    this.productCategoryId = productCategoryId;
    this.categoryName = categoryName;
    this.productName = productName;
    this.price = price;
    this.receiptDetailIds = receiptDetailIds;
  }
}
