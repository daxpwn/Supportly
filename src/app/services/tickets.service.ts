import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { environment } from '../../environments/environment';

export interface Ticket {
  id: number;
  ticketNumber: string;
  subject: string;
  status: string;
  priority: string;
  createdAt: string;
}

export interface TicketDetail extends Ticket {
  description: string;
  category: string;
  requester: string;
  assignee: string;
  updatedAt: string;
}

export interface TicketComment {
  id: number;
  author: string;
  body: string;
  isInternal: boolean;
  createdAt: string;
}

@Injectable({ providedIn: 'root' })
export class TicketsService {
  private readonly http = inject(HttpClient);

  getTickets(): Observable<Ticket[]> {
    const url = environment.ext
      ? `${environment.apiUrl}/tickets.list${environment.ext}` 
      : `${environment.apiUrl}/tickets`;
    return this.http.get<Ticket[]>(url);
  }

  getTicket(id: number): Observable<TicketDetail> {
    if (environment.ext) {
      return this.http
        .get<TicketDetail[]>(`${environment.apiUrl}/tickets.detail${environment.ext}`)
        .pipe(map((list) => list.find((t) => t.id === id)!));
    }
    return this.http.get<TicketDetail>(`${environment.apiUrl}/tickets/${id}`);
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
  ): Observable<TicketComment> {
    return this.http.post<TicketComment>(
      `${environment.apiUrl}/tickets/${ticketId}/comments`,
      comment,
    );
  }
}

