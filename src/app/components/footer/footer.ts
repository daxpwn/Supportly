import { Component, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';

/** Newsletter sign-up + footer widgets. */
@Component({
  selector: 'app-footer',
  imports: [FormsModule],
  templateUrl: './footer.html',
})
export class FooterComponent {
  readonly email = signal('');
  readonly subscribed = signal(false);

  subscribe(): void {
    if (!this.email().trim()) return;
    // Hook up your real newsletter API here.
    this.subscribed.set(true);
  }
}
