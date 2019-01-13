import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { purchaseOrderLineItemsUrl } from '../../config/api.config';

@Injectable()
export class EditLineItemsService {
    constructor(private http: HttpClient) {
    }

    public handle(query: EditLineItemsQuery): Observable<EditLineItemsResult> {
        const url = purchaseOrderLineItemsUrl(query.id);
        return this.http.get<EditLineItemsResult>(url);
    }
}

export class EditLineItemsQuery {
    constructor(readonly id: number) {
    }
}

export interface EditLineItemsResult {
    purchaseOrderId: number;
    purchaseOrderOrderNo: string;
    lineItems: EditLineItemsResultItem[];
    productOptions: SelectOption[];
}

export interface EditLineItemsResultItem {
    id: number;
    productId: number;
    purchasePrice: number;
    purchaseQty: number;
}

interface SelectOption {
    id: number;
    value: string;
}
