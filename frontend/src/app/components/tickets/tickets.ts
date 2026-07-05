import { Component, afterNextRender, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import {
  Ticket,
  TicketsService,
  TicketSortBy,
} from '../../services/tickets.service';

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
  readonly totalPages = signal(1);
  readonly totalCount = signal(0);

  searchTerm = signal('');

  sortPriority = signal<'none' | 'high' | 'low'>('none');
  sortStatus = signal<'none' | 'open' | 'closed'>('none');
  onlyOpen = signal(false);

  private searchDebounce?: ReturnType<typeof setTimeout>;

  onSearch(value: string) {
    this.searchTerm.set(value);
    clearTimeout(this.searchDebounce);
    this.searchDebounce = setTimeout(() => {
      this.currentPage.set(1);
      this.load();
    }, 300);
  }

  onSortPriority(value: 'none' | 'high' | 'low') {
    this.sortPriority.set(value);
    if (value !== 'none') this.sortStatus.set('none');
    this.currentPage.set(1);
    this.load();
  }

  onSortStatus(value: 'none' | 'open' | 'closed') {
    this.sortStatus.set(value);
    if (value !== 'none') this.sortPriority.set('none');
    this.currentPage.set(1);
    this.load();
  }

  onOnlyOpen(checked: boolean) {
    this.onlyOpen.set(checked);
    this.currentPage.set(1);
    this.load();
  }

  nextPage() {
    if (this.currentPage() < this.totalPages()) {
      this.currentPage.update((p) => p + 1);
      this.load();
    }
  }

  prevPage() {
    if (this.currentPage() > 1) {
      this.currentPage.update((p) => p - 1);
      this.load();
    }
  }

  private resolveSort(): { sortBy?: TicketSortBy; sortDir?: 'asc' | 'desc' } {
    if (this.sortPriority() !== 'none') {
      return {
        sortBy: 'priority',
        sortDir: this.sortPriority() === 'high' ? 'desc' : 'asc',
      };
    }
    if (this.sortStatus() !== 'none') {
      return {
        sortBy: 'status',
        sortDir: this.sortStatus() === 'open' ? 'asc' : 'desc',
      };
    }
    return {};
  }

  private load() {
    this.loading.set(true);
    this.ticketsService
      .getTickets({
        keyword: this.searchTerm().trim() || undefined,
        page: this.currentPage(),
        perPage: this.pageSize,
        onlyOpen: this.onlyOpen() || undefined,
        ...this.resolveSort(),
      })
      .subscribe({
        next: (res) => {
          this.tickets.set(res.items);
          this.totalCount.set(res.totalCount);
          this.totalPages.set(Math.max(1, res.pagesCount));
          this.loading.set(false);
        },
        error: (err) => {
          this.error.set('Error loading tickets.');
          this.loading.set(false);
          console.error(err);
        },
      });
  }

  constructor() {
    afterNextRender(() => {
      this.load();
    });
  }
}
