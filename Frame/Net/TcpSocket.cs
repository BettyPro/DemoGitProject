using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;

namespace Demo
{
    public enum TCPEvent
    {
        TcpConnect = ManagerID.NetManager + 1,
        TcpSendMsg
    }

    public class TcpSocket : NetBase
    {

        NetWorkToServer networkToServer;

        void Awake()
        {
            msgIDs = new ushort[]
            {
                (ushort) TCPEvent.TcpConnect,
                (ushort) TCPEvent.TcpSendMsg
            };
            RegistSelf(this, msgIDs);
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void ProcessEvent(MsgBase tmpMsg)
        {
            Debug.Log(11);

            //base.ProcessEvent(tmpMsg);
            switch (tmpMsg.msgID)
            {
                case (ushort) TCPEvent.TcpConnect:
                    TCPConnectMsg connectMsg = (TCPConnectMsg) tmpMsg;
                    networkToServer = new NetWorkToServer(connectMsg.ip, connectMsg.port);
                    break;
                case (ushort) TCPEvent.TcpSendMsg:
                    TCPMsg sendMsg = (TCPMsg) tmpMsg;
                    networkToServer.PutSendMsgToPool(sendMsg.netMsg);
                    break;
                default:
                    break;
            }
        }
    }

    public class TCPNetMsgs<T> : NetMsgBase where T : IExtensible
    {

        public TCPNetMsgs(T tmp, ushort msgID)
        {
            this.msgID = msgID;
            SetBuffer<T>(tmp, ref buffer);
            //byte[] tmpArr = IProtoTools.Serialize(tmp);
            //buffer = new byte[tmpArr.Length + 6];

            //byte[] dataLength = BitConverter.GetBytes(tmpArr.Length);
            //Buffer.BlockCopy(dataLength, 0, buffer, 0, 4);

            //byte[] eventID = BitConverter.GetBytes(msgID);
            //Buffer.BlockCopy(eventID, 0, buffer, 4, 2);

            //Buffer.BlockCopy(tmpArr, 0, buffer, 6, tmpArr.Length);
        }

        private void SetBuffer<V>(V tmp, ref byte[] tmpBuffer) where V : IExtensible
        {
            byte[] tmpArr = IProtoTools.Serialize(tmp);
            tmpBuffer = new byte[tmpArr.Length + 6];

            byte[] dataLength = BitConverter.GetBytes(tmpArr.Length);
            Buffer.BlockCopy(dataLength, 0, tmpBuffer, 0, 4);

            byte[] eventID = BitConverter.GetBytes(msgID);
            Buffer.BlockCopy(eventID, 0, tmpBuffer, 4, 2);

            Buffer.BlockCopy(tmpArr, 0, tmpBuffer, 6, tmpArr.Length);
        }

        public override byte[] GetNetBytes()
        {
            return buffer;
        }

        public void ChangeMsgData<U>(U tmp) where U : IExtensible
        {
            SetBuffer<U>(tmp, ref buffer);
        }

        public void ChangeMsgData(ushort msgID)
        {
            this.msgID = msgID;
            byte[] eventID = BitConverter.GetBytes(msgID);
            Buffer.BlockCopy(eventID, 0, buffer, 4, 2);
        }

        public void ChangeMsgData<U>(U tmp, ushort msgID) where U : IExtensible
        {
            this.msgID = msgID;
            SetBuffer<U>(tmp, ref buffer);
        }

        public U GetPBClass<U>() where U : IExtensible
        {
            int head = 6;
            byte[] tmpArr = new byte[buffer.Length - head];
            Buffer.BlockCopy(buffer, head, tmpArr, 0, buffer.Length - head);
            return IProtoTools.Deserialize<U>(tmpArr);
        }

        public U GetPBClass<U>(byte[] tmpArr) where U : IExtensible
        {
            buffer = tmpArr;
            return GetPBClass<U>();
        }
    }

    public class TCPConnectMsg : MsgBase
    {
        public string ip;
        public ushort port;

        public TCPConnectMsg(ushort msgID, string ip, ushort port)
        {
            this.msgID = msgID;
            this.ip = ip;
            this.port = port;
        }
    }

    public class TCPMsg : MsgBase
    {
        public NetMsgBase netMsg;

        public TCPMsg(ushort msgID, NetMsgBase netMsg)
        {
            this.msgID = msgID;
            this.netMsg = netMsg;
        }
    }
}