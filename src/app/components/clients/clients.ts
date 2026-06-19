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

/**
 * "What The Clients Say" testimonials with the tabbed accordion.
 * Replaces the jQuery `.naccs` click handler with an `activeIndex` signal.
 */
@Component({
  selector: 'app-clients',
  templateUrl: './clients.html',
})
export class ClientsComponent {
  readonly activeIndex = signal(0);

  readonly testimonials: readonly Testimonial[] = [
    {
      name: 'David Martino Co',
      date: '30 November 2021',
      category: 'Financial Apps',
      rating: '4.8',
      quote:
        '“Lorem ipsum dolor sit amet, consectetur adpiscing elit, sed do eismod tempor idunte ut labore et dolore magna aliqua darwin kengan lorem ipsum dolor sit amet, consectetur picing elit massive big blasta.”',
      author: 'David Martino',
      role: 'CEO of David Company',
    },
    {
      name: 'Jake Harris Nyo',
      date: '29 November 2021',
      category: 'Digital Business',
      rating: '4.5',
      quote:
        '“CTO, Lorem ipsum dolor sit amet, consectetur adpiscing elit, sed do eismod tempor idunte ut labore et dolore magna aliqua darwin kengan lorem ipsum dolor sit amet, consectetur picing elit massive big blasta.”',
      author: 'Jake H. Nyo',
      role: 'CTO of Digital Company',
    },
    {
      name: 'May Catherina',
      date: '27 November 2021',
      category: 'Business & Economics',
      rating: '4.7',
      quote:
        '“May, Lorem ipsum dolor sit amet, consectetur adpiscing elit, sed do eismod tempor idunte ut labore et dolore magna aliqua darwin kengan lorem ipsum dolor sit amet, consectetur picing elit massive big blasta.”',
      author: 'May C.',
      role: 'Founder of Catherina Co.',
    },
    {
      name: 'Random User',
      date: '24 November 2021',
      category: 'New App Ecosystem',
      rating: '3.9',
      quote:
        '“Lorem ipsum dolor sit amet, consectetur adpiscing elit, sed do eismod tempor idunte ut labore et dolore magna aliqua darwin kengan lorem ipsum dolor sit amet, consectetur picing elit massive big blasta.”',
      author: 'Random Staff',
      role: 'Manager, Digital Company',
    },
    {
      name: 'Mark Amber Do',
      date: '21 November 2021',
      category: 'Web Development',
      rating: '4.3',
      quote:
        '“Mark, Lorem ipsum dolor sit amet, consectetur adpiscing elit, sed do eismod tempor idunte ut labore et dolore magna aliqua darwin kengan lorem ipsum dolor sit amet, consectetur picing elit massive big blasta.”',
      author: 'Mark Am',
      role: 'CTO, Amber Do Company',
    },
  ];

  readonly stars = [1, 2, 3, 4, 5];

  select(index: number): void {
    this.activeIndex.set(index);
  }
}
