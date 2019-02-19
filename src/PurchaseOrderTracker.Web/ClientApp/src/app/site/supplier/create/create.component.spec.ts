import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';

import { MainSiteModule } from '../../main-site.module';
import { AppModule } from '../../../app.module';
import { CreateComponent } from './create.component';
import { MessagesService } from '../../shared/messages/messages.service';
import { CreateService, CreateResult } from './create.service';
import { editSupplierUrl } from '../../config/routing.config';

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

    describe('#onSubmit', () => {
        it('navigates to edit supplier URL when created successfully', () => {
            const supplierId = 1;
            const createResult: CreateResult = {
                supplierId: supplierId
            };
            const createService = fixture.debugElement.injector.get(CreateService) as CreateService;
            const createServiceSpy = spyOn(createService, 'handle').and.returnValue( of(createResult) );

            const router = fixture.debugElement.injector.get(Router) as Router;
            const routerSpy = spyOn(router, 'navigateByUrl');

            const navigateToUrl = editSupplierUrl(supplierId);

            component.onSubmit();

            expect(createServiceSpy).toHaveBeenCalledTimes(1);
            expect(routerSpy).toHaveBeenCalledWith(navigateToUrl);
        });

        it('sends error to messsage service if error returned', () => {
            const error = new Error('an error message');
            const createService = fixture.debugElement.injector.get(CreateService) as CreateService;
            const createServiceSpy = spyOn(createService, 'handle').and.returnValue( throwError(error) );

            const messagesService = fixture.debugElement.injector.get(MessagesService) as MessagesService;
            const messagesSpy = spyOn(messagesService, 'addHttpResponseError');

            component.onSubmit();

            expect(createServiceSpy).toHaveBeenCalledTimes(1);
            expect(messagesSpy).toHaveBeenCalledWith(error);
        });
    });
});
