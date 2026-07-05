import { Component, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-footer',
  imports: [FormsModule, RouterLink],
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
