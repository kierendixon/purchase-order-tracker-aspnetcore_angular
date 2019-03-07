import { HttpErrorResponse } from '@angular/common/http';

export class TestHelper {
  public static ErrorMessage = 'an error message';
  public static NonErrorMessage = 'a non-error message';

  public static buildError(): Error {
      return new Error(TestHelper.ErrorMessage);
  }

  public static buildHttpErrorResponse(): HttpErrorResponse {
      return new HttpErrorResponse({
          status: 500,
          error: {
              message: TestHelper.ErrorMessage
          }
      });
  }
}
