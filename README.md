# lightSwitcher
This project is aimed to control home lights.

[Demo](https://www.youtube.com/watch?v=fEmTeBbHTBU)

It consists of tree major parts:
1. A custom-made circuit board with switches (based on Triacs). The board description can be found [here](SwitcherBoard.md).
2. Arduino Mega board which controls the switches. Find Arduino scetch in the [ArduinoLightSwitcher](./ArduinoLightSwitcher/ArduinoLightSwitcher.ino) file.
3. Raspberry PI with a server (a .NET Core app written on C#) to contol Arduino.

Arduino Mega controls the optocouplers  by setting LOW or HIGH signal to the corresponding pins.
Raspberry PI sends simple byte commands via serial connection to Arduino. 
Arduino reads the commands, interprets them to turn on or off a particular switch.

Raspberry PI is connected to home wifi network and exposes a web API and a single [html page](RaspberryPi/LightSwitcherServer/LightSwitcherWeb/wwwroot/index.html) with UI, 
which should be accessible from the local network by any device. 

[See Raspberry PI setup](SetupRaspberry.md)

The lightswitcher is not accessible from the Internet, though one can expose a public IP trough their router and setup ports mapping. 
There is no security layer or any authentication other than the wifi network password.

Nevertheless, the light at home can be controlled from the Internet indirectly. 
A service app can read commands from some resource by polling it periodically. Options are:
1. Pub/sub with some cloud based service (not implemented)
2. There is a simple nodejs API for Yandex Alice Station to process voice commands. Yandex can only send requests to publicly available Webhooks. So that nodejs API for Yandex Station can be deployed as a Google Cloud function, which then updates a command file in the Cloud Storage. CloudCommandReader reads the blob every second to get a command.
3. Alternatively, there is a [php API](AliceDialog/AliceDialogApiPhp/alice-api.php) for Alice station, which does the same, but can be deployed to a cheaper shared hosting service.

On the Raspberry PI server side there is a [background service app](RaspberryPi/CloudCommandsReader/Worker.cs) running, which constantly pulls a Cloud Storage file for the new command. Furthermore, there is only one instance of a listener. 
It turned out, that simple [php script](AliceDialog/AliceDialogApiPhp/get-command.php) running on a local virtual hosting provider is even cheaper.
There should be other simple options to publish a command online like telegram bot or something else.

Todo: make an authenticated dialog for Alice station.
