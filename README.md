# SFHumio

Azure Service Fabric is a distributed systems platform that makes it easy to package, deploy, and manage scalable and reliable microservices and containers. Developers and administrators can avoid complex infrastructure problems and focus on implementing mission-critical, demanding workloads that are scalable, reliable, and manageable.

Humio is a log management system, which enables you to Log everything, answer anything in real-time. You can send, search, and visualize all logs instantly, easily, and affordably, On-Premises or in the Cloud.

This repo contains source and guides on how to use Humio together with Service Fabric clusters and applications.

**_Disclaimer_**

Not a full solution, just a set of examples - refer to SF docs on how to monitor Service Fabric.

## Problem

Three layers...
Three phases...

### Instrumentation

- OS
  - Performance Counters
- Runtime
  - Syslog, ETW and performance counters
- Application
  - Examples - abundance of logging frameworks and providers

### Collect and ship

- ETW
- EventSource
- Files
- Performance Counters

### Query and analyze



## Solution

### Instrumentation - Solution

- OS
    - Performance Counters
- Runtime
    - ETW Providers
- Applications
    - .net core console: SeriLog --> Files
    - asp.net core: ILogger --> ILoggerProvider for EventSources

#### Serilog

[Serilog](https://serilog.net) is a popular logging framework for .NET. Serilog is focused on *structured* logging.
Doing structured logging means thinking about how the logs will be searched and analyzed afterwards. E.g. instrumenting
a program with:

```csharp
Log.Information("Number of user sessions {@UserSessions}", userSessionCount);;
```

Should make it easy to aggregate `UserSessions`in your log management system afterwards.
With serilog you can configure where and how your log lines get outputtet. The option we are going to 
choose is writing JSON formatted log lines to files. The above line would read as:

TODO: add json log line

With the following configuration in code:

```csharp
Log.Logger = new LoggerConfiguration()
                .WriteTo.File(new RenderedCompactJsonFormatter(), logPath, rollingInterval: RollingInterval.Day)
                .CreateLogger();
```

Writing logs to a file, instead of e.g. shipping it directly over the wire, has several important properties.
Your disks are by and large as available as the machine running your program. Compare this to an exernal system 
which will have a different lifecycle and might not be available when you need to transmit your log line. The file also acts as a persistent 
buffer between your program and your log management system which enables us to handle retransmissions and long
lasting bursts of log lines.


### Collect and ship - Solution

- ClusterMonitor
    - ETW
    - EventSource
    - Performance Counters
- FileBeat
    - Files

#### Filebeat

We have our logs formatted in JSON and written to files on disk, what's still missing is shipping the logs to Humio.
[Filebeat](https://docs.humio.com/integrations/data-shippers/beats/filebeat/) is a lightweight, cross platform shipper that is compatible with Humio.
Filebeat uses few resources, is easy to install and handles network problems gracefully.

The following filebeat configuration scrapes all logs from `D:\\SvcFab\_App` and subfolders named `log` with files ending in `.log`.
Humio is compatible with the elastic bulk API so we are using `output.elasticsearch`. The `hosts` parameter points to Humio cloud. 
The `INGEST_TOKEN` needs to be replaced by a valid Ingest Token. Ingest Tokens are used in Humio to identify clients, selecting a repository 
for the incoming logs and selecting which parser should be used. 

```yaml
filebeat.inputs:
- type: log
  enabled: true
  paths:
    - "D:\\SvcFab\\_App\\**\\log\\*.log"
  encoding: utf-8

output.elasticsearch:
  hosts: ["https://cloud.humio.com:443/api/v1/ingest/elastic-bulk"]
  username: "INGEST_TOKEN"
  compression_level: 5
  bulk_max_size: 200
  worker: 1
```

TODO: Note about Humio cloud vs. [On-Premises](https://www.humio.com/download).



### Query and analyze - Solution



## Repo Structure

All contain a README.md with instructions

- Applications
    - .net core console Guest
    - Reliable Service .net core webapi
- Agents
    - ClusterMonitor
    - FileBeat
- Humio
    - Configuration
    - Queries
    - Dashboards
- Scenarios
    - Upgrade
    - Failure (chaos)
    - Exception hunting
    - Performance