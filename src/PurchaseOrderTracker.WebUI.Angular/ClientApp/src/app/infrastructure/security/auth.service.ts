import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';

import { loginAccountUrl, logoutAccountUrl, refreshAccountUrl } from '../../config/api.config';
import { LocalStorageService } from '../browser-storage/local-storage.service';

@Injectable()
export class AuthService {
  private readonly _storageKey = 'currentUser';
  private _currentUser?: CurrentUser = undefined;

  constructor(private http: HttpClient, private localStorageService: LocalStorageService) {
    const storedUser = this.localStorageService.get(this._storageKey);
    if (storedUser != null) {
      this._currentUser = JSON.parse(storedUser);
    }
  }

  public get currentUser(): CurrentUser {
    return this._currentUser;
  }

  public handleLoginCommand(command: LoginCommand): Observable<CurrentUser> {
    const that = this;

    return this.http.post<JsonWebToken>(loginAccountUrl, command).pipe(
      tap(val => that.handleSuccessfulLogin(val)),
      map(val => that.currentUser)
    );
  }

  private handleSuccessfulLogin(token: JsonWebToken): void {
    this._currentUser = {
      token: token
    };
    this.localStorageService.set(this._storageKey, JSON.stringify(this._currentUser));
  }

  public handleRefreshCommand(command: RefreshCommand): Observable<CurrentUser> {
    const that = this;

    return this.http.post<JsonWebToken>(refreshAccountUrl, command).pipe(
      tap(val => that.handleSuccessfulLogin(val)),
      map(val => that.currentUser),
      catchError((err: HttpErrorResponse) => that.handleLogoutFailure(err))
    );
  }

  private handleLogoutFailure(err: HttpErrorResponse) {
    if (err.status == 401) {
      this.clearCurrentUser();
    }
    return throwError(err);
  }

  public handleLogoutCommand(): Observable<null> {
    const that = this;

    return this.http.post<null>(logoutAccountUrl, {}).pipe(
      tap(val => that.clearCurrentUser()),
      catchError((err: HttpErrorResponse) => that.handleLogoutFailure(err))
    );
  }

  clearCurrentUser(): void {
    this._currentUser = undefined;
    this.localStorageService.remove(this._storageKey);
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
}

export interface JsonWebToken {
  access_token: string;
  refresh_token: string;
  expires_in: number;
  token_type: string;
}
