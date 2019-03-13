using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using WinterTools;

namespace Demo
{
    /// <summary>
    /// 把控件的script注册到UIManager中
    /// </summary>
    public class UIBehaviour : MonoBehaviour
    {
        Transform myTrans;

        void Awake()
        {
            ColorDebug.Instance.RedDebug(name, ".....", false);
            UIManager.Instance.RegistGameObject(name, gameObject);
            myTrans = transform;

        }


        void Start()
        {
            //myTrans = transform;
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
                btn.onClick.AddListener(delegate() { action(para, role); });
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
        
        //public void OnPointerDown(PointerEventData eventData)
        //{
        //    Debug.Log("a按下");
        //}

        //public void OnPointerUp(PointerEventData eventData)
        //{
        //    Debug.Log("a抬起");

        //}

        //public void OnPointerExit(PointerEventData eventData)
        //{
        //    Debug.Log("a退出");

        //}

        #endregion
    }
}