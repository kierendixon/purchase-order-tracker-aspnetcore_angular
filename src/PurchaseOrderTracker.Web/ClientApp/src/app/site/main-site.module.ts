import { NgModule } from '@angular/core';

import { MainSiteRoutingModule } from './main-site-routing.module';
import { PurchaseOrderModule } from './purchase-order/purchase-order.module';
import { SharedModule } from './shared/shared.module';
import { ShipmentModule } from './shipment/shipment.module';
import { SupplierModule } from './supplier/supplier.module';

import { MainSiteLandingComponent } from './main-site-landing.component';
import { MainSiteComponent } from './main-site.component';

import { MainSiteLandingResolver } from './main-site-landing-resolver.service';
import { MainSiteService } from './main-site.service';

@NgModule({
  imports: [MainSiteRoutingModule, SharedModule, SupplierModule, ShipmentModule, PurchaseOrderModule],
  declarations: [MainSiteComponent, MainSiteLandingComponent],
  providers: [MainSiteService, MainSiteLandingResolver]
})
export class MainSiteModule {}
