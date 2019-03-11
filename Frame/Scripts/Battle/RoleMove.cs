using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

namespace Demo
{
    [RequireComponent(typeof(CharacterController))]
    public class RoleMove : MonoBehaviour
    {
        public static RoleMove instance;

        [Header("Controls")] public string XAxis = "Horizontal";
        public string YAxis = "Vertical";
        public string JumpButton = "Jump";

        [Header("Moving")] public float walkSpeed = 1.5f;
        public float runSpeed = 10f;
        public float gravityScale = 6.6f;


        public GameObject mainCamera;

        //private RectTransform signPlayer;
        private Transform signPlayer;
        public bool playAudioClip = true;
        private bool signPlayerRotateLeft = true;
        private bool signPlayerRotateRight = true;
        public bool startBattle = true; //开启速度条
        public bool changeState = true; //切换动画状态
        private int leftCount = 0;

        //MiniMap
        private GameObject miniMapCamera;
        private Vector3 distance;
        private Image miniMapCover;

        private float distanceToTarget;
        [Header("Bools")] public bool canWalk = true;
        public bool letCanWalk = false;
        public bool playRun = true;
        private bool clickIsOtherPanel = false;


        CharacterController controller;
        Vector2 input = default(Vector2);
        Vector3 velocity = default(Vector3);
        bool isPc = true;


        void Awake()
        {
            instance = this;
            if (isPc && ADDUIBattle.instance.notUseAni3d)
            {
                this.gameObject.AddComponent<CharacterController>();
                controller = GetComponent<CharacterController>();
            }

            controller = GetComponent<CharacterController>();
        }

        // Use this for initialization
        void Start()
        {
            BindCamera();
            FindUI();
        }

        void Test()
        {
            // AndroidDebug.Log(Touch.fingerId);
        }

        // Update is called once per frame
        void Update()
        {
#if UNITY_ANDROID && false
        if (Input.touchCount == 0 || Input.touchCount != 1)
        {
            playRun = false;
            SetConfig.Instance.sendIdleMes = true;
            AndroidDebug.Log(false, Input.touchCount.ToString());
            AndroidDebug.Log(false, Input.touchCount.ToString(), "canwalk:--" + canWalk + "--playrun:--" + playRun);
        }

        if (Input.touchCount == 1)
        {
            playRun = true;
            AndroidDebug.Log(false, "11---" + Input.touchCount.ToString(),
                "canwalk:--" + canWalk + "--playrun:--" + playRun);
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            //TODO 检测了手指的移动
            //要做的工作
            AndroidDebug.Log(false, "按下了ui");
        }

        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
#if IPHONE || UNITY_ANDROID
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
#else
            if (EventSystem.current.IsPointerOverGameObject())
#endif
            {
                AndroidDebug.Log(false, "当前触摸在UI上");
                ADDUIBattle.instance.clickIsOtherPanel = true;
                clickIsOtherPanel = true;
            }
            else
            {
                AndroidDebug.Log(false, "当前没有触摸在UI上");
                ADDUIBattle.instance.clickIsOtherPanel = false;
                clickIsOtherPanel = false;
            }
        }

