import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { supplierUrl } from '../../config/api.config';

@Injectable()
export class EditService {
    constructor(private http: HttpClient) {
    }

    public handleQuery(query: EditQuery): Observable<EditQueryResult> {
        return this.http.get<EditQueryResult>(supplierUrl(query.id));
    }

    public handleCommand(command: EditCommand): Observable<EditCommandResult> {
        return this.http.post<EditCommandResult>(supplierUrl(command.id), command);
    }
}

export class EditQuery {
    constructor(readonly id: number) {
    }
}

export interface EditQueryResult {
    suppliers: EditQueryResultSupplier[];
}

export interface EditQueryResultSupplier {
    id: number;
    name: string;
}

export class EditCommand {
    constructor(readonly id: number, readonly name: string) {
    }
}

export interface EditCommandResult {
}