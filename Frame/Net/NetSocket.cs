using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;


public enum ErrorSocket
{
    Success = 0,
    TimeOut,
    SocketNull,
    SocketUnConnect,

    ConnectSuccess,
    ConnectUnSuccessUnKnow,
    ConnectError,

    SendSuccess,
    SendUnSuccessUnKnow,

    RecvSuccess,
    RecvUnSuccessUnKnow,

    DisConnectSuccess,
    DisConnectUnKnow
}

public delegate void CallbackNormal(bool success, ErrorSocket error, string exception);
//public delegate void CallbackSend(bool success, ErrorSocket error, string exception);
public delegate void CallbackRecv(bool success, ErrorSocket error, string exception, byte[] byteMsg, string strMsg);
//public delegate void CallbackDisConnect(bool success, ErrorSocket error, string exception);

public class NetSocket {

    private ErrorSocket errorSocket;
    private CallbackNormal callbackConnect;
    private CallbackNormal callbackSend;
    private CallbackNormal callbackDisConnect;
    private CallbackRecv callbackRecv;

    private Socket clientSocket;
    private SocketBuffer recvBuffer;

    private string addressIP;

    private ushort port;

    public NetSocket() {
        recvBuffer = new SocketBuffer(6, RecvMsgOver);
        recvBuf = new byte[1024];
    }


    #region Send

    public void AsynSend(byte[] sendBuffer, CallbackNormal callbackSend)
    {
        errorSocket = ErrorSocket.Success;
        this.callbackSend = callbackSend;
        if (clientSocket == null)
        {
            errorSocket = ErrorSocket.SocketNull;
            this.callbackSend(false, errorSocket, "");
        }
        else if (!clientSocket.Connected)
        {
            errorSocket = ErrorSocket.SocketUnConnect;
            this.callbackSend(false, errorSocket, "");
        }
        else
        {        
            IAsyncResult iar = clientSocket.BeginSend(sendBuffer, 0, sendBuffer.Length, SocketFlags.None, SendCallback, clientSocket);
            if (!WriteDot(iar))
            {
                errorSocket = ErrorSocket.SendUnSuccessUnKnow;
                this.callbackSend(false, errorSocket, "Send fail");
            }
        }
    }

    public void SendCallback(IAsyncResult iar) {
        try
        {
            int length = clientSocket.EndSend(iar);
            if (length > 0)
            {
                errorSocket = ErrorSocket.SendSuccess;
                callbackSend(true, errorSocket, "");
            }
            else
            {
                errorSocket = ErrorSocket.SendUnSuccessUnKnow;
                callbackSend(false, errorSocket, "");
            }
        }
        catch (Exception ex)
        {
            errorSocket = ErrorSocket.SendUnSuccessUnKnow;
            callbackSend(false, errorSocket, ex.ToString());
            throw;
        }        
    }

    #endregion

    #region Connect

