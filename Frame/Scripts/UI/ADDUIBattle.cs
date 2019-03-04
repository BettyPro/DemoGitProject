using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;


public class ADDUIBattle : UIBase
{
    //测试
    private UIBehaviour winbutton;

    int count = 0;
    public bool beginthethird;

    



    public static ADDUIBattle instance;

    public static ADDUIBattle GetInstance()
    {
        return instance;
    }
    public DealDataManager deal = new DealDataManager();
    public ResourcesLoadBattleSceneInfos ResourcesLoadBattleSceneInfos = new ResourcesLoadBattleSceneInfos();
    public RoleConfig.RoleInfo role;
    

    //SceneBattle
    AsyncOperation asyn;

    public RectTransform BattlePanel;
    private UIBehaviour skill1;
    private UIBehaviour skill2;
    private UIBehaviour skill3;
    //SpeedStrip
    private Transform speedshow;
    public Dictionary<int,RoleConfig> speedImagesDic = new Dictionary<int, RoleConfig>();//速度条所有的头像
    public List<RoleConfig> speedImages = new List<RoleConfig>();//速度条所有的头像
    public List<RoleConfig> roleSpeedImages = new List<RoleConfig>();//速度条人员头像
    public List<RoleConfig> monsterSpeedImages = new List<RoleConfig>();//速度条NPC头像
    public List<SkeletonAnimation> battleCurrentAll = new List<SkeletonAnimation>();//战斗遇敌所有的角色
    public List<int> battleCurrentAllName = new List<int>();//战斗遇敌所有的角色的名字
    public List<int> battleCurrentSpeeds = new List<int>();//战斗遇敌所有的角色的速度,暂时取消(无法判定)


    private Transform PlayerPanel;
    public RectTransform bloodParent;
    public RectTransform spineEffectsParent;
    private GameObject pausepanel;
    private UIBehaviour pausebutton;
    private UIBehaviour backbutton;
    private UIBehaviour backbattlebutton;
    private Text winText;


    private Transform PlayerControl;
    private Transform MonsterPanel;
    private Transform DialogBoxPanel;
    public RectTransform MiniMapControl;
    public UIBehaviour mapBottomBackGround;

    public Transform promptMessage;//地图前进提示信息1
    public Transform goMessage;//地图前进提示信息2
    public Transform winTitle;
    public Transform errorMessage;
    public Text errorMessageText;
    public Transform selectRightTarget;//选择目标提示
    public Transform selectRightSkill;//选择目标技能提示
    

    bool firstEnter = true;

    //3d
    public Transform PlayerContrBattle;
    public Transform MonsterContrBattle;
    public bool notUseAni3d = true;

    //bool
    bool isMoveSkill = false;
    bool change = true;
    public bool stopGame = false;
    public bool clickIsOtherPanel = false;



    //长按技能显示
    private Transform skillpopup;
    private Text skillname;
    private Text strengthenskill;
    private Text skilldes;
    private Text hunnum;

    void Awake()
    {
        instance = this;
        GetBool();
        BindUIBehaviour();

    }

    void Start()
    {
        CreatDoTweenAnimation();
    }

    void Update()
    {
    }

    void GetBool()
    {
        SetConfig.GetInstance().isSceneRole = SceneManager.GetActiveScene().name.Equals(SetConfig.sceneRole);
        SetConfig.GetInstance().isSceneBattle = SceneManager.GetActiveScene().name.Equals(SetConfig.sceneBattle);
    }

