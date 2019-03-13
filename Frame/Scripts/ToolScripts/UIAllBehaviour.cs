// *****************************************
//文件名(File Name):    UIAllBehaviour.cs
//作者(Author):         #Winter#
//作用(ToDo:);          #WhatToDo#
// *****************************************
using Demo;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WinterTools_Event;

namespace WinterTools
{
    [RequireComponent(typeof(EventTrigger))]
    public class UIAllBehaviour : MonoBehaviour
    {
        private Button _button;
        private EventTrigger _trigger;

        private EventTrigger.Entry en1;
        private EventTrigger.Entry en2;
        private EventTrigger.Entry en3;
        private EventTrigger.Entry en4;
        private EventTrigger.Entry en5;

        private static bool isNeedRemoveLister;
        public static bool GetisNeedRemoveLister
        {
            get { return isNeedRemoveLister; }
            set { isNeedRemoveLister = value; }
        }

        private static bool isNeedDebug;
        public static bool GetisNeedDebug
        {
            get { return isNeedDebug; }
            set { isNeedDebug = value; }
        }
        
        Transform myTrans;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _trigger = _button.gameObject.GetComponent<EventTrigger>();
            en1 = new EventTrigger.Entry();
            en2 = new EventTrigger.Entry();
            en3 = new EventTrigger.Entry();
            en4 = new EventTrigger.Entry();
            en5 = new EventTrigger.Entry();
            
            ColorDebug.Instance.RedDebug(name, ".....", false);
            UIManager.Instance.RegistGameObject(name, gameObject);
            myTrans = transform;
        }

        private void Start()
        {
            AddInfo();
        }

        private void AddInfo()
        {
            // 鼠标点击事件
            en1.eventID = EventTriggerType.PointerClick;
            // 鼠标进入事件 
            en2.eventID = EventTriggerType.PointerEnter;
            // 鼠标滑出事件 
            en3.eventID = EventTriggerType.PointerExit;
            //鼠标按下事件
            en4.eventID = EventTriggerType.PointerDown;
            //鼠标抬起事件
            en5.eventID = EventTriggerType.PointerUp;

            en1.callback = new EventTrigger.TriggerEvent();
            en1.callback.AddListener(ButtonClick);
            _trigger.triggers.Add(en1);

            en2.callback = new EventTrigger.TriggerEvent();
            en2.callback.AddListener(ButtonEnter);
            _trigger.triggers.Add(en2);

            en3.callback = new EventTrigger.TriggerEvent();
            en3.callback.AddListener(ButtonExit);
            _trigger.triggers.Add(en3);

            en4.callback = new EventTrigger.TriggerEvent();
            en4.callback.AddListener(ButtonDown);
            _trigger.triggers.Add(en4);

            en5.callback = new EventTrigger.TriggerEvent();
            en5.callback.AddListener(ButtonUp);
            _trigger.triggers.Add(en5);
        }

        public void ButtonClick(UnityAction<int> callBack,int a)
        {
            Debug.Log("鼠标点击了!");
            callBack(a);
        }
        
        public void ButtonEnter()
        {
            Debug.Log("鼠标进入了!");
        }

        public void ButtonExit()
        {
            Debug.Log("鼠标滑出了!");
        }

        public void ButtonDown()
        {
            Debug.Log("按下");
        }

        public void ButtonUp()
        {
            Debug.Log("抬起");
        }
        
        public void ButtonClick(BaseEventData pointData)
        {
            Debug.Log("鼠标点击了! 有参数");
//            Debug.Log(pointData.selectedObject.name);
        }

        public void ButtonEnter(BaseEventData pointData)
        {
            Debug.Log("鼠标进入了!有参数");
        }

        public void ButtonExit(BaseEventData pointData)
        {
            Debug.Log("鼠标滑出了!有参数");
        }

        public void ButtonDown(BaseEventData pointData)
        {
            Debug.Log("按下有参数");
        }

        public void ButtonUp(BaseEventData pointData)
        {
            Debug.Log("抬起有参数");
        }

         #region UI的监听事件

        public void AddButtonListener(UnityAction action)
        {
            if (action != null)
            {
                ColorDebug.Instance.RedDebug(myTrans, "------", false);
                Button btn = myTrans.GetComponent<Button>();
                btn.onClick.AddListener(action);
            }
        }

        public void AddButtonListener<T>(UnityAction<T> action, T para)
        {
            if (action != null)
            {
                Button btn = transform.GetComponent<Button>();
                btn.onClick.AddListener(delegate() { action(para); });
            }
        }

