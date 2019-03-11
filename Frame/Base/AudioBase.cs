using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    /// <summary>
    /// panel 层 用于与其他模块或者脚本通信
    /// </summary>
    public class AudioBase : MonoBase
    {
        public ushort[] msgIDs;

        public void RegistSelf(MonoBase mono, params ushort[] args)
        {
            AudioManager.Instance.RegistMsg(mono, args);
        }

        public void UnRegistSelf(MonoBase mono, params ushort[] args)
        {
            AudioManager.Instance.UnRegistMsg(mono, args);
        }

        public void SendMsg(MsgBase msg)
        {
            AudioManager.Instance.SendMsg(msg);
        }

        public override void ProcessEvent(MsgBase tmpMsg)
        {
            //throw new System.NotImplementedException();
            Debug.Log(44);
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