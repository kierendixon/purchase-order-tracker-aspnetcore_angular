import { HttpClient } from '@angular/common/http';
import { async } from '@angular/core/testing';
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot } from '@angular/router';

import { AuthGuard } from './auth.guard';
import { AuthService } from './auth.service';

describe('AuthGuard', () => {
  let authGuard: AuthGuard;
  let testAuthService: AuthService;

  beforeEach(async(() => {
    const routerSpy: Router = jasmine.createSpyObj('Router', ['navigate']);
    const httpClientSpy: HttpClient = jasmine.createSpyObj('HttpClient', ['get']);
    testAuthService = new AuthService(httpClientSpy);
    authGuard = new AuthGuard(routerSpy, testAuthService);
  }));

  describe('#canActivate', () => {
    it('returns true if user is authenticated', () => {
      const stubbedActivatedRouteSnapshot = {} as ActivatedRouteSnapshot;
      const stubbedRouterStateSnapshot = {} as RouterStateSnapshot;

      spyOnProperty(testAuthService, 'currentUser', 'get').and.returnValue({});

      const result = authGuard.canActivate(stubbedActivatedRouteSnapshot, stubbedRouterStateSnapshot);
      expect(result).toBe(true);
    });

    it('calls navigateToLoginPage to user is not authenticated', () => {
      const stubbedActivatedRouteSnapshot = {} as ActivatedRouteSnapshot;
      const stubbedRouterStateSnapshot = {} as RouterStateSnapshot;

      const navigateToLoginPageSpy = spyOn(authGuard, 'navigateToLoginPage');

      const result = authGuard.canActivate(stubbedActivatedRouteSnapshot, stubbedRouterStateSnapshot);
      expect(navigateToLoginPageSpy).toHaveBeenCalledTimes(1);
      expect(result).toBe(false);
      expect(testAuthService.currentUser).toBeUndefined();
    });
  });
});
