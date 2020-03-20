export interface CliArgs {
  sqlServerConnectionString: string;
  potWebsiteUrl: string;
  stepTimeout: number;
  startSqlDocker: boolean;
  startApplicationFrontend: boolean;
  startApplicationBackend: boolean;
}
