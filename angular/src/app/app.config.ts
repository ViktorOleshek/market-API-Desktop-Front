import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideClientHydration } from '@angular/platform-browser';
import { provideHttpClient, withFetch  } from '@angular/common/http';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { NoopInterceptor } from './interceptors/no-sslinterceptor.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [provideRouter(routes), provideClientHydration(), provideHttpClient(withFetch()),
    { provide: HTTP_INTERCEPTORS, useClass: NoopInterceptor, multi: true }]
};
