import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { baseSupplierUrl } from '../../config/api.config';

@Injectable()
export class CreateService {
  constructor(private http: HttpClient) {}

  public handle(command: CreateCommand): Observable<CreateResult> {
    return this.http.put<CreateResult>(baseSupplierUrl, command);
  }
}

export class CreateCommand {
  constructor(readonly name: string) {}
}
export interface CreateResult {
  supplierId: number;
}
