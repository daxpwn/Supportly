import { Injectable, inject, PLATFORM_ID, signal } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly platformId = inject(PLATFORM_ID);
  private readonly isBrowser = isPlatformBrowser(this.platformId);

  /** Reaktivno stanje prijave — navbar/guard ga čitaju kao signal. */
  readonly isLoggedIn = signal<boolean>(this.hasToken());

  private hasToken(): boolean {
    return this.isBrowser && !!localStorage.getItem('token');
  }

  /** Pozvati posle uspešnog login-a. */
  setToken(token: string): void {
    if (this.isBrowser) localStorage.setItem('token', token);
    this.isLoggedIn.set(true);
  }

  logout(): void {
    if (this.isBrowser) localStorage.removeItem('token');
    this.isLoggedIn.set(false);
  }
}
