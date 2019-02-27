using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//每一帧的回调
public delegate void LoadProgress(string bundle, float progress);
//加载完成了回调
public delegate void LoadFinish(string bundle);

public class IABLoad{
    private string bundleName;
    private string commonBundlePath;
    private WWW commonLoader;
    private float commonResLoadProgress;

    private LoadProgress loadProgress;
    private LoadFinish loadFinish;

    private IABResLoad resLoad;
    
    public IABLoad(LoadProgress tmpProgress, LoadFinish tmpFinish){
        bundleName = "";
        commonBundlePath = "";
        commonResLoadProgress = 0;
        loadProgress = tmpProgress;
        loadFinish = tmpFinish;
        resLoad = null;
    }

    /// <summary>
    /// 设置bundle包名
    /// </summary>
    /// <param name="bundle"></param>
    public void SetBundleName(string bundle) {
        this.bundleName = bundle;
    }

    /// <summary>
    /// 要求上层传递完整路径
    /// </summary>
    /// <param name="path"></param>
    public void LoadResources(string path) {
        commonBundlePath = path;
    }

    public IEnumerator CommonLoad() {
        commonLoader = new WWW(commonBundlePath);

        while (!commonLoader.isDone)
        {
            commonResLoadProgress = commonLoader.progress;

            if (loadProgress != null)
                loadProgress(bundleName, commonResLoadProgress);

            yield return commonLoader.progress;
            commonResLoadProgress = commonLoader.progress;
        }

        if (commonResLoadProgress >= 1.0f)
        {
            resLoad = new IABResLoad(commonLoader.assetBundle);
            if (loadProgress != null) {
                loadProgress(bundleName, commonResLoadProgress);
            }                
            if(loadFinish != null)
                this.loadFinish(bundleName);            
        }
        else
            Debug.LogError("Load bundle error ==" + bundleName);
        commonLoader = null;
    }    

    #region 下层提供功能

    public void DebugLoad()
    {
        if (commonLoader != null)
            resLoad.DebugAllRes();
    }

    /// <summary>
    /// 获取单一资源
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public UnityEngine.Object GetResources(string name)
    {
        if (resLoad == null)
            return null;
        return resLoad[name];
    }

    /// <summary>
    /// 获取多个资源
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public UnityEngine.Object[] GetMulRes(string name) {
        if (resLoad == null)
            return null;
        return resLoad.LoadResources(name);
    }

    /// <summary>
    /// 卸载单一资源
    /// </summary>
    /// <param name="tmpObj"></param>
    public void UnLoadAssetRes(UnityEngine.Object tmpObj) {
        if (resLoad != null)
            resLoad.UnLoadRes(tmpObj);
    }

    /// <summary>
    /// 释放功能
    /// </summary>
    public void Dispose() {
        if (resLoad != null)
        {
            resLoad.Dispose();
            resLoad = null;
        }
    }
    #endregion
}
