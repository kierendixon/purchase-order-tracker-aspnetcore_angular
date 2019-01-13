import { Injectable } from '@angular/core';
import { Router, NavigationStart } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { filter } from 'rxjs/operators';

import { Message } from './message';

@Injectable()
export class MessagesService {
    private messages: Message[] = [];

    constructor(router: Router) {
        router.events
            .pipe(filter(event => event instanceof NavigationStart))
            .subscribe(
                (event: NavigationStart) => {
                    this.clearMessages();
                }
            );
    }

    public addMessage(msg: string, isError = false) {
        this.messages.push(new Message(msg, isError));
    }

    public addErrorMessage(msg: string) {
        this.addMessage(msg, true);
    }

    public addHttpResponseError(err: any): void {
        if (err) {
            if (err.error instanceof Error) {
                // A client-side or network error occurred
                this.addErrorMessage(err.error.message);
            } else {
                // The backend returned an unsuccessful response code
                const httpErr = <HttpErrorResponse>err;
                this.addErrorMessage(`Error returned from server. Code: ${httpErr.status}, body: ${JSON.stringify(httpErr.error)}`);
            }
        }
    }

    public getMessages(): Message[] {
        return this.messages;
    }

    public hasMessages(): boolean {
        return this.messages.length > 0;
    }

    public removeMessage(index: number): void {
        this.messages.splice(index, 1);
    }

    public clearMessages(): void {
        this.messages = [];
    }
}
