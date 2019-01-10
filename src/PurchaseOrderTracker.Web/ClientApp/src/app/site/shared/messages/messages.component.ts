import { Component } from '@angular/core';

import { MessagesService } from './messages.service';

@Component({
    // TODO: remove tslint ignore
    // tslint:disable-next-line:component-selector
    selector: 'messages',
    templateUrl: './messages.component.html'
})
export class MessagesComponent {
    constructor(public messageService: MessagesService) { }
}
