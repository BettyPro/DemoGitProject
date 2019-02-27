using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void LoadAssetBundleCallBack(string sceneName, string bundleName);
/// <summary>
/// 单个场景中所有的bundle包的管理
/// </summary>
public class IABMgr {

    Dictionary<string, IABRelationMgr> loadHelper = new Dictionary<string, IABRelationMgr>();
    Dictionary<string, AssetResObj> loadObj = new Dictionary<string, AssetResObj>();

    string sceneName;

    public IABMgr(string tmpSceneName) {
        sceneName = tmpSceneName;
    }

    /// <summary>
    /// 是否加载了bundle
    /// </summary>
    /// <param name="bundleName"></param>
    /// <returns></returns>
    public bool IsLoadedAssetBundle(string bundleName)
    {
        if (!loadHelper.ContainsKey(bundleName))
        {
            return false;
        }
        return true;
    }

    #region 释放缓存物体

    public void DisposeResObj(string bundleName, string resName) {
        if (loadObj.ContainsKey(bundleName))
        {
            AssetResObj tmpObj = loadObj[bundleName];
            tmpObj.ReleaseResObj(resName);
        }
    }

    public void DisposeResObj(string bundleName) {
        if (loadObj.ContainsKey(bundleName))
        {
            AssetResObj tmpObj = loadObj[bundleName];
            tmpObj.ReleaseAllResOjbs();
        }
        Resources.UnloadUnusedAssets();
    }

    public void DisposeAllObj() {
        List<string> keys = new List<string>();
        keys.AddRange(loadObj.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            DisposeResObj(keys[i]);
        }
        loadObj.Clear();
    }

    public void DisposeBundle(string bundleName) {
        if (loadHelper.ContainsKey(bundleName))
        {
            IABRelationMgr load = loadHelper[bundleName];
            List<string> dependences = load.GetDependence();
            string tmpStr = "";
            for (int i = 0; i < dependences.Count; i++)
            {
                tmpStr = dependences[i];
                if (loadHelper.ContainsKey(tmpStr))
                {
                    IABRelationMgr dependence = loadHelper[tmpStr];
                    if (dependence.RemoveReference(bundleName))
                    {
                        DisposeBundle(dependence.BundleName);
                    }
                }
            }
            if (load.GetReference().Count <= 0)
            {
                load.Dispose();
                loadHelper.Remove(bundleName);
            }
        }
    }

    public void DisposeAllBundle() {
        List<string> keys = new List<string>();
        keys.AddRange(loadHelper.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            IABRelationMgr load = loadHelper[keys[i]];
            load.Dispose();
        }
        loadHelper.Clear();
    }

    public void DisposeAllBundleAndRes() {
        DisposeAllObj();
        List<string> keys = new List<string>();
        keys.AddRange(loadHelper.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            IABRelationMgr load = loadHelper[keys[i]];
            load.Dispose();
        }
        loadHelper.Clear();
    }

    #endregion

    public string[] GetDependence(string bundleName) {
        return IABManifestLoad.Instance.GetDependence(bundleName);
    }

    public void LoadAssetBundle(string bundleName, LoadProgress progress, LoadAssetBundleCallBack callback) {
        if (!loadHelper.ContainsKey(bundleName))
        {
            IABRelationMgr load = new IABRelationMgr();
            load.Initial(bundleName, progress);
            loadHelper.Add(bundleName, load);
            callback(sceneName, bundleName);
        }
        else
        {
            Debug.Log("IABMgr has contain bundleName == " + bundleName);
        }
    }

    public IEnumerator LoadAssetBundleDependence(string bundleName, string refName, LoadProgress progress) {
        if (!loadHelper.ContainsKey(bundleName))
        {
            IABRelationMgr load = new IABRelationMgr();
            load.Initial(bundleName, progress);
            if (refName != null)
            {
                load.AddReference(refName);
            }
            loadHelper.Add(bundleName, load);
            yield return LoadAssetBundles(bundleName);
        }
        else
        {
            if (refName != null)
            {
                IABRelationMgr load = loadHelper[bundleName];
                load.AddReference(refName);
            }
        }
    }

    //加载assetbundle先加载manifest
    public IEnumerator LoadAssetBundles(string bundleName)
    {
        while (!IABManifestLoad.Instance.IsLoadFinish())
        {
            yield return null;
        }

        IABRelationMgr load = loadHelper[bundleName];
        string[] dependence = GetDependence(bundleName);
        load.AddDependence(dependence);
        for (int i = 0; i < dependence.Length; i++)
        {
            yield return LoadAssetBundleDependence(dependence[i], bundleName, load.GetProgress());
        }

        yield return load.LoadAssetBundle();
    }

