import { Injectable } from '@angular/core';
import {
  HTTP_INTERCEPTORS,
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse,
} from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';

@Injectable()
export class ErrorTitleInterceptor implements HttpInterceptor {
  constructor() {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        if (!error.error.errors && !error.error.title) {
          alert(error.error.title ?? 'Непредвиденная ошибка');
          error.error.title = '';
        }
        return throwError(error);
      })
    );
  }
}

export const errorTitleInterceptorProviders = [
  { provide: HTTP_INTERCEPTORS, useClass: ErrorTitleInterceptor, multi: true },
];
