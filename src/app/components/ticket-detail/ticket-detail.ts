import { Component, afterNextRender, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { DatePipe } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { TicketComment, TicketDetail, TicketsService } from '../../services/tickets.service';

@Component({
  selector: 'app-ticket-detail',
  imports: [RouterLink, DatePipe, ReactiveFormsModule],
  templateUrl: './ticket-detail.html',
})

export class TicketDetailComponent {
  private readonly route = inject(ActivatedRoute);
  private readonly ticketsService = inject(TicketsService);

  readonly ticket = signal<TicketDetail | null>(null);
  readonly comments = signal<TicketComment[]>([]);
  readonly loading = signal(true);
  readonly error = signal('');
  readonly commentError = signal('');

  private readonly fb = inject(FormBuilder);
  private ticketId = 0;

  readonly commentForm = this.fb.nonNullable.group({
    body: ['', Validators.required],
    isInternal: [false],
  });

  constructor() {
    afterNextRender(() => {
      const id = Number(this.route.snapshot.paramMap.get('id'));
      this.ticketId = id;

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

  onSubmit() {
    this.commentError.set('');

    if (this.commentForm.invalid) {
      this.commentForm.markAllAsTouched();
      return;
    }

    const { body, isInternal } = this.commentForm.getRawValue();

    this.ticketsService.addComment(this.ticketId, { body, isInternal }).subscribe({
      next: (created) => {
        this.comments.update((list) => [...list, created]);
        this.commentForm.reset({ body: '', isInternal: false });
      },
      error: (err) => {
        this.commentError.set('Error, try again later');
        console.error(err);
      },
    });
  }
}