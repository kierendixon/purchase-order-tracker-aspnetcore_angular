import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute, Params } from '@angular/router';
import { of, throwError } from 'rxjs';

import { PurchaseOrderTestHelper } from '../../../../test/purchase-order-test-helper';
import { TestHelper } from '../../../../test/test-helper';
import { AppModule } from '../../../app.module';
import { pageNumberQueryParam, queryTypeQueryParam } from '../../config/routing.config';
import { MainSiteModule } from '../../main-site.module';
import { MessagesService } from '../../shared/messages/messages.service';
import { PurchaseOrderModule } from '../purchase-order.module';
import { InquiryComponent } from './inquiry.component';
import { InquiryService } from './inquiry.service';

describe('InquiryComponent', () => {
  let component: InquiryComponent;
  let fixture: ComponentFixture<InquiryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [AppModule, MainSiteModule, PurchaseOrderModule]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InquiryComponent);
    component = fixture.componentInstance;
  });

  it('constructs', () => {
    expect(component).toBeDefined();
  });

  describe('#ngOnInit', () => {
    it('calls refreshData with the provided query type and page number', () => {
      const queryTypeParam = 'queryTypeParam';
      const pageNumberParam = 1;
      const queryParams: Params = {};
      queryParams[queryTypeQueryParam] = queryTypeParam;
      queryParams[pageNumberQueryParam] = pageNumberParam;

      const route = fixture.debugElement.injector.get(ActivatedRoute);
      route.queryParams = of(queryParams);
      const refreshDataSpy = spyOn(component, 'refreshData');

      component.ngOnInit();

      expect(refreshDataSpy).toHaveBeenCalledWith(queryTypeParam, pageNumberParam);
    });

    it('calls refreshData with the default page number if no query param is provided', () => {
      const queryTypeParam = 'queryTypeParam';
      const defaultPageNumber = 1;
      const queryParams: Params = {};
      queryParams[queryTypeQueryParam] = queryTypeParam;

      const route = fixture.debugElement.injector.get(ActivatedRoute);
      route.queryParams = of(queryParams);
      const refreshDataSpy = spyOn(component, 'refreshData');

      component.ngOnInit();

      expect(refreshDataSpy).toHaveBeenCalledWith(queryTypeParam, defaultPageNumber);
    });
  });

  describe('#refreshData', () => {
    it("updates component's model with response", () => {
      const model = PurchaseOrderTestHelper.buildInquiryResult();
      const inquiryService = fixture.debugElement.injector.get(InquiryService);
      const handleSpy = spyOn(inquiryService, 'handle').and.returnValue(of(model));

      component.refreshData('queryType', 1);

      expect(handleSpy).toHaveBeenCalledTimes(1);
      expect(component.model).toEqual(model);
    });

    it('sends error to messsage service if error returned', () => {
      const error = TestHelper.buildError();
      const inquiryService = fixture.debugElement.injector.get(InquiryService);
      const handleSpy = spyOn(inquiryService, 'handle').and.returnValue(throwError(error));

      const messagesService = fixture.debugElement.injector.get(MessagesService);
      const addHttpResponseErrorSpy = spyOn(messagesService, 'addHttpResponseError');

      component.refreshData('queryType', 1);

      expect(handleSpy).toHaveBeenCalledTimes(1);
      expect(addHttpResponseErrorSpy).toHaveBeenCalledWith(error);
    });
  });

  describe('#hasRecords', () => {
    it("returns false if component's model is undefined", () => {
      expect(component.model).toBeUndefined();
      expect(component.hasRecords()).toBe(false);
    });

    it("returns false if component's model has no items", () => {
      component.model = PurchaseOrderTestHelper.buildInquiryResult();
      expect(component.hasRecords()).toBe(false);
    });

    it("returns true if component's model has items", () => {
      component.model = PurchaseOrderTestHelper.buildInquiryResultWithItemsCount(1);
      expect(component.hasRecords()).toBe(true);
    });
  });
});
