# Switcher board

This document described the board of switchers.
The board is based on triacs and optoisolated triac drivers.

The list of elements needed for one switcher is the following:
1. MOC3041 - a 6-pin DIP Zero-cross optoisolated Triac driver
2. BT134-600D or BT134-600E - Triac
3. 360Ω Ohm 1 Watt resistor
4. 330Ω Ohm 1 Watt resistor 
5. 39Ω Ohm 1 Watt resistor
6. 0.01mF capacitor

Each board consists of 8 switchers, there are two boards.

The switcher circuit is based on MOC3041 datasheet sample

    Vcc           1 --------- 6     __                         HOT
    +○--- Rin-----|         |-----|__|360Ω--⬤------⬤----------○
                 2|         |5              |      |          220V
    -○------------| MOC3041 |-○            (T2)   | |39Ω
                 3|         |4            [TRIAC] |_|   
                ○-|         |-------⬤-GATE-/ |     |
                   ---------        |       (T1)    |
                                     _        |    ___
                                    | |330Ω   |    ___0.01mF
                                    |_|       |     |           N
                                     |________⬤_____⬤--|LOAD|--○
                                     
Remove the capacitor and 39Ohm resistor if the light bulb is luminescent or starts binking.
Rin is calculated so that current is equal to 15mA (0.015A). 
Given that Arduino voltage is 5V, the Rin resistor should be around (5/0,015) = 333.3 Ohm.
Vcc + is connected to Arduino 5V pin, the other is connected to one of the programmed pins see [pinout](ArduinoAndLightControlBoardsPinout.txt). 
In 'off' state programmed pin voltage is high (5v). In 'on' state the voltage is low, so there is a 5v voltage between the 1 and 2 pins of the MOC3041 optocoupled triac driver. See changeSwitchState funciton in [Arduino programm](ArduinoLightSwitcher/ArduinoLightSwitcher.ino).

