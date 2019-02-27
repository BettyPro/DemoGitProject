using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IABManifestLoad {

    public AssetBundleManifest assetManifest;
    public AssetBundle manifestLoad;
    private bool isLoadFinish;
    public string manifestPath;
    

    public IABManifestLoad() {
        assetManifest = null;
        manifestLoad = null;
        isLoadFinish = false;
        manifestPath = IPathTools.GetWWWAssetBundlePath() + "/" + IPathTools.GetPlatformFolderName(Application.platform);
    }

    public IEnumerator LoadManifest() {
        WWW manifest = new WWW(manifestPath);
        yield return manifest;

        if (!string.IsNullOrEmpty(manifest.error))
        {
            Debug.Log(manifest.error);
        }
        else
        {
            if (manifest.progress >= 1.0f)
            {
                manifestLoad = manifest.assetBundle;
                assetManifest = manifestLoad.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
                isLoadFinish = true;
            }
        }
    }

    public string[] GetDependence(string name) {
        return assetManifest.GetAllDependencies(name);
    }

    public void UnLoadManifest()
    {
        manifestLoad.Unload(true);
    }

    public void SetManifestPath(string path) {
        manifestPath = path;
    }

    private static IABManifestLoad instance = null;

    public static IABManifestLoad Instance {
        get {
            if (instance == null)
            {
                instance = new IABManifestLoad();
            }
            return instance;
        }
    }

    public bool IsLoadFinish() {
        return isLoadFinish;
    }
}
