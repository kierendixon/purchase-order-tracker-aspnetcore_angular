import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { MainSiteComponent } from '../main-site.component';
import { SupplierComponent } from './supplier.component';
import { InquiryComponent } from './inquiry/inquiry.component';
import { CreateComponent } from './create/create.component';
import { EditComponent } from './edit/edit.component';
import { EditProductsComponent } from './edit-products/edit-products.component';
import { EditProductCategoriesComponent } from './edit-product-categories/edit-product-categories.component';

import { AuthGuard } from '../../infrastructure/security/auth-guard';

const supplierRoutes: Routes = [
    {
        path: 'main-site',
        component: MainSiteComponent,
        canActivate: [AuthGuard],
        children: [
            {
                path: 'suppliers',
                component: SupplierComponent,
                children: [
                    { path: 'inquiry', component: InquiryComponent },
                    { path: 'create', component: CreateComponent },
                    { path: ':id', component: EditComponent },
                    { path: ':id/edit-products', component: EditProductsComponent },
                    { path: ':id/edit-product-categories', component: EditProductCategoriesComponent }
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
export class SupplierRoutingModule { }
