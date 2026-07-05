import { Component, afterNextRender, computed, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { DatePipe } from '@angular/common';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Status, TicketComment, TicketDetail, TicketsService } from '../../services/tickets.service';
import { AuthService } from '../../services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-ticket-detail',
  imports: [RouterLink, DatePipe, ReactiveFormsModule, FormsModule],
  templateUrl: './ticket-detail.html',
})

export class TicketDetailComponent {
  private readonly route = inject(ActivatedRoute);
  private readonly ticketsService = inject(TicketsService);
  private readonly auth = inject(AuthService);
  private readonly toastr = inject(ToastrService);

  // Interni komentar i promena statusa: samo osoblje (admin/agent), ne i klijent.
  readonly isStaff = computed(() => this.auth.role() !== 'customer');

  readonly ticket = signal<TicketDetail | null>(null);
  readonly comments = signal<TicketComment[]>([]);
  readonly statuses = signal<Status[]>([]);
  readonly loading = signal(true);
  readonly error = signal('');
  readonly commentError = signal('');

  // Izabrani status u dropdown-u (id trenutnog statusa tiketa).
  selectedStatusId: number | null = null;

  private readonly fb = inject(FormBuilder);
  private ticketId = 0;

  readonly commentForm = this.fb.nonNullable.group({
    body: ['', Validators.required],
    isInternal: [false],
  });

  constructor() {
    afterNextRender(() => {
      this.ticketId = Number(this.route.snapshot.paramMap.get('id'));
      this.loadTicket();

      // Lista statusa iz baze — treba samo osoblju za dropdown.
      if (this.isStaff()) {
        this.ticketsService.getStatuses().subscribe({
          next: (list) => { this.statuses.set(list); this.syncSelectedStatus(); },
          error: (err) => console.error(err),
        });
      }
    });
  }

  private loadTicket() {
    this.ticketsService.getTicket(this.ticketId).subscribe({
      next: (data) => {
        this.ticket.set(data);
        this.loading.set(false);
        this.syncSelectedStatus();

        if (data.comments) {
          // Komentari stižu ugnježđeni u detalju tiketa (pravi backend).
          this.comments.set(data.comments);
        } else {
          // Fallback za mock: zaseban poziv za komentare.
          this.ticketsService.getComments(this.ticketId).subscribe({
            next: (list) => this.comments.set(list),
            error: (err) => console.error(err),
          });
        }
      },
      error: (err) => { this.error.set('Error while loading ticket'); this.loading.set(false); console.error(err); },
    });
  }

  // Uskladi dropdown sa trenutnim statusom tiketa (detalj vraća ime statusa, ne id).
  private syncSelectedStatus() {
    const current = this.ticket()?.status;
    const match = this.statuses().find((s) => s.name === current);
    this.selectedStatusId = match ? match.id : null;
  }

  onStatusChange(statusId: number) {
    const previous = this.selectedStatusId;
    this.selectedStatusId = statusId; // optimistički

    this.ticketsService.changeTicketStatus(this.ticketId, statusId).subscribe({
      next: () => {
        const name = this.statuses().find((s) => s.id === statusId)?.name;
        if (name) this.ticket.update((t) => (t ? { ...t, status: name } : t));
        this.toastr.success('Status je promenjen.');
      },
      error: (err) => {
        this.selectedStatusId = previous; // vrati na staro
        this.toastr.error('Greška pri promeni statusa.');
        console.error(err);
      },
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
      next: () => {
        this.commentForm.reset({ body: '', isInternal: false });
        // Backend vraća 201 bez tela; ponovo učitavamo komentare sa servera.
        this.loadTicket();
      },
      error: (err) => {
        this.commentError.set('Error, try again later');
        console.error(err);
      },
    });
  }
}