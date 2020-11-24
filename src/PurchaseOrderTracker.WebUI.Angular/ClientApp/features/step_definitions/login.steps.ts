import { Ensure, startsWith, endsWith, equals } from '@serenity-js/assertions';
import { actorCalled, actorInTheSpotlight, Duration } from '@serenity-js/core';
import { Navigate, Website, Enter, Target, Click, Wait, Text } from '@serenity-js/protractor';
import { Then, When, Given, TableDefinition } from 'cucumber';
import { by, protractor } from 'protractor';
import { User } from '../support/screenplay/questions/User';

Given(/^(.*) navigates to the Login page$/, (actorName: string) =>
  actorCalled(actorName).attemptsTo(
    //TODO
    Navigate.to('http://localhost:4202/account'),
    Wait.for(Duration.ofMilliseconds(500))
  )
);

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
  return actorInTheSpotlight().attemptsTo(Ensure.that(Website.url(), startsWith('http://localhost:4202/account')));
});

Given(/^(.*) is logged in$/, function(actorName: string) {
  return actorCalled(actorName).attemptsTo(
    Navigate.to('http://localhost:4202/account'),
    Enter.theValue('basic').into(Target.the('username field').located(by.name('username'))),
    Enter.theValue('basic').into(Target.the('password field').located(by.name('password'))),
    Click.on(Target.the('submit button').located(by.css('button[type="submit"]'))),
    Wait.for(Duration.ofMilliseconds(500))
  );
});

When(/^s?he logs in$/, function() {
  return actorInTheSpotlight().attemptsTo(
    Enter.theValue('basic').into(Target.the('username field').located(by.name('username'))),
    Enter.theValue('basic').into(Target.the('password field').located(by.name('password'))),
    Click.on(Target.the('submit button').located(by.css('button[type="submit"]')))
  );
});

Given(/^^(?!s?he)(.*) logs in$/, function(actorName: string) {
  return actorCalled(actorName).attemptsTo(
    Navigate.to('http://localhost:4202/account'),
    Enter.theValue('basic').into(Target.the('username field').located(by.name('username'))),
    Enter.theValue('basic').into(Target.the('password field').located(by.name('password'))),
    Click.on(Target.the('submit button').located(by.css('button[type="submit"]'))),
    Wait.for(Duration.ofMilliseconds(500))
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

Then(/^s?he should be shown a login error message "(.*)"$/, function(message: string) {
  return actorInTheSpotlight().attemptsTo(
    Ensure.that(
      Text.of(Target.the('login error message').located(by.css('div[class="alert alert-danger"]'))),
      equals(message)
    )
  );
});
