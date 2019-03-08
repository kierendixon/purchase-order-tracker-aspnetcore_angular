import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { purchaseOrderUrl } from '../../config/api.config';

@Injectable()
export class EditService {
  constructor(private http: HttpClient) {}

  public handleQuery(query: EditQuery): Observable<EditQueryResult> {
    return this.http.get<EditQueryResult>(purchaseOrderUrl(query.id));
  }

  public handleCommand(command: EditCommand): Observable<EditCommandResult> {
    return this.http.post<EditCommandResult>(purchaseOrderUrl(command.id), command);
  }
}

export class EditQuery {
  constructor(readonly id: number) {}
}

export interface EditQueryResult {
  id: number;
  createdDate: Date;
  supplierId: number;
  shipmentId: number;
  shipmentTrackingId: number;
  currentState: string;
  orderNo: string;

  isDelivered: boolean;
  isCancelled: boolean;
  isApproved: boolean;
  canTransitionToPendingApproval: boolean;
  canTransitionToApproved: boolean;
  canTransitionToCancelled: boolean;
  canShipmentBeUpdated: boolean;

  supplierOptions: SelectOption[];
  shipmentOptions: SelectOption[];
}

export class EditCommand {
  constructor(
    readonly id: number,
    readonly orderNo: string,
    readonly supplierId: number,
    readonly shipmentId: number
  ) {}
}

// tslint:disable-next-line:no-empty-interface
export interface EditCommandResult {}

class SelectOption {
  id: number;
  value: string;
}
