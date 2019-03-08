import { ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { of } from 'rxjs';

import { AuthGuard } from './auth-guard';
import { AuthService } from './auth.service';

describe('AuthGuard', () => {
  let authGuard: AuthGuard;
  let authServiceSpy: AuthService;

  beforeEach(() => {
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    authServiceSpy = jasmine.createSpyObj('AuthService', ['isUserAuthenticated']);
    authGuard = new AuthGuard(routerSpy, authServiceSpy);
  });

  describe('#canActivate', () => {
    it('returns true if user is authenticated', () => {
      const stubbedActivatedRouteSnapshot = {} as ActivatedRouteSnapshot;
      const stubbedRouterStateSnapshot = {} as RouterStateSnapshot;
      (authServiceSpy.isUserAuthenticated as jasmine.Spy).and.returnValue(of(true));
      const canActivate = authGuard.canActivate(stubbedActivatedRouteSnapshot, stubbedRouterStateSnapshot);

      expect(authServiceSpy.isUserAuthenticated).toHaveBeenCalledTimes(1);
      expect(canActivate).toBe(true);
    });

    it('calls navigateToLoginPage to user is not authenticated', () => {
      const stubbedActivatedRouteSnapshot = {} as ActivatedRouteSnapshot;
      const stubbedRouterStateSnapshot = {} as RouterStateSnapshot;

      (authServiceSpy.isUserAuthenticated as jasmine.Spy).and.returnValue(of(false));
      const navigateToLoginPageSpy = spyOn(authGuard, 'navigateToLoginPage');

      const canActivate = authGuard.canActivate(stubbedActivatedRouteSnapshot, stubbedRouterStateSnapshot);

      expect(authServiceSpy.isUserAuthenticated).toHaveBeenCalledTimes(1);
      expect(navigateToLoginPageSpy).toHaveBeenCalledTimes(1);
      expect(canActivate).toBe(false);
    });
  });
});
