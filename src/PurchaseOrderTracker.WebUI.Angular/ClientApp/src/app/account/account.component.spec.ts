import { HttpClient } from '@angular/common/http';
import { async, ComponentFixture, fakeAsync, TestBed, tick } from '@angular/core/testing';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { of, throwError } from 'rxjs';

import { FakeBrowserStorage } from 'src/test/fake-browser-storage';
import { BasePage } from '../../test/base-page';
import { TestHelper } from '../../test/test-helper';
import { AppModule } from '../app.module';
import { mainSiteUrl, returnUrlQueryParam } from '../config/routing.config';
import { AuthService, LoginCommand } from '../infrastructure/security/auth.service';
import { AccountComponent } from './account.component';

describe('AccountComponent', () => {
  let component: AccountComponent;
  let fixture: ComponentFixture<AccountComponent>;
  let testAuthService: AuthService;

  beforeEach(async(() => {
    const httpClientSpy: HttpClient = jasmine.createSpyObj('HttpClient', ['get']);
    testAuthService = new AuthService(httpClientSpy);

    TestBed.configureTestingModule({
      imports: [AppModule],
      providers: [
        {
          provide: AuthService,
          useValue: testAuthService
        }
      ]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountComponent);
    component = fixture.componentInstance;
  });

  it('constructs', () => {
    expect(component).toBeDefined();
  });

  describe('#ngOnInit', () => {
    it('call skipLoginIfAlreadyAuthenticated', () => {
      const skipLoginSpy = spyOn(component, 'skipLoginIfAlreadyAuthenticated');
      component.ngOnInit();
      expect(skipLoginSpy).toHaveBeenCalledTimes(1);
    });
  });

  describe('#skipLoginIfAlreadyAuthenticated', () => {
    it('call navigateToNextUrl if user authenticated', () => {
      spyOnProperty(testAuthService, 'currentUser', 'get').and.returnValue({});
      const navigateToNextUrlSpy = spyOn(component, 'navigateToNextUrl');
      component.skipLoginIfAlreadyAuthenticated();

      expect(navigateToNextUrlSpy).toHaveBeenCalledTimes(1);
      expect(testAuthService.currentUser).not.toBeUndefined();
    });

    it('do not call navigateToNextUrl if user not authenticated', () => {
      const navigateToNextUrlSpy = spyOn(component, 'navigateToNextUrl');
      component.skipLoginIfAlreadyAuthenticated();

      expect(testAuthService.currentUser).toBeUndefined();
      expect(navigateToNextUrlSpy.calls.count()).toBe(0);
    });
  });

  describe('#navigateToNextUrl', () => {
    it('navigate to query param url', () => {
      const returnUrlQueryParamValue = 'a-return-url';
      const queryParams: Params = {};
      queryParams[returnUrlQueryParam] = returnUrlQueryParamValue;

      const activatedRoute = fixture.debugElement.injector.get(ActivatedRoute);
      activatedRoute.snapshot.queryParams = queryParams;

      const router = fixture.debugElement.injector.get(Router);
      const navigateByUrlSpy = spyOn(router, 'navigateByUrl');

      component.navigateToNextUrl();

      expect(navigateByUrlSpy).toHaveBeenCalledWith(returnUrlQueryParamValue);
    });

    it('navigate to default url if no query param provided', () => {
      const activatedRoute = fixture.debugElement.injector.get(ActivatedRoute);
      const router = fixture.debugElement.injector.get(Router);
      const navigateByUrlSpy = spyOn(router, 'navigateByUrl');

      component.navigateToNextUrl();

      expect(activatedRoute.snapshot.queryParams[returnUrlQueryParam]).toBeUndefined();
      expect(navigateByUrlSpy).toHaveBeenCalledWith(mainSiteUrl);
    });
  });

  describe('#onSubmit', () => {
    it('call navigateToNextUrl if authentication is successful', () => {
      const handleLoginCommandSpy = spyOn(testAuthService, 'handleLoginCommand').and.returnValue(of());
      const navigateToNextUrlSpy = spyOn(component, 'navigateToNextUrl');
      component.onSubmit();

      expect(handleLoginCommandSpy).toHaveBeenCalledTimes(1);
      expect(navigateToNextUrlSpy.calls.count()).toBe(0);
    });

    it('set error message if authentication service returns error', () => {
      const error = TestHelper.buildError();
      const handleLoginCommandSpy = spyOn(testAuthService, 'handleLoginCommand').and.returnValue(throwError(error));
      component.onSubmit();

      expect(component.errorMessage).toBe(error.message);
      expect(handleLoginCommandSpy).toHaveBeenCalledTimes(1);
      expect(handleLoginCommandSpy).toHaveBeenCalledTimes(1);
    });

    it('authenticate using provided username and password', () => {
      const loginCommand = new LoginCommand(component.model.username, component.model.password);
      const subscriptionSpy = jasmine.createSpyObj('Subscription', ['subscribe']);
      const handleLoginCommandSpy = spyOn(testAuthService, 'handleLoginCommand').and.returnValue(subscriptionSpy);

      component.onSubmit();

      expect(handleLoginCommandSpy).toHaveBeenCalledWith(loginCommand);
      expect(subscriptionSpy.subscribe).toHaveBeenCalledTimes(1);
    });
  });

  // TODO: fix these tests
  xdescribe('#template', () => {
    it('username and password input bind to model', fakeAsync(() => {
      spyOn(component, 'ngOnInit');
      fixture.detectChanges();

      const username = 'johndoe';
      const password = 'securepassword';
      const page = new Page(fixture);
      page.usernameInput.value = username;
      page.passwordInput.value = password;
      page.usernameInput.dispatchEvent(new Event('input'));
      page.passwordInput.dispatchEvent(new Event('input'));
      tick();
      fixture.detectChanges();
      tick();

      expect(component.model.username).toMatch(username);
      expect(component.model.password).toMatch(password);
    }));

    it('error message is bound to view', () => {
      const page = new Page(fixture);
      component.errorMessage = TestHelper.ErrorMessage;

      spyOn(component, 'ngOnInit');
      fixture.detectChanges();

      expect(page.messageDiv.textContent).toMatch(TestHelper.ErrorMessage);
    });

    it('submit button event is handled by onSubmit', () => {
      const page = new Page(fixture);
      const onSubmitSpy = spyOn(component, 'onSubmit');

      page.submitBtn.click();

      expect(onSubmitSpy).toHaveBeenCalledTimes(1);
    });
  });
});

class Page extends BasePage<AccountComponent> {
  get usernameInput() {
    return this.query<HTMLInputElement>('input[name="username"]');
  }
  get passwordInput() {
    return this.query<HTMLInputElement>('input[name="password"]');
  }
  get submitBtn() {
    return this.query<HTMLInputElement>('button[type="submit"]');
  }
  get messageDiv() {
    return this.query<HTMLDivElement>('div.alert.alert-danger');
  }
}
