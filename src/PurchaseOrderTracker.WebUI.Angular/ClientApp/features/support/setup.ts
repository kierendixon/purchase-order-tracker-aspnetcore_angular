import { CliArgs } from './cliargs';
import { setWorldConstructor, setDefaultTimeout, BeforeAll, Before, After, Status, AfterAll } from 'cucumber';
import { CustomWorld } from './world';
import {
  checkIfDockerInstalled,
  startNewSqlServerContainer,
  DockerContainer,
  stopContainer
} from './docker/docker-manager';
import { protractor } from 'protractor';
import { engage } from '@serenity-js/core';
import { Actors } from './screenplay/Actors';

// TODO read args from cli but use these as defeaults
const config: CliArgs = {
  sqlServerConnectionString: '',
  potWebsiteUrl: '',
  stepTimeout: 30 * 1000,
  startSqlDocker: false,
  startApplicationBackend: false,
  startApplicationFrontend: false
};

// override cucumber's default step timeout of 5000ms
setDefaultTimeout(config.stepTimeout);

// 'World' is an isolated context for each scenario, exposed to the hooks and steps as `this`
setWorldConstructor(CustomWorld);

let sqlContainer: DockerContainer;

BeforeAll({ timeout: 60 * 1000 }, async () => {
  // workaround for cucumberjs bug where dryRun executes beforeAll hooks
  // this relies on dryRun being specified via the command line as a cucumber profile
  // see https://github.com/cucumber/cucumber-js/issues/1258
  if (process.argv.some(arg => arg.includes('dryRun'))) {
    return;
  }

  if (config.startSqlDocker) {
    checkIfDockerInstalled();
    sqlContainer = await startNewSqlServerContainer();
  }

  if (config.startApplicationBackend) {
    // TODO
  }

  if (config.startApplicationFrontend) {
    //TODO
  }
});

Before({ tags: 'not @manual' }, async function() {
  console.log('Cucumber :: scenario-level before');
  return engage(new Actors());
});

Before({ tags: '@manual' }, () => 'skipped');

// replaced with actors
After({ tags: 'not @manual' }, async function(scenario) {
  if (scenario.result.status === Status.FAILED /*|| config.documentationMode*/) {
    // const screenshot = await this.page.screenshot();
    // this.attach(screenshot, 'image/png' as never); // TODO
  }
  // reset database

  // the same browser instance is used for all tests
  await protractor.browser.executeScript('window.sessionStorage.clear();');
  await protractor.browser.executeScript('window.localStorage.clear();');
});

AfterAll({ timeout: 60 * 1000 }, async () => {
  if (sqlContainer !== undefined) {
    stopContainer(sqlContainer.id);
  }
});
