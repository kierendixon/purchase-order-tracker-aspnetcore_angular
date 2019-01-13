import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { supplierProductUrl } from '../../config/api.config';

@Injectable()
export class EditProductService {
    constructor(private http: HttpClient) {
    }

    public handle(command: EditProductCommand): Observable<EditProductResult> {
        const url = supplierProductUrl(command.supplierId, command.productId);
        return this.http.post<EditProductResult>(url, command);
    }
}

export class EditProductCommand {
    constructor(readonly supplierId: number,
        readonly productId: number,
        readonly prodCode: string,
        readonly name: string,
        readonly categoryId: number,
        readonly price: number) {
    }
}

// tslint:disable-next-line:no-empty-interface
export interface EditProductResult {
}
