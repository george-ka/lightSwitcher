# lightSwitcher
This project helps to contol home lights.

It contains tree major parts:
1. Physical circuit containing relays
2. Arduino Mega board which controls the relays
3. Raspberry PI with a server to contol Arduino

Arduino Mega controls the relays by setting LOW or HIGH signal to its corresponding pins.
Raspberry PI sends simple commands (bytes) via serial connection to Arduino. 
Arduino reads the commands and interprets them as commands to turn on or off particular switch by setting 
a corresponding pin to LOW or HIGH.

Raspberry PI will connect to home wifi network and expose a web API with Kestrel Web server, 
which should be accessible from the local network to any device connected to the home network. 
It will also provide a simple web interface.

# Setting up Raspberry PI

You will need to install .net core on raspberry 
https://edi.wang/post/2019/9/29/setup-net-core-30-runtime-and-sdk-on-raspberry-pi-4

You will also need to install arduino cli to be able to upload program to Arduino.