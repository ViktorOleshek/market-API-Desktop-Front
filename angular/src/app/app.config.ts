import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideClientHydration } from '@angular/platform-browser';
import { provideHttpClient, withFetch } from '@angular/common/http';
import { HTTP_INTERCEPTORS } from '@angular/common/http';

import { routes } from './app.routes';
import { NoopInterceptor } from './interceptors/no-sslinterceptor.interceptor';
import { AuthInterceptor } from './interceptors/auth.interceptor';
// import { ErrorHandlerInterceptor } from './interceptors/error-handler.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideClientHydration(),
    provideHttpClient(withFetch()),
    { provide: HTTP_INTERCEPTORS, useClass: NoopInterceptor, multi: true }, // Інтерсептор для SSL
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },  // Інтерсептор для токена JWT
    // { provide: HTTP_INTERCEPTORS, useClass: ErrorHandlerInterceptor, multi: true }, // Інтерсептор для обробки помилок
  ],
};
