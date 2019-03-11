﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WinterDebug
{
    public class AndroidDebug : MonoBehaviour
    {

        static bool mRayDebug = false;
        static List<string> mLines = new List<string>();
        static AndroidDebug mInstance = null;

        /// <summary>
        /// Set by UICamera. Can be used to show/hide raycast information.
        /// </summary>

        static public bool debugRaycast
        {
            get { return mRayDebug; }
            set
            {
                if (Application.isPlaying)
                {
                    mRayDebug = value;
                    if (value) CreateInstance();
                }
            }
        }

        /// <summary>
        /// Ensure we have an instance present.
        /// </summary>

        static public void CreateInstance()
        {
            if (mInstance == null)
            {
                GameObject go = new GameObject("_Android Debug");
                mInstance = go.AddComponent<AndroidDebug>();
                DontDestroyOnLoad(go);
            }
        }

        /// <summary>
        /// Add a new on-screen log entry.
        /// </summary>

        static void LogString(string text)
        {
#if UNITY_EDITOR
            Debug.Log(text);
#else
		if (Application.isPlaying)
		{
			if (mLines.Count > 20) mLines.RemoveAt(0);
			mLines.Add(text);
			CreateInstance();
		}
		else Debug.Log(text);
#endif
        }

        /// <summary>
        /// Add a new log entry, printing all of the specified parameters.
        /// </summary>

        static public void Log(bool isDebug = true, params object[] objs)
        {
            string text = "";
            if (isDebug && SetConfig.controleAndroidDebug)
            {
                for (int i = 0; i < objs.Length; ++i)
                {
                    if (i == 0)
                    {
                        text += objs[i].ToString();
                    }
                    else
                    {
                        text += ", " + objs[i].ToString();
                    }
                }

                LogString(text);
            }
        }

        /// <summary>
        /// Clear the logged text.
        /// </summary>

        static public void Clear()
        {
            mLines.Clear();
        }

        /// <summary>
        /// Draw bounds immediately. Won't be remembered for the next frame.
        /// </summary>

        static public void DrawBounds(Bounds b)
        {
            Vector3 c = b.center;
            Vector3 v0 = b.center - b.extents;
            Vector3 v1 = b.center + b.extents;
            Debug.DrawLine(new Vector3(v0.x, v0.y, c.z), new Vector3(v1.x, v0.y, c.z), Color.red);
            Debug.DrawLine(new Vector3(v0.x, v0.y, c.z), new Vector3(v0.x, v1.y, c.z), Color.red);
            Debug.DrawLine(new Vector3(v1.x, v0.y, c.z), new Vector3(v1.x, v1.y, c.z), Color.red);
            Debug.DrawLine(new Vector3(v0.x, v1.y, c.z), new Vector3(v1.x, v1.y, c.z), Color.red);
        }

        void OnGUI()
        {
            if (mLines.Count == 0)
            {
                //if (mRayDebug && UICamera.hoveredObject != null && Application.isPlaying)
                //{
                //    GUILayout.Label("Last Hit: " + NGUITools.GetHierarchy(UICamera.hoveredObject).Replace("\"", ""));
                //}
            }
            else
            {
                for (int i = 0, imax = mLines.Count; i < imax; ++i)
                {
                    GUILayout.Label(mLines[i]);
                }
            }
        }
    }
}