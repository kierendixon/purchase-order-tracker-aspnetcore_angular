import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';

import { TestHelper } from '../../../../test/test-helper';
import { AppModule } from '../../../app.module';
import { editShipmentUrl } from '../../config/routing.config';
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

  it('should create', () => {
    expect(component).toBeDefined();
  });

  describe('#onSubmit', () => {
    it('navigates to edit shipment URL when created successfully', () => {
      const shipmentId = 1;
      const createResult: CreateResult = {
        id: shipmentId
      };
      const createService = fixture.debugElement.injector.get(CreateService);
      const handleSpy = spyOn(createService, 'handle').and.returnValue(of(createResult));

      const router = fixture.debugElement.injector.get(Router);
      const navigateByUrlSpy = spyOn(router, 'navigateByUrl');

      const navigateToUrl = editShipmentUrl(shipmentId);

      component.onSubmit();

      expect(handleSpy).toHaveBeenCalledTimes(1);
      expect(navigateByUrlSpy).toHaveBeenCalledWith(navigateToUrl);
    });

    it('sends error to messsage service if error returned', () => {
      const error = TestHelper.buildError();
      const createService = fixture.debugElement.injector.get(CreateService);
      const handleSpy = spyOn(createService, 'handle').and.returnValue(throwError(error));

      const messagesService = fixture.debugElement.injector.get(MessagesService);
      const addHttpResponseErrorSpy = spyOn(messagesService, 'addHttpResponseError');

      component.onSubmit();

      expect(handleSpy).toHaveBeenCalledTimes(1);
      expect(addHttpResponseErrorSpy).toHaveBeenCalledWith(error);
    });
  });
});
