import { Component, OnInit, Inject } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { AuthService } from '../infrastructure/security/auth.service';
import { authServiceToken } from '../config/app.config';
import { mainSiteUrl, returnUrlQueryParam } from '../config/routing.config';

@Component({
    templateUrl: './account.component.html',
    styleUrls: ['./account.component.css']
})

export class AccountComponent implements OnInit {
    errorMessage: string;
    model = new AccountViewModel();

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        @Inject(authServiceToken) private authService: AuthService) { }

    ngOnInit() {
        this.skipLoginIfAlreadyAuthenticated();
    }

    skipLoginIfAlreadyAuthenticated() {
        this.authService.isUserAuthenticated().subscribe(
            isAuthenticated => {
                if (isAuthenticated)
                    this.navigateToNextUrl();
            },
            err => this.errorMessage = err
        );
    }

    navigateToNextUrl(): void {
        let returnUrl = this.route.snapshot.queryParams[returnUrlQueryParam];
        this.router.navigateByUrl(returnUrl ? returnUrl : mainSiteUrl);
    }

    onSubmit() {
        this.authService.authenticate(this.model.username, this.model.password).subscribe(
            result => this.navigateToNextUrl(),
            err => this.errorMessage = err
        );
    }
}

class AccountViewModel {
    username: string;
    password: string;
}