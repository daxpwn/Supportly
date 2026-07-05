import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

interface AboutBox {
  title: string;
  text: string;
}

@Component({
  selector: 'app-about',
  imports: [RouterLink],
  templateUrl: './about.html',
})
export class AboutComponent {
  readonly boxes: readonly AboutBox[] = [
    {
      title: 'Faster Resolutions',
      text: 'Resolve tickets quicker with a unified view of every customer conversation.',
    },
    {
      title: '24/7 Customer Support',
      text: 'Help customers help themselves with a knowledge base that is always on.',
    },
    {
      title: 'Smart Ticket Routing',
      text: 'Send each request to the right agent automatically, based on your own rules.',
    },
    {
      title: 'Team Collaboration',
      text: 'Work together with internal notes, mentions, and a shared team inbox.',
    },
  ];
}
