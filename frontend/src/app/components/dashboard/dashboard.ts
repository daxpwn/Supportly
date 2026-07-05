import { Component, afterNextRender, computed, inject, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Ticket, TicketsService } from '../../services/tickets.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.html',
  imports: [RouterLink, DatePipe],
})
export class DashboardComponent {
  private readonly ticketsService = inject(TicketsService);

  readonly tickets = signal<Ticket[]>([]);
  readonly loading = signal(true);
  readonly error = signal('');


  readonly latestTickets = computed(() =>
    [...this.tickets()]
      .sort((a, b) => b.createdAt.localeCompare(a.createdAt))
      .slice(0, 5),
  );


  private readonly closedStatuses = ['Rešen', 'Zatvoren'];
  readonly activeCount = computed(
    () => this.tickets().filter((t) => !this.closedStatuses.includes(t.status)).length,
  );
  readonly resolvedCount = computed(
    () => this.tickets().filter((t) => this.closedStatuses.includes(t.status)).length,
  );
  readonly criticalCount = computed(
    () => this.tickets().filter((t) => t.priority === 'Kritičan').length,
  );

  constructor() {
    afterNextRender(() => {
      this.ticketsService.getTickets().subscribe({
        next: (data) => {
          this.tickets.set(data);
          this.loading.set(false);
        },
        error: (err) => {
          this.error.set('Greška pri učitavanju tiketa.');
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
