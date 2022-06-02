import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { editPurchaseOrderUrl } from '../../config/routing.config';
import { MessagesService } from '../../shared/messages/messages.service';
import { CreateCommand, CreateService, SuppliersQuery } from './create.service';

@Component({
  templateUrl: './create.component.html'
})
export class CreateComponent implements OnInit {
  model = new PurchaseOrderViewModel();
  supplierOptions: any;
  objectKeys = Object.keys;

  constructor(private router: Router, private messagesService: MessagesService, private createService: CreateService) {}

  ngOnInit(): void {
    const query = new SuppliersQuery();
    this.createService.handleSuppliersQuery(query).subscribe(
      result => (this.supplierOptions = result.suppliers),
      err => this.messagesService.addHttpResponseError(err)
    );
  }

  onSubmit() {
    const command = new CreateCommand(this.model.orderNo, parseInt(this.model.supplierId, 10));
    this.createService.handleCommand(command).subscribe(
      result => this.router.navigateByUrl(editPurchaseOrderUrl(result.orderId)),
      err => this.messagesService.addHttpResponseError(err)
    );
  }
}

class PurchaseOrderViewModel {
  orderNo: string;
  supplierId: string;
}
