using System.Collections;
using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Spine.Unity;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class ResourcesLoadBattleSceneInfos {
    /// <summary>
    /// 技能
    /// </summary>
    public Sprite[] skillImages;
    public Dictionary<int, Sprite> skillImageDics;
    /// <summary>
    /// 行动头像
    /// </summary>
    public Sprite[] roleActionImages;
    public Dictionary<string, Sprite> roleActionImagesDics;
    /// <summary>
    /// buff
    /// </summary>
    public Sprite[] roleBuffImages;
    public Dictionary<string, Sprite> roleBuffImagesDics;
    /// <summary>
    /// 伤害数字
    /// </summary>
    public Object[] damageValuesShow; //伤害数字图片
    public Object[] critValuesShow; //伤害数字图片
    public Object[] bloodValuesShows; //伤害数字图片
    /// <summary>
    /// spine特效
    /// </summary>
    public SkeletonDataAsset[] spineEffects;
    public Dictionary<string, SkeletonDataAsset> spineEffectsDic = new Dictionary<string, SkeletonDataAsset> ();
    /// <summary>
    /// spine特效Pre
    /// </summary>
    public SkeletonAnimation spineEffectsPre;
    /// <summary>
    /// 资源读取
    /// </summary>
    public void Inis () {

        LoadSkillImages ();
        LoadRoleActionImages ();
        LoadBuff ();
        LoadValues ();
        LoadSpineEffects ();
        LoadSpineEffectsPre();
    }

    void LoadSkillImages () {
        skillImageDics = new Dictionary<int, Sprite> ();
        skillImages = Resources.LoadAll<Sprite> (SetConfig.skillImagesPath); //战斗场景技能图片集合
        for (int i = 0; i < skillImages.Length; i++) {
            skillImageDics.Add (int.Parse (skillImages[i].name), skillImages[i]);
        }
    }

    void LoadRoleActionImages () {
        roleActionImagesDics = new Dictionary<string, Sprite> ();
        roleActionImages = Resources.LoadAll<Sprite> (SetConfig.roleActionImagesPath); //战斗场景行动头像集合
        for (int i = 0; i < roleActionImages.Length; i++) {
            roleActionImagesDics.Add (roleActionImages[i].name, roleActionImages[i]);
        }
    }

    void LoadBuff () {
        roleBuffImagesDics = new Dictionary<string, Sprite> ();
        roleBuffImages = Resources.LoadAll<Sprite> (SetConfig.roleBuffImagesPath); //战斗场景buff图片集合
        for (int i = 0; i < roleBuffImages.Length; i++) {
            roleBuffImagesDics.Add (roleBuffImages[i].name, roleBuffImages[i]);
        }
    }

    void LoadValues () {
#if UNITY_EDITOR
        damageValuesShow = AssetDatabase.LoadAllAssetsAtPath (SetConfig.damageValueImagesPath);
        critValuesShow = AssetDatabase.LoadAllAssetsAtPath (SetConfig.critValueImagesPath);
        bloodValuesShows = AssetDatabase.LoadAllAssetsAtPath (SetConfig.bloodValueImagesPath);
        damageValuesShow = DeleteSomeValue (damageValuesShow);
        critValuesShow = DeleteSomeValue (critValuesShow);
        bloodValuesShows = DeleteSomeValue (bloodValuesShows);
#else
        damageValuesShow = Resources.LoadAll (SetConfig.damageValueImagesPath1);
        critValuesShow = Resources.LoadAll (SetConfig.critValueImagesPath1);
        bloodValuesShows = Resources.LoadAll (SetConfig.bloodValueImagesPath1);
        damageValuesShow = DeleteSomeValue (damageValuesShow);
        critValuesShow = DeleteSomeValue (critValuesShow);
        bloodValuesShows = DeleteSomeValue (bloodValuesShows);
#endif
        ColorDebug.Instance.ArrayDebug<Object> (damageValuesShow, false);
        ColorDebug.Instance.ArrayDebug<Object> (critValuesShow, false);
        ColorDebug.Instance.ArrayDebug<Object> (bloodValuesShows, false);
    }

    void LoadSpineEffects () {
        spineEffects = Resources.LoadAll<SkeletonDataAsset>(SetConfig.spineStrikeBack1);
        if(spineEffects == null)return;
        for (int i = 0; i < spineEffects.Length; i++) {
            spineEffectsDic.Add (spineEffects[i].name, spineEffects[i]);
        }
        ColorDebug.Instance.DicDebug<string,SkeletonDataAsset>(spineEffectsDic,true);
    }

    void LoadSpineEffectsPre () {
        spineEffectsPre = Resources.Load<SkeletonAnimation>(SetConfig.spineStrikeBackPre);
        if(spineEffectsPre == null)
        {
            Debug.Log("is null");
            return;
        }
    }

    /// <summary>
    /// tools
    /// </summary>
    /// <param name="objs"></param>
    /// <param name="pathName"></param>
    /// <returns></returns>
    Object[] ResourLoadAll (Object[] objs, string pathName) {
        objs = Resources.LoadAll (pathName);
        return objs;
    }

    Object ResourLoad<T> (Object obj, string pathName) {
        obj = Resources.Load (pathName, typeof (T));
        return obj;
    }

    Object[] DeleteSomeValue (Object[] objs) {
        Object[] temp = null;
        if(objs != null)
            temp = new Object[objs.Length];
        else
        {
            Debug.LogError("资源没有正确加载！");
            return null;
        } 
        int k = 0;
        for (int i = 0; i < objs.Length; i++) {
            temp[i] = objs[i];
        }
        objs = new Object[temp.Length - 1];
        for (int i = 1; i < temp.Length; i++) {
            objs[k] = temp[i];
            k++;
        }
        return objs;
    }

    
    void LoadValues1 () {
        DirectoryInfo rootDirInfo = new DirectoryInfo (Application.dataPath + "/Resources/" + SetConfig.bloodValueImagesPath);
        if (rootDirInfo != null) {
            // foreach (DirectoryInfo dirInfo in rootDirInfo.GetDirectories ()) {
            foreach (FileInfo fi in rootDirInfo.GetFiles ("*.png", SearchOption.AllDirectories)) {
                Debug.Log (fi.FullName);
                string allPath = fi.FullName;
                string assetPath = allPath.Substring (allPath.IndexOf ("Assets"));
                Debug.Log ("assetPath:--" + assetPath);
                // bloodValuesShow1 = AssetDatabase.LoadAssetAtPath<Sprite> (assetPath);
                // bloodValuesShows = AssetDatabase.LoadAllAssetsAtPath (assetPath);
                // bloodValuesShowsSpr = new Sprite[bloodValuesShows.Length-1];
                // for (int i = 1; i < bloodValuesShows.Length; i++) {
                //     Debug.Log (bloodValuesShows[i].name);
                //     bloodValuesShowsSpr[i-1] = (Sprite)bloodValuesShows[i];
                // }
                // Debug.Log (bloodValuesShowsSpr.Length);
                // if (bloodValuesShowsSpr != null) {
                //     for (int i = 0; i < bloodValuesShowsSpr.Length; i++) {
                //         Debug.Log (bloodValuesShowsSpr[i].name + "--------------");
                //     }
                // }
                // if (bloodValuesShow1 != null)
                //     Debug.Log (bloodValuesShow1.name);
                // else {
                //     Debug.Log ("is null");
                // }
            }
            Debug.Log (11111111111111111111);
            // }
        } else {
            Debug.Log ("is null");
        }
    }
}