import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { supplierProductUrl } from '../../config/api.config';

@Injectable()
export class DeleteProductService {
  constructor(private http: HttpClient) {}

  public handle(command: DeleteCommand): Observable<DeleteResult> {
    const url = supplierProductUrl(command.supplierId, command.productId);
    return this.http.delete<DeleteResult>(url);
  }
}

export class DeleteCommand {
  constructor(readonly supplierId: number, readonly productId: number) {}
}

// tslint:disable-next-line:no-empty-interface
export interface DeleteResult {}
