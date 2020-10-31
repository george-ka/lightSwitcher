# lightSwitcher
This project helps me to contol home lights.

[Demo](https://www.youtube.com/watch?v=fEmTeBbHTBU)

It consists of tree major parts:
1. Physical circuit board with relays
2. Arduino Mega board which controls the relays
3. Raspberry PI with a server to contol Arduino

Relays board description will come later.

You will find Arduino scetch in the [ArduinoLightSwitcher](./ArduinoLightSwitcher/ArduinoLightSwitcher.ino) directory

Arduino Mega controls the relays by setting LOW or HIGH signal to the corresponding pins.
Raspberry PI sends simple commands (bytes) via serial connection to Arduino. 
Arduino reads the commands, interprets them to turn on or off a particular switch.

Raspberry PI will connect to home wifi network and expose a web API via ASP.NET Core app on Kestrel Web server, 
which should be accessible from the local network by any device connected to it. 
It will also provide a simple web interface.

[See Raspberry PI setup](SetupRaspberry.md)

The lightswitcher is not accessible from the Internet, though one can expose a public IP trough his router and setup ports mapping. There is no security layer or any authentication other than the wifi network password.
Nevertheless, the light at home can be controlled from the Internet indirectly. 
There is a simple nodejs API for Yandex Station to process voice commands. Yandex can only send requests to publicly available Webhooks. So that nodejs API cor Yandex Station is currently deployed as a Google Cloud function, which then updates a command file in the Cloud Storage. 
On the Raspberry home server side there is a background service running, which is constantly pulling a file on Cloud Storage for the new command. I should probably have leveraged pub/sub, but for some reason I decided, that this storage version would be cheaper. Furthermore, there is only one instance of a listener. 
