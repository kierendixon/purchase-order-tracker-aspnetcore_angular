import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { purchaseOrderUrl } from '../../config/api.config';

@Injectable()
export class DeleteService {
    constructor(private http: HttpClient) {
    }

    public handle(command: DeleteCommand): Observable<DeleteResult> {
        return this.http.delete<DeleteResult>(purchaseOrderUrl(command.id));
    }
}

export class DeleteCommand {
    constructor(readonly id: number) {
    }
}

export interface DeleteResult {
}