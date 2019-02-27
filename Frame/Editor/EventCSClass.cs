using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCSClass : UIBase {

    public override void ProcessEvent(MsgBase tmpMsg)
    {
        Debug.Log(11);

        switch (tmpMsg.msgID)
        {
            case (ushort)0:

                break;

            default:
                break;
        }
    }

    void Awake() {
        msgIDs = new ushort[] { 
            
        };

        RegistSelf(this, msgIDs);
    }

    void Start() { 
        
    }

    void Update() { 
        
    }
}
