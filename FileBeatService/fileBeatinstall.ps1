mkdir c:\filebeat
cd filebeat
curl -UseBasicParsing -OutFile filebeat.yml https://mikhegn.blob.core.windows.net/temp/filebeat.yml
curl -UseBasicParsing -OutFile filebeat.exe https://mikhegn.blob.core.windows.net/temp/filebeat.exe
curl -UseBasicParsing -OutFile install-service-filebeat.ps1 https://mikhegn.blob.core.windows.net/temp/install-service-filebeat.ps1
./install-service-filebeat.ps1
start-service filebeat
Get-Service filebeat


curl -UseBasicParsing -OutFile .\eventFlowConfig.json https://mikhegn.blob.core.windows.net/temp/eventFlowConfig.json
Restart-Service ClusterMonitor
Get-Service ClusterMonitor
type eventFlowConfig.json