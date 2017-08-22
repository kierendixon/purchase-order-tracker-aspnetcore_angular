import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { PaginatedList } from '../../shared/pagination/paginated-list';
import { supplierInquiryUrl } from '../../config/api.config';

@Injectable()
export class InquiryService {
    constructor(private http: HttpClient) {
    }

    public handle(query: InquiryQuery): Observable<InquiryResult> {
        return this.http.get<InquiryResult>(supplierInquiryUrl(query.queryType, query.pageNumber.toString()));
    }
}

export class InquiryQuery {
    constructor(readonly queryType: string,
        readonly pageNumber: number){
    }
}

export interface InquiryResult{
    pagedList: PaginatedList<ResultSupplier>;
}

interface ResultSupplier {
    id: string;
    name: string;
}