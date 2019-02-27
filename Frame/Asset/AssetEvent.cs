using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AssetEvent
{
    HunkRes = ManagerID.AssetsManager + 1,
    ReleaseSingleObj,
    ReleaseBundleObj,
    ReleaseSceneObj,

    ReleaseSingleBundle,
    ReleaseSceneBundle,
    ReleaseAll
}

//上层需要发的
public class AssetRes : MsgBase {

    public string sceneName;
    public string bundleName;
    public string resName;

    public bool isSingle;

    public ushort backMsgID;

    public AssetRes(string tmpSceneName, string tmpBundleName, string tmpResName, bool tmpIsSingle, ushort tmpBackMsgID, ushort tmpMsgID)
    {
        this.sceneName = tmpSceneName;
        this.bundleName = tmpBundleName;
        this.resName = tmpResName;
        this.isSingle = tmpIsSingle;
        this.backMsgID = tmpBackMsgID;
        this.msgID = tmpMsgID;
    }
}

//返回给上层的消息
public class AssetResBack : MsgBase {

    public Object[] value;

    public AssetResBack() {
        msgID = 0;
        value = null;
    }

    public void Changer(ushort msgID, params Object[] tmpValue) {
        this.msgID = msgID;
        this.value = tmpValue;
    }

    public void Changer(ushort msgID) {
        this.msgID = msgID;
    }

    public void Changer(params Object[] tmpValue) {
        this.value = tmpValue;
    }
}
