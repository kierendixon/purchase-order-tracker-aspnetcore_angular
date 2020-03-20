const { ConsoleReporter } = require('@serenity-js/console-reporter'),
  { ArtifactArchiver, StreamReporter } = require('@serenity-js/core'),
  { SerenityBDDReporter } = require('@serenity-js/serenity-bdd');
//  isCI = require('is-ci');

require('ts-node').register({
  project: 'features/tsconfig.json'
});

exports.config = {
  chromeDriver: require('chromedriver/lib/chromedriver').path,
  SELENIUM_PROMISE_MANAGER: false,

  // restartBrowserBetweenTests: true,

  directConnect: true,

  allScriptsTimeout: 11000,

  specs: ['features/specs/**/*.feature'],

  framework: 'custom',
  frameworkPath: require.resolve('@serenity-js/protractor/adapter'),

  serenity: {
    crew: [
      ArtifactArchiver.storingArtifactsAt('./target/site/serenity'),
      new SerenityBDDReporter(),
      ConsoleReporter.forDarkTerminals()
      // Photographer.whoWill(TakePhotosOfInteractions) // or Photographer.whoWill(TakePhotosOfFailures),
      // new StreamReporter(),
    ]
  },

  /**
   * If you're interacting with a non-Angular application,
   * uncomment the below onPrepare section,
   * which disables Angular-specific test synchronisation.
   */
  onPrepare: function() {
    browser.waitForAngularEnabled(false);
  },

  cucumberOpts: {
    require: ['features/step_definitions/**/*.ts', 'features/support/setup.ts'],
    // 'require-module': ['ts-node/register']
    tags: ['@only']
  },

  capabilities: {
    browserName: 'chrome',

    chromeOptions: {
      args: ['--disable-infobars', '--no-sandbox', '--disable-gpu', '--window-size=1920x1080'] // '--headless'
      //].concat(isCI ? ['--headless'] : [])    // run in headless mode on the CI server
    }
  }
};
