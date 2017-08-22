import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { MainSiteComponent } from '../main-site.component';
import { PurchaseOrderComponent } from './purchase-order.component';
import { InquiryComponent } from './inquiry/inquiry.component';
import { CreateComponent } from './create/create.component';
import { EditComponent } from './edit/edit.component';
import { EditLineItemsComponent } from './edit-line-items/edit-line-items.component';

import { AuthGuard } from '../../infrastructure/security/auth-guard.service';

const poRoutes: Routes = [
    {
        path: 'main-site',
        component: MainSiteComponent,
        canActivate: [AuthGuard],
        children: [
            {
                path: 'purchase-orders',
                component: PurchaseOrderComponent,
                children: [
                    { path: 'inquiry', component: InquiryComponent },
                    { path: 'create', component: CreateComponent },
                    { path: ':id', component: EditComponent },
                    { path: ':id/edit-line-items', component: EditLineItemsComponent }
                ]
            }
        ]
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(poRoutes)
    ],
    exports: [
        RouterModule
    ]
})
export class PurchaseOrderRoutingModule { }