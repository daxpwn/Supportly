import { inject } from '@angular/core';
import {
  HttpErrorResponse,
  HttpInterceptorFn,
  HttpRequest,
} from '@angular/common/http';
import { Router } from '@angular/router';
import { catchError, switchMap, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';

function withToken(req: HttpRequest<unknown>, token: string): HttpRequest<unknown> {
  return req.clone({ setHeaders: { Authorization: `Bearer ${token}` } });
}

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const auth = inject(AuthService);
  const router = inject(Router);

  const token = auth.getToken();
  const authReq = token ? withToken(req, token) : req;

  return next(authReq).pipe(
    catchError((err: HttpErrorResponse) => {
      const isAuthCall =
        req.url.includes('/auth/login') ||
        req.url.includes('/auth/refresh') ||
        req.url.includes('/auth/logout');

      // JWT
      if (err.status === 401 && !isAuthCall && auth.getRefreshToken()) {
        return auth.refresh().pipe(
          switchMap((newToken) => next(withToken(req, newToken))),
          catchError((refreshErr) => {
            router.navigate(['/login']);
            return throwError(() => refreshErr);
          }),
        );
      }

      return throwError(() => err);
    }),
  );
};
