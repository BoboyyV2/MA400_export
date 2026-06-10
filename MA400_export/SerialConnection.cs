using ACadSharp.Tables;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MA400_export
{
    public class SerialConnection
    {
        private SerialPort _serialPort;
        private bool _isListening;

        private Queue<string> _queue = new Queue<string>();
        private Queue<PointF> _HeadPositionBuffer = new Queue<PointF>();

        public SerialConnection()
        {
            _isListening = false;
        }
        /*___________________________________________________________________ UTIL ___________________________________________________________________*/

        /**
         * <returns>true if the derial port is open</returns>
         */
        public bool IsOpen()
        {
            return _serialPort.IsOpen;
        }

        /**
         * <summary>Add a byte array to the recieved data buffer.</summary>
         * <remarks>depending on the format recieved, this should either be useless or heaviely modifiede</remarks>
         */
        private void AddByteLineToBuffer(byte[] line)
        {
            //TODO connaitre le typage et le formatage des données transmise, sans quoi je ne peux rien faire
            string stringline = "";
            _queue.Enqueue(stringline);
        }

        /**
         * <summary>Attempt to add a new position to the buffer containing the welding head position</summary>
         */
        private void TryAddHeadPositionToBuffer() 
        {
            if(_queue.Count < 2)
            {
                return;
            }
            double Y;
            double X;
            try
            {
                string Xstring = _queue.Dequeue();
                string Ystring = _queue.Dequeue();

                X = Convert.ToDouble(Xstring);
                Y = Convert.ToDouble(Ystring);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to get the Position of the Welding head. " + ex.Message);
                return;
            }
            _HeadPositionBuffer.Enqueue(new PointF( (float)X, (float)Y));
        }

        /*___________________________________________________________________ CONNECTION ___________________________________________________________________*/
       
        /**
         * <summary>Open a port with the specified data</summary>
         * <param name="data">the port data containing all the relevent informations.</param>
         */
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
                    ReadTimeout = 500,
                    WriteTimeout = 500,
                };
                _serialPort.DataReceived += new SerialDataReceivedEventHandler(OnDataReceived);


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

        /**
         * <summary>Close the port if it was open</summary>
         */
        public void CloseConnection()
        {
            if (IsOpen())
            {
                _serialPort.Close();
                _serialPort.Dispose();
                Console.WriteLine("Serial connection closed successfully.");
            }
        }

        /*___________________________________________________________________ DATASENDER ___________________________________________________________________*/


        /**
         * <summary>Send a string into the port if it is open.</summary>
         */
        public bool SendString(string data)
        {
            if (!_serialPort.IsOpen)
            {
                Console.WriteLine("Port is not open for sending data.");
                MessageBox.Show("Impossible d'envoyer les données, le port de communication est fermé.");
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
                MessageBox.Show($"Echec d'envoie des données. {ex.Message}");

                return false;
            }
        }

        /**
         * <summary>Send binary data into the port if it is open.</summary>
         */
        public bool SendBinaryData(byte[] data)
        {
            if (!_serialPort.IsOpen)
            {
                Console.WriteLine("Port is not open for binary transmission.");
                MessageBox.Show("Impossible d'envoyer les données, le port de communication est fermé.");

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
                MessageBox.Show($"Echec d'envoie des données. {ex.Message}");

                return false;
            }
        }

        /**
         * <summary>Send Hexa data into the port if it's open.</summary>
         */
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


        /**
         * <summary>read a string from the port if it's open.</summary>
         */
        public string ReadStringData()
        {
            if (!_serialPort.IsOpen)
            {
                Console.WriteLine("Port is not open for reading.");
                return null;
            }

            try
            {

                string receivedData = _serialPort.ReadLine();
                Console.WriteLine($"Received: {receivedData}");
                return receivedData.Trim();
            }
            catch (TimeoutException)
            {
                Console.WriteLine("Timeout occurred while reading data.");
                MessageBox.Show("Timeout lors de la récupération de données. ");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading data: {ex.Message}");
                MessageBox.Show($"Erreur lors de la récupération de données. {ex.Message}");

                return null;
            }
        }

        /**
         * <summary>read binary data from the port if it's open.</summary>
         */
        public byte[] ReadBinaryData(int expectedBytes)
        {
            if (!_serialPort.IsOpen)
                return null;

            try
            {
                byte[] buffer = new byte[expectedBytes];
                int bytesRead = _serialPort.Read(buffer, 0, expectedBytes);

                if (bytesRead > 0)
                {
                    byte[] actualData = new byte[bytesRead];
                    Array.Copy(buffer, actualData, bytesRead);
                    return actualData;
                }
            }
            catch (TimeoutException)
            {
                Console.WriteLine("Timeout while reading binary data.");
                MessageBox.Show("Timeout lors de la récupération de données binaire. ");

            }

            return null;
        }

        /** <summary>
         * Send a command and await a certain number of lines in response.<br></br>
         * Should be called from a non-UI thread ( Task.Run() ).
         * </summary>
         * <param name="command">The command to send</param>
         * <param name="expectedLines">The expected number of lines</param>
         * <param name="timeoutPerLineMs">The max delay between two lines in ms</param>
         * <returns>Liste des lignes reçues.</returns>
         * */
        public string[] SendAndReceiveLines(string command, int expectedLines, int timeoutPerLineMs = 250)//potentiellement super long 
        {
            if (!IsOpen()) return null;

            var result = new string[expectedLines];
            bool wasListening = false;
            if (_isListening)
            {
                wasListening = true;
                _isListening = false; //async listen
            }
            int ReadTimeout = _serialPort.ReadTimeout;//sauvegarde
            int i = 0;
            try
            {
                _serialPort.DiscardInBuffer();
                _serialPort.ReadTimeout = timeoutPerLineMs;

                _serialPort.WriteLine(command);
                Console.WriteLine($"Envoyé : {command}, attente de {expectedLines} lignes.");

                for ( i = 0 ; i < expectedLines; i++)
                {
                    string line = _serialPort.ReadLine();//
                    result[i] = line;
                }

                return result;
            }
            catch (TimeoutException)
            {
                Console.WriteLine($"Envoyé : {command}, Timeout après {i}/{expectedLines} lignes.");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Envoyé : {command}, Erreur : {ex.Message}");
                return null;
            }
            finally
            {
                //reset to previous values
                _serialPort.ReadTimeout = ReadTimeout;
                if (wasListening)
                {
                    _isListening = true;
                }
                
            }
        }

        
        /** <summary>
        * Send a command and await a certain number of bytes in response.<br></br>
        * Should be called from a non-UI thread ( Task.Run() ).
        * </summary>
        * <param name="command">The command to send</param>
        * <param name="expectedBytes">The expected number of bytes</param>
        * <param name="timeoutMs">The max delay between two bytes in ms</param>
        * <returns>Liste des lignes reçues.</returns>
        * */
        public byte[] SendAndReceiveBytes(byte[] command, int expectedBytes, int timeoutMs = 100)
        {
            
            if (!IsOpen()) return null;

            bool wasListening = false;
            if (_isListening)
            {
                wasListening = true;
                _isListening = false; //async listen
            }
            int ReadTimeout = _serialPort.ReadTimeout;//sauvegarde
            try
            {
                _serialPort.DiscardInBuffer();
                _serialPort.ReadTimeout = timeoutMs;

                _serialPort.Write(command, 0, command.Length);

                byte[] buffer = new byte[expectedBytes];
                int totalRead = 0;
                while (totalRead < expectedBytes)
                {
                    int read = _serialPort.Read(buffer, totalRead, expectedBytes - totalRead);
                    if (read == 0) break;
                    totalRead += read;
                }

                byte[] result = new byte[totalRead];
                Array.Copy(buffer, result, totalRead);
                return result;
            }
            catch (TimeoutException)
            {
                Console.WriteLine($"[Request] Timeout binaire.");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Request] Erreur : {ex.Message}");
                return null;
            }
            finally
            {
                //reset to previous values
                _serialPort.ReadTimeout = ReadTimeout;
                if (wasListening)
                {
                    _isListening = true;
                }
            }
        }

        /*___________________________________________________________________ ASYNC DATARECVER ___________________________________________________________________*/

        /**
         * <summary>
         * handle the behavior when reciving data (welding head position) into the port.<br></br>
         * Send it to a buffer.
         * </summary>
         */
        private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (!_isListening)
                return;

            try
            {
                if (_serialPort.BytesToRead > 0)
                {
                    // Read as string
                    string stringData = _serialPort.ReadExisting();
                    if (!string.IsNullOrEmpty(stringData))
                    {
                        //DataReceived?.Invoke(this, stringData);
                        _queue.Enqueue(stringData);
                        MessageBox.Show("got something : " + stringData);
                    }

                    // For binary data reception
                    if (_serialPort.BytesToRead > 0)
                    {
                        byte[] binaryData = new byte[_serialPort.BytesToRead];
                        _serialPort.Read(binaryData, 0, binaryData.Length);
                        //BinaryDataReceived?.Invoke(this, binaryData);
                        AddByteLineToBuffer(binaryData);
                        MessageBox.Show("got some binary shee : " + binaryData);

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in async data reception: {ex.Message}");
            }
            TryAddHeadPositionToBuffer();
        }

        /**
         * <summary>Start listening to the port</summary>
         */
        public void StartListening()
        {
            //_serialPort.DiscardInBuffer(); rique de casse
            _isListening = true;
            Console.WriteLine("Started listening for serial data...");
        }


        /**
         * <summary>Stop listening to the port</summary>
         */
        public void StopListening()
        {
            _isListening = false;
            Console.WriteLine("Stopped listening for serial data.");
        }
    }

}



