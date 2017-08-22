import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { SharedModule } from '../shared/shared.module';
import { ShipmentRoutingModule } from './shipment-routing.module';

import { ShipmentComponent } from './shipment.component';
import { InquiryComponent } from './inquiry/inquiry.component';
import { CreateComponent } from './create/create.component';
import { EditComponent } from './edit/edit.component';

import { CreateService } from './create/create.service';
import { EditService } from './edit/edit.service';
import { DeleteService } from './edit/delete.service';
import { EditStatusService } from './edit/edit-status.service';
import { InquiryService } from './inquiry/inquiry.service';

@NgModule({
    imports: [CommonModule, FormsModule, HttpClientModule, NgbModule,
        SharedModule, ShipmentRoutingModule],
    declarations: [ShipmentComponent, InquiryComponent, CreateComponent, EditComponent],
    providers: [CreateService, EditService, DeleteService, EditStatusService, InquiryService]
})
export class ShipmentModule { }

