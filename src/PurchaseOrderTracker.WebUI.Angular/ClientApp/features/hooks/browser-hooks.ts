import { After, AfterAll, Before, BeforeAll, setDefaultTimeout } from 'cucumber';
import { launch } from 'puppeteer';
import { scope } from '../support/env';

const { execSync } = require('child_process');
const sqlServerImage = 'microsoft/mssql-server-windows-developer:2017-CU3';
const sqlServerContainerPort = 1433;
const sqlServerSaPassword = 'PoTracker001'; // 'UPPERlower10';
const sleep = seconds => new Promise(resolve => setTimeout(resolve, seconds * 1000));
let containerId: string;

setDefaultTimeout(60 * 1000);

Before(function() {
  return scope.browser
    .createIncognitoBrowserContext()
    .then(ctx => {
      scope.browserCtx = ctx;
      return ctx.newPage();
    })
    .then(page => {
      page.setDefaultTimeout(this.parameters.pageTimeout);
      this.pptrPage = page;

      return page.setViewport({
        width: 2048,
        height: 1536,
        deviceScaleFactor: 1
      });
    });
  // https://pptr.dev/#?product=Puppeteer&version=v2.0.0&show=api-class-coverage
  // .then(() => {
  //   return Promise.all([this.pptrPage.coverage.startJSCoverage(), this.pptrPage.coverage.startCSSCoverage()]);
  // });
});

let x = 1;
After(async function() {
  if (x == 1) {
    this.attach('{"name": "some JSON"}', 'application/json');
    this.attach('some text');

    const screenshotBuffer = await this.pptrPage.screenshot({
      fullPage: false
    });

    await this.attach(screenshotBuffer, 'image/png');
    x++;
  }

  this.pptrPage = null;
  await scope.browserCtx.close();
});

AfterAll(async () => {
  if (scope.browser) {
    await scope.browser.close();
  }

  // tryStopSqlDockerContainer();

  // scope.api.shutdown(() => console.log('\nAPI is shut down'));
  // scope.web.shutdown(() => console.log('Web is shut down'));

  // todo: scope.browser.pages().forEach(page => .. take screenshot)
});

BeforeAll({ timeout: 60 * 1000 }, () => {
  throwExceptionIfPreReqNotFound();
  // downloadSqlServerDockerImage();
  // return startSqlServerContainer()
  //   .then(() => startWebApplication())
  //   .then(() => {

  // start app
  const headless = process.env.headless != null;
  return launch({ headless }).then(browser => {
    scope.browser = browser;
  });
  // });
});

function startWebApplication() {
  // TODO
  return Promise.resolve(null);
}

function throwExceptionIfPreReqNotFound() {
  const programs = ['docker', 'dotnet'];
  for (let program of programs) {
    try {
      execSync(`where ${program}`);
    } catch (error) {
      throw new Error(`Cannot find ${program} on path`);
    }
  }
}

function downloadSqlServerDockerImage() {
  const output = execSync(`docker images ${sqlServerImage} -q`);
  if (output == '') {
    console.log(`Docker image missing: ${sqlServerImage}`);
    console.log('Downloading image');
    execSync(`docker image pull ${sqlServerImage}`, { stdio: 'inherit' });
  }
}

async function startSqlServerContainer() {
  console.log('Attempting to start SQL Server Docker container.');
  containerId = execSync(
    `docker run -d --rm -p ${sqlServerContainerPort}:1433 -e sa_password=${sqlServerSaPassword} -e ACCEPT_EULA=Y ${sqlServerImage}`
  );

  try {
    console.log(`Container started ${containerId}`);
    console.log('Waiting for SQL Server to fully start');
    await sleep(15);

    const timeout = 60 * 1;
    let timeoutCounter = 15;
    // TODO: check error in output
    while (execSync(`docker container logs ${containerId}`).indexOf('Started SQL Server') == -1) {
      timeoutCounter += 5;
      sleep(5);
      if (timeoutCounter > timeout) {
        throw new Error(`SQL Server docker container failed to start after ${timeout} minutes`);
      }
    }
  } catch (error) {
    console.log(error);
    tryStopSqlDockerContainer();
    throw error;
  }
}

function tryStopSqlDockerContainer() {
  if (containerId != undefined) {
    console.log(`Attempting to stop docker container ${containerId}`);
    execSync(`docker container stop -t 1 ${containerId}`);
  }
}
