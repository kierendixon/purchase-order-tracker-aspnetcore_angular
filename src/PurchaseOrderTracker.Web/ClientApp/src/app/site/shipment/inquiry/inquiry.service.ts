import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { PaginatedList } from '../../shared/pagination/paginated-list';
import { shipmentInquiryUrl } from '../../config/api.config';

@Injectable()
export class InquiryService {
    constructor(private http: HttpClient) {
    }

    public handle(query: InquiryQuery): Observable<InquiryResult> {
        const url = shipmentInquiryUrl(query.queryType, query.pageNumber.toString());
        return this.http.get<InquiryResult>(url);
    }
}

export class InquiryQuery {
    constructor(readonly queryType: string,
        readonly pageNumber: number) {
    }
}

export interface InquiryResult {
    pagedList: PaginatedList<ResultShipment>;
}

export interface ResultShipment {
    id: number;
    trackingId: string;
    company: string;
    estimatedArrivalDate: Date;
    comments: string;
    shippingCost: number;
    status: string;
    destinationAddress: string;
    isDelayed: boolean;
    isDelayedMoreThan7Days: boolean;
    isScheduledForDeliveryToday: boolean;
}
