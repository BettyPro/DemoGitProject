// *****************************************
//文件名(File Name):    EventCenter.cs
//作者(Author):         #Winter#
//作用(ToDo:);          #事件的广播机制#
// *****************************************
using System;
using System.Collections.Generic;

namespace WinterTools_Event
{
    /// <summary>
    /// 事件消息号定义
    /// </summary>
    public enum EventID
    {
        showText,
        showTextSingle,
        toButtonOne,
        toButtonTwo,
    }

    #region 回调的委托定义
    public delegate void CallBack();

    public delegate void CallBack<T>(T arg);

    public delegate void CallBack<T, X>(T arg, X arg1);

    public delegate void CallBack<T, X, Y>(T arg, X arg1, Y arg2);

    public delegate void CallBack<T, X, Y, Z>(T arg, X arg1, Y arg2, Z arg3);

    public delegate void CallBack<T, X, Y, Z, W>(T arg, X arg1, Y arg2, Z arg3, W arg4);
    #endregion
  
    
    public class EventCenter 
    {
        private static Dictionary<EventID, Delegate> m_EventTable = new Dictionary<EventID, Delegate>();

        private static void RegistAdding(EventID eventType, Delegate callback)
        {
            if (!m_EventTable.ContainsKey(eventType))
            {
                m_EventTable.Add(eventType, null);
            }

            Delegate currenDel = m_EventTable[eventType];
            if (currenDel != null && currenDel.GetType() != callback.GetType())
            {
                throw new Exception(string.Format("尝试为事件{0}添加不同类型的委托，当前事件所对应的委托类型是{1}，要添加的委托类型是{2}", eventType,
                    currenDel.GetType(),
                    callback.GetType()));
            }
        }

        private static void UnRegistRemoving(EventID eventType, Delegate callback)
        {
            if (m_EventTable.ContainsKey(eventType))
            {
                Delegate currentDel = m_EventTable[eventType];
                if (currentDel == null)
                {
                    throw new Exception(String.Format("事件移除事件注册错误：事件{0}没有对应的委托", eventType));
                }
                else if (currentDel.GetType() != callback.GetType())
                {
                    throw new Exception(String.Format("事件移除事件注册错误：尝试为事件{0}事件移除不同类型的委托，当前委托的类型为{1}，要事件移除的委托类型为{2}", eventType,
                        currentDel.GetType(), callback.GetType()));
                }
            }
            else
            {
                throw new Exception(String.Format("事件移除事件注册错误：没有事件码{0}", eventType));
            }
        }

        private static void OnListenerRemoved(EventID eventType)
        {
            if (eventType == null)
            {
                m_EventTable.Remove(eventType);
            }
        }

        //无参的事件注册
        public static void RegistEvent(EventID eventType, CallBack callback)
        {
            RegistAdding(eventType, callback);
            m_EventTable[eventType] = (CallBack) m_EventTable[eventType] + callback;
        }

        //一个参数的事件注册
        public static void RegistEvent<T>(EventID eventType, CallBack<T> callback)
        {
            RegistAdding(eventType, callback);
            m_EventTable[eventType] = (CallBack<T>) m_EventTable[eventType] + callback;
        }

        //两个参数的事件注册
        public static void RegistEvent<T, X>(EventID eventType, CallBack<T, X> callback)
        {
            RegistAdding(eventType, callback);
            m_EventTable[eventType] = (CallBack<T, X>) m_EventTable[eventType] + callback;
        }

        //三个参数的事件注册
        public static void RegistEvent<T, X, Y>(EventID eventType, CallBack<T, X, Y> callback)
        {
            RegistAdding(eventType, callback);
            m_EventTable[eventType] = (CallBack<T, X, Y>) m_EventTable[eventType] + callback;
        }

        //四个参数的事件注册
        public static void RegistEvent<T, X, Y, Z>(EventID eventType, CallBack<T, X, Y, Z> callback)
        {
            RegistAdding(eventType, callback);
            m_EventTable[eventType] = (CallBack<T, X, Y, Z>) m_EventTable[eventType] + callback;
        }

        //五个参数的事件注册
        public static void RegistEvent<T, X, Y, Z, W>(EventID eventType, CallBack<T, X, Y, Z, W> callback)
        {
            RegistAdding(eventType, callback);
            m_EventTable[eventType] = (CallBack<T, X, Y, Z, W>) m_EventTable[eventType] + callback;
        }

        //无参的事件移除事件注册
        public static void UnRegistEvent(EventID eventType, CallBack callback)
        {
            UnRegistRemoving(eventType, callback);
            m_EventTable[eventType] = (CallBack) m_EventTable[eventType] - callback;
            OnListenerRemoved(eventType);
        }

