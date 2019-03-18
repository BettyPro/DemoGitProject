using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IPathTools {

    public static string GetPlatformFolderName(RuntimePlatform platform) {
        string platformStr = "";
        switch (platform)
        {
            case RuntimePlatform.Android:
                platformStr = "Android";
                break;
            case RuntimePlatform.IPhonePlayer:
                platformStr = "IOS";
                break;
            case RuntimePlatform.OSXEditor:
                platformStr = "OSX";
                break;
            case RuntimePlatform.OSXPlayer:
                platformStr = "OSX";
                break;
            case RuntimePlatform.WindowsEditor:
                platformStr = "Windows";
                break;
            case RuntimePlatform.WindowsPlayer:
                platformStr = "Windows";
                break;
            default:
                break;
        }
        return platformStr;
    }

    public static string GetAppFilePath() {
        string tmpPath = "";
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
            tmpPath = Application.streamingAssetsPath;
        else
            tmpPath = Application.persistentDataPath;
        return tmpPath;
    }

    public static string GetAssetBundlePath() {
        string platFolder = GetPlatformFolderName(Application.platform);
        Debug.Log(Application.platform);
        string filePath = Path.Combine(GetAppFilePath(), platFolder);//Assets/StreamingAsset/Android  adnroid端是沙河路径
        return filePath;
    }

    public static string GetWWWAssetBundlePath() {
        string filePath = "";
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
            filePath = "file:///" + GetAssetBundlePath();
        else
        {
            filePath = GetAssetBundlePath();
#if UNITY_ANDROID
            filePath = "jar:file://" + filePath;
#elif UNITY_STANDALONE_WIN
            filePath = "file:///" + filePath;
#else
            filePath = "file://" + filePath;
#endif
        }
        return filePath;
    }
}
