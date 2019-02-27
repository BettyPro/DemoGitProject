using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using Spine.Unity;


public class ResourcesLoadInfos : MonoBehaviour
{
    public static ResourcesLoadInfos instance;

    private bool allRead = true;//-Me-- 音效资源整体读取

    public static int imageCounts = 10;//-Me-- battle 战斗场景速度条头像数量
    public AudioClip[] audioclips = new AudioClip[9];
    public Sprite[] roleImages = new Sprite[5];
    public Sprite[] allImages = new Sprite[imageCounts];
    public Sprite[] skillImages;
    public Image bloodImage;//血条

    public Font[] fonts;

    //-Me-- 按照文件夹读取
    //public AudioClip[] audioClip = new AudioClip[ReadJsonfiles.Instance.audioPath.Count];
    public List<AudioClip> audioClip = new List<AudioClip>();
    public Dictionary<int, AudioClip> audioClipDic = new Dictionary<int, AudioClip>();



    public SkeletonAnimation[] skeleons;
    public int[] levels = { 20001, 20002, 20003, 20004 };
    public Dictionary<int, SkeletonAnimation> skeleonDics = new Dictionary<int, SkeletonAnimation>(0);




    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// 资源读取
    /// </summary>
    public void Inis()
    {
        if (allRead)
            audioclips = Resources.LoadAll<AudioClip>(SetConfig.audioPath);
        else
            PartReadClips();

        roleImages = Resources.LoadAll<Sprite>(SetConfig.roleImagesPath);//角色场景人物展示集合
        allImages = Resources.LoadAll<Sprite>(SetConfig.allImagesPath);//战斗场景速度条头像集合
        skillImages = Resources.LoadAll<Sprite>(SetConfig.skillImagesPath);//战斗场景技能图片集合
        PackageSkeleons();
        bloodImage = Resources.Load<Image>(SetConfig.bloodShow);
        LoadFonts();


    }

    void PackageSkeleons()
    {
        if (SetConfig.GetInstance().isTest)
            skeleons = Resources.LoadAll<SkeletonAnimation>(SetConfig.monsterSpineShow);
        else
            skeleons = Resources.LoadAll<SkeletonAnimation>(SetConfig.monsterSpineAniShowTest);
        //skeleons = Resources.LoadAll<SkeletonAnimation>(SetConfig.monsterSpineAniShowTest);//战斗场景怪物集合  原来用这个
        for (int i = 0; i < skeleons.Length; i++)
        {
            skeleonDics.Add(levels[i], skeleons[i]);
        }

        foreach (KeyValuePair<int,SkeletonAnimation> item in skeleonDics)
        {
            // Debug.Log(item.Key+" ... " + item.Value);
        }
    }

    void PartReadClips()
    {
        for (int i = 0; i < ReadJsonfiles.Instance.audioPath.Count; i++)
        {
            //StringSpr(ReadJsonfiles.Instance.audioPath[i]);
            AudioClip clip = Resources.Load<AudioClip>("Audios/" + StringSpr(ReadJsonfiles.Instance.audioPath[i]));
            audioClip.Add(clip);
            audioClipDic.Add(ReadJsonfiles.Instance.audioNumOrder[i], clip);
        }

        foreach (KeyValuePair<int, AudioClip> item in audioClipDic)
        {
            Debug.Log(item.Key + "....." + item.Value);
        }
    }

    string StringSpr(string str)
    {

        string a = ".mp3";
        Regex r = new Regex(a);
        Match m = r.Match(str);
        if (m.Success)
            str = str.Replace(a, "");
        return str;
    }

    void LoadFonts()
    {
        fonts = Resources.LoadAll<Font>(SetConfig.fonts);
    }
}
