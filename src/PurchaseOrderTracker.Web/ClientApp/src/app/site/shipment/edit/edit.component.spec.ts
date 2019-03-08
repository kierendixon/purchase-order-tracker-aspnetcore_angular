import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { of, throwError } from 'rxjs';

import { MainSiteModule } from '../../main-site.module';
import { AppModule } from '../../../app.module';
import { EditComponent } from './edit.component';
import { MessagesService } from '../../shared/messages/messages.service';
import { EditService } from './edit.service';
import { EditStatusService } from './edit-status.service';
import { DeleteService } from './delete.service';
import { idParam, shipmentsUrl } from '../../config/routing.config';
import { TestHelper } from '../../../../test/test-helper';
import { ShipmentTestHelper } from 'src/test/shipment-test-helper';

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
    it('sets shipmentId as query param value', () => {
      const params: Params = {};
      params[idParam] = 1;

      const activatedRoute = fixture.debugElement.injector.get(ActivatedRoute) as ActivatedRoute;
      activatedRoute.snapshot.params = params;
      spyOn(component, 'refreshData');

      component.ngOnInit();

      expect(component.shipmentId).toBe(params[idParam]);
    });

    it('calls refreshData', () => {
      const refreshDataSpy = spyOn(component, 'refreshData');
      component.ngOnInit();
      expect(refreshDataSpy).toHaveBeenCalledTimes(1);
    });
  });

  describe('#refreshData', () => {
    it("updates component's model with response", () => {
      const editQueryResult = ShipmentTestHelper.buildEditQueryResult();
      const editService = fixture.debugElement.injector.get(EditService) as EditService;
      const handleQuerySpy = spyOn(editService, 'handleQuery').and.returnValue(of(editQueryResult));

      component.refreshData();

      expect(handleQuerySpy).toHaveBeenCalledTimes(1);
      expect(component.model.id).toBe(editQueryResult.id);
      expect(component.model.trackingId).toBe(editQueryResult.trackingId);
      expect(component.model.company).toBe(editQueryResult.company);
      expect(component.model.comments).toBe(editQueryResult.comments);
      expect(component.model.shippingCost).toBe(editQueryResult.shippingCost);
      expect(component.model.destinationAddress).toBe(editQueryResult.destinationAddress);
      expect(component.model.isDelivered).toBe(editQueryResult.isDelivered);
      expect(component.model.canTransitionToAwaitingShipping).toBe(editQueryResult.canTransitionToAwaitingShipping);
      expect(component.model.canTransitionToShipped).toBe(editQueryResult.canTransitionToShipped);
      expect(component.model.canTransitionToDelivered).toBe(editQueryResult.canTransitionToDelivered);
      // TODO expect(component.model.estimatedArrivalDate).toBe(editQueryResult.estimatedArrivalDate);
    });

    it('sends error to messsage service if error returned', () => {
      const error = TestHelper.buildError();
      const editService = fixture.debugElement.injector.get(EditService) as EditService;
      const handleQuerySpy = spyOn(editService, 'handleQuery').and.returnValue(throwError(error));

      const messagesService = fixture.debugElement.injector.get(MessagesService) as MessagesService;
      const messagesSpy = spyOn(messagesService, 'addHttpResponseError');

      component.refreshData();

      expect(handleQuerySpy).toHaveBeenCalledTimes(1);
      expect(messagesSpy).toHaveBeenCalledWith(error);
    });
  });

  describe('#onSubmit', () => {
    it('adds message to message service if successfully edited', () => {
      component.model = ShipmentTestHelper.buildEditViewModel();
      component.shipmentId = 1;

      const editService = fixture.debugElement.injector.get(EditService) as EditService;
      const handleCommandSpy = spyOn(editService, 'handleCommand').and.returnValue(of({}));

      const messagesService = fixture.debugElement.injector.get(MessagesService) as MessagesService;
      const addMessageSpy = spyOn(messagesService, 'addMessage');

      component.onSubmit();

      expect(handleCommandSpy).toHaveBeenCalledTimes(1);
      expect(addMessageSpy).toHaveBeenCalledTimes(1);
    });

    it('sends error to messsage service if error returned', () => {
      component.model = ShipmentTestHelper.buildEditViewModel();
      const error = TestHelper.buildError();
      const editService = fixture.debugElement.injector.get(EditService) as EditService;
      const handleCommandSpy = spyOn(editService, 'handleCommand').and.returnValue(throwError(error));

      const messagesService = fixture.debugElement.injector.get(MessagesService) as MessagesService;
      const messagesSpy = spyOn(messagesService, 'addHttpResponseError');

      component.onSubmit();

      expect(handleCommandSpy).toHaveBeenCalledTimes(1);
      expect(messagesSpy).toHaveBeenCalledWith(error);
    });
  });

  describe('#onDelete', () => {
    it('navigates to shipments URL when deleted successfully', () => {
      component.model = ShipmentTestHelper.buildEditViewModel();
      const deleteService = fixture.debugElement.injector.get(DeleteService) as DeleteService;
      const handleCommandSpy = spyOn(deleteService, 'handle').and.returnValue(of({}));

      const router = fixture.debugElement.injector.get(Router) as Router;
      const navigateByUrlSpy = spyOn(router, 'navigateByUrl');

      component.onDelete();

      expect(handleCommandSpy).toHaveBeenCalledTimes(1);
      expect(navigateByUrlSpy).toHaveBeenCalledWith(shipmentsUrl);
    });

    it('sends error to messsage service if error returned', () => {
      component.model = ShipmentTestHelper.buildEditViewModel();
      const error = TestHelper.buildError();
      const deleteService = fixture.debugElement.injector.get(DeleteService) as DeleteService;
      const handleSpy = spyOn(deleteService, 'handle').and.returnValue(throwError(error));

      const messagesService = fixture.debugElement.injector.get(MessagesService) as MessagesService;
      const messagesSpy = spyOn(messagesService, 'addHttpResponseError');

      component.onDelete();

      expect(handleSpy).toHaveBeenCalledTimes(1);
      expect(messagesSpy).toHaveBeenCalledWith(error);
    });
  });

  describe('#onUpdateStatus', () => {
    it('adds message to message service if successfully edited', () => {
      component.model = ShipmentTestHelper.buildEditViewModel();

      const editStatusService = fixture.debugElement.injector.get(EditStatusService) as EditStatusService;
      const handleSpy = spyOn(editStatusService, 'handle').and.returnValue(of({}));

      const messagesService = fixture.debugElement.injector.get(MessagesService) as MessagesService;
      const addMessageSpy = spyOn(messagesService, 'addMessage');

      component.onUpdateStatus('status');

      expect(handleSpy).toHaveBeenCalledTimes(1);
      expect(addMessageSpy).toHaveBeenCalledTimes(1);
    });

    it('sends error to messsage service if error returned', () => {
      component.model = ShipmentTestHelper.buildEditViewModel();
      const error = TestHelper.buildError();
      const editStatusService = fixture.debugElement.injector.get(EditStatusService) as EditStatusService;
      const handleSpy = spyOn(editStatusService, 'handle').and.returnValue(throwError(error));

      const messagesService = fixture.debugElement.injector.get(MessagesService) as MessagesService;
      const messagesSpy = spyOn(messagesService, 'addHttpResponseError');

      component.onUpdateStatus('status');

      expect(handleSpy).toHaveBeenCalledTimes(1);
      expect(messagesSpy).toHaveBeenCalledWith(error);
    });
  });
});
