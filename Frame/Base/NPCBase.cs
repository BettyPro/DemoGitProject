using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    /// <summary>
    /// panel 层 用于与其他模块或者脚本通信
    /// </summary>
    public class NPCBase : MonoBase
    {
        public ushort[] msgIDs;

        public void RegistSelf(MonoBase mono, params ushort[] args)
        {
            NPCManager.Instance.RegistMsg(mono, args);
        }

        public void UnRegistSelf(MonoBase mono, params ushort[] args)
        {
            NPCManager.Instance.UnRegistMsg(mono, args);
        }

        public void SendMsg(MsgBase msg)
        {
            NPCManager.Instance.SendMsg(msg);
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
}