import fetchWrapper from '../FetchWrapper';
import { userUrl } from '../../../config/api.config';

export class EditUserCommand {
  constructor(
    readonly id: string,
    readonly userName: string,
    readonly isAdmin: boolean | null,
    readonly lockoutEnd: Date | null
  ) {}
}

interface EditUserResult {
  succeeded: boolean;
  errors?: IdentityError[];
}

interface IdentityError {
  code: string;
  description: string;
}

interface Result {
  success: boolean;
  errors?: string[];
}

// todo write tests
async function submit(req: EditUserCommand): Promise<Result> {
  let resp: Response | undefined;

  try {
    resp = await fetchWrapper(`${userUrl}/${req.id}`, 'POST', req);
  } catch (error) {
    return { success: false, errors: ['Unable to connect to server'] };
  }

  if (resp !== undefined) {
    const responseData = (await resp.json()) as EditUserResult;
    if (responseData.succeeded) {
      return {
        success: true,
      };
      // todo duplicate error handling code from create client
    } else if (resp.status.toString()[0] === '4') {
      return {
        success: false,
        errors: responseData.errors?.map((err) => `${err.code} - ${err.description}`),
      };
    } else if (resp.status.toString()[0] === '5') {
      return {
        success: false,
        errors: ['Unexpected server error'],
      };
    }
  }

  return { success: false, errors: ['Unknown error'] };
}

const EditUserClient = { submit };
export default EditUserClient;
