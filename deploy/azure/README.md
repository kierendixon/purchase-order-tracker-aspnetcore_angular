# Azure Environment

Use `all-resources.json` Azure ARM template to create a deployment environment.

It will create 38 resources, including:

1. Virtual Network with multiple subnets
1. Network Security Group
1. Application Gateway
1. Public IP
1. Private Endpoints
1. Private DNS Zones
1. App Service Plan with multiple App Services
1. SQL Server with multiple databases
1. Virtual Machine Scale Set

[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fkierendixon%2Fpurchase-order-tracker-aspnetcore_angular%2Fmaster%2Fdeploy%2Fazure%2Fall-resources.json)

The deployment environment will look like the following:

![Azure Environment](Azure%20Environment.png?raw=true)  
*Diagram created with https://online.visual-paradigm.com/diagrams/features/azure-architecture-diagram-tool/*

## Useful ARM documentation and tool links

1. [ARM templates documentation](https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates)
1. [ARM templates functions](https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions)
1. [ARM templates reference](https://docs.microsoft.com/en-us/azure/templates)
1. [Azure naming conventions](https://docs.microsoft.com/en-us/azure/cloud-adoption-framework/ready/azure-best-practices/resource-naming)
1. [ARM test toolkit](https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/test-toolkit)
1. [Azure Resource Explorer](https://resources.azure.com/)
1. [App Service team blog](https://azure.github.io/AppService)
