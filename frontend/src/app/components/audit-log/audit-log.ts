import { Component, afterNextRender, inject, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { AuditLogService, UseCaseLog } from '../../services/audit-log.service';

@Component({
  selector: 'app-audit-log',
  imports: [DatePipe],
  templateUrl: './audit-log.html',
})
export class AuditLogComponent {
  private readonly auditService = inject(AuditLogService);

  readonly logs = signal<UseCaseLog[]>([]);
  readonly loading = signal(true);
  readonly error = signal('');
  readonly pageSize = 20;
  readonly currentPage = signal(1);
  readonly totalPages = signal(1);
  readonly totalCount = signal(0);

  // Filteri
  readonly username = signal('');
  readonly useCaseName = signal('');
  readonly from = signal('');
  readonly to = signal('');

  private debounce?: ReturnType<typeof setTimeout>;

  onUsername(value: string) {
    this.username.set(value);
    this.debouncedReload();
  }

  onUseCaseName(value: string) {
    this.useCaseName.set(value);
    this.debouncedReload();
  }

  onFrom(value: string) {
    this.from.set(value);
    this.currentPage.set(1);
    this.load();
  }

  onTo(value: string) {
    this.to.set(value);
    this.currentPage.set(1);
    this.load();
  }

  resetFilters() {
    this.username.set('');
    this.useCaseName.set('');
    this.from.set('');
    this.to.set('');
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

  private debouncedReload() {
    clearTimeout(this.debounce);
    this.debounce = setTimeout(() => {
      this.currentPage.set(1);
      this.load();
    }, 300);
  }

  private load() {
    this.loading.set(true);
    this.auditService
      .getLogs({
        username: this.username().trim() || undefined,
        useCaseName: this.useCaseName().trim() || undefined,
        // 'to' pokriva ceo izabrani dan (do 23:59:59)
        from: this.from() ? `${this.from()}T00:00:00` : undefined,
        to: this.to() ? `${this.to()}T23:59:59` : undefined,
        page: this.currentPage(),
        perPage: this.pageSize,
      })
      .subscribe({
        next: (res) => {
          this.logs.set(res.items);
          this.totalCount.set(res.totalCount);
          this.totalPages.set(Math.max(1, res.pagesCount));
          this.loading.set(false);
        },
        error: (err) => {
          this.error.set('Error loading audit log.');
          this.loading.set(false);
          console.error(err);
        },
      });
  }

  constructor() {
    afterNextRender(() => this.load());
  }
}
