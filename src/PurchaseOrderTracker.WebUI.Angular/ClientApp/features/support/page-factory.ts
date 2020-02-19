import { Page } from 'puppeteer';
import { AccountPage } from '../pages/account.po';
import { PotPage } from './page-identifier';
import { PageObject } from '../pages/page';

export class PageFactory {
  public static get(page: Page, potPage: PotPage): PageObject {
    if (potPage == PotPage.Login) {
      return new AccountPage(page);
    }

    throw new Error('shouildnt get here');
    // no page object exists for the given PotPage, return null
    return null;
  }
}
