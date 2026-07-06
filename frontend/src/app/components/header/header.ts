import {
  Component,
  HostListener,
  PLATFORM_ID,
  afterNextRender,
  inject,
  signal,
} from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { LoginService } from '../../services/login.service';

@Component({
  selector: 'app-header',
  imports: [RouterLink],
  templateUrl: './header.html',
})
export class HeaderComponent {
  private readonly platformId = inject(PLATFORM_ID);
  private readonly auth = inject(AuthService);
  private readonly loginService = inject(LoginService);
  private readonly router = inject(Router);

  readonly isLoggedIn = this.auth.isLoggedIn;

  readonly role = this.auth.role;

  readonly scrolled = signal(false);
  readonly menuOpen = signal(false);

  constructor() {
    afterNextRender(() => this.onScroll());
  }

  @HostListener('window:scroll')
  onScroll(): void {
    if (!isPlatformBrowser(this.platformId)) return;
    this.scrolled.set(window.scrollY > 80);
  }

  toggleMenu(): void {
    this.menuOpen.update((open) => !open);
  }

  logout(event: Event): void {
    event.preventDefault();
    this.menuOpen.set(false);

    // Prvo obavesti backend (dok token još postoji), pa lokalno očisti sesiju
    // bez obzira na ishod (best-effort server-side invalidacija).
    this.loginService.logout().subscribe({
      next: () => this.finishLogout(),
      error: () => this.finishLogout(),
    });
  }

  private finishLogout(): void {
    this.auth.logout();
    this.router.navigate(['/login']);
  }
}
