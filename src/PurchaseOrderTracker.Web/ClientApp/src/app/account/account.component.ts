import { Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { mainSiteUrl, returnUrlQueryParam } from '../config/routing.config';
import { AuthService, LoginCommand } from '../infrastructure/security/auth.service';

@Component({
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss']
})
export class AccountComponent implements OnInit {
  errorMessage: string;
  model = new AccountViewModel();

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit() {
    this.skipLoginIfAlreadyAuthenticated();
  }

  skipLoginIfAlreadyAuthenticated() {
    const that = this;

    this.authService.isUserAuthenticated().subscribe(
      result => {
        if (result) {
          that.navigateToNextUrl();
        }
      },
      err => that.errorMessage = err
    );
  }

  navigateToNextUrl(): void {
    const returnUrl = this.route.snapshot.queryParams[returnUrlQueryParam];
    this.router.navigateByUrl(returnUrl ? returnUrl : mainSiteUrl);
  }

  onSubmit() {
    const command = new LoginCommand(this.model.username, this.model.password);
    this.authService.handleLoginCommand(command)
      .subscribe(result => this.navigateToNextUrl(), err => (this.errorMessage = err));
  }
}

class AccountViewModel {
  username: string;
  password: string;
}
