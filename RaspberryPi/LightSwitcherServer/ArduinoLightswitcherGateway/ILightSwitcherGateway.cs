using System;

namespace ArduinoLightswitcherGateway
{
    public interface ILightSwitcherGateway : IDisposable
    {
        bool ChangeSwitchState(byte switchId, bool isOn);

        void SwitchAllOn();

        void SwitchAllOff();

        SwitchState[] GetStatus();
    }
}