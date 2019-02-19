import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { of, throwError } from 'rxjs';

import { MainSiteModule } from '../../main-site.module';
import { AppModule } from '../../../app.module';
import { SupplierModule } from '../supplier.module';
import { CreateProductComponent } from './create-product.component';
import { MessagesService } from '../../shared/messages/messages.service';
import { CreateProductService } from './create-product.service';

describe('CreateProductComponent', () => {
    let component: CreateProductComponent;
    let fixture: ComponentFixture<CreateProductComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [AppModule, MainSiteModule, SupplierModule],
            providers: [NgbActiveModal]
        })
        .compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(CreateProductComponent);
        component = fixture.componentInstance;
    });

    it('should create', () => {
        expect(component).toBeDefined();
    });

    describe('#onSubmit', () => {
        it('closes modal when created successfully', () => {
            component.model = {
                prodCode: 'prodcode',
                name: 'name',
                categoryId: 1,
                price: 100
            };
            const createService = fixture.debugElement.injector.get(CreateProductService) as CreateProductService;
            const createServiceSpy = spyOn(createService, 'handle').and.returnValue( of({}) );

            const activeModal = fixture.debugElement.injector.get(NgbActiveModal) as NgbActiveModal;
            const activeModalSpy = spyOn(activeModal, 'close');

            component.onSubmit();

            expect(createServiceSpy).toHaveBeenCalledTimes(1);
            expect(activeModalSpy).toHaveBeenCalledTimes(1);
        });

        it('sends error to messsage service if error returned', () => {
            const error = new Error('an error message');
            const createService = fixture.debugElement.injector.get(CreateProductService) as CreateProductService;
            const createServiceSpy = spyOn(createService, 'handle').and.returnValue( throwError(error) );

            const messagesService = fixture.debugElement.injector.get(MessagesService) as MessagesService;
            const messagesSpy = spyOn(messagesService, 'addHttpResponseError');

            component.onSubmit();

            expect(createServiceSpy).toHaveBeenCalledTimes(1);
            expect(messagesSpy).toHaveBeenCalledWith(error);
        });
    });
});
