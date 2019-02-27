using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetMsgBase : MsgBase {

    public byte[] buffer;

    public NetMsgBase() { 
        
    }

    public NetMsgBase(byte[] byteArr) {
        buffer = byteArr;
        msgID = BitConverter.ToUInt16(byteArr, 4);
    }

    public virtual byte[] GetNetBytes() {
        return buffer;  
    }
}
