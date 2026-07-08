import { Component, afterNextRender, inject, signal } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { RoleWithUseCases, RolesService } from '../../services/roles.service';

@Component({
  selector: 'app-roles',
  templateUrl: './roles.html',
})
export class RolesComponent {
  private readonly rolesService = inject(RolesService);
  private readonly toastr = inject(ToastrService);

  readonly roles = signal<RoleWithUseCases[]>([]);
  readonly catalog = signal<string[]>([]);
  readonly loading = signal(true);
  readonly error = signal('');

  readonly selected = signal<Record<number, string>>({});

  constructor() {
    afterNextRender(() => this.load());
  }

  availableFor(role: RoleWithUseCases): string[] {
    return this.catalog().filter((uc) => !role.useCaseIds.includes(uc));
  }

  onSelect(roleId: number, useCaseId: string): void {
    this.selected.update((s) => ({ ...s, [roleId]: useCaseId }));
  }

  add(role: RoleWithUseCases): void {
    const useCaseId = this.selected()[role.id];
    if (!useCaseId) return;

    this.rolesService.addUseCase(role.id, useCaseId).subscribe({
      next: () => {
        this.toastr.success(`Dodato "${useCaseId}" roli ${role.name}.`);
        this.selected.update((s) => ({ ...s, [role.id]: '' }));
        this.load();
      },
      error: (err) => {
        this.toastr.error('Greška pri dodavanju dozvole.');
        console.error(err);
      },
    });
  }

  remove(role: RoleWithUseCases, useCaseId: string): void {
    this.rolesService.removeUseCase(role.id, useCaseId).subscribe({
      next: () => {
        this.toastr.success(`Uklonjeno "${useCaseId}" iz role ${role.name}.`);
        this.load();
      },
      error: (err) => {
        this.toastr.error('Greška pri uklanjanju dozvole.');
        console.error(err);
      },
    });
  }

  private load(): void {
    this.loading.set(true);
    this.rolesService.getRoles().subscribe({
      next: (roles) => {
        this.roles.set(roles);
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set('Greška pri učitavanju rola.');
        this.loading.set(false);
        console.error(err);
      },
    });

    this.rolesService.getUseCaseCatalog().subscribe({
      next: (catalog) => this.catalog.set(catalog),
      error: (err) => console.error(err),
    });
  }
}
