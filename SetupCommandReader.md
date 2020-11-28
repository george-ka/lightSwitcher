
# Setting up Command Reader on Raspberry PI

## Prerequisites
Install the lightswitcher server first 
[See Raspberry PI setup](SetupRaspberry.md)

The CommandReader checks a blob located on Google Cloud Storage every second and reads a byte command from it. 
It has a mapping in the setting file between input command byte and output range of switchers that should be turned on or off.

[See Google Cloud Setup](AliceDialog/GoogleCoudInitializationCommands.md) to install a function, which implements a [dialog](AliceDialog/AliceDialogApi/index.js) with Alice home station and writes a byte command into a blob.

.NET Core should has already been installed on Raspberry.

## Installing the Command Reader on Raspberry

Publish the app first

```bash
# lightswitcher source has been git-cloned before
cd ~/lightswitcher-source/lightSwitcher/RaspberryPi/CloudCommandsReader/
dotnet publish -c Release

mkdir ~/lightswitcher/command-reader-service
rsync -ar bin/Release/netcoreapp3.1/publish/* ~/lightswitcher/command-reader-service

# Create appsettings.Production.json and specify Google Cloud Storage Key file path
cd ~/lightswitcher/command-reader-service
cat appsettings.Development.json > appsettings.Production.json
nano appsettings.Production.json

# Change CloudCommandsReaderSettings.KeyFilePath to a key file location. For instance "/home/pi/lightswitcher/gcloud_service_account_key.json"

```

Copy key file from your local machine
```bash
scp gcloud_service_account_key.json pi@xxx.xxx.x.x:/home/pi/lightswitcher
```

Make dotnet process auto-restart, create a systemd service.

```bash
sudo nano /etc/systemd/system/lightswitcher-commandreader.service
```

With the following content:

```
[Unit]
Description=ASP.NET Core 3.0 App - LightSwitcher Command Reader Service

[Service]
# We can only use absolute path in systemd configuation
WorkingDirectory=/home/pi/lightswitcher/command-reader-service/
ExecStart=/home/pi/dotnet/dotnet/dotnet /home/pi/lightswitcher/command-reader-service/CloudCommandsReader.dll
Restart=always
# Restart service after 10 seconds if the dotnet service crashes:
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=lightswitcher-command-reader
User=pi
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

[Install] 
WantedBy=multi-user.target
```


Register and start the service:

```bash
sudo systemctl enable lightswitcher-commandreader.service
sudo systemctl start lightswitcher-commandreader.service
sudo systemctl status lightswitcher-commandreader.service

# to see service stdout
sudo journalctl -u lightswitcher-commandreader.service
```
