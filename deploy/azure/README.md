# Azure Environment

Use `all-resources.json` Azure ARM template to create a deployment environment.

This template will create 33 resources, including:

- 1x App Service Plan
- 4x App Service
- 1x Application Gateway
- 1x Key Vault
- 7x Private Link (Private Endpoint)
- 7x Network Interface Card
- 1x Network Security Group
- 4x Private DNS Zone
- 1x Public IP Address
- 1x SQL Server
- 2x SQL Database
- 1x Storage Account
- 1x Virtual Machine Scale Set
- 1x Virtual Machine

[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fkierendixon%2Fpurchase-order-tracker-aspnetcore_angular%2Fmaster%2Fdeploy%2Fazure%2Fall-resources.json)

"Azure role assignments may take several minutes to propagate."
https://docs.microsoft.com/en-us/azure/storage/common/identity-library-acquire-token

"Azure role assignments may take up to 30 minutes to propagate."
https://docs.microsoft.com/en-us/azure/storage/blobs/authorize-access-azure-active-directory


"The back-end services for managed identities maintain a cache per resource URI for around 24 hours. If you update the access policy of a particular target resource and immediately retrieve a token for that resource, you may continue to get a cached token with outdated permissions until that token expires. There's currently no way to force a token refresh."
https://docs.microsoft.com/en-us/azure/app-service/overview-managed-identity

**It may take up to 30 minutes for Azure Active Directory role based access control permissions to take affect**

https://docs.microsoft.com/en-us/azure/app-service/overview-managed-identity?#configure-target-resource


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



https://github.com/Azure/bicep/issues/1013

https://github.com/Azure/bicep/issues/1013#issuecomment-1021412292
https://docs.microsoft.com/en-us/azure/role-based-access-control/troubleshooting#symptom---assigning-a-role-sometimes-fails-with-rest-api-or-arm-templates
https://docs.microsoft.com/en-us/azure/role-based-access-control/role-assignments-template#new-service-principal


You must wait up to 30 minues to RBAC roles to replicate before executing the next template
https://docs.microsoft.com/en-us/azure/role-based-access-control/troubleshooting#symptom---role-assignment-changes-are-not-being-detected


***Note: You must have creator and key vault data reader roles assigned
'Key Vault Administrator' role

(adding them as just a Key Vault Crypto Officer or Contributor is not enough, only Owner works)

ensure the user executing the arm template has Owner role assigned, otherwise they will receive "ForbiddenByRbac" error when creating the Key Vault key




# Troubleshooting

**This virtual machine scale set is already in use by pool azure-vmss**
When creating an agent pool in Azure DevOps, if you receive the above error, you will need to logon to Azure Portal and remove the "_AzureDevOpsElasticPool" tag from the VMSS.