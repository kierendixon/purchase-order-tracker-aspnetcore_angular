import { Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { authServiceToken } from '../config/app.config';
import { mainSiteUrl, returnUrlQueryParam } from '../config/routing.config';
import { AuthService } from '../infrastructure/security/auth.service';

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
    @Inject(authServiceToken) private authService: AuthService
  ) {}

  ngOnInit() {
    this.skipLoginIfAlreadyAuthenticated();
  }

  skipLoginIfAlreadyAuthenticated() {
    this.authService.isUserAuthenticated().subscribe(
      isAuthenticated => {
        if (isAuthenticated) {
          this.navigateToNextUrl();
        }
      },
      err => (this.errorMessage = err)
    );
  }

  navigateToNextUrl(): void {
    const returnUrl = this.route.snapshot.queryParams[returnUrlQueryParam];
    this.router.navigateByUrl(returnUrl ? returnUrl : mainSiteUrl);
  }

  onSubmit() {
    this.authService
      .authenticate(this.model.username, this.model.password)
      .subscribe(result => this.navigateToNextUrl(), err => (this.errorMessage = err));
  }
}

class AccountViewModel {
  username: string;
  password: string;
}
