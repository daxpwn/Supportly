import { Component, afterNextRender, computed, inject, signal } from '@angular/core';
import { DatePipe, KeyValuePipe } from '@angular/common';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-profile',
  imports: [DatePipe, KeyValuePipe],
  templateUrl: './profile.html',
})
export class ProfileComponent {
  private readonly auth = inject(AuthService);

  /** Svi claim-ovi iz JWT tokena (null dok se ne pročita u browseru). */
  readonly profile = signal<Record<string, unknown> | null>(null);

  /** Najčešća imena claim-ova za prikaz, sa fallback-ovima (.NET/standard JWT). */
  readonly name = computed(() =>
    this.pick(['name', 'given_name', 'unique_name', 'preferred_username']),
  );
  readonly email = computed(() =>
    this.pick([
      'email',
      'emailaddress',
      'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress',
    ]),
  );
  readonly role = computed(() =>
    this.pick([
      'role',
      'roles',
      'http://schemas.microsoft.com/ws/2008/06/identity/claims/role',
    ]),
  );

  /** Unix-sekunde -> milisekunde za DatePipe, ili null ako nema. */
  readonly issuedAt = computed(() => this.toDate(this.profile()?.['iat']));
  readonly expiresAt = computed(() => this.toDate(this.profile()?.['exp']));

  constructor() {
    afterNextRender(() => {
      this.profile.set(this.auth.getProfile());
    });
  }

  /** Prvi claim koji postoji iz liste mogućih ključeva. */
  private pick(keys: string[]): string {
    const p = this.profile();
    if (!p) return '—';
    for (const key of keys) {
      const value = p[key];
      if (value != null && value !== '') {
        return Array.isArray(value) ? value.join(', ') : String(value);
      }
    }
    return '—';
  }

  private toDate(value: unknown): number | null {
    return typeof value === 'number' ? value * 1000 : null;
  }
}
