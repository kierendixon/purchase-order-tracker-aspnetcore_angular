import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';

import { loginAccountUrl, logoutAccountUrl, refreshAccountUrl } from '../../config/api.config';

@Injectable()
export class AuthService {
  private readonly _currentUserKey = 'currentUser';
  private _currentUser?: CurrentUser = undefined;

  constructor(private http: HttpClient) {
    const storedUser = localStorage.getItem(this._currentUserKey);
    if (storedUser != null) {
      this._currentUser = JSON.parse(storedUser);
    }
  }

  public get currentUser(): CurrentUser {
    return this._currentUser;
  }

  public handleLoginCommand(command: LoginCommand): Observable<CurrentUser> {
    const that = this;

    return this.http.post<JwtToken>(loginAccountUrl, command).pipe(
      tap(val => {
        that._currentUser = {
          username: 'TODO',
          jwtToken: val
        };
        // TODO: dependency inject
        localStorage.setItem(this._currentUserKey, JSON.stringify(that._currentUser));
      }),
      map(val => that.currentUser)
    );
  }

  public handleRefreshCommand(): Observable<CurrentUser> {
    const that = this;

    return this.http.post<JwtToken>(refreshAccountUrl, {}).pipe(
      // TODO: duplicated code
      tap(val => {
        that._currentUser = {
          username: 'TODO',
          jwtToken: val
        };
        localStorage.setItem(this._currentUserKey, JSON.stringify(that._currentUser));
      }),
      map(val => that.currentUser),
      catchError((err: HttpErrorResponse) => {
        if (err.status == 401) {
          that.clearCurrentUser();
        }
        return throwError(err);
      })
    );
  }

  public handleLogoutCommand(): Observable<null> {
    const that = this;

    return this.http.post<null>(logoutAccountUrl, {}).pipe(
      tap(val => {
        that.clearCurrentUser();
      }),
      catchError((err: HttpErrorResponse) => {
        if (err.status == 401) {
          that.clearCurrentUser();
        }
        return throwError(err);
      })
    );
  }

  clearCurrentUser(): void {
    this._currentUser = null;
    localStorage.removeItem(this._currentUserKey);
  }
}

export class LoginCommand {
  constructor(readonly username: string, readonly password: string) {}
}

export interface CurrentUser {
  username: string;
  jwtToken: JwtToken;
}

interface JwtToken {
  access_token: string;
  refresh_token: string;
  expires_in: number;
  token_type: string;
}
