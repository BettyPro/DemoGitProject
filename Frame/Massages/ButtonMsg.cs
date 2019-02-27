using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMsg : MsgBase
{


    private static ButtonMsg instance;
    public static ButtonMsg GetInstance
    {
        get
        {
            if (instance == null)
                instance = new ButtonMsg();
            return instance;
        }
    }

    public string name;
    public string des;

    public ButtonMsg() { }
    public ButtonMsg ChangeInfo(ushort tmp_id)
    {
        msgID = tmp_id;//最重要
        return instance;
    }

    public ButtonMsg ChangeInfo(ushort tmp_id,int role_id)
    {
        msgID = tmp_id;//最重要
        roleID = role_id;
        return instance;
    }

    public ButtonMsg ChangeInfo(ushort tmp_id,string attack_name)
    {
        msgID = tmp_id;//最重要
        attackName = attack_name;
        return instance;
    }

    public ButtonMsg ChangeInfo(ushort tmp_id,bool all_action)
    {
        msgID = tmp_id;//最重要
        allAction = all_action;
        return instance;
    }

    public ButtonMsg ChangeInfo(ushort tmp_id,bool all_action,bool is_loop)
    {
        msgID = tmp_id;//最重要
        allAction = all_action;
        isLoop = is_loop;
        return instance;
    }

    public ButtonMsg ChangeInfo(ushort tmp_id,int role_id,string attack_name)
    {
        msgID = tmp_id;//最重要
        roleID = role_id;
        attackName = attack_name;
        return instance;
    }

    public ButtonMsg ChangeInfo(ushort tmp_id,int role_id,bool is_loop)
    {
        msgID = tmp_id;//最重要
        roleID = role_id;
        isLoop = is_loop;
        return instance;
    }

    public ButtonMsg ChangeInfo(ushort tmp_id,int role_id,string attack_name,bool all_action)
    {
        msgID = tmp_id;//最重要
        roleID = role_id;
        attackName = attack_name;
        allAction = all_action;
        return instance;
    }

    public ButtonMsg ChangeInfo(ushort tmp_id,int role_id,string attack_name,bool all_action,bool is_loop)
    {
        msgID = tmp_id;//最重要
        roleID = role_id;
        attackName = attack_name;
        allAction = all_action;
        isLoop = is_loop;
        return instance;
    }


}
