# Cluster ARM templates

This template and parameter file, creates a five-node Service Fabrc cluster, and installs the ClusterMonitor service to emit Service Fabric platform events to Humio.

## Preparation

1. Add your Humio access token to the [eventFlowConfig.json](./../ClusterMonitorService/eventFlowConfig.json) file `"basicAuthenticationUserName": "<access_token>",`

**Note**
> To configure the log levels to collect from Service Fabric platform, you can provide level and keywords to the ETW provider configuration. Please refer to the Eventflow documentation here: https://github.com/Azure/diagnostics-eventflow#etw-event-tracing-for-windows

**Note**
> To learn more about Serviec Fabric platform events, refer to this: https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-diagnostics-event-aggregation-wad

1. Upload the following files to a location, accessible through https (e.g. an Azure Storage account)
    - [InstallClusterMonitor.ps1](./../ClusterMonitorService/InstallClusterMonitor.ps1)
    - [ClusterMonitor.zip](./../ClusterMonitorService/ClusterMonitor.zip)
    - [eventFlowConfig.json](./../ClusterMonitorService/eventFlowConfig.json)

1. Change the 'clusterMonitor_artifactsLocation' parameter in the ARM template [parameter file](./../ClusterARM/parameters.json) file to that location

## Installation
The installation insttructions are using PowerShell.

1. Log-in to Azure `Connect-AzurermAccount`

1. Create a new resourece group `New-AzureRmResourceGroup -Name <some_name> -Location <some_location>`

1. Start the deployment `New-AzureRmResourceGroupDeployment -ResourceGroupName <some_name> -TemplateParameterFile parameters.json -TemplateFile template.json` - you will be prompted for the admin password for the VMSS nodes. The username is defined in the template.

1. Once the deployment completes the cluster is setup and emitting platform evetns to Humio