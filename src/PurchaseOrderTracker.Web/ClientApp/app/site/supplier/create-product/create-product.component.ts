import { Component, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { MessagesService } from '../../shared/messages/messages.service';
import { CreateProductService, CreateProductCommand } from './create-product.service';

@Component({
    templateUrl: './create-product.component.html'
})
export class CreateProductComponent {
    @Input() supplierId: number;
    @Input() categoryOptions: Map<number, string>;
    objectKeys = Object.keys;
    model = new CreateProductViewModel();

    constructor(public activeModal: NgbActiveModal,
        private addProductService: CreateProductService,
        private messagesService: MessagesService) { }

    onSubmit() {
        let command = this.buildCommand();
        this.addProductService.handle(command).subscribe(
            () => this.activeModal.close("Product created"),
            err => this.messagesService.addHttpResponseError(err)
        );
    }

    private buildCommand(): CreateProductCommand {
        return new CreateProductCommand(this.supplierId, this.model.prodCode, this.model.name,
            this.model.categoryId, this.model.price);
    }
}

class CreateProductViewModel {
    prodCode: string;
    name: string;
    categoryId: number;
    price: number;
}