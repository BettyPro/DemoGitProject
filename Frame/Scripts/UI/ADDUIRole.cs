using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;
using WinterColorDebug;
using WinterDebug;

namespace Demo
{


    public class ADDUIRole : UIBase
    {
        public static ADDUIRole instance;
        public int roleId;
        public DealDataManager deal = new DealDataManager();
        public Roles roleUse;
        public Sprite[] images = { };
        private List<SkeletonGraphic> skeletons = new List<SkeletonGraphic>();
        public List<GameObject> speedImages = new List<GameObject>();

        //Bool
        private bool isMove = false; //人物详细信息面板
        private bool isMoveSkill = false; //技能详细信息面板
        private bool isUseAni = true; //人物展示界面是否启用spine动画

        //SceneRole
        private GameObject entrancepanel;
        private UIBehaviour herobutton;
        private UIBehaviour riskbutton;


        private GameObject rolepanel;
        private UIBehaviour backentrancebutton;
        private UIBehaviour down;

        private Transform role;
        private Transform roles;


        private OnButtonPressed skill1;
        private OnButtonPressed skill2;
        private OnButtonPressed skill3;

        //Temp
        private UIBehaviour quitButton;





        //SceneRole  Text
        private Transform roleinfoshow;
        private Transform fightvalues;
        private Text rank;
        private Text fight;
        private Text detailedinfo;

        private Transform skillpopup;

        private Text skillname;
        private Text strengthenskill;
        private Text skilldes;
        private Text hunnum;

        private Text info;
        private Text nickname;
        private Text degree;
        private Text rankinfo;
        private Text intro;
        private Text fightvalue;





        //SceneBattle
        AsyncOperation asyn;
        private bool isdone;
        private GameObject pausepanel;
        private UIBehaviour pausebutton;
        private UIBehaviour backbutton;


        void Awake()
        {
            instance = this;
            UIManager.Instance.RegistGameObject(name, gameObject);
            GetBool();
            BindUIBehaviour();

            QuitButtonClik(); //-Me-- 临时
        }

        void Start()
        {
            CreatDoTweenAnimation();
        }

        private void Update()
        {
        }




        void GetBool()
        {
            SetConfig.GetInstance().isSceneRole = SceneManager.GetActiveScene().name.Equals(SetConfig.sceneRole);
            SetConfig.GetInstance().isSceneBattle = SceneManager.GetActiveScene().name.Equals(SetConfig.sceneBattle);
        }

