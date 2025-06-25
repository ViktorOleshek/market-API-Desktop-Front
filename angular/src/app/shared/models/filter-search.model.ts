export class FilterSearchModel {
  categoryId?: number | null;
  minPrice?: number | null;
  maxPrice?: number | null;

  constructor(
    categoryId?: number | null,
    minPrice?: number | null,
    maxPrice?: number | null
  ) {
    this.categoryId = categoryId;
    this.minPrice = minPrice;
    this.maxPrice = maxPrice;
  }
}
