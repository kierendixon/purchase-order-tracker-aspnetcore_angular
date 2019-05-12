import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';

import { TestHelper } from '../../../../test/test-helper';
import { AppModule } from '../../../app.module';
import { editSupplierUrl } from '../../config/routing.config';
import { MainSiteModule } from '../../main-site.module';
import { MessagesService } from '../../shared/messages/messages.service';
import { CreateComponent } from './create.component';
import { CreateResult, CreateService } from './create.service';

describe('CreateComponent', () => {
  let component: CreateComponent;
  let fixture: ComponentFixture<CreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [AppModule, MainSiteModule]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateComponent);
    component = fixture.componentInstance;
  });

  it('constructs', () => {
    expect(component).toBeDefined();
  });

  describe('#onSubmit', () => {
    it('navigates to edit supplier URL when created successfully', () => {
      const supplierId = 1;
      const createResult: CreateResult = {
        supplierId
      };
      const createService = fixture.debugElement.injector.get(CreateService);
      const createServiceSpy = spyOn(createService, 'handle').and.returnValue(of(createResult));

      const router = fixture.debugElement.injector.get(Router);
      const routerSpy = spyOn(router, 'navigateByUrl');

      const navigateToUrl = editSupplierUrl(supplierId);

      component.onSubmit();

      expect(createServiceSpy).toHaveBeenCalledTimes(1);
      expect(routerSpy).toHaveBeenCalledWith(navigateToUrl);
    });

    it('sends error to messsage service if error returned', () => {
      const error = new Error(TestHelper.ErrorMessage);
      const createService = fixture.debugElement.injector.get(CreateService);
      const createServiceSpy = spyOn(createService, 'handle').and.returnValue(throwError(error));

      const messagesService = fixture.debugElement.injector.get(MessagesService);
      const messagesSpy = spyOn(messagesService, 'addHttpResponseError');

      component.onSubmit();

      expect(createServiceSpy).toHaveBeenCalledTimes(1);
      expect(messagesSpy).toHaveBeenCalledWith(error);
    });
  });
});
