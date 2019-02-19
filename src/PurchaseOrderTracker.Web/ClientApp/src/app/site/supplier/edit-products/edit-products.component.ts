import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { MessagesService } from '../../shared/messages/messages.service';
import { idParam, pageNumberQueryParam } from '../../config/routing.config';
import { PaginatedList } from '../../shared/pagination/paginated-list';
import { CreateProductComponent } from '../create-product/create-product.component';
import { EditProductsService, EditProductsQuery, EditProductsResultProduct } from './edit-products.service';
import { EditProductService, EditProductCommand } from './edit-product.service';
import { DeleteProductService, DeleteCommand } from './delete-product.service';

@Component({
    templateUrl: './edit-products.component.html'
})
export class EditProductsComponent implements OnInit {
    readonly defaultPageNumber = 1;
    pageNumber: number;
    objectKeys = Object.keys;
    supplierId: number;
    model: PaginatedList<EditProductViewModel>;
    productsAreFiltered: boolean;
    categoryOptions = new Map<number, string>();
    supplierName: string;

    constructor(
        private route: ActivatedRoute,
        private messagesService: MessagesService,
        private editProductsService: EditProductsService,
        private editProductService: EditProductService,
        private deleteProductService: DeleteProductService,
        private modalService: NgbModal) {
    }

    ngOnInit(): void {
        this.supplierId = this.route.snapshot.params[idParam];
        this.route.queryParams.subscribe(params => {
            this.pageNumber = params[pageNumberQueryParam] === undefined
                ? this.defaultPageNumber
                : params[pageNumberQueryParam];
            this.refreshData();
        });
    }

    refreshData(resetPageNumber?: boolean, productFilter?: string): void {
        if (resetPageNumber) {
            this.pageNumber = this.defaultPageNumber;
        }
        const query = new EditProductsQuery(this.supplierId, this.pageNumber, productFilter);

        this.editProductsService.handle(query).subscribe(
            result => {
                this.model = this.convertQueryResultToViewModel(result.products);
                this.productsAreFiltered = result.productsAreFiltered;
                this.categoryOptions = result.categoryOptions;
                this.supplierName = result.supplierName;
            },
            err => this.messagesService.addHttpResponseError(err)
        );
    }

    private convertQueryResultToViewModel(data: PaginatedList<EditProductsResultProduct>): PaginatedList<EditProductViewModel> {
        // properties are exactly the same
        return <PaginatedList<EditProductViewModel>>data;
    }

    showAddProductModal() {
        const modalRef = this.modalService.open(CreateProductComponent);
        modalRef.componentInstance.categoryOptions = this.categoryOptions;
        modalRef.componentInstance.supplierId = this.supplierId;
        modalRef.result.then(
            result => {
                if (result) {
                    this.messagesService.addMessage(result);
                    this.refreshData(true);
                }
            }
        );
    }

    onDeleteProduct(productIndex: number): void {
        const product = this.model.items[productIndex];
        const command = new DeleteCommand(this.supplierId, product.productId);
        this.deleteProductService.handle(command).subscribe(
            result => {
                this.messagesService.addMessage(`Product deleted: (${product.productId}) ${product.name}`);
                this.refreshData(true);
            },
            err => this.messagesService.addHttpResponseError(err)
        );
    }

    onSubmitEditProduct(productIndex: number): void {
        const product = this.model.items[productIndex];
        const command = this.buildEditProductCommand(product);
        this.editProductService.handle(command).subscribe(
            result => this.messagesService.addMessage(`Product updated: (${product.productId}) ${product.name}`),
            err => this.messagesService.addHttpResponseError(err)
        );
    }

    private buildEditProductCommand(product: EditProductViewModel): EditProductCommand {
        return new EditProductCommand(this.supplierId,
            product.productId,
            product.prodCode,
            product.name,
            product.categoryId,
            product.price);
    }

    onSearch(prodCode: string): void {
        this.refreshData(true, prodCode);
    }

    onClearSearch(): void {
        this.refreshData();
    }

    hasProducts(): boolean {
        return this.model ? this.model.items.length > 0 : false;
    }
}

export class EditProductViewModel {
    productId: number;
    prodCode: string;
    name: string;
    categoryId: number;
    price: number;
}
