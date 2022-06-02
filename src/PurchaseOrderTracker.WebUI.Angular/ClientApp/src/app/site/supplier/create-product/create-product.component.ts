import { Component, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { MessagesService } from '../../shared/messages/messages.service';
import { CreateProductCommand, CreateProductService } from './create-product.service';

@Component({
  templateUrl: './create-product.component.html'
})
export class CreateProductComponent {
  @Input() supplierId: number;
  @Input() categoryOptions: Map<number, string>;
  objectKeys = Object.keys;
  model = new CreateProductViewModel();

  constructor(
    public activeModal: NgbActiveModal,
    private addProductService: CreateProductService,
    private messagesService: MessagesService
  ) {}

  onSubmit() {
    const command = this.buildCommand();
    this.addProductService.handle(command).subscribe(
      () => this.activeModal.close('Product created'),
      err => this.messagesService.addHttpResponseError(err)
    );
  }

  private buildCommand(): CreateProductCommand {
    return new CreateProductCommand(
      this.supplierId,
      this.model.prodCode,
      this.model.name,
      parseInt(this.model.categoryId, 10),
      this.model.price
    );
  }
}

export class CreateProductViewModel {
  prodCode: string;
  name: string;
  categoryId: string;
  price: number;
}