    void BindUIBehaviour()
    {
        this.gameObject.AddComponent<BattleLogic>();
        ResourcesLoadBattleSceneInfos.Inis();
        try
        {
            if (SetConfig.GetInstance().isSceneBattle)
            {
                if (notUseAni3d)
                {
                    PlayerControl = GameObject.Find("CanvasBattle/PlayerPanel/PlayerControl").transform;
                    PlayerControl.gameObject.AddComponent<RoleMove>();
                    CreatSkeleton.Instance.CreatSkeletonInfo(PlayerControl, "", -300, -85, 0);

                }
                else
                {
                    PlayerContrBattle = GameObject.Find("PlayerContrBattle").transform;
                    PlayerContrBattle.gameObject.AddComponent<RoleMove>();
                    CreatSkeleton.Instance.CreatSkeletonAniInfo(PlayerContrBattle, 200, 0, 0);
                    MonsterContrBattle = GameObject.Find("MonsterContrBattle").transform;
                    //CreatSkeleton.Instance.CreatMonsterSkeletonAniInfo(MonsterContrBattle,1000f,1000,0);
                }



                PlayerPanel = GameObject.Find("CanvasBattle/PlayerPanel").transform;
                MonsterPanel = GameObject.Find("CanvasBattle/MonsterPanel").transform;
                DialogBoxPanel = GameObject.Find("CanvasBattle/DialogBoxPanel").transform;


                BattlePanel = GameObject.Find("CanvasBattle/BattlePanel").GetComponent<RectTransform>();
                //skill1 = GameObject.Find("CanvasBattle/BattlePanel/skillshow/skill1").AddComponent<UIBehaviour>();
                //skill2 = GameObject.Find("CanvasBattle/BattlePanel/skillshow/skill2").AddComponent<UIBehaviour>();
                //skill3 = GameObject.Find("CanvasBattle/BattlePanel/skillshow/skill3").AddComponent<UIBehaviour>();
                speedshow = GameObject.Find("CanvasBattle/BattlePanel/speedshow").transform;


                bloodParent = GameObject.Find("CanvasBattle/PlayerPanel/bloodParent").GetComponent<RectTransform>();
                spineEffectsParent = GameObject.Find("CanvasBattle/PlayerPanel/spineEffectsParent").GetComponent<RectTransform>();
                pausepanel = GameObject.Find("CanvasBattle/PlayerPanel/pausepanel");
                pausebutton = GameObject.Find("CanvasBattle/PlayerPanel/pausebutton").AddComponent<UIBehaviour>();
                backbutton = GameObject.Find("CanvasBattle/PlayerPanel/pausepanel/backbutton")
                    .AddComponent<UIBehaviour>();
                backbattlebutton = GameObject.Find("CanvasBattle/PlayerPanel/pausepanel/backbattlebutton")
                    .AddComponent<UIBehaviour>();
                winText = GameObject.Find("CanvasBattle/PlayerPanel/pausepanel/winText").GetComponent<Text>();
                winText.enabled = false;
                pausepanel.SetActive(false);
                BattlePanel.localPosition = new Vector3(-3000, 0, 0);

                //ceshi
                winbutton = GameObject.Find("CanvasBattle/PlayerPanel/winbutton").AddComponent<UIBehaviour>();

                //map
                MiniMapControl = GameObject.Find("CanvasMiniMap/MiniMapControl").GetComponent<RectTransform>();
                mapBottomBackGround = GameObject.Find("CanvasMiniMap/MiniMapControl/mapBottom/mapBottomBackGround").AddComponent<UIBehaviour>();

                //other
                FindOtherUI();
            }
            else
                Debug.Log("Battle场景问题！");
        }
        catch (Exception e)
        {
            ColorDebug.Instance.RedDebug(this.gameObject, "面板没有找到", true);
        }
        RegisterEvent();

    }

    void FindOtherUI()
    {
        promptMessage = GameObject.Find("DialogBoxPanel/promptMessage").transform;
        goMessage = GameObject.Find("DialogBoxPanel/goMessage").transform;
        winTitle = GameObject.Find("DialogBoxPanel/winTitle").transform;
        errorMessage = GameObject.Find("DialogBoxPanel/errorMessage").transform;
        errorMessageText = errorMessage.Find("errorMessageText").GetComponent<Text>();
        selectRightTarget = GameObject.Find("DialogBoxPanel/selectRightTarget").transform;
        selectRightSkill = GameObject.Find("DialogBoxPanel/selectRightSkill").transform;

        //技能长按显示面板
        skillpopup = GameObject.Find("CanvasBattle/PlayerPanel/skillpopup").transform;
        skillname = GameObject.Find("CanvasBattle/PlayerPanel/skillpopup/skillname").GetComponent<Text>();
        strengthenskill = GameObject.Find("CanvasBattle/PlayerPanel/skillpopup/strengthenskill").GetComponent<Text>();
        skilldes = GameObject.Find("CanvasBattle/PlayerPanel/skillpopup/skilldes").GetComponent<Text>();
        hunnum = GameObject.Find("CanvasBattle/PlayerPanel/skillpopup/hunnum").GetComponent<Text>();
        
    }