        void BindUIBehaviour()
        {
            try
            {
                if (SetConfig.GetInstance().isSceneRole)
                {
                    rolepanel = GameObject.Find("rolepanel");
                    backentrancebutton = GameObject.Find("rolepanel/backentrancebutton").AddComponent<UIBehaviour>();
                    down = GameObject.Find("rolepanel/down").AddComponent<UIBehaviour>();
                    FindWillUseUI(); //查找
                    rolepanel.SetActive(false);

                    entrancepanel = GameObject.Find("entrancepanel");
                    herobutton = GameObject.Find("entrancepanel/left/herobutton").AddComponent<UIBehaviour>();
                    riskbutton = GameObject.Find("entrancepanel/right/riskbutton").AddComponent<UIBehaviour>();
                }
                else
                    ColorDebug.Instance.RedDebug(SetConfig.GetInstance().isSceneRole, "当前场景不是Battle", true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ColorDebug.Instance.RedDebug(this.gameObject, "面板没有找到", true);
                throw e;
            }

            RegisterEvent();
        }

        void FindWillUseUI()
        {
            fightvalues = GameObject.Find("rolepanel/Popups/fightvalues").transform;
            fightvalues.gameObject.AddComponent<DOTweenAnimation>();
            rank = GameObject.Find("rolepanel/Popups/fightvalues/rank").GetComponent<Text>();
            fight = GameObject.Find("rolepanel/Popups/fightvalues/fight").GetComponent<Text>();
            detailedinfo = GameObject.Find("rolepanel/Popups/fightvalues/detailedinfo").GetComponent<Text>();

            skillpopup = GameObject.Find("rolepanel/Popups/skillpopup").transform;
            skillpopup.gameObject.AddComponent<DOTweenAnimation>();

            skillname = GameObject.Find("skillname").GetComponent<Text>();
            strengthenskill = GameObject.Find("strengthenskill").GetComponent<Text>();
            skilldes = GameObject.Find("skilldes").GetComponent<Text>();
            hunnum = GameObject.Find("hunnum").GetComponent<Text>();

            roleinfoshow = GameObject.Find("rolepanel/roleinfoshow").transform;
            roleinfoshow.gameObject.AddComponent<DOTweenAnimation>();
            info = GameObject.Find("rolepanel/roleinfoshow/roleinfo/info").GetComponent<Text>();
            nickname = GameObject.Find("rolepanel/roleinfoshow/roleinfo/nickname").GetComponent<Text>();
            degree = GameObject.Find("rolepanel/roleinfoshow/roleinfo/degree").GetComponent<Text>();
            rankinfo = GameObject.Find("rolepanel/roleinfoshow/roleinfo/rankinfo").GetComponent<Text>();
            intro = GameObject.Find("rolepanel/roleinfoshow/roleinfo/intro").GetComponent<Text>();
            fightvalue = GameObject.Find("rolepanel/roleinfoshow/fightvalue/fightvalue").GetComponent<Text>();

            role = GameObject.Find("rolepanel/role").transform;
            if (isUseAni)
                role.GetComponent<Image>().enabled = false;
            role.GetComponent<Image>().sprite = ResourcesLoadInfos.instance.roleImages[0];
            roles = GameObject.Find("rolepanel/roles").transform;

            //CreatSkeletonInfo();//改用下面的动画单独管理
            if (isUseAni)
                CreatSkeleton.Instance.CreatSkeletonInfo(role);
            CreatRoleSingleInfo(roles);

            skill1 = GameObject.Find("rolepanel/roleinfoshow/skills/skill1").AddComponent<OnButtonPressed>();
            skill2 = GameObject.Find("rolepanel/roleinfoshow/skills/skill2").AddComponent<OnButtonPressed>();
            skill3 = GameObject.Find("rolepanel/roleinfoshow/skills/skill3").AddComponent<OnButtonPressed>();



        }

        public void CreatRoleSingleInfo(Transform trans, float x = 10f, float y = 195f, float z = 0f,
            bool fixPos = false, float width = 150f, float height = 110f, bool isRole = true, int length = 5)
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject obj =
                    GameObject.Instantiate(Resources.Load(SetConfig.roleSceneRoleSingleShow)) as GameObject;
                obj.transform.SetParent(trans);
                obj.name = "rolepre" + i.ToString();
                if (isRole)
                    obj.GetComponent<Image>().sprite = ResourcesLoadInfos.instance.roleImages[i];
                else
                    obj.GetComponent<Image>().sprite = ResourcesLoadInfos.instance.allImages[i];

                if (!fixPos)
                    obj.transform.localPosition = new Vector3(x, y - 90 * i, z);
                else
                {
                    //obj.transform.localPosition = new Vector3(x, y - 20 * i, z);
                    obj.transform.localPosition = new Vector3(x, y, z);

                }

                obj.transform.localScale = Vector3.one;
                obj.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);

