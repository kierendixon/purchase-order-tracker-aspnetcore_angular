import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { accountUrl } from '../../../config/routing.config';
import { AuthService } from '../../../infrastructure/security/auth.service';
import { MessagesService } from '../messages/messages.service';

@Component({
  selector: 'nav-top',
  templateUrl: './nav-top.component.html'
})
export class NavTopComponent {
  constructor(private authService: AuthService, private router: Router, private messageService: MessagesService) {}

  public onLogout() {
    const that = this;

    this.authService
      .handleLogoutCommand()
      .subscribe(() => that.router.navigate([accountUrl]), err => this.messageService.addHttpResponseError(err));
  }
}
