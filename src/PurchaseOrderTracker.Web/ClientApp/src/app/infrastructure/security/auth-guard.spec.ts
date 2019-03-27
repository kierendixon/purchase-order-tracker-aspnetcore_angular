import { async, fakeAsync, tick } from '@angular/core/testing';
import { ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { of } from 'rxjs';

import { AuthGuard } from './auth-guard';
import { AuthService } from './auth.service';

describe('AuthGuard', () => {
  let authGuard: AuthGuard;
  let authServiceSpy: AuthService;

  beforeEach(async(() => {
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    authServiceSpy = jasmine.createSpyObj('AuthService', ['isUserAuthenticated']);
    authGuard = new AuthGuard(routerSpy, authServiceSpy);
  }));

  describe('#canActivate', () => {
    it('returns true if user is authenticated', fakeAsync(() => {
      const stubbedActivatedRouteSnapshot = {} as ActivatedRouteSnapshot;
      const stubbedRouterStateSnapshot = {} as RouterStateSnapshot;
      (authServiceSpy.isUserAuthenticated as jasmine.Spy).and.returnValue(of(true));
      authGuard.canActivate(stubbedActivatedRouteSnapshot, stubbedRouterStateSnapshot).subscribe(
        result => {
          expect(authServiceSpy.isUserAuthenticated).toHaveBeenCalledTimes(1);
          expect(result).toBe(true);
        },
        err => fail()
      );
      tick();
    }));

    it('calls navigateToLoginPage to user is not authenticated', fakeAsync(() => {
      const stubbedActivatedRouteSnapshot = {} as ActivatedRouteSnapshot;
      const stubbedRouterStateSnapshot = {} as RouterStateSnapshot;

      (authServiceSpy.isUserAuthenticated as jasmine.Spy).and.returnValue(of(false));
      const navigateToLoginPageSpy = spyOn(authGuard, 'navigateToLoginPage');

      authGuard.canActivate(stubbedActivatedRouteSnapshot, stubbedRouterStateSnapshot).subscribe(
        result => {
          expect(authServiceSpy.isUserAuthenticated).toHaveBeenCalledTimes(1);
          expect(navigateToLoginPageSpy).toHaveBeenCalledTimes(1);
          expect(result).toBe(false);
        },
        err => fail()
      );
      tick();
    }));
  });
});
