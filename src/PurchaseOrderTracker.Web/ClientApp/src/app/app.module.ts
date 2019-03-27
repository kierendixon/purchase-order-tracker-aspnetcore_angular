import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { AppRoutingModule } from './app-routing.module';
import { MainSiteModule } from './site/main-site.module';

import { AccountComponent } from './account/account.component';
import { AppComponent } from './app.component';
import { AuthGuard } from './infrastructure/security/auth-guard';
import { AuthService } from './infrastructure/security/auth.service';
import { RouteNotFoundComponent } from './route-not-found/route-not-found.component';

@NgModule({
  declarations: [AppComponent, AccountComponent, RouteNotFoundComponent],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    NgbModule,
    MainSiteModule,
    AppRoutingModule
  ],
  providers: [AuthService, AuthGuard],
  bootstrap: [AppComponent]
})
export class AppModule {}
