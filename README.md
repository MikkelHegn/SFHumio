# SFHumio

Azure Service Fabric is a distributed systems platform that makes it easy to package, deploy, and manage scalable and reliable microservices and containers. Developers and administrators can avoid complex infrastructure problems and focus on implementing mission-critical, demanding workloads that are scalable, reliable, and manageable.

Humio is a log management system, which enables you to Log everything, answer anything in real-time. You can send, search, and visualize all logs instantly, easily, and affordably, On-Premises or in the Cloud.

This repo contains source and guides on how to use Humio together with Service Fabric clusters and applications.

- Setup a Service Fabric cluster and Humio
  - Windows and ETW
  - Linux fluentd

- Setup application logging with .net core and Humio running on Service Fabric

- Throughput test

- Troubleshooting scenarios with Humio

## Setup cluster

We created a cluster using these guidelines: https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-tutorial-create-vnet-and-windows-cluster