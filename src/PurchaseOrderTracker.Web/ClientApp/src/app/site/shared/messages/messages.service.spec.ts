import { TestBed } from '@angular/core/testing';
import { NavigationStart, Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';

import { TestHelper } from '../../../../test/test-helper';
import { MessagesService } from './messages.service';

describe('MessagesService', () => {
    let service: MessagesService;
    let eventsSub: BehaviorSubject<any>;

    beforeEach(() => {
        eventsSub = new BehaviorSubject<any>(null);

        TestBed.configureTestingModule({
            providers: [
                MessagesService,
                {
                    provide: Router,
                    useValue: {
                      events: eventsSub
                    }
                }
            ]
        });

        service = TestBed.get(MessagesService);
    });

    describe('#addMessage', () => {
        it('adds message as error type', () => {
            service.addMessage(TestHelper.ErrorMessage, true);
            expectSingleMessage(true, TestHelper.ErrorMessage);
        });

        it('adds message as non-error type', () => {
            service.addMessage(TestHelper.NonErrorMessage, false);
            expectSingleMessage(false, TestHelper.NonErrorMessage);
        });

        it('adds message as non-error type when type not specified', () => {
            service.addMessage(TestHelper.NonErrorMessage);
            expectSingleMessage(false, TestHelper.NonErrorMessage);
        });
    });

    describe('#addErrorMessage', () => {
        it('adds message as error type', () => {
            service.addErrorMessage(TestHelper.ErrorMessage);
            expectSingleMessage(true, TestHelper.ErrorMessage);
        });
    });

    describe('#addHttpResponseError', () => {
        it('converts Error type to message', () => {
            const error = TestHelper.buildError();
            service.addHttpResponseError({ error });
            expectSingleMessage(true, error.message);
        });

        it('converts HttpErrorResponse type to message', () => {
            const error = TestHelper.buildHttpErrorResponse();
            service.addHttpResponseError(error);

            expect(service.getMessages().length).toBe(1);
            expect(service.getMessages()[0].isError).toBe(true);
            expect(service.getMessages()[0].msg).toContain(error.status.toString());
            expect(service.getMessages()[0].msg).toContain(error.error.message);
        });
    });

    describe('#getMessages', () => {
        it('returns correct message count', () => {
            createAndAddMessages(3);

            expect(service.getMessages().length).toBe(3);
        });
    });

    describe('#hasMessages', () => {
        it('returns true when it has messages', () => {
            service.addMessage('a message1');
            expect(service.hasMessages()).toBe(true);
        });

        it('returns false when it has no messages', () => {
            expect(service.hasMessages()).toBe(false);
        });
    });

    describe('#removeMessage', () => {
        it('removes 3rd message in a list of 5 messages', () => {
            createAndAddMessages(5);

            const messages = service.getMessages();
            const thirdMessage = messages[2];
            service.removeMessage(2);

            expect(messages.length).toBe(4);
            expect(messages).not.toContain(thirdMessage);
        });

        it('removes 1st message in a list of 5 messages', () => {
            createAndAddMessages(5);

            const messages = service.getMessages();
            const firstMessage = messages[0];
            service.removeMessage(0);

            expect(messages.length).toBe(4);
            expect(messages).not.toContain(firstMessage);
        });

        it('removes 5th message in a list of 5 messages', () => {
            createAndAddMessages(5);

            const messages = service.getMessages();
            const fifthMessage = messages[4];
            service.removeMessage(4);

            expect(messages.length).toBe(4);
            expect(messages).not.toContain(fifthMessage);
        });
    });

    describe('#clearMessages', () => {
        it('clears messages when route changes', () => {
            service.addMessage('a message');

            const routerEvent = new NavigationStart(0, '/anywhere');
            eventsSub.next(routerEvent);

            expect(service.getMessages().length).toBe(0);
        });

        it('clears messages', () => {
            service.addMessage('a message');
            service.clearMessages();

            expect(service.getMessages().length).toBe(0);
        });
    });

    // helper functions

    function expectSingleMessage(isError: boolean, expectedMessage: string) {
        expect(service.getMessages().length).toBe(1);
        expect(service.getMessages()[0].isError).toBe(isError);
        expect(service.getMessages()[0].msg).toBe(expectedMessage);
    }

    function createAndAddMessages(count: number) {
        for (let i = 0; i < count; i++) {
            service.addMessage(`a message${i + 1}`);
        }
    }
});
