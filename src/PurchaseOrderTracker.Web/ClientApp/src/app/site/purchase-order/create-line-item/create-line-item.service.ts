import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { purchaseOrderLineItemsUrl } from '../../config/api.config';

@Injectable()
export class CreateLineItemService {
    constructor(private http: HttpClient) {
    }

    public handle(command: CreateLineItemCommand): Observable<CreateLineItemCommandResult> {
        return this.http.put<CreateLineItemCommandResult>(purchaseOrderLineItemsUrl(command.purchaseOrderId), command);
    }
}

export class CreateLineItemCommand {
    constructor(readonly purchaseOrderId,
        readonly productId: string,
        readonly purchasePrice: number,
        readonly purchaseQty: number) {
    }
}

// tslint:disable-next-line:no-empty-interface
export interface CreateLineItemCommandResult {
}
