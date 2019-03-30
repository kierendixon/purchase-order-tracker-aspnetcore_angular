import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { of, throwError } from 'rxjs';

import { TestHelper } from '../../../../test/test-helper';
import { AppModule } from '../../../app.module';
import { MainSiteModule } from '../../main-site.module';
import { MessagesService } from '../../shared/messages/messages.service';
import { SupplierModule } from '../supplier.module';
import { CreateProductCategoryComponent } from './create-product-category.component';
import { CreateProductCategoryService } from './create-product-category.service';

describe('CreateProductCategoryComponent', () => {
  let component: CreateProductCategoryComponent;
  let fixture: ComponentFixture<CreateProductCategoryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [AppModule, MainSiteModule, SupplierModule],
      providers: [NgbActiveModal]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateProductCategoryComponent);
    component = fixture.componentInstance;
  });

  it('constructs', () => {
    expect(component).toBeDefined();
  });

  describe('#onSubmit', () => {
    it('closes modal when created successfully', () => {
      component.supplierId = 1;
      component.model = {
        name: 'name'
      };
      const createService = fixture.debugElement.injector.get(CreateProductCategoryService);
      const createServiceSpy = spyOn(createService, 'handle').and.returnValue(of({}));

      const activeModal = fixture.debugElement.injector.get(NgbActiveModal);
      const activeModalSpy = spyOn(activeModal, 'close');

      component.onSubmit();

      expect(createServiceSpy).toHaveBeenCalledTimes(1);
      expect(activeModalSpy).toHaveBeenCalledTimes(1);
    });

    it('sends error to messsage service if error returned', () => {
      const error = TestHelper.buildError();
      const createService = fixture.debugElement.injector.get(CreateProductCategoryService);
      const createServiceSpy = spyOn(createService, 'handle').and.returnValue(throwError(error));

      const messagesService = fixture.debugElement.injector.get(MessagesService);
      const messagesSpy = spyOn(messagesService, 'addHttpResponseError');

      component.onSubmit();

      expect(createServiceSpy).toHaveBeenCalledTimes(1);
      expect(messagesSpy).toHaveBeenCalledWith(error);
    });
  });
});
