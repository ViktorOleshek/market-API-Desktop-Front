import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../shared/services/auth.service';
import { FormsModule } from '@angular/forms';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-register',
  imports: [
    FormsModule,
    NgIf
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  // Form data
  username = '';
  password = '';
  confirmPassword = '';
  acceptTerms = false;

  // UI state
  showPassword = false;
  showConfirmPassword = false;
  isLoading = false;
  errorMessage = '';
  successMessage = '';

  constructor(private authService: AuthService, private router: Router) {}

  /**
   * Navigate to login page
   */
  navigateToLogin(): void {
    this.router.navigate(['/login']);
  }

  /**
   * Toggle password visibility
   */
  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  /**
   * Toggle confirm password visibility
   */
  toggleConfirmPasswordVisibility(): void {
    this.showConfirmPassword = !this.showConfirmPassword;
  }

  /**
   * Handle form submission
   */
  onSubmit(): void {
    if (this.isLoading) return;

    this.clearMessages();

    if (this.password !== this.confirmPassword) {
      this.errorMessage = 'Passwords do not match.';
      return;
    }

    this.isLoading = true;

    this.authService.register(this.username, this.password).subscribe({
      next: (response) => {
        this.isLoading = false;
        this.successMessage = 'Account created successfully! Redirecting to login...';

        setTimeout(() => {
          this.router.navigate(['/login']);
        }, 2000);
      },
      error: (error) => {
        this.isLoading = false;
        this.errorMessage = error?.error?.message || 'Registration failed. Please try again.';
      }
    });
  }

  /**
   * Clear success and error messages
   */
  private clearMessages(): void {
    this.errorMessage = '';
    this.successMessage = '';
  }
}
