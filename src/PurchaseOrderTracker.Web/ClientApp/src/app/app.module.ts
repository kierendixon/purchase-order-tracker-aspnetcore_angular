import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { AppRoutingModule } from './app-routing.module';
import { MainSiteModule } from './site/main-site.module';

import { AccountComponent } from './account/account.component';
import { AppComponent } from './app.component';
import { AuthGuard } from './infrastructure/security/auth.guard';
import { AuthService } from './infrastructure/security/auth.service';
import { JwtHttpInterceptor } from './infrastructure/security/jwt.http-interceptor';
import { LogoutHttpInterceptor } from './infrastructure/security/logout.http-interceptor';
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
  // TODO: move these to shared module?
  providers: [
    AuthService,
    AuthGuard,
    { provide: HTTP_INTERCEPTORS, useClass: JwtHttpInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: JwtHttpInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: LogoutHttpInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
