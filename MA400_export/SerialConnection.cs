using ACadSharp.Tables;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MA400_export
{
    public class SerialConnection
    {
        private SerialPort _serialPort;

        public SerialConnection(SerialPort port)
        {
            _serialPort = port;
        }

        /*___________________________________________________________________ CONNECTION ___________________________________________________________________*/
        public bool OpenConnection(SerialData data)
        {
            try
            {
                _serialPort = new SerialPort(data.COM)
                {
                    BaudRate = data.BaudRate,
                    DataBits = data.DataBits,
                    Parity = data.ParityBit,
                    StopBits = data.StopBit,
                    Handshake = Handshake.None,
                    ReadTimeout = 3000,
                    WriteTimeout = 3000
                };

                if (!_serialPort.IsOpen)
                {
                    _serialPort.Open();
                    Console.WriteLine($"Successfully connected to {data.COM}");
                    return true;
                }
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine($"Access denied to {data.COM}. Port may be in use.");
                return false;
            }
            catch (ArgumentException)
            {
                Console.WriteLine($"Invalid port name: {data.COM}");
                return false;
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine($"Port {data.COM} is already open.");
                return false;
            }

            return false;
        }

        public void CloseConnection()
        {
            if (_serialPort?.IsOpen == true)
            {
                _serialPort.Close();
                _serialPort.Dispose();
                Console.WriteLine("Serial connection closed successfully.");
            }
        }

        /*___________________________________________________________________ DATASENDER ___________________________________________________________________*/

        

        public bool SendString(string data)
        {
            if (!_serialPort.IsOpen)
            {
                Console.WriteLine("Port is not open for sending data.");
                return false;
            }

            try
            {
                _serialPort.WriteLine(data);
                Console.WriteLine($"Sent: {data}");
                return true;
            }
            catch (TimeoutException)
            {
                Console.WriteLine("Timeout occurred while sending data.");
                return false;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Error sending data: {ex.Message}");
                return false;
            }
        }

        public bool SendBinaryData(byte[] data)
        {
            if (!_serialPort.IsOpen)
            {
                Console.WriteLine("Port is not open for binary transmission.");
                return false;
            }

            try
            {
                // Clear buffers before sending
                _serialPort.DiscardInBuffer();
                _serialPort.DiscardOutBuffer();

                _serialPort.Write(data, 0, data.Length);

                Console.WriteLine($"Sent {data.Length} bytes: {BitConverter.ToString(data)}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Binary transmission failed: {ex.Message}");
                return false;
            }
        }

        public bool SendHexCommand(string hexString)
        {
            try
            {
                // Convert hex string to byte array
                byte[] bytes = new byte[hexString.Length / 2];
                for (int i = 0; i < bytes.Length; i++)
                {
                    bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
                }

                return SendBinaryData(bytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hex command conversion failed: {ex.Message}");
                return false;
            }
        }


        /*___________________________________________________________________ DATARECVER ___________________________________________________________________*/

    }

}

