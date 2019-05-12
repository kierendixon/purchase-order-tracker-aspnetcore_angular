import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { shipmentSummaryResultResolverDataKey } from './config/routing.config';
import { ShipmentSummaryResult } from './main-site.service';

@Component({
  templateUrl: './main-site-landing.component.html'
})
export class MainSiteLandingComponent implements OnInit {
  model: ShipmentSummaryResult;

  constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.route.data.subscribe(data => {
      if (data && data[shipmentSummaryResultResolverDataKey]) {
        this.model = data[shipmentSummaryResultResolverDataKey];
      }
    });
  }
}
