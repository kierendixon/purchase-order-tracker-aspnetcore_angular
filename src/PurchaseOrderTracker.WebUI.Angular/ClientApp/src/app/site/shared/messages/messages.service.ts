import { Injectable } from '@angular/core';
import { NavigationStart, Router } from '@angular/router';
import { filter } from 'rxjs/operators';

import { Message } from './message';
import { MessagesHelper } from './messages.helper';

@Injectable()
export class MessagesService {
  private messages: Message[] = [];

  constructor(router: Router) {
    router.events.pipe(filter(event => event instanceof NavigationStart)).subscribe((event: NavigationStart) => {
      this.clearMessages();
    });
  }

  public addMessage(msg: string, isError = false) {
    this.messages.push(new Message(msg, isError));
  }

  public addErrorMessage(msg: string) {
    this.addMessage(msg, true);
  }

  public addHttpResponseError(err: any): void {
    this.addErrorMessage(MessagesHelper.ConvertErrorToFriendlyMessage(err));
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
