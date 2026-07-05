import { Component, afterNextRender, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { forkJoin } from 'rxjs';
import {
  Category,
  Department,
  Priority,
  TicketsService,
} from '../../services/tickets.service';

@Component({
  selector: 'app-apply-ticket',
  imports: [FormsModule],
  templateUrl: './apply-ticket.html',
})
export class ApplyTicketComponent {
  private readonly ticketsService = inject(TicketsService);
  private readonly router = inject(Router);
  private readonly toastr = inject(ToastrService);

  subject = '';
  description = '';
  categoryId: number | null = null;
  priorityId: number | null = null;
  departmentId: number | null = null;
  files: File[] = [];

  readonly categories = signal<Category[]>([]);
  readonly priorities = signal<Priority[]>([]);
  readonly departments = signal<Department[]>([]);
  readonly submitting = signal(false);
  readonly error = signal('');

  constructor() {
    afterNextRender(() => {
      this.ticketsService.getCategories().subscribe({
        next: (data) => this.categories.set(data),
        error: (err) => console.error(err),
      });
      this.ticketsService.getPriorities().subscribe({
        next: (data) => this.priorities.set(data),
        error: (err) => console.error(err),
      });
      this.ticketsService.getDepartments().subscribe({
        next: (data) => this.departments.set(data),
        error: (err) => console.error(err),
      });
    });
  }

  onSubmit(): void {
    this.error.set('');

    if (this.subject.trim() === '' || this.description.trim() === '') {
      this.error.set('Subject and description are required.');
      return;
    }
    if (this.categoryId === null || this.priorityId === null) {
      this.error.set('Please select a category and priority.');
      return;
    }

    this.submitting.set(true);
    this.ticketsService
      .createTicket({
        subject: this.subject,
        description: this.description,
        categoryId: this.categoryId,
        priorityId: this.priorityId,
        departmentId: this.departmentId,
      })
      .subscribe({
        next: (res) => {
          // Prilozi se kače na već kreiran tiket (treba nam njegov id).
          if (this.files.length === 0) {
            this.finishSuccess();
            return;
          }

          if (!res?.id) {
            // Tiket je kreiran, ali backend nije vratio id (npr. nije restartovan) — bez priloga.
            this.submitting.set(false);
            this.toastr.warning('Tiket je kreiran, ali prilozi nisu mogli da se zakače.');
            this.router.navigate(['/my-tickets']);
            return;
          }

          forkJoin(
            this.files.map((f) => this.ticketsService.uploadAttachment(res.id, f)),
          ).subscribe({
            next: () => this.finishSuccess(),
            error: (err) => {
              // Tiket je kreiran, ali prilog nije prošao — ne gubimo tiket.
              this.submitting.set(false);
              this.toastr.warning('Tiket je kreiran, ali prilog nije otpremljen.');
              this.router.navigate(['/my-tickets']);
              console.error(err);
            },
          });
        },
        error: (err) => {
          this.submitting.set(false);
          this.error.set('Error submitting ticket.');
          console.error(err);
        },
      });
  }

  onFileChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.files = input.files ? Array.from(input.files) : [];
  }

  private finishSuccess(): void {
    this.submitting.set(false);
    this.toastr.success('Tiket je uspešno poslat.');
    this.router.navigate(['/my-tickets']);
  }
}
