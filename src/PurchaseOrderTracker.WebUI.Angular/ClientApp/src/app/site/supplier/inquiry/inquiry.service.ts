import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { supplierInquiryUrl } from '../../config/api.config';
import { PaginatedList } from '../../shared/pagination/paginated-list';

@Injectable()
export class InquiryService {
  constructor(private http: HttpClient) {}

  public handle(query: InquiryQuery): Observable<InquiryResult> {
    return this.http.get<InquiryResult>(supplierInquiryUrl(query.queryType, query.pageNumber.toString()));
  }
}

export class InquiryQuery {
  constructor(readonly queryType: string, readonly pageNumber: number) {}
}

export interface InquiryResult {
  pagedList: PaginatedList<ResultSupplier>;
}

export interface ResultSupplier {
  id: string;
  name: string;
}
