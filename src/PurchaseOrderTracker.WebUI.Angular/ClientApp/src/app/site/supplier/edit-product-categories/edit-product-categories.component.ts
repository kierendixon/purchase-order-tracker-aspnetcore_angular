import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { idParam, pageNumberQueryParam } from '../../config/routing.config';
import { MessagesService } from '../../shared/messages/messages.service';
import { PaginatedList } from '../../shared/pagination/paginated-list';
import { CreateProductCategoryComponent } from '../create-product-category/create-product-category.component';
import { DeleteCommand, DeleteProductCategoryService } from './delete-product-category.service';
import {
  EditProductCategoriesQuery,
  EditProductCategoriesService,
  ProductCategoryResult
} from './edit-product-categories.service';
import { EditProductCategoryCommand, EditProductCategoryService } from './edit-product-category.service';

@Component({
  templateUrl: './edit-product-categories.component.html'
})
export class EditProductCategoriesComponent implements OnInit {
  readonly defaultPageNumber = 1;
  pageNumber: number;
  supplierId: number;
  supplierName: string;
  model: PaginatedList<ProductCategory>;

  constructor(
    private route: ActivatedRoute,
    private editProductCategoriesService: EditProductCategoriesService,
    private deleteProductCategoryService: DeleteProductCategoryService,
    private editProductCategoryService: EditProductCategoryService,
    private messagesService: MessagesService,
    private modalService: NgbModal
  ) {}

  ngOnInit(): void {
    this.supplierId = this.route.snapshot.params[idParam];
    this.route.queryParams.subscribe(params => {
      this.pageNumber =
        params[pageNumberQueryParam] === undefined ? this.defaultPageNumber : params[pageNumberQueryParam];
      this.refreshData();
    });
  }

  refreshData(): void {
    const query = new EditProductCategoriesQuery(this.supplierId, this.pageNumber);
    this.editProductCategoriesService.handle(query).subscribe(
      result => {
        this.supplierName = result.supplierName;
        this.model = this.convertQueryResultToViewModel(result.categories);
      },
      err => this.messagesService.addHttpResponseError(err)
    );
  }

  private convertQueryResultToViewModel(data: PaginatedList<ProductCategoryResult>): PaginatedList<ProductCategory> {
    // properties are exactly the same
    return data as PaginatedList<ProductCategory>;
  }

  showAddProductCategoryModal() {
    const modalRef = this.modalService.open(CreateProductCategoryComponent);
    modalRef.componentInstance.supplierId = this.supplierId;
    modalRef.result.then(result => {
      if (result) {
        this.messagesService.addMessage(result);
        this.refreshData();
      }
    });
  }

  onDeleteCategory(index: number): void {
    const category = this.model.items[index];
    const command = new DeleteCommand(this.supplierId, category.id);

    this.deleteProductCategoryService.handle(command).subscribe(
      result => {
        this.messagesService.addMessage(`Category deleted: (${category.id}) ${category.name}`);
        this.refreshData();
      },
      err => this.messagesService.addHttpResponseError(err)
    );
  }

  onSubmitEditCategory(index: number): void {
    const category = this.model.items[index];
    const command = new EditProductCategoryCommand(this.supplierId, category.id, category.name);

    this.editProductCategoryService
      .handle(command)
      .subscribe(
        result => this.messagesService.addMessage(`Category updated: (${category.id}) ${category.name}`),
        err => this.messagesService.addHttpResponseError(err)
      );
  }

  hasCategories(): boolean {
    return this.model ? this.model.items.length > 0 : false;
  }
}

export class ProductCategory {
  id: number;
  name: string;
}
