import { Injectable, signal } from '@angular/core';

/** Which form is currently visible inside the login popup. */
export type LoginModalView = 'social' | 'login' | 'register';

/**
 * Shared state for the login/register popup.
 * Replaces the jQuery `leanModal` + show/hide logic from the original template.
 */
@Injectable({ providedIn: 'root' })
export class ModalService {
  readonly isOpen = signal(false);
  readonly view = signal<LoginModalView>('social');

  open(): void {
    this.view.set('social');
    this.isOpen.set(true);
  }

  close(): void {
    this.isOpen.set(false);
  }

  showView(view: LoginModalView): void {
    this.view.set(view);
  }
}
