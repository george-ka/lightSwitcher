using System;
using Serilog;
using ArduinoLightswitcherGateway;

namespace testconsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            // Get a list of serial port names.             
            
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            using (var arduinoGateway = new ArduinoGateway(
                new ArduinoGatewayConfig("ttyACM", 9600)))
            using (var lightswitcher = new LightSwitcherGateway(
                arduinoGateway,
                new LightSwitcherConfig(48, 97, 14)))
            {
                arduinoGateway.Send(0);

                var command = "";
                ShowUsage();
            
                while (true)
                {
                    command = Console.ReadLine();
                    
                    if (command == EXIT_COMMAND)
                    {
                        break;
                    }

                    if (command == SWITCH_ALL_ON_COMMAND)
                    {
                        lightswitcher.SwitchAllOn();
                        continue;
                    }

                    if (command == SWITCH_ALL_OFF_COMMAND)
                    {
                        lightswitcher.SwitchAllOff();
                        continue;
                    }

                    if (command == GET_STATE_COMMAND)
                    {
                        lightswitcher.GetStatus();
                        continue;
                    }

                    var parts = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    var turnOn = false;
                    if (parts.Length == 0)
                    {
                        ShowUsage();
                        continue;
                    }

                    if (parts[0] == "1")
                    {
                        turnOn = true;
                    } 
                    else if (parts[0] == "0")
                    {
                        turnOn = false;
                    }
                    else
                    {
                        Console.WriteLine("Unrecognized command");
                        ShowUsage();
                        continue;
                    }

                    if (!byte.TryParse(parts[1], out byte pin))
                    {
                        Console.WriteLine("Can't parse pin");
                        ShowUsage();
                        continue;
                    }

                    var result = lightswitcher.ChangeSwitchState(pin, turnOn);
                    Console.WriteLine($"result: {result}");
                }
            }
        }

        private static void ShowUsage()
        {
            Console.WriteLine("Print '1|0 <SwitchId>' to turn on of off switch");
            Console.WriteLine($"{SWITCH_ALL_ON_COMMAND} to turn all switches on");
            Console.WriteLine($"{SWITCH_ALL_OFF_COMMAND} to turn all switches off");
            Console.WriteLine($"{GET_STATE_COMMAND} to show state");
            Console.WriteLine($"{EXIT_COMMAND} for exit");
        }

        private const string EXIT_COMMAND = "x";
        private const string SWITCH_ALL_ON_COMMAND = "allon";
        private const string SWITCH_ALL_OFF_COMMAND = "alloff";
        private const string GET_STATE_COMMAND = "state";
    }
}
