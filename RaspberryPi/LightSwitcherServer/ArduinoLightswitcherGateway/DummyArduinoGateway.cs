using System;
using System.Text;
using Serilog;

namespace ArduinoLightswitcherGateway
{
    ///
    /// This class is for debugging purposes only
    /// it mimics the real Arduino board
    /// 
    public class DummyArduinoGateway : IArduinoGateway
    {
        public void Open()
        {
            _logger.Information("Opening Fake Arduino Gateway.");
        }

        public string Send(byte command)
        {
            _logger.Information("Sending a fake command {command}", command);
            var pins = new[] { 23, 25, 27, 29, 31, 33, 35, 37, 39, 41, 43, 45, 47, 49, 51, 53 };
            
            switch (command)
            {
                case SHOW_STATE_COMMAND:
                    var stringBuilder = new StringBuilder("State:");
                    int index;
                    var rand = new Random();
                    for (var i = 0; i < NUMBER_OF_SWITCHES; i++)
                    {
                        stringBuilder.Append($"{i}={rand.Next(0, 1)} ");
                    }

                    return stringBuilder.ToString();

                case TURN_ALL_ON_COMMAND:
                    return "Turned all on";
              
                case TURN_ALL_OFF_COMMAND:
                    return "Turned all off";
                
                case var cmd when command >= TURN_ON_OFFSET && cmd < TURN_ON_OFFSET + NUMBER_OF_SWITCHES:
                    index = command - TURN_ON_OFFSET;
                    return $"Turn ON pin: {pins[index]} switch: {index}";

                case var cmd when cmd >= TURN_OFF_OFFSET && cmd < TURN_OFF_OFFSET + NUMBER_OF_SWITCHES:
                    index = command - TURN_OFF_OFFSET;
                    return $"Turn OFF pin: {pins[index]} switch: {index}";

                default:
                    return "Unrecognized command";
            }       
        }

        public void Close()
        {
            _logger.Information("Closing Fake Arduino Gateway.");
        }

        public void Dispose()
        {
            _logger.Information("Disposing of Fake Arduino Gateway.");
        }

        private const byte NUMBER_OF_SWITCHES = 16;
        private const byte TURN_ON_OFFSET = 48;
        private const byte TURN_OFF_OFFSET = 97;
        private const byte SHOW_STATE_COMMAND = 5;
        private const byte TURN_ALL_OFF_COMMAND = 6;
        private const byte TURN_ALL_ON_COMMAND = 7;
        
        private readonly ILogger _logger = Log.ForContext<DummyArduinoGateway>();
    }
}