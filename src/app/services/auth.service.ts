import { Injectable, computed, inject, PLATFORM_ID, signal } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly platformId = inject(PLATFORM_ID);
  private readonly isBrowser = isPlatformBrowser(this.platformId);

  readonly isLoggedIn = signal<boolean>(this.hasToken());

  readonly role = computed<string | null>(() => {
    this.isLoggedIn();
    return (this.getProfile()?.['role'] as string | undefined) ?? null;
  });

  private hasToken(): boolean {
    return this.isBrowser && !!localStorage.getItem('token');
  }

  setToken(token: string): void {
    if (this.isBrowser) localStorage.setItem('token', token);
    this.isLoggedIn.set(true);
  }

  logout(): void {
    if (this.isBrowser) localStorage.removeItem('token');
    this.isLoggedIn.set(false);
  }

  getToken(): string | null {
    return this.isBrowser ? localStorage.getItem('token') : null;
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
