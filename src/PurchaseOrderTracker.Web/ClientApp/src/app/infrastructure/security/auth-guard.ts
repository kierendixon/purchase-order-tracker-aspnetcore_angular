import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { Observable, of } from 'rxjs';
import { tap } from 'rxjs/operators';

import { accountUrl, returnUrlQueryParam } from '../../config/routing.config';
import { AuthService } from './auth.service';

@Injectable()
export class AuthGuard implements CanActivate {
  constructor(private router: Router, private authService: AuthService) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    return this.authService.isUserAuthenticated().pipe(
      tap(result => {
        if (result == false) {
          this.navigateToLoginPage(state);
        }
      })
    );
  }

  navigateToLoginPage(state: RouterStateSnapshot): void {
    const queryParams: any = {};
    queryParams[returnUrlQueryParam] = state.url;
    this.router.navigate([accountUrl], { queryParams });
  }
}
