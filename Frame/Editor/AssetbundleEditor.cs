using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using WinterTools;

public class AssetbundleEditor{


    [MenuItem("BuildTools/BuildAssetBundle")]
    public static void BuildAssetBundle() {
        //string outPath = IPathTools.GetAssetBundlePath();//Application.dataPath + "/AssetBundle";
        string outPath = Application.dataPath + "/AssetBundle";
        ColorDebug.Instance.GreenDebug(outPath,"bundle路径",false);
        Debug.Log(ColorDebug.Instance.GreenDebugInfo(outPath, "bundle路径"));
        BuildPipeline.BuildAssetBundles(outPath,0,EditorUserBuildSettings.activeBuildTarget);
        AssetDatabase.Refresh();
    }

    [MenuItem("BuildTools/MarkAssetBundle")]
    public static void MarkAssetBundle() {
        AssetDatabase.RemoveUnusedAssetBundleNames();

        string path = Application.dataPath + "/Art/Scenes/"; //Application.streamingAssetsPath + "";
        DirectoryInfo dir = new DirectoryInfo(path);
        FileSystemInfo[] fileInfo = dir.GetFileSystemInfos();

        for (int i = 0; i < fileInfo.Length; i++)
        {
            FileSystemInfo tmpFile = fileInfo[i];
            if (tmpFile is DirectoryInfo)
            {
                string tmpPath = Path.Combine(path, tmpFile.Name);
                SceneOverView(tmpPath);
            }
        }
        string outPath = IPathTools.GetAssetBundlePath();
        CopyRecord(path, outPath);
        AssetDatabase.Refresh();
    }

    public static void CopyRecord(string sourPath, string disPath)
    {
        DirectoryInfo dir = new DirectoryInfo(sourPath);
        if (!dir.Exists)
        {
            Debug.LogErrorFormat("The path  {0}  is not exit", sourPath);
            return;
        }
        if (!Directory.Exists(disPath))
            Directory.CreateDirectory(disPath);
        FileSystemInfo[] files = dir.GetFileSystemInfos();
        for (int i = 0; i < files.Length; i++)
        {
            FileInfo file = files[i] as FileInfo;
            if (file != null && file.Extension == ".txt")
            {
                string sourFile = sourPath + file.Name;
                string disFile = disPath + "/" + file.Name;
                File.Copy(sourFile, disFile, true);
            }
        }
    }

    //对整个需要打包的文件夹进行遍历
    public static void SceneOverView(string scenePath){
        string textFileName = "Record.txt";
        string tmpPath = scenePath + textFileName;

        FileStream fs = new FileStream(tmpPath, FileMode.OpenOrCreate);
        StreamWriter bw = new StreamWriter(fs);

        //存储对应关系
        Dictionary<string, string> readDic = new Dictionary<string, string>();
        ChangeHead(tmpPath, readDic);

        foreach (string key in readDic.Keys)
        {
            bw.Write("{0} {1}\n",key,readDic[key]);
        }

        bw.Close();
        fs.Close();
    }

    //截取相对路径
    public static void ChangeHead(string fullPath, Dictionary<string, string> theWriter) {
        int tmpCount = fullPath.IndexOf("Assets");
        int tmpLength = fullPath.Length;
        string replacePath = fullPath.Substring(tmpCount, tmpLength - tmpCount);

        DirectoryInfo dir = new DirectoryInfo(fullPath);
        if (dir != null)
        {
            ListFiles(dir, replacePath, theWriter);
        }
        else
            Debug.Log(string.Format("The path {0} is not exit",fullPath));
    }

    //遍历每个子文件夹
    public static void ListFiles(DirectoryInfo dir, string replacePath, Dictionary<string, string> theWriter) {
        if (!dir.Exists)
        {
            Debug.Log(string.Format("The path {0} is not exit", replacePath));
            return;
        }

        FileSystemInfo[] files = dir.GetFileSystemInfos();
        for (int i = 0; i < files.Length; i++)
        {
            FileInfo file = files[i] as FileInfo;

            //对文件进行操作
            if (file != null)
            {
                ChangeMark(file, replacePath, theWriter);
            }
            else//对文件夹进行操作
            {
                ListFiles(files[i] as DirectoryInfo, replacePath, theWriter);
            }
        }
    }

    public static void ChangeMark(FileInfo info, string replacePath, Dictionary<string, string> theWriter) {
        if (info.Extension == ".meta")
        {
            return;
        }

        string markStr = GetBundlePath(info, replacePath);

        ChangeAssetMark(info, markStr, theWriter);
    }

    public static void ChangeAssetMark(FileInfo info, string markStr, Dictionary<string, string> theWriter) {
        string fullPath = info.FullName;
        int assetCount = fullPath.IndexOf("Assets");
        string assetPath = fullPath.Substring(assetCount, fullPath.Length - assetCount);

        AssetImporter importer = AssetImporter.GetAtPath(assetPath);
        importer.assetBundleName = markStr;

        if (info.Extension == ".unity")
        {
            importer.assetBundleVariant = "u3d";
        }
        else
        {
            importer.assetBundleVariant = "ld";
        }

        string modleName = "";
        string[] subMark = markStr.Split("/".ToCharArray());
        if (subMark.Length > 1)
        {
            modleName = subMark[1];
        }
        else
        {
            modleName = markStr;
        }

        string modlePath = markStr.ToLower() + "." + importer.assetBundleVariant;
        if (!theWriter.ContainsKey(modleName))
        {
            theWriter.Add(modleName, modlePath);
        }
    }

    public static string GetBundlePath(FileInfo file, string replacePath)
    {
        string tmpPath = file.FullName;
        tmpPath = FixedPath(tmpPath);

        int assetCount = tmpPath.IndexOf(replacePath);
        assetCount += replacePath.Length + 1;

        int nameCount = tmpPath.LastIndexOf(file.Name);
        int tmpCount = replacePath.LastIndexOf("/");
        string sceneHead = replacePath.Substring(tmpCount + 1, replacePath.Length - tmpCount - 1);
        int tmpLength = nameCount - assetCount;

        if (tmpLength > 0)
        {
            string subString = tmpPath.Substring(assetCount, tmpPath.Length - assetCount);
            string[] result = subString.Split("/".ToCharArray());
            return sceneHead + "/" + result[0];
        }
        return sceneHead;
    }

    public static string FixedPath(string filePath) {
        filePath = filePath.Replace("\\", "/");
        return filePath;
    }
}
