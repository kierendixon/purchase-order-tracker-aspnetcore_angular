import { Component, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { MessagesService } from '../../shared/messages/messages.service';
import { CreateLineItemService, CreateLineItemCommand } from './create-line-item.service';

@Component({
    templateUrl: './create-line-item.component.html'
})
export class CreateLineItemComponent {
    @Input() purchaseOrderId: number;
    @Input() supplierOptions: Map<number, string>;
    objectKeys = Object.keys;
    model = new CreateLineItemViewModel();

    constructor(public activeModal: NgbActiveModal,
        private createLineItemService: CreateLineItemService,
        private messagesService: MessagesService) { }

    onSubmit() {
        let command = this.buildCommand();
        this.createLineItemService.handle(command).subscribe(
            () => this.activeModal.close("Line Item created"),
            err => this.messagesService.addHttpResponseError(err)
        );
    }

    private buildCommand(): CreateLineItemCommand {
        return new CreateLineItemCommand(this.purchaseOrderId, this.model.productId, this.model.purchasePrice,
            this.model.purchaseQty);
    }
}

class CreateLineItemViewModel {
    productId: string;
    purchasePrice: number;
    purchaseQty: number;
}
