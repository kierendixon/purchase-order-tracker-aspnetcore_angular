import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { supplierProductsUrl } from '../../config/api.config';
import { PaginatedList } from '../../shared/pagination/paginated-list';

@Injectable()
export class EditProductsService {
  constructor(private http: HttpClient) {}

  public handle(query: EditProductsQuery): Observable<EditProductsResult> {
    const url = supplierProductsUrl(query.id, query.pageNumber.toString(), query.productCodeFilter);
    return this.http.get<EditProductsResult>(url);
  }
}

export class EditProductsQuery {
  constructor(readonly id: number, readonly pageNumber?: number, readonly productCodeFilter?: string) {}
}

export interface EditProductsResult {
  products: PaginatedList<EditProductsResultProduct>;
  productsAreFiltered: boolean;
  categoryOptions: any;
  supplierId: number;
  supplierName: string;
}

export interface EditProductsResultProduct {
  productId: number;
  prodCode: string;
  name: string;
  categoryId: number;
  price: number;
}
