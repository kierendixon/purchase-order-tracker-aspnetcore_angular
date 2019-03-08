import { async, ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { ActivatedRoute, Params } from '@angular/router';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { of, throwError } from 'rxjs';

import { MainSiteModule } from '../../main-site.module';
import { AppModule } from '../../../app.module';
import { EditProductsComponent } from './edit-products.component';
import { CreateProductComponent } from '../create-product/create-product.component';
import { MessagesService } from '../../shared/messages/messages.service';
import { EditProductsService } from './edit-products.service';
import { EditProductService, EditProductResult } from './edit-product.service';
import { DeleteProductService, DeleteResult } from './delete-product.service';
import { idParam, pageNumberQueryParam } from '../../config/routing.config';
import { TestHelper } from '../../../../test/test-helper';
import { SupplierTestHelper } from 'src/test/supplier-test-helper';

describe('EditProductsComponent', () => {
  let component: EditProductsComponent;
  let fixture: ComponentFixture<EditProductsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [AppModule, MainSiteModule],
      providers: [NgbActiveModal] // TODO
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditProductsComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeDefined();
  });

  describe('#ngOnInit', () => {
    it('sets supplierId as query param value', () => {
      const params: Params = {};
      params[idParam] = 1;

      const activatedRoute = fixture.debugElement.injector.get(ActivatedRoute);
      activatedRoute.snapshot.params = params;
      activatedRoute.snapshot.queryParams = of({});

      component.ngOnInit();

      expect(component.supplierId).toBe(params[idParam]);
    });

    it('sets pageNumber with the provided query param and calls refreshData', () => {
      const queryParams: Params = {};
      queryParams[pageNumberQueryParam] = 1;

      const route = fixture.debugElement.injector.get(ActivatedRoute);
      route.queryParams = of(queryParams);
      const refreshDataSpy = spyOn(component, 'refreshData');

      component.ngOnInit();

      expect(component.pageNumber).toBe(queryParams[pageNumberQueryParam]);
      expect(refreshDataSpy).toHaveBeenCalledTimes(1);
    });

    it('sets pageNumber with the default page number if no query param is provided and calls refreshData', () => {
      const defaultPageNumber = 1;
      const queryParams: Params = {};

      const route = fixture.debugElement.injector.get(ActivatedRoute);
      route.queryParams = of(queryParams);
      const refreshDataSpy = spyOn(component, 'refreshData');

      component.ngOnInit();

      expect(component.pageNumber).toBe(defaultPageNumber);
      expect(refreshDataSpy).toHaveBeenCalledTimes(1);
    });
  });

  describe('#refreshData', () => {
    it('resets pageNumber if param is true', () => {
      const editProductsService = fixture.debugElement.injector.get(EditProductsService);
      spyOn(editProductsService, 'handle').and.returnValue(of(SupplierTestHelper.buildEditProductsResult()));

      component.refreshData(true);

      expect(component.pageNumber).toBe(1);
    });

    it('does not reset pageNUmber if param is false', () => {
      const pageNumber = 5;
      component.pageNumber = pageNumber;

      const editProductsService = fixture.debugElement.injector.get(EditProductsService);
      spyOn(editProductsService, 'handle').and.returnValue(of(SupplierTestHelper.buildEditProductsResult()));

      component.refreshData(false);

      expect(component.pageNumber).toBe(pageNumber);
    });

    it("updates component's model with response", () => {
      const editProductsResult = SupplierTestHelper.buildEditProductsResult();

      const editProductsService = fixture.debugElement.injector.get(EditProductsService);
      const handleSpy = spyOn(editProductsService, 'handle').and.returnValue(of(editProductsResult));

      component.refreshData();

      expect(handleSpy).toHaveBeenCalledTimes(1);
      expect(component.productsAreFiltered).toBe(editProductsResult.productsAreFiltered);
      expect(component.categoryOptions).toBe(editProductsResult.categoryOptions);
      expect(component.supplierName).toBe(editProductsResult.supplierName);
      expect(component.model.hasNextPage).toBe(editProductsResult.products.hasNextPage);
      expect(component.model.hasPreviousPage).toBe(editProductsResult.products.hasPreviousPage);
      expect(component.model.items).toBe(editProductsResult.products.items);
      expect(component.model.pageCount).toBe(editProductsResult.products.pageCount);
      expect(component.model.pageNumber).toBe(editProductsResult.products.pageNumber);
      expect(component.model.pageSize).toBe(editProductsResult.products.pageSize);
    });

    it('sends error to messsage service if error returned', () => {
      const error = TestHelper.buildError();
      const editProductsService = fixture.debugElement.injector.get(EditProductsService);
      const handleSpy = spyOn(editProductsService, 'handle').and.returnValue(throwError(error));

      const messagesService = fixture.debugElement.injector.get(MessagesService);
      const messagesSpy = spyOn(messagesService, 'addHttpResponseError');

      component.refreshData();

      expect(handleSpy).toHaveBeenCalledTimes(1);
      expect(messagesSpy).toHaveBeenCalledWith(error);
    });
  });

  describe('#showAddProductModal', () => {
    it('opens a CreateProductComponent modal and sets categoryOptions and supplierId', () => {
      component.supplierId = 1;
      component.categoryOptions = SupplierTestHelper.buildEditProductsResult().categoryOptions;
      const modal = fixture.debugElement.injector.get(NgbModal);
      const modalRef = modal.open(CreateProductComponent);
      const openSpy = spyOn(modal, 'open').and.returnValue(modalRef);

      component.showAddProductModal();

      expect(openSpy).toHaveBeenCalledWith(CreateProductComponent);
      expect(modalRef.componentInstance.supplierId).toBe(component.supplierId);
      expect(modalRef.componentInstance.categoryOptions).toBe(component.categoryOptions);
    });

    it('adds a message to mesageService and calls refreshData', fakeAsync(() => {
      const modal = fixture.debugElement.injector.get(NgbModal);
      const modalRef = modal.open(CreateProductComponent);
      modalRef.result = Promise.resolve({});
      spyOn(modal, 'open').and.returnValue(modalRef);

      const messagesService = fixture.debugElement.injector.get(MessagesService);
      const addMessageSpy = spyOn(messagesService, 'addMessage');
      const refreshDataSpy = spyOn(component, 'refreshData');

      component.showAddProductModal();
      tick();

      expect(addMessageSpy).toHaveBeenCalledTimes(1);
      expect(refreshDataSpy).toHaveBeenCalledTimes(1);
    }));

    it('does not add a message to mesageService or call refreshData if modal returns an undefined response', () => {
      const modal = fixture.debugElement.injector.get(NgbModal);
      const modalRef = modal.open(CreateProductComponent);
      modalRef.result = Promise.resolve(undefined);
      spyOn(modal, 'open').and.returnValue(modalRef);

      const messagesService = fixture.debugElement.injector.get(MessagesService);
      const addMessageSpy = spyOn(messagesService, 'addMessage');
      const refreshDataSpy = spyOn(component, 'refreshData');

      component.showAddProductModal();

      expect(addMessageSpy).not.toHaveBeenCalled();
      expect(refreshDataSpy).not.toHaveBeenCalled();
    });
  });

  describe('#onDeleteProduct', () => {
    const modelIndex = 0;

    beforeEach(() => {
      component.model = SupplierTestHelper.buildPaginatedListOfEditProductViewModel(1);
    });

    it('adds message to messageService and calls refreshData', () => {
      const result: DeleteResult = {};
      const deleteProductService = fixture.debugElement.injector.get(DeleteProductService);
      const handleSpy = spyOn(deleteProductService, 'handle').and.returnValue(of(result));

      const refreshDataSpy = spyOn(component, 'refreshData');

      const messagesService = fixture.debugElement.injector.get(MessagesService);
      const addMessageSpy = spyOn(messagesService, 'addMessage');

      component.onDeleteProduct(modelIndex);

      expect(handleSpy).toHaveBeenCalledTimes(1);
      expect(refreshDataSpy).toHaveBeenCalledTimes(1);
      expect(addMessageSpy).toHaveBeenCalledTimes(1);
    });

    it('sends error to messsage service if error returned', () => {
      const error = TestHelper.buildError();
      const deleteProductService = fixture.debugElement.injector.get(DeleteProductService);
      const handleSpy = spyOn(deleteProductService, 'handle').and.returnValue(throwError(error));

      const messagesService = fixture.debugElement.injector.get(MessagesService);
      const messagesSpy = spyOn(messagesService, 'addHttpResponseError');

      component.onDeleteProduct(modelIndex);

      expect(handleSpy).toHaveBeenCalledTimes(1);
      expect(messagesSpy).toHaveBeenCalledWith(error);
    });
  });

  describe('#onSubmitEditProduct', () => {
    const modelIndex = 0;

    beforeEach(() => {
      component.model = SupplierTestHelper.buildPaginatedListOfEditProductViewModel(1);
    });

    it('adds message to messageService', () => {
      const result: EditProductResult = {};
      const editProductService = fixture.debugElement.injector.get(EditProductService);
      const handleSpy = spyOn(editProductService, 'handle').and.returnValue(of(result));

      const messagesService = fixture.debugElement.injector.get(MessagesService);
      const addMessageSpy = spyOn(messagesService, 'addMessage');

      component.onSubmitEditProduct(modelIndex);

      expect(handleSpy).toHaveBeenCalledTimes(1);
      expect(addMessageSpy).toHaveBeenCalledTimes(1);
    });

    it('sends error to messsage service if error returned', () => {
      const error = TestHelper.buildError();
      const editProductService = fixture.debugElement.injector.get(EditProductService);
      const handleSpy = spyOn(editProductService, 'handle').and.returnValue(throwError(error));

      const messagesService = fixture.debugElement.injector.get(MessagesService);
      const messagesSpy = spyOn(messagesService, 'addHttpResponseError');

      component.onSubmitEditProduct(modelIndex);

      expect(handleSpy).toHaveBeenCalledTimes(1);
      expect(messagesSpy).toHaveBeenCalledWith(error);
    });
  });

  describe('#onSearch', () => {
    it('calls refreshData', () => {
      const prodCode = 'prodCode';
      const refreshDataSpy = spyOn(component, 'refreshData');

      component.onSearch(prodCode);

      expect(refreshDataSpy).toHaveBeenCalledWith(true, prodCode);
    });
  });

  describe('#onClearSearch', () => {
    it('calls refreshData', () => {
      const refreshDataSpy = spyOn(component, 'refreshData');

      component.onClearSearch();

      expect(refreshDataSpy).toHaveBeenCalledTimes(1);
    });
  });

  describe('#hasProducts', () => {
    it("returns false if component's model is undefined", () => {
      expect(component.model).toBeUndefined();
      expect(component.hasProducts()).toBe(false);
    });

    it("returns false if component's model has no items", () => {
      component.model = SupplierTestHelper.buildPaginatedListOfEditProductViewModel(0);
      expect(component.model.items.length).toBe(0);
      expect(component.hasProducts()).toBe(false);
    });

    it("returns true if component's model has items", () => {
      component.model = SupplierTestHelper.buildPaginatedListOfEditProductViewModel(1);
      expect(component.model.items.length).not.toBe(0);
      expect(component.hasProducts()).toBe(true);
    });
  });
});
