import { Ensure, startsWith, endsWith, equals } from '@serenity-js/assertions';
import { actorCalled, actorInTheSpotlight, Duration } from '@serenity-js/core';
import { Navigate, Website, Enter, Target, Click, Wait, Text } from '@serenity-js/protractor';
import { Then, When, Given, TableDefinition } from 'cucumber';
import { by, protractor } from 'protractor';
import { User } from '../support/screenplay/questions/User';

Given(/^(.*) navigates to the All Suppliers report$/, (actorName: string) =>
  actorCalled(actorName).attemptsTo(
    Navigate.to('http://localhost:4202/main-site/suppliers/inquiry?queryType=All'),
    Wait.for(Duration.ofMilliseconds(500)),
    Enter.theValue('basic').into(Target.the('username field').located(by.name('username'))),
    Enter.theValue('basic').into(Target.the('password field').located(by.name('password'))),
    Click.on(Target.the('submit button').located(by.css('button[type="submit"]'))),
    Wait.for(Duration.ofMilliseconds(500))
  )
);

When(/^s?he clicks on a supplier's id/, () =>
  actorInTheSpotlight().attemptsTo(
    Click.on(Target.the('first supplier in the report').located(by.css('table td a'))),
    Wait.for(Duration.ofMilliseconds(100))
  )
);

When(/^s?he clicks on a supplier's products link/, () =>
  actorInTheSpotlight().attemptsTo(
    Click.on(Target.the('first supplier in the report').located(by.css('table td:nth-child(3) a'))),
    Wait.for(Duration.ofMilliseconds(100))
  )
);

Then(/^s?he should be navigated to the supplier's details page$/, () =>
  actorInTheSpotlight().attemptsTo(Ensure.that(Website.url(), endsWith('main-site/suppliers/2')))
);

Then(/^s?he should be navigated to the supplier's products page$/, () =>
  actorInTheSpotlight().attemptsTo(Ensure.that(Website.url(), endsWith('main-site/suppliers/2/edit-products')))
);

Given(/^s?he opens the All Suppliers report$/, () =>
  actorInTheSpotlight().attemptsTo(
    Click.on(Target.the('Supplier navbar button').located(by.css('.navbar li:nth-child(2) a'))),
    Wait.for(Duration.ofMilliseconds(100)),
    Click.on(Target.the('All Suppliers report button').located(by.css('#nav-left a:nth-child(1)'))),
    Wait.for(Duration.ofMilliseconds(100))
  )
);

Then(/she should see the following suppliers/, (dataTable: TableDefinition) => {
  const expectedSuppliers = dataTable.rows().map(r => r[0]);

  return actorInTheSpotlight().attemptsTo(
    Ensure.that(
      Text.ofAll(Target.all('supplier names').located(by.css('table td:nth-child(2)'))),
      equals(expectedSuppliers)
    )
  );
});