    //速度条头像
    public void CreatRoleSingleInfo(Transform trans, float x = 0f, float y = 242f, float z = 0f, float width = 52.4f, float height = 42.5f)
    {
        Roles roleUse;

        for (int i = 0; i < ResourcesLoadInfos.instance.allImages.Length; i++)
        {
            GameObject obj = GameObject.Instantiate(Resources.Load(SetConfig.roleSceneRoleSingleShow)) as GameObject;
            obj.transform.SetParent(trans);
            obj.name = "rolepre" + i.ToString();
            obj.GetComponent<Image>().sprite = ResourcesLoadInfos.instance.allImages[i];

            obj.transform.localPosition = new Vector3(x, y, z);
            obj.transform.localScale = Vector3.one;
            obj.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
            obj.GetComponent<Button>().enabled = false;

            roleUse = ReadJsonfiles.Instance.roleInfos[i];
            RoleConfig re = obj.GetComponent<RoleConfig>();
            re.id = ReadJsonfiles.Instance.roleIds[i];
            re.UpdateInfo(roleUse);

            speedImages.Add(re);
            if (i < 5)
                roleSpeedImages.Add(re);
            else
                monsterSpeedImages.Add(re);

        }
    }

    //速度条头像(跑图遇敌类暂用)
    public void CreatRoleSingleInfoToRoleMove(float x = 0f, float y = 242f, float z = 0f, float width = 52.4f, float height = 42.5f)
    {
        RoleConfig.RoleInfo roleUse;

        for (int i = 0; i < battleCurrentAll.Count; i++)
        {
            GameObject obj = GameObject.Instantiate(Resources.Load(SetConfig.roleSceneRoleSingleShow)) as GameObject;
            obj.transform.SetParent(speedshow);
            obj.name = "rolepre" + i.ToString();

            obj.transform.localPosition = new Vector3(x, y, z);
            obj.transform.localScale = Vector3.one;
            obj.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
            obj.GetComponent<Button>().enabled = false;

            roleUse = battleCurrentAll[i].GetComponent<RoleConfig>().m_RoleInfo;
            role = battleCurrentAll[i].GetComponent<RoleConfig>().m_RoleInfo;
            //RoleConfig re = obj.GetComponent<RoleConfig>();
            RoleConfig re = obj.AddComponent<RoleConfig>();
            re.id = battleCurrentAll[i].GetComponent<RoleConfig>().id;
            re.iddif = battleCurrentAll[i].GetComponent<RoleConfig>().iddif;
            re.UpdateInfo(roleUse);

            if (re.iddif == 0)
            {
                obj.name = re.id.ToString();
                speedImagesDic.Add(re.id,re);
            }

            else
            {
                obj.name = re.iddif.ToString();
                speedImagesDic.Add(re.iddif, re);
            }

            for (int j = 0; j < ResourcesLoadInfos.instance.allImages.Length; j++)
            {
                if (obj.GetComponent<RoleConfig>().id.ToString() == ResourcesLoadInfos.instance.allImages[j].name)
                {
                    obj.GetComponent<Image>().sprite = ResourcesLoadInfos.instance.allImages[j];
                    break;
                }
            }

            battleCurrentSpeeds.Add(int.Parse(roleUse.speed.ToString()));
            speedImages.Add(re);

            if (!ReadJsonfiles.Instance.monsterDic.ContainsKey(re.id))
                roleSpeedImages.Add(re);
            else
                monsterSpeedImages.Add(re);

            //if (i < 5)
            //    roleSpeedImages.Add(re);
            //else
            //    monsterSpeedImages.Add(re);

        }
    }

    public void GiveSkillData(int skillId = 1000101)
    {
        skillname.text = deal.SkillNameInfo(skillId);
        skilldes.text = deal.SkillDesInfo(skillId);
        hunnum.text = deal.HunNumInfo(skillId);
        strengthenskill.text = deal.StrengthenSkillInfo(skillId);
    }

    /// <summary>
    /// 事件注册
    /// </summary>
    void RegisterEvent()
    {
        pausebutton.AddButtonListener(PauseButtonClick);
        backbutton.AddButtonListener(BackButtonClick);
        backbattlebutton.AddButtonListener(BackBattleButtonClick);
        mapBottomBackGround.AddButtonListener(mapBottomBackGroundButtonClick);

        winbutton.AddButtonListener(WinButtonClick);

    }
    void UnRegisterEvent()
    {
        Debug.Log("移除事件");
        pausebutton.RemoveButtonListener(PauseButtonClick);
        backbutton.RemoveButtonListener(BackButtonClick);
        backbattlebutton.RemoveButtonListener(BackBattleButtonClick);
    }


