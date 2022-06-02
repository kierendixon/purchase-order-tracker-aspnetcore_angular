import fetchWrapper from '../FetchWrapper';
import { userUrl } from '../../../config/api.config';

export class CreateCommand {
  constructor(readonly username: string, readonly onetimePassword: string, readonly isAdmin?: Boolean) {}
}

interface CreateResult {
  userId: number;
  succeeded: boolean;
  errors?: IdentityError[];
}

interface IdentityError {
  code: string;
  description: string;
}

interface Result {
  success: boolean;
  userId?: number;
  errors?: string[];
}

async function submit(req: CreateCommand): Promise<Result> {
  let resp: Response | undefined;

  try {
    resp = await fetchWrapper(userUrl, 'POST', req);
  } catch (error) {
    return { success: false, errors: ['Unable to connect to server'] };
  }

  if (resp !== undefined) {
    const responseData = (await resp.json()) as CreateResult;
    if (responseData.succeeded) {
      return {
        success: true,
        userId: responseData.userId,
      };
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

const CreateUserClient = { submit };
export default CreateUserClient;
