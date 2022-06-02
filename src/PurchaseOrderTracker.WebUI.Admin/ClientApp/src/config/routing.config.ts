import { homepage } from '../../package.json';

// URLs

export const adminWebsiteUrlPathPrefix = `${homepage}/`;
export const accountUrl = '/account';
export const mainSiteUrl = '/main-site';

// TODO: this will navigate the user to the logon page but not log them out (cookie will not be cleared)
// change it to POST to identity/account/logout which should return a 302 to /account
export const logoutUrl = '/account';
