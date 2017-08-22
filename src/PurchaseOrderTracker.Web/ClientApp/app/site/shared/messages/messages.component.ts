import { Component } from '@angular/core';

import { MessagesService } from './messages.service';

@Component({
    selector: 'messages',
    templateUrl: './messages.component.html'
})
export class MessagesComponent {
    constructor(private messageService: MessagesService) { }
}
