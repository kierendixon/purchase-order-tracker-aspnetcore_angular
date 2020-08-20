[![Build status](https://dev.azure.com/purchase-order-tracker/Purchase%20Order%20Tracker%20-%20Angular/_apis/build/status/Purchase%20Order%20Tracker%20-%20Angular%20.NET%20Core)](https://dev.azure.com/purchase-order-tracker/Purchase%20Order%20Tracker%20-%20Angular/_build/latest?definitionId=2)
<a href="https://sonarcloud.io/dashboard?id=purchase-order-tracker-aspnetcore_angular"><img src="https://sonarcloud.io/images/project_badges/sonarcloud-white.svg" height="24" width="102" ></a>

# Purchase Order Tracker (ASP.NET Core + Angular)

Purchase Order Tracker is a line-of-business system used to track the delivery of purchase orders.

You can maintain **Suppliers** and their **Products**, create **Purchase Orders** with **Line Items**, and assign orders to **Shipments**.

# Purpose

The purpose of building this system is to learn more about designing software and to learn about the technologies used.

# Documentation

Application user guides and technical design documentation are available in the [Wiki](https://github.com/kierendixon/purchase-order-tracker-aspnetcore_angular/wiki).

# Running the App using Docker

To run the app using Docker:

1. Run `docker build --tag purchase-order-tracker .` to build an image containing the website and webapi
1. Run `docker-compose up` to start the environment

Once the containers are started you can access the application at http://localhost:4202

# Technology Stack

This project is built using ASP.NET Core MVC v2 and Angular. Some of the key libraries used are listed below - refer to each of the csproj and package.json files for a full list of dependencies.

C# libraries:

- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [Mediator](https://github.com/jbogard/MediatR)
- [AutoMapper](https://github.com/AutoMapper/AutoMapper)
- [Feature Folders](https://github.com/OdeToCode/AddFeatureFolders)
- [Stateless](https://github.com/dotnet-state-machine/stateless)
- [Swagger](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)

Javascript/CSS libraries:

- [Angular](https://angular.io/)
- [Angular CLI](https://angular.io/cli)
- [Bootstrap](https://getbootstrap.com/)
- [NG Bootstrap](https://ng-bootstrap.github.io)
- [Font Awesome](http://fontawesome.io)
- [lodash](https://lodash.com)

Testing libraries:

- [NUnit3](https://github.com/nunit/docs/wiki)
- [Moq](https://github.com/moq/moq4)
- [Entity Framework Core InMemory Provider](https://docs.microsoft.com/en-us/ef/core/providers/in-memory/)
- [ASP.NET Core Test Host](https://www.nuget.org/packages/Microsoft.AspNetCore.TestHost)
- [Jasmine](https://jasmine.github.io/)
- [Karma](https://karma-runner.github.io)

# Solution Structure

The solution structure is influenced by the [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html) pattern and structured into the following components:

- **PurchaseOrderTracker.Domain** - enterprise business rules (rules that would exist even if this software system did not)
- **PurchaseOrderTracker.Application** - application business rules (rules that define or constrain the way that an automated system works, i.e., workflow logic)
- **PurchaseOrderTracker.WebUI.Angular** - the website user interface
- **PurchaseOrderTracker.WebApi** - an rpc-style web API the website uses
- **PurchaseOrderTracker.Persistence** - data persistence
- **PurchaseOrderTracker.Cache** - data caching

# Developer Environment Setup

1. Install Visual Studio IDE (2019 or higher)
1. Optionally, install these Visual Studio IDE extensions:
   1. [Project PowerTools](https://marketplace.visualstudio.com/items?itemName=ms-madsk.ProjectFileTools)
   1. [Double-Click Maximise](https://marketplace.visualstudio.com/items?itemName=VisualStudioPlatformTeam.Double-ClickMaximize)
   1. [Solution Error Visualizer](https://marketplace.visualstudio.com/items?itemName=VisualStudioPlatformTeam.SolutionErrorVisualizer)
   1. [Output Enhancer](https://marketplace.visualstudio.com/items?itemName=NikolayBalakin.Outputenhancer)
1. Install NodeJs (v12.18.3)
1. Install Visual Studio Code (v1.42 or higher)
1. Install these VS Code extensions:
   1. [Prettier](https://marketplace.visualstudio.com/items?itemName=esbenp.prettier-vscode)
   1. [stylelint](https://marketplace.visualstudio.com/items?itemName=shinnn.stylelint)
   1. [TSlint](https://marketplace.visualstudio.com/items?itemName=ms-vscode.vscode-typescript-tslint-plugin)
1. Install Docker and switch to Linux containers
1. Clone the repo

## SQL Server Setup

The **PurchaseOrderTracker.WebApi** project requires access to an instance of SQL Server 2017 CU3. You can either run an instance locally or startup a container and connect to it. If it doesn't already exist, the application will create the database schema and populate it with sample data.

To start a docker instance:

`docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=PoTracker001" -p 1433:1433 -d microsoft/mssql-server-windows-developer:2017-CU3`

## Running the App

To run the application in debug mode:

1. Start your SQL Server instance (see SQL Server Setup instructions)
2. Update the WebApi project's SQL connection string to point to your SQL Server instance
3. Start the app through Visual Studio. This will run both the WebUI (port 4200) and WebAPI (port 4202) projects
4. Serve the angular app by running `ng serve` from within the ClientApp folder of the WebUI project. This runs on port 4201.

You can then access the website at http://localhost:4200/
