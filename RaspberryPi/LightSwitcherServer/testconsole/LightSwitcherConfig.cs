using System;

namespace ArduinoLightswitcherGateway
{
    public class LightSwitcherConfig
    {
        public LightSwitcherConfig(
            int turnOnOffset, 
            int turnOffOffset, 
            int numberOfSwitches,
            byte showStateCommand = 5,
            byte switchAllOnCommand = 6,
            byte switchAllOffCommand = 7)
        {
            TurnOnOffset = turnOnOffset;
            TurnOffOffset = turnOffOffset;
            NumberOfSwitches = numberOfSwitches;
            ShowStateCommand = showStateCommand;
            SwitchAllOnCommand = switchAllOnCommand;
            SwitchAllOffCommand = switchAllOffCommand;
        }

        public int TurnOnOffset { get; }

        public int TurnOffOffset { get; }

        public int NumberOfSwitches { get; }

        public byte ShowStateCommand { get; }

        public byte SwitchAllOnCommand { get; }

        public byte SwitchAllOffCommand { get; }
    }
}