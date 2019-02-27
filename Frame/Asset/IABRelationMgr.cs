using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IABRelationMgr {
    //储存资源依赖关系
    private List<string> dependenceBundle;
    //储存资源被依赖关系
    private List<string> referBundle;

    private IABLoad assetLoad;

    private bool isLoadFinish;
    private string bundleName;
    private LoadProgress loadProgress;

    public string BundleName { get { return bundleName; } }

    public IABRelationMgr() {
        dependenceBundle = new List<string>();
        referBundle = new List<string>();
    }

    public void Initial(string _bundleName, LoadProgress progresss) {
        isLoadFinish = false;
        bundleName = _bundleName;
        loadProgress = progresss;
        assetLoad = new IABLoad(progresss, BundleLoadFinish);
        assetLoad.SetBundleName(_bundleName);
        string bundlePath = IPathTools.GetWWWAssetBundlePath() + "/" + _bundleName;
        assetLoad.LoadResources(bundlePath);
    }

    public LoadProgress GetProgress() {
        return loadProgress;
    }

    public void BundleLoadFinish(string bundleName) {
        isLoadFinish = true;
    }

    public bool IsBundleLoadFinish() {
        return isLoadFinish;
    }

    /// <summary>
    /// 添加被依赖关系
    /// </summary>
    /// <param name="bundleName"></param>
    public void AddReference(string bundleName) {
        referBundle.Add(bundleName);
    }

    /// <summary>
    /// 添加依赖关系
    /// </summary>
    /// <param name="bundleNames"></param>
    public void AddDependence(params string[] bundleNames) {
        if (bundleNames.Length > 0)
            dependenceBundle.AddRange(bundleNames);
    }

    /// <summary>
    /// 获取被依赖关系
    /// </summary>
    /// <returns></returns>
    public List<string> GetReference() {
        return referBundle;
    }

    /// <summary>
    /// 获取依赖关系
    /// </summary>
    /// <returns></returns>
    public List<string> GetDependence() {
        return dependenceBundle;
    }

    public bool RemoveReference(string bundleName) {
        for (int i = 0; i < referBundle.Count; i++)
        {
            if (bundleName.Equals(referBundle[i])) {
                referBundle.RemoveAt(i);
                break;
            }                
        }

        if (referBundle.Count <= 0)
        {
            //Dispose();
            return true;
        }
        return false;
    }

    public void RemoveDependence(string bundleName) {
        for (int i = 0; i < dependenceBundle.Count; i++)
        {
            if (bundleName.Equals(dependenceBundle[i])) {
                dependenceBundle.RemoveAt(i);
                break;
            }
        }
    }
    
    #region 由下层提供API

    public void DebugAsset() {
        if (assetLoad != null)
            assetLoad.DebugLoad();
        else
            Debug.Log("Asset load is NULL");
    }

    public IEnumerator LoadAssetBundle() {
        yield return assetLoad.CommonLoad();
    }

    public UnityEngine.Object GetSingleResource(string bundleName) {
        return assetLoad.GetResources(bundleName);
    }

    public UnityEngine.Object[] GetMulResource(string bundleName) {
        return assetLoad.GetMulRes(bundleName);
    }

    public void Dispose() {
        assetLoad.Dispose();
    }
    #endregion
}
