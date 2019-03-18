using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class NativeResLoad : AssetsBase
    {

        private AssetResBack resBackMsg = null;
        private NativeResCallbackMgr callback = null;

        public AssetResBack ReleaseBack
        {
            get
            {
                if (resBackMsg == null)
                    resBackMsg = new AssetResBack();
                return resBackMsg;
            }
        }

        public NativeResCallbackMgr Callback
        {
            get
            {
                if (callback == null)
                    callback = new NativeResCallbackMgr();
                return callback;
            }
        }

        void Awake()
        {
            msgIDs = new ushort[]
            {
                (ushort) AssetEvent.HunkRes,

                (ushort) AssetEvent.ReleaseSingleObj,
                (ushort) AssetEvent.ReleaseBundleObj,
                (ushort) AssetEvent.ReleaseSceneObj,

                (ushort) AssetEvent.ReleaseSingleBundle,
                (ushort) AssetEvent.ReleaseSceneBundle,
                (ushort) AssetEvent.ReleaseAll
            };
            RegistSelf(this, msgIDs);
        }

        public void GetResources(string sceneName, string bundleName, string resName, bool isSingle, ushort backID)
        {
            if (!ILoadMgr.Instance.IsLoadedAssetBundle(sceneName, bundleName))
            {
                ILoadMgr.Instance.LoadAsset(sceneName, bundleName, LoadProgress);
                string bundleFullName = ILoadMgr.Instance.GetBundleName(sceneName, bundleName);
                if (bundleName != null)
                {
                    NativeResCallbackNode tmpNode = new NativeResCallbackNode(sceneName, bundleName, resName, isSingle,
                        backID, null, SendToBackMsg);
                    Callback.AddBundle(bundleFullName, tmpNode);
                }
                else
                    Debug.LogErrorFormat("Don't contain bundle == {0}", bundleName);
            }
            else if (ILoadMgr.Instance.IsLoadFinish(sceneName, bundleName))
            {
                if (isSingle)
                {
                    Object tmpObj = ILoadMgr.Instance.GetSingleResource(sceneName, bundleName, resName);
                    ReleaseBack.Changer(backID, tmpObj);
                }
                else
                {
                    Object[] tmpObjArr = ILoadMgr.Instance.GetMulResource(sceneName, bundleName, resName);
                    ReleaseBack.Changer(backID, tmpObjArr);
                }

                SendMsg(ReleaseBack);
            }
            else
            {
                string bundleFullName = ILoadMgr.Instance.GetBundleName(sceneName, bundleName);
                if (bundleName != null)
                {
                    NativeResCallbackNode tmpNode = new NativeResCallbackNode(sceneName, bundleName, resName, isSingle,
                        backID, null, SendToBackMsg);
                    Callback.AddBundle(bundleFullName, tmpNode);
                }
                else
                    Debug.LogErrorFormat("Don't contain bundle == {0}", bundleName);
            }
        }

        private void LoadProgress(string bundleName, float progress)
        {
            if (progress >= 1.0f)
            {
                Callback.CallbackRes(bundleName);
                Callback.Dispose(bundleName);
            }
        }

        //node回调
        public void SendToBackMsg(NativeResCallbackNode tmpNode)
        {
            if (tmpNode.isSingle)
            {
                Object tmpObj =
                    ILoadMgr.Instance.GetSingleResource(tmpNode.sceneName, tmpNode.bundleName, tmpNode.resName);
                this.ReleaseBack.Changer(tmpNode.backMsgID, tmpObj);
            }
            else
            {
                Object[] tmpObjs =
                    ILoadMgr.Instance.GetMulResource(tmpNode.sceneName, tmpNode.bundleName, tmpNode.resName);
                this.ReleaseBack.Changer(tmpNode.backMsgID, tmpObjs);
            }

            SendMsg(ReleaseBack);
        }

        public override void ProcessEvent(MsgBase tmpMsg)
        {
            AssetRes assetRes = (AssetRes) tmpMsg;
            switch (tmpMsg.msgID)
            {
                case (ushort) AssetEvent.HunkRes:
                    GetResources(assetRes.sceneName, assetRes.bundleName, assetRes.resName, assetRes.isSingle,
                        assetRes.backMsgID);
                    break;
                case (ushort) AssetEvent.ReleaseSingleObj:
                    ILoadMgr.Instance.DisposeResObj(assetRes.sceneName, assetRes.bundleName, assetRes.resName);
                    break;
                case (ushort) AssetEvent.ReleaseBundleObj:
                    ILoadMgr.Instance.DisposeResObj(assetRes.sceneName, assetRes.bundleName);
                    break;
                case (ushort) AssetEvent.ReleaseSceneObj:
                    ILoadMgr.Instance.DisposeAllObj(assetRes.sceneName);
                    break;

                case (ushort) AssetEvent.ReleaseSingleBundle:
                    ILoadMgr.Instance.DisposeBundle(assetRes.sceneName, assetRes.bundleName);
                    break;
                case (ushort) AssetEvent.ReleaseSceneBundle:
                    ILoadMgr.Instance.DisposeAllBundle(assetRes.sceneName);
                    break;
                case (ushort) AssetEvent.ReleaseAll:
                    ILoadMgr.Instance.DisposeAllBundleAndRes(assetRes.sceneName);
                    break;
                default:
                    break;
            }
        }
    }

    public delegate void NativeResCallback(NativeResCallbackNode tmpNode);

    public class NativeResCallbackNode
    {

        public string sceneName;
        public string bundleName;
        public string resName;

        public bool isSingle;

        public ushort backMsgID;

        public NativeResCallbackNode nextValue;

        public NativeResCallback callback;

        public NativeResCallbackNode(string tmpSceneName, string tmpBundleName, string tmpResName, bool tmpIsSingle,
            ushort tmpBackMsgID, NativeResCallbackNode tmpNextValue, NativeResCallback tmpCallback)
        {
            this.sceneName = tmpSceneName;
            this.bundleName = tmpBundleName;
            this.resName = tmpResName;
            this.isSingle = tmpIsSingle;
            this.backMsgID = tmpBackMsgID;
            this.nextValue = tmpNextValue;
            this.callback = tmpCallback;
        }

        public void Dispose()
        {
            this.sceneName = null;
            this.bundleName = null;
            this.resName = null;
            this.isSingle = false;
            this.backMsgID = 0;
            this.nextValue = null;
            this.callback = null;
        }
    }

    public class NativeResCallbackMgr
    {

        Dictionary<string, NativeResCallbackNode> manager = null;

        public NativeResCallbackMgr()
        {
            manager = new Dictionary<string, NativeResCallbackNode>();
        }

        public void AddBundle(string bundleName, NativeResCallbackNode tmpNode)
        {
            if (manager.ContainsKey(bundleName))
            {
                NativeResCallbackNode tmpCallbackNode = manager[bundleName];
                while (tmpCallbackNode.nextValue != null)
                {
                    tmpCallbackNode = tmpCallbackNode.nextValue;
                }

                tmpCallbackNode.nextValue = tmpNode;
            }
            else
                manager.Add(bundleName, tmpNode);
        }

        public void Dispose(string bundleName)
        {
            if (manager.ContainsKey(bundleName))
            {
                NativeResCallbackNode tmpNode = manager[bundleName];
                NativeResCallbackNode curNode = null;
                while (tmpNode.nextValue != null)
                {
                    curNode = tmpNode;
                    tmpNode = tmpNode.nextValue;
                    curNode.Dispose();
                }

                tmpNode.Dispose();
                manager.Remove(bundleName);
            }
        }

        public void CallbackRes(string bundleName)
        {
            if (manager.ContainsKey(bundleName))
            {
                NativeResCallbackNode tmpNode = manager[bundleName];
                do
                {
                    tmpNode.callback(tmpNode);
                    tmpNode = tmpNode.nextValue;
                } while (tmpNode != null);
            }
        }
    }
}