using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class MsgCenter : MonoBase
    {

        public static MsgCenter Instance = null;


        void Awake()
        {
            Instance = this;
            //gameObject.AddComponent<UIManager>();
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SendToMsg(MsgBase tmpMsg)
        {
            AnasysisMsg(tmpMsg);
        }

        private void AnasysisMsg(MsgBase tmpMsg)
        {
            ManagerID tmpID = tmpMsg.GetManagerID();
            Debug.Log(tmpID + "---");
            switch (tmpID)
            {
                case ManagerID.GameManager:
                    ProcessEvent(tmpMsg);
                    break;
                case ManagerID.UIManager:
                    ProcessEvent(tmpMsg);
                    break;
                case ManagerID.AudioManager:
                    AudioMsg msg = UnitySingleton<GameApp>.Instance.GetComponent<AudioMsg>();
                    msg.ProcessEvent(tmpMsg);
                    break;
                case ManagerID.NPCManager:
                    break;
                case ManagerID.CharactorManager:
                    break;
                case ManagerID.AssetsManager:
                    ProcessEvent(tmpMsg);
                    break;
                case ManagerID.NetManager:
                    break;
                case ManagerID.RoleSpineAniManager:
                    RoleSpineAniMsg roleSpineMsg = UnitySingleton<GameApp>.Instance.GetComponent<RoleSpineAniMsg>();
                    roleSpineMsg.ProcessEvent(tmpMsg);
                    break;
                case ManagerID.MonsterSpineAniManager:
                    MonsterSpineAniMsg MonsterSpineAniMsg =
                        UnitySingleton<GameApp>.Instance.GetComponent<MonsterSpineAniMsg>();
                    MonsterSpineAniMsg.ProcessEvent(tmpMsg);
                    break;
                default:
                    break;
            }
        }

        public override void ProcessEvent(MsgBase tmpMsg)
        {
            Debug.Log(121212);
            //throw new System.NotImplementedException();

        }
    }
}