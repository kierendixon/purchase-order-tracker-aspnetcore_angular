import { Browser, BrowserContext, Page } from 'puppeteer';
import { PageObject } from 'features/pages/page';

export const scope: Env = {
  browser: null,
  browserCtx: null,
  args: require('minimist')(process.argv.slice(2))
};

interface Env {
  browserCtx?: BrowserContext;
  browser?: Browser;
  args: any;
}

// redefine the world object ('this') for intellisense
declare module 'cucumber' {
  interface World {
    pageObject: PageObject;
    pptrPage: Page;
  }
}
