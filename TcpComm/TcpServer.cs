﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;
using System.Runtime.Remoting.Messaging;
using System.Threading;


namespace TcpComm
{
    public class StateObject
    {
        public Socket workSocket = null;
        public const int BufferSize = 4098;
        public byte[] buffer = new byte[BufferSize];
        public StringBuilder sb = new StringBuilder();
    }    

    public class TcpServer : TcpBase
    {
        
        // Thread signal.
        public static ManualResetEvent allDone = new ManualResetEvent(false);        
        TcpListener _tcpListener;

        private bool _listening = false;

        public TcpServer(ICommDevice recloser,int bsize)
            : base(recloser,bsize)
        {
            StartTime = DateTime.Now;
        }

        public override void StartListening()
        {
            try
            {
                

                if (this._listening)
                {
                    return;
                }
                StartTime = DateTime.Now;
                //Assign the any IP of the machine and listen on port                
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, _device.Port);
                _tcpListener = new TcpListener(ipEndPoint);
                _tcpListener.Start();
                _listening = true;

                _tcpListener.BeginAcceptSocket(new AsyncCallback(OnAcceptSocket), _tcpListener);
                RaiseStatusChangedEvent("Listening");
       
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "AtcServer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public override void StopListening()
        {
            try
            {
                _listening = false;

                if (_tcpListener != null)
                    _tcpListener.Stop();

                OnSocketDisconnected();
            }
            catch (Exception ex)
            {
                FA_Accounting.Common.LogService.Logger.Error("StopListening Error", ex);
            }
        }        

        protected override void OnReceive(IAsyncResult ar)
        {
            if (_socket == null || ar == null)
                return;

            var state = (StateObject)ar.AsyncState;
            if (state == null) return;
            Socket clientSocket = null;
            bool readError = false;

            try
            {
                clientSocket = state.workSocket;

                int receivedBytes = clientSocket.EndReceive(ar);

                if (receivedBytes > 0)
                {
                    var data = new byte[receivedBytes];
                    Array.Copy(state.buffer, 0, data, 0, receivedBytes);

                    // Raise event
                    RaiseDataReceiveEvent(data); // this mainly for showing com log 
                    ProcessRingBuffer(data);
                }
                else
                {   
                    readError = true;
                }
            }
            catch (Exception ex)
            {
                FA_Accounting.Common.LogService.Logger.Error("OnReceive", ex);
                readError = true;
            }

            //continue receiving
            try
            {
                if (clientSocket != null && readError == false)
                    clientSocket.BeginReceive(state.buffer, 0, state.buffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), state);
                else
                    OnSocketDisconnected();
            }
            catch (Exception ex)
            {
                FA_Accounting.Common.LogService.Logger.Error("Call BeginReceive Error", ex);
                try
                {
                    RaiseStatusChangedEvent(string.Format("Lost connection from {0}.", _socket.RemoteEndPoint));
                }
                catch
                {

                }
                Restart();
            }
        }
        
        private void OnAcceptSocket(IAsyncResult ar)
        {
            if (_listening == false) return;

            var listener = ar.AsyncState as TcpListener; 

            try 
            {   
                if (listener != null && listener.Server != null && listener.Server.IsBound) 
                {
                    if(_socket != null)
                        OnSocketDisconnected();

                    _socket = listener.EndAcceptSocket(ar); 
                    _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true); 
                    RaiseStatusChangedEvent(string.Format("Connected from {0}.", _socket.RemoteEndPoint));
                    success = true;
                }
            }
            catch (Exception ex) 
            {
                FA_Accounting.Common.LogService.Logger.Error("OnAccept Error", ex); 
            } 
                       
            // Start receiving and & continue listening            
            try
            {
                if (_socket != null && _socket.IsBound)
                {
                    StateObject state = new StateObject();
                    state.workSocket = _socket;
                    _socket.BeginReceive(state.buffer, 0, state.buffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), state);                    
                }

                if (_tcpListener != null && _tcpListener.Server.IsBound)
                {
                    _tcpListener.BeginAcceptSocket(new AsyncCallback(OnAcceptSocket), _tcpListener);
                }
            }
            catch (Exception ex)
            {
                FA_Accounting.Common.LogService.Logger.Error("Call BeginAcceptSocket Error", ex); 
            }

        }

        protected override void OnSocketDisconnected()
        {
            try
            {
                if (_socket != null)
                    RaiseStatusChangedEvent(string.Format("Disconnected from {0}.", _socket.RemoteEndPoint));
                else
                    RaiseStatusChangedEvent("Disconnected");
                success = false;
                base.OnSocketDisconnected();
            }
            catch (Exception ex)
            {
                FA_Accounting.Common.LogService.WriteError(ex.Message, ex.ToString());
            }
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
    }    
}
