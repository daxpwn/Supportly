import { Component, afterNextRender, inject, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UserListItem, UsersService } from '../../services/users.service';

@Component({
  selector: 'app-users',
  imports: [RouterLink, DatePipe],
  templateUrl: './users.html',
})
export class UsersComponent {
  private readonly usersService = inject(UsersService);
  private readonly toastr = inject(ToastrService);

  readonly users = signal<UserListItem[]>([]);
  readonly loading = signal(true);
  readonly error = signal('');

  constructor() {
    afterNextRender(() => this.load());
  }

  private load(): void {
    this.loading.set(true);
    this.usersService.getUsers().subscribe({
      next: (data) => {
        this.users.set(data);
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set('Error loading users.');
        this.loading.set(false);
        console.error(err);
      },
    });
  }

  onDelete(user: UserListItem): void {
    if (!confirm(`Obrisati korisnika "${user.fullName}"?`)) return;

    this.usersService.deleteUser(user.id).subscribe({
      next: () => {
        this.users.update((list) => list.filter((u) => u.id !== user.id));
        this.toastr.success('Korisnik je obrisan.');
      },
      error: (err) => {
        this.toastr.error('Greška pri brisanju korisnika.');
        console.error(err);
      },
    });
  }

  roleClass(role: string): string {
    switch (role) {
      case 'admin':
        return 'bg-purple-100 text-purple-700';
      case 'agent':
        return 'bg-sky-100 text-sky-700';
      case 'customer':
        return 'bg-gray-100 text-gray-600';
      default:
        return 'bg-gray-100 text-gray-600';
    }
  }
}
