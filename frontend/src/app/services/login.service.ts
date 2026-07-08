import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  fullName: string;
  email: string;
  password: string;
  phone: string;
}

export interface LoginResponse {
  token: string;
  refreshToken: string;
}

@Injectable({ providedIn: 'root' })
export class LoginService {
  private readonly http = inject(HttpClient);

  login(creds: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(
      `${environment.apiUrl}/auth/login`,
      creds,
    );
  }

  register(creds: RegisterRequest): Observable<void> {
    return this.http.post<void>(`${environment.apiUrl}/register`, creds);
  }

  logout(): Observable<void> {
    return this.http.post<void>(`${environment.apiUrl}/auth/logout`, {});
  }
}
