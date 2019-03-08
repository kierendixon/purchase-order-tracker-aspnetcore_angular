import { Component, Input } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';

import { pageNumberQueryParam } from '../../config/routing.config';
import { PaginatedList } from './paginated-list';

@Component({
  // TODO: remove tslint ignore
  // tslint:disable-next-line:component-selector
  selector: 'pagination',
  templateUrl: './pagination.component.html'
})
export class PaginationComponent {
  @Input() paginatedList: PaginatedList<any>;
  private readonly queryStringStartChar = '?';

  constructor(private router: Router, private route: ActivatedRoute) {}

  pageNumbers(): number[] {
    const pageNumbers: number[] = [];
    for (let i = 0; i < this.paginatedList.pageCount; i++) {
      pageNumbers.push(i + 1);
    }
    return pageNumbers;
  }

  queryParamsWithPageNumber(pageNumber: number): Params {
    const queryParams = {};
    this.route.queryParams.subscribe((data: Params) => {
      // TODO: remove tslint ignore
      // tslint:disable-next-line:forin
      for (const key in data) {
        queryParams[key] = data[key];
      }
    });
    queryParams[pageNumberQueryParam] = pageNumber;

    return queryParams;
  }

  urlWithoutQueryParams(): string {
    return this.router.url.split(this.queryStringStartChar)[0];
  }
}
