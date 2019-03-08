import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { idParam } from '../../config/routing.config';
import { MessagesService } from '../../shared/messages/messages.service';
import { CreateLineItemComponent } from '../create-line-item/create-line-item.component';
import { DeleteCommand, DeleteLineItemService } from './delete-line-item.service';
import { EditLineItemCommand, EditLineItemService } from './edit-line-item.service';
import {
  EditLineItemsQuery,
  EditLineItemsResult,
  EditLineItemsResultItem,
  EditLineItemsService
} from './edit-line-items.service';

@Component({
  templateUrl: './edit-line-items.component.html'
})
export class EditLineItemsComponent implements OnInit {
  objectKeys = Object.keys;
  purchaseOrderId: number;
  model: EditLineItemsResult;

  constructor(
    private route: ActivatedRoute,
    private messagesService: MessagesService,
    private editLineItemService: EditLineItemService,
    private editLineItemsService: EditLineItemsService,
    private deleteLineItemService: DeleteLineItemService,
    private modalService: NgbModal
  ) {}

  ngOnInit(): void {
    this.purchaseOrderId = this.route.snapshot.params[idParam];
    this.refreshData();
  }

  refreshData(): void {
    const query = new EditLineItemsQuery(this.purchaseOrderId);
    this.editLineItemsService
      .handle(query)
      .subscribe(result => (this.model = result), err => this.messagesService.addHttpResponseError(err));
  }

  showAddLineItemModal() {
    const modalRef = this.modalService.open(CreateLineItemComponent);
    modalRef.componentInstance.productOptions = this.model.productOptions;
    modalRef.componentInstance.purchaseOrderId = this.purchaseOrderId;
    modalRef.result.then(result => {
      if (result) {
        this.messagesService.addMessage(result);
        this.refreshData();
      }
    });
  }

  onDeleteLineItem(index: number): void {
    const lineItem = this.model.lineItems[index];
    const command = new DeleteCommand(this.purchaseOrderId, lineItem.id);
    this.deleteLineItemService.handle(command).subscribe(
      result => {
        this.messagesService.addMessage(`Line item deleted: (${lineItem.id})`);
        this.refreshData();
      },
      err => this.messagesService.addHttpResponseError(err)
    );
  }

  onSubmitEditLineItem(index: number): void {
    const lineItem = this.model.lineItems[index];
    const command = this.buildEditLineItemCommand(lineItem);
    this.editLineItemService
      .handle(command)
      .subscribe(
        result => this.messagesService.addMessage('Line Item updated'),
        err => this.messagesService.addHttpResponseError(err)
      );
  }

  private buildEditLineItemCommand(lineItem: EditLineItemsResultItem): EditLineItemCommand {
    return new EditLineItemCommand(
      this.purchaseOrderId,
      lineItem.id,
      lineItem.productId,
      lineItem.purchasePrice,
      lineItem.purchaseQty
    );
  }

  hasLineItems(): boolean {
    return this.model ? this.model.lineItems.length > 0 : false;
  }
}
