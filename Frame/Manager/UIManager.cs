using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WinterColorDebug;
using WinterDebug;

namespace Demo
{
    public class UIManager : ManagerBase
    {


        public static UIManager Instance = new UIManager();

        Dictionary<string, GameObject> sonMembers = new Dictionary<string, GameObject>();

        void Awake()
        {
            Instance = this;
        }

        public void SendMsg(MsgBase msg)
        {
            ColorDebug.Instance.RedDebug(msg.GetManagerID(), "消息发过来的id", true);
            if (msg.GetManagerID() == ManagerID.UIManager)
                ProcessEvent(msg);
            else
                MsgCenter.Instance.SendToMsg(msg);
        }

        public void RegistGameObject(string name, GameObject obj)
        {
            if (!sonMembers.ContainsKey(name))
                sonMembers.Add(name, obj);
            for (int i = 0; i < sonMembers.Count; i++)
            {
                //Debug.Log("<color=green>" + "所有注册的UI成员：--" + "</color>" + sonMembers[name]);
                ColorDebug.GetInstance().CeruleanBlueDebug(sonMembers[name], "所有注册的UI成员", false);
            }

            foreach (KeyValuePair<string, GameObject> pair in sonMembers)
            {
                //Debug.Log(pair.Key + "     " + pair.Value);
            }
        }

        public GameObject GetGameObject(string name)
        {
            if (sonMembers.ContainsKey(name))
                return sonMembers[name];
            return null;
        }

        public void UnRegistGameObject(string name)
        {
            if (sonMembers.ContainsKey(name))
                sonMembers.Remove(name);
        }
    }
}