        if (Input.touchCount == 1 && canWalk && !ADDUIBattle.instance.stopGame &&
            !clickIsOtherPanel && playRun && !BattleLogic.instance.isAlreadyWin)
        {
            if (Input.touches[0].position.x < Camera.main.WorldToScreenPoint(this.transform.position).x &&
                !ADDUIBattle.instance.stopGame)
            {
                leftCount = 1;
                ExChangePlayerPosLeft();

                for (int i = 0; i < CreatSkeleton.Instance.skeletonsBattleAni.Count; i++)
                {
                    CreatSkeleton.Instance.skeletonsBattleAni[i].transform.rotation = new Quaternion(0, -180, 0, 0);
                }

                canWalk = true;
                letCanWalk = true;
                ADDUIBattle.instance.BattlePanel.localPosition = new Vector3(-3000, 0, 0);
                mainCamera.GetComponent<ConstrainCamera>().offset.x = -400f;
                ADDUIBattle.instance.promptMessage.DOPlayBackwards();
                ADDUIBattle.instance.goMessage.gameObject.SetActive(false);

                transform.position = Vector3.Lerp(transform.position, SetConfig.negativePlace,
                    (float) 0.4 * Time.deltaTime * SetConfig.changeAndroidMoveSpeed);
                //transform.position = Vector3.Lerp(transform.position, SetConfig.negativePlace, 1/(transform.position-SetConfig.negativePlace).magnitude);

                //提示信息
                RoleSpineAniManager.Instance.SendMsg(
                    ButtonMsg.GetInstance.ChangeInfo((ushort) RoleSpineAniId.run, true, true));
            }
            else if (Input.touches[0].position.x > Camera.main.WorldToScreenPoint(this.transform.position).x &&
                     !ADDUIBattle.instance.stopGame)
            {
                mainCamera.GetComponent<ConstrainCamera>().offset.x = 400f;
                ADDUIBattle.instance.promptMessage.DOPlayForward();
                ADDUIBattle.instance.goMessage.gameObject.SetActive(true);
                ADDUIBattle.instance.goMessage.DOPlayForward();

                ExChangePlayerPosRight();
                if (distance.magnitude < (signPlayer.transform.position - miniMapCamera.transform.position).magnitude)
                {
                    miniMapCamera.transform.position +=
                        signPlayer.transform.position - miniMapCamera.transform.position - distance;
                }

                miniMapCamera.transform.position = new Vector3(miniMapCamera.transform.position.x, 352f, -333f);

                for (int i = 0; i < CreatSkeleton.Instance.skeletonsBattleAni.Count; i++)
                {
                    CreatSkeleton.Instance.skeletonsBattleAni[i].transform.rotation = new Quaternion(0, 0, 0, 0);
                }

                transform.position = Vector3.Lerp(transform.position, SetConfig.firstPlace,
                    (float) 0.4 * Time.deltaTime * SetConfig.changeAndroidMoveSpeed);
                //transform.position = Vector3.Lerp(transform.position, SetConfig.firstPlace, 1 / (transform.position - SetConfig.negativePlace).magnitude);

                if (Vector3.Distance(this.transform.position, SetConfig.firstPlace) < 100f)
                {
                    if (!letCanWalk)
                        canWalk = false;

                    // ADDUIBattle.instance.MiniMapControl.anchoredPosition = new Vector3(640, -150f, 0);
                    ADDUIBattle.instance.BattlePanel.localPosition = Vector3.zero;

                    if (playAudioClip)
                    {
                        ADDUIBattle.instance.MiniMapControl.anchoredPosition = new Vector3(640, -150f, 0);
                        UIManager.Instance.SendMsg(ButtonMsg.GetInstance.ChangeInfo((ushort) AudioId.battle));
                        playAudioClip = false;

                        Vector3 ver = new Vector3(ADDUIBattle.instance.PlayerContrBattle.transform.position.x + 700f,
                            20, 0);
                        Debug.Log(ADDUIBattle.instance.PlayerContrBattle.transform.position.x);
                        //CreatSkeleton.Instance.CreatMonsterSkeletonAniInfo(ADDUIBattle.instance.MonsterContrBattle, ver.x, ver.y, ver.z);//改用下面的配置表读取
                        CreatSkeleton.Instance.CreatMonsterSkeletonAniInfos(SetConfig.firstPlace,
                            ADDUIBattle.instance.MonsterContrBattle, ver.x, ver.y, ver.z);
                        ADDUIBattle.instance.CreatRoleSingleInfoToRoleMove();
                    }

                    //开启战斗头像速度条
                    if (startBattle)
                    {
                        RoleSpineAniManager.Instance.SendMsg(
                            ButtonMsg.GetInstance.ChangeInfo((ushort) RoleSpineAniId.idle, "normal"));
                        BattleLogic.instance.testCancel = true;
                        BattleLogic.instance.cancelLoop = true;
                        BattleLogic.instance.firstLoop = true;
                        startBattle = false;
                    }

                    //动画切换
                    if (changeState)
                    {
                        CreatSkeleton.Instance.ControlBloodShow(true);
                        RoleSpineAniManager.Instance.SendMsg(
                            ButtonMsg.GetInstance.ChangeInfo((ushort) RoleSpineAniId.idle, "normal"));
                        changeState = false;
                    }

                    ADDUIBattle.instance.goMessage.gameObject.SetActive(false);
                }
                else
                {
                    letCanWalk = false;
                    RoleSpineAniManager.Instance.SendMsg(
                        ButtonMsg.GetInstance.ChangeInfo((ushort) RoleSpineAniId.run, true, true));
                }
            }

            //CreatSkeleton.Instance.ControlBloodShow(false);
        }
        else if (Input.touchCount != 1 && canWalk && !playRun)
        {
            AndroidDebug.Log(false, "sendIdel:--" + SetConfig.Instance.sendIdleMes);
            if (SetConfig.Instance.sendIdleMes)
            {
                RoleSpineAniManager.Instance.SendMsg(
                    ButtonMsg.GetInstance.ChangeInfo((ushort) RoleSpineAniId.idle, "normal"));
                SetConfig.Instance.sendIdleMes = false;
                SetConfig.Instance.sendRunMes = true;
            }

            CreatSkeleton.Instance.ControlBloodShow(true, false);
        }
        else if (Input.touchCount != 1)
        {
            if (SetConfig.Instance.sendIdleMes)
            {
                RoleSpineAniManager.Instance.SendMsg(
                    ButtonMsg.GetInstance.ChangeInfo((ushort) RoleSpineAniId.idle, "normal"));
                SetConfig.Instance.sendIdleMes = false;
                SetConfig.Instance.sendRunMes = true;
            }
        }
#endif

