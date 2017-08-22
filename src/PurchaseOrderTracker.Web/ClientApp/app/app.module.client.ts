import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { sharedConfig } from './app.module.shared';

@NgModule({
    imports: [
        BrowserModule,
        FormsModule,
        HttpModule,
        ...sharedConfig.imports
    ],
    declarations: sharedConfig.declarations,
    providers: [
        sharedConfig.providers,
        { provide: 'ORIGIN_URL', useValue: location.origin }
    ],
    bootstrap: sharedConfig.bootstrap
})
export class AppModule {
}