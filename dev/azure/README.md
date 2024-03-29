# Azure Test Environment

To create a test environment:

1. Create a new Azure resource group
1. In the resource group, assign 'Owner' role to the user that will run the ARM template (this is required to create keys in Azure Key Vault). Note that role assignments take up to 30 minutes to propagate.
1. Run the `all-resources.json` ARM template  
   [![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fkierendixon%2Fpurchase-order-tracker-aspnetcore_angular%2Fmaster%2Fdeploy%2Fazure%2Fall-resources.json)

The test environment will look like the following:
![Azure Environment](Azure%20Environment.png?raw=true)  
_Diagram created with https://online.visual-paradigm.com/diagrams/features/azure-architecture-diagram-tool/_

The ARM template will create 33 resources:

- 1x Virtual Network
- 1x Network Security Group
- 1x App Service Plan
- 4x App Service
- 1x Application Gateway
- 1x Key Vault
- 7x Private Link (Private Endpoint)
- 7x Network Interface Card
- 4x Private DNS Zone
- 1x Public IP Address
- 1x SQL Server
- 2x SQL Database
- 1x Storage Account
- 1x Virtual Machine Scale Set

## Troubleshooting

Refer to the following links to troubleshoot role-based access control (RBAC) issues.

- ["Azure role assignments may take several minutes to propagate"](https://docs.microsoft.com/en-us/azure/storage/common/identity-library-acquire-token)
- ["Azure role assignments may take up to 30 minutes to propagate."](https://docs.microsoft.com/en-us/azure/storage/blobs/authorize-access-azure-active-directory)
- ["When you assign roles or remove role assignments, it can take up to 30 minutes for changes to take effect"](https://docs.microsoft.com/en-us/azure/role-based-access-control/troubleshooting#symptom---role-assignment-changes-are-not-being-detected)
- ["If you create a new service principal and immediately try to assign a role to that service principal, that role assignment can fail in some cases"](https://docs.microsoft.com/en-us/azure/role-based-access-control/role-assignments-template#new-service-principal)
- [Wait and retry](https://github.com/Azure/bicep/issues/1013#issuecomment-1021412292)

## Useful links

1. [ARM templates documentation](https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates)
1. [ARM templates functions](https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions)
1. [ARM templates reference](https://docs.microsoft.com/en-us/azure/templates)
1. [Azure naming conventions](https://docs.microsoft.com/en-us/azure/cloud-adoption-framework/ready/azure-best-practices/resource-naming)
1. [App Service team blog](https://azure.github.io/AppService)
1. [ARM test toolkit](https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/test-toolkit)
