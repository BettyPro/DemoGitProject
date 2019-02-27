using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 普通的泛型
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> where T : new() //where T : new()为泛型约束，通俗来说就是确保T类型是可以被new的
{
    private static T _instance; //私有的T类型的静态变量

    public static T Instance
    {
        get
        {
            if (_instance == null) //判断实例是否已存在
            {
                _instance = new T(); //不存在则创建新的实例
            }

            return _instance; //返回实例
        }
    }

    public static T GetInstance() //获取实例的函数
    {
        return Instance;
    }

}

/// <summary>
/// 继承与Mono的泛型
/// </summary>
/// <typeparam name="T"></typeparam>
public class UnitySingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static bool _instanceInitialized;

        private static object _lock = new object();

        public static T GetInstance()
        {
            if (applicationIsQuitting)
            {
                Debug.LogWarning("[UnitySingleton] Instance '" + typeof(T) +
                                 "' already destroyed on application quit." +
                                 " Won't create again - returning null.");
                return null;
            }

            if (_instance != null && _instanceInitialized)
            {
                return _instance;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));

                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogError("[UnitySingleton] Something went really wrong " +
                                       " - there should never be more than 1 singleton!" +
                                       " Reopenning the scene might fix it.");

                        _instanceInitialized = true;
                        return _instance;
                    }

                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject();


                        _instance = singleton.AddComponent<T>();
                        singleton.name = "(UnitySingleton) " + typeof(T).ToString();

                        DontDestroyOnLoad(singleton);
                    }
                    else
                    {
                        Debug.Log("[UnitySingleton] Using instance already created: " + _instance.gameObject.name);
                    }

                    _instanceInitialized = true;
                }

                return _instance;
            }
        }

        public static T Instance
        {
            get
            {
                return GetInstance();
            }
        }

        private static bool applicationIsQuitting = false;
        /// <summary>  
        /// When Unity quits, it destroys objects in a random order.  
        /// In principle, a Singleton is only destroyed when application quits.  
        /// If any script calls Instance after it have been destroyed,   
        ///   it will create a buggy ghost object that will stay on the Editor scene  
        ///   even after stopping playing the Application. Really bad!  
        /// So, this was made to be sure we're not creating that buggy ghost object.  
        /// </summary>  
        public void OnDestroy()
        {
            applicationIsQuitting = true;
        }

        public void OnApplicationQuit()
        {
            applicationIsQuitting = true;
        }
    }
