using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;
using WinterCamera;
using WinterTools;

namespace Demo
{
    public partial class BattleLogic : MonoBehaviour
    {
        public static BattleLogic instance;

        //bool
        private bool isRole = false;
        private bool isSkillId3 = false;
        public bool testCancel = false; //速度条开始计算
        public bool cancelLoop = false; //循环直到到达终点
        public bool firstLoop = false; //找到终点
        public bool roleStartAttack = false; //开始射线检测攻击
        private bool isSpecialLoop = true;
        private bool beginSpecialAttack = true; //同时到达特殊攻击开始
        private bool playNormalAnimation = true; //用于行动动画结束播放normal
        public bool isSpineEnd = true;

        private List<float> savePos_y = new List<float>(); //保存更新位置插值列表
        private List<RoleConfig> selectedOutAttackers = new List<RoleConfig>(); //被选出的攻击者
        private List<float> selectedOutDistances = new List<float>(); //被选出的攻击者的距离比较

        private Dictionary<RoleConfig, float>
            selectedOutAttackersDic = new Dictionary<RoleConfig, float>(); //被选出的攻击者Dic

        private Dictionary<float, RoleConfig>
            attackersTimeDistancesDic = new Dictionary<float, RoleConfig>(); //攻击者的时时距离Dic

        private List<float> attackersTimeDistances = new List<float>(); //攻击者的时时距离
        private float add = 0.0001f; //相同距离的区分判断
        private int countReset = 0; //相同距离的区分判断
        public bool sortSpeedImage = true;

        private Transform startpoint;
        private Transform destination;
        private float allDistance;
        private RoleConfig.RoleInfo roleInfo = new RoleConfig.RoleInfo();
        public RoleConfig roleconWhole;
        private RoleConfig roleconSelected;

        //Skill
        private Transform skillshow;
        private UIBehaviour skill1;
        private UIBehaviour skill2;
        private UIBehaviour skill3;
        private Image skill1_Sprite;
        private Image skill2_Sprite;
        private Image skill3_Sprite;
        private Image skill1_SpriteShade;
        private Image skill2_SpriteShade;
        private Image skill3_SpriteShade;
        List<Image> skillShade = new List<Image>();
        private Text skill1_ContinueBoutText;
        private Text skill2_ContinueBoutText;
        private Text skill3_ContinueBoutText;
        List<Text> continueBoutText = new List<Text>();

        //Buff
        private Transform buffshow;

        //Head
        private Transform attackroleshow;
        private Image headImage;
        private Text headImageName;

        private Image skillImage;
        private Text skillImageName;

        //测试区
        private int count;
        private bool testBool = true;
        float testValue1;

        private void Awake()
        {
            instance = this;
            FindUI();
        }

        void Start()
        {
            CreatDoTweenAnimation();
        }

        void Update()
        {
            SpeedLoopNormal();
            RayTest();
            MyTimerTool.Instance.wtime.Update();
        }

        private void FixedUpdate()
        {
            MyTimerTool.Instance.wtime.FixUpdate();
        }

