# lightSwitcher
This project is amed to described home lights control system.

[Demo](https://www.youtube.com/watch?v=fEmTeBbHTBU)

It consists of tree major parts:
1. A custom-made circuit board with switchers (based on Triacs). The board description can be found [here](SwitcherBoard.md).
2. Arduino Mega board which controls the switchers. Find Arduino scetch in the [ArduinoLightSwitcher](./ArduinoLightSwitcher/ArduinoLightSwitcher.ino) file.
3. Raspberry PI with a server (a .NET Core app written on C#) to contol Arduino

Arduino Mega controls the optocouplers  by setting LOW or HIGH signal to the corresponding pins.
Raspberry PI sends simple commands (bytes) via serial connection to Arduino. 
Arduino reads the commands, interprets them to turn on or off a particular switch.

Raspberry PI id connected to home wifi network and exposes a web API and a single [html page](RaspberryPi/LightSwitcherServer/LightSwitcherWeb/wwwroot/index.html), 
which should be accessible from the local network by any device. 

[See Raspberry PI setup](SetupRaspberry.md)

The lightswitcher is not accessible from the Internet, though one can expose a public IP trough their router and setup ports mapping. 
There is no security layer or any authentication other than the wifi network password.

Nevertheless, the light at home can be controlled from the Internet indirectly. 
There is a simple nodejs API for Yandex Station to process voice commands. Yandex can only send requests to publicly available Webhooks. So that nodejs API cor Yandex Station is currently deployed as a Google Cloud function, which then updates a command file in the Cloud Storage. 
Alternatively, there is a [php API](AliceDialog/AliceDialogApiPhp/alice-api.php) for Alice station, which does the same. 

On the Raspberry PI server side there is a [background service](RaspberryPi/CloudCommandsReader/Worker.cs) running, which constantly pulls a file on Cloud Storage for the new command. I should probably have leveraged pub/sub, but for some reason I decided, that this storage version would be cheaper. Furthermore, there is only one instance of a listener. 
It turned out, that simple [php script](AliceDialog/AliceDialogApiPhp/get-command.php) running on a local virtual hosting provider is even cheaper, than Google Cloud, because reading a file from Cloud Storage is far from being free. 
