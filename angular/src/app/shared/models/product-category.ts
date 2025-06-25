import { BaseModel } from './base.model';

export class ProductCategory extends BaseModel {
  categoryName: string;
  productIds: number[];

  constructor(
    id: number = 0,
    categoryName: string = '',
    productIds: number[] = []
  ) {
    super(id);
    this.categoryName = categoryName;
    this.productIds = productIds;
  }
}
