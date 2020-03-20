import { execSync } from 'child_process';
import * as portfinder from 'portfinder';

const sqlContainer = 'microsoft/mssql-server-windows-developer:2017-CU3';

export function checkIfDockerInstalled() {
  console.log('Checking if Docker is installed...');
  // TODO pass stdio in
  // this will throw an exception if `docker` command cannot be found
  const output = execSync('docker --version');
  process.stdout.write(output);
}

export async function startNewSqlServerContainer(saPassword = 'PoTracker001') {
  const port = await portfinder.getPortPromise();
  console.log('Starting SQL Server Docker container...');
  const output = execSync(
    `docker run --rm -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=${saPassword}" -p ${port}:1433 -d ${sqlContainer}`
  );
  // check if output contains a container id
  if (output.length == 65) {
    console.log(`Started container ${output.toString()}`);
    return new DockerContainer(output.toString().substr(0, 64), port);
  }

  console.log(output.toString());
  throw new Error('Error starting SQL Server Docker container - container ID not returned by start command');
}

export function stopContainer(containerId: string) {
  console.log('Stopping SQL Server Docker container...');
  console.log(`Container id:  ${containerId}`);
  execSync(`docker container stop -t 10 ${containerId}`);
}

export class DockerContainer {
  constructor(public id: string, public port: number) {}
}
