# Switcher board

This document described the board of switchers.
The board is based on triacs and optoisolator triac drivers.

The list of elements needed for one switcher is the following:
1. MOC3041 - a 6-pin DIP Zero-cross optoisolated Triac driver
2. BT134-600D or BT134-600E - Triac
3. 360Ω Ohm 1 Watt resistor
4. 330Ω Ohm 1 Watt resistor 
5. 39Ω Ohm 1 Watt resistor
6. 0.01F capacitor

Each board consists of 8 switchers, there are two boards.

The switcher circuit is based on MOC3041 datasheet sample

    Vcc           1 --------- 6     __                         HOT
     ○--- Rin -----|         |-----|__|360Ω--⬤------⬤----------○
                  2|         |5              |      |
             ○-----| MOC3041 |-○            (T2)   | |39Ω
                  3|         |4            [TRIAC] |_|   
                ○--|         |-------⬤-GATE-/ |     |
                    ---------        |      (T1)    |
                                     _        |    ___
                                    | |330Ω   |    ___0.01F
                                    |_|       |     |           N
                                     |________⬤_____⬤--|LOAD|--○
                                     
Remove the capacitor and 39Ohm resistor if the light bulb is luminescent or starts binking.
