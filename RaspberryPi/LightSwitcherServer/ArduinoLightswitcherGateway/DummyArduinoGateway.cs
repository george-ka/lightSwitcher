using System;
using System.Linq;
using Serilog;

namespace ArduinoLightswitcherGateway
{
    public class DummyArduinoGateway : IArduinoGateway
    {
        public DummyArduinoGateway(ArduinoGatewayConfig gatewayConfig)
        {
            if (gatewayConfig == null)
            {
                throw new ArgumentException("Config can't be null", nameof(gatewayConfig));
            }

            _arduinoGatewayConfig = gatewayConfig;
        }

        public void Open()
        {
            _logger.Information("Opening Fake Arduino Gateway.");
        }

        public string Send(byte command)
        {
            _logger.Information("Sending a fake command {command}", command);
            return "Dummy response";            
        }

        public void Close()
        {
            _logger.Information("Closing Fake Arduino Gateway.");
        }

        public void Dispose()
        {
            _logger.Information("Disposing of Fake Arduino Gateway.");
        }

        private readonly ArduinoGatewayConfig _arduinoGatewayConfig;
        private readonly ILogger _logger = Log.ForContext<DummyArduinoGateway>();
    }
}