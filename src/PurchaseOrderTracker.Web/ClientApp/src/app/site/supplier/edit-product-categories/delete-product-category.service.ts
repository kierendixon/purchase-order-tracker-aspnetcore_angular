import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { supplierProductCategoryUrl } from '../../config/api.config';

@Injectable()
export class DeleteProductCategoryService {
  constructor(private http: HttpClient) {}

  public handle(command: DeleteCommand): Observable<DeleteResult> {
    return this.http.delete<DeleteResult>(supplierProductCategoryUrl(command.supplierId, command.categoryId));
  }
}

export class DeleteCommand {
  constructor(readonly supplierId: number, readonly categoryId: number) {}
}

// tslint:disable-next-line:no-empty-interface
export interface DeleteResult {}
