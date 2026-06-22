import { Component } from '@angular/core';
import { BannerComponent } from '../banner/banner';
import { ServicesComponent } from '../services/services';
import { AboutComponent } from '../about/about';
import { ClientsComponent } from '../clients/clients';
import { PricingComponent } from '../pricing/pricing';

@Component({
  selector: 'app-home',
  imports: [
    BannerComponent,
    ServicesComponent,
    AboutComponent,
    ClientsComponent,
    PricingComponent,
  ],
  templateUrl: './home.html',
})
export class HomeComponent {}