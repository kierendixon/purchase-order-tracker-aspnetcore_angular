/// <reference path="../../node_modules/@types/jasmine/index.d.ts" />
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { APP_BASE_HREF } from '@angular/common';

import { AppComponent } from './app.component';
import { AppModule } from './app.module';

describe('AppComponent', function () {
    let comp: AppComponent;
    let fixture: ComponentFixture<AppComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [AppModule],
            providers: [
                { provide: APP_BASE_HREF, useValue: '/' }
            ]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(AppComponent);
        comp = fixture.componentInstance;
    });

    it('renders', () => {
        expect(comp).toBeDefined();
    });
});