            if (isPc && !BattleLogic.instance.isAlreadyWin)
            {
                input.x = Input.GetAxis(XAxis);
                input.y = Input.GetAxis(YAxis);
                float dt = Time.deltaTime;

                if (Vector3.Distance(this.transform.position, SetConfig.firstPlace) < 50f)
                {
                    velocity.x = 0f;
                    if (!letCanWalk)
                        canWalk = false;

                    // ADDUIBattle.instance.MiniMapControl.anchoredPosition = new Vector3(640, -150f, 0);
                    ADDUIBattle.instance.BattlePanel.localPosition = Vector3.zero;
                    if (playAudioClip)
                    {
                        ADDUIBattle.instance.MiniMapControl.anchoredPosition = new Vector3(640, -150f, 0);
                        UIManager.Instance.SendMsg(ButtonMsg.GetInstance.ChangeInfo((ushort) AudioId.battle));
                        playAudioClip = false;

                        Vector3 ver = new Vector3(ADDUIBattle.instance.PlayerContrBattle.transform.position.x + 700f,
                            20,
                            0);
                        Debug.Log(ADDUIBattle.instance.PlayerContrBattle.transform.position.x);
                        //CreatSkeleton.Instance.CreatMonsterSkeletonAniInfo(ADDUIBattle.instance.MonsterContrBattle, ver.x, ver.y, ver.z);//改用下面的配置表读取
                        CreatSkeleton.Instance.CreatMonsterSkeletonAniInfos(SetConfig.firstPlace,
                            ADDUIBattle.instance.MonsterContrBattle, ver.x, ver.y, ver.z);
                        ADDUIBattle.instance.CreatRoleSingleInfoToRoleMove();
                    }

                    //开启战斗头像速度条
                    if (startBattle)
                    {
                        BattleLogic.instance.testCancel = true;
                        BattleLogic.instance.cancelLoop = true;
                        BattleLogic.instance.firstLoop = true;
                        startBattle = false;
                    }

                    //动画切换
                    if (changeState)
                    {
                        CreatSkeleton.Instance.ControlBloodShow(true);
                        RoleSpineAniManager.Instance.SendMsg(
                            ButtonMsg.GetInstance.ChangeInfo((ushort) RoleSpineAniId.idle, "normal"));
                        changeState = false;
                    }

                    ADDUIBattle.instance.goMessage.gameObject.SetActive(false);
                    //CreatSkeleton.Instance.ControlBloodShow(true);
                    return;
                }
                else
                {
                    letCanWalk = false;
                }

                if (input.x != 0 && canWalk)
                {
                    velocity.x = Mathf.Abs(input.x) > 0.6f ? runSpeed : walkSpeed;
                    velocity.x *= Mathf.Sign(input.x);
                }

                if (input.x == 0)
                {
                    velocity.x = 0f;
                    CreatSkeleton.Instance.ControlBloodShow(true, false);
                    //if (startBattle)
                    //    RoleSpineAniManager.Instance.SendMsg(ButtonMsg.GetInstance.ChangeInfo((ushort)RoleSpineAniId.idle));
                    if (SetConfig.Instance.sendIdleMes)
                    {
                        RoleSpineAniManager.Instance.SendMsg(
                            ButtonMsg.GetInstance.ChangeInfo((ushort) RoleSpineAniId.idle, "normal"));
                        SetConfig.Instance.sendIdleMes = false;
                        SetConfig.Instance.sendRunMes = true;
                    }
                }

                if (input.x != 0 && !ADDUIBattle.instance.stopGame)
                {
                    if (SetConfig.Instance.sendRunMes)
                    {
                        RoleSpineAniManager.Instance.SendMsg(
                            ButtonMsg.GetInstance.ChangeInfo((ushort) RoleSpineAniId.run, true, true));
                        SetConfig.Instance.sendRunMes = false;
                        SetConfig.Instance.sendIdleMes = true;
                    }
                }

                if (input.x < 0 && !ADDUIBattle.instance.stopGame)
                {
                    canWalk = true;
                    letCanWalk = true;
                    //ADDUIBattle.instance.BattlePanel.GetComponent<RectTransform>().localPosition = new Vector3(-3000, 0, 0);

                    for (int i = 0; i < CreatSkeleton.Instance.skeletonsBattleAni.Count; i++)
                    {
                        CreatSkeleton.Instance.skeletonsBattleAni[i].transform.rotation = new Quaternion(0, -180, 0, 0);
                    }

                    leftCount = 1;

                    ExChangePlayerPosLeft();

                    mainCamera.GetComponent<ConstrainCamera>().offset.x = -400f;
                    ADDUIBattle.instance.promptMessage.DOPlayBackwards();
                    ADDUIBattle.instance.goMessage.gameObject.SetActive(false);
                    CreatSkeleton.Instance.ControlBloodShow(false);
                }
                else if (input.x > 0 && !ADDUIBattle.instance.stopGame)
                {
                    for (int i = 0; i < CreatSkeleton.Instance.skeletonsBattleAni.Count; i++)
                    {
                        CreatSkeleton.Instance.skeletonsBattleAni[i].transform.rotation = new Quaternion(0, 0, 0, 0);
                    }


                    ExChangePlayerPosRight();
                    mainCamera.GetComponent<ConstrainCamera>().offset.x = 400f;
                    ADDUIBattle.instance.promptMessage.DOPlayForward();
                    ADDUIBattle.instance.goMessage.gameObject.SetActive(true);
                    ADDUIBattle.instance.goMessage.DOPlayForward();

                    if (distance.magnitude <
                        (signPlayer.transform.position - miniMapCamera.transform.position).magnitude)
                    {
                        miniMapCamera.transform.position +=
                            signPlayer.transform.position - miniMapCamera.transform.position - distance;
                    }

                    miniMapCamera.transform.position = new Vector3(miniMapCamera.transform.position.x, 352f, -333f);
                    CreatSkeleton.Instance.ControlBloodShow(false);
                }

                if (!ADDUIBattle.instance.stopGame)
                    controller.Move(velocity * dt * SetConfig.changePCMoveSpeed);
                //controller.SimpleMove(velocity);
            }
        }

