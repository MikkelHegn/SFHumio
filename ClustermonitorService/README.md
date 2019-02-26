# Cluster Monitor Service

This service is used to collect platform events from Service Fabric. The service wraps the EventFlow library in a Windows Service to listen to, and forward Event Traces emitted by Service Fabric.

The ClusterMonitor source can be found here: https://github.com/dkkapur/service-fabric-monitoring-eventflow/tree/master/windows-service/ClusterMonitor/ClusterMonitor

## Install Cluster Monitor in a Service Fabric Windows cluster

1. Upload the eventFlow.json configuration file to a location, accessible through https

1. Change the 'clusterMonitor_artifactsLocation' parameter to the location of the eventFlowConfig.json file

1. Change the forceUpdateTag in the extension to a different string (e.g. use as version number)

1. Update the scale set model (or rerun SF cluster deployment as an incremental deployment)