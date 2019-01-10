import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/observable';

import { PaginatedList } from '../../shared/pagination/paginated-list';
import { purchaseOrderInquiryUrl } from '../../config/api.config';

@Injectable()
export class InquiryService {
    constructor(private http: HttpClient) {
    }

    public handle(query: InquiryQuery): Observable<InquiryResult> {
        return this.http.get<InquiryResult>(purchaseOrderInquiryUrl(query.queryType, query.pageNumber.toString()));
    }
}

export class InquiryQuery {
    constructor(readonly queryType: string,
        readonly pageNumber: number) {
    }
}

export interface InquiryResult {
    pagedList: PaginatedList<ResultPurchaseOrder>;
}

interface ResultPurchaseOrder {
    id: string;
    orderNo: string;
    createdDate: Date;
    supplierName: string;
    currentState: string;
}
