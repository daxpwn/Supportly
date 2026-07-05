import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

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
  imports: [RouterLink],
  templateUrl: './pricing.html',
})
export class PricingComponent {
  readonly plans: readonly PricingPlan[] = [
    {
      price: '$15',
      title: 'Starter Plan',
      variant: 'pricing-item-regular',
      features: [
        { label: 'Up to 3 agents', enabled: true },
        { label: 'Shared team inbox', enabled: true },
        { label: 'Email ticketing', enabled: true },
        { label: 'Knowledge base', enabled: false },
        { label: 'Automation rules', enabled: false },
        { label: 'Priority support', enabled: false },
      ],
    },
    {
      price: '$29',
      title: 'Growth Plan',
      variant: 'pricing-item-pro',
      features: [
        { label: 'Up to 10 agents', enabled: true },
        { label: 'Shared team inbox', enabled: true },
        { label: 'Email & live chat', enabled: true },
        { label: 'Knowledge base', enabled: true },
        { label: 'Automation rules', enabled: true },
        { label: 'Priority support', enabled: false },
      ],
    },
    {
      price: '$59',
      title: 'Enterprise Plan',
      variant: 'pricing-item-regular',
      features: [
        { label: 'Unlimited agents', enabled: true },
        { label: 'Shared team inbox', enabled: true },
        { label: 'Email, chat & phone', enabled: true },
        { label: 'Knowledge base', enabled: true },
        { label: 'Advanced automation', enabled: true },
        { label: 'Priority support', enabled: true },
      ],
    },
  ];
}
