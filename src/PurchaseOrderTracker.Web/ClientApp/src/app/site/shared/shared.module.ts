import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { MessagesComponent } from './messages/messages.component';
import { NavLeftComponent } from './nav-left/nav-left.component';
import { NavTopComponent } from './nav-top/nav-top.component';
import { PaginationComponent } from './pagination/pagination.component';

import { MessagesService } from './messages/messages.service';

@NgModule({
  imports: [CommonModule, RouterModule],
  declarations: [MessagesComponent, NavLeftComponent, NavTopComponent, PaginationComponent],
  exports: [MessagesComponent, NavLeftComponent, NavTopComponent, PaginationComponent],
  providers: [MessagesService]
})
export class SharedModule {}
