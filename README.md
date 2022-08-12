# Purchase Order Tracker

A line-of-business system that tracks the delivery of purchase orders.

Maintain suppliers and their products, create purchase orders with line items, and assign orders to shipments for delivery.

_this is a pet project for the purpose of learning about software design and development._

## User Guides

Learn how to use Purchase Order Tracker by reading the user guides in the [Wiki](https://github.com/kierendixon/purchase-order-tracker-aspnetcore_angular/wiki).

## Quick Start

Purchase Order Tracker can be quickly and easily started using Docker.
You don't need to install anything other than Docker itself because multi-stage builds are used to compile source code in containers.

Execute the following commands from the `dev\local\docker` directory:

1. `docker-compose build`
1. `docker-compose up`

Ports 1433, 4890, and 15200 must be free. These can be changed in the `docker-compose.yml` file.

Once the containers are started, you can access the application at http://localhost:4890.

Two accounts are created by default that can be used to login:

| Username | Password | Is Admin |
| -------- | -------- | -------- |
| basic    | basic    | false    |
| super    | super    | true     |

# Technical Details

Purchase Order Tracker is comprised of multiple components:

1. The main website - for day-to-day work, including creating purchase orders, assigning them to shipments, and maintaining related data.
1. The admin website - to perform administrative tasks, including maintaining user access, and maintaining suppliers and their available products.
1. An api - used by both the main and admin websites to execute business logic and interact with the database.
1. An identity server - to verify login credentials and create session cookies.
1. A reverse proxy - to access the websites and api through a single domain and port.
1. A database - to persist data.

The core technologies used are:

1. Main website - Angular 9 frontend and ASP.NET 6 backend
1. Admin website - React 16 frontend and ASP.NET 6 backend
1. Web Api - ASP.NET 6
1. Identity Server - ASP.NET 6 using ASP.NET Identity for identity features
1. Reverse proxy - Envoy Proxy 1.14 in the local dev environment and Azure App Gateway in the Azure test environment
1. Database - Microsoft SQL Server 2019 in the local dev environment and Azure SQL Databsae in the Azure test environment

Additional design documentation is available in the [Wiki](https://github.com/kierendixon/purchase-order-tracker-aspnetcore_angular/wiki/Design).

## Environments

Three runtime environments are supported:

1. Microsoft Azure (refer to [dev/azure](dev/azure) folder)
1. Local using Docker (refer to [Quick Start](#quick-start))
1. Local development (refer to [Developmer Environment Setup](#developer-environment-setup))

## Continuous Integration

Source code is built using [Azure DevOps Pipelines](https://dev.azure.com/purchase-order-tracker/Purchase%20Order%20Tracker%20-%20Angular/_build). Pipeline definitions are stored in the [dev/azure-devops](dev/azure-devops) folder.

| Build         | Status |
| ------------- | --- |
| Source code   | [![Build Status](https://dev.azure.com/purchase-order-tracker/Purchase%20Order%20Tracker%20-%20Angular/_apis/build/status/Purchase%20Order%20Tracker)](https://dev.azure.com/purchase-order-tracker/Purchase%20Order%20Tracker%20-%20Angular/_build/latest?definitionId=4)                       |
| Docker images | [![Build Status](https://dev.azure.com/purchase-order-tracker/Purchase%20Order%20Tracker%20-%20Angular/_apis/build/status/Purchase%20Order%20Tracker%20-%20Docker%20Images)](https://dev.azure.com/purchase-order-tracker/Purchase%20Order%20Tracker%20-%20Angular/_build/latest?definitionId=6) |

## Continuous Deployment

The continuous integration pipeline will deploy *master* branch builds to the Azure test environment if the environment exists. Typically the environment will not exist to avoid costs.

To setup continuous deployments:

1. In Azure:
   - Create the deployment environment using the [all-resources.json](dev/azure) Azure Resource Manager (ARM) template
1. In Azure DevOps:
   - Use the Virtual Machine Scale Set (VMSS) created by `all-resources.json` to [create an agent pool](https://docs.microsoft.com/en-us/azure/devops/pipelines/agents/scale-set-agents?view=azure-devops) named _azure-vmss_. This is required to deploy to App Services configured with [private endpoints](https://docs.microsoft.com/en-us/azure/app-service/networking/private-endpoint)
   - Update the _azure-resource-names_ variable group in [Azure Pipelines](https://dev.azure.com/purchase-order-tracker/Purchase%20Order%20Tracker%20-%20Angular/_library?itemType=VariableGroups) with the name of the Azure App Services that were created by `all-resources.json`
   - Execute a new build

**Troubleshooting**

- When creating a DevOps agent pool, if you encounter the error message "this virtual machine scale set is already in use by pool azure-vmss", logon to Azure Portal and remove the `_AzureDevOpsElasticPool` tag from the VMSS.

# Developer Environment Setup

Install the following software to setup your developer environment.

1. Visual Studio IDE 2022 (v17.2 or higher)
1. Visual Studio IDE extensions:
   1. [Output Enhancer](https://marketplace.visualstudio.com/items?itemName=NikolayBalakin.Outputenhancer)
1. [.NET SDK 6.0.301](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) (which is part of .NET 6.0.6)
1. Visual Studio Code (v1.61 or higher)
1. VS Code extensions:
   1. [Prettier](https://marketplace.visualstudio.com/items?itemName=esbenp.prettier-vscode)
   1. [stylelint](https://marketplace.visualstudio.com/items?itemName=shinnn.stylelint)
   1. [TSlint](https://marketplace.visualstudio.com/items?itemName=ms-vscode.vscode-typescript-tslint-plugin)
   1. [Azure Resource Manager Tools](https://marketplace.visualstudio.com/items?itemName=msazurermtools.azurerm-vscode-tools)
1. Chrome extensions:
   1. [Angular DevTools](https://chrome.google.com/webstore/detail/angular-devtools/ienfalfjdbdpebioblfackkekamfmbnh?hl=en)
   1. [React DevTools](https://chrome.google.com/webstore/detail/react-developer-tools/fmkadmapgofadopljbjfkapdkoienihi?hl=en)
   1. [Redux DevTools](https://chrome.google.com/webstore/detail/redux-devtools/lmhkpmbekcpmknklioeibfkpmmfibljd?hl=en)
1. NodeJs (v16.14.2)
1. Docker Desktop
1. SQL Server Management Studio (v17 or higher)

Additionally:
1. Configure Visual Studio IDE  
   - Tools -> Options -> Text Editor -> Code Cleanup -> select "Run Code Cleanup profile on Save"
   - Tools -> Options -> Text Editor -> C# -> Advanced -> for "run background code analysis for" select "Entire solution"
1. Switch Docker to use Linux containers

## Debugging

To debug applications locally, start an instance of SQL Server and Envoy Proxy, then start the ASP.Net applications in Visual Studio IDE and the frontend SPAs using npm.

The following ports must be free:

- 1434 - SQL Server
- 4200 - Main website (Angular) backend
- 4201 - Main website (Angular) frontend
- 4202 - WebApi
- 4203 - Admin website backend
- 4204 - Admin website frontend
- 4891 - Envoy Proxy
- 5111 - Identity Server

Once everything is started, access the application via Envoy Proxy at http://localhost:4891

Additional instructions are below.

### SQL Server

Use Docker to create a SQL Server container:  
`docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=PoTracker001" -p 1434:1433 -d mcr.microsoft.com/mssql/server:2019-CU8-ubuntu-16.04`

SQL Server is used by both PurchaseOrderTracker.WebApi and PurchaseOrderTracker.Identity. If it's the first time starting the SQL Server container, application code will create necessary databases, database objects, and populate tables with sample data.

If you use a different SQL Server port or want to use a locally running instance of SQL Server, override `ConnectionStrings:IdentityDatabase` and `ConnectionStrings:PoTrackerDatabase` configuration values by setting environment variables in the relevant project's debug settings.

### Envoy Proxy

Use Docker to create an Envoy Proxy container:  
`docker run --rm -p 4891:80 -v ${PWD}:/etc/envoy envoyproxy/envoy:v1.14.4 envoy -c /etc/envoy/envoy.debug.yaml`

### ASP.NET Applications

Open Visual Studio IDE and press F5. Multiple startup projects will be selected by default which will start the following applications:

1. PurchaseOrderTracker.WebApi
1. PurchaseOrderTracker.WebUI.Admin
1. PurchaseOrderTracker.WebUI.Angular
1. PurchaseOrderTracker.Identity

### Frontend SPAs

Open each of the websites in Visual Studio Code, then run `npm start` to start serving them.

The SPAs are located at:

1. PurchaseOrderTracker.WebUI.Angular/ClientApp
1. PurchaseOrderTracker.WebUI.Admin/ClientApp
