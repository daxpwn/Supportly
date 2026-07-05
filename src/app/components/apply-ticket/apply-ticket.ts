import { Component, afterNextRender, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
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
        next: () => {
          this.submitting.set(false);
          // Backend vraća 201 (bez tela) — obavesti korisnika i vodi ga na njegove tikete.
          this.toastr.success('Tiket je uspešno poslat.');
          this.router.navigate(['/my-tickets']);
        },
        error: (err) => {
          this.submitting.set(false);
          this.error.set('Error submitting ticket.');
          console.error(err);
        },
      });
  }
}