        void BindCamera()
        {
            mainCamera = GameObject.Find("Main Camera") as GameObject;
            mainCamera.GetComponent<ConstrainCamera>().target = this.transform;
            miniMapCamera = GameObject.Find("MiniMapCamera") as GameObject;
            distance = this.transform.position - miniMapCamera.transform.position;
            miniMapCover = GameObject.Find("MiniMapControl/miniMapCover").transform.GetComponent<Image>();
        }

        void FindUI()
        {
            if (!ADDUIBattle.instance.notUseAni3d)
                signPlayer = GameObject.Find("PlayerContrBattle/signplayer").GetComponent<Transform>();
            else
                signPlayer = GameObject.Find("PlayerControl/signplayer").GetComponent<Transform>();
        }

        void ExChangePlayerPosLeft()
        {
            if (signPlayerRotateLeft)
            {
                if (ADDUIBattle.instance.notUseAni3d)
                {
                    Vector3 tempPos = CreatSkeleton.Instance.skeletonsBattle[4].transform.position;
                    CreatSkeleton.Instance.skeletonsBattle[4].transform.position =
                        CreatSkeleton.Instance.skeletonsBattle[0].transform.position;
                    CreatSkeleton.Instance.skeletonsBattle[0].transform.position = tempPos;
                }
                else
                {
                    Vector3 tempPos = CreatSkeleton.Instance.skeletonsBattleAni[4].transform.position;
                    CreatSkeleton.Instance.skeletonsBattleAni[4].transform.position =
                        CreatSkeleton.Instance.skeletonsBattleAni[0].transform.position;
                    CreatSkeleton.Instance.skeletonsBattleAni[0].transform.position = tempPos;
                }

                //signPlayer.localRotation = Quaternion.Euler(0,0,180);
                signPlayer.localRotation = Quaternion.Euler(0, 0, -90);
                signPlayer.localPosition = new Vector3(-200, 204, 0);

                signPlayerRotateLeft = false;
                signPlayerRotateRight = true;
            }
        }