    #region 所有的事件函数

    void Skill1Click()
    {
        AndroidDebug.Log(true, 11);
        Debug.Log(1);
    }

    void Skill2Click()
    {
        Debug.Log(2);
    }

    void Skill3Click()
    {
        Debug.Log(3);
    }

    public void WinButtonClick()
    {
        if (!RoleMove.instance.canWalk)
        {
            if (count == 0)
            {
                SetConfig.firstPlace = SetConfig.secondPlace;
                RoleMove.instance.canWalk = true;
                SendMsg(ButtonMsg.GetInstance.ChangeInfo((ushort)AudioId.win));
                WinButtonClickInfo();
            }
            else if (count == 1)
            {
                SetConfig.firstPlace = SetConfig.threePlace;
                RoleMove.instance.canWalk = true;
                SendMsg(ButtonMsg.GetInstance.ChangeInfo((ushort)AudioId.win));
                WinButtonClickInfo();
                beginthethird = true;//ceshi
            }

            else
            {
                SetConfig.firstPlace = SetConfig.gameOverPlace;
                Debug.Log("通关结束！");
                //TODO 通关后的处理
                BattleLogic.instance.isAlreadyFinish = true;
                RoleMove.instance.canWalk = false;
                FinishGame();
                WinButtonClickInfo();
                BattleLogic.instance.isAlreadyWin = true;
                BattleLogic.instance.ControlRoleSpineAni("idle",true,true);
            }
#region 方法替换
            //count++;
            //BattlePanel.localPosition = new Vector3(-3000, 0, 0);
            //MiniMapControl.anchoredPosition = new Vector3(640, -360, 0);
            //RoleMove.instance.playAudioClip = true;

            ////角色状态切换
            //RoleSpineAniManager.Instance.SendMsg(ButtonMsg.GetInstance.ChangeInfo((ushort)RoleSpineAniId.run));
            //RoleMove.instance.changeState = true;

            ////清理NPC
            //for (int i = 0; i < CreatSkeleton.Instance.skeletonsMonsterBattleAni.Count; i++)
            //{
            //    //CreatSkeleton.Instance.skeletonsMonsterBattleAni[i].gameObject.SetActive(false);
            //    Destroy(CreatSkeleton.Instance.skeletonsMonsterBattleAni[i].gameObject);
            //}
            //CreatSkeleton.Instance.skeletonsMonsterBattleAni.Clear();
            //CreatSkeleton.Instance.skeletonsMonsterBattleAniDic.Clear();
            ////清理NPC血条
            //for (int i = 0; i < CreatSkeleton.Instance.monsterBloods.Count; i++)
            //{
            //    //CreatSkeleton.Instance.monsterBloods[i].gameObject.SetActive(false);
            //    Destroy(CreatSkeleton.Instance.monsterBloods[i].gameObject);
            //}
            //CreatSkeleton.Instance.monsterBloods.Clear();

            //int num = battleCurrentAll.Count;
            //for (int i = 0; i < num; i++)
            //{
            //    //speedImages[i].gameObject.SetActive(false);
            //    Destroy(speedImages[i].gameObject);//清除所有头像

            //    if (i >= 5)
            //    {
            //        //battleCurrentAll[i].gameObject.SetActive(false);
            //        Destroy(battleCurrentAll[i].gameObject);//清除怪物
            //    }
            //}
            //battleCurrentAll.RemoveRange(5,num-5);
            //speedImages.Clear();
            //speedImagesDic.Clear();
            //battleCurrentSpeeds.Clear();
            //roleSpeedImages.Clear();
            //monsterSpeedImages.Clear();
            //RoleMove.instance.startBattle = true;
            //BattleLogic.instance.sortSpeedImage = true;
            //BattleLogic.instance.cancelLoop = false;
#endregion

        }
    }

