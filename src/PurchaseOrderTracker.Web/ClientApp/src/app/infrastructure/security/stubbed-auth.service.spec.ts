import { fakeAsync, tick } from '@angular/core/testing';

import { StubbedAuthService } from './stubbed-auth.service';

describe('StubbedAuthService', () => {
    let service: StubbedAuthService;

    beforeEach(() => {
        service = new StubbedAuthService();
    });

    describe('#isUserAuthenticated', () => {
        it('returns true if token is not undefined', fakeAsync(() => {
            service.token = 'token';

            let isUserAuthenticated: boolean;
            service.isUserAuthenticated().subscribe(result => {
                isUserAuthenticated = result;
            });
            tick();

            expect(service.token).toBeDefined();
            expect(isUserAuthenticated).toBe(true);
        }));

        it('returns false is token is undefined', fakeAsync(() => {
            let isUserAuthenticated: boolean;
            service.isUserAuthenticated().subscribe(result => {
                isUserAuthenticated = result;
            });
            tick();

            expect(service.token).toBeUndefined();
            expect(isUserAuthenticated).toBe(false);
        }));
    });

    describe('#authenticate', () => {
        it('returns true if credentials are basic/basic', fakeAsync(() => {
            const username = 'basic';
            const password = 'basic';

            let token: string;
            service.authenticate(username, password).subscribe(result => {
                token = result;
            });
            tick();

            expect(token).toBeDefined();
        }));

        it('returns true if credentials are super/super', fakeAsync(() => {
            const username = 'super';
            const password = 'super';

            let token: string;
            service.authenticate(username, password).subscribe(result => {
                token = result;
            });
            tick();

            expect(token).toBeDefined();
        }));

        it('returns false if credentials are not for basic or super user', fakeAsync(() => {
            const username = 'username';
            const password = 'password';

            let errorMessage: string;
            service.authenticate(username, password).subscribe(
                result => {},
                error => errorMessage = error
            );
            tick();

            expect(errorMessage).toBeDefined();

        }));
    });
});
