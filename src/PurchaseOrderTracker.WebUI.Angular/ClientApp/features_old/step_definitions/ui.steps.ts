import { Ensure, equals } from '@serenity-js/assertions';
import { actorCalled, actorInTheSpotlight, engage } from '@serenity-js/core';
import { Navigate, UseAngular, Website } from '@serenity-js/protractor';
import { After, Before, Then, When } from 'cucumber';
import { Actors } from '../support/screenplay/Actors';

Before(() => {
  console.log('Cucumber :: sneario-level before');
  engage(new Actors());
});

When(/^(.*) navigates to the test website$/, (actorName: string) =>
  actorCalled(actorName).attemptsTo(UseAngular.disableSynchronisation(), Navigate.to('http://localhost:4201'))
);

Then(/(?:he|she|they) should see the title of "(.*)"/, (expectedTitle: string) =>
  actorInTheSpotlight().attemptsTo(Ensure.that(Website.title(), equals(expectedTitle)))
);
