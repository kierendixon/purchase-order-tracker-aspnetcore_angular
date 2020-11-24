import { Ensure, startsWith, endsWith, equals } from '@serenity-js/assertions';
import { actorCalled, actorInTheSpotlight, Duration } from '@serenity-js/core';
import { Navigate, Website, Enter, Target, Click, Wait, Text } from '@serenity-js/protractor';
import { Then, When, Given, TableDefinition } from 'cucumber';
import { by, protractor } from 'protractor';
import { User } from '../support/screenplay/questions/User';

When(/^s?he logs out$/, function() {
  return actorInTheSpotlight().attemptsTo(
    Click.on(Target.the('logout button').located(by.css('a#logout'))),
    Wait.for(Duration.ofMilliseconds(500))
  );
});

Then(/^s?he will no longer be authenticated$/, function() {
  // TODO save the token before logging out and perform a HTTP request to ensure
  // that the token doesn't work for API calls anymore
  return actorInTheSpotlight().attemptsTo(Ensure.that(User.isAuthenticated(), equals(false)));
});
