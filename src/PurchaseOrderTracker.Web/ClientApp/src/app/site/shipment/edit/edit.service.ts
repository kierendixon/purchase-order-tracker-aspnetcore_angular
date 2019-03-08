import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { shipmentUrl } from '../../config/api.config';

@Injectable()
export class EditService {
  constructor(private http: HttpClient) {}

  public handleQuery(query: EditQuery): Observable<EditQueryResult> {
    return this.http.get<EditQueryResult>(shipmentUrl(query.id));
  }

  public handleCommand(command: EditCommand): Observable<EditCommandResult> {
    return this.http.post<EditCommandResult>(shipmentUrl(command.id), command);
  }
}

export class EditQuery {
  constructor(readonly id: number) {}
}

export interface EditQueryResult {
  id: number;
  trackingId: string;
  company: string;
  comments: string;
  estimatedArrivalDate: string;
  shippingCost: number;
  destinationAddress: string;
  isDelivered: boolean;
  canTransitionToAwaitingShipping: boolean;
  canTransitionToShipped: boolean;
  canTransitionToDelivered: boolean;
}

export class EditCommand {
  constructor(
    readonly id: number,
    readonly trackingId: string,
    readonly company: string,
    readonly estimatedArrivalDate: Date,
    readonly comments: string,
    readonly shippingCost: number,
    readonly destinationAddress: string
  ) {}
}

// tslint:disable-next-line:no-empty-interface
export interface EditCommandResult {}
