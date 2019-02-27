using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ILoadMgr : MonoBehaviour {

    private static ILoadMgr instance;
    public static ILoadMgr Instance { get { return instance; } }

    private Dictionary<string, IABSceneMgr> sceneMgrDic;

    void Awake() {
        instance = this;

        StartCoroutine(IABManifestLoad.Instance.LoadManifest());
    }

    public void ReadConfig(string sceneName) {
        if (!sceneMgrDic.ContainsKey(sceneName))
        {
            IABSceneMgr sceneMgr = new IABSceneMgr(sceneName);
            sceneMgr.ReadConfig(sceneName);
            sceneMgrDic.Add(sceneName, sceneMgr);
        }
    }

    //资源加载
    public void LoadAsset(string sceneName, string bundleName, LoadProgress progress) {
        if (!sceneMgrDic.ContainsKey(sceneName))
        {
            ReadConfig(sceneName);
        }
        sceneMgrDic[sceneName].LoadAsset(bundleName, progress, LoadCallback);
    }

    public void LoadCallback(string sceneName, string bundleName) {
        if (sceneMgrDic.ContainsKey(sceneName))
        {
            IABSceneMgr sceneMgr = sceneMgrDic[sceneName];
            StartCoroutine(sceneMgr.LoadAssetAsyn(bundleName));
        }
        else
            Debug.LogErrorFormat("SceneName is not contain == {0}", sceneName);
    }

    #region 由下层提供功能

    public Object GetSingleResource(string sceneName, string bundleName, string resName)
    {
        if (sceneMgrDic.ContainsKey(sceneName))
        {
            return sceneMgrDic[sceneName].GetSingleResource(bundleName, resName);
        }
        else
        {
            Debug.LogErrorFormat("The sceneName == {0} has not loaded!", sceneName);
            return null;
        }
    }

    public Object[] GetMulResource(string sceneName, string bundleName, string resName)
    {
        if (sceneMgrDic.ContainsKey(sceneName))
            return sceneMgrDic[sceneName].GetMulResource(bundleName, resName);
        else
        {
            Debug.LogErrorFormat("The sceneName == {0} has not loaded!", sceneName);
            return null;
        }
    }

    #region 释放资源

    //释放单个bundle中的单个资源
    public void DisposeResObj(string sceneName, string bundleName, string resName)
    {
        if (sceneMgrDic.ContainsKey(sceneName))
            sceneMgrDic[sceneName].DisposeResObj(bundleName, resName);
        else
            Debug.LogErrorFormat("The sceneName == {0} has not loaded!", sceneName);
    }

    //释放单个bundle中的全部资源
    public void DisposeResObj(string sceneName, string bundleName)
    {
        if (sceneMgrDic.ContainsKey(sceneName))
            sceneMgrDic[sceneName].DisposeResObj(bundleName);
        else
            Debug.LogErrorFormat("The sceneName == {0} has not loaded!", sceneName);
    }

    //释放单个场景中的全部资源
    public void DisposeAllObj(string sceneName)
    {
        if (sceneMgrDic.ContainsKey(sceneName))
            sceneMgrDic[sceneName].DisposeAllObj();
        else
            Debug.LogErrorFormat("The sceneName == {0} has not loaded!", sceneName);
    }

    //释放单个场景中的单个资源包
    public void DisposeBundle(string sceneName, string bundleName)
    {
        if (sceneMgrDic.ContainsKey(sceneName))
            sceneMgrDic[sceneName].DisposeBundle(bundleName);
        else
            Debug.LogErrorFormat("The sceneName == {0} has not loaded!", sceneName);
    }

    //释放单个场景中的全部资源包
    public void DisposeAllBundle(string sceneName)
    {
        if (sceneMgrDic.ContainsKey(sceneName))
            sceneMgrDic[sceneName].DisposeAllBundle();
        else
            Debug.LogErrorFormat("The sceneName == {0} has not loaded!", sceneName);
    }

    //释放单个场景中的全部资源包和资源
    public void DisposeAllBundleAndRes(string sceneName)
    {
        if (sceneMgrDic.ContainsKey(sceneName))
            sceneMgrDic[sceneName].DisposeAllBundleAndRes();
        else
            Debug.LogErrorFormat("The sceneName == {0} has not loaded!", sceneName);
    }

    #endregion

    public void DebugAssetBundle(string sceneName)
    {
        if (sceneMgrDic.ContainsKey(sceneName))
            sceneMgrDic[sceneName].DebugAssetBundle();
        else
            Debug.LogErrorFormat("The sceneName == {0} has not loaded!", sceneName);
    }

    public bool IsLoadFinish(string sceneName, string bundleName)
    {
        if (sceneMgrDic.ContainsKey(sceneName))
            return sceneMgrDic[sceneName].IsLoadFinish(bundleName);
        else
        {
            Debug.LogErrorFormat("The sceneName == {0} has not loaded!", sceneName);
            return false;
        }
    }

    public bool IsLoadedAssetBundle(string sceneName, string bundleName)
    {
        if (sceneMgrDic.ContainsKey(sceneName))
            return sceneMgrDic[sceneName].IsLoadedAssetBundle(bundleName);
        else
        {
            Debug.LogErrorFormat("The sceneName == {0} has not loaded!", sceneName);
            return false;
        }
    }

    public string GetBundleName(string sceneName, string bundleName) {
        if (sceneMgrDic.ContainsKey(sceneName))
            return sceneMgrDic[sceneName].GetBundleName(bundleName);
        else
        {
            Debug.LogErrorFormat("The sceneName == {0} has not loaded!", sceneName);
            return null;
        }
    }

    #endregion

    void OnDestroy() {
        sceneMgrDic.Clear();
        System.GC.Collect();
    }
}
