import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AuthGuard } from '../../infrastructure/security/auth-guard.service';
import { MainSiteComponent } from '../main-site.component';
import { ShipmentComponent } from './shipment.component';
import { InquiryComponent } from './inquiry/inquiry.component';
import { CreateComponent } from './create/create.component';
import { EditComponent } from './edit/edit.component';

const supplierRoutes: Routes = [
    {
        path: 'main-site',
        component: MainSiteComponent,
        canActivate: [AuthGuard],
        children: [
            {
                path: 'shipments',
                component: ShipmentComponent,
                children: [
                    { path: 'inquiry', component: InquiryComponent },
                    { path: 'create', component: CreateComponent },
                    { path: ':id', component: EditComponent }
                ]
            }
        ]
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(supplierRoutes)
    ],
    exports: [
        RouterModule
    ]
})
export class ShipmentRoutingModule { }