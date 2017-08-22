import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { SharedModule } from '../shared/shared.module';
import { PurchaseOrderRoutingModule } from './purchase-order-routing.module';

import { PurchaseOrderComponent } from './purchase-order.component';
import { InquiryComponent } from './inquiry/inquiry.component';
import { CreateComponent } from './create/create.component';
import { EditComponent } from './edit/edit.component';
import { EditLineItemsComponent } from './edit-line-items/edit-line-items.component';
import { CreateLineItemComponent } from './create-line-item/create-line-item.component';

import { CreateService } from './create/create.service';
import { DeleteService } from './edit/delete.service';
import { EditService } from './edit/edit.service';
import { EditStatusService } from './edit/edit-status.service';
import { InquiryService } from './inquiry/inquiry.service';
import { EditLineItemsService } from './edit-line-items/edit-line-items.service';
import { EditLineItemService } from './edit-line-items/edit-line-item.service';
import { DeleteLineItemService } from './edit-line-items/delete-line-item.service';
import { CreateLineItemService } from './create-line-item/create-line-item.service';

@NgModule({
    imports: [CommonModule, FormsModule, HttpClientModule, NgbModule, SharedModule, PurchaseOrderRoutingModule],
    declarations: [PurchaseOrderComponent, InquiryComponent, CreateComponent, EditComponent, EditLineItemsComponent,
        CreateLineItemComponent],
    entryComponents: [CreateLineItemComponent],
    providers: [CreateService, EditService, EditStatusService, DeleteService, InquiryService,
        EditLineItemsService, EditLineItemService, DeleteLineItemService, CreateLineItemService]
})
export class PurchaseOrderModule { }