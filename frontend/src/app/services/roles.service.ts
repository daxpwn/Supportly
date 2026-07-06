import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface RoleWithUseCases {
  id: number;
  name: string;
  description: string | null;
  useCaseIds: string[];
}

@Injectable({ providedIn: 'root' })
export class RolesService {
  private readonly http = inject(HttpClient);

  getRoles(): Observable<RoleWithUseCases[]> {
    return this.http.get<RoleWithUseCases[]>(`${environment.apiUrl}/roles`);
  }

  getUseCaseCatalog(): Observable<string[]> {
    return this.http.get<string[]>(`${environment.apiUrl}/roles/usecases`);
  }

  addUseCase(roleId: number, useCaseId: string): Observable<void> {
    return this.http.post<void>(`${environment.apiUrl}/roles/${roleId}/usecases`, {
      useCaseId,
    });
  }

  removeUseCase(roleId: number, useCaseId: string): Observable<void> {
    return this.http.delete<void>(
      `${environment.apiUrl}/roles/${roleId}/usecases/${encodeURIComponent(useCaseId)}`,
    );
  }
}
