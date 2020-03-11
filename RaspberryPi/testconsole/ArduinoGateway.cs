using System;
using System.Linq;
using System.IO.Ports;
using Serilog;
using System.Text;
using System.Threading;

namespace ArduinoLightswitcherGateway
{
    public class ArduinoGateway : IDisposable
    {
        public ArduinoGateway(ArduinoGatewayConfig gatewayConfig)
        {
            if (gatewayConfig == null)
            {
                throw new ArgumentException("Config can't be null", nameof(gatewayConfig));
            }

            _arduinoGatewayConfig = gatewayConfig;
        }

        public void Open()
        {
            _logger.Debug("Opening Arduino Gateway. Searching for port with prefix {prefix}", _arduinoGatewayConfig.SerialPortNamePrefix);
            var portNames = SerialPort.GetPortNames();

            _logger.Debug("Found ports {portNames}", portNames);
            var arduinoPort = portNames.FirstOrDefault(port => port.Contains(_arduinoGatewayConfig.SerialPortNamePrefix));
            if (arduinoPort == null)
            {
                throw new ArduinoGatewayException($"No serial port with prefix {_arduinoGatewayConfig.SerialPortNamePrefix} was found.");
            }

            _serialPort = new SerialPort(arduinoPort, _arduinoGatewayConfig.SerialPortBaudRate);
            _serialPort.ReadTimeout = 15000;
            _serialPort.WriteTimeout = 15000;
            _serialPort.Open();
        }

        public string Send(byte command)
        {
            if (_serialPort == null || !_serialPort.IsOpen)
            {
                _logger.Information("Port wasn't open. Opening...");
                Open();
            }

            _logger.Debug("Sending command {command}", command);
            _serialPort.Write(new byte[] { command }, 0, 1);
            _logger.Debug("Waiting for a response");

            var i = 0;
            var response = new StringBuilder();

            var buffer = new byte[150];
            var bytesRed = 0;
            _logger.Debug("Arduino response: {response}", response);
            while (i < 200)
            {
                if (_serialPort.BytesToRead > 0)
                {
                    bytesRed = _serialPort.Read(buffer, 0, Math.Min(_serialPort.BytesToRead, buffer.Length));
                }
                
                var bufferedResponse = ASCIIEncoding.ASCII.GetString(buffer);
                _logger.Debug("{byte}", bufferedResponse);

                response.Append(bufferedResponse);
                i++;
                Thread.Sleep(100);
            }

            return response.ToString();
        }

        public void Close()
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
            }
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

                _serialPort.Dispose();

                // TODO: free unmanage d resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ArduinoGateway()
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

        
        private SerialPort _serialPort;
        private readonly ArduinoGatewayConfig _arduinoGatewayConfig;
        private readonly ILogger _logger = Log.ForContext<ArduinoGateway>();
    }
}