import { Page } from 'puppeteer';
import { accountUrl, mainSiteUrl } from '../../src/app/config/routing.config';

export enum PotPage {
  Login = 'Login',
  MainSiteLanding = 'MainSiteLanding'
}

export function pageForUrl(url: string): PotPage {
  if (url.indexOf(accountUrl) > -1) {
    return PotPage.Login;
  } else if (url.indexOf(mainSiteUrl) > -1) {
    return PotPage.MainSiteLanding;
  }

  throw new Error(`cannot identify page from url ${url}`);
}

export function urlForPage(page: PotPage): string {
  switch (page) {
    case PotPage.Login:
      return accountUrl;
    case PotPage.MainSiteLanding:
      return mainSiteUrl;
  }

  throw new Error(`unable to determine url for page ${page}`);
}
