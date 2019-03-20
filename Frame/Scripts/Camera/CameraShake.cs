// *****************************************
//文件名(File Name):    CameraShake.cs
//作者(Author):         #Winter#
//作用(ToDo:);          #WhatToDo#
// *****************************************

using DG.Tweening;
using UnityEngine;

namespace WinterCamera
{
    public class CameraShake
    {
        public float shakeLevel = 3f; // 震动幅度
        public float setShakeTime = 0.5f; // 震动时间
        public float shakeFps = 45f; // 震动的FPS

        private bool isshakeCamera = false; // 震动标志
        private float fps;
        private float shakeTime = 0.0f;
        private float frameTime = 0.0f;
        private float shakeDelta = 0.005f;
        private Camera selfCamera;

        //方向
        private bool up = true;
        private bool left = false;
        private bool leftUp;
        private bool rightUp = false;

        public void ChangeCameraShakeDirection()
        {
            //TODO 处理相机震动的方向 次数
            isshakeCamera = true;
            selfCamera = CameraEffects.Instance._Camera;
            shakeTime = setShakeTime;
            fps = shakeFps;
            frameTime = 0.03f;
            shakeDelta = 0.005f;
            Debug.Log(Random.value + "-----------------------------------------");
        }

        public void SmoothChangeCameraShakeUpdate()
        {
            if (isshakeCamera)
            {
                if (shakeTime > 0)
                {
                    shakeTime -= Time.deltaTime;
                    if (shakeTime <= 0)
                    {
//					enabled = false;
                        selfCamera.rect = new Rect(0, 0, 1.0f, 1.0f);
                    }
                    else
                    {
                        frameTime += Time.deltaTime;
                        if (frameTime > 1.0 / fps)
                        {
                            frameTime = 0;
                            if (up)
                            {
//                                selfCamera.rect = new Rect(0, shakeDelta * (-0.50f + shakeLevel * Random.value * 2),
//                                    1.0f,
//                                    1.0f); //上下
//                                selfCamera.rect.Set(0, shakeDelta * (-0.50f + shakeLevel * Random.value * 2),
//                                    1.0f,
//                                    1.0f);
//                                selfCamera.DOShakePosition(0.5f, new Vector3(0,1*15,0));//上下
                                selfCamera.DOShakePosition(0.5f, new Vector3(1*15,0,0),12,100);//左右
//                                selfCamera.DOShakePosition(0.5f, new Vector3(1*25,-1*10,0));//左上
//                                selfCamera.DOShakePosition(0.5f, new Vector3(1*25,-1*10,0));//右上
                            }

                            if (left)
                            {
                                selfCamera.rect = new Rect(shakeDelta * (1.0f + shakeLevel * Random.value), 0, 1.0f,
                                    1.0f); //左右
                            }

                            if (leftUp)
                            {
                                selfCamera.rect = new Rect(shakeDelta * (-10.0f + shakeLevel * Random.value),
                                    shakeDelta * (5.0f + shakeLevel * Random.value), 1.0f, 1.0f); //左上
                            }

                            if (rightUp)
                            {
                                selfCamera.rect = new Rect(shakeDelta * (-1.0f + shakeLevel * Random.value * 2),
                                    shakeDelta * (-1.0f + shakeLevel * Random.value * 2), 1.0f, 1.0f); //右上
                            }
                        }
                    }
                }
            }
        }
    }
}