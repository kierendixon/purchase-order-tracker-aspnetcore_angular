import { Given, Then, When } from 'cucumber';
import { PageFactory } from '../support/page-factory';
import { pageForUrl, PotPage, urlForPage } from '../support/page-identifier';

Given('a user navigates to {word}', async function(url: string) {
  await Object.bind(goto, this)(url);
});

Given('a user navigates to the {word} page', async function(potPage: keyof typeof PotPage) {
  await Object.bind(goto, this)(urlForPage(PotPage[potPage]));
  this.pageObject = PageFactory.get(this.pptrPage, PotPage[potPage]);
});

Then('they should be redirected to {word}', function(url: string) {
  if (this.pptrPage.url().indexOf(url) == -1) {
    throw new Error(`expected url ${url} but was ${this.pptrPage.url()}`);
  }
});

Given('they should be shown the {word} page', async function(potPage: keyof typeof PotPage) {
  const pageUrl = this.pptrPage.url();
  const currentPage = pageForUrl(pageUrl);
  if (potPage != currentPage) {
    throw new Error(`expected ${potPage} page but was ${currentPage}. Url: ${pageUrl}`);
  }
});

const goto = async (url: string): Promise<void> =>
  await this.page.goto(getAbsoluteUrl(url), { waitUntil: 'domcontentloaded' });

const getAbsoluteUrl = (url: string): string =>
  url.indexOf('http') == 0 ? url : this.parameters.baseUrl + (url[0] == '/' ? url : '/' + url);
