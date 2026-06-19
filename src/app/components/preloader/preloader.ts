import { Component, afterNextRender, signal } from '@angular/core';

/**
 * Page loading animation. The original template added the `loaded` class on
 * `window.load`; here we flip a signal once the browser has rendered.
 */
@Component({
  selector: 'app-preloader',
  template: `
    <div id="js-preloader" class="js-preloader" [class.loaded]="loaded()">
      <div class="preloader-inner">
        <span class="dot"></span>
        <div class="dots">
          <span></span>
          <span></span>
          <span></span>
        </div>
      </div>
    </div>
  `,
})
export class PreloaderComponent {
  readonly loaded = signal(false);

  constructor() {
    afterNextRender(() => this.loaded.set(true));
  }
}
