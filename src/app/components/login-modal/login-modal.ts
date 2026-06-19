import { Component, inject } from '@angular/core';
import { ModalService } from '../../services/modal.service';

/**
 * Login / Register popup. Replaces the jQuery `leanModal` overlay and the
 * show/hide form switching from custom.js with signal-driven state.
 */
@Component({
  selector: 'app-login-modal',
  templateUrl: './login-modal.html',
})
export class LoginModalComponent {
  readonly modal = inject(ModalService);
}
