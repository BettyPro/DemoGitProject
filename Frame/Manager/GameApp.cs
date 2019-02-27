using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameApp : MonoBehaviour
{

    private void Awake()
    {
    }

    public void InstantGameEnter()
    {
        SetScreenRotate();

        this.gameObject.AddComponent<AudioSource>();
        this.gameObject.AddComponent<MsgCenter>();
        //Manager
        this.gameObject.AddComponent<UIManager>();
        this.gameObject.AddComponent<AudioManager>();
        this.gameObject.AddComponent<MonsterSpineAniManager>();
        this.gameObject.AddComponent<RoleSpineAniManager>();

        //Msg
        this.gameObject.AddComponent<AudioMsg>();
        this.gameObject.AddComponent<MonsterSpineAniMsg>();
        this.gameObject.AddComponent<RoleSpineAniMsg>();

        //Other
        this.gameObject.AddComponent<ResourcesLoadInfos>();

        
        //主界面Audio
        AudioManager.Instance.SendMsg(AudioMsgDetail.GetInstance.ChangeInfo((ushort)AudioId.mainMenu));
    }

    public void ReadFile()
    {
        Singleton<ReadJsonfiles>.Instance.ReadRoleJson();
        Singleton<ReadJsonfiles>.Instance.ReadSkillJson();
        Singleton<ReadJsonfiles>.Instance.ReadBuffJson();
        Singleton<ReadJsonfiles>.Instance.ReadAudioJson();
        Singleton<ReadJsonfiles>.Instance.ReadLevelJson();

        ResourcesLoadInfos.instance.Inis();

        //主界面Audio
        AudioManager.Instance.SendMsg(AudioMsgDetail.GetInstance.ChangeInfo((ushort)AudioId.mainMenu));
    }

    void SetScreenRotate()
    {
        Screen.orientation = ScreenOrientation.AutoRotation;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
    }

}
