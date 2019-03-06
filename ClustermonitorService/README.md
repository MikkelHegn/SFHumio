# Cluster Monitor Service

This service is used to collect platform events from Service Fabric. The service wraps the .Net EventFlow diagnostics library in a Windows Service to listen to, and forward Event Traces emitted by Service Fabric.

The ClusterMonitor source can be found here: https://github.com/dkkapur/service-fabric-monitoring-eventflow/tree/master/windows-service/ClusterMonitor/ClusterMonitor

We choose to run this aas a Windows Service, as it will monitor the Service Fabric platform. The Windows service can be installed using Extensions to the VMSS model.

## Install or Update Cluster Monitor

The following procedure describes how to install or update the ClusterMonitor service, using the VMSS Custom Scripts Extension.

1. Upload the following files to a location, accessible through https
    - InstallClusterMonitor.ps1
    - ClusterMonitor.zip
    - eventFlowConfig.1.json

1. Change the 'clusterMonitor_artifactsLocation' parameter in the ARM template [parameter file](./../ClusterARM/parameters.json) file to that location. (If you are setting up a new cluster, refer to [these instrcutions](./../ClusterARM/README.md))

1. Change the forceUpdateTag in the extension to a different string (e.g. use as version number)

1. Update the scale set model (or re-run SF cluster deployment as an incremental deployment)