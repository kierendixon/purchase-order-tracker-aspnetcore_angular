import { async, ComponentFixture, TestBed, tick, fakeAsync } from '@angular/core/testing';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { of, throwError } from 'rxjs';

import { AppModule } from '../app.module';
import { AccountComponent } from './account.component';
import * as CONFIG from '../config/app.config';
import { mainSiteUrl, returnUrlQueryParam } from '../config/routing.config';
import { BasePage } from '../../test/base-page';

describe('AccountComponent', () => {
    let component: AccountComponent;
    let fixture: ComponentFixture<AccountComponent>;
    let authService: any;

    beforeEach(async(() => {
        authService = jasmine.createSpyObj('AuthService', ['isUserAuthenticated', 'authenticate']);

        TestBed.configureTestingModule({
            imports: [AppModule],
            providers: [
                {
                    provide: CONFIG.authServiceToken,
                    useValue: authService
                }
            ]
        })
        .compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(AccountComponent);
        component = fixture.componentInstance;
    });

    it('should create', () => {
        expect(component).toBeDefined();
    });

    describe('#ngOnInit', () => {
        it('should call skipLoginIfAlreadyAuthenticated', () => {
            const skipLoginSpy = spyOn(component, 'skipLoginIfAlreadyAuthenticated');
            component.ngOnInit();
            expect(skipLoginSpy).toHaveBeenCalledTimes(1);
        });
    });

    describe('#skipLoginIfAlreadyAuthenticated', () => {
        it('should set error message if authentication service returns error', () => {
            const isUserAuthenticatedSpy = authService.isUserAuthenticated.and.returnValue( throwError('an error ocurred') );
            component.skipLoginIfAlreadyAuthenticated();

            expect(component.errorMessage).toBe('an error ocurred');
            expect(isUserAuthenticatedSpy).toHaveBeenCalledTimes(1);
        });

        it('should call navigateToNextUrl if user authenticated', () => {
            const isUserAuthenticatedSpy = authService.isUserAuthenticated.and.returnValue( of(true) );
            const navigateToNextUrlSpy = spyOn(component, 'navigateToNextUrl');
            component.skipLoginIfAlreadyAuthenticated();

            expect(isUserAuthenticatedSpy).toHaveBeenCalledTimes(1);
            expect(navigateToNextUrlSpy).toHaveBeenCalledTimes(1);
        });

        it('should not call navigateToNextUrl if user not authenticated', () => {
            const isUserAuthenticatedSpy = authService.isUserAuthenticated.and.returnValue( of(false) );
            const navigateToNextUrlSpy = spyOn(component, 'navigateToNextUrl');
            component.skipLoginIfAlreadyAuthenticated();

            expect(isUserAuthenticatedSpy).toHaveBeenCalledTimes(1);
            expect(navigateToNextUrlSpy.calls.count()).toBe(0);
        });
    });

    describe('#navigateToNextUrl', () => {
        it('should navigate to query param url', () => {
            const returnUrlQueryParamValue = 'a-return-url';
            const queryParams: Params = {};
            queryParams[returnUrlQueryParam] = returnUrlQueryParamValue;

            const activatedRoute = fixture.debugElement.injector.get(ActivatedRoute) as ActivatedRoute;
            activatedRoute.snapshot.queryParams = queryParams;

            const router = fixture.debugElement.injector.get(Router) as Router;
            const navigateByUrlSpy = spyOn(router, 'navigateByUrl');

            component.navigateToNextUrl();

            expect(navigateByUrlSpy).toHaveBeenCalledWith(returnUrlQueryParamValue);
        });

        it('should navigate to default url if no query param provided', () => {
            const activatedRoute = fixture.debugElement.injector.get(ActivatedRoute) as ActivatedRoute;
            const router = fixture.debugElement.injector.get(Router) as Router;
            const navigateByUrlSpy = spyOn(router, 'navigateByUrl');

            component.navigateToNextUrl();

            expect(activatedRoute.snapshot.queryParams[returnUrlQueryParam]).toBeUndefined();
            expect(navigateByUrlSpy).toHaveBeenCalledWith(mainSiteUrl);
        });
    });

    describe('#onSubmit', () => {
        it('should call navigateToNextUrl if authentication is successful', () => {
            const isUserAuthenticatedSpy = authService.authenticate.and.returnValue( of() );
            const navigateToNextUrlSpy = spyOn(component, 'navigateToNextUrl');
            component.onSubmit();

            expect(isUserAuthenticatedSpy).toHaveBeenCalledTimes(1);
            expect(navigateToNextUrlSpy.calls.count()).toBe(0);
        });

        it('should set error message if authentication service returns error', () => {
            const isUserAuthenticatedSpy = authService.authenticate.and.returnValue( throwError('an error ocurred') );
            component.onSubmit();

            expect(component.errorMessage).toBe('an error ocurred');
            expect(isUserAuthenticatedSpy).toHaveBeenCalledTimes(1);
            expect(isUserAuthenticatedSpy).toHaveBeenCalledTimes(1);
        });

        it('should authenticate using provided username and password', () => {
            const username = 'a-username';
            const password = 'a-password';
            component.model.username = username;
            component.model.password = password;
            const subscriptionSpy = jasmine.createSpyObj('Subscription', ['subscribe']);
            const isUserAuthenticatedSpy = authService.authenticate.and.returnValue( subscriptionSpy );

            component.onSubmit();

            expect(isUserAuthenticatedSpy).toHaveBeenCalledWith(username, password);
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
            const errorMessage = 'an error message';
            const page = new Page(fixture);
            component.errorMessage = errorMessage;

            spyOn(component, 'ngOnInit');
            fixture.detectChanges();

            expect(page.messageDiv.textContent).toMatch(errorMessage);
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
    get usernameInput() { return this.query<HTMLInputElement>('input[name="username"]'); }
    get passwordInput() { return this.query<HTMLInputElement>('input[name="password"]'); }
    get submitBtn() { return this.query<HTMLInputElement>('button[type="submit"]'); }
    get messageDiv() { return this.query<HTMLDivElement>('div.alert.alert-danger'); }
}
