import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { AuthService } from './auth.service';

@Injectable()
export class JwtHttpInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    let requestWithJwt = request;
    const currentUser = this.authService.currentUser;
    if (currentUser) {
      requestWithJwt = request.clone({
        setHeaders: {
          Authorization: `Bearer ${currentUser.jwtToken.access_token}`
        }
      });
    }

    return next.handle(requestWithJwt);
  }
}
