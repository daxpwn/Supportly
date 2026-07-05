import { Component } from '@angular/core';

interface ServiceItem {
  /** Position class that drives the icon background in the template CSS. */
  variant: string;
  title: string;
  text: string;
}

/** "Everything Your Support Team Needs" features section. */
@Component({
  selector: 'app-services',
  templateUrl: './services.html',
})
export class ServicesComponent {
  readonly items: readonly ServiceItem[] = [
    {
      variant: 'first-service',
      title: 'Shared Inbox',
      text: 'Bring every email, live chat, and contact form into a single shared inbox so no customer request ever slips through the cracks.',
    },
    {
      variant: 'second-service',
      title: 'Ticket Management',
      text: 'Assign, prioritize, and track every ticket from open to resolved, with a clear view of who is working on what.',
    },
    {
      variant: 'third-service',
      title: 'Automation & Workflows',
      text: 'Automate repetitive tasks with rules, canned replies, and smart routing so your team can focus on real conversations.',
    },
    {
      variant: 'fourth-service',
      title: '24/7 Help & Support',
      text: 'Give customers a self-service knowledge base and round-the-clock support so they get answers any time, day or night.',
    },
  ];
}
