import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';

import { AppModule } from '../../../app.module';
import { AuthService } from '../../../infrastructure/security/auth.service';
import { MainSiteModule } from '../../main-site.module';
import { MessagesService } from '../../shared/messages/messages.service';
import { NavTopComponent } from './nav-top.component';

describe('NavTopComponent', () => {
  let component: NavTopComponent;
  let fixture: ComponentFixture<NavTopComponent>;
  let authService: any;

  beforeEach(async(() => {
    authService = jasmine.createSpyObj('AuthService', ['handleLogoutCommand']);

    TestBed.configureTestingModule({
      imports: [AppModule, MainSiteModule],
      providers: [
        {
          provide: AuthService,
          useValue: authService
        }
      ]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NavTopComponent);
    component = fixture.componentInstance;
  });

  it('constructs', () => {
    expect(component).toBeDefined();
  });

  describe('#onLogout', () => {
    it('call router.navigate if logout is successful', () => {
      const router = fixture.debugElement.injector.get(Router);
      const navigateSpy = spyOn(router, 'navigate');
      const handleLogoutCommandSpy = authService.handleLogoutCommand.and.returnValue(of({}));
      component.onLogout();

      expect(handleLogoutCommandSpy).toHaveBeenCalledTimes(1);
      expect(navigateSpy).toHaveBeenCalledTimes(1);
    });

    it('sends error to messsage service if authentication service returns error', () => {
      const messagesService = fixture.debugElement.injector.get(MessagesService);
      const handleLogoutCommandSpy = authService.handleLogoutCommand.and.returnValue(throwError('an error ocurred'));
      const addHttpResponseErrorSpy = spyOn(messagesService, 'addHttpResponseError');
      component.onLogout();

      expect(handleLogoutCommandSpy).toHaveBeenCalledTimes(1);
      expect(addHttpResponseErrorSpy).toHaveBeenCalledTimes(1);
    });
  });
});
