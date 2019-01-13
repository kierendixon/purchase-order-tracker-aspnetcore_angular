import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { shipmentStatusUrl } from '../../config/api.config';

@Injectable()
export class EditStatusService {
    constructor(private http: HttpClient) {
    }

    public handle(command: EditStatusCommand): Observable<EditStatusResult> {
        return this.http.post<EditStatusResult>(shipmentStatusUrl(command.id), command);
    }
}

export class EditStatusCommand {
    constructor(readonly id: number,
        readonly updatedStatus: string) {
    }
}

// tslint:disable-next-line:no-empty-interface
export interface EditStatusResult {
}
