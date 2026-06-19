import {
  Component,
  HostListener,
  PLATFORM_ID,
  afterNextRender,
  inject,
  signal,
} from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { ModalService } from '../../services/modal.service';

interface NavLink {
  id: string;
  label: string;
}

/**
 * Sticky header + main navigation.
 * Replaces the jQuery behaviour from custom.js: sticky `background-header` on
 * scroll, mobile menu slide-toggle, smooth scroll-to-section and scroll-spy.
 */
@Component({
  selector: 'app-header',
  templateUrl: './header.html',
})
export class HeaderComponent {
  private readonly modal = inject(ModalService);
  private readonly platformId = inject(PLATFORM_ID);

  readonly links: readonly NavLink[] = [
    { id: 'top', label: 'Home' },
    { id: 'services', label: 'Services' },
    { id: 'about', label: 'About' },
    { id: 'pricing', label: 'Pricing' },
    { id: 'newsletter', label: 'Newsletter' },
  ];

  readonly scrolled = signal(false);
  readonly menuOpen = signal(false);
  readonly activeSection = signal('top');

  constructor() {
    // Set the correct sticky/active state once the DOM exists (browser only).
    afterNextRender(() => this.onScroll());
  }

  @HostListener('window:scroll')
  onScroll(): void {
    if (!isPlatformBrowser(this.platformId)) return;

    this.scrolled.set(window.scrollY > 80);

    const offset = window.scrollY + 1;
    for (const link of this.links) {
      const el = document.getElementById(link.id);
      if (!el) continue;
      const top = el.offsetTop;
      if (top <= offset && top + el.offsetHeight > offset) {
        this.activeSection.set(link.id);
      }
    }
  }

  toggleMenu(): void {
    this.menuOpen.update((open) => !open);
  }

  scrollTo(event: Event, id: string): void {
    event.preventDefault();
    this.activeSection.set(id);
    this.menuOpen.set(false);
    if (!isPlatformBrowser(this.platformId)) return;
    document.getElementById(id)?.scrollIntoView({ behavior: 'smooth' });
  }

  openSignIn(event: Event): void {
    event.preventDefault();
    this.menuOpen.set(false);
    this.modal.open();
  }
}
