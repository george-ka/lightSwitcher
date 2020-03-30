using System;

namespace ArduinoLightswitcherGateway
{
    public interface ILightSwitcherGateway : IDisposable
    {
        string ChangeSwitchState(byte switchId, bool isOn);

        void SwitchAllOn();

        void SwitchAllOff();

        SwitchState[] GetStatus();
    }
}