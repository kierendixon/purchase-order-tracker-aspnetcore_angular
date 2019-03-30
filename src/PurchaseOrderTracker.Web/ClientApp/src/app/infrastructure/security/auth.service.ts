import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map, tap } from 'rxjs/operators';

import { isAuthenticatedUrl, loginAccountUrl, logoutAccountUrl } from '../../config/api.config';

@Injectable()
export class AuthService {
  private isAuthenticated?: boolean = undefined;

  constructor(private http: HttpClient) {}

  public handleLoginCommand(command: LoginCommand): Observable<string> {
    const that = this;

    return this.http.post<null>(loginAccountUrl, command).pipe(
      tap(val => (that.isAuthenticated = true))
    );
  }

  public handleLogoutCommand(): Observable<null> {
    const that = this;

    return this.http.post<null>(logoutAccountUrl, {}).pipe(
      tap(val => (that.isAuthenticated = false))
    );
  }

  public isUserAuthenticated(): Observable<boolean> {
    if (this.isAuthenticated == undefined) {
      var that = this;

      return this.http.get<IsAuthenticatedQueryResult>(isAuthenticatedUrl).pipe(
        tap(val => that.isAuthenticated = val.isAuthenticated),
        map(val => val.isAuthenticated)
      );
    }
    else {
      return of(this.isAuthenticated);
    }
  }
}

export class LoginCommand {
  constructor(readonly username: string, readonly password: string) {}
}

interface IsAuthenticatedQueryResult {
  isAuthenticated: boolean
}