        void ExChangePlayerPosRight()
        {
            if (signPlayerRotateRight && leftCount == 1)
            {
                if (ADDUIBattle.instance.notUseAni3d)
                {
                    Vector3 tempPos = CreatSkeleton.Instance.skeletonsBattle[4].transform.position;
                    CreatSkeleton.Instance.skeletonsBattle[4].transform.position =
                        CreatSkeleton.Instance.skeletonsBattle[0].transform.position;
                    CreatSkeleton.Instance.skeletonsBattle[0].transform.position = tempPos;
                }
                else
                {
                    Vector3 tempPos = CreatSkeleton.Instance.skeletonsBattleAni[4].transform.position;
                    CreatSkeleton.Instance.skeletonsBattleAni[4].transform.position =
                        CreatSkeleton.Instance.skeletonsBattleAni[0].transform.position;
                    CreatSkeleton.Instance.skeletonsBattleAni[0].transform.position = tempPos;
                }

                //signPlayer.localRotation = Quaternion.Euler(Vector3.zero);
                signPlayer.localRotation = Quaternion.Euler(0, 0, 90);
                signPlayer.localPosition = new Vector3(0, 204, 0);


                signPlayerRotateRight = false;
                signPlayerRotateLeft = true;
            }
        }

        void ExChangeValue()
        {
            if (ADDUIBattle.instance.notUseAni3d)
            {
                for (int i = 0; i < CreatSkeleton.Instance.skeletonsBattle.Count; i++)
                {
                    //CreatSkeleton.Instance.skeletonsBattleAni.Add(CreatSkeleton.Instance.skeletonsBattle[i]) as SkeletonAnimation;
                }
            }
        }
    }
}