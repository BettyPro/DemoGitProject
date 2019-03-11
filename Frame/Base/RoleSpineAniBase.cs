using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class RoleSpineAniBase : MonoBase
    {

        public ushort[] msgIDs;

        public void RegistSelf(MonoBase mono, params ushort[] args)
        {
            RoleSpineAniManager.Instance.RegistMsg(mono, args);
        }

        public void UnRegistSelf(MonoBase mono, params ushort[] args)
        {
            RoleSpineAniManager.Instance.UnRegistMsg(mono, args);
        }

        public void SendMsg(MsgBase msg)
        {
            RoleSpineAniManager.Instance.SendMsg(msg);
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