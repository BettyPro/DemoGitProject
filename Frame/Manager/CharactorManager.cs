using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorManager : ManagerBase
{
    public static CharactorManager Instance = null;

    Dictionary<string,GameObject> sonMembers = new Dictionary<string,GameObject>();

    void Awake() {
        Instance = this;
    }

    public void SendMsg(MsgBase msg) {
        if (msg.GetManagerID() == ManagerID.CharactorManager)
        {
            ProcessEvent(msg);
        }
        else
            MsgCenter.Instance.SendToMsg(msg);
    }

    public void RegistGameObject(string name, GameObject obj) {
        if (!sonMembers.ContainsKey(name))
        {
            sonMembers.Add(name, obj);
        }
    }

    public GameObject GetGameObject(string name) {
        if (sonMembers.ContainsKey(name))
        {
            return sonMembers[name];
        }
        return null;
    }

    public void UnRegistGameObject(string name) {
        if (sonMembers.ContainsKey(name))
        {
            sonMembers.Remove(name);
        }
    }
}
