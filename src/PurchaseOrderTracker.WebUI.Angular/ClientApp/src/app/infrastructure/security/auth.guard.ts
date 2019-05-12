import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';

import { accountUrl, returnUrlQueryParam } from '../../config/routing.config';
import { AuthService } from './auth.service';

@Injectable()
export class AuthGuard implements CanActivate {
  constructor(private router: Router, private authService: AuthService) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    if (this.authService.currentUser == null) {
      this.navigateToLoginPage(state);
      return false;
    }

    return true;
  }

  navigateToLoginPage(state: RouterStateSnapshot): void {
    const queryParams: any = {};
    queryParams[returnUrlQueryParam] = state.url;
    this.router.navigate([accountUrl], { queryParams });
  }
}
