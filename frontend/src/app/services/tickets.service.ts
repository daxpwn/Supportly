import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { environment } from '../../environments/environment';
import { PagedResponse } from '../interfaces/paged-response';

export interface Ticket {
  id: number;
  ticketNumber: string;
  subject: string;
  status: string;
  isClosed: boolean;
  priority: string;
  createdAt: string;
}

export interface TicketDetail extends Ticket {
  description: string;
  category: string;
  requester: string;
  assignee: string;
  updatedAt: string;
  comments?: TicketComment[];
  attachments?: Attachment[];
}

export interface TicketComment {
  id: number;
  author: string;
  body: string;
  isInternal: boolean;
  createdAt: string;
  attachments?: Attachment[];
}

export interface Attachment {
  id: number;
  fileName: string;
  filePath: string;
  mimeType: string;
}

export interface Category {
  id: number;
  name: string;
  parentId: number | null;
}

export interface Priority {
  id: number;
  name: string;
}

export interface Department {
  id: number;
  name: string;
  email: string;
}

export interface Status {
  id: number;
  name: string;
  isClosed: boolean;
}

export interface CreateTicketRequest {
  subject: string;
  description: string;
  categoryId: number;
  priorityId: number;
  departmentId: number | null;
}

export interface CreateTicketResponse {
  id: number;
  ticketNumber: string;
}

export type TicketSortBy =
  | 'createdAt'
  | 'priority'
  | 'status'
  | 'subject'
  | 'ticketNumber';

export interface TicketSearchParams {
  keyword?: string;
  page?: number;
  perPage?: number;
  statusId?: number;
  priorityId?: number;
  onlyOpen?: boolean;
  sortBy?: TicketSortBy;
  sortDir?: 'asc' | 'desc';
}

@Injectable({ providedIn: 'root' })
export class TicketsService {
  private readonly http = inject(HttpClient);

  // SERVER SIDE PRETRAGA I PAGINACIJA
  getTickets(params: TicketSearchParams = {}): Observable<PagedResponse<Ticket>> {
    return this.http.get<PagedResponse<Ticket>>(`${environment.apiUrl}/tickets`, {
      params: this.toHttpParams(params),
    });
  }

  private toHttpParams(params: TicketSearchParams): HttpParams {
    let httpParams = new HttpParams();
    if (params.keyword) httpParams = httpParams.set('keyword', params.keyword);
    if (params.page) httpParams = httpParams.set('page', params.page);
    if (params.perPage) httpParams = httpParams.set('perPage', params.perPage);
    if (params.statusId != null)
      httpParams = httpParams.set('statusId', params.statusId);
    if (params.priorityId != null)
      httpParams = httpParams.set('priorityId', params.priorityId);
    if (params.onlyOpen) httpParams = httpParams.set('onlyOpen', true);
    if (params.sortBy) httpParams = httpParams.set('sortBy', params.sortBy);
    if (params.sortDir) httpParams = httpParams.set('sortDir', params.sortDir);
    return httpParams;
  }

  getMyTickets(): Observable<Ticket[]> {
    return this.http
      .get<PagedResponse<Ticket>>(`${environment.apiUrl}/tickets/my`)
      .pipe(map((res) => res.items));
  }

  getTicket(id: number): Observable<TicketDetail> {
    return this.http
      .get<TicketDetail[]>(`${environment.apiUrl}/tickets/${id}`)
      .pipe(map((list) => list[0]));
  }

  getComments(ticketId: number): Observable<TicketComment[]> {
    return this.http.get<TicketComment[]>(
      `${environment.apiUrl}/tickets/${ticketId}/comments`,
    );
  }

  addComment(
    ticketId: number,
    comment: { body: string; isInternal: boolean },
  ): Observable<{ id: number }> {
    return this.http.post<{ id: number }>(`${environment.apiUrl}/comment`, {
      ticketId,
      body: comment.body,
      isInternal: comment.isInternal,
    });
  }

  getCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(`${environment.apiUrl}/categories`);
  }

  getPriorities(): Observable<Priority[]> {
    return this.http.get<Priority[]>(`${environment.apiUrl}/priorities`);
  }

  getDepartments(): Observable<Department[]> {
    return this.http.get<Department[]>(`${environment.apiUrl}/departments`);
  }

  getStatuses(): Observable<Status[]> {
    return this.http.get<Status[]>(`${environment.apiUrl}/statuses`);
  }

  changeTicketStatus(ticketId: number, statusId: number): Observable<void> {
    return this.http.patch<void>(
      `${environment.apiUrl}/tickets/${ticketId}/status`,
      { statusId },
    );
  }

  createTicket(payload: CreateTicketRequest): Observable<CreateTicketResponse> {
    return this.http.post<CreateTicketResponse>(
      `${environment.apiUrl}/tickets`,
      payload,
    );
  }

  uploadAttachment(
    ticketId: number,
    file: File,
    commentId?: number,
  ): Observable<void> {
    const form = new FormData();
    form.append('file', file);
    if (commentId != null) {
      form.append('commentId', String(commentId));
    }
    return this.http.post<void>(
      `${environment.apiUrl}/tickets/${ticketId}/attachments`,
      form,
    );
  }

  attachmentUrl(filePath: string): string {
    const base = environment.apiUrl.replace(/\/api\/?$/, '');
    return `${base}${filePath}`;
  }
}

