import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../shared/services/auth.service';
import { FormsModule } from '@angular/forms';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-login',
  imports: [
    FormsModule,
    NgIf
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  // Form data
  username = '';
  password = '';
  confirmPassword = '';
  rememberMe = false;
  acceptTerms = false;

  // UI state
  isLoginMode = true;
  showPassword = false;
  showConfirmPassword = false;
  isLoading = false;
  errorMessage = '';
  successMessage = '';

  constructor(private authService: AuthService, private router: Router) {}

  /**
   * Toggle between login and registration mode
   */
  toggleMode(): void {
    this.isLoginMode = !this.isLoginMode;
    this.clearMessages();
    this.resetForm();
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
    this.isLoading = true;

    if (this.isLoginMode) {
      this.performLogin();
    } else {
      this.performRegistration();
    }
  }

  /**
   * Perform login
   */
  private performLogin(): void {
    this.authService.login(this.username, this.password).subscribe({
      next: (response) => {
        this.isLoading = false;
        this.successMessage = 'Login successful! Redirecting...';

        setTimeout(() => {
          this.router.navigate(['/receipts']);
        }, 1000);
      },
      error: (error) => {
        this.isLoading = false;
        this.errorMessage = error?.error?.message || 'Login failed. Please check your credentials.';
      }
    });
  }

  /**
   * Perform registration
   */
  private performRegistration(): void {
    if (this.password !== this.confirmPassword) {
      this.isLoading = false;
      this.errorMessage = 'Passwords do not match.';
      return;
    }

    this.authService.register(this.username, this.password).subscribe({
      next: (response) => {
        this.isLoading = false;
        this.successMessage = 'Account created successfully! Please sign in.';

        setTimeout(() => {
          this.isLoginMode = true;
          this.resetForm();
          this.clearMessages();
        }, 2000);
      },
      error: (error) => {
        this.isLoading = false;
        this.errorMessage = error?.error?.message || 'Registration failed. Please try again.';
      }
    });
  }

  /**
   * Handle Google OAuth login
   */
  loginWithGoogle(): void {
    this.clearMessages();
    this.isLoading = true;

    this.authService.loginWithGoogle().subscribe({
      next: (response) => {
        this.isLoading = false;
        this.successMessage = 'Google login successful! Redirecting...';

        setTimeout(() => {
          this.router.navigate(['/receipts']);
        }, 1000);
      },
      error: (error) => {
        this.isLoading = false;
        this.errorMessage = 'Google login failed. Please try again.';
      }
    });
  }

  /**
   * Handle Microsoft OAuth login
   */
  loginWithMicrosoft(): void {
    this.clearMessages();
    this.isLoading = true;

    this.authService.loginWithMicrosoft().subscribe({
      next: (response) => {
        this.isLoading = false;
        this.successMessage = 'Microsoft login successful! Redirecting...';

        setTimeout(() => {
          this.router.navigate(['/receipts']);
        }, 1000);
      },
      error: (error) => {
        this.isLoading = false;
        this.errorMessage = 'Microsoft login failed. Please try again.';
      }
    });
  }

  /**
   * Handle forgot password
   */
  forgotPassword(event: Event): void {
    event.preventDefault();

    if (!this.username) {
      this.errorMessage = 'Please enter your username first.';
      return;
    }

    this.clearMessages();
    this.isLoading = true;

    this.authService.forgotPassword(this.username).subscribe({
      next: (response) => {
        this.isLoading = false;
        this.successMessage = 'Password reset link sent to your email.';
      },
      error: (error) => {
        this.isLoading = false;
        this.errorMessage = 'Failed to send reset email. Please try again.';
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

  /**
   * Reset form data
   */
  private resetForm(): void {
    this.username = '';
    this.password = '';
    this.confirmPassword = '';
    this.rememberMe = false;
    this.acceptTerms = false;
    this.showPassword = false;
    this.showConfirmPassword = false;
  }
}
