import { Component } from '@angular/core';

interface Feature {
  label: string;
  /** Greyed-out (`non-function`) feature in the original template. */
  enabled: boolean;
}

interface PricingPlan {
  price: string;
  title: string;
  /** `pricing-item-regular` or `pricing-item-pro`. */
  variant: 'pricing-item-regular' | 'pricing-item-pro';
  features: Feature[];
}

/** "We Have The Best Pre-Order Prices" pricing tables. */
@Component({
  selector: 'app-pricing',
  templateUrl: './pricing.html',
})
export class PricingComponent {
  readonly plans: readonly PricingPlan[] = [
    {
      price: '$12',
      title: 'Standard Plan App',
      variant: 'pricing-item-regular',
      features: [
        { label: 'Lorem Ipsum Dolores', enabled: true },
        { label: '20 TB of Storage', enabled: true },
        { label: 'Life-time Support', enabled: false },
        { label: 'Premium Add-Ons', enabled: false },
        { label: 'Fastest Network', enabled: false },
        { label: 'More Options', enabled: false },
      ],
    },
    {
      price: '$25',
      title: 'Business Plan App',
      variant: 'pricing-item-pro',
      features: [
        { label: 'Lorem Ipsum Dolores', enabled: true },
        { label: '50 TB of Storage', enabled: true },
        { label: 'Life-time Support', enabled: true },
        { label: 'Premium Add-Ons', enabled: true },
        { label: 'Fastest Network', enabled: false },
        { label: 'More Options', enabled: false },
      ],
    },
    {
      price: '$66',
      title: 'Premium Plan App',
      variant: 'pricing-item-regular',
      features: [
        { label: 'Lorem Ipsum Dolores', enabled: true },
        { label: '120 TB of Storage', enabled: true },
        { label: 'Life-time Support', enabled: true },
        { label: 'Premium Add-Ons', enabled: true },
        { label: 'Fastest Network', enabled: true },
        { label: 'More Options', enabled: true },
      ],
    },
  ];
}
