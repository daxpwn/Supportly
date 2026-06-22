import { Component, afterNextRender, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { DatePipe } from '@angular/common';
import { TicketComment, TicketDetail, TicketsService } from '../../services/tickets.service';

@Component({
  selector: 'app-ticket-detail',
  imports: [RouterLink, DatePipe],
  templateUrl: './ticket-detail.html',
})

export class TicketDetailComponent {
  private readonly route = inject(ActivatedRoute);
  private readonly ticketsService = inject(TicketsService);

  readonly ticket = signal<TicketDetail | null>(null);
  readonly comments = signal<TicketComment[]>([]);
  readonly loading = signal(true);
  readonly error = signal('');

  constructor() {
    afterNextRender(() => {
      const id = Number(this.route.snapshot.paramMap.get('id'));

      this.ticketsService.getTicket(id).subscribe({
        next: (data) => { this.ticket.set(data); this.loading.set(false); },
        error: (err) => { this.error.set('Error while loading ticket'); this.loading.set(false); console.error(err); },
      });

      this.ticketsService.getComments(id).subscribe({
        next: (data) => this.comments.set(data),
        error: (err) => console.error(err),
      });
    });
  }
}