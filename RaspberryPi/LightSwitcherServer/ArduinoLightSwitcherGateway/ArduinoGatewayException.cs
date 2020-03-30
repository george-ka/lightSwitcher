using System;
using System.Runtime.Serialization;

namespace ArduinoLightswitcherGateway
{
    [Serializable]
    public class ArduinoGatewayException : Exception
    {
        public ArduinoGatewayException()
        {
        }

        public ArduinoGatewayException(string message) : base(message)
        {
        }

        public ArduinoGatewayException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ArduinoGatewayException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}