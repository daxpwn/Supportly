import { Component, ElementRef, afterNextRender, computed, inject, signal, viewChild } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { DatePipe } from '@angular/common';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { forkJoin } from 'rxjs';
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

  readonly isStaff = computed(() => this.auth.role() !== 'customer');

  readonly ticket = signal<TicketDetail | null>(null);
  readonly comments = signal<TicketComment[]>([]);
  readonly statuses = signal<Status[]>([]);
  readonly loading = signal(true);
  readonly error = signal('');
  readonly commentError = signal('');

  selectedStatusId: number | null = null;

  commentFiles: File[] = [];
  private readonly commentFileInput = viewChild<ElementRef<HTMLInputElement>>('commentFileInput');

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
          this.comments.set(data.comments);
        } else {
          this.ticketsService.getComments(this.ticketId).subscribe({
            next: (list) => this.comments.set(list),
            error: (err) => console.error(err),
          });
        }
      },
      error: (err) => { this.error.set('Error while loading ticket'); this.loading.set(false); console.error(err); },
    });
  }

  private syncSelectedStatus() {
    const current = this.ticket()?.status;
    const match = this.statuses().find((s) => s.name === current);
    this.selectedStatusId = match ? match.id : null;
  }

  attachmentUrl(filePath: string): string {
    return this.ticketsService.attachmentUrl(filePath);
  }

  onStatusChange(statusId: number) {
    const previous = this.selectedStatusId;
    this.selectedStatusId = statusId;

    this.ticketsService.changeTicketStatus(this.ticketId, statusId).subscribe({
      next: () => {
        const name = this.statuses().find((s) => s.id === statusId)?.name;
        if (name) this.ticket.update((t) => (t ? { ...t, status: name } : t));
        this.toastr.success('Status je promenjen.');
      },
      error: (err) => {
        this.selectedStatusId = previous;
        this.toastr.error('Greška pri promeni statusa.');
        console.error(err);
      },
    });
  }

  onCommentFileChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.commentFiles = input.files ? Array.from(input.files) : [];
  }

  onSubmit() {
    this.commentError.set('');

    if (this.commentForm.invalid) {
      this.commentForm.markAllAsTouched();
      return;
    }

    const { body, isInternal } = this.commentForm.getRawValue();

    this.ticketsService.addComment(this.ticketId, { body, isInternal }).subscribe({
      next: (res) => {
        const files = this.commentFiles;
        if (files.length === 0) {
          this.afterComment();
          return;
        }

        forkJoin(
          files.map((f) =>
            this.ticketsService.uploadAttachment(this.ticketId, f, res?.id),
          ),
        ).subscribe({
          next: () => this.afterComment(),
          error: (err) => {
            this.toastr.warning('Komentar je dodat, ali prilog nije otpremljen.');
            this.afterComment();
            console.error(err);
          },
        });
      },
      error: (err) => {
        this.commentError.set('Error, try again later');
        console.error(err);
      },
    });
  }

  private afterComment(): void {
    this.commentForm.reset({ body: '', isInternal: false });
    this.commentFiles = [];
    const input = this.commentFileInput();
    if (input) input.nativeElement.value = '';
    this.loadTicket();
  }
}