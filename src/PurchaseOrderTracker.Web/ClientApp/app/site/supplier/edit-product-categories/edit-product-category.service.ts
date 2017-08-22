import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { supplierProductCategoryUrl } from '../../config/api.config';

@Injectable()
export class EditProductCategoryService {
    constructor(private http: HttpClient) {
    }

    public handle(command: EditProductCategoryCommand): Observable<EditProductCategoryResult> {
        let url = supplierProductCategoryUrl(command.supplierId, command.categoryId);
        return this.http.post<EditProductCategoryResult>(url, command);
    }
}

export class EditProductCategoryCommand {
    constructor(readonly supplierId: number,
        readonly categoryId: number,
        readonly name: string) {
    }
}

export interface EditProductCategoryResult {
}