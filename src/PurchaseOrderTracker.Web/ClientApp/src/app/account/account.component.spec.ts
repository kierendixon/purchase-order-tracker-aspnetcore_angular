import { async, ComponentFixture, fakeAsync, TestBed, tick } from '@angular/core/testing';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { of, throwError } from 'rxjs';

import { BasePage } from '../../test/base-page';
import { TestHelper } from '../../test/test-helper';
import { AppModule } from '../app.module';
import { mainSiteUrl, returnUrlQueryParam } from '../config/routing.config';
import { AuthService, LoginCommand } from '../infrastructure/security/auth.service';
import { AccountComponent } from './account.component';

describe('AccountComponent', () => {
  let component: AccountComponent;
  let fixture: ComponentFixture<AccountComponent>;
  let authService: any;

  beforeEach(async(() => {
    authService = jasmine.createSpyObj('AuthService', ['isUserAuthenticated', 'handleLoginCommand']);

    TestBed.configureTestingModule({
      imports: [AppModule],
      providers: [
        {
          provide: AuthService,
          useValue: authService
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
    it('set error message if authentication service returns error', () => {
      const isUserAuthenticatedSpy = authService.isUserAuthenticated.and.returnValue(
        throwError(TestHelper.ErrorMessage)
      );
      component.skipLoginIfAlreadyAuthenticated();

      expect(component.errorMessage).toBe(TestHelper.ErrorMessage);
      expect(isUserAuthenticatedSpy).toHaveBeenCalledTimes(1);
    });

    it('call navigateToNextUrl if user authenticated', () => {
      const isUserAuthenticatedSpy = authService.isUserAuthenticated.and.returnValue(of(true));
      const navigateToNextUrlSpy = spyOn(component, 'navigateToNextUrl');
      component.skipLoginIfAlreadyAuthenticated();

      expect(isUserAuthenticatedSpy).toHaveBeenCalledTimes(1);
      expect(navigateToNextUrlSpy).toHaveBeenCalledTimes(1);
    });

    it('not call navigateToNextUrl if user not authenticated', () => {
      const isUserAuthenticatedSpy = authService.isUserAuthenticated.and.returnValue(of(false));
      const navigateToNextUrlSpy = spyOn(component, 'navigateToNextUrl');
      component.skipLoginIfAlreadyAuthenticated();

      expect(isUserAuthenticatedSpy).toHaveBeenCalledTimes(1);
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
      const handleLoginCommandSpy = authService.handleLoginCommand.and.returnValue(of());
      const navigateToNextUrlSpy = spyOn(component, 'navigateToNextUrl');
      component.onSubmit();

      expect(handleLoginCommandSpy).toHaveBeenCalledTimes(1);
      expect(navigateToNextUrlSpy.calls.count()).toBe(0);
    });

    it('set error message if authentication service returns error', () => {
      const handleLoginCommandSpy = authService.handleLoginCommand.and.returnValue(throwError('an error ocurred'));
      component.onSubmit();

      expect(component.errorMessage).toBe('an error ocurred');
      expect(handleLoginCommandSpy).toHaveBeenCalledTimes(1);
      expect(handleLoginCommandSpy).toHaveBeenCalledTimes(1);
    });

    it('authenticate using provided username and password', () => {
      const loginCommand = new LoginCommand(component.model.username, component.model.password);
      const subscriptionSpy = jasmine.createSpyObj('Subscription', ['subscribe']);
      const handleLoginCommandSpy = authService.handleLoginCommand.and.returnValue(subscriptionSpy);

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
