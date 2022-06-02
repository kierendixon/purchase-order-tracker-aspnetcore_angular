[![Build status](https://dev.azure.com/purchase-order-tracker/Purchase%20Order%20Tracker%20-%20Angular/_apis/build/status/Purchase%20Order%20Tracker%20-%20Angular%20.NET%20Core)](https://dev.azure.com/purchase-order-tracker/Purchase%20Order%20Tracker%20-%20Angular/_build/latest?definitionId=2)
<a href="https://sonarcloud.io/dashboard?id=purchase-order-tracker-aspnetcore_angular"><img src="https://sonarcloud.io/images/project_badges/sonarcloud-white.svg" height="24" width="102" ></a>

# Purchase Order Tracker

Purchase Order Tracker is a line-of-business system that tracks the delivery of purchase orders.

_This is a pet project for the purpose of learning about software design and development._

## User Guides

Application user guides are available in the [Wiki](https://github.com/kierendixon/purchase-order-tracker-aspnetcore_angular/wiki).

## Run Using Docker

A new instance of Purchase Order Tracker can be quickly and easily started using Docker.
You don't need to install anything other than Docker itself because multi-stage builds are used to compile the application within containers.

Execute the following commands:

1. `docker build --tag purchase-order-tracker-webapi -f src/PurchaseOrderTracker.WebApi/Dockerfile .`
1. `docker build --tag purchase-order-tracker-angular -f src/PurchaseOrderTracker.WebUI.Angular/Dockerfile .`
1. `docker build --tag purchase-order-tracker-admin -f src/PurchaseOrderTracker.WebUI.Admin/Dockerfile .`
1. `docker build --tag purchase-order-tracker-identity -f src/PurchaseOrderTracker.Identity/Dockerfile .`
1. `docker-compose up`

Once the containers are started you can access the application at http://localhost:4890

# Technical Details

The Purchase Order Tracker system is comprised of multiple components:

1. The main website - for day-to-day work, including creating purchase orders, assigning them to shipments, and maintaining related data.
1. The admin website - to perform administrative tasks, including maintaining user access, and maintaining suppliers and their available products.
1. An api - used by both the main and admin websites to execute business logic and interact with the database.
1. An identity server - to verify login credentials and create session cookies.
1. A reverse proxy - to access the websites and api through a single domain and port.
1. A database - to persist data.

The core technologies used include:

1. Main website - Angular (v9.0) frontend and ASP.NET Core (v5) backend
1. Admin website - React (v16.10) frontend and ASP.NET Core (v5) backend
1. Api - ASP.NET Core (v5)
1. Identity Server - ASP.NET Core (v5) using ASP.NET Core Identity for identity features
1. Reverse proxy - Envoy Proxy (v1.14)
1. Database - Microsoft SQL Server (v2019)

# Developer Environment Setup

1. Install Visual Studio IDE 2022 (v17.2 or higher)
1. Install Visual Studio IDE extensions:
   1. [Output Enhancer](https://marketplace.visualstudio.com/items?itemName=NikolayBalakin.Outputenhancer)
1. Configure Visual Studio IDE:
   1. Tools -> Options -> Text Editor -> Code Cleanup -> select "Run Code Cleanup profile on Save"
1. Install NodeJs (v16.14.2)
1. Install Visual Studio Code (v1.61 or higher)
1. Install VS Code extensions:
   1. [Prettier](https://marketplace.visualstudio.com/items?itemName=esbenp.prettier-vscode)
   1. [stylelint](https://marketplace.visualstudio.com/items?itemName=shinnn.stylelint)
   1. [TSlint](https://marketplace.visualstudio.com/items?itemName=ms-vscode.vscode-typescript-tslint-plugin)
1. Install Chrome extensions:
   1. [Angular DevTools](https://chrome.google.com/webstore/detail/angular-devtools/ienfalfjdbdpebioblfackkekamfmbnh?hl=en)
   1. [React DevTools](https://chrome.google.com/webstore/detail/react-developer-tools/fmkadmapgofadopljbjfkapdkoienihi?hl=en)
   1. [Redux DevTools](https://chrome.google.com/webstore/detail/redux-devtools/lmhkpmbekcpmknklioeibfkpmmfibljd?hl=en)
1. Install SQL Server Management Studio (v17 or higher)

## Debugging

To debug applications locally, you must first start an instance of SQL Server and Envoy Proxy, then start the ASP.Net applications in Visual Studio IDE, then the frontend SPAs using npm.

The following ports must be free:

- 1434 - SQL Server
- 4200 - Main website (Angular) backend
- 4201 - Main website (Angular) frontend
- 4202 - WebApi
- 4203 - Admin website backend
- 4204 - Admin website frontend
- 4891 - Envoy Proxy
- 5111 - Identity Server

Once everything is started up access the application via Envoy Proxy at http://localhost:4891

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
