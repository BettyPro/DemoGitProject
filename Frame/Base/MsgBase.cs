using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MsgBase
{
   
    public ushort msgID;

    //role and monster 
    public int roleID;//-Me-- add 用于角色怪物通信
    public string attackName;//-Me-- add 用于角色怪物通信
    public bool allAction = true;//-Me-- add 用于角色怪物通信
    public bool isLoop = false;//-Me-- add 用于角色怪物通信

    // audio
    public bool isNeedWait;

    public ManagerID GetManagerID() {
        int tempID = msgID / FrameTools.msgSpawn;
        ColorDebug.Instance.RedDebug(msgID, "消息中得到的id", true);
        return (ManagerID)(tempID * FrameTools.msgSpawn);
    }

    public MsgBase()
    {
        msgID = 0;
    }

    public MsgBase(ushort tempID) {
        msgID = tempID;
    }
}

public class MsgBase<T> : MsgBase {
    public T tValue;
    public MsgBase(ushort tmpID, T tmpT) : base(tmpID)
    {
        msgID = tmpID;
        tValue = tmpT;
    }
}