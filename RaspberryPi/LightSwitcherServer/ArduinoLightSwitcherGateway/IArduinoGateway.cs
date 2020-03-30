using System;

namespace ArduinoLightswitcherGateway
{
    public interface IArduinoGateway : IDisposable
    {
        void Open();
        string Send(byte command);
        void Close();
    }
}