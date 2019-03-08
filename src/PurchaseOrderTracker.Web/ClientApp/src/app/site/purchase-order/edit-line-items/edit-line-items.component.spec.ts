import { async, ComponentFixture, fakeAsync, TestBed, tick } from '@angular/core/testing';
import { ActivatedRoute, Params } from '@angular/router';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { of, throwError } from 'rxjs';

import { PurchaseOrderTestHelper } from '../../../../test/purchase-order-test-helper';
import { TestHelper } from '../../../../test/test-helper';
import { AppModule } from '../../../app.module';
import { idParam, pageNumberQueryParam } from '../../config/routing.config';
import { MainSiteModule } from '../../main-site.module';
import { MessagesService } from '../../shared/messages/messages.service';
import { CreateLineItemComponent } from '../create-line-item/create-line-item.component';
import { DeleteLineItemService, DeleteResult } from './delete-line-item.service';
import { EditLineItemResult, EditLineItemService } from './edit-line-item.service';
import { EditLineItemsComponent } from './edit-line-items.component';
import { EditLineItemsService } from './edit-line-items.service';

describe('EditLineItemsComponent', () => {
  let component: EditLineItemsComponent;
  let fixture: ComponentFixture<EditLineItemsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [AppModule, MainSiteModule],
      providers: [NgbActiveModal] // TODO
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditLineItemsComponent);
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
      activatedRoute.snapshot.queryParams = of({});
      spyOn(component, 'refreshData');

      component.ngOnInit();

      expect(component.purchaseOrderId).toBe(params[idParam]);
    });

    it('calls refreshData', () => {
      const queryParams: Params = {};
      queryParams[pageNumberQueryParam] = 1;

      const route = fixture.debugElement.injector.get(ActivatedRoute);
      route.queryParams = of(queryParams);
      const refreshDataSpy = spyOn(component, 'refreshData');

      component.ngOnInit();

      expect(refreshDataSpy).toHaveBeenCalledTimes(1);
    });
  });

  describe('#refreshData', () => {
    it("updates component's model with response", () => {
      const editLineItemsResult = PurchaseOrderTestHelper.buildEditLineItemsResult();

      const editLineItemsService = fixture.debugElement.injector.get(EditLineItemsService);
      const handleSpy = spyOn(editLineItemsService, 'handle').and.returnValue(of(editLineItemsResult));

      component.refreshData();

      expect(handleSpy).toHaveBeenCalledTimes(1);
      expect(component.model).toBe(editLineItemsResult);
    });

    it('sends error to messsage service if error returned', () => {
      const error = TestHelper.buildError();
      const editLineItemsService = fixture.debugElement.injector.get(EditLineItemsService);
      const handleSpy = spyOn(editLineItemsService, 'handle').and.returnValue(throwError(error));

      const messagesService = fixture.debugElement.injector.get(MessagesService);
      const addHttpResponseErrorSpy = spyOn(messagesService, 'addHttpResponseError');

      component.refreshData();

      expect(handleSpy).toHaveBeenCalledTimes(1);
      expect(addHttpResponseErrorSpy).toHaveBeenCalledWith(error);
    });
  });

  describe('#showAddLineItemModal', () => {
    const editLineItemsResult = PurchaseOrderTestHelper.buildEditLineItemsResult();

    beforeEach(() => {
      component.model = editLineItemsResult;
      component.purchaseOrderId = 1;
    });

    it('opens a CreateLineItemComponent modal and sets the purchaseOrderId', () => {
      const modal = fixture.debugElement.injector.get(NgbModal);
      const modalRef = modal.open(CreateLineItemComponent);
      const openSpy = spyOn(modal, 'open').and.returnValue(modalRef);
      spyOn(modalRef, 'result');

      component.showAddLineItemModal();

      expect(openSpy).toHaveBeenCalledWith(CreateLineItemComponent);
      expect(modalRef.componentInstance.purchaseOrderId).toBe(component.purchaseOrderId);
      expect(modalRef.componentInstance.productOptions).toBe(editLineItemsResult.productOptions);
    });

    it('adds a message to mesageService and calls refreshData', fakeAsync(() => {
      const modal = fixture.debugElement.injector.get(NgbModal);
      const modalRef = modal.open(CreateLineItemComponent);
      modalRef.result = Promise.resolve({});
      spyOn(modal, 'open').and.returnValue(modalRef);

      const messagesService = fixture.debugElement.injector.get(MessagesService);
      const addMessageSpy = spyOn(messagesService, 'addMessage');
      const refreshDataSpy = spyOn(component, 'refreshData');

      component.showAddLineItemModal();
      tick();

      expect(addMessageSpy).toHaveBeenCalledTimes(1);
      expect(refreshDataSpy).toHaveBeenCalledTimes(1);
    }));

    it('does not add a message to mesageService or call refreshData if modal returns an undefined response', () => {
      const modal = fixture.debugElement.injector.get(NgbModal);
      const modalRef = modal.open(CreateLineItemComponent);
      modalRef.result = Promise.resolve(undefined);
      spyOn(modal, 'open').and.returnValue(modalRef);

      const messagesService = fixture.debugElement.injector.get(MessagesService);
      const addMessageSpy = spyOn(messagesService, 'addMessage');
      const refreshDataSpy = spyOn(component, 'refreshData');

      component.showAddLineItemModal();

      expect(addMessageSpy).not.toHaveBeenCalled();
      expect(refreshDataSpy).not.toHaveBeenCalled();
    });
  });

  describe('#onDeleteLineItem', () => {
    const modelIndex = 0;

    beforeEach(() => {
      const items = PurchaseOrderTestHelper.buildEditLineItemsResultItemWithCount(1);
      component.model = PurchaseOrderTestHelper.buildEditLineItemsResult(items);
    });

    it('adds message to messageService and calls refreshData', () => {
      const result: DeleteResult = {};
      const deleteLineItemService = fixture.debugElement.injector.get(DeleteLineItemService);
      const handleSpy = spyOn(deleteLineItemService, 'handle').and.returnValue(of(result));

      const refreshDataSpy = spyOn(component, 'refreshData');

      const messagesService = fixture.debugElement.injector.get(MessagesService);
      const addMessageSpy = spyOn(messagesService, 'addMessage');

      component.onDeleteLineItem(modelIndex);

      expect(handleSpy).toHaveBeenCalledTimes(1);
      expect(refreshDataSpy).toHaveBeenCalledTimes(1);
      expect(addMessageSpy).toHaveBeenCalledTimes(1);
    });

    it('sends error to messsage service if error returned', () => {
      const error = TestHelper.buildError();
      const deleteLineItemService = fixture.debugElement.injector.get(DeleteLineItemService);
      const handleSpy = spyOn(deleteLineItemService, 'handle').and.returnValue(throwError(error));

      const messagesService = fixture.debugElement.injector.get(MessagesService);
      const messagesSpy = spyOn(messagesService, 'addHttpResponseError');

      component.onDeleteLineItem(modelIndex);

      expect(handleSpy).toHaveBeenCalledTimes(1);
      expect(messagesSpy).toHaveBeenCalledWith(error);
    });
  });

  describe('#onSubmitEditLineItem', () => {
    const modelIndex = 0;

    beforeEach(() => {
      const items = PurchaseOrderTestHelper.buildEditLineItemsResultItemWithCount(1);
      component.model = PurchaseOrderTestHelper.buildEditLineItemsResult(items);
    });

    it('adds message to messageService', () => {
      const result: EditLineItemResult = {};
      const editLineItemService = fixture.debugElement.injector.get(EditLineItemService);
      const handleSpy = spyOn(editLineItemService, 'handle').and.returnValue(of(result));

      const messagesService = fixture.debugElement.injector.get(MessagesService);
      const addMessageSpy = spyOn(messagesService, 'addMessage');

      component.onSubmitEditLineItem(modelIndex);

      expect(handleSpy).toHaveBeenCalledTimes(1);
      expect(addMessageSpy).toHaveBeenCalledTimes(1);
    });

    it('sends error to messsage service if error returned', () => {
      const error = TestHelper.buildError();
      const editLineItemService = fixture.debugElement.injector.get(EditLineItemService);
      const handleSpy = spyOn(editLineItemService, 'handle').and.returnValue(throwError(error));

      const messagesService = fixture.debugElement.injector.get(MessagesService);
      const messagesSpy = spyOn(messagesService, 'addHttpResponseError');

      component.onSubmitEditLineItem(modelIndex);

      expect(handleSpy).toHaveBeenCalledTimes(1);
      expect(messagesSpy).toHaveBeenCalledWith(error);
    });
  });

  describe('#hasLineItems', () => {
    it("returns false if component's model is undefined", () => {
      expect(component.model).toBeUndefined();
      expect(component.hasLineItems()).toBe(false);
    });

    it("returns false if component's model has no items", () => {
      component.model = PurchaseOrderTestHelper.buildEditLineItemsResult();
      expect(component.model.lineItems.length).toBe(0);
      expect(component.hasLineItems()).toBe(false);
    });

    it("returns true if component's model has items", () => {
      const items = PurchaseOrderTestHelper.buildEditLineItemsResultItemWithCount(1);
      component.model = PurchaseOrderTestHelper.buildEditLineItemsResult(items);
      expect(component.model.lineItems.length).not.toBe(0);
      expect(component.hasLineItems()).toBe(true);
    });
  });
});
