using ArduinoLightswitcherGateway;

namespace ArduinoLightswitcherGateway
{
    public static class LightSwitcherGatewayFactory
    {
        public static ILightSwitcherGateway CreateLightswitcherGateway()
        {
            return new LightSwitcherGateway(
                new ArduinoGateway(
                    new ArduinoGatewayConfig("ttyACM", 9600)),
                new LightSwitcherConfig(48, 97, 14));
        }
    }
}