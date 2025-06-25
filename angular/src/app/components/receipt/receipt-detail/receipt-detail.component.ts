import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ReceiptService } from '../../../shared/services/receipt.service';
import { ProductService } from '../../../shared/services/product.service';
import { Receipt } from '../../../shared/models/receipt';
import { Product } from '../../../shared/models/product';
import { ReceiptDetail } from '../../../shared/models/receipt-detail';
import { ProductCategory } from '../../../shared/models/product-category';
import { FilterSearchModel } from '../../../shared/models/filter-search.model';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-receipt-detail',
  templateUrl: './receipt-detail.component.html',
  styleUrls: ['./receipt-detail.component.css'],
  imports: [
    FormsModule,
    CommonModule
  ]
})
export class ReceiptDetailComponent implements OnInit {
  receipt: Receipt = new Receipt();
  receiptDetails: ReceiptDetail[] = [];
  availableProducts: Product[] = [];
  filteredProducts: Product[] = [];
  categories: ProductCategory[] = [];

  // Фільтри та сортування
  searchQuery: string = '';
  selectedCategoryId: number | null = null;
  minPrice: number | null = null;
  maxPrice: number | null = null;
  sortBy: string = 'name'; // 'name', 'price', 'category'
  sortOrder: string = 'asc'; // 'asc', 'desc'

  // UI стан
  isLoading: boolean = false;
  showAddProductModal: boolean = false;
  selectedProduct: Product | null = null;
  quantityToAdd: number = 1;
  errorMessage: string = '';
  successMessage: string = '';

  // Пагінація продуктів
  currentPage: number = 1;
  itemsPerPage: number = 6;
  totalPages: number = 1;

  constructor(
    private receiptService: ReceiptService,
    private productService: ProductService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');

    if (id && id !== '0') {
      this.loadReceipt(Number(id));
      this.loadReceiptDetails(Number(id));
    }

    this.loadProducts();
    this.loadCategories();
  }

  loadReceipt(id: number): void {
    this.isLoading = true;
    this.receiptService.getReceiptById(id).subscribe({
      next: (data) => {
        this.receipt = data;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading receipt:', error);
        this.errorMessage = 'Помилка завантаження замовлення';
        this.isLoading = false;
      }
    });
  }

  loadReceiptDetails(receiptId: number): void {
    this.receiptService.getReceiptDetails(receiptId).subscribe({
      next: (data) => {
        this.receiptDetails = data;
      },
      error: (error) => {
        console.error('Error loading receipt details:', error);
        this.errorMessage = 'Помилка завантаження деталей замовлення';
      }
    });
  }

  loadProducts(): void {
    const filter: FilterSearchModel = {
      categoryId: this.selectedCategoryId,
      minPrice: this.minPrice,
      maxPrice: this.maxPrice
    };

    this.productService.getProductsByFilter(filter).subscribe({
      next: (data) => {
        this.availableProducts = data;
        this.applyFiltersAndSort();
      },
      error: (error) => {
        console.error('Error loading products:', error);
        this.errorMessage = 'Помилка завантаження продуктів';
      }
    });
  }

  loadCategories(): void {
    this.productService.getCategories().subscribe({
      next: (data) => {
        this.categories = data;
      },
      error: (error) => {
        console.error('Error loading categories:', error);
      }
    });
  }

  applyFiltersAndSort(): void {
    let filtered = [...this.availableProducts];

    // Пошук
    if (this.searchQuery) {
      filtered = filtered.filter(product =>
        product.productName.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
        product.categoryName.toLowerCase().includes(this.searchQuery.toLowerCase())
      );
    }

    // Сортування
    filtered.sort((a, b) => {
      let comparison = 0;

      switch (this.sortBy) {
        case 'name':
          comparison = a.productName.localeCompare(b.productName);
          break;
        case 'price':
          comparison = a.price - b.price;
          break;
        case 'category':
          comparison = a.categoryName.localeCompare(b.categoryName);
          break;
      }

      return this.sortOrder === 'desc' ? -comparison : comparison;
    });

    this.filteredProducts = filtered;
    this.totalPages = Math.ceil(this.filteredProducts.length / this.itemsPerPage);
    this.currentPage = Math.min(this.currentPage, this.totalPages || 1);
  }

