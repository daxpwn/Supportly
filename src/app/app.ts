import { Component } from '@angular/core';
import { PreloaderComponent } from './components/preloader/preloader';
import { HeaderComponent } from './components/header/header';
import { LoginModalComponent } from './components/login-modal/login-modal';
import { BannerComponent } from './components/banner/banner';
import { ServicesComponent } from './components/services/services';
import { AboutComponent } from './components/about/about';
import { ClientsComponent } from './components/clients/clients';
import { PricingComponent } from './components/pricing/pricing';
import { FooterComponent } from './components/footer/footer';

@Component({
  selector: 'app-root',
  imports: [
    PreloaderComponent,
    HeaderComponent,
    LoginModalComponent,
    BannerComponent,
    ServicesComponent,
    AboutComponent,
    ClientsComponent,
    PricingComponent,
    FooterComponent,
  ],
  templateUrl: './app.html',
})
export class App {}
