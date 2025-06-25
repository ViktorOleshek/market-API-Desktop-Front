import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../shared/services/auth.service';

@Component({
  selector: 'app-oauth-callback',
  template: '<div>Processing authentication...</div>'
})
export class OAuthCallbackComponent implements OnInit {
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      const code = params['code'];
      const provider = params['provider'] || 'google';

      if (code) {
        this.authService.handleOAuthCallback(code, provider).subscribe({
          next: (response) => {
            this.router.navigate(['/receipts']);
          },
          error: (error) => {
            this.router.navigate(['/login'], {
              queryParams: { error: 'OAuth authentication failed' }
            });
          }
        });
      } else {
        this.router.navigate(['/login']);
      }
    });
  }
}