        void FindUI()
        {
            startpoint = GameObject.Find("CanvasBattle/BattlePanel/speedshow/startpoint").transform;
            destination = GameObject.Find("CanvasBattle/BattlePanel/speedshow/destination").transform;
            allDistance = Vector3.Distance(startpoint.position, destination.position);

            skillshow = GameObject.Find("CanvasBattle/BattlePanel/skillshow").transform;
            skill1 = GameObject.Find("CanvasBattle/BattlePanel/skillshow/skill1").AddComponent<UIBehaviour>();
            skill2 = GameObject.Find("CanvasBattle/BattlePanel/skillshow/skill2").AddComponent<UIBehaviour>();
            skill3 = GameObject.Find("CanvasBattle/BattlePanel/skillshow/skill3").AddComponent<UIBehaviour>();
            skill1.gameObject.AddComponent<OnButtonPressed>();
            skill2.gameObject.AddComponent<OnButtonPressed>();
            skill3.gameObject.AddComponent<OnButtonPressed>();
            skill1_Sprite = skill1.GetComponent<Image>();
            skill2_Sprite = skill2.GetComponent<Image>();
            skill3_Sprite = skill3.GetComponent<Image>();
            skill1_SpriteShade = skill1.transform.Find("shade").GetComponent<Image>();
            skill2_SpriteShade = skill2.transform.Find("shade").GetComponent<Image>();
            skill3_SpriteShade = skill3.transform.Find("shade").GetComponent<Image>();
            skillShade.Add(skill1_SpriteShade);
            skillShade.Add(skill2_SpriteShade);
            skillShade.Add(skill3_SpriteShade);

            skill1_ContinueBoutText = skill1.transform.Find("continueBout").GetComponent<Text>();
            skill2_ContinueBoutText = skill2.transform.Find("continueBout").GetComponent<Text>();
            skill3_ContinueBoutText = skill3.transform.Find("continueBout").GetComponent<Text>();
            continueBoutText.Add(skill1_ContinueBoutText);
            continueBoutText.Add(skill2_ContinueBoutText);
            continueBoutText.Add(skill3_ContinueBoutText);

            attackroleshow = GameObject.Find("CanvasBattle/BattlePanel/attackroleshow").transform;
            headImage = GameObject.Find("CanvasBattle/BattlePanel/attackroleshow/headImage").GetComponent<Image>();
            headImageName = headImage.transform.Find("headImageName").GetComponent<Text>();
            skillImage = GameObject.Find("CanvasBattle/BattlePanel/attackroleshow/skillImage").GetComponent<Image>();
            skillImageName = skillImage.transform.Find("skillImageName").GetComponent<Text>();

            buffshow = GameObject.Find("CanvasBattle/BattlePanel/buffshow").transform;

            RegisterEvent();
        }

        //区分了各个遇敌阶段的速度
        void SpeedLoopNormal()
        {
            if (Input.GetMouseButtonDown(1))
            {
                testCancel = true;
                cancelLoop = true;
                firstLoop = true;
            }

            if (Input.GetMouseButtonDown(0))
                Debug.Log(testCancel);
            if (testCancel)
            {
                float addFl = 0f;
                ReadJsonfiles.Instance.allSpeeds.Sort();
                ReadJsonfiles.Instance.allSpeeds.Reverse();
                RoleConfig roleCon;

                if (Input.GetMouseButtonDown(0))
                    Debug.Log(cancelLoop);
                ColorDebug.Instance.ListDebug<SkeletonAnimation>(ADDUIBattle.instance.battleCurrentAll, false);

                while (cancelLoop)
                {
                    for (int i = 0; i < ADDUIBattle.instance.battleCurrentAll.Count; i++)
                    {
                        roleCon = ADDUIBattle.instance.speedImages[i];

                        if (beginSpecialAttack)
                        {
                            if (firstLoop)
                            {
                                addFl = 0;
                                addFl += float.Parse(ADDUIBattle.instance.speedImages[i].m_RoleInfo.speed.ToString()) *
                                         0.0001f;
                                savePos_y.Add(addFl);
                                if (savePos_y.Count > 9)
                                {
                                    for (int z = 9; z < savePos_y.Count; z++)
                                    {
                                        savePos_y.RemoveAt(z);
                                    }
                                }
                            }
                            else
                            {
                                addFl = savePos_y[i];
                                addFl += float.Parse(ADDUIBattle.instance.speedImages[i].m_RoleInfo.speed.ToString()) *
                                         0.0001f;
                                savePos_y.RemoveAt(i);
                                savePos_y.Insert(i, addFl);
                            }
                        }

                        if (addFl - roleCon.transform.localPosition.y >= -destination.localPosition.y)
                        {
                            ConstrainCamera.instance.target = roleCon.transform;
                            selectedOutAttackers.Add(roleCon);
                            Debug.Log("选出来的攻击者个数：-------------- " + selectedOutAttackers.Count);
                            selectedOutAttackersDic.Add(roleCon, addFl - roleCon.transform.localPosition.y);
                            roleCon.transform.localPosition = new Vector3(roleCon.transform.localPosition.x,
                                destination.localPosition.y, roleCon.transform.localPosition.z);

                            //同时到达后跳过增加计算距离
                            if (selectedOutAttackers.Count > 1)
                                beginSpecialAttack = false;
                            else
                                beginSpecialAttack = true;

                            cancelLoop = false;
                        }
                        else
                        {
                            roleCon.transform.localPosition = new Vector3(roleCon.transform.localPosition.x,
                                -addFl + roleCon.transform.localPosition.y, roleCon.transform.localPosition.z);
                        }

                        ADDUIBattle.instance.speedImages.RemoveAt(i);
                        ADDUIBattle.instance.speedImages.Insert(i, roleCon);
                    }

                    for (int j = 0; j < savePos_y.Count; j++)
                    {
                        if (ADDUIBattle.instance.speedImages[j].transform.localPosition.y >=
                            -destination.localPosition.y)
                        {
                            cancelLoop = false;
                            savePos_y.Clear();
                            break;
                        }
                        else
                        {
                            firstLoop = false;
                            break;
                        }
                    }
                }

                testCancel = false;
                TheSameArriveDeal();
                System.GC.Collect();
            }
        }

