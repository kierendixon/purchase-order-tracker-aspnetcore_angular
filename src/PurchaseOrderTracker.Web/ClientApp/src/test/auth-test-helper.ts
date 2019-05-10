import { CurrentUser, JsonWebToken } from 'src/app/infrastructure/security/auth.service';

export class AuthTestHelper {
  public static buildCurrentUser(): CurrentUser {
    return {
      token: AuthTestHelper.buildJsonWebToken()
    };
  }

  public static buildJsonWebToken(access_token = 'access_token'): JsonWebToken {
    return {
      access_token,
      expires_in: 1,
      refresh_token: 'refresh_token',
      token_type: 'token_type'
    };
  }
}
