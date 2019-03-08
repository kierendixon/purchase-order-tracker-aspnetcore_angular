import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { SharedModule } from '../shared/shared.module';
import { ShipmentRoutingModule } from './shipment-routing.module';

import { CreateComponent } from './create/create.component';
import { EditComponent } from './edit/edit.component';
import { InquiryComponent } from './inquiry/inquiry.component';
import { ShipmentComponent } from './shipment.component';

import { CreateService } from './create/create.service';
import { DeleteService } from './edit/delete.service';
import { EditStatusService } from './edit/edit-status.service';
import { EditService } from './edit/edit.service';
import { InquiryService } from './inquiry/inquiry.service';

@NgModule({
  imports: [CommonModule, FormsModule, HttpClientModule, NgbModule, SharedModule, ShipmentRoutingModule],
  declarations: [ShipmentComponent, InquiryComponent, CreateComponent, EditComponent],
  providers: [CreateService, EditService, DeleteService, EditStatusService, InquiryService]
})
export class ShipmentModule {}
