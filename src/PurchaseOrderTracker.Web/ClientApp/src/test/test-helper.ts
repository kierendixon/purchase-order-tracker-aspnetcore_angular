import { HttpErrorResponse } from '@angular/common/http';

export class TestHelper {
  public static ErrorMessage = 'an error message';
  public static NonErrorMessage = 'a non-error message';

  public static buildError(): Error {
    return new Error(TestHelper.ErrorMessage);
  }

  public static buildHttpErrorResponse(status = 500): HttpErrorResponse {
    return new HttpErrorResponse({
      status,
      error: {
        message: TestHelper.ErrorMessage
      }
    });
  }
}