    #region 由下层提供API

    public void DebugAssetBundle(string bundleName) {
        if (loadHelper.ContainsKey(bundleName)) {
            IABRelationMgr loader = loadHelper[bundleName];
            loader.DebugAsset();
        }
    }

    public bool IsLoadFinish(string bundleName) {
        if (loadHelper.ContainsKey(bundleName))
        {
            IABRelationMgr loader = loadHelper[bundleName];
            return loader.IsBundleLoadFinish();
        }
        else
        {
            Debug.Log("IABRelation no contain bundle == " + bundleName);
            return false;
        }
    }

    public UnityEngine.Object GetSingleResource(string bundleName, string resName) {
        //是否已缓存
        if (loadObj.ContainsKey(bundleName))
        {
            AssetResObj tmpRes = loadObj[bundleName];
            List<UnityEngine.Object> tmpObj = tmpRes.GetResObj(resName);
            if (tmpObj != null)
            {
                return tmpObj[0];
            }
        }
        //是否已加载
        if (loadHelper.ContainsKey(bundleName))
        {
            IABRelationMgr load = loadHelper[bundleName];
            UnityEngine.Object tmpObj = load.GetSingleResource(resName);
            AssetObj tmpAssetObj = new AssetObj(tmpObj);
            if (!loadObj.ContainsKey(bundleName))
            {
                AssetResObj tmpRes = new AssetResObj(resName,tmpAssetObj);
                loadObj.Add(bundleName, tmpRes);
            }
            else
            {
                AssetResObj tmpRes = loadObj[bundleName];
                tmpRes.AddResObj(resName, tmpAssetObj);
            }
            return tmpObj;
        }
        return null;
    }

    public UnityEngine.Object[] GetMulResource(string bundleName,string resName){
        if (loadObj.ContainsKey(bundleName))
        {
            AssetResObj tmpRes = loadObj[bundleName];
            List<UnityEngine.Object> tmpObj = tmpRes.GetResObj(resName);
            if (tmpObj != null)
            {
                return tmpObj.ToArray();
            }
        }
        //是否已加载
        if (loadHelper.ContainsKey(bundleName))
        {
            IABRelationMgr load = loadHelper[bundleName];
            UnityEngine.Object[] tmpObj = load.GetMulResource(resName);
            AssetObj tmpAssetObj = new AssetObj(tmpObj);
            if (!loadObj.ContainsKey(bundleName))
            {
                AssetResObj tmpRes = new AssetResObj(resName, tmpAssetObj);
                loadObj.Add(bundleName, tmpRes);
            }
            else
            {
                AssetResObj tmpRes = loadObj[bundleName];
                tmpRes.AddResObj(resName, tmpAssetObj);
            }
            return tmpObj;
        }
        return null;
    }

    #endregion

}

#region 单个assetbundle中的obj
/// <summary>
/// 多个OBJ操作
/// </summary>
public class AssetResObj {
    public Dictionary<string, AssetObj> resObjs;

    public AssetResObj() {
        resObjs = new Dictionary<string, AssetObj>();
    }

    public AssetResObj(string resName,AssetObj tmpOjb){
        if (resObjs == null)
            resObjs = new Dictionary<string, AssetObj>();
        resObjs.Add(resName, tmpOjb);
    }

    public void AddResObj(string name, AssetObj tmpObj) {
        resObjs.Add(name, tmpObj);
    }

    public void ReleaseAllResOjbs() {

        List<string> keys = new List<string>();
        keys.AddRange(resObjs.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            ReleaseResObj(keys[i]);
        }
    }

    public void ReleaseResObj(string name) {
        if (resObjs.ContainsKey(name))
        {
            AssetObj tmpObj = resObjs[name];
            tmpObj.ReleaseObj();
        }
        else
            Debug.Log("release object name is not exit == " + name);
    }

    public List<UnityEngine.Object> GetResObj(string name) {
        if (resObjs.ContainsKey(name))
        {
            AssetObj tmpObj = resObjs[name];
            return tmpObj.objs;
        }
        return null;
    }
}

/// <summary>
/// 单个OBJ操作
/// </summary>
public class AssetObj {
    public List<UnityEngine.Object> objs;

    public AssetObj(params UnityEngine.Object[] objArr) {
        objs = new List<Object>();
        objs.AddRange(objArr);
    }

    public void ReleaseObj() {
        for (int i = 0; i < objs.Count; i++)
        {
            Resources.UnloadAsset(objs[i]);
        }
    }
}
#endregion