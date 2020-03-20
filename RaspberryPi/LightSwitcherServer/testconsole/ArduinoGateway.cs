using System;
using System.Linq;
using System.IO.Ports;
using System.Threading;
using Serilog;
using System.Text;

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
            lock (_lockRoot)
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
                _serialPort.ReadTimeout = READ_WRITE_TIMEOUT;
                _serialPort.WriteTimeout = READ_WRITE_TIMEOUT;
                _serialPort.DtrEnable = false;
                _serialPort.RtsEnable = false;
                _serialPort.Parity = Parity.None;
                _serialPort.StopBits = StopBits.One;
                _serialPort.DataReceived += OnDataReceived;
                _serialPort.Open();
            }
        }

        public string Send(byte command)
        {
            lock (_lockRoot)
            {
                if (_serialPort == null || !_serialPort.IsOpen)
                {
                    _logger.Information("Port wasn't open. Opening...");
                    Open();
                }

                _logger.Debug("Sending command {command}.", command);
                _serialPort.Write(new byte[] { command }, 0, 1);
                _logger.Debug("Waiting for a response...");

                string lastResponse;
                do 
                {
                    lastResponse = _lastResponses.Current;
                    // get responses untill they end
                    Thread.Sleep(SLEEP_TIME_MS_TO_GET_RESPONSE);
                }
                while(lastResponse != _lastResponses.Current);
                return _lastResponses.Pop();
            }
        }

        private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var port = (SerialPort)sender;

            var buffer = new byte[READ_BUFFER_SIZE];
            var bytesRed = 0;
            while (port.BytesToRead > 0)
            {
                bytesRed = port.Read(buffer, 0, Math.Min(port.BytesToRead, buffer.Length));
                var bufferedResponse = ASCIIEncoding.ASCII.GetString(buffer, 0, bytesRed);
                _responseBuffer.Append(bufferedResponse);
            }
            
            var lastSubstringIndex = 0;
            for (var i = 0; i < _responseBuffer.Length; i++)
            {
                if (_responseBuffer[i] == '\n')
                {
                    var substring = _responseBuffer.ToString(lastSubstringIndex, i - lastSubstringIndex);
                    _logger.Information("Data received: {data}", substring);
                    _lastResponses.Add(substring);
                    lastSubstringIndex = i + 1;
                }
            }

            if (lastSubstringIndex < _responseBuffer.Length)
            {
                _responseBuffer.Remove(0, lastSubstringIndex);                
            }
            else
            {
                _responseBuffer.Clear();
            }
        }

        public void Close()
        {
            lock (_lockRoot)
            {
                if (_serialPort.IsOpen)
                {
                    _serialPort.DataReceived -= OnDataReceived;
                    _serialPort.Close();
                }
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

        private StringBuilder _responseBuffer = new StringBuilder();

        private CircularBuffer<string> _lastResponses = new CircularBuffer<string>(RESPONSES_BUFFER_SIZE);

        private object _lockRoot = new object();
        private readonly ArduinoGatewayConfig _arduinoGatewayConfig;

        private const int READ_WRITE_TIMEOUT = 5000;
        private const int SLEEP_TIME_MS_TO_GET_RESPONSE = 300;
        private const int READ_BUFFER_SIZE = 150;
        private const int RESPONSES_BUFFER_SIZE = 5;
        private readonly ILogger _logger = Log.ForContext<ArduinoGateway>();
    }
}