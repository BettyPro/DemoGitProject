using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// panel 层 用于与其他模块或者脚本通信
/// </summary>
public class NetBase : MonoBase
{
    public ushort[] msgIDs;
    public void RegistSelf(MonoBase mono, params ushort[] args) {
        NetManager.Instance.RegistMsg(mono, args);
    }

    public void UnRegistSelf(MonoBase mono, params ushort[] args) {
        NetManager.Instance.UnRegistMsg(mono, args);
    }

    public void SendMsg(MsgBase msg) {
        NetManager.Instance.SendMsg(msg);
    }

    public override void ProcessEvent(MsgBase tmpMsg)
    {
        //throw new System.NotImplementedException();
        Debug.Log(11);

    }

    void OnDestory() {
        if (msgIDs != null)
        {
            UnRegistSelf(this, msgIDs);
        }
    }
}
