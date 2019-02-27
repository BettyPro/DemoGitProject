using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IABResLoad:IDisposable
{
    private AssetBundle ABRes;

    public IABResLoad(AssetBundle tmpBundle)
    {
        ABRes = tmpBundle;
    }

    /// <summary>
    /// 加载单个资源
    /// </summary>
    /// <param name="resName"></param>
    /// <returns></returns>
    public UnityEngine.Object this[string resName]
    {
        get {
            if (this.ABRes == null || !this.ABRes.Contains(resName)) {
                return null;
            }
            return ABRes.LoadAsset(resName);
        }
    }

    /// <summary>
    /// 加载多个资源
    /// </summary>
    /// <param name="resName"></param>
    /// <returns></returns>
    public UnityEngine.Object[] LoadResources(string resName)
    {
        if (this.ABRes == null || !this.ABRes.Contains(resName))
        {
            return null;
        }
        return this.ABRes.LoadAssetWithSubAssets(resName);
    }

    /// <summary>
    /// 卸载单个资源
    /// </summary>
    /// <param name="resObj"></param>
    public void UnLoadRes(UnityEngine.Object resObj)
    {
        Resources.UnloadAsset(resObj);
    }

    /// <summary>
    /// 释放assetbundle
    /// </summary>
    public void Dispose() {
        if (this.ABRes == null)
            return;
        ABRes.Unload(false);
    }

    /// <summary>
    /// 调试打印
    /// </summary>
    public void DebugAllRes() {
        string[] tmpAssetName = ABRes.GetAllAssetNames();

        for (int i = 0; i < tmpAssetName.Length; i++)
        {
            Debug.Log("ABRes Contain Assetname == " + tmpAssetName[i]);
        }
    }
}
