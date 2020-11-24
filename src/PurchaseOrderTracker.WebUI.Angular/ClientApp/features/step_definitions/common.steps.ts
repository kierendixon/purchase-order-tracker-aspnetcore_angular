import { Ensure, startsWith, endsWith, equals } from '@serenity-js/assertions';
import { actorCalled, actorInTheSpotlight, Duration } from '@serenity-js/core';
import { Navigate, Website, Enter, Target, Click, Wait, Text } from '@serenity-js/protractor';
import { Then, When, Given, TableDefinition } from 'cucumber';
import { by, protractor } from 'protractor';
import { User } from '../support/screenplay/questions/User';

Given(/^(.*) navigates to ([^\s]+)$/, (actorName: string, url: string) =>
  actorCalled(actorName).attemptsTo(
    // StartLocalServer.onRandomPort(), UseAngular.disableSynchronisation()
    Navigate.to(url[0] == '/' ? 'http://localhost:4202/' + url : url), // LocalServer.url()
    Wait.for(Duration.ofMilliseconds(500))
  )
);
