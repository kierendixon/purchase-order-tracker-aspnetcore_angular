import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { of, throwError } from 'rxjs';

import { TestHelper } from '../../../../test/test-helper';
import { AppModule } from '../../../app.module';
import { MainSiteModule } from '../../main-site.module';
import { MessagesService } from '../../shared/messages/messages.service';
import { PurchaseOrderModule } from '../purchase-order.module';
import { CreateLineItemComponent } from './create-line-item.component';
import { CreateLineItemService } from './create-line-item.service';

describe('CreateLineItemComponent', () => {
    let component: CreateLineItemComponent;
    let fixture: ComponentFixture<CreateLineItemComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [AppModule, MainSiteModule, PurchaseOrderModule],
            providers: [NgbActiveModal]
        })
        .compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(CreateLineItemComponent);
        component = fixture.componentInstance;
    });

    it('should create', () => {
        expect(component).toBeDefined();
    });

    describe('#onSubmit', () => {
        it('closes modal when created successfully', () => {
            component.model = {
                productId: 'productId',
                purchasePrice: 1,
                purchaseQty: 1
            };
            const createService = fixture.debugElement.injector.get(CreateLineItemService);
            const handleSpy = spyOn(createService, 'handle').and.returnValue( of({}) );

            const activeModal = fixture.debugElement.injector.get(NgbActiveModal);
            const activeModalSpy = spyOn(activeModal, 'close');

            component.onSubmit();

            expect(handleSpy).toHaveBeenCalledTimes(1);
            expect(activeModalSpy).toHaveBeenCalledTimes(1);
        });

        it('sends error to messsage service if error returned', () => {
            const error = TestHelper.buildError();
            const createService = fixture.debugElement.injector.get(CreateLineItemService);
            const handleSpy = spyOn(createService, 'handle').and.returnValue( throwError(error) );

            const messagesService = fixture.debugElement.injector.get(MessagesService);
            const addHttpResponseErrorSpy = spyOn(messagesService, 'addHttpResponseError');

            component.onSubmit();

            expect(handleSpy).toHaveBeenCalledTimes(1);
            expect(addHttpResponseErrorSpy).toHaveBeenCalledWith(error);
        });
    });
});
