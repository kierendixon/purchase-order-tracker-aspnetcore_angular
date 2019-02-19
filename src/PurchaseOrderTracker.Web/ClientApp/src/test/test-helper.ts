import { HttpErrorResponse } from '@angular/common/http';

export class TestHelper {
    public static buildError(): Error {
        return new Error('an error message');
    }

    public static buildHttpErrorResponse(): HttpErrorResponse {
        return new HttpErrorResponse({
            status: 500,
            error: {
                message: 'an error message'
            }
        });
    }
}
