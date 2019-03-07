using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum AudioId
{
    mainMenu = ManagerID.AudioManager + 1,
    search,
    battle,
    win_loop,
    boss,
    win = ManagerID.AudioManager + 101,
    fight,
    enter,
    cancel,
    stop //-Me-- 预留处理其他逻辑
}

public enum AudioUseId
{
    battle,
    boss,
    mainMenu,
    search,
    win_loop,
    cancel,
    enter,
    fight,
    win
}

public class AudioMsgBack : MsgBase
{
    public string name;
    public ushort id;
    public string des;
}

public class AudioMsg : AudioBase
{
    private AudioSource mainSource;
    private AudioSource roleSource;
    private AudioSource battleSource;

    private void Awake()
    {
        msgIDs = new ushort[]
        {
            (ushort) AudioId.mainMenu,
            (ushort) AudioId.search,
            (ushort) AudioId.battle,
            (ushort) AudioId.win_loop,
            (ushort) AudioId.boss,
            (ushort) AudioId.win,
            (ushort) AudioId.fight,
            (ushort) AudioId.enter,
            (ushort) AudioId.cancel,
            (ushort) AudioId.stop,
        };
        RegistSelf(this, msgIDs);
    }

    public override void ProcessEvent(MsgBase tmpMsg)
    {
        switch (tmpMsg.msgID)
        {
            case (ushort)AudioId.mainMenu:
                PlayClip((ushort)AudioUseId.mainMenu);
                Debug.Log("播放主界面音乐");
                break;
            case (ushort)AudioId.search:
                PlayClip((ushort)AudioUseId.search,needWait: tmpMsg.isNeedWait);
                Debug.Log("战斗搜索时音乐");
                break;
            case (ushort)AudioId.battle:
                PlayClip((ushort)AudioUseId.battle);
                Debug.Log("遇敌时音乐");
                break;
            case (ushort)AudioId.win_loop:
                PlayClip((ushort)AudioUseId.win_loop, true, true);
                Debug.Log("胜利之后循环音乐");
                break;
            case (ushort)AudioId.boss:
                PlayClip((ushort)AudioUseId.boss, false);
                Debug.Log("遇到Boss时音乐");
                break;
            case (ushort)AudioId.win:
                PlayClip((ushort)AudioUseId.win, false);
                Debug.Log("胜利时音乐");
                break;
            case (ushort)AudioId.fight:
                PlayRoleClip((ushort)AudioUseId.fight, false);
                Debug.Log("冒险按钮音乐");
                break;
            case (ushort)AudioId.enter:
                PlayRoleClip((ushort)AudioUseId.enter, false);
                Debug.Log("确定,打开时音乐");
                break;
            case (ushort)AudioId.cancel:
                PlayRoleClip((ushort)AudioUseId.cancel, false);
                Debug.Log("取消,退出时音乐");
                break;
            case (ushort)AudioId.stop:
                StopClip();
                Debug.Log("停止某音乐(特殊处理)");
                break;
        }
    }

    void PlayRoleClip(int count, bool isLoop = true)
    {
        if (SceneManager.GetActiveScene().name.Equals(SetConfig.sceneRole))
        {
            roleSource = GameObject.Find("GameRole").GetComponent<AudioSource>();
        }
        else if (SceneManager.GetActiveScene().name.Equals(SetConfig.sceneBattle))
        {
            roleSource = GameObject.Find("GameBattle").GetComponent<AudioSource>();
        }
        //roleSource = GameObject.Find("GameRole").GetComponent<AudioSource>();
        if (roleSource.isPlaying)
        {
            roleSource.Stop();
        }
        roleSource.clip = ResourcesLoadInfos.instance.audioclips[count];
        roleSource.loop = isLoop;
        roleSource.volume = 0.8f;
        roleSource.Play();
    }

    /// <summary>
    /// audioclips 0->battle 1->boss 2->main-menu 3->search 4->win_loop
    /// 5->cancel 6->enter 7->foght 8->win
    /// </summary>
    /// <param name="count"></param>
    /// <param name="isLoop"></param>
    void PlayClip(int count, bool isLoop = true, bool needWait = false, float volume = 0.3f)
    {
        
        mainSource = UnitySingleton<GameApp>.Instance.GetComponent<AudioSource>();
        if (needWait)
        {
            // StartCoroutine("FixClip",count);
            StartCoroutine(FixClip(count));
        }
        else
        {
            if (mainSource.isPlaying)
            {
                mainSource.Stop();
            }
            mainSource.clip = ResourcesLoadInfos.instance.audioclips[count];
            mainSource.loop = isLoop;
            mainSource.volume = volume;
            mainSource.Play();
        }
      
    }

    public IEnumerator FixClip(int count)
    {
        yield return new WaitForSeconds(mainSource.clip.length);
        mainSource.clip = ResourcesLoadInfos.instance.audioclips[count];
        mainSource.loop = true;
        mainSource.volume = 0.5f;
        mainSource.Play();
    }

    void StopClip()
    {
        mainSource = UnitySingleton<GameApp>.Instance.GetComponent<AudioSource>();
        mainSource.Stop();
    }
}
