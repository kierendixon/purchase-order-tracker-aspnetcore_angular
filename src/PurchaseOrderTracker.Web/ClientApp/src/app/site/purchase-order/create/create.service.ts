import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { basePurchaseOrderUrl } from '../../config/api.config';
import { baseSupplierUrl } from '../../config/api.config';

@Injectable()
export class CreateService {
    constructor(private http: HttpClient) {
    }

    public handleCommand(command: CreateCommand): Observable<CreateResult> {
        return this.http.put<CreateResult>(basePurchaseOrderUrl, command);
    }

    public handleSuppliersQuery(query: SuppliersQuery): Observable<SuppliersResult> {
        return this.http.get<SuppliersResult>(baseSupplierUrl);
    }
}

export class CreateCommand {
    constructor(readonly orderNo: string,
        readonly supplierId: number) {
    }
}

export class SuppliersQuery {
}

export interface CreateResult {
    orderId: number;
}

export interface SuppliersResult {
    suppliers: SuppliersResultSupplier[];
}

interface SuppliersResultSupplier {
    id: number;
    name: string;
}
