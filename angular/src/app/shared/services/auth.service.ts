import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

export interface AuthResponse {
  token: string;
  user?: {
    id: string;
    email: string;
    username: string;
  };
  message?: string;
}

export interface LoginRequest {
  username: string;
  password: string;
}

export interface RegisterRequest {
  username: string;
  password: string;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private tokenKey = 'authToken';
  private userKey = 'currentUser';
  private apiUrl = environment.apiUrl;

  // API endpoints
  private loginUrl = `${this.apiUrl}/auth/login`;
  private registerUrl = `${this.apiUrl}/auth/register`;
  private googleAuthUrl = `${this.apiUrl}/auth/google`;
  private microsoftAuthUrl = `${this.apiUrl}/auth/microsoft`;
  private forgotPasswordUrl = `${this.apiUrl}/auth/forgot-password`;

  constructor(private http: HttpClient, private router: Router) {}

  /**
   * Login with email and password
   */
  login(username: string, password: string): Observable<AuthResponse> {
    const loginData: LoginRequest = { username, password };

    return this.http.post<AuthResponse>(this.loginUrl, loginData).pipe(
      tap((response) => {
        this.storeAuthData(response);
      })
    );
  }

  /**
   * Register new user
   */
  register(username: string, password: string): Observable<AuthResponse> {
    const registerData = { username, password };

    return this.http.post<AuthResponse>(this.registerUrl, registerData);
  }

  /**
   * Login with Google OAuth
   */
  loginWithGoogle(): Observable<AuthResponse> {
    // In a real implementation, this would redirect to Google OAuth
    // For now, we'll simulate the OAuth flow
    return this.http.get<AuthResponse>(`${this.googleAuthUrl}/callback`).pipe(
      tap((response) => {
        this.storeAuthData(response);
      })
    );
  }

  /**
   * Login with Microsoft OAuth
   */
  loginWithMicrosoft(): Observable<AuthResponse> {
    // In a real implementation, this would redirect to Microsoft OAuth
    // For now, we'll simulate the OAuth flow
    return this.http.get<AuthResponse>(`${this.microsoftAuthUrl}/callback`).pipe(
      tap((response) => {
        this.storeAuthData(response);
      })
    );
  }

  /**
   * Send forgot password email
   */
  forgotPassword(email: string): Observable<{message: string}> {
    return this.http.post<{message: string}>(this.forgotPasswordUrl, { email });
  }

  /**
   * Logout user
   */
  logout(): void {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.userKey);
    this.router.navigate(['/login']);
  }

  /**
   * Get stored auth token
   */
  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  /**
   * Get current user data
   */
  getCurrentUser(): any {
    const userData = localStorage.getItem(this.userKey);
    return userData ? JSON.parse(userData) : null;
  }

  /**
   * Check if user is logged in
   */
  isLoggedIn(): boolean {
    const token = this.getToken();
    if (!token) return false;

    // Check if token is expired (basic check)
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      const now = Math.floor(Date.now() / 1000);
      return payload.exp > now;
    } catch (error) {
      return false;
    }
  }

  /**
   * Store authentication data in localStorage
   */
  private storeAuthData(response: AuthResponse): void {
    if (response.token) {
      localStorage.setItem(this.tokenKey, response.token);
    }

    if (response.user) {
      localStorage.setItem(this.userKey, JSON.stringify(response.user));
    }
  }

  /**
   * Initialize Google OAuth (for real implementation)
   */
  private initGoogleAuth(): void {
    // This would load Google OAuth SDK and configure it
    // window.location.href = `${this.googleAuthUrl}/redirect`;
  }

  /**
   * Initialize Microsoft OAuth (for real implementation)
   */
  private initMicrosoftAuth(): void {
    // This would load Microsoft OAuth SDK and configure it
    // window.location.href = `${this.microsoftAuthUrl}/redirect`;
  }
}
