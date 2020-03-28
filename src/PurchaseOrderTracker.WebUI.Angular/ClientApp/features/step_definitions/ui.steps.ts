import { Ensure, startsWith, endsWith, equals } from '@serenity-js/assertions';
import { actorCalled, actorInTheSpotlight, Duration } from '@serenity-js/core';
import { Navigate, Website, Enter, Target, Click, Wait, Text } from '@serenity-js/protractor';
import { Then, When, Given, TableDefinition } from 'cucumber';
import { by, protractor } from 'protractor';
import { User } from '../support/screenplay/questions/User';

Given(/^(.*) navigates to ([^\s]+)$/, (actorName: string, url: string) =>
  actorCalled(actorName).attemptsTo(
    // StartLocalServer.onRandomPort(),
    // UseAngular.disableSynchronisation()
    Navigate.to(url[0] == '/' ? 'http://localhost:4200/' + url : url), // LocalServer.url()
    Wait.for(Duration.ofMilliseconds(500))
  )
);

Given(/^(.*) navigates to the ([^\s]+) page$/, (actorName: string, page: string) => {
  let url: string;
  switch (page) {
    case 'Login':
      url = '/account';
      break;
    default:
      throw new Error('unknown page ' + page);
  }

  return actorCalled(actorName).attemptsTo(
    Navigate.to('http://localhost:4200' + url),
    Wait.for(Duration.ofMilliseconds(500))
  );
});

When(/^s?he opens the user guide$/, () =>
  actorInTheSpotlight().attemptsTo(Click.on(Target.the('Help button').located(by.css('#navbarNav a:nth-child(2)'))))
);

Then('the user guide should be displayed', async () => {
  //TODO: move into serenityjs question or action
  const handles = await protractor.browser.getAllWindowHandles();
  const newWindowHandle = handles[1]; // this is your new window
  await protractor.browser.switchTo().window(newWindowHandle);
  const currentUrl = await protractor.browser.getCurrentUrl();

  await actorInTheSpotlight().attemptsTo(
    Ensure.that(
      currentUrl,
      equals('https://github.com/kierendixon/purchase-order-tracker-aspnetcore_angular/wiki/User-Guide')
    )
  );
  await protractor.browser.close();
  // TODO: if this doesn't work the next tests may fail unexpectedly
  await protractor.browser.switchTo().window(handles[0]);
});

When(/^s?he enters username (.*)$/, function(username: string) {
  return actorInTheSpotlight().attemptsTo(
    Enter.theValue(username).into(Target.the('username field').located(by.name('username')))
  );
});

When(/^s?he enters password (.*)$/, function(password: string) {
  return actorInTheSpotlight().attemptsTo(
    Enter.theValue(password).into(Target.the('password field').located(by.name('password')))
  );
});

When(/^s?he submits the login form$/, function() {
  return actorInTheSpotlight().attemptsTo(
    Click.on(Target.the('submit button').located(by.css('button[type="submit"]'))),
    Wait.for(Duration.ofMilliseconds(500))
  );
});

Given(/^s?he is asked to login$/, function() {
  return actorInTheSpotlight().attemptsTo(Ensure.that(Website.url(), startsWith('http://localhost:4200/account')));
});

Given(/^(.*) is logged in$/, function(actorName: string) {
  return actorCalled(actorName).attemptsTo(
    Navigate.to('http://localhost:4200/account'),
    Enter.theValue('basic').into(Target.the('username field').located(by.name('username'))),
    Enter.theValue('basic').into(Target.the('password field').located(by.name('password'))),
    Click.on(Target.the('submit button').located(by.css('button[type="submit"]'))),
    Wait.for(Duration.ofMilliseconds(500))
  );
});
Given(/^(.*) logs in$/, function(actorName: string) {
  return actorCalled(actorName).attemptsTo(
    Navigate.to('http://localhost:4200/account'),
    Enter.theValue('basic').into(Target.the('username field').located(by.name('username'))),
    Enter.theValue('basic').into(Target.the('password field').located(by.name('password'))),
    Click.on(Target.the('submit button').located(by.css('button[type="submit"]'))),
    Wait.for(Duration.ofMilliseconds(500))
  );
});

When(/^s?he logs out$/, function() {
  return actorInTheSpotlight().attemptsTo(
    Click.on(Target.the('logout button').located(by.css('a#logout'))),
    Wait.for(Duration.ofMilliseconds(500))
  );
});

Then(/^s?he will no longer be authenticated$/, function() {
  // TODO save the token before logging out and perform a HTTP request to ensure that
  // the token doesn't work for API calls anymore
  return actorInTheSpotlight().attemptsTo(Ensure.that(User.isAuthenticated(), equals(false)));
});

When(/^s?he logs in$/, function() {
  return actorInTheSpotlight().attemptsTo(
    Enter.theValue('basic').into(Target.the('username field').located(by.name('username'))),
    Enter.theValue('basic').into(Target.the('password field').located(by.name('password'))),
    Click.on(Target.the('submit button').located(by.css('button[type="submit"]')))
  );
});

Then(/^s?he should be redirected to ([^\s]+)$/, function(url: string) {
  return actorInTheSpotlight().attemptsTo(
    Wait.for(Duration.ofMilliseconds(500)),
    Ensure.that(Website.url(), endsWith(url))
  );
});

Then(/^s?he should be (?:redirected to|shown) the default landing page$/, function() {
  return actorInTheSpotlight().attemptsTo(
    Wait.for(Duration.ofMilliseconds(500)),
    Ensure.that(Website.url(), endsWith('/main-site'))
  );
});

Then(/^s?he should see the following purchase order summary$/, (dataTable: TableDefinition) => {
  interface DataTable {
    OpenOrders: string;
    DeliveriesToday: string;
    LateDeliveries: string;
    VeryLateDeliveries: string;
  }
  const expectedValues: DataTable = (dataTable.hashes()[0] as unknown) as DataTable;

  return actorInTheSpotlight().attemptsTo(
    Ensure.that(
      Text.of(Target.the('total open orders').located(by.css('main span:nth-child(3)'))),
      equals(`Total open orders: ${expectedValues.OpenOrders}`)
    ),
    Ensure.that(
      Text.of(Target.the('deliveries for today').located(by.css('main span:nth-child(5)'))),
      equals(`Shipments scheduled for delivered today: ${expectedValues.DeliveriesToday}`)
    ),
    Ensure.that(
      Text.of(Target.the('late deliveries').located(by.css('main span:nth-child(7)'))),
      equals(`Shipments past delivery due date: ${expectedValues.LateDeliveries}`)
    ),
    Ensure.that(
      Text.of(Target.the('very late deliveries').located(by.css('main span:nth-child(9)'))),
      equals(`Shipments past delivery due date more than 7 days: ${expectedValues.VeryLateDeliveries}`)
    )
  );
});

Then(/^s?he should be shown a login error message "(.*)"$/, function(message: string) {
  return actorInTheSpotlight().attemptsTo(
    Ensure.that(
      Text.of(Target.the('login error message').located(by.css('div[class="alert alert-danger"]'))),
      equals(message)
    )
  );
});
