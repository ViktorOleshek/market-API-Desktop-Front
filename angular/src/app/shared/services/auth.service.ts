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
  private googleAuthObserver: any = null;


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

// У auth.service.ts замінити методи Google автентифікації:

  /**
   * Initialize Google OAuth
   */
  initializeGoogleAuth(): Promise<void> {
    return new Promise((resolve, reject) => {
      if (!(window as any).google) {
        reject('Google SDK not loaded');
        return;
      }

      (window as any).google.accounts.id.initialize({
        client_id: environment.googleClientId,
        callback: (response: any) => this.handleGoogleCallback(response)
      });

      resolve();
    });
  }

  handleOAuthCallback(code: string, provider: string): Observable<AuthResponse> {
    const callbackUrl = `${this.apiUrl}/auth/${provider}/callback`;
    return this.http.post<AuthResponse>(callbackUrl, { code }).pipe(
      tap((response) => {
        this.storeAuthData(response);
      })
    );
  }

  loginWithGoogle(): Observable<AuthResponse> {
    return new Observable(observer => {
      this.initializeGoogleAuth().then(() => {
        (window as any).google.accounts.id.prompt((notification: any) => {
          if (notification.isNotDisplayed() || notification.isSkippedMoment()) {
            // Fallback to popup
            (window as any).google.accounts.id.renderButton(
              document.getElementById('google-signin-button'),
              { theme: 'outline', size: 'large' }
            );
          }
        });

        // Store observer for callback
        this.googleAuthObserver = observer;
      }).catch(error => {
        observer.error(error);
      });
    });
  }

  /**
   * Handle Google OAuth callback
   */
  private handleGoogleCallback(response: any): void {
    // Відправляємо JWT token на бекенд для верифікації
    const payload = {
      credential: response.credential
    };
    console.log('Send to back-end:', payload);

    this.http.post<AuthResponse>(`${this.apiUrl}/auth/google-verify`, {
      credential: response.credential
    }).subscribe({
      next: (authResponse) => {
        this.storeAuthData(authResponse);
        if (this.googleAuthObserver) {
          this.googleAuthObserver.next(authResponse);
          this.googleAuthObserver.complete();
        }
      },
      error: (error) => {
        if (this.googleAuthObserver) {
          this.googleAuthObserver.error(error);
        }
      }
    });
  }

  // Додайте цей метод до класу AuthService
  getCustomerIdFromToken(): number | null {
    const token = this.getToken();
    if (!token) return null;

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      // .NET ClaimTypes.NameIdentifier = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
      const nameId = payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];
      return nameId ? parseInt(nameId) : null;
    } catch (error) {
      console.error('Error parsing token:', error);
      return null;
    }
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
