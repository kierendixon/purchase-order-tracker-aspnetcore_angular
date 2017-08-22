import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { purchaseOrderLineItemUrl } from '../../config/api.config';

@Injectable()
export class DeleteLineItemService {
    constructor(private http: HttpClient) {
    }

    public handle(command: DeleteCommand): Observable<DeleteResult> {
        let url = purchaseOrderLineItemUrl(command.purchaseOrderId, command.lineItemId);
        return this.http.delete<DeleteResult>(url);
    }
}

export class DeleteCommand {
    constructor(readonly purchaseOrderId: number,
        readonly lineItemId: number) {
    }
}

export interface DeleteResult {
}