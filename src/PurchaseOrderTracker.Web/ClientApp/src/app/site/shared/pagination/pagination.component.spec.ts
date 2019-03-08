import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { of } from 'rxjs';

import { MainSiteModule } from '../../main-site.module';
import { AppModule } from '../../../app.module';
import { PaginationComponent } from './pagination.component';
import { PaginatedList } from './paginated-list';
import { pageNumberQueryParam } from '../../config/routing.config';

describe('PaginationComponent', () => {
  let component: PaginationComponent;
  let fixture: ComponentFixture<PaginationComponent>;
  const pageCount = 5;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [AppModule, MainSiteModule]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PaginationComponent);
    component = fixture.componentInstance;
    const paginatedList = new PaginatedList<any>(null, pageCount, 1, 5, false, true);

    component.paginatedList = paginatedList;
  });

  it('should create', () => {
    expect(component).toBeDefined();
  });

  describe('#pageNumbers', () => {
    it('returns an array of consecutive page numbers for the page count', () => {
      const pageNumbers = component.pageNumbers();

      expect(pageNumbers.length).toBe(pageCount);
      expect(pageNumbers[0]).toBe(1);
      expect(pageNumbers[1]).toBe(2);
      expect(pageNumbers[2]).toBe(3);
      expect(pageNumbers[3]).toBe(4);
      expect(pageNumbers[4]).toBe(5);
    });
  });

  describe('#urlWithoutQueryParams', () => {
    it('returns the url without query parameters', () => {
      const url = 'a-test/url';
      const urlWithQueryParams = url + '?param1=a&param2=b';

      const router = fixture.debugElement.injector.get(Router);
      spyOnProperty(router, 'url', 'get').and.returnValue(urlWithQueryParams);

      expect(component.urlWithoutQueryParams()).toBe(url);
    });
  });

  describe('#queryParamsWithPageNumber', () => {
    it('returns current query params with page number', () => {
      const queryParams: Params = {
        param1: 'param1value',
        param2: 'param1value',
        param3: 'param1value'
      };

      const route = fixture.debugElement.injector.get(ActivatedRoute);
      route.queryParams = of(queryParams);

      const pageNumber = 3;
      const queryParamsWithPageNumber = component.queryParamsWithPageNumber(pageNumber);

      expect(Object.keys(queryParamsWithPageNumber).length).toBe(4);
      expect(queryParamsWithPageNumber[pageNumberQueryParam]).toBe(pageNumber);
      expect(queryParamsWithPageNumber.param1).toBe(queryParams.param1);
      expect(queryParamsWithPageNumber.param2).toBe(queryParams.param2);
      expect(queryParamsWithPageNumber.param3).toBe(queryParams.param3);
    });

    it('returns just page number when there are no query params', () => {
      const route = fixture.debugElement.injector.get(ActivatedRoute);
      route.queryParams = of({});

      const pageNumber = 3;
      const queryParamsWithPageNumber = component.queryParamsWithPageNumber(pageNumber);

      expect(Object.keys(queryParamsWithPageNumber).length).toBe(1);
      expect(queryParamsWithPageNumber[pageNumberQueryParam]).toBe(pageNumber);
    });
  });
});