                if (isRole)
                {
                    roleUse = ReadJsonfiles.Instance.roleInfos[i];
                    //RoleConfig re = obj.GetComponent<RoleConfig>();
                    RoleConfig re = obj.AddComponent<RoleConfig>();
                    re.id = ReadJsonfiles.Instance.roleIdsRoleSceneUse[i];
                    re.UpdateInfo(roleUse);

                    obj.AddComponent<UIBehaviour>()
                        .AddButtonListener<UIBehaviour>(RoleShowSingle, obj.GetComponent<UIBehaviour>(), roleUse);
                }
                else
                {
                    speedImages.Add(obj);
                }
            }
        }


        public void CreatSkeletonInfo()
        {
            SkeletonGraphic[] skeleon = Resources.LoadAll<SkeletonGraphic>(SetConfig.roleSceneSpineAniShowUI);

            for (int i = 0; i < skeleon.Length; i++)
            {
                SkeletonGraphic obj = SkeletonGraphic.Instantiate(skeleon[i]);
                obj.transform.SetParent(role);
                obj.name = "skeleon" + (i + 1).ToString();
                obj.transform.localPosition = new Vector3(-30f, -140f, 0f);
                obj.transform.localScale = Vector3.one;
                skeletons.Add(obj);
                obj.gameObject.SetActive(false);
                if (i > 1)
                    obj.transform.localScale = new Vector3(0.5f, 0.5f, 1);
            }
        }

        #region

        /// <summary>
        /// 处理文本数据
        /// </summary>
        /// <param name="str"></param>

        void GiveRoleData(int roleId = 10001)
        {
            rank.text = deal.RankInfo(roleId);
            fight.text = deal.FightInfo(roleId);
            detailedinfo.text = deal.DetailedInfo(roleId);

            info.text = deal.Info(roleId);
            nickname.text = deal.NickName(roleId);
            degree.text = deal.Degree(roleId);
            rankinfo.text = deal.RankInfo(roleId);
            intro.text = deal.Intre(roleId);
            fightvalue.text = deal.FightInfo(roleId);


        }

        public void GiveSkillData(int skillId = 1000101)
        {
            skillname.text = deal.SkillNameInfo(skillId);
            skilldes.text = deal.SkillDesInfo(skillId);
            hunnum.text = deal.HunNumInfo(skillId);
            strengthenskill.text = deal.StrengthenSkillInfo(skillId);
        }


        #endregion

        /// <summary>
        /// 事件注册
        /// </summary>
        void RegisterEvent()
        {
            Debug.Log("注册事件");
            herobutton.AddButtonListener(HeroButtonClick);
            riskbutton.AddButtonListener(RiskButtonClick);

            backentrancebutton.AddButtonListener(BackEntranceButtonClick);
            down.AddButtonListener(DownClick);
        }

        void UnRegisterEvent()
        {
            Debug.Log("移除事件");
            herobutton.RemoveButtonListener(HeroButtonClick);
            riskbutton.RemoveButtonListener(RiskButtonClick);

            backentrancebutton.RemoveButtonListener(BackEntranceButtonClick);
            down.RemoveButtonListener(DownClick);
        }


        #region 所有的事件函数

        void SkillShow(UIBehaviour ui, Roles rol)
        {
            Debug.Log(roleId);
            if (ui.name.Equals("skill1"))
            {
                rol.skillid1 = int.Parse(roleId + "01");
            }
            else if (ui.name.Equals("skill2"))
            {
                rol.skillid1 = int.Parse(roleId + "02");
            }
            else
                rol.skillid1 = int.Parse(roleId + "03");

            GiveSkillData(rol.skillid1);
            if (!isMoveSkill)
            {
                skillpopup.DOPlayForward(); //正播
                isMoveSkill = true;
            }
            else
            {
                skillpopup.DOPlayBackwards(); //倒播
                isMoveSkill = false;
            }
        }



        void RoleShow(UIBehaviour ui, Roles rol)
        {
            ui.transform.SetAsLastSibling();
            role.GetComponent<Image>().sprite = ui.transform.GetComponent<Image>().sprite;
            //-Me-- up修改为展示spineAni
            int num = ui.transform.GetComponent<RoleConfig>().id % 10000;
            for (int i = 0; i < skeletons.Count; i++)
            {
                skeletons[i].gameObject.SetActive(false);
                if (num <= 5)
                {
                    skeletons[num - 1].gameObject.SetActive(true);
                }
            }


            GiveRoleData(rol.roleid);
            GiveSkillData(rol.skillid1);
            roleId = rol.roleid;
        }

        //-Me-- down 动画单独管理的事件
        void RoleShowSingle(UIBehaviour ui, Roles rol)
        {
            ui.transform.SetAsLastSibling();
            role.GetComponent<Image>().sprite = ui.transform.GetComponent<Image>().sprite;
            //-Me-- up修改为展示spineAni
            int num = ui.transform.GetComponent<RoleConfig>().id % 10000;
            for (int i = 0; i < CreatSkeleton.Instance.skeletons.Count; i++)
            {
                CreatSkeleton.Instance.skeletons[i].gameObject.SetActive(false);
                if (num <= 5)
                {
                    CreatSkeleton.Instance.skeletons[num - 1].gameObject.SetActive(true);
                }
            }


            GiveRoleData(rol.roleid);
            GiveSkillData(rol.skillid1);
            roleId = rol.roleid;
        }

        void RoleShow(UIBehaviour ui)
        {
            ui.transform.SetAsLastSibling();
            role.GetComponent<Image>().sprite = ui.transform.GetComponent<Image>().sprite;
            Debug.Log(this.gameObject.name);

        }

        public void HeroButtonClick()
        {
            SendMsg(ButtonMsg.GetInstance.ChangeInfo((ushort) AudioId.enter));
            ColorDebug.Instance.RedDebug(herobutton, "点了hero", true);
            entrancepanel.SetActive(false);
            rolepanel.SetActive(true);
            roleId = 10001;
            role.GetComponent<Image>().sprite = ResourcesLoadInfos.instance.roleImages[0];

            if (isUseAni)
            {
                CreatSkeleton.Instance.skeletons[0].gameObject.SetActive(true);
                for (int i = 1; i < CreatSkeleton.Instance.skeletons.Count; i++)
                {
                    CreatSkeleton.Instance.skeletons[i].gameObject.SetActive(false);
                }
            }

            //TODO 读取显示文本
            GiveRoleData();
            GiveSkillData();
        }

        void BackEntranceButtonClick()
        {
            SendMsg(ButtonMsg.GetInstance.ChangeInfo((ushort) AudioId.cancel));
            ColorDebug.Instance.RedDebug(entrancepanel, "点了返回", false);
            fightvalues.DOPlayBackwards();
            roleinfoshow.DOPlayBackwards();
            isMove = false;
            rolepanel.SetActive(false);
            entrancepanel.SetActive(true);
            //SendMsg(ButtonMsg.GetInstance.ChangeInfo((ushort)AudioId.mainMenu));
        }

        void DownClick()
        {
            if (!isMove)
            {
                roleinfoshow.DOPlayForward();
                fightvalues.DOPlayForward(); //正播
                isMove = true;
            }
            else
            {
                fightvalues.DOPlayBackwards(); //倒播
                roleinfoshow.DOPlayBackwards();
                isMove = false;
            }
        }

        void RiskButtonClick()
        {
            FixWillUseValue();
            SendMsg(ButtonMsg.GetInstance.ChangeInfo((ushort) AudioId.fight));


            ColorDebug.Instance.RedDebug(riskbutton, "点了risk", false);
            CreatSkeleton.Instance.skeletons.Clear();
            CreatSkeleton.Instance.skeletonsBattle.Clear();
            CreatSkeleton.Instance.skeletonsBattleAni.Clear();
            CreatSkeleton.Instance.skeletonsMonsterBattleAni.Clear();
            CreatSkeleton.Instance.roleBloods.Clear();

            CreatSkeleton.Instance.roleSkeletonsBattleAni.Clear();
            CreatSkeleton.Instance.monsterSkeletonsMonsterBattleAni.Clear();


            SetConfig.firstPlace = new Vector3(800f, 179f, 0f);
            //SendMsg(ButtonMsg.GetInstance.ChangeInfo((ushort)AudioId.search));


            EnterBattleScene();
            SendMsg(ButtonMsg.GetInstance.ChangeInfo((ushort) AudioId.search));

        }

        void FixWillUseValue()
        {
            ReadJsonfiles.Instance.roleIds.Clear();
            for (int i = 0; i < ReadJsonfiles.Instance.roleIdsRoleSceneUse.Count; i++)
            {
                ReadJsonfiles.Instance.roleIds.Add(ReadJsonfiles.Instance.roleIdsRoleSceneUse[i]);
            }
        }

        void EnterBattleScene()
        {
            asyn = SceneManager.LoadSceneAsync(SetConfig.sceneBattle);

            //SetConfig.GetInstance().isSceneBattle = SceneManager.GetActiveScene().name.Equals(SetConfig.sceneBattle);
            //StartCoroutine(EnterBattle());
            //StopCoroutine(EnterBattle());

        }

        IEnumerator EnterBattle()
        {
            yield return new WaitForSeconds(0.03f);
            Debug.Log(asyn.isDone);
            if (asyn.isDone)
            {
                SetConfig.Instance.isSceneBattle = SceneManager.GetActiveScene().name.Equals(SetConfig.sceneBattle);
                Debug.Log(12121212);
                if (SetConfig.Instance.isSceneBattle)
                {
                    pausebutton = GameObject.Find("pausebutton").AddComponent<UIBehaviour>();
                    pausebutton.AddButtonListener(PauseButtonClick);
                    pausepanel = GameObject.Find("pausepanel");
                    backbutton = GameObject.Find("backbutton").AddComponent<UIBehaviour>();
                    backbutton.AddButtonListener(BackButtonClick);
                    Debug.Log(pausepanel + ".." + backbutton);
                    pausepanel.SetActive(false);
                    Debug.Log("tianjian le zu jian ");
                }
                else
                {
                    Debug.Log(313131);
                }

            }
            else
            {
                Debug.Log(8989809);
            }

        }

        void PauseButtonClick()
        {
            Debug.Log("暂停");
            Time.timeScale = 0f;
            if (SetConfig.Instance.isSceneBattle)
            {
                pausepanel.SetActive(true);

            }
        }

        //长按显示
        public void LongPressShow()
        {
            if (!isMoveSkill)
            {
                skillpopup.DOPlayForward(); //正播
                isMoveSkill = true;
            }
        }

        public void LongPressShowExit()
        {
            if (isMoveSkill)
            {
                skillpopup.DOPlayBackwards(); //倒播
                isMoveSkill = false;
            }
        }

        void BackButtonClick()
        {
            EnterRoleScene();
        }

        void EnterRoleScene()
        {
            SceneManager.LoadSceneAsync(SetConfig.sceneRole);
        }



        #endregion

        /// <summary>
        /// 创建动画
        /// </summary>
        void CreatDoTweenAnimation()
        {
            Tweener tween = fightvalues.DOLocalMove(new Vector3(-511f, -17f, 0f), 0.2f);
            tween.SetAutoKill(false);
            tween.Pause();
            tween = roleinfoshow.DOLocalMove(new Vector3(-1000f, -17f, 0f), 0.2f);
            tween.SetAutoKill(false);
            tween.Pause();
            tween = skillpopup.DOLocalMove(new Vector3(-511f, 82f, 0f), 0.2f);
            tween.SetAutoKill(false);
            tween.Pause();
        }



        private void OnDestroy()
        {
            ColorDebug.Instance.RedDebug(null, "enter the OnDestory", true);
            //UnRegisterEvent();
        }

        /// <summary>
        /// 临时区
        /// </summary>
        void QuitButtonClik()
        {

            quitButton = GameObject.Find("quitButton").AddComponent<UIBehaviour>();
            quitButton.AddButtonListener(() =>
                {
                    //Debug.Log(1);
                    Application.Quit();
                }
            );
        }
    }

}