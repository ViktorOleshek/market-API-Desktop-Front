import { Component, OnInit, OnDestroy } from '@angular/core';
import {Router, NavigationEnd, RouterLinkActive} from '@angular/router';
import { RouterLink, RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from './shared/services/auth.service';
import { Subscription } from 'rxjs';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink, CommonModule, RouterLinkActive],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit, OnDestroy {
  title = 'Trade Market Application';
  sidebarCollapsed = false;
  private routerSubscription: Subscription = new Subscription();

  constructor(
    public authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    // Listen to route changes to handle sidebar state
    this.routerSubscription = this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe((event: NavigationEnd) => {
        this.handleRouteChange(event.url);
      });

    // Load sidebar state from localStorage
    this.loadSidebarState();
  }

  ngOnDestroy(): void {
    if (this.routerSubscription) {
      this.routerSubscription.unsubscribe();
    }
  }

  /**
   * Toggle sidebar collapsed state
   */
  toggleSidebar(): void {
    this.sidebarCollapsed = !this.sidebarCollapsed;
    this.saveSidebarState();
  }

  /**
   * Handle user logout
   */
  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  /**
   * Get CSS classes for sidebar based on state
   */

  isMobile(): boolean {
    return window.innerWidth <= 768;
  }

  getSidebarClasses(): string {
    const classes: string[] = [];

    if (this.sidebarCollapsed) {
      classes.push('collapsed');
    }

    return classes.join(' ');
  }

  /**
   * Handle route changes
   */
  private handleRouteChange(url: string): void {
    // Auto-collapse sidebar on mobile for better UX
    if (this.isMobile()) {
      this.sidebarCollapsed = true;
    }

    // Handle specific route logic if needed
    if (url === '/login') {
      // Maybe reset some state when user goes to login
      this.sidebarCollapsed = true;
    }
  }

  /**
   * Check if current view is mobile
   */
  private isMobileView(): boolean {
    return window.innerWidth < 768;
  }

  /**
   * Save sidebar state to localStorage
   */
  private saveSidebarState(): void {
    try {
      localStorage.setItem('sidebarCollapsed', JSON.stringify(this.sidebarCollapsed));
    } catch (error) {
      console.warn('Could not save sidebar state to localStorage:', error);
    }
  }

  /**
   * Load sidebar state from localStorage
   */
  private loadSidebarState(): void {
    try {
      const savedState = localStorage.getItem('sidebarCollapsed');
      if (savedState !== null) {
        this.sidebarCollapsed = JSON.parse(savedState);
      }
    } catch (error) {
      console.warn('Could not load sidebar state from localStorage:', error);
      this.sidebarCollapsed = false;
    }
  }

  /**
   * Check if user is on a protected route
   */
  isOnProtectedRoute(): boolean {
    const protectedRoutes = ['/customers', '/receipts'];
    const currentUrl = this.router.url;
    return protectedRoutes.some(route => currentUrl.startsWith(route));
  }

  /**
   * Get current route name for display
   */
  getCurrentRouteName(): string {
    const url = this.router.url;
    if (url.startsWith('/customers')) return 'Customers';
    if (url.startsWith('/receipts')) return 'Receipts';
    if (url === '/home') return 'Home';
    if (url === '/login') return 'Login';
    return 'Trade Market';
  }

  /**
   * Handle window resize events
   */
  onWindowResize(): void {
    if (this.isMobileView() && !this.sidebarCollapsed) {
      this.sidebarCollapsed = true;
      this.saveSidebarState();
    }
  }
}
