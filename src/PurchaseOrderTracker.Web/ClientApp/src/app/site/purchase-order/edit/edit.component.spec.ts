import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { of, throwError } from 'rxjs';

import { PurchaseOrderTestHelper } from 'src/test/purchase-order-test-helper';
import { TestHelper } from '../../../../test/test-helper';
import { AppModule } from '../../../app.module';
import { idParam, purchaseOrdersUrl } from '../../config/routing.config';
import { MainSiteModule } from '../../main-site.module';
import { MessagesService } from '../../shared/messages/messages.service';
import { DeleteService } from './delete.service';
import { EditComponent } from './edit.component';
import { EditService } from './edit.service';

describe('EditComponent', () => {
  let component: EditComponent;
  let fixture: ComponentFixture<EditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [AppModule, MainSiteModule]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeDefined();
  });

  describe('#ngOnInit', () => {
    it('sets purchaseOrderId as query param value', () => {
      const params: Params = {};
      params[idParam] = 1;

      const activatedRoute = fixture.debugElement.injector.get(ActivatedRoute);
      activatedRoute.snapshot.params = params;
      spyOn(component, 'refreshData');

      component.ngOnInit();

      expect(component.purchaseOrderId).toBe(params[idParam]);
    });

    it('calls refreshData', () => {
      const refreshDataSpy = spyOn(component, 'refreshData');
      component.ngOnInit();
      expect(refreshDataSpy).toHaveBeenCalledTimes(1);
    });
  });

  describe('#refreshData', () => {
    it("updates component's model with response", () => {
      const editQueryResult = PurchaseOrderTestHelper.buildEditQueryResult();

      const editService = fixture.debugElement.injector.get(EditService);
      const handleQuerySpy = spyOn(editService, 'handleQuery').and.returnValue(of(editQueryResult));

      component.refreshData();

      expect(handleQuerySpy).toHaveBeenCalledTimes(1);
      expect(component.model).toBe(editQueryResult);
    });

    it('sends error to messsage service if error returned', () => {
      const error = TestHelper.buildError();
      const editService = fixture.debugElement.injector.get(EditService);
      const handleQuerySpy = spyOn(editService, 'handleQuery').and.returnValue(throwError(error));

      const messagesService = fixture.debugElement.injector.get(MessagesService);
      const addHttpResponseErrorSpy = spyOn(messagesService, 'addHttpResponseError');

      component.refreshData();

      expect(handleQuerySpy).toHaveBeenCalledTimes(1);
      expect(addHttpResponseErrorSpy).toHaveBeenCalledWith(error);
    });
  });

  describe('#onSubmit', () => {
    it('adds message to message service if successfully edited', () => {
      component.model = PurchaseOrderTestHelper.buildEditQueryResult();

      const editService = fixture.debugElement.injector.get(EditService);
      const handleCommandSpy = spyOn(editService, 'handleCommand').and.returnValue(of({}));

      const messagesService = fixture.debugElement.injector.get(MessagesService);
      const addMessageSpy = spyOn(messagesService, 'addMessage');

      component.onSubmit();

      expect(handleCommandSpy).toHaveBeenCalledTimes(1);
      expect(addMessageSpy).toHaveBeenCalledTimes(1);
    });

    it('sends error to messsage service if error returned', () => {
      component.model = PurchaseOrderTestHelper.buildEditQueryResult();
      const error = TestHelper.buildError();
      const editService = fixture.debugElement.injector.get(EditService);
      const handleCommandSpy = spyOn(editService, 'handleCommand').and.returnValue(throwError(error));

      const messagesService = fixture.debugElement.injector.get(MessagesService);
      const messagesSpy = spyOn(messagesService, 'addHttpResponseError');

      component.onSubmit();

      expect(handleCommandSpy).toHaveBeenCalledTimes(1);
      expect(messagesSpy).toHaveBeenCalledWith(error);
    });
  });

  describe('#onDelete', () => {
    it('navigates to purchase orders URL when deleted successfully', () => {
      const deleteService = fixture.debugElement.injector.get(DeleteService);
      const handleCommandSpy = spyOn(deleteService, 'handle').and.returnValue(of({}));

      const router = fixture.debugElement.injector.get(Router);
      const navigateByUrlSpy = spyOn(router, 'navigateByUrl');

      component.onDelete();

      expect(handleCommandSpy).toHaveBeenCalledTimes(1);
      expect(navigateByUrlSpy).toHaveBeenCalledWith(purchaseOrdersUrl);
    });

    it('sends error to messsage service if error returned', () => {
      const error = TestHelper.buildError();
      const deleteService = fixture.debugElement.injector.get(DeleteService);
      const handleSpy = spyOn(deleteService, 'handle').and.returnValue(throwError(error));

      const messagesService = fixture.debugElement.injector.get(MessagesService);
      const messagesSpy = spyOn(messagesService, 'addHttpResponseError');

      component.onDelete();

      expect(handleSpy).toHaveBeenCalledTimes(1);
      expect(messagesSpy).toHaveBeenCalledWith(error);
    });
  });

  describe('#hasShipmentOptions', () => {
    it("returns false if component's model is undefined", () => {
      expect(component.model).toBeUndefined();
      expect(component.hasShipmentOptions()).toBe(false);
    });

    it("returns false if component's model has no shipmentOptions", () => {
      component.model = PurchaseOrderTestHelper.buildEditQueryResult();
      expect(component.hasShipmentOptions()).toBe(false);
    });

    it("returns true if component's model has shipmentOptions", () => {
      const shipmentOptions = ['1'];
      component.model = PurchaseOrderTestHelper.buildEditQueryResult(shipmentOptions);
      expect(component.hasShipmentOptions()).toBe(true);
    });
  });
});