        /// <summary>
        /// 包含特殊情况处理(同时到达)
        /// </summary>
        void TheSameArriveDeal()
        {
            float temp;
            int randomInt;
            RoleConfig role;

            if (sortSpeedImage)
            {
                DealAttackImageSortShow();
                sortSpeedImage = false;
            }

            if (selectedOutAttackers.Count >= 2)
            {
                //同时到达攻击
                for (int i = 0; i < selectedOutAttackers.Count; i++)
                {
                    if (selectedOutAttackersDic.ContainsKey(selectedOutAttackers[i]))
                    {
                        selectedOutAttackersDic.TryGetValue(selectedOutAttackers[i], out temp);
                        Debug.Log("temp:== " + temp);
                        selectedOutDistances.Add(temp);
                    }
                }

                selectedOutDistances.Sort();
                selectedOutDistances.Reverse();

                for (int j = 0; j < selectedOutDistances.Count; j++)
                {
                    if (selectedOutDistances[j] == selectedOutDistances[j + 1] && isSpecialLoop)
                    {
                        randomInt = Random.Range(0, selectedOutDistances.Count);
                        role = selectedOutAttackers[randomInt];
                        selectedOutDistances.RemoveAt(randomInt);
                    }
                    else
                    {
                        role = selectedOutAttackers[j];
                        selectedOutDistances.RemoveAt(j);
                    }

                    DealHeadImage(role);
                    RoleAttackLogic(role);
                    MonsterAttackLogic(role);

                    if (selectedOutDistances.Count >= 2)
                    {
                        j = j - 1;
                    }

                    if (selectedOutDistances.Count == 1)
                    {
                        isSpecialLoop = false;
                        j = 0;
                    }

                    if (selectedOutDistances.Count == 0)
                        break;
                    break;
                }
            }
            else
            {
                DealHeadImage(selectedOutAttackers[0]);
                //正常攻击
                RoleAttackLogic(selectedOutAttackers[0]);
                MonsterAttackLogic(selectedOutAttackers[0]);
            }
        }

