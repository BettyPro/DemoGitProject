using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Demo
{


    /// <summary>
    /// 需要检测长按的按钮的添加此脚本进行通信
    /// </summary>
    public class OnButtonPressed : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {


        // 延迟时间
        private float delay = 0.5f;

        // 按钮是否是按下状态
        private bool isDown = false;
        private bool isLongTimeDown = false;
        private bool startRecordTime = true;

        private float currenIsDownTime;

        // 按钮最后一次是被按住状态时候的时间
        private float lastIsDownTime;



        void Awake()
        {
            UIManager.Instance.RegistGameObject(name, gameObject);
        }

        void Update()
        {
            // 如果按钮是被按下状态
            // if (isDown)
            // {
            //    // 当前时间 -  按钮最后一次被按下的时间 > 延迟时间0.2秒
            //    if (Time.time - lastIsDownTime > delay)
            //    {
            //        // 触发长按方法
            //        Debug.Log("长按");
            //        isLongTimeDown = true;
            //        // 记录按钮最后一次被按下的时间
            //        lastIsDownTime = Time.time;

            //    }
            // }

            // 如果按钮是被按下状态
            if (isDown)
            {
                if (Time.time - currenIsDownTime > delay)
                {
                    // 触发长按方法
                    Debug.Log("长按");
                    isLongTimeDown = true;
                    LongTimePress();
                }
            }

        }

        void SkillShow()
        {
            ADDUIRole role = ADDUIRole.instance;
            Debug.Log(role);
            int num;
            if (this.name.Equals("skill1"))
            {
                role.roleUse.skillid1 = int.Parse(role.roleId + "01");
            }
            else if (this.name.Equals("skill2"))
            {
                role.roleUse.skillid1 = int.Parse(role.roleId + "02");
            }
            else
            {
                role.roleUse.skillid1 = int.Parse(role.roleId + "03");
            }


            role.GiveSkillData(role.roleUse.skillid1);
        }

        void SkillShowInBattle()
        {
            ADDUIBattle.instance.GiveSkillData(int.Parse(this.gameObject.name));
        }

        // 当按钮被按下后系统自动调用此方法
        public void OnPointerDown(PointerEventData eventData)
        {

            isDown = true;
            //lastIsDownTime = Time.time;
            //Debug.Log("按下按钮");
            if (startRecordTime)
            {
                currenIsDownTime = Time.time;
                startRecordTime = false;
            }

            if (!SetConfig.Instance.isSceneBattle)
            {
                SkillShow();
                ADDUIRole.instance.LongPressShow();
            }
            // else if(isLongTimeDown)
            // {
            //     isLongTimeDown = false;
            //     SkillShowInBattle();
            //     ADDUIBattle.instance.LongPressShow();
            // }

        }

        // 当按钮抬起的时候自动调用此方法
        public void OnPointerUp(PointerEventData eventData)
        {
            startRecordTime = true;
            isDown = false;
            // lastIsDownTime = Time.time;
            //Debug.Log("长按按钮");
            if (!SetConfig.Instance.isSceneBattle)
                ADDUIRole.instance.LongPressShowExit();
            else
                ADDUIBattle.instance.LongPressShowExit();
        }

        // 当鼠标从按钮上离开的时候自动调用此方法
        public void OnPointerExit(PointerEventData eventData)
        {
            isDown = false;
            //Debug.Log("离开按钮");

        }

        IEnumerator DelaySomeSeconds()
        {
            yield return new WaitForSeconds(0.5f);
            ADDUIBattle.instance.LongPressShow();
        }

        void LongTimePress()
        {
            if (isLongTimeDown && SetConfig.Instance.isSceneBattle)
            {
                SkillShowInBattle();
                ADDUIBattle.instance.LongPressShow();
                isLongTimeDown = false;
                isDown = false;
            }
        }

    }
}
