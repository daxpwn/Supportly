import { Component, afterNextRender, inject, signal } from '@angular/core';
import { Ticket, TicketsService } from '../../services/tickets.service';

@Component({
  selector: 'app-tickets',
  imports: [],
  templateUrl: './tickets.html',
})
export class Tickets {

  private readonly ticketsService = inject(TicketsService);

  readonly tickets = signal<Ticket[]>([]);
  readonly loading = signal(true);
  readonly error = signal('');
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
