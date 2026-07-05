import { Component, afterNextRender, inject, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { UsersService } from '../../services/users.service';
import { Department, TicketsService } from '../../services/tickets.service';

@Component({
  selector: 'app-user-edit',
  imports: [FormsModule, RouterLink],
  templateUrl: './user-edit.html',
})
export class UserEditComponent {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly usersService = inject(UsersService);
  private readonly ticketsService = inject(TicketsService);
  private readonly toastr = inject(ToastrService);

  private userId = 0;

  readonly loading = signal(true);
  readonly saving = signal(false);
  readonly error = signal('');
  readonly departments = signal<Department[]>([]);

  // Role nemaju backend endpoint — fiksne vrednosti iz seed-a (id-jevi iz baze).
  readonly roles = [
    { id: 1, name: 'admin' },
    { id: 2, name: 'agent' },
    { id: 3, name: 'customer' },
  ];

  fullName = '';
  email = '';
  phone = '';
  roleId: number | null = null;
  departmentId: number | null = null;
  isActive = true;

  constructor() {
    afterNextRender(() => {
      this.userId = Number(this.route.snapshot.paramMap.get('id'));

      this.ticketsService.getDepartments().subscribe({
        next: (list) => this.departments.set(list),
        error: (err) => console.error(err),
      });

      this.usersService.getUser(this.userId).subscribe({
        next: (u) => {
          this.fullName = u.fullName;
          this.email = u.email;
          this.phone = u.phone ?? '';
          this.roleId = u.roleId;
          this.departmentId = u.departmentId;
          this.isActive = u.isActive;
          this.loading.set(false);
        },
        error: (err) => {
          this.error.set('Error loading user.');
          this.loading.set(false);
          console.error(err);
        },
      });
    });
  }

  onSubmit(): void {
    this.error.set('');

    if (this.fullName.trim() === '' || this.email.trim() === '' || this.roleId === null) {
      this.error.set('Full name, email and role are required.');
      return;
    }

    this.saving.set(true);
    this.usersService
      .updateUser(this.userId, {
        fullName: this.fullName,
        email: this.email,
        phone: this.phone,
        roleId: this.roleId,
        departmentId: this.departmentId,
        isActive: this.isActive,
      })
      .subscribe({
        next: () => {
          this.saving.set(false);
          this.toastr.success('Korisnik je sačuvan.');
          this.router.navigate(['/users']);
        },
        error: (err) => {
          this.saving.set(false);
          this.error.set('Error saving user.');
          console.error(err);
        },
      });
  }
}
