import { Component, afterNextRender, inject, signal, computed, effect } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Ticket, TicketsService } from '../../services/tickets.service';

@Component({
  selector: 'app-tickets',
  imports: [RouterLink],
  templateUrl: './tickets.html',
})
export class Tickets {
  private readonly ticketsService = inject(TicketsService);

  readonly tickets = signal<Ticket[]>([]);
  readonly loading = signal(true);
  readonly error = signal('');
  readonly pageSize = 10;
  readonly currentPage = signal(1);

  searchTerm = signal('');

  sortPriority = signal<'none' | 'high' | 'low'>('none');

  sortStatus = signal<'none' | 'open' | 'closed'>('none');

  private readonly priorityRank: Record<string, number> = {
    'Kritičan': 4,
    'Visok': 3,
    'Srednji': 2,
    'Nizak': 1,
  };

  private readonly statusRank: Record<string, number> = {
    'Otvoren': 1,
    'U obradi': 2,
    'Čeka korisnika': 3,
    'Rešen': 4,
    'Zatvoren': 5,
  };

  readonly filteredTickets = computed(() => {
    const q = this.searchTerm().toLowerCase().trim();
    const list = this.tickets();

    if (!q) {
      return list;
    }

    return list.filter((ticket) =>
      ticket.subject.toLowerCase().includes(q) ||
      ticket.ticketNumber.toLowerCase().includes(q) ||
      ticket.status.toLowerCase().includes(q),
    );
  });

  readonly sortedTickets = computed(() => {
    const p = this.sortPriority();
    const s = this.sortStatus();
    return [...this.filteredTickets()].sort((a, b) => {

      if (p !== 'none') {
        const diff =
          (this.priorityRank[b.priority] ?? 0) - (this.priorityRank[a.priority] ?? 0);
        const d = p === 'high' ? diff : -diff;
        if (d !== 0) return d;
      }

      if (s !== 'none') {
        const diff = (this.statusRank[a.status] ?? 0) - (this.statusRank[b.status] ?? 0);
        const d = s === 'open' ? diff : -diff;
        if (d !== 0) return d;
      }
      return 0;
    });
  });

  readonly paginatedTickets = computed(() => {
    const all = this.sortedTickets();
    const start = (this.currentPage() - 1) * this.pageSize;
    return all.slice(start, start + this.pageSize);
  });

  readonly totalPages = computed(() =>
    Math.ceil(this.sortedTickets().length / this.pageSize),
  );

  nextPage() {
    if (this.currentPage() < this.totalPages()) {
      this.currentPage.update((p) => p + 1);
    }
  }

  prevPage() {
    if (this.currentPage() > 1) {
      this.currentPage.update((p) => p - 1);
    }
  }

  constructor() {
    effect(() => {
      this.searchTerm();
      this.sortPriority();
      this.sortStatus();
      this.currentPage.set(1);
    });
    afterNextRender(() => {
      this.ticketsService.getTickets().subscribe({
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
}
