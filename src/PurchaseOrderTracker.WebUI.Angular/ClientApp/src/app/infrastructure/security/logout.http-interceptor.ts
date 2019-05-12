import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { loginAccountUrl, logoutAccountUrl } from '../../config/api.config';
import { accountUrl } from '../../config/routing.config';
import { AuthService, RefreshCommand } from './auth.service';

@Injectable()
export class LogoutHttpInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService, private router: Router) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const that = this;

    if (request.url.includes(loginAccountUrl) || request.url.includes(logoutAccountUrl)) {
      return next.handle(request);
    } else {
      return next.handle(request).pipe(
        catchError((err: HttpErrorResponse) => {
          if (err.status == 401) {
            const tokenIsExpired =
              err.headers.has('WWW-Authenticate') &&
              err.headers.get('WWW-Authenticate').includes('The token is expired');

            if (tokenIsExpired) {
              const command = new RefreshCommand(this.authService.currentUser.token.refresh_token);
              this.authService.handleRefreshCommand(command).subscribe(result => {}, err => that.handleLogout());
            } else {
              that.handleLogout();
            }
          }

          return throwError(err);
        })
      );
    }
  }

  handleLogout() {
    this.authService
      .handleLogoutCommand()
      .subscribe(() => this.router.navigate([accountUrl]), err => this.router.navigate([accountUrl]));
  }
}
