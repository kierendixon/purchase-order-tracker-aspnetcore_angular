import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';

import { MainSiteModule } from './site/main-site.module';
import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { AccountComponent } from './account/account.component';

@NgModule({
    imports: [BrowserModule, FormsModule, HttpModule, MainSiteModule, AppRoutingModule],
    declarations: [AppComponent, AccountComponent],
    bootstrap: [AppComponent]
})
export class AppModule { }