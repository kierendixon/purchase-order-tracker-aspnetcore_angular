// import { Given, Then, When } from 'cucumber';
// import expect = require('expect');
// import { AccountPage } from '../pages/account.po';
// import { Page } from 'puppeteer';
// import { pageForUrl, PotPage } from '../support/page-identifier';

// // // debugging:
// // // https://sylvain.pontoreau.com/2018/04/30/typescript-cucumber-getting-started/
// // // https://github.com/hdorgeval/cucumber-ts-starter
// // // pptr debugging tips: https://pptr.dev/

// When('a user enters username {word}', async function(username: string) {
//   // await this.pageObject.enterUsername(username);
// });

// When('they enter password {word}', async function(password: string) {
//   // await this.pageObject.enterPassword(password);
// });

// // When('they submit the login form', async function() {
// //   await this.pageObject.submit();
// // });

// // Then('they should be shown a login error message {string}', async function(errorMessage: string) {
// //   const alertText = await this.pageObject.getAlertText();
// //   if (errorMessage != alertText) {
// //     throw new Error(`Unexpected alert text: ${alertText}`);
// //   }
// // });

// // When('they login', async function() {
// //   await this.pageObject.enterUsername('basic');
// //   await this.pageObject.enterPassword('basic');
// //   await this.pageObject.submit();
// // });

// // Given('they are asked to login', async function() {
// //   // TODO: can we add these helper functions to world instead?
// //   if (pageForUrl(this.pptrPage.url()) != PotPage.Login) {
// //     throw new Error('not on login apge');
// //   }
// // });
