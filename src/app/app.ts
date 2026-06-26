import { Component, inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import {
  NavigationCancel,
  NavigationEnd,
  NavigationError,
  NavigationStart,
  Router,
  RouterOutlet,
} from '@angular/router';
import NProgress from 'nprogress';
import { PreloaderComponent } from './components/preloader/preloader';
import { HeaderComponent } from './components/header/header';
import { FooterComponent } from './components/footer/footer';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    PreloaderComponent,
    HeaderComponent,
    FooterComponent,
  ],
  templateUrl: './app.html',
})
export class App {
  private readonly router = inject(Router);
  private readonly platformId = inject(PLATFORM_ID);

  constructor() {
    if (!isPlatformBrowser(this.platformId)) {
      return;
    }

    NProgress.configure({ showSpinner: false });

    this.router.events.subscribe((event) => {
      if (event instanceof NavigationStart) {
        NProgress.start();
      } else if (
        event instanceof NavigationEnd ||
        event instanceof NavigationCancel ||
        event instanceof NavigationError
      ) {
        NProgress.done();
      }
    });
  }
}