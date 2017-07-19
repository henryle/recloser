using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;


namespace TcpComm
{
    public class TcpBase
    {
        #region Members & Properties
        //protected static readonly int MaxBufferSize = 4098;
        public  int buffersize;
        protected byte[] _rvcBuffer;
        public DateTime StartTime { get; set; }
        protected Socket _socket;
        public bool success;
        public event EventHandler<StatusChangedEventArgs> OnStatusChanged;
        public event EventHandler<DataTransferEventArg> OnDataReceived;
        public event EventHandler<DataTransferEventArg> OnDataSent;

        protected RingBuffer _ringBuffer;
        public RingBuffer ReceiveBuffer
        {
            get { return _ringBuffer; }
        }

        public Func<Type, RingBuffer, byte[]> DoParse { get; set; }
        
        protected ICommDevice _device;
        public ICommDevice Device
        {
            get
            {
                return _device;
            }
        }
        
        private static int _instanceCount;

        public bool IsConnected
        {
            get
            {
                return _socket != null;
            }
        }
        #endregion

        #region Ctor()
        //static TcpBase()
        //{
        //    _instanceCount = 0;
            
        //}

        public TcpBase(ICommDevice device, int bsize)
        {
            buffersize = bsize;
            InitilizeComponent(device);
            
            
            _rvcBuffer = new byte[buffersize];
        }         
        #endregion

        #region Helpers
        protected void InitilizeComponent(ICommDevice device)
        {
            System.Threading.Interlocked.Increment(ref _instanceCount);
            _device = device;
            _ringBuffer = new RingBuffer(buffersize);
            
        }
        public void Dispose()
        {
            this.Dispose();
        }
        public virtual void Send(byte[] data)
        {
            if (_socket != null)
            {
                try
                {
                    if (_socket.Connected)
                    {

                        int bytcount = _socket.Send(data, 0, data.Length, SocketFlags.None);
                        if (this.OnDataSent != null)
                            this.OnDataSent(this, new DataTransferEventArg(data, _device.Port));

                        //if (Ultility.IsConnected(_socket))
                        //{
                        //    _socket.Send(data, 0, data.Length, SocketFlags.None);
                        //    if (this.OnDataSent != null)
                        //        this.OnDataSent(this, new DataTransferEventArg(data, _device.Port));
                        //}
                        //else
                        //{
                        //    // _sockect is disconnect
                        //    OnSocketDisconnected();
                        //}
                    }
                }
                catch (Exception ex)
                {
                    FA_Accounting.Common.LogService.Logger.Error("Send", ex);
                }
            }
        }

        protected virtual void OnSocketDisconnected()
        {
            try
            {
                if (_socket != null)
                {
                    _socket.Shutdown(SocketShutdown.Both);
                    _socket.Disconnect(false);
                    _socket = null;
                }
            }
            catch (Exception ex)
            {
                FA_Accounting.Common.LogService.Logger.Error("OnSocketDisconnected", ex);
            }
        }

        protected void ProcessRingBuffer(byte[] data)
        {
            try
            {
                if (_device == null) return;
                bool found = false;
                byte[] buff;
                bool dataAdded;

            Process:
                dataAdded = false;

                if (!_ringBuffer.IsFull)
                {
                    _ringBuffer.Write(data, 0, data.Length);
                    dataAdded = true;
                }

                /*if (DoParse != null)
                {
                    found = false;
                    buff = DoParse(_device.GetType(), _ringBuffer);

                    while (buff != null)
                    {
                        found = true;
                        _device.UpdateData(buff);

                        buff = DoParse(_device.GetType(), _ringBuffer);
                    }
                }
                */
                found = false;
                buff = _device.DoParse(_ringBuffer);

                while (buff != null)
                {
                    found = true;
                    _device.UpdateData(buff);

                    buff = _device.DoParse(_ringBuffer);
                }
                // incase no data package found
                // Remove unidentified bytes from buffer to release spaces
                if (!found && _ringBuffer.IsFull)
                {
                    var byteToRelease = _ringBuffer.Capacity - 200;

                    if (byteToRelease < 0)
                        byteToRelease = _ringBuffer.Capacity - 100;

                    var unusedBytes = new byte[byteToRelease];

                    _ringBuffer.Read(unusedBytes, 0, byteToRelease);
                }

                if (!dataAdded) goto Process;
            }
            catch (Exception ex)
            {
                FA_Accounting.Common.LogService.Logger.Error("ProcessRingBuffer", ex);
            }
        } 
        #endregion
        
        #region Async Result
        protected void OnSend(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                client.EndSend(ar);                
            }
            catch (Exception ex)
            {
                FA_Accounting.Common.LogService.Logger.Error("OnSend", ex);                
            }
        }
        public virtual void StopListening()
        {
        }
        public virtual void StartListening()
        {
        }
        public virtual void Restart()
        {
        }
        
        protected virtual void OnReceive(IAsyncResult ar)
        {
            //try
            //{
            //    Socket clientSocket = (Socket)ar.AsyncState;
            //    int receivedBytes = clientSocket.EndReceive(ar);
                
            //    if (receivedBytes > 0)
            //    {
            //        var data = new byte[receivedBytes];
            //        Array.Copy(_rvcBuffer, 0, data, 0, receivedBytes);

            //        // Add data to ring buffer
            //        _ringBuffer.Write(data, 0, data.Length);
            //        ProcessRingBuffer();

            //        // Raise event
            //        RaiseDataReceiveEvent(data);                    
            //    }

            //    //continue receiving
            //    clientSocket.BeginReceive(_rvcBuffer, 0, _rvcBuffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), clientSocket);
            //}
            //catch (Exception ex)
            //{
            //    LogService.Logger.Error("OnReceive", ex);                
            //}
        }
        public bool Alertprogram = false;
        protected void RaiseDataReceiveEvent(byte[] data)
        {
            if (this.OnDataReceived != null)
                this.OnDataReceived(this, new DataTransferEventArg(data, _device.Port));
        }

        protected void RaiseStatusChangedEvent(string status)
        {
            if (this.OnStatusChanged != null)
                OnStatusChanged(this, new StatusChangedEventArgs(status));
        }
        #endregion
    }
}
