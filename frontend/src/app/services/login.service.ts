import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map, of } from 'rxjs';
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

interface MockUser extends LoginResponse { email: string; password: string; }

@Injectable({ providedIn: 'root' })
export class LoginService {
  private readonly http = inject(HttpClient);

  login(creds: LoginRequest): Observable<LoginResponse> {
    if (environment.ext) {
      return this.http
        .get<MockUser[]>(`${environment.apiUrl}/auth.login${environment.ext}`)
        .pipe(
          map((users) => {
            const u = users.find(
              (x) => x.email === creds.email && x.password === creds.password,
            );
            if (!u) throw new Error('Invalid credentials');
            return { token: u.token, refreshToken: u.refreshToken };
          }),
        );
    }
    return this.http.post<LoginResponse>(
      `${environment.apiUrl}/auth/login`,
      creds,
    );
  }

  // Backend: POST /api/register — pravi customer nalog, vraća 201 bez tela (bez tokena).
  register(creds: RegisterRequest): Observable<void> {
    return this.http.post<void>(`${environment.apiUrl}/register`, creds);
  }

  // Backend: POST /api/auth/logout — invalidira JWT + refresh token na serveru
  // (TokenId se čita iz Bearer headera koji interceptor sam dodaje).
  logout(): Observable<void> {
    if (environment.ext) return of(void 0);
    return this.http.post<void>(`${environment.apiUrl}/auth/logout`, {});
  }
}