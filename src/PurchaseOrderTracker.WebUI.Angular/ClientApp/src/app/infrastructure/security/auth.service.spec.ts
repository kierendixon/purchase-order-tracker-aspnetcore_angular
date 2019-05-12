import { HttpClient } from '@angular/common/http';
import { fakeAsync, tick } from '@angular/core/testing';
import { of, throwError } from 'rxjs';
import { AuthService, LoginCommand, RefreshCommand } from './auth.service';

import { AuthTestHelper } from 'src/test/auth-test-helper';
import { FakeBrowserStorage } from 'src/test/fake-browser-storage';
import { TestHelper } from 'src/test/test-helper';
import { LocalStorageService } from '../browser-storage/local-storage.service';

describe('AuthService', () => {
  const _storageKey = 'currentUser';
  let httpSpy: HttpClient;
  let storage: LocalStorageService;

  beforeEach(() => {
    httpSpy = jasmine.createSpyObj('HttpClient', ['post']);
    storage = new LocalStorageService(new FakeBrowserStorage());
  });

  describe('constructor', () => {
    it('loads session from storage when it exists', () => {
      const currentUser = AuthTestHelper.buildCurrentUser();
      storage.set(_storageKey, JSON.stringify(currentUser));

      const service = new AuthService(httpSpy, storage);

      expect(service.currentUser).toEqual(currentUser);
    });
  });

  describe('#handleLoginCommand', () => {
    it('saves logged in user token when successfull login', fakeAsync(() => {
      const token = AuthTestHelper.buildJsonWebToken();
      (httpSpy.post as jasmine.Spy).and.returnValue(of(token));
      const service = new AuthService(httpSpy, storage);
      const command = new LoginCommand('username', 'password');

      service.handleLoginCommand(command).subscribe(
        result => {
          expect(result.token).toEqual(token);
          expect(httpSpy.post).toHaveBeenCalledTimes(1);
          expect(service.currentUser.token).toEqual(token);
          expect(JSON.parse(storage.get(_storageKey)).token).toEqual(token);
        },
        error => fail('Unexpected error: ' + JSON.stringify(error))
      );
      tick();
    }));
  });

  describe('#handleRefreshCommand', () => {
    it('saves refreshed user token when successfully logged in', fakeAsync(() => {
      const refreshedToken = AuthTestHelper.buildJsonWebToken('refreshed access_token');
      (httpSpy.post as jasmine.Spy).and.returnValue(of(refreshedToken));
      const service = new AuthService(httpSpy, storage);
      const command = new RefreshCommand('refresh token');

      service.handleRefreshCommand(command).subscribe(
        result => {
          expect(result.token).toEqual(refreshedToken);
          expect(httpSpy.post).toHaveBeenCalledTimes(1);
          expect(service.currentUser.token).toEqual(refreshedToken);
          expect(JSON.parse(storage.get(_storageKey)).token).toEqual(refreshedToken);
        },
        error => fail('Unexpected error: ' + JSON.stringify(error))
      );
      tick();
    }));

    it('logs out user when refresh fails with 401 Unauthorized error', fakeAsync(() => {
      const error = TestHelper.buildHttpErrorResponse(401);
      (httpSpy.post as jasmine.Spy).and.returnValue(throwError(error));
      const service = new AuthService(httpSpy, storage);
      const command = new RefreshCommand('refresh token');

      service
        .handleRefreshCommand(command)
        .subscribe(result => fail('Expected error but received result'), err => expect(err).toBe(error));
      tick();
    }));
  });

  describe('#clearCurrentUser', () => {
    it('clears user from storage', () => {
      const currentUser = AuthTestHelper.buildJsonWebToken();
      storage.set(_storageKey, JSON.stringify(currentUser));
      const service = new AuthService(httpSpy, storage);

      service.clearCurrentUser();

      expect(service.currentUser).toBeUndefined();
      expect(storage.get(_storageKey)).toBeUndefined();
    });
  });
});
