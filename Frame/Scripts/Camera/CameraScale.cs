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
        public void ChangeCameraScaleToAttack()
        {
            //TODO 缩放处理
            RoleMove.instance.mainCamera.GetComponent<ConstrainCamera>().target = SkillEffect.Instance.attack_roleIns.transform;
            RoleMove.instance.mainCamera.GetComponent<ConstrainCamera>().offset = new Vector3(-4,84,-620);
            RoleMove.instance.mainCamera.GetComponent<Camera>().fieldOfView = 40f;
            
        }
        
        public void ChangeCameraScaleToTarget()
        {
            //TODO 缩放处理
            RoleMove.instance.mainCamera.GetComponent<ConstrainCamera>().target = SkillEffect.Instance.target_roleIns.transform;
            RoleMove.instance.mainCamera.GetComponent<ConstrainCamera>().offset = new Vector3(-89,90,-620);
            RoleMove.instance.mainCamera.GetComponent<Camera>().fieldOfView = 40f;
        }

        public void RecoverNormal()
        {
            //TODO 恢复正常
            RoleMove.instance.mainCamera.GetComponent<ConstrainCamera>().target =
                ADDUIBattle.instance.PlayerContrBattle.transform;
            RoleMove.instance.mainCamera.GetComponent<Camera>().fieldOfView = 65f;
            RoleMove.instance.mainCamera.GetComponent<ConstrainCamera>().offset = new Vector3(400, 180, -620);
        }

        public void ChangeCameraFollow()
        {
            //TODO 改变相机的跟随模式
        }
    }
}