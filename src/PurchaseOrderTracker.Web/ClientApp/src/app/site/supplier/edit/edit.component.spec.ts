import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { of, throwError } from 'rxjs';

import { MainSiteModule } from '../../main-site.module';
import { AppModule } from '../../../app.module';
import { EditComponent } from './edit.component';
import { MessagesService } from '../../shared/messages/messages.service';
import { EditService, EditQueryResult } from './edit.service';
import { DeleteService } from './delete.service';
import { idParam, suppliersUrl } from '../../config/routing.config';
import { TestHelper } from '../../../../test/test-helper';
import { SupplierTestHelper } from 'src/test/supplier-test-helper';

describe('EditComponent', () => {
    let component: EditComponent;
    let fixture: ComponentFixture<EditComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [AppModule, MainSiteModule]
        })
        .compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(EditComponent);
        component = fixture.componentInstance;
    });

    it('should create', () => {
        expect(component).toBeDefined();
    });

    describe('#ngOnInit', () => {
        it('sets supplierId as query param value', () => {
            const params: Params = {};
            params[idParam] = 1;

            const activatedRoute = fixture.debugElement.injector.get(ActivatedRoute) as ActivatedRoute;
            activatedRoute.snapshot.params = params;
            spyOn(component, 'refreshData');

            component.ngOnInit();

            expect(component.supplierId).toBe(params[idParam]);
        });

        it('calls refreshData', () => {
            const refreshDataSpy = spyOn(component, 'refreshData');
            component.ngOnInit();
            expect(refreshDataSpy).toHaveBeenCalledTimes(1);
        });
    });

    describe('#refreshData', () => {
        it('updates component\'s model with response', () => {
            const supplier = SupplierTestHelper.buildEditQueryResultSupplier();
            const editQueryResult: EditQueryResult = {
                suppliers: [supplier]
            };

            const editService = fixture.debugElement.injector.get(EditService) as EditService;
            const handleQuerySpy = spyOn(editService, 'handleQuery').and.returnValue( of(editQueryResult) );

            component.refreshData();

            expect(handleQuerySpy).toHaveBeenCalledTimes(1);
            expect(component.model.id).toBe(supplier.id);
            expect(component.model.name).toBe(supplier.name);
        });

        it('sends error to messsage service if error returned', () => {
            const error = TestHelper.buildError();
            const editService = fixture.debugElement.injector.get(EditService) as EditService;
            const handleQuerySpy = spyOn(editService, 'handleQuery').and.returnValue( throwError(error) );

            const messagesService = fixture.debugElement.injector.get(MessagesService) as MessagesService;
            const messagesSpy = spyOn(messagesService, 'addHttpResponseError');

            component.refreshData();

            expect(handleQuerySpy).toHaveBeenCalledTimes(1);
            expect(messagesSpy).toHaveBeenCalledWith(error);
        });
    });

    describe('#onSubmit', () => {
        it('adds message to message service if successfully edited', () => {
            component.model = SupplierTestHelper.buildEditViewModel();
            component.supplierId = 1;

            const editService = fixture.debugElement.injector.get(EditService) as EditService;
            const handleCommandSpy = spyOn(editService, 'handleCommand').and.returnValue( of({}) );

            const messagesService = fixture.debugElement.injector.get(MessagesService) as MessagesService;
            const addMessageSpy = spyOn(messagesService, 'addMessage');

            component.onSubmit();

            expect(handleCommandSpy).toHaveBeenCalledTimes(1);
            expect(addMessageSpy).toHaveBeenCalledTimes(1);
        });

        it('sends error to messsage service if error returned', () => {
            component.model = SupplierTestHelper.buildEditViewModel();
            const error = TestHelper.buildError();
            const editService = fixture.debugElement.injector.get(EditService) as EditService;
            const handleCommandSpy = spyOn(editService, 'handleCommand').and.returnValue( throwError(error) );

            const messagesService = fixture.debugElement.injector.get(MessagesService) as MessagesService;
            const messagesSpy = spyOn(messagesService, 'addHttpResponseError');

            component.onSubmit();

            expect(handleCommandSpy).toHaveBeenCalledTimes(1);
            expect(messagesSpy).toHaveBeenCalledWith(error);
        });
    });

    describe('#onDelete', () => {
        it('navigates to suppliers URL when deleted successfully', () => {
            component.supplierId = 1;

            const deleteService = fixture.debugElement.injector.get(DeleteService) as DeleteService;
            const handleCommandSpy = spyOn(deleteService, 'handle').and.returnValue( of({}) );

            const router = fixture.debugElement.injector.get(Router) as Router;
            const navigateByUrlSpy = spyOn(router, 'navigateByUrl');

            component.onDelete();

            expect(handleCommandSpy).toHaveBeenCalledTimes(1);
            expect(navigateByUrlSpy).toHaveBeenCalledWith(suppliersUrl);
        });

        it('sends error to messsage service if error returned', () => {
            const error = TestHelper.buildError();
            const deleteService = fixture.debugElement.injector.get(DeleteService) as DeleteService;
            const handleSpy = spyOn(deleteService, 'handle').and.returnValue( throwError(error) );

            const messagesService = fixture.debugElement.injector.get(MessagesService) as MessagesService;
            const messagesSpy = spyOn(messagesService, 'addHttpResponseError');

            component.onDelete();

            expect(handleSpy).toHaveBeenCalledTimes(1);
            expect(messagesSpy).toHaveBeenCalledWith(error);
        });
    });
});
