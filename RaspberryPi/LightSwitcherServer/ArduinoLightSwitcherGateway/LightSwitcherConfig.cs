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
            byte switchAllOffCommand = 6,
            byte switchAllOnCommand = 7)
        {
            TurnOnOffset = turnOnOffset;
            TurnOffOffset = turnOffOffset;
            NumberOfSwitches = numberOfSwitches;
            ShowStateCommand = showStateCommand;
            SwitchAllOffCommand = switchAllOffCommand;
            SwitchAllOnCommand = switchAllOnCommand;
        }

        public int TurnOnOffset { get; }

        public int TurnOffOffset { get; }

        public int NumberOfSwitches { get; }

        public byte ShowStateCommand { get; }

        public byte SwitchAllOffCommand { get; }
        
        public byte SwitchAllOnCommand { get; }
    }
}