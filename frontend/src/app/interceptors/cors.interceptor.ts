import { Injectable } from '@angular/core';
import {
  HTTP_INTERCEPTORS,
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
} from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class CorsInterceptor implements HttpInterceptor {
  constructor() {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    let authReq = req;

    authReq = req.clone({
      headers: req.headers.set('Access-Control-Allow-Origin', '*'),
    });

    return next.handle(authReq);
  }
}

export const corsInterceptorProviders = [
  { provide: HTTP_INTERCEPTORS, useClass: CorsInterceptor, multi: true },
];
