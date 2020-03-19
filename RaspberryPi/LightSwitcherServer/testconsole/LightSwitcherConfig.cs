using System;

namespace ArduinoLightswitcherGateway
{
    public class LightSwitcherConfig
    {
        public LightSwitcherConfig(int turnOnOffset, int turnOffOffset, int numberOfSwitches)
        {
            TurnOnOffset = turnOnOffset;
            TurnOffOffset = turnOffOffset;
            NumberOfSwitches = numberOfSwitches;
        }

        public int TurnOnOffset { get; private set; }

        public int TurnOffOffset { get; private set; }

        public int NumberOfSwitches { get; private set; }
    }
}