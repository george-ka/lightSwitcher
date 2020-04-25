# lightSwitcher
This project helps to contol home lights.

[Demo](https://www.youtube.com/watch?v=fEmTeBbHTBU)

It consists of tree major parts:
1. Physical circuit board with relays
2. Arduino Mega board which controls the relays
3. Raspberry PI with a server to contol Arduino

Relays board description will come later.

You will find Arduino scetch in the [ArduinoLightSwitcher](./ArduinoLightSwitcher/ArduinoLightSwitcher.ino) directory

Arduino Mega controls the relays by setting LOW or HIGH signal to its corresponding pins.
Raspberry PI sends simple commands (bytes) via serial connection to Arduino. 
Arduino reads the commands and interprets them as commands to turn on or off particular switch by setting 
a corresponding pin to LOW or HIGH.

Raspberry PI will connect to home wifi network and expose a web API via ASP.NET Core app on Kestrel Web server, 
which should be accessible from the local network to any device connected to the home network. 
It will also provide a simple web interface.

[See Raspberry PI setup](SetupRaspberry.md)
