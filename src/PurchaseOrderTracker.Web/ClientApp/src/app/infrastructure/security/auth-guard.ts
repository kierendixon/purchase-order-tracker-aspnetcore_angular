import { Injectable, Inject } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

import { AuthService } from './auth.service';
import { authServiceToken } from '../../config/app.config';
import { accountUrl, returnUrlQueryParam } from '../../config/routing.config';

@Injectable()
export class AuthGuard implements CanActivate {
    constructor(private router: Router,
        @Inject(authServiceToken) private authService: AuthService) {
    }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
        let canActivate = false;
        this.authService.isUserAuthenticated().subscribe(
            isUserAuthenticated => {
                if (isUserAuthenticated) {
                    canActivate = true;
                } else {
                    this.navigateToLoginPage(state);
                }
            }
        );
        return canActivate;
    }

    navigateToLoginPage(state: RouterStateSnapshot): void {
        const queryParams: any = {};
        queryParams[returnUrlQueryParam] = state.url;
        this.router.navigate([accountUrl], { queryParams: queryParams });
    }
}
