import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface UserListItem {
  id: number;
  fullName: string;
  email: string;
  phone: string;
  isActive: boolean;
  createdAt: string;
  role: string;
}

export interface UserDetails {
  id: number;
  fullName: string;
  email: string;
  phone: string;
  roleId: number;
  role: string;
  departmentId: number | null;
  department: string;
  isActive: boolean;
  createdAt: string;
  updatedAt: string;
}

export interface UserUpdate {
  fullName: string;
  email: string;
  phone: string;
  roleId: number;
  departmentId: number | null;
  isActive: boolean;
}

@Injectable({ providedIn: 'root' })
export class UsersService {
  private readonly http = inject(HttpClient);

  getUsers(): Observable<UserListItem[]> {
    return this.http.get<UserListItem[]>(`${environment.apiUrl}/users`);
  }

  getUser(id: number): Observable<UserDetails> {
    return this.http.get<UserDetails>(`${environment.apiUrl}/users/${id}`);
  }

  updateUser(id: number, dto: UserUpdate): Observable<void> {
    return this.http.put<void>(`${environment.apiUrl}/users/${id}`, dto);
  }

  deleteUser(id: number): Observable<void> {
    return this.http.delete<void>(`${environment.apiUrl}/users/${id}`);
  }
}
