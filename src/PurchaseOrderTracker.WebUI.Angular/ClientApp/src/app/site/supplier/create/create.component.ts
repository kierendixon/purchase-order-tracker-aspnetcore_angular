import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { editSupplierUrl } from '../../config/routing.config';
import { MessagesService } from '../../shared/messages/messages.service';
import { CreateCommand, CreateService } from './create.service';

@Component({
  templateUrl: './create.component.html'
})
export class CreateComponent {
  model = new SupplierViewModel();

  constructor(private router: Router, private createService: CreateService, private messagesService: MessagesService) {}

  onSubmit() {
    const command = new CreateCommand(this.model.name);
    this.createService
      .handle(command)
      .subscribe(
        result => this.router.navigateByUrl(editSupplierUrl(result.supplierId)),
        err => this.messagesService.addHttpResponseError(err)
      );
  }
}

class SupplierViewModel {
  name: string;
}
