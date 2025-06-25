import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Product } from '../models/product';
import { ProductCategory } from '../models/product-category';
import { FilterSearchModel } from '../models/filter-search.model';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  private readonly apiUrl = `${environment.apiUrl}/products`;

  constructor(private http: HttpClient) {}

  getProductsByFilter(filter: FilterSearchModel): Observable<Product[]> {
    return this.http.get<Product[]>(this.apiUrl, { params: this.buildFilterParams(filter) });
  }

  getCategories(): Observable<ProductCategory[]> {
    return this.http.get<ProductCategory[]>(`${this.apiUrl}/categories`);
  }

  private buildFilterParams(filter: FilterSearchModel): any {
    const params: any = {};
    if (filter.categoryId !== null && filter.categoryId !== undefined) {
      params.CategoryId = filter.categoryId.toString();
    }
    if (filter.minPrice !== null && filter.minPrice !== undefined) {
      params.MinPrice = filter.minPrice.toString();
    }
    if (filter.maxPrice !== null && filter.maxPrice !== undefined) {
      params.MaxPrice = filter.maxPrice.toString();
    }
    return params;
  }
}