        void DealAttackImageSortShow()
        {
            float distance;
            RoleConfig role;
            bool cancle = true;
            for (int i = 0; i < ADDUIBattle.instance.speedImages.Count; i++)
            {
                distance = ADDUIBattle.instance.speedImages[i].GetComponent<RectTransform>().localPosition.y;
                ADDUIBattle.instance.speedImages[i].distance = distance;

                if (i == ADDUIBattle.instance.speedImages.Count - 1)
                {
                    cancle = false;
                }

                if (cancle)
                {
                    if (ADDUIBattle.instance.speedImages[i].GetComponent<RectTransform>().localPosition.y ==
                        ADDUIBattle.instance.speedImages[i + 1].GetComponent<RectTransform>().localPosition.y)
                    {
                        distance += add;
                        add += 0.0001f;
                    }
                }

                attackersTimeDistances.Add(distance);
                attackersTimeDistancesDic.Add(distance, ADDUIBattle.instance.speedImages[i]);
            }

            attackersTimeDistances.Sort();
            attackersTimeDistances.Reverse();
            for (int i = 0; i < ADDUIBattle.instance.speedImages.Count; i++)
            {
                if (attackersTimeDistancesDic.ContainsKey(attackersTimeDistances[i]))
                {
                    attackersTimeDistancesDic.TryGetValue(attackersTimeDistances[i], out role);
                    role.transform.SetSiblingIndex(i + 2);
                }
            }

            Debug.Log(attackersTimeDistances.Count);
            Debug.Log(attackersTimeDistancesDic.Count);
            attackersTimeDistances.Clear();
            attackersTimeDistancesDic.Clear();
            countReset++;
            if (countReset == 125)
            {
                countReset = 0;
                add = 0.0001f;
            }
        }

        void RoleAttackLogic(RoleConfig roleCon)
        {
            roleconWhole = roleCon;
            for (int j = 0; j < ADDUIBattle.instance.roleSpeedImages.Count; j++)
            {
                if (roleCon.m_RoleInfo.roleid == ADDUIBattle.instance.roleSpeedImages[j].m_RoleInfo.roleid)
                {
                    isRole = true;
                    RoleSpineAniManager.Instance.SendMsg(ButtonMsg.GetInstance.ChangeInfo(
                        (ushort) RoleSpineAniId.action,
                        (ushort) roleconWhole.m_RoleInfo.roleid, "start", false, false));
                   
                    
                    StartCoroutine(DelaySomeTimeToPlay(1));
                    //BUFF检查计算

                    //TODO 展示技能UI
                    ShowSkillUI(roleCon);
                    //TODO BUFF检查计算
                    CheckBeginBuff();
                   

                    //给一个默认的技能
                    GiveRoleDefaultSkill();
                    //TODO 人物攻击逻辑

                    roleStartAttack = true;
                    Debug.Log("该谁进行攻击：-- " + roleCon.m_RoleInfo.roleid);
                }
            }
        }

        void ShowSkillUI(RoleConfig roleCon, bool isRole = true)
        {
            ColorDebug.Instance.DicDebug(ADDUIBattle.instance.ResourcesLoadBattleSceneInfos.skillImageDics,
                false);
            RefreshSkillCD(isRole, roleCon.gameObject);
            Sprite spr;
            if (ADDUIBattle.instance.ResourcesLoadBattleSceneInfos.skillImageDics.ContainsKey(roleCon.m_RoleInfo
                .skillid1))
            {
                ADDUIBattle.instance.ResourcesLoadBattleSceneInfos.skillImageDics.TryGetValue(
                    roleCon.m_RoleInfo.skillid1,
                    out spr);
                skill1_Sprite.sprite = spr;
                skill1_Sprite.name = roleCon.m_RoleInfo.skillid1.ToString();
            }

            if (ADDUIBattle.instance.ResourcesLoadBattleSceneInfos.skillImageDics.ContainsKey(roleCon.m_RoleInfo
                .skillid2))
            {
                ADDUIBattle.instance.ResourcesLoadBattleSceneInfos.skillImageDics.TryGetValue(
                    roleCon.m_RoleInfo.skillid2,
                    out spr);
                skill2_Sprite.sprite = spr;
                skill2_Sprite.name = roleCon.m_RoleInfo.skillid2.ToString();
            }

            if (ADDUIBattle.instance.ResourcesLoadBattleSceneInfos.skillImageDics.ContainsKey(roleCon.m_RoleInfo
                .skillid3))
            {
                ADDUIBattle.instance.ResourcesLoadBattleSceneInfos.skillImageDics.TryGetValue(
                    roleCon.m_RoleInfo.skillid3,
                    out spr);
                skill3_Sprite.sprite = spr;
                skill3_Sprite.name = roleCon.m_RoleInfo.skillid3.ToString();
            }

            if (isRole && !SkillEffect.Instance.canStrikeBack)
                skillshow.DOPlayForward();

            SkillShowSign();
        }

