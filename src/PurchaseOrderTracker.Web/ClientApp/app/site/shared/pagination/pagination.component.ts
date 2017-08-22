import { Component, Input } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { PaginatedList } from './paginated-list';
import { pageNumberQueryParam } from '../../config/routing.config';

@Component({
    selector: 'pagination',
    templateUrl: './pagination.component.html'
})
export class PaginationComponent {
    @Input() paginatedList: PaginatedList<any>;
    private readonly queryStringStartChar = '?';

    constructor(private router: Router, private route: ActivatedRoute) {
    }

    pageNumbers(): number[] {
        let pageNumbers: number[] = [];
        for (var i = 0; i < this.paginatedList.pageCount; i++) {
            pageNumbers.push(i + 1);
        }
        return pageNumbers;
    }

    pageNavQueryParams(pageNumber: number): any {
        let queryParams = {};
        this.route.queryParams.subscribe(data => {
            for (let key in data) queryParams[key] = data[key];
        });
        queryParams[pageNumberQueryParam] = pageNumber;
        return queryParams;
    }

    urlWithoutQueryParams(): string {
        return this.router.url.split(this.queryStringStartChar)[0];
    }
}