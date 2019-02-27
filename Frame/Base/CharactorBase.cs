using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// panel 层 用于与其他模块或者脚本通信
/// </summary>
public class CharactorBase : MonoBase
{
    public ushort[] msgIDs;
    public void RegistSelf(MonoBase mono, params ushort[] args) {
        CharactorManager.Instance.RegistMsg(mono, args);
    }

    public void UnRegistSelf(MonoBase mono, params ushort[] args) {
        CharactorManager.Instance.UnRegistMsg(mono, args);
    }

    public void SendMsg(MsgBase msg) {
        CharactorManager.Instance.SendMsg(msg);
    }

    public override void ProcessEvent(MsgBase tmpMsg)
    {
        //throw new System.NotImplementedException();
    }

    void OnDestory() {
        if (msgIDs != null)
        {
            UnRegistSelf(this, msgIDs);
        }
    }
}
