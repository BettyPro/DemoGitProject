using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class NetWorkToServer
    {

        private Queue<NetMsgBase> recvMsgPool = null;
        private Queue<NetMsgBase> sendMsgPool = null;

        private NetSocket clientSocket;

        private Thread sendThread;

        public NetWorkToServer(string ip, ushort port)
        {
            recvMsgPool = new Queue<NetMsgBase>();
            sendMsgPool = new Queue<NetMsgBase>();
            clientSocket = new NetSocket();
            clientSocket.AsynConnect(ip, port, AsynConnectCallback, AsynRecevieCallback);
        }

        private void AsynConnectCallback(bool success, ErrorSocket error, string exception)
        {
            if (success)
            {
                if (sendThread == null)
                    sendThread = new Thread(LoopSendMsg);
                sendThread.Start();
            }
        }

        #region SendMsg

        private void LoopSendMsg()
        {
            while (clientSocket != null && clientSocket.IsConnect())
            {
                lock (sendMsgPool)
                {
                    while (sendMsgPool.Count > 0)
                    {
                        NetMsgBase netMsgBase = sendMsgPool.Dequeue();
                        clientSocket.AsynSend(netMsgBase.GetNetBytes(), SendCallback);
                    }
                }

                Thread.Sleep(100);
            }
        }

        private void SendCallback(bool success, ErrorSocket error, string exception)
        {

        }

        public void PutSendMsgToPool(NetMsgBase netMsgBase)
        {
            lock (sendMsgPool)
            {
                sendMsgPool.Enqueue(netMsgBase);
            }
        }

        #endregion

        #region ReceiveMsg

        private void AsynRecevieCallback(bool success, ErrorSocket error, string exception, byte[] byteMsg,
            string strMsg)
        {
            if (success)
            {
                PutRecvMsgToPool(byteMsg);
            }
        }

        private void PutRecvMsgToPool(byte[] recvMsg)
        {
            NetMsgBase netMsgBase = new NetMsgBase(recvMsg);
            recvMsgPool.Enqueue(netMsgBase);
        }

        public void Update()
        {
            if (recvMsgPool != null)
            {
                while (recvMsgPool.Count > 0)
                {
                    NetMsgBase netMsgBase = recvMsgPool.Dequeue();
                    AnalyseData(netMsgBase);
                }
            }
        }

        private void AnalyseData(NetMsgBase netMsgBase)
        {
            MsgCenter.Instance.SendToMsg(netMsgBase);
        }

        #endregion

        #region DisConnect

        public void DisConnect()
        {
            if (clientSocket != null && clientSocket.IsConnect())
                clientSocket.AsynDisConnect(DisconnectCallback);

        }

        private void DisconnectCallback(bool success, ErrorSocket error, string exception)
        {
            if (success)
                sendThread.Abort();
        }

        #endregion
    }
}