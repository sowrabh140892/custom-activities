## AzureCreateVMDisk - Activity to create a Data Disk.

Remark - The portal needs to be configured first. https://portal.azure.com

##### DLL's to reference
Microsoft.Azure.Management.Compute.Fluent.dll
Microsoft.Azure.Management.Fluent.dll
Microsoft.Azure.Management.ResourceManager.Fluent.dll
Microsoft.Azure.Management.Network.Fluent.dll
Microsoft.Azure.Management.Sql.Fluent.dll
Microsoft.Rest.ClientRuntime.dll
Microsoft.Rest.ClientRuntime.Azure.dll
System.Net.Http.dll
Newtonsoft.Json.dll

##### Libraries to import
using Microsoft.Azure.Management.Compute.Fluent;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core


You'd need to store the API specific information from the portal.

ApplicationId
TenantId
Secret

### Mandatory fields when creating a VM disk:

**subscriptionId**		- The azure portal subscription Id (Free Trial/Premium)

**resourceGroupName**	- Resource Group where to create the disk. So this disk can be attached to a VM inside the same group.

**diskName**			- The name of the new diskName

**sizeGB**				- Size in GB for the new disk
