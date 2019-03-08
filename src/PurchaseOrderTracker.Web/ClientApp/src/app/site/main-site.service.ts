import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { reportingShipmentSummaryUrl } from './config/api.config';

@Injectable()
export class MainSiteService {
  constructor(private http: HttpClient) {}

  public handleShipmentSummaryQuery(): Observable<ShipmentSummaryResult> {
    return this.http.get<ShipmentSummaryResult>(reportingShipmentSummaryUrl);
  }
}

export interface ShipmentSummaryResult {
  totalOpenOrders: number;
  shipmentsSchedForDeliveryToday: number;
  shipmentsDelayed: number;
  shipmentsDelayedMoreThan7Days: number;
}