        IEnumerator WaitFixPos(RoleConfig roleCon)
        {
            yield return new WaitForSeconds(1f);
            roleCon.transform.localPosition = new Vector3(roleCon.transform.localPosition.x, startpoint.localPosition.y,
                roleCon.transform.localPosition.z);
            roleCon.transform.SetAsFirstSibling();
        }

        public void MonsterAttackLogic(RoleConfig roleCon)
        {
            roleconWhole = roleCon;
            if (ADDUIBattle.instance.stopGame)
                return;
            for (int j = 0; j < ADDUIBattle.instance.monsterSpeedImages.Count; j++)
            {
                if (roleCon.m_RoleInfo.roleid == ADDUIBattle.instance.monsterSpeedImages[j].m_RoleInfo.roleid)
                {
                    //TODO 展示技能UI
                    ShowSkillUI(roleCon, false);
                    //BUFF检查计算
                    CheckBeginBuff();

                    if (SkillEffect.Instance.attack_roleIns.m_RoleInfo.isDizziness)
                    {
                        StartCoroutine(DelaySomeTime(2f));
                    }
                    else
                    {
                        MonsterSpineAniManager.Instance.SendMsg(
                            ButtonMsg.GetInstance.ChangeInfo((ushort) MonsterSpineAniId.action, roleconWhole.iddif,
                                "start", false, false));
                        // StartCoroutine (WaitMonsterActionAni (roleCon));

                        // //TODO 展示技能UI
                        // ShowSkillUI (roleCon, false);

                        // //BUFF检查计算
                        // CheckBeginBuff ();

                        //判断技能优先级,给定技能
                        GiveMonsterPrioritySkill();

                        //TODO 怪物攻击逻辑
                        // GetRoleTargetInfos(roleCon.gameObject);
                        MonsterJudgeSkillTargetToSelectAttackMode(roleCon.gameObject);
                    }

                    Debug.Log("该谁进行攻击：-- " + roleCon.m_RoleInfo.roleid);
                    break;
                }
            }
        }

        void RayTest()
        {
            if (roleStartAttack)
            {
                if (Input.GetMouseButtonDown(0) && !ADDUIBattle.instance.stopGame)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    bool hitInfo = Physics.Raycast(ray, out hit, 10000f);
                    Debug.DrawRay(Input.mousePosition, ray.direction, Color.red * 100);
                    if (hit.collider != null)
                    {
                        Debug.Log(11);
                        if (isSelectSkill && !ADDUIBattle.instance.stopGame)
                        {
                            GetMonsterTargetInfo(hit.collider.gameObject);
                            // RoleJudgeSkillTargetToSelectAttackMode(hit.collider.gameObject);
                            if (isSelectRightTarget)
                            {
                                isSelectSkill = false;
                                isSelectRightTarget = false;
                                skillshow.DOPlayBackwards();
                                DealAttackingSkillImage();
                                //StartCoroutine(DelayTime());
                                Debug.Log(hit.collider.gameObject.name);
                            }
                        }
                        else
                        {
                            //TODO 提示选择技能面板
                            Debug.Log("请选择技能进行攻击");
                            // StartCoroutine (ErrorMessageShow ());//暂时去除
                        }
                    }
                    else
                    {
                        Debug.Log("没有检测到物体");
                    }
                }
            }
        }

