import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';

import { loginAccountUrl, logoutAccountUrl, refreshAccountUrl } from '../../config/api.config';

@Injectable()
export class AuthService {
  constructor(private http: HttpClient) {}

  public handleLoginCommand(command: LoginCommand): Observable<null> {
    return this.http.post<null>(loginAccountUrl, command);
  }

  public handleLogoutCommand(): Observable<null> {
    return this.http.post<null>(logoutAccountUrl, {});
  }
}

export class LoginCommand {
  constructor(readonly username: string, readonly password: string) {}
}

export class RefreshCommand {
  constructor(readonly refreshToken: string) {}
}

export interface CurrentUser {
  token: JsonWebToken;
  isAdmin: boolean;
}

export interface User {
  username: string;
  isAdmin: boolean;
}

// todo delete
export interface JsonWebToken {
  access_token: string;
  refresh_token: string;
  expires_in: number;
  token_type: string;
}
