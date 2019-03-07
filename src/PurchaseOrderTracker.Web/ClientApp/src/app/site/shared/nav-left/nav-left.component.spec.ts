import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';

import { AppModule } from '../../../app.module';
import { purchaseOrdersUrl, shipmentsUrl, suppliersUrl } from '../../config/routing.config';
import { MainSiteModule } from '../../main-site.module';
import { NavLeftComponent } from './nav-left.component';

describe('NavLeftComponent', () => {
    let component: NavLeftComponent;
    let fixture: ComponentFixture<NavLeftComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [AppModule, MainSiteModule]
        })
        .compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(NavLeftComponent);
        component = fixture.componentInstance;
    });

    it('should create', () => {
        expect(component).toBeDefined();
    });

    describe('#isDisplayPurchaseOrdersNav', () => {
        it('returns true if the current url is the purchase orders feature url', () => {
            const router = fixture.debugElement.injector.get(Router);
            spyOnProperty(router, 'url', 'get').and.returnValue(purchaseOrdersUrl);
            expect(component.isDisplayPurchaseOrdersNav()).toBe(true);
        });

        it('returns false if the current url is not the purchase orders feature url', () => {
            const router = fixture.debugElement.injector.get(Router);
            spyOnProperty(router, 'url', 'get').and.returnValue('any-url');
            expect(component.isDisplayPurchaseOrdersNav()).toBe(false);
        });
    });

    describe('#isDisplayShipmentsNav', () => {
        it('returns true if the current url is the shipments feature url', () => {
            const router = fixture.debugElement.injector.get(Router);
            spyOnProperty(router, 'url', 'get').and.returnValue(shipmentsUrl);
            expect(component.isDisplayShipmentsNav()).toBe(true);
        });

        it('returns false if the current url is not the shipments feature url', () => {
            const router = fixture.debugElement.injector.get(Router);
            spyOnProperty(router, 'url', 'get').and.returnValue('any-url');
            expect(component.isDisplayShipmentsNav()).toBe(false);
        });
    });

    describe('#isDisplaySuppliersNav', () => {
        it('returns true if the current url is the suppliers feature url', () => {
            const router = fixture.debugElement.injector.get(Router);
            spyOnProperty(router, 'url', 'get').and.returnValue(suppliersUrl);
            expect(component.isDisplaySuppliersNav()).toBe(true);
        });

        it('returns false if the current url is not the suppliers feature url', () => {
            const router = fixture.debugElement.injector.get(Router);
            spyOnProperty(router, 'url', 'get').and.returnValue('any-url');
            expect(component.isDisplayPurchaseOrdersNav()).toBe(false);
        });
    });
});