  get paginatedProducts(): Product[] {
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    return this.filteredProducts.slice(startIndex, startIndex + this.itemsPerPage);
  }

  onSearchChange(): void {
    this.currentPage = 1;
    this.applyFiltersAndSort();
  }

  onFilterChange(): void {
    this.currentPage = 1;
    this.loadProducts();
  }

  onSortChange(): void {
    this.applyFiltersAndSort();
  }

  clearFilters(): void {
    this.searchQuery = '';
    this.selectedCategoryId = null;
    this.minPrice = null;
    this.maxPrice = null;
    this.sortBy = 'name';
    this.sortOrder = 'asc';
    this.currentPage = 1;
    this.loadProducts();
  }

  openAddProductModal(product: Product): void {
    this.selectedProduct = product;
    this.quantityToAdd = 1;
    this.showAddProductModal = true;
  }

  closeAddProductModal(): void {
    this.showAddProductModal = false;
    this.selectedProduct = null;
    this.quantityToAdd = 1;
  }

  addProductToReceipt(): void {
    if (!this.selectedProduct || this.quantityToAdd <= 0) return;

    this.isLoading = true;
    this.receiptService.addProductToReceipt(
      this.receipt.id,
      this.selectedProduct.id,
      this.quantityToAdd
    ).subscribe({
      next: () => {
        this.successMessage = `${this.selectedProduct!.productName} додано до замовлення`;
        this.loadReceiptDetails(this.receipt.id);
        this.closeAddProductModal();
        this.isLoading = false;
        setTimeout(() => this.successMessage = '', 3000);
      },
      error: (error) => {
        console.error('Error adding product:', error);
        this.errorMessage = 'Помилка додавання продукту';
        this.isLoading = false;
        setTimeout(() => this.errorMessage = '', 3000);
      }
    });
  }

  removeProductFromReceipt(detail: ReceiptDetail, quantityToRemove: number = 1): void {
    if (quantityToRemove <= 0) return;

    this.receiptService.removeProductFromReceipt(
      this.receipt.id,
      detail.productId,
      quantityToRemove
    ).subscribe({
      next: () => {
        this.successMessage = 'Продукт видалено з замовлення';
        this.loadReceiptDetails(this.receipt.id);
        setTimeout(() => this.successMessage = '', 3000);
      },
      error: (error) => {
        console.error('Error removing product:', error);
        this.errorMessage = 'Помилка видалення продукту';
        setTimeout(() => this.errorMessage = '', 3000);
      }
    });
  }

  checkOutReceipt(): void {
    if (this.receiptDetails.length === 0) {
      this.errorMessage = 'Неможливо оформити порожнє замовлення';
      setTimeout(() => this.errorMessage = '', 3000);
      return;
    }

    this.isLoading = true;
    this.receiptService.checkOutReceipt(this.receipt.id).subscribe({
      next: () => {
        this.receipt.isCheckedOut = true;
        this.successMessage = 'Замовлення успішно оформлено';
        this.isLoading = false;
        setTimeout(() => this.successMessage = '', 3000);
      },
      error: (error) => {
        console.error('Error checking out receipt:', error);
        this.errorMessage = 'Помилка оформлення замовлення';
        this.isLoading = false;
        setTimeout(() => this.errorMessage = '', 3000);
      }
    });
  }

  getTotalAmount(): number {
    return this.receiptDetails.reduce((total, detail) =>
      total + (detail.discountUnitPrice * detail.quantity), 0
    );
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
    }
  }

  cancel(): void {
    this.router.navigate(['/receipts']);
  }

  protected readonly Math = Math;
}
