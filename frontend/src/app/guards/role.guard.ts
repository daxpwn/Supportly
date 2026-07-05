import { inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export function roleGuard(...allowedRoles: string[]): CanActivateFn {
  return () => {
    const platformId = inject(PLATFORM_ID);
    const auth = inject(AuthService);
    const router = inject(Router);

    if (!isPlatformBrowser(platformId)) return true;

    const role = auth.role();

    if (!role) return router.createUrlTree(['/login']);
    if (allowedRoles.includes(role)) return true;

    return router.createUrlTree([role === 'admin' ? '/dashboard' : '/my-tickets']);
  };
}