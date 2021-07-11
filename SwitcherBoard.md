# Switcher board

This document describes the board of switchers.
The board is based on triacs and optoisolated triac drivers.

Below is the list of elements needed to assemble one switcher:
1. MOC3041 - a 6-pin DIP Zero-cross optoisolated Triac driver
2. BT134-600D or BT134-600E - Triac
3. 360Ω Ohm 1 Watt resistor
4. 330Ω Ohm 1 Watt resistor 
5. 39Ω Ohm 1 Watt resistor
6. 0.01mF capacitor

Each board consists of 8 switchers, there are two boards.

The switcher circuit is based on MOC3041 datasheet sample:

    Vcc          1 _________ 6     __                         HOT
    +○--- Rin-----|         |-----|__|360Ω--●------●-----------○
                 2|         |5              |      |          220V
    -○------------| MOC3041 |-○            (T2)   | |39Ω
                 3|         |4            [TRIAC] |_|   
                ○-|_________|-------●-GATE-/ |     |
                                    |       (T1)   |
                                    ┴        |    ___
                                   | |330Ω   |    ___0.01mF
                                   |_|       |     |           N
                                    |________●_____●--|LOAD|---○
                                     
## Tips
Remove the capacitor and 39Ohm resistor if the light bulb is luminescent or starts binking.
Rin is calculated so that current is equal to 15mA (0.015A).

Given that Arduino voltage is 5V, the Rin resistor should be around (5/0,015) = 333.3 Ohm.

1st MOC3041 optocoupler leg is connected to Arduino 5V pin, the second leg is connected to one of the programmed pins see [pinout](ArduinoAndLightControlBoardsPinout.txt). 
In 'off' state programmed pin voltage is high (5v) so there is no voltage difference between the legs and optocoupler is closed. 
In 'on' state the voltage is low, which creates a 5v voltage between the 1 and 2 legs of the optocoupler, so it opens and engages the Triac. 
See changeSwitchState funciton in [Arduino programm](ArduinoLightSwitcher/ArduinoLightSwitcher.ino).
