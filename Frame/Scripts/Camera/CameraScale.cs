// *****************************************
//文件名(File Name):    CameraScale.cs
//作者(Author):         #Winter#
//作用(ToDo:);          #WhatToDo#
// *****************************************
using Demo;
using UnityEngine;

namespace WinterCamera
{
    public class CameraScale
    {
        private bool beginRecover = false;
        private bool beginsScale = false;
        public void ChangeCameraScaleToAttack()
        {
            beginsScale = true;
            //TODO 缩放处理
            CameraEffects.Instance._ConstrainCamera.target = SkillEffect.Instance.attack_roleIns.transform;
            CameraEffects.Instance._ConstrainCamera.offset = new Vector3(-4, 84, -620);
            CameraEffects.Instance._ConstrainCamera.smoothing = 3;
        }

        public void ChangeCameraScaleToTarget()
        {
            beginsScale = true;
            //TODO 缩放处理
            CameraEffects.Instance._ConstrainCamera.target = SkillEffect.Instance.target_roleIns.transform;
            CameraEffects.Instance._ConstrainCamera.offset = new Vector3(-89, 90, -620);
            CameraEffects.Instance._ConstrainCamera.smoothing = 3;
        }

        public void RecoverNormal()
        {
            beginRecover = true;
            //TODO 恢复正常
            CameraEffects.Instance._ConstrainCamera.target =
                ADDUIBattle.instance.PlayerContrBattle.transform;
            CameraEffects.Instance._ConstrainCamera.smoothing = 5;
            CameraEffects.Instance._ConstrainCamera.offset = new Vector3(400, 180, -620);
        }

        public void SmoothChangeFieldOfViewUpdate()
        {
            if (beginRecover)
            {
                CameraEffects.Instance._Camera.fieldOfView += Time.deltaTime * SetConfig.cameraRecoverValue;
                if (CameraEffects.Instance._Camera.fieldOfView >= 65)
                {
                    CameraEffects.Instance._Camera.fieldOfView = 65;
                    beginRecover = false;
                }
            }

            if (beginsScale)
            {
                CameraEffects.Instance._Camera.fieldOfView -= Time.deltaTime * SetConfig.cameraScaleValue;
                if (CameraEffects.Instance._Camera.fieldOfView <= 40)
                {
                    CameraEffects.Instance._Camera.fieldOfView = 40;
                    beginsScale = false;
                }
            }
        }

        public void ChangeCameraFollow()
        {
            //TODO 改变相机的跟随模式
        }
    }
}