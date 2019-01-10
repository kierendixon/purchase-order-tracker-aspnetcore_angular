import { NgModule } from '@angular/core';

import { SharedModule } from './shared/shared.module';
import { SupplierModule } from './supplier/supplier.module';
import { ShipmentModule } from './shipment/shipment.module';
import { PurchaseOrderModule } from './purchase-order/purchase-order.module';
import { MainSiteRoutingModule } from './main-site-routing.module';

import { MainSiteComponent } from './main-site.component';
import { MainSiteLandingComponent } from './main-site-landing.component';

import { MainSiteService } from './main-site.service';
import { MainSiteLandingResolver } from './main-site-landing-resolver.service';

@NgModule({
    imports: [MainSiteRoutingModule, SharedModule, SupplierModule, ShipmentModule, PurchaseOrderModule],
    declarations: [MainSiteComponent, MainSiteLandingComponent],
    providers: [MainSiteService, MainSiteLandingResolver]
})
export class MainSiteModule { }
