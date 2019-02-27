using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class ColorDebug : Singleton<ColorDebug>
{
    private string info = "";

    /// <summary>
    /// 第一种
    /// </summary>
    /// <param name="str"></param>
    /// <param name="backupsStr"></param>
    /// <param name="isLog"></param>
    public void GreenDebug(object str = null, string backupsStr = "----->", bool isLog = true)
    {
        if (SetConfig.GetInstance().IsDebug && isLog)
            Debug.Log("<color=green>" + backupsStr + ":-- </color>" + str);
    }

    public void BlueDebug(object str = null, string backupsStr = "----->", bool isLog = true)
    {
        if (SetConfig.GetInstance().IsDebug && isLog)
            Debug.Log("<color=cerulean blue>" + backupsStr + ":-- </color>" + str);
    }

    public void YellowDebug(object str = null, string backupsStr = "----->", bool isLog = true)
    {
        if (SetConfig.GetInstance().IsDebug && isLog)
            Debug.Log("<color=yellow>" + backupsStr + ":-- </color>" + str);
    }

    public void RedDebug(object str = null, string backupsStr = "----->", bool isLog = true)
    {
        if (SetConfig.GetInstance().IsDebug && isLog)
            Debug.Log("<color=red>" + backupsStr + ":-- </color>" + str);
    }

    

    public void CeruleanBlueDebug(object str = null, string backupsStr = "----->", bool isLog = true)
    {
        if (SetConfig.GetInstance().IsDebug && isLog)
            Debug.Log("<color=blue>" + backupsStr + ":-- </color>" + str);
    }

    public void BlackDebug(object str = null, string backupsStr = "----->", bool isLog = true)
    {
        if (SetConfig.GetInstance().IsDebug && isLog)
            Debug.Log("<color=black>" + backupsStr + ":-- </color>" + str);
    }

    public void DicDebug<T,Y>(Dictionary<T, Y> dics,bool isLog = true)
    {
        if (SetConfig.GetInstance().IsDebug && isLog)
        {
            foreach (KeyValuePair<T, Y> item in dics)
            {
                Debug.Log("<color=red>" + item.Key + "------" + item.Value + ":-- </color>");
            }
        }
    }

    public void ListDebug<T>(List<T> dics, bool isLog = true,string str = "各个元素依次是：-- ")
    {
        if (SetConfig.GetInstance().IsDebug && isLog)
        {
            for (int i = 0; i < dics.Count; i++)
            {
                Debug.Log(str + dics[i]);
            }
        }
    }

    public void ListDebugSpine<SkeletonAnimation>(List<SkeletonAnimation> dics, bool isLog = true)
    {
        if (SetConfig.GetInstance().IsDebug && isLog)
        {
            for (int i = 0; i < dics.Count; i++)
            {
                Debug.Log("各个元素依次是：-- " + dics[i]);
            }
        }
    }

    public void ArrayDebug<T>(T[] dics, bool isLog = true)
    {
        if (SetConfig.GetInstance().IsDebug && isLog)
        {
            for (int i = 0; i < dics.Length; i++)
            {
                Debug.Log("各个元素依次是：-- " + dics[i]);
            }
        }
    }


    /// <summary>
    /// 第二种
    /// </summary>
    /// <param name="str"></param>
    /// <param name="backupsStr"></param>
    /// <param name="isLog"></param>
    /// <returns></returns>
    public string GreenDebugInfo(object str = null, string backupsStr = "----->", bool isLog = true)
    {
        if (SetConfig.GetInstance().IsDebug && isLog)
            info = "<color=green>" + backupsStr + ":-- </color>" + str;

        return info;
    }

    public string BlueDebugInfo(object str = null, string backupsStr = "----->", bool isLog = true)
    {
        if (SetConfig.GetInstance().IsDebug && isLog)
            info = "<color=cerulean blue>" + backupsStr + ":-- </color>" + str;

        return info;
    }

    public string YellowDebugInfo(object str = null, string backupsStr = "----->", bool isLog = true)
    {
        if (SetConfig.GetInstance().IsDebug && isLog)
            info = "<color=yellow>" + backupsStr + ":-- </color>" + str;

        return info;
    }

    public string RedDebugInfo(object str = null, string backupsStr = "----->", bool isLog = true)
    {
        if (SetConfig.GetInstance().IsDebug && isLog)

        {
            info = "<color=red>" + backupsStr + ":-- </color>" + str;

        }
        else
        {
            Debug.DebugBreak();
            info = "";
        }

        return info;
    }



    public string CeruleanBlueDebugInfo(object str = null, string backupsStr = "----->", bool isLog = true)
    {
        if (SetConfig.GetInstance().IsDebug && isLog)
            info = "<color=blue>" + backupsStr + ":-- </color>" + str;

        return info;
    }

    public string BlackDebugInfo(object str = null, string backupsStr = "----->", bool isLog = true)
    {
        if (SetConfig.GetInstance().IsDebug && isLog)
            info = "<color=black>" + backupsStr + ":-- </color>" + str;

        return info;
    }


   

    //带默认值的参数
    public void EnterScene(string sceneName, bool showLoading = false, bool autoSwitch = false, string loadingPrefab = "", string abName = "")
    {
    }
}
