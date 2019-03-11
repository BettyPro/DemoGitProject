using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WinterDebug;

public class EventNode {
    public MonoBase data;
    public EventNode next;

    public EventNode(MonoBase tempMono) {
        this.data = tempMono;
        this.next = null;
    }
}
public class ManagerBase : MonoBase {

    public Dictionary<ushort, EventNode> eventTree = new Dictionary<ushort, EventNode>();

    public void RegistMsg(MonoBase mono,params ushort[] msgs) {
        for (int i = 0; i < msgs.Length; i++)
        {
            EventNode node = new EventNode(mono);
            RegistMsg(msgs[i], node);
            ColorDebug.Instance.RedDebug(msgs[i],"所有的UI ids",true);
            //Debug.Log(node.data);
            //Debug.Log(node.next);
        }
    }

    public void RegistMsg(ushort id, EventNode eventNode) {
        if (!eventTree.ContainsKey(id))
            eventTree.Add(id, eventNode);
        else
        {
            EventNode node = eventTree[id];
            while (node.next != null)
            {
                node = node.next;
            }
            node.next = eventNode;
        }
    }

    public void UnRegistMsg(ushort id, MonoBase node) {
        if (!eventTree.ContainsKey(id))
        {
            return;
        }
        EventNode tmp = eventTree[id];
        if (tmp.data == node)
        {
            EventNode header = tmp;
            if (header.next != null)
            {
                header.data = tmp.next.data;
                header.next = tmp.next.next;
            }
            else
            {
                eventTree.Remove(id);
            }
        }
        else
        {
            while (tmp.next != null && tmp.next.data != node)
            {
                tmp = tmp.next; 
            }
            if (tmp.next.next != null){
                tmp.next = tmp.next.next;
                tmp.next.next = null;
            }                
            else
                tmp.next = null;
        }
    }

    public void UnRegistMsg(MonoBase mono, params ushort[] args) {
        for (int i = 0; i < args.Length; i++)
        {
            UnRegistMsg(args[i], mono);
        }
    }

    public override void ProcessEvent(MsgBase tmpMsg)
    {
        //Debug.Log(111);
        //Debug.Log(eventTree.Count+"--suoyoude ");
        //for (int i = 0; i < eventTree.Count; i++)
        //{
        //    Debug.Log(eventTree.Values);
        //    Debug.Log(1);
        //}

        if (!eventTree.ContainsKey(tmpMsg.msgID))
        {
            Debug.LogError("msg not contain msgID " + tmpMsg.msgID);
            return;
        }
        EventNode tmp = eventTree[tmpMsg.msgID];

        do
        {
            tmp.data.ProcessEvent(tmpMsg);
            //Debug.Log(11);

            tmp = tmp.next;
        } while (tmp != null);
    }
}
