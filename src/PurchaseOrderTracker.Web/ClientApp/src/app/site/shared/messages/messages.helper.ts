import { HttpErrorResponse } from '@angular/common/http';

export class MessagesHelper {
  public static ConvertErrorToFriendlyMessage(err: any): string {
    if (err) {
      if (err.error instanceof Error) {
        // A client-side or network error occurred
        return err.error.message;
      } else if (err instanceof Error) {
        // Error created from this codebase using new Error()
        return err.message;
      } else {
        // The backend returned an unsuccessful response code
        const httpErr = err as HttpErrorResponse;
        return `Error returned from server. Code: ${httpErr.status}, body: ${JSON.stringify(httpErr.error)}`;
      }
    }

    return '';
  }
}
