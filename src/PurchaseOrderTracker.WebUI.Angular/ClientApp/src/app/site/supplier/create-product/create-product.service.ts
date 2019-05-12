import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { supplierProductsUrl } from '../../config/api.config';

@Injectable()
export class CreateProductService {
  constructor(private http: HttpClient) {}

  public handle(command: CreateProductCommand): Observable<CreateProductResult> {
    return this.http.put<CreateProductResult>(supplierProductsUrl(command.supplierId), command);
  }
}

export class CreateProductCommand {
  constructor(
    readonly supplierId,
    readonly prodCode: string,
    readonly name: string,
    readonly categoryId: number,
    readonly price: number
  ) {}
}

// tslint:disable-next-line:no-empty-interface
export interface CreateProductResult {}
