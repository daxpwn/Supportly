import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  refreshToken: string;
}

@Injectable({ providedIn: 'root' })
export class LoginService {
  private readonly http = inject(HttpClient);

  login(creds: LoginRequest): Observable<LoginResponse> {

    if (environment.ext) {
      return this.http.get<LoginResponse>(
        `${environment.apiUrl}/auth.login${environment.ext}`,
      );
    }

    return this.http.post<LoginResponse>(
      `${environment.apiUrl}/auth/login`,
      creds,
    );
  }
}