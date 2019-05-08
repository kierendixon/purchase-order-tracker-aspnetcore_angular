import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { mainSiteUrl, returnUrlQueryParam } from '../config/routing.config';
import { AuthService, LoginCommand } from '../infrastructure/security/auth.service';
import { MessagesHelper } from '../site/shared/messages/messages.helper';

@Component({
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss']
})
export class AccountComponent implements OnInit {
  private static invalidCredentialsUserMsg = 'Username or password is invalid';

  errorMessage: string;
  model = new AccountViewModel();

  constructor(private route: ActivatedRoute, private router: Router, private authService: AuthService) {}

  ngOnInit() {
    this.skipLoginIfAlreadyAuthenticated();
  }

  // TODO: this causes the account page to display momentarily
  skipLoginIfAlreadyAuthenticated() {
    if (this.authService.currentUser != null) {
      this.navigateToNextUrl();
    }
  }

  navigateToNextUrl(): void {
    const returnUrl = this.route.snapshot.queryParams[returnUrlQueryParam];
    this.router.navigateByUrl(returnUrl ? returnUrl : mainSiteUrl);
  }

  onSubmit() {
    const command = new LoginCommand(this.model.username, this.model.password);
    this.authService.handleLoginCommand(command).subscribe(
      result => this.navigateToNextUrl(),
      err => {
        if (err.status == 401) {
          this.errorMessage = AccountComponent.invalidCredentialsUserMsg;
        } else {
          this.errorMessage = MessagesHelper.ConvertErrorToFriendlyMessage(err);
        }
      }
    );
  }
}

class AccountViewModel {
  username: string;
  password: string;
}
