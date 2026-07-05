import { Component, afterNextRender, computed, inject, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Ticket, TicketsService } from '../../services/tickets.service';

@Component({
  selector: 'app-my-tickets',
  templateUrl: './my-tickets.html',
  imports: [RouterLink, DatePipe],
})
export class MyTicketsComponent {
  private readonly ticketsService = inject(TicketsService);

  readonly tickets = signal<Ticket[]>([]);
  readonly loading = signal(true);
  readonly error = signal('');

  readonly sortedTickets = computed(() =>
    [...this.tickets()].sort((a, b) => b.createdAt.localeCompare(a.createdAt)),
  );

  constructor() {
    afterNextRender(() => {
      this.ticketsService.getMyTickets().subscribe({
        next: (data) => {
          this.tickets.set(data);
          this.loading.set(false);
        },
        error: (err) => {
          this.error.set('Error loading tickets.');
          this.loading.set(false);
          console.error(err);
        },
      });
    });
  }

  statusClass(status: string): string {
    switch (status) {
      case 'Otvoren':
        return 'bg-blue-100 text-blue-700';
      case 'U obradi':
        return 'bg-amber-100 text-amber-700';
      case 'Čeka korisnika':
        return 'bg-purple-100 text-purple-700';
      case 'Rešen':
        return 'bg-green-100 text-green-700';
      case 'Zatvoren':
        return 'bg-gray-200 text-gray-600';
      default:
        return 'bg-gray-100 text-gray-700';
    }
  }

  priorityClass(priority: string): string {
    switch (priority) {
      case 'Nizak':
        return 'bg-gray-100 text-gray-600';
      case 'Srednji':
        return 'bg-sky-100 text-sky-700';
      case 'Visok':
        return 'bg-orange-100 text-orange-700';
      case 'Kritičan':
        return 'bg-red-100 text-red-700';
      default:
        return 'bg-gray-100 text-gray-600';
    }
  }
}
