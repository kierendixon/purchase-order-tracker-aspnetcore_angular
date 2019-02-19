import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';

import { MainSiteModule } from '../../main-site.module';
import { AppModule } from '../../../app.module';
import { CreateComponent } from './create.component';
import { MessagesService } from '../../shared/messages/messages.service';
import { CreateService, CreateResult } from './create.service';
import { editPurchaseOrderUrl } from '../../config/routing.config';
import { PurchaseOrderTestHelper } from '../../../../test/purchase-order-test-helper';

describe('CreateComponent', () => {
    let component: CreateComponent;
    let fixture: ComponentFixture<CreateComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [AppModule, MainSiteModule]
        })
        .compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(CreateComponent);
        component = fixture.componentInstance;
    });

    it('should create', () => {
        expect(component).toBeDefined();
    });

    describe('#ngOnInit', () => {
        it('updates component\'s model with response', () => {
            const suppliersQueryResult = PurchaseOrderTestHelper.buildSuppliersResult();
            const createService = fixture.debugElement.injector.get(CreateService) as CreateService;
            const handleSuppliersQuerySpy = spyOn(createService, 'handleSuppliersQuery').
                and.returnValue( of(suppliersQueryResult) );

            component.ngOnInit();

            expect(handleSuppliersQuerySpy).toHaveBeenCalledTimes(1);
            expect(component.supplierOptions).toBe(suppliersQueryResult.suppliers);
        });

        it('sends error to messsage service if error returned', () => {
            const error = new Error('an error message');
            const createService = fixture.debugElement.injector.get(CreateService) as CreateService;
            const handleSuppliersQuerySpy = spyOn(createService, 'handleSuppliersQuery')
                .and.returnValue( throwError(error) );

            const messagesService = fixture.debugElement.injector.get(MessagesService) as MessagesService;
            const messagesSpy = spyOn(messagesService, 'addHttpResponseError');

            component.ngOnInit();

            expect(handleSuppliersQuerySpy).toHaveBeenCalledTimes(1);
            expect(messagesSpy).toHaveBeenCalledWith(error);
        });
    });

    describe('#onSubmit', () => {
        it('navigates to edit supplier URL when created successfully', () => {
            component.model.orderNo = '1';
            component.model.supplierId = 1;
            const createResult: CreateResult = {
                orderId: 1
            };
            const createService = fixture.debugElement.injector.get(CreateService) as CreateService;
            const handleCommandSpy = spyOn(createService, 'handleCommand').and.returnValue( of(createResult) );

            const router = fixture.debugElement.injector.get(Router) as Router;
            const routerSpy = spyOn(router, 'navigateByUrl');

            const navigateToUrl = editPurchaseOrderUrl(createResult.orderId);

            component.onSubmit();

            expect(handleCommandSpy).toHaveBeenCalledTimes(1);
            expect(routerSpy).toHaveBeenCalledWith(navigateToUrl);
        });

        it('sends error to messsage service if error returned', () => {
            const error = new Error('an error message');
            const createService = fixture.debugElement.injector.get(CreateService) as CreateService;
            const handleCommandSpy = spyOn(createService, 'handleCommand').and.returnValue( throwError(error) );

            const messagesService = fixture.debugElement.injector.get(MessagesService) as MessagesService;
            const messagesSpy = spyOn(messagesService, 'addHttpResponseError');

            component.onSubmit();

            expect(handleCommandSpy).toHaveBeenCalledTimes(1);
            expect(messagesSpy).toHaveBeenCalledWith(error);
        });
    });
});
