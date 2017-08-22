import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { purchaseOrderLineItemUrl } from '../../config/api.config';

@Injectable()
export class EditLineItemService {
    constructor(private http: HttpClient) {
    }

    public handle(command: EditLineItemCommand): Observable<EditLineItemResult> {
        let url = purchaseOrderLineItemUrl(command.purchaseOrderId, command.lineItemId);
        return this.http.post<EditLineItemResult>(url, command);
    }
}

export class EditLineItemCommand {
    constructor(readonly purchaseOrderId: number,
        readonly lineItemId: number,
        readonly productId: number,
        readonly purchasePrice: number,
        readonly purchaseQty: number) {
    }
}
export interface EditLineItemResult {
}
