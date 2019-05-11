[![Build status](https://dev.azure.com/purchase-order-tracker/Purchase%20Order%20Tracker%20-%20Angular/_apis/build/status/Purchase%20Order%20Tracker%20-%20Angular%20.NET%20Core)](https://dev.azure.com/purchase-order-tracker/Purchase%20Order%20Tracker%20-%20Angular/_build/latest?definitionId=2)
<a href="https://sonarcloud.io/dashboard?id=purchase-order-tracker-aspnetcore_angular"><img src="https://sonarcloud.io/images/project_badges/sonarcloud-white.svg" height="24" width="102" ></a>

# Purchase Order Tracker (ASP.NET Core + Angular)

Purchase Order Tracker is a line-of-business CRUD (create/read/update/delete) system used to track the delivery of purchase orders.

You can maintain **Suppliers** and their **Products**, create **Purchase Orders** with **Line Items**, and assign orders to **Shipments**.

# Purpose

The purpose of building this system is to learn more about designing software and to learn about the technologies used.

# Documentation

Application user guides and technical design documentation is available in this project's [Wiki](https://github.com/kierendixon/purchase-order-tracker-aspnetcore_angular/wiki).

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

This solution follows a [Clean Architecture](https://www.amazon.com/dp/0134494164) style. To support this, the codebase is structured into the following components:

- **PurchaseOrderTracker.Domain** - enterprise business rules (rules that would exist even if this software system did not)
- **PurchaseOrderTracker.Application** - application business rules (rules that define or constrain the way that an automated system works, i.e., workflow logic)
- **PurchaseOrderTracker.WebUI.Angular** - a website user interface
- **PurchaseOrderTracker.WebApi** - an rpc-style web API
- **PurchaseOrderTracker.Persistence** - persistence related infrastructure details
- **PurchaseOrderTracker.Cache** - cache related infrastructure details

# Development Environment Setup

- Install SQL Server 2017 (CU3 or higher)
- Install NodeJs (v10.15 or higher)
- Install Visual Studio 2017 (v15.8.4 or higher)
- Optionally, install these Visual Studio IDE extensions:
  - [Project PowerTools](https://marketplace.visualstudio.com/items?itemName=ms-madsk.ProjectFileTools)
  - [Double-Click Maximise](https://marketplace.visualstudio.com/items?itemName=VisualStudioPlatformTeam.Double-ClickMaximize)
  - [Solution Error Visualizer](https://marketplace.visualstudio.com/items?itemName=VisualStudioPlatformTeam.SolutionErrorVisualizer)
  - [Output Enhancer](https://marketplace.visualstudio.com/items?itemName=NikolayBalakin.Outputenhancer)
- Install Visual Studio Code (v1.31 or higher)
- Install these VS Code extensions:
  - [Prettier](https://marketplace.visualstudio.com/items?itemName=esbenp.prettier-vscode)
  - [stylelint](https://marketplace.visualstudio.com/items?itemName=shinnn.stylelint)
  - [TSlint](https://marketplace.visualstudio.com/items?itemName=ms-vscode.vscode-typescript-tslint-plugin)
- Clone git repository
