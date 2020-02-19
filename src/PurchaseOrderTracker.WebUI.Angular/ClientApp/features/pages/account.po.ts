import { Page, ElementHandle } from 'puppeteer';
import { PageObject } from './page';

export class AccountPage implements PageObject {
  constructor(private page: Page) {}

  async getAlertText(): Promise<string> {
    return this.page.$('.alert').then(() => this.page.$eval('.alert', el => el.textContent));
  }

  async isLoggedIn(): Promise<boolean> {
    return this.page.$('#logout').then(elem => elem != null);
  }

  async submit(): Promise<ElementHandle<Element>> {
    return (
      this.page
        .$('#signin')
        .then(elem => elem.click())
        // wait for HTTP response to be processed, which will either display an Alert for an error
        // or display the dashboard where the logout button exists, or display the Not Found page
        .then(() => this.page.waitForSelector('.alert, #logout, #page-not-found', { visible: true }))
    );
  }

  async enterUsername(username) {
    return this.page.$('[name=username]').then(elem => elem.type(username));
  }

  // todo: remove 'enter'
  enterPassword(password) {
    return this.page.$('[name=password]').then(elem => elem.type(password));
  }
}
