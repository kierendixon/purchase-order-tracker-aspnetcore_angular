import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';

import { MainSiteModule } from '../../main-site.module';
import { AppModule } from '../../../app.module';
import { CreateComponent } from './create.component';
import { MessagesService } from '../../shared/messages/messages.service';
import { CreateService, CreateResult } from './create.service';
import { editShipmentUrl } from '../../config/routing.config';

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
        it('navigates to edit shipment URL when created successfully', () => {
            const shipmentId = 1;
            const createResult: CreateResult = {
                id: shipmentId
            };
            const createService = fixture.debugElement.injector.get(CreateService) as CreateService;
            const handleSpy = spyOn(createService, 'handle').and.returnValue( of(createResult) );

            const router = fixture.debugElement.injector.get(Router) as Router;
            const navigateByUrlSpy = spyOn(router, 'navigateByUrl');

            const navigateToUrl = editShipmentUrl(shipmentId);

            component.onSubmit();

            expect(handleSpy).toHaveBeenCalledTimes(1);
            expect(navigateByUrlSpy).toHaveBeenCalledWith(navigateToUrl);
        });

        it('sends error to messsage service if error returned', () => {
            const error = new Error('an error message');
            const createService = fixture.debugElement.injector.get(CreateService) as CreateService;
            const handleSpy = spyOn(createService, 'handle').and.returnValue( throwError(error) );

            const messagesService = fixture.debugElement.injector.get(MessagesService) as MessagesService;
            const addHttpResponseErrorSpy = spyOn(messagesService, 'addHttpResponseError');

            component.onSubmit();

            expect(handleSpy).toHaveBeenCalledTimes(1);
            expect(addHttpResponseErrorSpy).toHaveBeenCalledWith(error);
        });
    });
});
