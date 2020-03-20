// require('ts-node').register({
//   project: 'features/tsconfig.json'
// });

// // https://github.com/cucumber/cucumber-js/blob/master/docs/cli.md
// var common = [
//   './features/specs/**/*.feature',
//   '--require ./features/**/*.ts',
//   `--format-options '{"snippetInterface": "async-await"}'`,
//   //'--format json:./features/reports/cucumber-report.json' // TODO
//   `--world-parameters '{"pageTimeout": 30000}'`,
//   `--world-parameters '{"baseUrl": "http://localhost:4200"}'`
// ];

// var local = common.concat(['--format progress-bar']);
// var ciServer = common.concat(['--format usage']);

// // Using the `--dry-run` or `-d` flag gives you a way to quickly scan your features without actually running them.
// var dryRun = local.concat(['--dry-run']);

// // Run only features tagged with @only
// var only = local.concat(['--tags "@only"']);
// module.exports = {
//   default: local.join(' '),
//   dryRun: dryRun.join(' '),
//   only: only.join(' '),
//   ciServer: ciServer.join(' ')
// };
