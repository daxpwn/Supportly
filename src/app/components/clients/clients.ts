import { Component, signal } from '@angular/core';

interface Testimonial {
  name: string;
  date: string;
  category: string;
  rating: string;
  quote: string;
  author: string;
  role: string;
}

@Component({
  selector: 'app-clients',
  templateUrl: './clients.html',
})
export class ClientsComponent {
  readonly activeIndex = signal(0);

  readonly testimonials: readonly Testimonial[] = [
    {
      name: 'BrightCart',
      date: '12 May 2026',
      category: 'E-commerce',
      rating: '4.9',
      quote:
        '“Supportly cut our first-response time in half. Everything our customers send now lands in one shared inbox, and nothing gets lost. Our agents finally feel on top of their queue.”',
      author: 'David Martino',
      role: 'Head of Support, BrightCart',
    },
    {
      name: 'CloudNest',
      date: '04 May 2026',
      category: 'SaaS',
      rating: '4.8',
      quote:
        '“The automation rules are a game changer. Tickets route to the right team instantly, so our agents spend time helping customers instead of sorting messages.”',
      author: 'Jake Harris',
      role: 'CTO, CloudNest',
    },
    {
      name: 'Lumen Studio',
      date: '27 April 2026',
      category: 'Agency',
      rating: '4.7',
      quote:
        '“We switched from a messy email setup to Supportly and never looked back. The shared inbox and internal notes make collaborating on tricky tickets effortless.”',
      author: 'May Catherina',
      role: 'Founder, Lumen Studio',
    },
    {
      name: 'Northwind Retail',
      date: '18 April 2026',
      category: 'Retail',
      rating: '4.6',
      quote:
        '“Our knowledge base now deflects a huge chunk of repetitive questions. Customers get instant answers and our team handles the conversations that really matter.”',
      author: 'Sara Whitman',
      role: 'Support Manager, Northwind Retail',
    },
    {
      name: 'PixelForge',
      date: '09 April 2026',
      category: 'Software',
      rating: '4.8',
      quote:
        '“Reporting gives us a clear picture of response times and customer satisfaction. We make better staffing decisions and our CSAT keeps climbing.”',
      author: 'Mark Amber',
      role: 'COO, PixelForge',
    },
  ];

  readonly stars = [1, 2, 3, 4, 5];

  select(index: number): void {
    this.activeIndex.set(index);
  }
}
