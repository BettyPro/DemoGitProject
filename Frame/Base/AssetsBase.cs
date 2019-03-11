using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class AssetsBase : MonoBase
    {
        public ushort[] msgIDs;

        public void RegistSelf(MonoBase mono, params ushort[] args)
        {
            AssetsManager.Instance.RegistMsg(mono, args);
        }

        public void UnRegistSelf(MonoBase mono, params ushort[] args)
        {
            AssetsManager.Instance.UnRegistMsg(mono, args);
        }

        public void SendMsg(MsgBase msg)
        {
            AssetsManager.Instance.SendMsg(msg);
        }

        public override void ProcessEvent(MsgBase tmpMsg)
        {
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