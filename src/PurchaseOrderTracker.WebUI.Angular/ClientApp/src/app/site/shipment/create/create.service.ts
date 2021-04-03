import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { baseShipmentUrl } from '../../config/api.config';

@Injectable()
export class CreateService {
  constructor(private http: HttpClient) {}

  public handle(command: CreateCommand): Observable<CreateResult> {
    return this.http.put<CreateResult>(baseShipmentUrl, command);
  }
}

export class CreateCommand {
  constructor(
    readonly trackingId: string,
    readonly company: string,
    readonly estimatedArrivalDate: Date,
    readonly shippingCost: number,
    readonly destinationAddress: string,
    readonly comments: string
  ) {}
}

export interface CreateResult {
  id: number;
}
