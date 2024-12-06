import { HttpInterceptorFn } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';

@Injectable()
export class NoopInterceptor implements HttpInterceptor {
  intercept(req: HttpRequest<any>, next: HttpHandler) {
    const secureReq = req.clone({

    });
    return next.handle(secureReq);
  }
}
