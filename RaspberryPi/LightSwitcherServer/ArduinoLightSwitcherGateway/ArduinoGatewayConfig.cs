using System;

namespace ArduinoLightswitcherGateway
{
    public class ArduinoGatewayConfig
    {
        public ArduinoGatewayConfig(string serialPortNamePrefix, int serialPortBaudRate)
        {
            if (string.IsNullOrEmpty(serialPortNamePrefix))
            {
                throw new ArgumentException($"{nameof(serialPortNamePrefix)} can't be empty", nameof(serialPortNamePrefix));
            }

            if (serialPortBaudRate <= 0)
            {
                throw new ArgumentException($"{nameof(serialPortBaudRate)} must be positive", nameof(serialPortBaudRate));
            }

            SerialPortNamePrefix = serialPortNamePrefix;
            SerialPortBaudRate = serialPortBaudRate;
        }

        public string SerialPortNamePrefix { get; private set; }

        public int SerialPortBaudRate { get; private set; }
    }
}