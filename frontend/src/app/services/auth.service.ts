import { Injectable, computed, inject, PLATFORM_ID, signal } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import {
  Observable,
  catchError,
  finalize,
  map,
  shareReplay,
  tap,
  throwError,
} from 'rxjs';
import { environment } from '../../environments/environment';

interface TokenPair {
  token: string;
  refreshToken: string;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly platformId = inject(PLATFORM_ID);
  private readonly isBrowser = isPlatformBrowser(this.platformId);
  private readonly http = inject(HttpClient);

  private static readonly TOKEN_KEY = 'token';
  private static readonly REFRESH_KEY = 'refreshToken';

  readonly isLoggedIn = signal<boolean>(this.hasToken());

  readonly role = computed<string | null>(() => {
    this.isLoggedIn();
    return (this.getProfile()?.['role'] as string | undefined) ?? null;
  });

  // Deljeni poziv /auth/refresh — svi paralelni 401 čekaju isti refresh.
  private refreshInFlight$: Observable<string> | null = null;

  private hasToken(): boolean {
    return this.isBrowser && !!localStorage.getItem(AuthService.TOKEN_KEY);
  }

  setSession(token: string, refreshToken: string): void {
    if (this.isBrowser) {
      localStorage.setItem(AuthService.TOKEN_KEY, token);
      localStorage.setItem(AuthService.REFRESH_KEY, refreshToken);
    }
    this.isLoggedIn.set(true);
  }

  logout(): void {
    if (this.isBrowser) {
      localStorage.removeItem(AuthService.TOKEN_KEY);
      localStorage.removeItem(AuthService.REFRESH_KEY);
    }
    this.refreshInFlight$ = null;
    this.isLoggedIn.set(false);
  }

  getToken(): string | null {
    return this.isBrowser ? localStorage.getItem(AuthService.TOKEN_KEY) : null;
  }

  getRefreshToken(): string | null {
    return this.isBrowser ? localStorage.getItem(AuthService.REFRESH_KEY) : null;
  }

  // Razmeni refresh token za novi par (backend invalidira stari JWT+refresh).
  // Single-flight: dok jedan refresh traje, ostali pozivi dele isti Observable.
  refresh(): Observable<string> {
    if (this.refreshInFlight$) return this.refreshInFlight$;

    const refreshToken = this.getRefreshToken();
    if (!refreshToken) {
      return throwError(() => new Error('No refresh token'));
    }

    this.refreshInFlight$ = this.http
      .post<TokenPair>(`${environment.apiUrl}/auth/refresh`, { refreshToken })
      .pipe(
        tap((res) => this.setSession(res.token, res.refreshToken)),
        map((res) => res.token),
        catchError((err) => {
          this.logout();
          return throwError(() => err);
        }),
        finalize(() => (this.refreshInFlight$ = null)),
        shareReplay(1),
      );

    return this.refreshInFlight$;
  }

  getProfile(): Record<string, unknown> | null {
    const token = this.getToken();
    if (!token) return null;

    try {
      const payload = token.split('.')[1];
      const base64 = payload.replace(/-/g, '+').replace(/_/g, '/');
      const json = decodeURIComponent(
        atob(base64)
          .split('')
          .map((c) => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
          .join(''),
      );
      return JSON.parse(json);
    } catch {
      return null;
    }
  }
}
