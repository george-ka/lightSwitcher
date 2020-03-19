
#!/bin/bash

path=$(pwd)
aduino-cli compile -b "arduino:avr:mega" -p /dev/ttyACM0 -u $path
