import { Component, afterNextRender, inject, signal, computed } from '@angular/core';
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

  searchTerm = signal('');

  readonly filteredTickets = computed(() => {
  const q = this.searchTerm().toLowerCase().trim();
  const list = this.tickets();

  if (!q) {
    return list;
  }

  return list.filter((ticket) =>
      ticket.subject.toLowerCase().includes(q) ||
      ticket.ticketNumber.toLowerCase().includes(q) ||
      ticket.status.toLowerCase().includes(q)
    );
  });


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
  
}