    void WinButtonClickInfo(bool notNeedFix = true)
    {
        count++;
        BattlePanel.localPosition = new Vector3(-3000, 0, 0);
        MiniMapControl.anchoredPosition = new Vector3(640, -360, 0);
        RoleMove.instance.playAudioClip = true;

        //角色状态切换
        // RoleSpineAniManager.Instance.SendMsg(ButtonMsg.GetInstance.ChangeInfo((ushort)RoleSpineAniId.run));
        RoleSpineAniManager.Instance.SendMsg(ButtonMsg.GetInstance.ChangeInfo((ushort)RoleSpineAniId.idle,10001,"normal",true,true));
        RoleMove.instance.changeState = true;

        //清理NPC
        for (int i = 0; i < CreatSkeleton.Instance.skeletonsMonsterBattleAni.Count; i++)
        {
            //CreatSkeleton.Instance.skeletonsMonsterBattleAni[i].gameObject.SetActive(false);
            Destroy(CreatSkeleton.Instance.skeletonsMonsterBattleAni[i].gameObject);
        }
        CreatSkeleton.Instance.skeletonsMonsterBattleAni.Clear();
        CreatSkeleton.Instance.skeletonsMonsterBattleAniDic.Clear();
        //清理NPC血条
        for (int i = 0; i < CreatSkeleton.Instance.monsterBloods.Count; i++)
        {
            //CreatSkeleton.Instance.monsterBloods[i].gameObject.SetActive(false);
            Destroy(CreatSkeleton.Instance.monsterBloods[i].gameObject);
        }
        CreatSkeleton.Instance.monsterBloods.Clear();

        int num = battleCurrentAll.Count;
        for (int i = 0; i < num; i++)
        {
            //speedImages[i].gameObject.SetActive(false);
            Destroy(speedImages[i].gameObject);//清除所有头像

            if (i >= 5)
            {
                //battleCurrentAll[i].gameObject.SetActive(false);
                Destroy(battleCurrentAll[i].gameObject);//清除怪物
            }
        }
        //battleCurrentAll.RemoveRange(5, num - 5);//出现问题 死亡已经移除
        speedImages.Clear();
        speedImagesDic.Clear();
        battleCurrentSpeeds.Clear();
        roleSpeedImages.Clear();
        monsterSpeedImages.Clear();
        //RoleMove.instance.startBattle = true;
        RoleMove.instance.startBattle = notNeedFix;
        BattleLogic.instance.sortSpeedImage = true;
        BattleLogic.instance.cancelLoop = false;
        BattleLogic.instance.isAlreadyWin = false;
    }

    public void FinishGame(string str = "",bool changeStr = false)
    {
        AudioManager.Instance.SendMsg (AudioMsgDetail.GetInstance.ChangeInfo ((ushort) AudioId.win_loop,is_NeedWait:true));
        if (changeStr)
            winText.text = str;
        winText.enabled = true;
        pausepanel.SetActive(true);
    }

    void PauseButtonClick()
    {
        Debug.Log("暂停");
        stopGame = true;
        clickIsOtherPanel = true;
        //Time.timeScale = 0f;
        SendMsg(ButtonMsg.GetInstance.ChangeInfo((ushort)AudioId.cancel));
        SendMsg(ButtonMsg.GetInstance.ChangeInfo((ushort)AudioId.stop));

        //SendMsg(ButtonMsg.GetInstance.ChangeInfo((ushort)AudioId.win));
        //SendMsg(ButtonMsg.GetInstance.ChangeInfo((ushort)AudioId.win_loop));

        //StartCoroutine(FixClip());
        pausepanel.SetActive(true);
    }

    IEnumerator FixClip()
    {
        yield return new WaitForSeconds(5.6f);
        SendMsg(ButtonMsg.GetInstance.ChangeInfo((ushort)AudioId.win_loop));
    }

    void BackButtonClick()
    {

        SendMsg(ButtonMsg.GetInstance.ChangeInfo((ushort)AudioId.cancel));
        SendMsg(ButtonMsg.GetInstance.ChangeInfo((ushort)AudioId.mainMenu));

        //StartCoroutine(FixClipMain());
        
        //退出清理
        CreatSkeleton.Instance.skeletonsBattleAni.Clear();
        CreatSkeleton.Instance.skeletonsMonsterBattleAni.Clear();
        CreatSkeleton.Instance.skeletonsMonsterBattleAniDic.Clear();

        CreatSkeleton.Instance.roleBloods.Clear();
        CreatSkeleton.Instance.monsterBloods.Clear();
        CreatSkeleton.Instance.skeletonsBattleAniDic.Clear();

        CreatSkeleton.Instance.monsterSkeletonsMonsterBattleAni.Clear();
        CreatSkeleton.Instance.monsterSkeletonsMonsterBattleAniDic.Clear();
        CreatSkeleton.Instance.roleSkeletonsBattleAni.Clear();
        CreatSkeleton.Instance.roleSkeletonsBattleAniDic.Clear();

        CreatSkeleton.Instance.BloodsDic.Clear();
        CreatSkeleton.Instance.BloodsObjDic.Clear();
        

        EnterRoleScene();
    }

