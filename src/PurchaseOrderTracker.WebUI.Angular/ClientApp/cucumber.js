require('ts-node').register({
  project: 'features/tsconfig.json'
});

// https://github.com/cucumber/cucumber-js/blob/master/docs/cli.md

// different formatter for ci/tty
// `--format ${
//   process.env.CI || !process.stdout.isTTY ? 'progress' : 'progress-bar'
// }`,

// customer formatter
// '--format progress-bar', // Load custom formatter
// '--format node_modules/cucumber-pretty' // Load custom formatter

var common = [
  './features/specs/*.feature',
  // '--require-module ts-node/register',
  '--require ./features/**/*.ts',
  `--format-options '{"snippetInterface": "synchronous"}'`,
  `--world-parameters '{"pageTimeout": 30000}'`,
  `--world-parameters '{"baseUrl": "http://localhost:4200"}'`,
  '--tags @only',
  //'--format-options "{\\"snippetInterface\\": \\"async-await\\"}"',
  '--format json:reports/cucumber-report.json'

  // --format summary --format progress-bar
  // --format node_modules/cucumber-pretty",
  //   "snippets": "./node_modules/cucumber/bin/cucumber-js
].join(' ');

module.exports = {
  default: common
  //dryRyn: '--dry-run' // TODO add this param to common
};
