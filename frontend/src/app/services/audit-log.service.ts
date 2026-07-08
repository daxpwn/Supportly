import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { PagedResponse } from '../interfaces/paged-response';

export interface UseCaseLog {
  id: number;
  userId: number | null;
  username: string;
  useCaseId: string;
  useCaseName: string;
  executedAt: string;
  durationMs: number;
  succeeded: boolean;
  payload: string | null;
}

export interface AuditLogSearchParams {
  userId?: number;
  username?: string;
  useCaseName?: string;
  from?: string;
  to?: string;
  page?: number;
  perPage?: number;
}

@Injectable({ providedIn: 'root' })
export class AuditLogService {
  private readonly http = inject(HttpClient);

  getLogs(
    params: AuditLogSearchParams = {},
  ): Observable<PagedResponse<UseCaseLog>> {
    let httpParams = new HttpParams();
    if (params.userId != null)
      httpParams = httpParams.set('userId', params.userId);
    if (params.username) httpParams = httpParams.set('username', params.username);
    if (params.useCaseName)
      httpParams = httpParams.set('useCaseName', params.useCaseName);
    if (params.from) httpParams = httpParams.set('from', params.from);
    if (params.to) httpParams = httpParams.set('to', params.to);
    if (params.page) httpParams = httpParams.set('page', params.page);
    if (params.perPage) httpParams = httpParams.set('perPage', params.perPage);

    return this.http.get<PagedResponse<UseCaseLog>>(
      `${environment.apiUrl}/auditlog`,
      { params: httpParams },
    );
  }
}