    IEnumerator FixClipMain()
    {
        //yield return new WaitForSeconds(UnitySingleton<GameApp>.Instance.GetComponent<AudioSource>().clip.length);
        yield return new WaitForSeconds(7f);
        SendMsg(ButtonMsg.GetInstance.ChangeInfo((ushort)AudioId.mainMenu));
        EnterRoleScene();
    }

    void EnterRoleScene()
    {
        SceneManager.LoadSceneAsync(SetConfig.sceneRole);
    }

    void BackBattleButtonClick()
    {
        pausepanel.SetActive(false);
        //TODO返回战斗的数据保存
        stopGame = false;
    }

    void mapBottomBackGroundButtonClick()
    {
        if(change && !ADDUIBattle.instance.stopGame)
        {
            change = false;
            MiniMapControl.anchoredPosition = new Vector3(640, -150f, 0);
        }
        else if(!change && !ADDUIBattle.instance.stopGame)
        {
            change = true;
            MiniMapControl.anchoredPosition = new Vector3(640, -360, 0);
        }
        clickIsOtherPanel = true;
    }

    void EnterBattleScene()
    {
        StartCoroutine(EnterBattle());
        StopCoroutine(EnterBattle());
    }

    IEnumerator EnterBattle()
    {
        yield return null;
    }

    //长按显示
    public void LongPressShow()
    {
        if (!isMoveSkill)
        {
            skillpopup.DOPlayBackwards();//倒播
            isMoveSkill = true;
        }
    }

    public void LongPressShowExit()
    {
        if (isMoveSkill)
        {
            skillpopup.DOPlayForward();//正播
            isMoveSkill = false;
        }
    }


    #endregion

    /// <summary>
    /// 处理文本数据
    /// </summary>
    /// <param name="str"></param>





    /// <summary>
    /// 创建动画
    /// </summary>
    void CreatDoTweenAnimation()
    {
        Tweener tween = promptMessage.DOLocalMove(new Vector3(0f, -1000f, 0f), 0.2f);
        tween.SetAutoKill(false);
        //tween.Pause();

        tween = goMessage.DOLocalMove(new Vector3(550f, -97f, 0f), 0.5f);
        tween.SetLoops<Tween>(-1, LoopType.Yoyo);
        tween.SetAutoKill(false);
        //tween.Pause();

        tween = winTitle.DOLocalMove(new Vector3(0f, 1000f, 0f), 0.2f);
        tween.SetAutoKill(false);
        //tween.Pause();

        tween = errorMessage.DOLocalMove(new Vector3(0f, 1000f, 0f), 0.2f);
        tween.SetAutoKill(false);
        //tween.Pause();

        tween = selectRightTarget.DOLocalMove(new Vector3(0f, 1000f, 0f), 0.2f);
        tween.SetAutoKill(false);
        //tween.Pause();

        tween = selectRightSkill.DOLocalMove(new Vector3(0f, 1000f, 0f), 0.2f);
        tween.SetAutoKill(false);
        //tween.Pause();

        tween = skillpopup.DOLocalMove(new Vector3(470f, -700f, 90f), 0.1f);
        tween.SetAutoKill(false);
        // tween.Pause();
        
        
    }

    public IEnumerator DelayBackTween(Transform trans)
    {
        trans.DOPlayBackwards();
        yield return new WaitForSeconds(1f);
        trans.DOPlayForward();
    }

    public IEnumerator DelayBackTweenTwo(Transform trans,Text changedBloodText,RectTransform managerValue)
    {
        // trans.DOPlayForward();
        yield return new WaitForSeconds(1f);
        changedBloodText.text = "";
        trans.DOPlayBackwards();
        BattleLogic.instance.HideValueImage(managerValue);
    }

    public void MonsterEnterBttleAni(Transform trans,Vector3 ver,float time)
    {
        Tweener tween = trans.DOLocalMove(ver, time);
        tween.SetAutoKill(false);
    }

   

    private void OnDestroy()
    {
        ColorDebug.Instance.RedDebug(null, "enter the OnDestory", true);
        speedImages.Clear();
        speedImagesDic.Clear();
        roleSpeedImages.Clear();
        monsterSpeedImages.Clear();
        //UnRegisterEvent();
    }


}

