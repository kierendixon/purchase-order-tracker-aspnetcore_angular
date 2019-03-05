[![Build status](https://ci.appveyor.com/api/projects/status/vxwmgkoefrwmwlct/branch/master?svg=true)](https://ci.appveyor.com/project/kierendixon/purchase-order-tracker-aspnetcore-angular/branch/master)

# Purchase Order Tracker (ASP.NET Core + Angular)

Purchase Order Tracker is a line-of-business system used to track the delivery of purchase orders.

You can maintain **Suppliers** and their **Products**, create **Purchase Orders** with **Line Items**, and assign orders to **Shipments**.

## Purpose

The purpose of building this system is to teach myself more about the technologies used, not to release it as a real product.

## Technology Stack

This project is built using ASP.NET Core MVC v2 and Angular. Some of the key libraries used are listed below - refer to each of the csproj and package.json files for a full list of dependencies.

C# libraries:

  * [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
  * [Mediator](https://github.com/jbogard/MediatR)
  * [AutoMapper](https://github.com/AutoMapper/AutoMapper)
  * [Feature Folders](https://github.com/OdeToCode/AddFeatureFolders)
  * [Stateless](https://github.com/dotnet-state-machine/stateless)

Javascript/CSS libraries:

  * [Angular](https://angular.io/)
  * [Angular CLI](https://angular.io/cli)
  * [Bootstrap](https://getbootstrap.com/)
  * [NG Bootstrap](https://ng-bootstrap.github.io)
  * [Webpack](https://webpack.js.org)
  * [Font Awesome](http://fontawesome.io)
  * [lodash](https://lodash.com)

Testing libraries:

  * [NUnit3](https://github.com/nunit/docs/wiki)
  * [Moq](https://github.com/moq/moq4)
  * [Entity Framework Core InMemory Provider](https://docs.microsoft.com/en-us/ef/core/providers/in-memory/)
  * [ASP.NET Core Test Host](https://www.nuget.org/packages/Microsoft.AspNetCore.TestHost)
  * [Jasmine](https://jasmine.github.io/)
  * [Karma](https://karma-runner.github.io)

# Development Environment Setup

  * Install SQL Server 2017 (CU3 or higher)
  * Install NodeJs (v10.15 or higher)
  * Install Visual Studio 2017 (v15.8.4 or higher)
  * Install Visual Studio Code (v1.31 or higher)
  * Install these VS Code extensions:
    * EditorConfig
    * stylelint
    * TSlint
  * Clone git repository

# Running in Docker for Windows

  * Install Docker for Windows (v18.09.0 or higher)
  * From within the root source folder run the command `docker-compose up`. This will download the base images, build a new Purchase Order Tracker image, and create and startup the containers.

# Documentation

See the [Wiki](https://github.com/kierendixon/purchase-order-tracker-aspnetcore_angular/wiki) for further documentation including user guides and design documents.