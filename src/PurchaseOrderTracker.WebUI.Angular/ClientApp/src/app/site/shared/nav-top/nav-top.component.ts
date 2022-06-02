import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { accountUrl } from '../../../config/routing.config';
import { AuthService } from '../../../infrastructure/security/auth.service';
import { MessagesService } from '../messages/messages.service';
import { User, UserService } from './user.service';

@Component({
  selector: 'nav-top',
  templateUrl: './nav-top.component.html'
})
export class NavTopComponent implements OnInit {
  public user: User;

  constructor(
    private authService: AuthService,
    private userService: UserService,
    private router: Router,
    private messageService: MessagesService
  ) {}

  ngOnInit() {
    this.userService.handle().subscribe(
      val => (this.user = val),
      err => this.messageService.addHttpResponseError(err)
    );
  }

  public onLogout() {
    const that = this;

    this.authService.handleLogoutCommand().subscribe(
      () => that.router.navigate([accountUrl]),
      err => this.messageService.addHttpResponseError(err)
    );
  }
}
