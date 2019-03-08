import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { MessagesService } from '../../shared/messages/messages.service';
import { DeleteService, DeleteCommand } from './delete.service';
import { EditService, EditQuery, EditQueryResult, EditCommand } from './edit.service';
import { EditStatusService, EditStatusCommand } from './edit-status.service';
import { idParam, purchaseOrdersUrl } from '../../config/routing.config';

@Component({
  templateUrl: './edit.component.html'
})
export class EditComponent implements OnInit {
  purchaseOrderId: number;
  model: EditQueryResult;
  objectKeys = Object.keys;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private deleteService: DeleteService,
    private editService: EditService,
    private editStatusService: EditStatusService,
    private messagesService: MessagesService
  ) {}

  ngOnInit(): void {
    this.purchaseOrderId = this.route.snapshot.params[idParam];
    this.refreshData();
  }

  refreshData(): void {
    const query = new EditQuery(this.purchaseOrderId);
    this.editService
      .handleQuery(query)
      .subscribe(result => (this.model = result), err => this.messagesService.addHttpResponseError(err));
  }

  onSubmit() {
    const command = new EditCommand(
      this.purchaseOrderId,
      this.model.orderNo,
      this.model.supplierId,
      this.model.shipmentId
    );
    this.editService
      .handleCommand(command)
      .subscribe(
        () => this.messagesService.addMessage('Purchase Order updated'),
        err => this.messagesService.addHttpResponseError(err)
      );
  }

  onDelete() {
    const command = new DeleteCommand(this.purchaseOrderId);
    this.deleteService
      .handle(command)
      .subscribe(
        () => this.router.navigateByUrl(purchaseOrdersUrl),
        err => this.messagesService.addHttpResponseError(err)
      );
  }

  onUpdateStatus(status: string) {
    const command = new EditStatusCommand(this.model.id, status);
    this.editStatusService.handle(command).subscribe(
      () => {
        this.messagesService.addMessage('Status updated');
        this.refreshData();
      },
      err => this.messagesService.addHttpResponseError(err)
    );
  }

  hasShipmentOptions(): boolean {
    return this.model !== undefined && Object.keys(this.model.shipmentOptions).length > 0;
  }
}