        IEnumerator DelayTime(bool needWait = false)
        {
            if (needWait)
                yield return new WaitForSeconds(0.5f);
            else
                yield return new WaitForSeconds(2f);

            if (!isAlreadyWin_delay)
            {
                isAlreadyWin_delay = false;
                roleconWhole.transform.localPosition = new Vector3(roleconWhole.transform.localPosition.x,
                    startpoint.localPosition.y, roleconWhole.transform.localPosition.z);
                roleconWhole.transform.SetAsFirstSibling();

                testCancel = true;
                cancelLoop = true;
                firstLoop = true;
                roleStartAttack = false;
//            beginFllow = false;

                SkillEffect.Instance.strikeBackOver = false;
            }
        }

        IEnumerator DelaySomeTime(float value)
        {
            yield return new WaitForSeconds(value);
            AgainAttack();
        }

        IEnumerator DelaySomeTimeToPlay(float value = 0f)
        {
            yield return new WaitForSeconds(animationDurationTime);
            if (roleconWhole.m_RoleInfo.roleid == 10001)
                RoleSpineAniManager.Instance.SendMsg(ButtonMsg.GetInstance.ChangeInfo((ushort) RoleSpineAniId.idle,
                    (ushort) roleconWhole.m_RoleInfo.roleid,
                    "normal", all_action: false, is_loop: true));
            else
                RoleSpineAniManager.Instance.SendMsg(ButtonMsg.GetInstance.ChangeInfo((ushort) RoleSpineAniId.idle,
                    (ushort) roleconWhole.m_RoleInfo.roleid,
                    "normal", all_action: false, is_loop: true));
        }

        void DealHeadImage(RoleConfig roleCon)
        {
            // headImage.sprite = roleCon.GetComponent<Image>().sprite;//攻击者头像展示
            ColorDebug.Instance.DicDebug(
                ADDUIBattle.instance.ResourcesLoadBattleSceneInfos.roleActionImagesDics, false);

            if (ADDUIBattle.instance.ResourcesLoadBattleSceneInfos.roleActionImagesDics.ContainsKey(
                roleCon.id.ToString()))
            {
                headImage.sprite =
                    ADDUIBattle.instance.ResourcesLoadBattleSceneInfos.roleActionImagesDics[roleCon.id.ToString()];
            }

            headImage.transform.DOPlayBackwards();
            headImage.transform.DOPlayForward();
            headImageName.text = roleCon.m_RoleInfo.nickname;
            skillImageName.text = roleCon.m_RoleInfo.nickname;
        }

        void DealAttackingSkillImage()
        {
            if (ADDUIBattle.instance.ResourcesLoadBattleSceneInfos.skillImageDics.ContainsKey(
                int.Parse(whichSkill.name)))
            {
                skillImage.sprite =
                    ADDUIBattle.instance.ResourcesLoadBattleSceneInfos.skillImageDics[int.Parse(whichSkill.name)];
            }
            skillImage.transform.DOPlayForward();
        }

        /// <summary>
        /// 创建动画
        /// </summary>
        void CreatDoTweenAnimation()
        {
            Tweener tween = skillshow.DOLocalMove(new Vector3(-155 + 640f, 45 - 360f, 0f), 0.2f);
            tween.SetAutoKill(false);
            tween.Pause();

            tween = headImage.transform.DOLocalMove(new Vector3(-80f, 40f, 0f), 0.3f);
            //tween.SetLoops<Tween>(-1, LoopType.Yoyo);
            tween.SetAutoKill(false);
            tween.Pause();

            tween = skillImage.transform.DOLocalMove(new Vector3(-156f, 557f, 0f), 0.3f);
            //tween.SetLoops<Tween>(-1, LoopType.Yoyo);
            tween.SetAutoKill(false);
            tween.Pause();
        }
    }
}