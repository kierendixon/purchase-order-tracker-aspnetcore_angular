import { async, ComponentFixture, fakeAsync, TestBed, tick } from '@angular/core/testing';
import { ActivatedRoute, Params } from '@angular/router';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { of, throwError } from 'rxjs';

import { SupplierTestHelper } from 'src/test/supplier-test-helper';
import { TestHelper } from '../../../../test/test-helper';
import { AppModule } from '../../../app.module';
import { idParam, pageNumberQueryParam } from '../../config/routing.config';
import { MainSiteModule } from '../../main-site.module';
import { MessagesService } from '../../shared/messages/messages.service';
import { CreateProductCategoryComponent } from '../create-product-category/create-product-category.component';
import { DeleteProductCategoryService, DeleteResult } from './delete-product-category.service';
import { EditProductCategoriesComponent } from './edit-product-categories.component';
import { EditProductCategoriesService } from './edit-product-categories.service';
import { EditProductCategoryResult, EditProductCategoryService } from './edit-product-category.service';

describe('EditProductCategoriesComponent', () => {
  let component: EditProductCategoriesComponent;
  let fixture: ComponentFixture<EditProductCategoriesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [AppModule, MainSiteModule],
      providers: [NgbActiveModal]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditProductCategoriesComponent);
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
    it("updates component's model with response", () => {
      const editCategoriesResult = SupplierTestHelper.buildEditProductCategoriesResult();

      const editProductCategoriesService = fixture.debugElement.injector.get(EditProductCategoriesService);
      const handleSpy = spyOn(editProductCategoriesService, 'handle').and.returnValue(of(editCategoriesResult));

      component.refreshData();

      expect(handleSpy).toHaveBeenCalledTimes(1);
      expect(component.supplierName).toBe(editCategoriesResult.supplierName);
      expect(component.model.hasNextPage).toBe(editCategoriesResult.categories.hasNextPage);
      expect(component.model.hasPreviousPage).toBe(editCategoriesResult.categories.hasPreviousPage);
      expect(component.model.items).toBe(editCategoriesResult.categories.items);
      expect(component.model.pageCount).toBe(editCategoriesResult.categories.pageCount);
      expect(component.model.pageNumber).toBe(editCategoriesResult.categories.pageNumber);
      expect(component.model.pageSize).toBe(editCategoriesResult.categories.pageSize);
    });

    it('sends error to messsage service if error returned', () => {
      const error = TestHelper.buildError();
      const editProductCategoriesService = fixture.debugElement.injector.get(EditProductCategoriesService);
      const handleSpy = spyOn(editProductCategoriesService, 'handle').and.returnValue(throwError(error));

      const messagesService = fixture.debugElement.injector.get(MessagesService);
      const messagesSpy = spyOn(messagesService, 'addHttpResponseError');

      component.refreshData();

      expect(handleSpy).toHaveBeenCalledTimes(1);
      expect(messagesSpy).toHaveBeenCalledWith(error);
    });
  });

  describe('#showAddProductCategoryModal', () => {
    it('opens a CreateProductCategoryComponent modal and sets the supplierId', () => {
      component.supplierId = 1;
      const modal = fixture.debugElement.injector.get(NgbModal);
      const modalRef = modal.open(CreateProductCategoryComponent);
      const openSpy = spyOn(modal, 'open').and.returnValue(modalRef);
      spyOn(modalRef, 'result');

      component.showAddProductCategoryModal();

      expect(openSpy).toHaveBeenCalledWith(CreateProductCategoryComponent);
      expect(modalRef.componentInstance.supplierId).toBe(component.supplierId);
    });

    it('adds a message to mesageService and calls refreshData', fakeAsync(() => {
      const modal = fixture.debugElement.injector.get(NgbModal);
      const modalRef = modal.open(CreateProductCategoryComponent);
      modalRef.result = Promise.resolve({});
      spyOn(modal, 'open').and.returnValue(modalRef);

      const messagesService = fixture.debugElement.injector.get(MessagesService);
      const addMessageSpy = spyOn(messagesService, 'addMessage');
      const refreshDataSpy = spyOn(component, 'refreshData');

      component.showAddProductCategoryModal();
      tick();

      expect(addMessageSpy).toHaveBeenCalledTimes(1);
      expect(refreshDataSpy).toHaveBeenCalledTimes(1);
    }));

    it('does not add a message to mesageService or call refreshData if modal returns an undefined response', () => {
      const modal = fixture.debugElement.injector.get(NgbModal);
      const modalRef = modal.open(CreateProductCategoryComponent);
      modalRef.result = Promise.resolve(undefined);
      spyOn(modal, 'open').and.returnValue(modalRef);

      const messagesService = fixture.debugElement.injector.get(MessagesService);
      const addMessageSpy = spyOn(messagesService, 'addMessage');
      const refreshDataSpy = spyOn(component, 'refreshData');

      component.showAddProductCategoryModal();

      expect(addMessageSpy).not.toHaveBeenCalled();
      expect(refreshDataSpy).not.toHaveBeenCalled();
    });
  });

  describe('#onDeleteCategory', () => {
    const modelIndex = 0;

    beforeEach(() => {
      component.model = SupplierTestHelper.buildPaginatedListOfProductCategory(1);
    });

    it('adds message to messageService and calls refreshData', () => {
      const result: DeleteResult = {};
      const deleteProductCategoryService = fixture.debugElement.injector.get(DeleteProductCategoryService);
      const handleSpy = spyOn(deleteProductCategoryService, 'handle').and.returnValue(of(result));

      const refreshDataSpy = spyOn(component, 'refreshData');

      const messagesService = fixture.debugElement.injector.get(MessagesService);
      const addMessageSpy = spyOn(messagesService, 'addMessage');

      component.onDeleteCategory(modelIndex);

      expect(handleSpy).toHaveBeenCalledTimes(1);
      expect(refreshDataSpy).toHaveBeenCalledTimes(1);
      expect(addMessageSpy).toHaveBeenCalledTimes(1);
    });

    it('sends error to messsage service if error returned', () => {
      const error = TestHelper.buildError();
      const deleteProductCategoryService = fixture.debugElement.injector.get(DeleteProductCategoryService);
      const handleSpy = spyOn(deleteProductCategoryService, 'handle').and.returnValue(throwError(error));

      const messagesService = fixture.debugElement.injector.get(MessagesService);
      const messagesSpy = spyOn(messagesService, 'addHttpResponseError');

      component.onDeleteCategory(modelIndex);

      expect(handleSpy).toHaveBeenCalledTimes(1);
      expect(messagesSpy).toHaveBeenCalledWith(error);
    });
  });

  describe('#onSubmitEditCategory', () => {
    const modelIndex = 0;

    beforeEach(() => {
      component.model = SupplierTestHelper.buildPaginatedListOfProductCategory(1);
    });

    it('adds message to messageService', () => {
      const result: EditProductCategoryResult = {};
      const editProductCategoryService = fixture.debugElement.injector.get(EditProductCategoryService);
      const handleSpy = spyOn(editProductCategoryService, 'handle').and.returnValue(of(result));

      const messagesService = fixture.debugElement.injector.get(MessagesService);
      const addMessageSpy = spyOn(messagesService, 'addMessage');

      component.onSubmitEditCategory(modelIndex);

      expect(handleSpy).toHaveBeenCalledTimes(1);
      expect(addMessageSpy).toHaveBeenCalledTimes(1);
    });

    it('sends error to messsage service if error returned', () => {
      const error = TestHelper.buildError();
      const editProductCategoryService = fixture.debugElement.injector.get(EditProductCategoryService);
      const handleSpy = spyOn(editProductCategoryService, 'handle').and.returnValue(throwError(error));

      const messagesService = fixture.debugElement.injector.get(MessagesService);
      const messagesSpy = spyOn(messagesService, 'addHttpResponseError');

      component.onSubmitEditCategory(modelIndex);

      expect(handleSpy).toHaveBeenCalledTimes(1);
      expect(messagesSpy).toHaveBeenCalledWith(error);
    });
  });

  describe('#hasCategories', () => {
    it("returns false if component's model is undefined", () => {
      expect(component.model).toBeUndefined();
      expect(component.hasCategories()).toBe(false);
    });

    it("returns false if component's model has no items", () => {
      component.model = SupplierTestHelper.buildPaginatedListOfProductCategory(0);
      expect(component.model.items.length).toBe(0);
      expect(component.hasCategories()).toBe(false);
    });

    it("returns true if component's model has items", () => {
      component.model = SupplierTestHelper.buildPaginatedListOfProductCategory(1);
      expect(component.model.items.length).not.toBe(0);
      expect(component.hasCategories()).toBe(true);
    });
  });
});
