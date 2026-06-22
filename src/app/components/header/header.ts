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

/**
 * Sticky header + glavna navigacija.
 * Sticky `background-header` na skrol; mobilni meni slide-toggle; login modal.
 */
@Component({
  selector: 'app-header',
  imports: [RouterLink],
  templateUrl: './header.html',
})
export class HeaderComponent {
  private readonly platformId = inject(PLATFORM_ID);
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);

  /** Da li je korisnik ulogovan (signal iz AuthService). */
  readonly isLoggedIn = this.auth.isLoggedIn;

  readonly scrolled = signal(false);
  readonly menuOpen = signal(false);

  constructor() {
    // Postavi tačno sticky stanje kad DOM postoji (samo u browseru).
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
    this.auth.logout();
    this.router.navigate(['/login']);
  }
}