        //一个参数的事件事件移除
        public static void UnRegistEvent<T, X>(EventID eventType, CallBack<T, X> callback)
        {
            UnRegistRemoving(eventType, callback);
            m_EventTable[eventType] = (CallBack<T, X>) m_EventTable[eventType] - callback;
            OnListenerRemoved(eventType);
        }

        //两个参数的事件移除
        public static void UnRegistEvent<T>(EventID eventType, CallBack<T> callback)
        {
            UnRegistRemoving(eventType, callback);
            m_EventTable[eventType] = (CallBack<T>) m_EventTable[eventType] - callback;
            OnListenerRemoved(eventType);
        }

        //三个参数的事件移除
        public static void UnRegistEvent<T, X, Y>(EventID eventType, CallBack<T, X, Y> callback)
        {
            UnRegistRemoving(eventType, callback);
            m_EventTable[eventType] = (CallBack<T, X, Y>) m_EventTable[eventType] - callback;
            OnListenerRemoved(eventType);
        }

        //四个参数的事件移除
        public static void UnRegistEvent<T, X, Y, Z>(EventID eventType, CallBack<T, X, Y, Z> callback)
        {
            UnRegistRemoving(eventType, callback);
            m_EventTable[eventType] = (CallBack<T, X, Y, Z>) m_EventTable[eventType] - callback;
            OnListenerRemoved(eventType);
        }

        //五个参数的事件移除
        public static void UnRegistEvent<T, X, Y, Z, W>(EventID eventType, CallBack<T, X, Y, Z, W> callback)
        {
            UnRegistRemoving(eventType, callback);
            m_EventTable[eventType] = (CallBack<T, X, Y, Z, W>) m_EventTable[eventType] - callback;
            OnListenerRemoved(eventType);
        }

        public static void BroadCast(EventID eventType)
        {
            Delegate currentDel;
            if (m_EventTable.TryGetValue(eventType, out currentDel))
            {
                CallBack callBack = (CallBack) currentDel;
                if (callBack != null)
                {
                    callBack();
                }
                else
                {
                    throw new Exception(String.Format("广播事件错误：事件{0}对应的委托具有不同的类型", eventType));
                }
            }
        }

        public static void BroadCast<T>(EventID eventType, T arg)
        {
            Delegate currentDel;
            if (m_EventTable.TryGetValue(eventType, out currentDel))
            {
                CallBack<T> callBack = (CallBack<T>) currentDel;
                if (callBack != null)
                {
                    callBack(arg);
                }
                else
                {
                    throw new Exception(String.Format("广播事件错误：事件{0}对应的委托具有不同的类型", eventType));
                }
            }
        }

        public static void BroadCast<T, X>(EventID eventType, T arg, X arg1)
        {
            Delegate currentDel;
            if (m_EventTable.TryGetValue(eventType, out currentDel))
            {
                CallBack<T, X> callBack = (CallBack<T, X>) currentDel;
                if (callBack != null)
                {
                    callBack(arg, arg1);
                }
                else
                {
                    throw new Exception(String.Format("广播事件错误：事件{0}对应的委托具有不同的类型", eventType));
                }
            }
        }

        public static void BroadCast<T, X, Y>(EventID eventType, T arg, X arg1, Y arg2)
        {
            Delegate currentDel;
            if (m_EventTable.TryGetValue(eventType, out currentDel))
            {
                CallBack<T, X, Y> callBack = (CallBack<T, X, Y>) currentDel;
                if (callBack != null)
                {
                    callBack(arg, arg1, arg2);
                }
                else
                {
                    throw new Exception(String.Format("广播事件错误：事件{0}对应的委托具有不同的类型", eventType));
                }
            }
        }

        public static void BroadCast<T, X, Y, Z>(EventID eventType, T arg, X arg1, Y arg2, Z arg3)
        {
            Delegate currentDel;
            if (m_EventTable.TryGetValue(eventType, out currentDel))
            {
                CallBack<T, X, Y, Z> callBack = (CallBack<T, X, Y, Z>) currentDel;
                if (callBack != null)
                {
                    callBack(arg, arg1, arg2, arg3);
                }
                else
                {
                    throw new Exception(String.Format("广播事件错误：事件{0}对应的委托具有不同的类型", eventType));
                }
            }
        }

        public static void BroadCast<T, X, Y, Z, W>(EventID eventType, T arg, X arg1, Y arg2, Z arg3, W arg4)
        {
            Delegate currentDel;
            if (m_EventTable.TryGetValue(eventType, out currentDel))
            {
                CallBack<T, X, Y, Z, W> callBack = (CallBack<T, X, Y, Z, W>) currentDel;
                if (callBack != null)
                {
                    callBack(arg, arg1, arg2, arg3, arg4);
                }
                else
                {
                    throw new Exception(String.Format("广播事件错误：事件{0}对应的委托具有不同的类型", eventType));
                }
            }
        }
    }
}