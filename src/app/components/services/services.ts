import { Component } from '@angular/core';

interface ServiceItem {
  /** Position class that drives the icon background in the template CSS. */
  variant: string;
  title: string;
  text: string;
}

/** "Amazing Services & Features" section. */
@Component({
  selector: 'app-services',
  templateUrl: './services.html',
})
export class ServicesComponent {
  readonly items: readonly ServiceItem[] = [
    {
      variant: 'first-service',
      title: 'App Maintenance',
      text: 'You are not allowed to redistribute this template ZIP file on any other website.',
    },
    {
      variant: 'second-service',
      title: 'Rocket Speed of App',
      text: 'You are allowed to use the Chain App Dev HTML template. Feel free to modify or edit this layout.',
    },
    {
      variant: 'third-service',
      title: 'Multi Workflow Idea',
      text: 'If this template is beneficial for your work, please support us a little via PayPal. Thank you.',
    },
    {
      variant: 'fourth-service',
      title: '24/7 Help & Support',
      text: 'Lorem ipsum dolor consectetur adipiscing elit sedder williamsburg photo booth quinoa and fashion axe.',
    },
  ];
}
