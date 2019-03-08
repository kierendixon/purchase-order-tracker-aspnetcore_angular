import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { supplierProductCategoriesUrl } from '../../config/api.config';

@Injectable()
export class CreateProductCategoryService {
  constructor(private http: HttpClient) {}

  public handle(command: CreateProductCategoryCommand): Observable<CreateProductCategoryResult> {
    return this.http.put<CreateProductCategoryResult>(supplierProductCategoriesUrl(command.supplierId), command);
  }
}

export class CreateProductCategoryCommand {
  constructor(readonly supplierId: number, readonly name: string) {}
}

// tslint:disable-next-line:no-empty-interface
export interface CreateProductCategoryResult {}