    public void AsynConnect(string ip, ushort port, CallbackNormal connectBack, CallbackRecv callbackRecv) {
        errorSocket = ErrorSocket.Success;
        this.callbackConnect = connectBack;
        this.callbackRecv = callbackRecv;
        if (clientSocket != null && clientSocket.Connected)
        {
            //重复连接
            this.callbackConnect(false, ErrorSocket.ConnectError, "connect repeat!!!");
        }
        else if (clientSocket == null || !clientSocket.Connected)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAdress = IPAddress.Parse(ip);
            IPEndPoint endPoint = new IPEndPoint(ipAdress, port);
            IAsyncResult iar = clientSocket.BeginConnect(endPoint, ConnectCallback, clientSocket);
            if (!WriteDot(iar)){
                errorSocket = ErrorSocket.TimeOut;
                this.callbackConnect(false, errorSocket, "连接超时！！！");
            }
                
        }            
    }

    private void ConnectCallback(IAsyncResult iar) {
        try
        {
            clientSocket.EndConnect(iar);
            if (!clientSocket.Connected)
            {
                errorSocket = ErrorSocket.ConnectUnSuccessUnKnow;
                callbackConnect(false, errorSocket, "连接超时！！！");
                return;
            }
            else
            {
                errorSocket = ErrorSocket.Success;
                callbackConnect(true, errorSocket, "Success!!!");
            }
        }
        catch (Exception ex)
        {
            callbackConnect(false, errorSocket, ex.ToString());
            throw;
        }
    }

    public bool IsConnect() {
        if (clientSocket != null && clientSocket.Connected)
            return true;
        return false;
    }

    #endregion

    #region Recv

    byte[] recvBuf;

    public void Receive() {
        if (clientSocket != null && clientSocket.Connected)
        {
            IAsyncResult iar = clientSocket.BeginReceive(recvBuf, 0, recvBuf.Length, SocketFlags.None, RecvCallback, clientSocket);
            if (!WriteDot(iar))
            {
                errorSocket = ErrorSocket.RecvUnSuccessUnKnow;
                callbackRecv(false, errorSocket, "Receive fail", null, "");
            }
        }
    }

    private void RecvCallback(IAsyncResult iar) {
        try
        {
            if (!clientSocket.Connected)
            {
                errorSocket = ErrorSocket.RecvUnSuccessUnKnow;
                callbackRecv(false, errorSocket, "Connect fail when receive", null, "");
                return;
            }
            int length = clientSocket.EndReceive(iar);
            if(length == 0)
                return;
            recvBuffer.RecvByte(recvBuf, length);
        }
        catch (Exception ex)
        {
            errorSocket = ErrorSocket.RecvUnSuccessUnKnow;
            callbackRecv(false, errorSocket, ex.ToString(), null, "");
            throw;
        }
        Receive();
    }

    public void RecvMsgOver(byte[] allByte)
    {
        errorSocket = ErrorSocket.RecvSuccess;
        callbackRecv(true, errorSocket, "", allByte, "receive back success");
    }

    #endregion

    #region DisConnect

    public void AsynDisConnect(CallbackNormal callbackDisConnect) {
        try
        {
            errorSocket = ErrorSocket.Success;
            this.callbackDisConnect = callbackDisConnect;
            if (clientSocket == null)
            {
                errorSocket = ErrorSocket.DisConnectUnKnow;
                this.callbackDisConnect(false, errorSocket, "Client is null");
            }
            else if (!clientSocket.Connected)
            {
                errorSocket = ErrorSocket.DisConnectUnKnow;
                this.callbackDisConnect(false, errorSocket, "Client is disconnect");
            }
            else
            {
                IAsyncResult iar = clientSocket.BeginDisconnect(false, DisconnectCallback, clientSocket);
                if (!WriteDot(iar))
                {
                    errorSocket = ErrorSocket.DisConnectUnKnow;
                    this.callbackDisConnect(false, errorSocket, "Disconnect fail");
                }
            }
        }
        catch (Exception ex)
        {
            errorSocket = ErrorSocket.DisConnectUnKnow;
            this.callbackDisConnect(false, errorSocket, ex.ToString());
            throw;
        }
    }
    
    public void DisconnectCallback(IAsyncResult iar) {
        try
        {
            clientSocket.EndDisconnect(iar);
            clientSocket.Close();
            clientSocket = null;
            errorSocket = ErrorSocket.DisConnectSuccess;
            callbackDisConnect(true, errorSocket, "");
        }
        catch (Exception ex)
        {
            errorSocket = ErrorSocket.DisConnectUnKnow;
            callbackDisConnect(false, errorSocket, ex.ToString());
            throw;
        }
    }

    #endregion

    #region TimeOut check

    private bool WriteDot(IAsyncResult iar) {
        int time = 0;
        while (!iar.IsCompleted)
        {
            time++;
            if (time > 20)
            {
                errorSocket = ErrorSocket.TimeOut;
                return false;
            }
            Thread.Sleep(100);
        }
        return true;
    }

    #endregion
}