        public void AddButtonListener<T>(UnityAction<T, Roles> action, T para, Roles role)
        {
            if (action != null)
            {
                Button btn = transform.GetComponent<Button>();
                btn.onClick.AddListener(delegate { action(para, role); });
            }
        }
        
        public void AddButtonListener<T,K>(UnityAction<T, K> action, T para, K role)
        {
            if (action != null)
            {
                Button btn = transform.GetComponent<Button>();
                btn.onClick.AddListener(delegate { action(para, role); });
            }
        }
        
        public void AddButtonListener<T,K,A>(UnityAction<T, K,A> action, T para, K role,A _a)
        {
            if (action != null)
            {
                Button btn = transform.GetComponent<Button>();
                btn.onClick.AddListener(delegate { action(para, role,_a); });
            }
        }
        
        public void AddButtonListener<T,K,A,B>(UnityAction<T, K,A,B> action, T para, K role,A _a,B _b)
        {
            if (action != null)
            {
                Button btn = transform.GetComponent<Button>();
                btn.onClick.AddListener(delegate { action(para, role,_a,_b); });
            }
        }

        //public static void AddButtonListener<T>(Button button, UnityAction action,  T param)
        //{
        //    if (action != null)
        //    {
        //        button.onClick.AddListener(delegate ()
        //        {
        //            action(param);
        //        });
        //    }
        //}

        public void RemoveButtonListener<T>(UnityAction<T, Roles> action, T para, Roles role)
        {
            if (action != null)
            {
                Button btn = transform.GetComponent<Button>();
                btn.onClick.AddListener(delegate() { action(para, role); });
            }
        }


        public void RemoveButtonListener(UnityAction action)
        {
            if (action != null)
            {
                Button btn = myTrans.GetComponent<Button>();
                btn.onClick.RemoveListener(action);
            }
        }

        public void AddToggleListener(UnityAction<bool> action)
        {
            if (action != null)
            {
                Toggle btn = myTrans.GetComponent<Toggle>();
                btn.onValueChanged.AddListener(action);
            }
        }

        public void RemoveToggleListener(UnityAction<bool> action)
        {
            if (action != null)
            {
                Toggle btn = myTrans.GetComponent<Toggle>();
                btn.onValueChanged.RemoveListener(action);
            }
        }

        public void AddSliderListener(UnityAction<float> action)
        {
            if (action != null)
            {
                Slider btn = myTrans.GetComponent<Slider>();
                btn.onValueChanged.AddListener(action);
            }
        }

        public void RemoveSliderListener(UnityAction<float> action)
        {
            if (action != null)
            {
                Slider btn = myTrans.GetComponent<Slider>();
                btn.onValueChanged.RemoveListener(action);
            }
        }

        public void AddInputListener(UnityAction<string> action)
        {
            if (action != null)
            {
                InputField btn = myTrans.GetComponent<InputField>();
                btn.onValueChanged.AddListener(action);
            }
        }

        public void RemoveInputListener(UnityAction<string> action)
        {
            if (action != null)
            {
                InputField btn = myTrans.GetComponent<InputField>();
                btn.onValueChanged.RemoveListener(action);
            }
        }

        public void AddInputFinishListener(UnityAction<string> action)
        {
            if (action != null)
            {
                Debug.Log(myTrans);
                InputField btn = myTrans.GetComponent<InputField>();
                btn.onEndEdit.AddListener(action);
            }
        }

        public void RemoveInputFinishListener(UnityAction<string> action)
        {
            if (action != null)
            {
                InputField btn = myTrans.GetComponent<InputField>();
                btn.onEndEdit.RemoveListener(action);
            }
        }
        #endregion
        
        private void OnDestroy()
        {
            if (isNeedRemoveLister)
            {
                en1.callback.RemoveListener(ButtonClick);
                en2.callback.RemoveListener(ButtonEnter);
                en3.callback.RemoveListener(ButtonExit);
                en4.callback.RemoveListener(ButtonDown);
                en5.callback.RemoveListener(ButtonUp);
                ColorDebug.Instance.GreenDebug(gameObject, "物体销毁", isNeedDebug);
            }
        }

        private void OnDisable()
        {
            if (isNeedRemoveLister)
            {
                en1.callback.RemoveListener(ButtonClick);
                en2.callback.RemoveListener(ButtonEnter);
                en3.callback.RemoveListener(ButtonExit);
                en4.callback.RemoveListener(ButtonDown);
                en5.callback.RemoveListener(ButtonUp);
                ColorDebug.Instance.GreenDebug(gameObject, "物体不启用", isNeedDebug);
            }
        }
    }
}