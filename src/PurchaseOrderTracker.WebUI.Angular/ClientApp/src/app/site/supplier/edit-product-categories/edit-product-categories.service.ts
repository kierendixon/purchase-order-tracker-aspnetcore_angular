import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { supplierProductCategoriesUrl } from '../../config/api.config';
import { PaginatedList } from '../../shared/pagination/paginated-list';

@Injectable()
export class EditProductCategoriesService {
  constructor(private http: HttpClient) {}

  public handle(query: EditProductCategoriesQuery): Observable<EditProductCategoriesResult> {
    const url = supplierProductCategoriesUrl(query.id, query.pageNumber.toString());
    return this.http.get<EditProductCategoriesResult>(url);
  }
}

export class EditProductCategoriesQuery {
  constructor(readonly id: number, readonly pageNumber: number) {}
}

export interface EditProductCategoriesResult {
  supplierName: string;
  categories: PaginatedList<ProductCategoryResult>;
}

export class ProductCategoryResult {
  id: number;
  name: string;
}
