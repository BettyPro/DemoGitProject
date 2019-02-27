using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpineAniBase : MonoBase
{

    public ushort[] msgIDs;
    public void RegistSelf(MonoBase mono, params ushort[] args)
    {
        MonsterSpineAniManager.Instance.RegistMsg(mono, args);
    }

    public void UnRegistSelf(MonoBase mono, params ushort[] args)
    {
        MonsterSpineAniManager.Instance.UnRegistMsg(mono, args);
    }

    public void SendMsg(MsgBase msg)
    {
        MonsterSpineAniManager.Instance.SendMsg(msg);
    }

    public override void ProcessEvent(MsgBase tmpMsg)
    {
        Debug.Log(11);

        //throw new System.NotImplementedException();
    }

    void OnDestory()
    {
        if (msgIDs != null)
        {
            UnRegistSelf(this, msgIDs);
        }
    }
}
