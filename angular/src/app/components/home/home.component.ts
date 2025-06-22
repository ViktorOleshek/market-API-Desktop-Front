import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

interface Stats {
  totalCustomers: number;
  totalReceipts: number;
  activeToday: number;
  pendingReceipts: number;
}

@Component({
  selector: 'app-home',
  imports: [CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {

  stats: Stats = {
    totalCustomers: 0,
    totalReceipts: 0,
    activeToday: 0,
    pendingReceipts: 0
  };

  private isLoading = false;

  constructor(private router: Router) {}

  ngOnInit(): void {
    this.loadDashboardStats();
  }

  /**
   * Navigate to customers page
   */
  navigateToCustomers(): void {
    this.router.navigate(['/customers']);
  }

  /**
   * Navigate to receipts page
   */
  navigateToReceipts(): void {
    this.router.navigate(['/receipts']);
  }

  /**
   * Load dashboard statistics
   * In a real application, this would call a service to get actual data
   */
  private loadDashboardStats(): void {
    this.isLoading = true;

    // Simulate API call with setTimeout
    setTimeout(() => {
      this.stats = {
        totalCustomers: this.generateRandomNumber(50, 500),
        totalReceipts: this.generateRandomNumber(100, 1000),
        activeToday: this.generateRandomNumber(5, 50),
        pendingReceipts: this.generateRandomNumber(0, 25)
      };
      this.isLoading = false;
    }, 1000);
  }

  /**
   * Generate random number between min and max
   */
  private generateRandomNumber(min: number, max: number): number {
    return Math.floor(Math.random() * (max - min + 1)) + min;
  }

  /**
   * Refresh dashboard data
   */
  refreshStats(): void {
    this.loadDashboardStats();
  }

  /**
   * Handle quick action clicks with analytics
   */
  onQuickActionClick(action: string): void {
    console.log(`Quick action clicked: ${action}`);

    switch (action) {
      case 'add-customer':
        this.navigateToCustomers();
        break;
      case 'create-receipt':
        this.navigateToReceipts();
        break;
      case 'search-customers':
        this.navigateToCustomers();
        break;
      case 'recent-activity':
        this.navigateToReceipts();
        break;
      default:
        console.warn('Unknown quick action:', action);
    }
  }

  /**
   * Get current time greeting
   */
  getTimeGreeting(): string {
    const hour = new Date().getHours();

    if (hour < 12) {
      return 'Good Morning';
    } else if (hour < 17) {
      return 'Good Afternoon';
    } else {
      return 'Good Evening';
    }
  }

  /**
   * Get loading state
   */
  get loading(): boolean {
    return this.isLoading;
  }

  /**
   * Format number for display
   */
  formatNumber(num: number): string {
    if (num >= 1000) {
      return (num / 1000).toFixed(1) + 'K';
    }
    return num.toString();
  }

  /**
   * Get stats card class based on value
   */
  getStatsCardClass(value: number, type: string): string {
    const baseClass = 'text-';

    switch (type) {
      case 'customers':
        return baseClass + 'primary';
      case 'receipts':
        return baseClass + 'success';
      case 'active':
        return baseClass + 'info';
      case 'pending':
        return value > 10 ? baseClass + 'warning' : baseClass + 'success';
      default:
        return baseClass + 'secondary';
    }
  }

  /**
   * Handle feature card click
   */
  onFeatureClick(feature: string): void {
    console.log(`Feature clicked: ${feature}`);

    switch (feature) {
      case 'customers':
        this.navigateToCustomers();
        break;
      case 'receipts':
        this.navigateToReceipts();
        break;
      case 'analytics':
        // Future implementation
        console.log('Analytics feature coming soon');
        break;
    }
  }

  /**
   * Get current date formatted
   */
  getCurrentDate(): string {
    return new Date().toLocaleDateString('en-US', {
      weekday: 'long',
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  }

  /**
   * Handle error states
   */
  onError(error: any): void {
    console.error('Home component error:', error);
    // In a real app, you might show a toast notification or error message
  }
}
