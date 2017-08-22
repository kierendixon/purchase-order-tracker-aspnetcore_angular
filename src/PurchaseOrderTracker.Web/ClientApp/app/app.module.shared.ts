import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { AppRoutingModule } from './app-routing.module';
import { MainSiteModule } from './site/main-site.module';

import { AppComponent } from './app.component'
import { AccountComponent } from './account/account.component';
import { RouteNotFoundComponent } from './route-not-found/route-not-found.component';

import * as CONFIG from './config/app.config';
import { AuthGuard } from './infrastructure/security/auth-guard.service';
import { StubbedAuthService } from './infrastructure/security/stubbed-auth.service';

export const sharedConfig: NgModule = {
    imports: [BrowserModule, FormsModule, HttpModule, NgbModule.forRoot(),
        MainSiteModule, AppRoutingModule],
    declarations: [AppComponent, AccountComponent, RouteNotFoundComponent],
    bootstrap: [AppComponent],
    providers: [{ provide: CONFIG.authServiceToken, useClass: StubbedAuthService },
        AuthGuard]
};