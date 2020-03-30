using System;
using System.Linq;
using Serilog;

namespace ArduinoLightswitcherGateway
{
    public class LightSwitcherGateway : ILightSwitcherGateway
    {
        public LightSwitcherGateway(IArduinoGateway arduinoGateway, LightSwitcherConfig lightSwitcherConfig)
        {
            if (arduinoGateway == null)
            {
                throw new ArgumentException("Gateway can't be null.", nameof(arduinoGateway));
            }

            if (lightSwitcherConfig == null)
            {
                throw new ArgumentException("lightswitcher config can't be null", nameof(lightSwitcherConfig));
            }

            _arduinoGateway = arduinoGateway;
            _lightSwitcherConfig = lightSwitcherConfig;
        }

        public string ChangeSwitchState(byte switchId, bool isOn)
        {
            var response = _arduinoGateway.Send((byte)(isOn 
                ? switchId + _lightSwitcherConfig.TurnOnOffset
                : switchId + _lightSwitcherConfig.TurnOffOffset));
            
            if ((isOn && !response.StartsWith(TURNED_ON_RESPONSE))
            || !isOn && !response.StartsWith(TURNED_OFF_RESPONSE))
            {
                return $"Received unexpected response: {isOn}, {response}";
            }

            var responseParts = response.Split(':');
            if (responseParts.Length != 3
                || !byte.TryParse(responseParts[2].Trim(), out byte switchIdParsed))
            {
                return $"Received unexpected response: {response}";
            }

            if (switchIdParsed != switchId)
            {
                return $"Returned switchId is not equal to the one was sent {switchIdParsed} {switchId}";
            }

            return $"Successfully change state of switch {switchId} to {isOn}";
        }

        public void SwitchAllOn()
        {
            var response = _arduinoGateway.Send(_lightSwitcherConfig.SwitchAllOnCommand);
            _logger.Information("Switch all on response {response}", response);
        }

        public void SwitchAllOff()
        {
            var response = _arduinoGateway.Send(_lightSwitcherConfig.SwitchAllOffCommand);
            _logger.Information("Switch all off response {response}", response);
        }
        
        public SwitchState[]  GetStatus()
        {
            var response = _arduinoGateway.Send(_lightSwitcherConfig.ShowStateCommand);
            _logger.Information("State {response}", response);

            const string STATE_RESPONSE_PREFIX = "State:";
            if (!response.StartsWith(STATE_RESPONSE_PREFIX))
            {
                return new SwitchState[0];
            }

            var parts = response
                .Substring(STATE_RESPONSE_PREFIX.Length)
                .Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

            return parts
                .Select(state => state.Split(new [] { '=' })[1] == "0" ? SwitchState.Off : SwitchState.On)
                .ToArray();
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                _arduinoGateway.Dispose();
                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~LightSwitcherGateway()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion
        
        private readonly IArduinoGateway _arduinoGateway;

        private  readonly LightSwitcherConfig _lightSwitcherConfig;
        
        private readonly ILogger _logger = Log.ForContext<LightSwitcherGateway>();

        private const string TURNED_ON_RESPONSE = "Turn ON pin:";
        
        private const string TURNED_OFF_RESPONSE = "Turn OFF pin:";
    }
}