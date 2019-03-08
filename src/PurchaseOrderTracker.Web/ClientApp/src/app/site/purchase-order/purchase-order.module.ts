import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { SharedModule } from '../shared/shared.module';
import { PurchaseOrderRoutingModule } from './purchase-order-routing.module';

import { CreateLineItemComponent } from './create-line-item/create-line-item.component';
import { CreateComponent } from './create/create.component';
import { EditLineItemsComponent } from './edit-line-items/edit-line-items.component';
import { EditComponent } from './edit/edit.component';
import { InquiryComponent } from './inquiry/inquiry.component';
import { PurchaseOrderComponent } from './purchase-order.component';

import { CreateLineItemService } from './create-line-item/create-line-item.service';
import { CreateService } from './create/create.service';
import { DeleteLineItemService } from './edit-line-items/delete-line-item.service';
import { EditLineItemService } from './edit-line-items/edit-line-item.service';
import { EditLineItemsService } from './edit-line-items/edit-line-items.service';
import { DeleteService } from './edit/delete.service';
import { EditStatusService } from './edit/edit-status.service';
import { EditService } from './edit/edit.service';
import { InquiryService } from './inquiry/inquiry.service';

@NgModule({
  imports: [CommonModule, FormsModule, HttpClientModule, NgbModule, SharedModule, PurchaseOrderRoutingModule],
  declarations: [
    PurchaseOrderComponent,
    InquiryComponent,
    CreateComponent,
    EditComponent,
    EditLineItemsComponent,
    CreateLineItemComponent
  ],
  entryComponents: [CreateLineItemComponent],
  providers: [
    CreateService,
    EditService,
    EditStatusService,
    DeleteService,
    InquiryService,
    EditLineItemsService,
    EditLineItemService,
    DeleteLineItemService,
    CreateLineItemService
  ]
})
export class PurchaseOrderModule {}
