using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;



namespace TcpComm
{
    public class TcpClient : TcpBase
    {
        
        
        public string ServerAddress { get; set; }
        public int ServerPort { get; set; }

        public TcpClient(ICommDevice recloser, string serverAddress, int serverPort,int bsize)
            : base(recloser,bsize)
        {
            this.ServerAddress = serverAddress;
            this.ServerPort = serverPort;
            StartTime = DateTime.Now;
            
        }

        public void Connect()
        {
            if (_socket == null)
            {
                _socket = CreateSocketConnection();
            }
        }
        
        
        private Socket CreateSocketConnection()
        {
            //int localport1 = this.ServerPort+10000;
            //int localport2 = this.ServerPort;
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //System.Net.Sockets.TcpClient _client = new System.Net.Sockets.TcpClient(
            //IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
            //if (Alertprogram)
            //{
            //    socket.Bind(new IPEndPoint(localIPs[1], 0));
            //    FA_Accounting.Common.LogService.WriteInfo("Bind Alert, IP", localIPs[1].ToString() + " port: " + localport1);
            //}
            //else
            //{
            //    socket.Bind(new IPEndPoint(localIPs[2], 0));
            //    FA_Accounting.Common.LogService.WriteInfo("Bind normal, IP", localIPs[3].ToString() + " port: " + localport2);
            //}
            try
            {
                //socket.Connect(new IPEndPoint(IPAddress.Parse(this.ServerAddress), this.ServerPort));
                //IAsyncResult result = socket.BeginConnect(new IPEndPoint(IPAddress.Parse(this.ServerAddress), this.ServerPort),null,null);
                IAsyncResult result = socket.BeginConnect(new IPEndPoint(IPAddress.Parse(this.ServerAddress), this.ServerPort), null, null);
                socket.ReceiveTimeout = 10000;
                socket.SendTimeout = 10000;
                success = result.AsyncWaitHandle.WaitOne(2000, true);

                if (!success)
                {
                    // NOTE, MUST CLOSE THE SOCKET

                    socket.Close();
                    //throw new ApplicationException("Failed to connect server.");
                    RaiseStatusChangedEvent(string.Format("Failed to connect to {0}.", this.ServerAddress));
                }
                else
                {
                    socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                    socket.BeginReceive(_rvcBuffer, 0, _rvcBuffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), socket);

                    RaiseStatusChangedEvent(string.Format("Connected to {0}.", this.ServerAddress));
                }
            }
            catch(Exception ex)
            {
                FA_Accounting.Common.LogService.Logger.Error("Connect error", ex);
            }
            return socket;
        }

        public void Disconnect()
        {
            try
            {
                if (_socket != null)
                {
                    _socket.Shutdown(SocketShutdown.Both);
                    _socket.BeginDisconnect(false, OnDisconnect, _socket);
                    RaiseStatusChangedEvent(string.Format("Disconnected to {0}.", this.ServerAddress));
                }
            }
            catch (Exception ex)
            {
                FA_Accounting.Common.LogService.WriteError(ex.Message, ex.ToString());
            }

        }

        public override void StopListening()
        {

            Disconnect();


        }
        public override void StartListening()
        {
            Connect();
        }
        public override void Restart()
        {
            try
            {
                StopListening();
                FA_Accounting.Common.LogService.Logger.Info("Restart listening server.");
                RaiseStatusChangedEvent("Restart listening server.");

                _ringBuffer = new RingBuffer(buffersize);
                //System.Threading.Thread.Sleep(100);
                StartListening();
            }
            catch (Exception ex)
            {
                FA_Accounting.Common.LogService.Logger.Error("Restart Error", ex);
            }
        }
        private void OnDisconnect(IAsyncResult ar)
        {
            Socket clientSocket = (Socket)ar.AsyncState;
            clientSocket.EndDisconnect(ar);

            if (ar.IsCompleted)
            {
                _socket = null;
            }
        }

        public override void Send(byte[] data)
        {
            if (_socket != null)
            {
                if (!_socket.Poll(-1, SelectMode.SelectWrite))
                {
                    _socket.Disconnect(false);
                    _socket.Dispose();

                    _socket = CreateSocketConnection();
                }
            }
            else
            {
                _socket = CreateSocketConnection();
            }

            base.Send(data);
        }

        protected override void OnReceive(IAsyncResult ar)
        {

            Socket clientSocket = (Socket)ar.AsyncState;
            
            int receivedBytes = 0;
            try
            {
                if (clientSocket.Connected)
                { receivedBytes = clientSocket.EndReceive(ar); }
                

                if (receivedBytes > 0)
                {
                    //state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                    var data = new byte[receivedBytes];
                    Array.Copy(_rvcBuffer, 0, data, 0, receivedBytes);

                    // Raise event                    
                    RaiseDataReceiveEvent(data);

                    // Add data to ring buffer
                    //_ringBuffer.Write(data, 0, data.Length);
                    ProcessRingBuffer(data);
                }
            }
            catch (Exception ex)
            {
                FA_Accounting.Common.LogService.Logger.Error("OnReceive", ex);
            }
            finally
            {
                //continue receiving
                if (clientSocket.Connected)
                {
                    try
                    {
                        clientSocket.BeginReceive(_rvcBuffer, 0, _rvcBuffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), clientSocket);
                    }
                    catch (Exception ex)
                    {
                        FA_Accounting.Common.LogService.WriteError("Begin Receive", ex.ToString());
                    }
                }
            }
        }
    }
}
