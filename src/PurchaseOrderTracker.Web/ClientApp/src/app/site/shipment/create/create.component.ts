import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap/datepicker/ngb-date-struct';

import { editShipmentUrl } from '../../config/routing.config';
import { MessagesService } from '../../shared/messages/messages.service';
import { CreateCommand, CreateService } from './create.service';

@Component({
  templateUrl: './create.component.html'
})
export class CreateComponent {
  model = new ShipmentViewModel();

  constructor(private router: Router, private messagesService: MessagesService, private createService: CreateService) {}

  onSubmit() {
    const command = this.buildCommand();
    this.createService
      .handle(command)
      .subscribe(
        result => this.router.navigateByUrl(editShipmentUrl(result.id)),
        err => this.messagesService.addHttpResponseError(err)
      );
  }

  private buildCommand(): CreateCommand {
    const etaAsDate: Date | undefined =
      this.model.eta === undefined
        ? undefined
        : new Date(this.model.eta.year, this.model.eta.month, this.model.eta.day);
    return new CreateCommand(
      this.model.trackingId,
      this.model.company,
      etaAsDate,
      this.model.shippingCost,
      this.model.destinationAddress,
      this.model.comments
    );
  }
}

class ShipmentViewModel {
  trackingId: string;
  company: string;
  eta: NgbDateStruct;
  shippingCost: number;
  destinationAddress: string;
  comments: string;
}
