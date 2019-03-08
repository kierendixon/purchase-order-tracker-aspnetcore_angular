import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { SharedModule } from '../shared/shared.module';
import { SupplierRoutingModule } from './supplier-routing.module';

import { SupplierComponent } from './supplier.component';
import { InquiryComponent } from './inquiry/inquiry.component';
import { CreateComponent } from './create/create.component';
import { EditComponent } from './edit/edit.component';
import { CreateProductComponent } from './create-product/create-product.component';
import { EditProductsComponent } from './edit-products/edit-products.component';
import { CreateProductCategoryComponent } from './create-product-category/create-product-category.component';
import { EditProductCategoriesComponent } from './edit-product-categories/edit-product-categories.component';

import { CreateProductService } from './create-product/create-product.service';
import { CreateProductCategoryService } from './create-product-category/create-product-category.service';
import { CreateService } from './create/create.service';
import { DeleteService } from './edit/delete.service';
import { EditService } from './edit/edit.service';
import { DeleteProductCategoryService } from './edit-product-categories/delete-product-category.service';
import { EditProductCategoriesService } from './edit-product-categories/edit-product-categories.service';
import { EditProductCategoryService } from './edit-product-categories/edit-product-category.service';
import { DeleteProductService } from './edit-products/delete-product.service';
import { EditProductsService } from './edit-products/edit-products.service';
import { EditProductService } from './edit-products/edit-product.service';
import { InquiryService } from './inquiry/inquiry.service';

@NgModule({
  imports: [CommonModule, FormsModule, HttpClientModule, NgbModule, SharedModule, SupplierRoutingModule],
  declarations: [
    SupplierComponent,
    InquiryComponent,
    CreateComponent,
    EditComponent,
    EditProductsComponent,
    CreateProductComponent,
    EditProductCategoriesComponent,
    CreateProductCategoryComponent
  ],
  entryComponents: [CreateProductComponent, CreateProductCategoryComponent],
  providers: [
    CreateProductService,
    CreateProductCategoryService,
    CreateService,
    DeleteService,
    EditService,
    DeleteProductCategoryService,
    EditProductCategoriesService,
    EditProductCategoryService,
    DeleteProductService,
    EditProductsService,
    EditProductService,
    InquiryService
  ]
})
export class SupplierModule {}
