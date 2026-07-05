import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { environment } from '../../environments/environment';

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

export interface PagedResponse<T> {
  totalCount: number;
  pagesCount: number;
  items: T[];
  currentPage: number;
  perPage: number;
}

@Injectable({ providedIn: 'root' })
export class TicketsService {
  private readonly http = inject(HttpClient);

  getTickets(): Observable<Ticket[]> {
    if (environment.ext) {
      return this.http.get<Ticket[]>(
        `${environment.apiUrl}/tickets.list${environment.ext}`,
      );
    }
    return this.http
      .get<PagedResponse<Ticket>>(`${environment.apiUrl}/tickets`)
      .pipe(map((res) => res.items));
  }

  getMyTickets(): Observable<Ticket[]> {
    if (environment.ext) {
      return this.http.get<Ticket[]>(
        `${environment.apiUrl}/tickets.list${environment.ext}`,
      );
    }
    return this.http
      .get<PagedResponse<Ticket>>(`${environment.apiUrl}/tickets/my`)
      .pipe(map((res) => res.items));
  }

  getTicket(id: number): Observable<TicketDetail> {
    if (environment.ext) {
      return this.http
        .get<TicketDetail[]>(`${environment.apiUrl}/tickets.detail${environment.ext}`)
        .pipe(map((list) => list.find((t) => t.id === id)!));
    }
    // Backend vraća niz sa jednim elementom, a komentari su ugnježđeni u njemu.
    return this.http
      .get<TicketDetail[]>(`${environment.apiUrl}/tickets/${id}`)
      .pipe(map((list) => list[0]));
  }

  getComments(ticketId: number): Observable<TicketComment[]> {
    const url = environment.ext
      ? `${environment.apiUrl}/tickets.comments${environment.ext}`
      : `${environment.apiUrl}/tickets/${ticketId}/comments`;
    return this.http.get<TicketComment[]>(url);
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
    const url = environment.ext
      ? `${environment.apiUrl}/categories${environment.ext}`
      : `${environment.apiUrl}/categories`;
    return this.http.get<Category[]>(url);
  }

  getPriorities(): Observable<Priority[]> {
    const url = environment.ext
      ? `${environment.apiUrl}/priorities${environment.ext}`
      : `${environment.apiUrl}/priorities`;
    return this.http.get<Priority[]>(url);
  }

  getDepartments(): Observable<Department[]> {
    const url = environment.ext
      ? `${environment.apiUrl}/departments${environment.ext}`
      : `${environment.apiUrl}/departments`;
    return this.http.get<Department[]>(url);
  }

  getStatuses(): Observable<Status[]> {
    const url = environment.ext
      ? `${environment.apiUrl}/statuses${environment.ext}`
      : `${environment.apiUrl}/statuses`;
    return this.http.get<Status[]>(url);
  }

  changeTicketStatus(ticketId: number, statusId: number): Observable<void> {
    return this.http.patch<void>(
      `${environment.apiUrl}/tickets/${ticketId}/status`,
      { statusId },
    );
  }

  createTicket(payload: CreateTicketRequest): Observable<CreateTicketResponse> {
    if (environment.ext) {
      return this.http.get<CreateTicketResponse>(
        `${environment.apiUrl}/tickets.create${environment.ext}`,
      );
    }
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

