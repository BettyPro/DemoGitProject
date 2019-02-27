using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IABSceneMgr {

    private Dictionary<string, string> allAsset;

    IABMgr abManager;

    public IABSceneMgr(string sceneName) {
        allAsset = new Dictionary<string,string>();
        abManager = new IABMgr(sceneName);
    }

    public void ReadConfig(string fileName) {
        string path = IPathTools.GetAssetBundlePath() + '/' + fileName + "Record.txt";
        ReadFromPath(path);
    }

    private void ReadFromPath(string path) {
        FileStream fs = new FileStream(path, FileMode.Open);
        StreamReader sr = new StreamReader(fs);
        string line = sr.ReadLine();
        int count = int.Parse(line);
        string tmpStr = "";
        string[] tmpArr;
        for (int i = 0; i < count; i++)
        {
            tmpStr = sr.ReadLine();
            tmpArr = tmpStr.Split(" ".ToCharArray());
            allAsset.Add(tmpArr[0], tmpArr[1]);
        }
        sr.Close();
        fs.Close();
    }

    public void LoadAsset(string bundleName, LoadProgress progress, LoadAssetBundleCallBack callback) {
        if (allAsset.ContainsKey(bundleName))
        {
            string tmpStr = allAsset[bundleName];
            abManager.LoadAssetBundle(bundleName, progress, callback);
        }
        else
            Debug.LogErrorFormat("Don't contain the bundle == {0}", bundleName);
    }

    #region 由下层提供功能

    public IEnumerator LoadAssetAsyn(string bundleName) {
        yield return abManager.LoadAssetBundles(bundleName);
    }

    public Object GetSingleResource(string bundleName, string resName) {
        if (allAsset.ContainsKey(bundleName))
            return abManager.GetSingleResource(allAsset[bundleName], resName);
        else
        {
            Debug.LogErrorFormat("Don't contain the bundle == {0}", bundleName);
            return null;
        }
    }

    public Object[] GetMulResource(string bundleName, string resName) {
        if (allAsset.ContainsKey(bundleName))
            return abManager.GetMulResource(allAsset[bundleName], resName);
        else
        {
            Debug.LogErrorFormat("Don't contain the bundle =={0}", bundleName);
            return null;
        }
    }

    #region 释放资源

    //释放单个bundle中的单个资源
    public void DisposeResObj(string bundleName, string resName)
    {
        if (allAsset.ContainsKey(bundleName))
            abManager.DisposeResObj(allAsset[bundleName], resName);
        else
            Debug.LogErrorFormat("Don't contain the bundle =={0}", bundleName);
    }

    //释放单个bundle中的全部资源
    public void DisposeResObj(string bundleName)
    {
        if (allAsset.ContainsKey(bundleName))
            abManager.DisposeResObj(allAsset[bundleName]);
        else
            Debug.LogErrorFormat("Don't contain the bundle =={0}", bundleName);
    }

    public void DisposeAllObj()
    {
        abManager.DisposeAllObj();
    }

    public void DisposeBundle(string bundleName) {
        if (allAsset.ContainsKey(bundleName))
            abManager.DisposeBundle(bundleName);
        else
            Debug.LogErrorFormat("Don't contain the bundle =={0}", bundleName);
    }

    public void DisposeAllBundle() {
        abManager.DisposeAllBundle();
        allAsset.Clear();
    }

    public void DisposeAllBundleAndRes() {
        abManager.DisposeAllBundleAndRes();
        allAsset.Clear();
    }

    #endregion

    public void DebugAssetBundle() {
        List<string> keys = new List<string>();
        keys.AddRange(allAsset.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            abManager.DebugAssetBundle(allAsset[keys[i]]);
        }
    }

    public bool IsLoadFinish(string bundleName)
    {
        if (allAsset.ContainsKey(bundleName))
            return abManager.IsLoadFinish(allAsset[bundleName]);
        else
        {
            Debug.LogErrorFormat("Don't contain the bundle =={0}", bundleName);
            return false;
        }
    }

    public bool IsLoadedAssetBundle(string bundleName)
    {
        if (allAsset.ContainsKey(bundleName))
            return abManager.IsLoadedAssetBundle(allAsset[bundleName]);
        else
        {
            Debug.LogErrorFormat("Don't contain the bundle =={0}", bundleName);
            return false;
        }
    }

    public string GetBundleName(string bundleName) {
        if (allAsset.ContainsKey(bundleName))
            return allAsset[bundleName];
        else
        {
            Debug.LogErrorFormat("Don't contain the bundle =={0}", bundleName);
            return null;
        }
    }

    #endregion
}
