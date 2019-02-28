using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public partial class BattleLogic : MonoBehaviour {

    private int random;
    private bool removeRoleElement = false;
    private bool removeMonsterElement = false;
    private bool isSelectSkill = true; //判定有无选择技能
    private bool isSelectRightTarget = false; //判定有无选择正确目标
    public bool isAlreadyFinish = false; //判定是否过关
    public bool isMaxDamage = false; //发生暴击
    public bool isAlreadyWin = false; //检测是否已经胜利

    //攻击者信息
    private RoleConfig attack_roleIns;
    private float attack_roleSkillCoefficient = 1; //技能系数
    private float attack_monsterSkillCoefficient = 1; //技能系数
    private string recordSkillName;

    private RoleConfig lastAttacker;

    private float skillDamage = 1; //技能基本伤害

    //目标信息
    public RoleConfig target_roleIns;
    private float target_defense; //防御
    private float target_life; //血量
    private Image moveImage; //血量滑块
    private float target_percent; //血量百分比

    //技能信息
    public GameObject whichSkill;

    //结果信息
    public float damage;
    public float animationDurationTime = 1f; //动画持续的时间

    private SkeletonAnimation attackerAll;
    private bool attackIsRoleAll;
    private Vector3 attackerOldPosAll;

    /// <summary>
    /// 注册事件
    /// </summary>
    void RegisterEvent () {
        skill1.AddButtonListener<GameObject> (SkillClick, skill1.gameObject);
        skill2.AddButtonListener<GameObject> (SkillClick, skill2.gameObject);
        skill3.AddButtonListener<GameObject> (SkillClick, skill3.gameObject);
    }

    /// <summary>
    /// 事件函数
    /// </summary>
    void SkillClick (GameObject obj) {
        Debug.Log (roleconWhole.name);
        Debug.Log (obj.name);
        isSelectSkill = true;
        whichSkill = obj;
        FindWhichSkill (obj.name, obj);
        // recordSkillName = obj.name;

        //TODO 沉默使用普攻
        if (SkillEffect.Instance.attack_roleIns.m_RoleInfo.isSilience || SkillEffect.Instance.attack_roleIns.m_RoleInfo.isForceAttack) {
            recordSkillName = "skill1";
            return;
        }
    }

    void FindWhichSkill (string str, GameObject obj) {

        if (int.Parse (str.Substring (str.Length - 1, 1)) == 1)
            recordSkillName = "skill1";
        else if (int.Parse (str.Substring (str.Length - 1, 1)) == 2)
            recordSkillName = "skill2";
        else {
            recordSkillName = "skill3a";
        }

        Transform trans;
        if (JudgeSkillCDCanUse (null)) {
            whichSkill = obj;

            skill1.transform.Find ("sign").gameObject.SetActive (false);
            trans = whichSkill.transform.Find ("sign");
            whichSkill.transform.Find ("sign").gameObject.SetActive (true);

            if (int.Parse (str.Substring (str.Length - 1, 1)) == 1) {
                skill2.transform.Find ("sign").gameObject.SetActive (false);
                skill3.transform.Find ("sign").gameObject.SetActive (false);
            }
            if (int.Parse (str.Substring (str.Length - 1, 1)) == 2) {
                skill1.transform.Find ("sign").gameObject.SetActive (false);
                skill3.transform.Find ("sign").gameObject.SetActive (false);
            }
            if (int.Parse (str.Substring (str.Length - 1, 1)) == 3) {
                skill1.transform.Find ("sign").gameObject.SetActive (false);
                skill2.transform.Find ("sign").gameObject.SetActive (false);
            }
        }
    }

    void FindWhichSkill () {
        if (int.Parse (whichSkill.name.Substring (whichSkill.name.Length - 1, 1)) == 1)
            recordSkillName = "skill1";
        else if (int.Parse (whichSkill.name.Substring (whichSkill.name.Length - 1, 1)) == 2)
            recordSkillName = "skill2";
        else {
            recordSkillName = "skill3a";
        }
    }

    void SkillShowSign () {
        //被动取消点击效果
        if (skill2.name.Equals (SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillid2.ToString ()) && SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillCd2 == 0) {
            skill2.GetComponent<Button> ().transition = Selectable.Transition.None;
        } else if (skill3.name.Equals (SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillid3.ToString ()) && SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillCd3 == 0) {
            skill3.GetComponent<Button> ().transition = Selectable.Transition.None;
        }

        //默认的选中一技能效果
        skill1.transform.Find ("sign").gameObject.SetActive (true);
        skill2.transform.Find ("sign").gameObject.SetActive (false);
        skill3.transform.Find ("sign").gameObject.SetActive (false);
    }

    void BasicDamage (float skillCoefficient = 1f) {
        isMaxDamage = false;
        if (float.Parse (roleconWhole.m_RoleInfo.attack.ToString ()) > target_defense)
            damage = float.Parse (roleconWhole.m_RoleInfo.attack.ToString ()) - target_defense;
        else
            damage = Random.Range (30, 61);
        Debug.Log ("role damage：--" + damage);

        random = Random.Range (1, 101);
        if (random <= int.Parse (roleconWhole.m_RoleInfo.critchance.Split ('%') [0])) {
            isMaxDamage = true;
        }
        SkillEffect.Instance.BasicRoleSkillDamageNor (whichSkill.gameObject);
        Debug.LogError (SkillEffect.Instance.skillDamage);
        Debug.LogError (SkillEffect.Instance.skillCoefficient);
        IsJudgeAllDemage ();
        // if (random <= int.Parse(roleconWhole.m_RoleInfo.critchance.Split('%')[0]))
        // {
        //     Debug.LogError("随机值是：-- " + random);
        //     Debug.LogError("概率值是：-- " + int.Parse(roleconWhole.m_RoleInfo.critchance.Split('%')[0]));

        //     damage *= SkillEffect.Instance.skillDamage * SkillEffect.Instance.skillCoefficient;
        //     Debug.Log(int.Parse(roleconWhole.m_RoleInfo.critdamage.Split('%')[0]));
        // }
        // else
        // {
        //     damage *= SkillEffect.Instance.skillDamage * SkillEffect.Instance.skillCoefficient;
        //     Debug.LogError("没有出现暴击");
        // }
        // Debug.LogError(damage);

        //ToDo buff debuff 展示
        // SkillEffect.Instance.BuffShow();
    }

    void BasicDamage () {
        isMaxDamage = false;
        random = Random.Range (1, SetConfig.changeCrit);
        if (random <= int.Parse (roleconWhole.m_RoleInfo.critchance.Split ('%') [0])) {
            isMaxDamage = true;
        }
        SkillEffect.Instance.BasicRoleSkillDamageNor (whichSkill.gameObject);
        Debug.LogError ("SkillEffect.Instance.skillDamage:-- " + SkillEffect.Instance.skillDamage);
        Debug.LogError ("SkillEffect.Instance.skillCoefficient:-- " + SkillEffect.Instance.skillCoefficient);
        IsJudgeAllDemage ();
    }

    //区分群体伤害与单个伤害
    void IsJudgeAllDemage () {
        string effectid = ReadJsonfiles.Instance.skillDics[int.Parse (whichSkill.gameObject.name)].effectid1;
        string effectType = ReadJsonfiles.Instance.buffDics[(int.Parse (effectid))].effectType;
        int target = ReadJsonfiles.Instance.buffDics[(int.Parse (effectid))].target;
        if (effectType == "1" && target == 5) {
            //TODO 群体伤害
            Debug.LogError ("这里进行群体伤害计算");
            isAllDemage = true;
            AllDamage ();
        } else {
            isAllDemage = false;
            SingleDamage (target_defense);
        }
    }

    #region Hero攻击模式整合  Hero攻击开始入口
    /// <summary>
    /// 对怪物的伤害处理
    /// </summary>
    void GetMonsterTargetInfo (GameObject obj) {
        if (JudgeSkillCDCanUse ()) {
            JudgeTargetIsRight (obj);

            SkillEffect.Instance.target_roleIns = obj.GetComponent<RoleConfig> ();
            SkillEffect.Instance.savetarget_roleIns = obj.GetComponent<RoleConfig> ();
            target_roleIns = obj.GetComponent<RoleConfig> ();
            // target_roleIns = SkillEffect.Instance.target_roleIns;
            target_defense = float.Parse (target_roleIns.m_RoleInfo.defense.ToString ());

            Debug.Log ("人物攻击者：" + roleconWhole.name + "——减去目标" + obj.name);
            if (isSelectRightTarget) {
                // JudgeDistanceShowAni(obj);
                RoleJudgeSkillTargetToSelectAttackMode (obj);
            }
        }

    }
    #endregion

    void RoleJudgeSkillTargetToSelectAttackMode (GameObject obj) {
        Debug.LogError ("进行攻击的技能是：-- " + whichSkill.name);
        Skills usingSkill = ReadJsonfiles.Instance.skillDics[int.Parse (whichSkill.name)];

        if (usingSkill.releaseobj == (int) TargetType.self ||
            usingSkill.releaseobj == (int) TargetType.singleFriend) {
            Debug.LogError ("11111111" + target_roleIns);
            StartCoroutine (RoleNotChangePos (target_roleIns));
        } else if (usingSkill.releaseobj == (int) TargetType.allFriend) {
            StartCoroutine (RoleNotChangePos (target_roleIns, false));
        } else {
            JudgeDistanceShowAni (obj);
        }
    }

    void GiveRoleDefaultSkill () {
        isSelectSkill = true;
        whichSkill = skill1.gameObject;
        recordSkillName = "skill1";
        Debug.LogError ("默认技能是：-- " + whichSkill.name);

    }

    void GiveMonsterPrioritySkill () {
        //TODO 沉默使用普攻
        if (SkillEffect.Instance.attack_roleIns.m_RoleInfo.isSilience || SkillEffect.Instance.attack_roleIns.m_RoleInfo.isForceAttack) {
            whichSkill = skill1.gameObject;
            return;
        }
        //TODO 技能优先级判断处理
        if (roleconWhole.m_RoleInfo.skillid3 == 0) {
            if (skillShade[1].fillAmount == 0) whichSkill = skill2.gameObject;
            else
                whichSkill = skill1.gameObject;
        } else {
            if (skillShade[2].fillAmount != 0) {
                if (skillShade[1].fillAmount == 0) whichSkill = skill2.gameObject;
                else
                    whichSkill = skill1.gameObject;
            }
            if (skillShade[2].fillAmount == 0 && ReadJsonfiles.Instance.skillDics[roleconWhole.m_RoleInfo.skillid2].isnpcskill >
                ReadJsonfiles.Instance.skillDics[roleconWhole.m_RoleInfo.skillid3].isnpcskill)
                whichSkill = skill2.gameObject;
            else
                whichSkill = skill3.gameObject;
        }
        whichSkill = skill1.gameObject; //ceshi
        Debug.LogError ("给定的技能是：-- " + whichSkill.name);
    }

    //判断技能CD是否可用，展示选中效果
    bool JudgeSkillCDCanUse (GameObject obj) {
        float amount = whichSkill.transform.Find ("shade").GetComponent<Image> ().fillAmount;
        if (amount != 0) {
            StartCoroutine (ADDUIBattle.instance.DelayBackTween (ADDUIBattle.instance.selectRightSkill));
            return false;
        } else if (whichSkill.name.Equals (SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillid2.ToString ()) && SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillCd2 == 0) {
            return false;
        } else if (whichSkill.name.Equals (SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillid3.ToString ()) && SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillCd3 == 0)
            return false;
        else
            return true;

    }

    //判断技能CD是否可用
    //TODO 添加技能选择
    bool JudgeSkillCDCanUse () {
        float amount = whichSkill.transform.Find ("shade").GetComponent<Image> ().fillAmount;
        if (amount != 0) {
            StartCoroutine (ADDUIBattle.instance.DelayBackTween (ADDUIBattle.instance.selectRightSkill));
            return false;
        } else if (whichSkill.name.Equals (SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillid2.ToString ()) && SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillCd2 == 0) {
            if (skill1.transform.Find ("sign").gameObject.activeSelf) {
                whichSkill = skill1.gameObject;
                recordSkillName = "skill1";
            } else {
                whichSkill = skill3.gameObject;
                recordSkillName = "skill3a";
            }
            return true;
        } else if (whichSkill.name.Equals (SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillid3.ToString ()) && SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillCd3 == 0) {
            if (skill1.transform.Find ("sign").gameObject.activeSelf) {
                whichSkill = skill1.gameObject;
                recordSkillName = "skill1";
            } else {
                whichSkill = skill2.gameObject;
                recordSkillName = "skill2";
            }
            return true;
        } else
            return true;
    }

    //判断技能目标,展示提示
    void JudgeTargetIsRight (GameObject targetObj) {
        Debug.LogError ("进行攻击的技能是：-- " + whichSkill.name);
        Skills usingSkill = ReadJsonfiles.Instance.skillDics[int.Parse (whichSkill.name)];

        if (usingSkill.releaseobj == (int) TargetType.self ||
            usingSkill.releaseobj == (int) TargetType.singleFriend ||
            usingSkill.releaseobj == (int) TargetType.allFriend) {
            StartCoroutine (ShowErrorMessage (targetObj, false));
        } else {
            StartCoroutine (ShowErrorMessage (targetObj, true));
        }
    }

    IEnumerator ShowErrorMessage (GameObject targetObj, bool isSelf) {
        Vector3 oldPos = ADDUIBattle.instance.selectRightTarget.position;
        if (targetObj.tag == "Hero" && isSelf) {
            //DOTween.To(() => ADDUIBattle.instance.selectRightTarget.position, (Vector3 ver) => ADDUIBattle.instance.selectRightTarget.position = ver,
            //    targetObj.transform.position, 0.5f);
            //yield return new WaitForSeconds(1f);
            //DOTween.To(() => ADDUIBattle.instance.selectRightTarget.position, (Vector3 ver) => ADDUIBattle.instance.selectRightTarget.position = ver,
            //    oldPos, 0.5f);

            ADDUIBattle.instance.selectRightTarget.DOPlayBackwards ();
            yield return new WaitForSeconds (1f);
            ADDUIBattle.instance.selectRightTarget.DOPlayForward ();
            Debug.LogError ("请选择正确的目标进行攻击!");
            isSelectRightTarget = false;
        } else if (targetObj.tag == "Enemy" && !isSelf) {
            //DOTween.To(() => ADDUIBattle.instance.selectRightTarget.position, (Vector3 ver) => ADDUIBattle.instance.selectRightTarget.position = ver,
            //    targetObj.transform.position, 0.5f);
            //yield return new WaitForSeconds(1f);
            //DOTween.To(() => ADDUIBattle.instance.selectRightTarget.position, (Vector3 ver) => ADDUIBattle.instance.selectRightTarget.position = ver,
            //    oldPos, 0.5f);

            ADDUIBattle.GetInstance ().selectRightTarget.DOPlayBackwards ();
            yield return new WaitForSeconds (1f);
            ADDUIBattle.GetInstance ().selectRightTarget.DOPlayForward ();
            Debug.LogError ("请选择正确的目标进行攻击!");
            isSelectRightTarget = false;
        } else {
            isSelectRightTarget = true;
        }
    }

    //role attack monster 伤害计算
    float RoleToMonsterDamageCalculate () {
        // damage = 0;
        // Text showText;
        // GameObject moveImageTest;

        BasicDamage ();
        Debug.Log ("role damage：--" + damage);
        if (!isAllDemage) {
            isAllDemage = false;
            MonsterBloodsCalculate ();
        }
        return damage;
    }

    void MonsterBloodsCalculate () {
        Text showText;
        GameObject moveImageTest;

        target_life = ADDUIBattle.instance.deal.ChangeBloodTime (target_roleIns);
        target_life -= damage;
        //血量变化展示
        BloodChangedShow (target_roleIns, damage);

        if (target_life <= 0) {
            target_life = 0;
            removeMonsterElement = true;
        }

        target_roleIns.m_RoleInfo.life = target_life;
        ColorDebug.Instance.DicDebug<int, Text> (CreatSkeleton.Instance.BloodsDic, false);
        // for (int i = 0; i < CreatSkeleton.Instance.BloodsDic.Count; i++)
        // {
        if (CreatSkeleton.Instance.BloodsDic.ContainsKey (target_roleIns.iddif)) {
            CreatSkeleton.Instance.BloodsDic.TryGetValue (target_roleIns.iddif, out showText);
            CreatSkeleton.Instance.BloodsObjDic.TryGetValue (target_roleIns.iddif, out moveImageTest);
            showText.text = ADDUIBattle.instance.deal.BloodShow (target_life, target_roleIns.id);
            moveImage = moveImageTest.transform.Find ("moveImage").GetComponent<Image> ();
            moveImage.fillAmount = ADDUIBattle.instance.deal.BloodShowPrecent (target_life, target_roleIns.id);
        }
        // }
        target_life = 0f;
        BloodCalaulateShow (target_roleIns);
        if (removeMonsterElement) {
            RemoveTheDeadMonster (target_roleIns);
            removeMonsterElement = false;
        }

    }

    void RemoveTheDeadMonster (RoleConfig target_roleIns) {
        ColorDebug.Instance.RedDebug (target_roleIns.iddif, "iddf是", false);
        ColorDebug.Instance.DicDebug<int, Text> (CreatSkeleton.Instance.BloodsDic, true);
        Debug.Log (target_roleIns.gameObject.name);

        CreatSkeleton.Instance.BloodsDic.Remove (target_roleIns.iddif);
        CreatSkeleton.Instance.monsterBloods.Remove (CreatSkeleton.Instance.BloodsObjDic[target_roleIns.iddif].gameObject);
        CreatSkeleton.Instance.BloodsObjDic[target_roleIns.iddif].gameObject.SetActive (false);
        CreatSkeleton.Instance.BloodsObjDic.Remove (target_roleIns.iddif);
        CreatSkeleton.Instance.monsterSkeletonsMonsterBattleAniDic.Remove (target_roleIns.iddif); //新添加--测试技能伤害计算
        CreatSkeleton.Instance.monsterSkeletonsMonsterBattleAni.Remove (target_roleIns.GetComponent<SkeletonAnimation> ()); //新添加--测试技能伤害计算
        target_roleIns.gameObject.SetActive (false);

        ADDUIBattle.instance.battleCurrentAll.Remove (CreatSkeleton.Instance.skeletonsMonsterBattleAniDic[target_roleIns.iddif]);
        for (int i = 0; i < ADDUIBattle.instance.speedImages.Count; i++) {
            if (ADDUIBattle.instance.speedImages[i] == ADDUIBattle.instance.speedImagesDic[target_roleIns.iddif]) {
                ADDUIBattle.instance.speedImages[i].gameObject.SetActive (false);
                Destroy (ADDUIBattle.instance.speedImages[i].gameObject);
            }
        }
        ADDUIBattle.instance.speedImages.Remove (ADDUIBattle.instance.speedImagesDic[target_roleIns.iddif]);

        // CreatSkeleton.Instance.skeletonsMonsterBattleAniDic.Remove(target_roleIns.iddif);//测试

        Debug.Log ("xuetiao：--" + CreatSkeleton.Instance.BloodsDic.Count);
        Debug.Log ("怪物：--" + CreatSkeleton.Instance.monsterBloods.Count);
        CheackIsWin ();
    }

    void CheackIsWin () {
        Debug.Log (CreatSkeleton.Instance.monsterBloods.Count);
        if (CreatSkeleton.Instance.monsterBloods.Count == 0) {
            isAlreadyWin = true;
            //TOTO 胜利后buffCD-1
            AllBuffReduceOne ();

            StartCoroutine (WaitRoleBackOldPos ());

            ////TODO胜利动画 音乐播放
            //AudioManager.Instance.SendMsg(AudioMsgDetail.GetInstance.ChangeInfo((ushort)AudioId.win));
            //Debug.Log("Finial Win!");
            ////TODO胜利的界面处理
            //ADDUIBattle.instance.WinButtonClick();
            //StartCoroutine(WinTitleShow());

        }
    }

    IEnumerator WaitRoleBackOldPos () {
        yield return new WaitForSeconds (0.6f);

        //TODO胜利动画 音乐播放
        AudioManager.Instance.SendMsg (AudioMsgDetail.GetInstance.ChangeInfo ((ushort) AudioId.win));
        Debug.Log ("Finial Win!");
        if (!isAlreadyFinish)
            AudioManager.Instance.SendMsg (AudioMsgDetail.GetInstance.ChangeInfo ((ushort) AudioId.search, is_NeedWait : true));

        //TODO胜利的界面处理
        ADDUIBattle.instance.WinButtonClick ();
        StartCoroutine (WinTitleShow ());
    }

    IEnumerator WinTitleShow () {
        if (!isAlreadyFinish) {
            ADDUIBattle.instance.winTitle.DOPlayBackwards ();
            yield return new WaitForSeconds (3f);
            ADDUIBattle.instance.winTitle.DOPlayForward ();
            isAlreadyFinish = false;
        }
    }

    IEnumerator ErrorMessageShow (string str = "请先选择技能再进行战斗!!!") {
        ADDUIBattle.instance.errorMessageText.text = str;
        ADDUIBattle.instance.errorMessage.DOPlayBackwards ();
        yield return new WaitForSeconds (1f);
        ADDUIBattle.instance.errorMessage.DOPlayForward ();
    }

    /// <summary>
    /// 对人物的伤害处理(从dic中处理)
    /// </summary>
    void GetRoleTargetInfos (GameObject obj) {
        if (!ADDUIBattle.instance.speedImages.Contains (roleconWhole)) {
            StartCoroutine (DelayTime ());
            return;
        }

        int ran;
        ran = Random.Range (0, ReadJsonfiles.Instance.roleIds.Count - 5);
        if (ADDUIBattle.instance.beginthethird)
        {
            // ran = 3; //ceshi
        }
        Debug.Log ("随机值;--" + ran);
        Debug.Log ("总长;--" + ReadJsonfiles.Instance.roleIds.Count);
        //Debug.Log("内部的值;--" + CreatSkeleton.Instance.skeletonsBattleAniDic[0]);
        ColorDebug.Instance.DicDebug<int, SkeletonAnimation> (CreatSkeleton.Instance.skeletonsBattleAniDic, false);
        SkillEffect.Instance.target_roleIns = CreatSkeleton.Instance.skeletonsBattleAniDic[ReadJsonfiles.Instance.roleIds[ran]].GetComponent<RoleConfig> ();
        SkillEffect.Instance.savetarget_roleIns = CreatSkeleton.Instance.skeletonsBattleAniDic[ReadJsonfiles.Instance.roleIds[ran]].GetComponent<RoleConfig> ();
        target_roleIns = CreatSkeleton.Instance.skeletonsBattleAniDic[ReadJsonfiles.Instance.roleIds[ran]].GetComponent<RoleConfig> ();
        target_defense = float.Parse (target_roleIns.m_RoleInfo.defense.ToString ());

        Debug.Log (obj.name);
        Debug.Log ("怪物攻击者：" + roleconWhole.name + "——减去目标" + target_roleIns.name);

        JudgeDistanceShowAni (obj, false);
        //MonsterToRoleDamageCalculate();
    }

    #region NPC攻击模式整合  NPC攻击开始入口
    void MonsterJudgeSkillTargetToSelectAttackMode (GameObject obj) {
        Debug.LogError ("进行攻击的技能是：-- " + whichSkill.name);
        Skills usingSkill = ReadJsonfiles.Instance.skillDics[int.Parse (whichSkill.name)];

        if (usingSkill.releaseobj == (int) TargetType.self ||
            usingSkill.releaseobj == (int) TargetType.singleFriend) {
            SkillEffect.Instance.target_roleIns = MonsterSelectTarget (target_roleIns);
            SkillEffect.Instance.savetarget_roleIns = MonsterSelectTarget (target_roleIns);
            StartCoroutine (MonsterNotChangePos (SkillEffect.Instance.target_roleIns));
        } else if (usingSkill.releaseobj == (int) TargetType.allFriend) {
            StartCoroutine (MonsterNotChangePos (target_roleIns, false));
        } else {
            GetRoleTargetInfos (obj);
        }
    }

    #endregion

    //monster attack role 伤害计算
    float MonsterToRoleDamageCalculate () {
        damage = 0;
        Text showText;
        GameObject moveImageTest;

        BasicDamage ();
        Debug.Log ("monster damage：--" + damage);
        SkillEffect.Instance.cheeckIsStrikeBack = true;

        if (!isAllDemage) {
            isAllDemage = false;
            RoleBooldCalaulate ();
        }

        return damage;
    }

    void RoleBooldCalaulate () {
        Text showText;
        GameObject moveImageTest;

        target_life = ADDUIBattle.instance.deal.ChangeBloodTime (target_roleIns);
        target_life -= damage;
        //血量变化展示
        BloodChangedShow (target_roleIns, damage);

        if (target_life <= 0) {
            target_life = 0;
            removeRoleElement = true;
        }

        target_roleIns.m_RoleInfo.life = target_life;
        ColorDebug.Instance.DicDebug<int, Text> (CreatSkeleton.Instance.BloodsDic, false);

        if (CreatSkeleton.Instance.BloodsDic.ContainsKey (target_roleIns.id)) {
            CreatSkeleton.Instance.BloodsDic.TryGetValue (target_roleIns.id, out showText);
            CreatSkeleton.Instance.BloodsObjDic.TryGetValue (target_roleIns.id, out moveImageTest);
            showText.text = ADDUIBattle.instance.deal.BloodShow (target_life, target_roleIns.id);
            moveImage = moveImageTest.transform.Find ("moveImage").GetComponent<Image> ();
            moveImage.fillAmount = ADDUIBattle.instance.deal.BloodShowPrecent (target_life, target_roleIns.id);
        } else {
            Debug.Log ("血量没有减少！");
        }

        BloodCalaulateShow (target_roleIns);
        if (removeRoleElement) {
            ReMoveTheDeadRole ();
            removeRoleElement = false;
        }
    }

    public void RoleBooldCalaulate (RoleConfig target_roleIns, float damage) {
        Text showText;
        GameObject moveImageTest;

        target_life = ADDUIBattle.instance.deal.ChangeBloodTime (target_roleIns);
        target_life -= damage;
        //血量变化展示
        BloodChangedShow (target_roleIns, damage);

        if (target_life <= 0) {
            target_life = 0;
            removeRoleElement = true;
        }

        target_roleIns.m_RoleInfo.life = target_life;
        ColorDebug.Instance.DicDebug<int, Text> (CreatSkeleton.Instance.BloodsDic, false);

        if (CreatSkeleton.Instance.BloodsDic.ContainsKey (target_roleIns.id)) {
            CreatSkeleton.Instance.BloodsDic.TryGetValue (target_roleIns.id, out showText);
            CreatSkeleton.Instance.BloodsObjDic.TryGetValue (target_roleIns.id, out moveImageTest);
            showText.text = ADDUIBattle.instance.deal.BloodShow (target_life, target_roleIns.id);
            moveImage = moveImageTest.transform.Find ("moveImage").GetComponent<Image> ();
            moveImage.fillAmount = ADDUIBattle.instance.deal.BloodShowPrecent (target_life, target_roleIns.id);
        } else {
            Debug.Log ("血量没有减少！");
        }

        BloodCalaulateShow (target_roleIns);
        if (removeRoleElement) {
            ReMoveTheDeadRole ();
            removeRoleElement = false;
        }
    }

    //血量计算条
    void BloodCalaulateShow (RoleConfig target_roleIns) {

    }

    void ReMoveTheDeadRole () {
        //CreatSkeleton.Instance.skeletonsBattleAni.Remove(target_roleIns.GetComponent<SkeletonAnimation>());

        ColorDebug.Instance.RedDebug (target_roleIns.iddif, "iddf是", false);
        ColorDebug.Instance.DicDebug<int, Text> (CreatSkeleton.Instance.BloodsDic, true);
        Debug.Log (target_roleIns.gameObject.name);
        Debug.Log (target_roleIns.id);

        Debug.Log (CreatSkeleton.Instance.BloodsObjDic[target_roleIns.id].gameObject);
        for (int i = 0; i < CreatSkeleton.Instance.roleBloods.Count; i++) {
            Debug.Log (CreatSkeleton.Instance.roleBloods[i].name);
        }

        //CreatSkeleton.Instance.skeletonsBattleAni.Remove(CreatSkeleton.Instance.skeletonsBattleAniDic[target_roleIns.id]);
        CreatSkeleton.Instance.BloodsDic.Remove (target_roleIns.id);
        CreatSkeleton.Instance.roleBloods.Remove (CreatSkeleton.Instance.BloodsObjDic[target_roleIns.id].gameObject);

        CreatSkeleton.Instance.BloodsObjDic[target_roleIns.id].gameObject.SetActive (false);
        CreatSkeleton.Instance.BloodsObjDic.Remove (target_roleIns.id);
        CreatSkeleton.Instance.roleSkeletonsBattleAniDic.Remove (target_roleIns.id); //新添加--测试技能伤害计算
        CreatSkeleton.Instance.roleSkeletonsBattleAni.Remove (target_roleIns.GetComponent<SkeletonAnimation> ()); //新添加--测试技能伤害计算
        // CreatSkeleton.Instance.skeletonsBattleAniDic.Remove(target_roleIns.id);//测试
        ReadJsonfiles.Instance.roleIds.Remove (target_roleIns.id);
        target_roleIns.gameObject.SetActive (false);

        ADDUIBattle.instance.battleCurrentAll.Remove (CreatSkeleton.Instance.skeletonsBattleAniDic[target_roleIns.id]);
        for (int i = 0; i < ADDUIBattle.instance.speedImages.Count; i++) {
            if (ADDUIBattle.instance.speedImages[i] == ADDUIBattle.instance.speedImagesDic[target_roleIns.id]) {
                ADDUIBattle.instance.speedImages[i].gameObject.SetActive (false);
                Destroy (ADDUIBattle.instance.speedImages[i].gameObject);
            }
        }
        ADDUIBattle.instance.speedImages.Remove (ADDUIBattle.instance.speedImagesDic[target_roleIns.id]);

        ColorDebug.Instance.DicDebug<int, SkeletonAnimation> (CreatSkeleton.Instance.skeletonsBattleAniDic, false);
        Debug.Log ("xuetiao：--" + CreatSkeleton.Instance.BloodsDic.Count);
        Debug.Log ("人物：--" + CreatSkeleton.Instance.roleBloods.Count);

        //TODO 承伤主目标死亡，移除所有承伤buff
        if (target_roleIns.id == 10004) {
            DestoryProjectBuff ("isProjectTeammate");
        }

        CheackIsFail ();
    }

    void CheackIsFail () {
        Debug.Log (CreatSkeleton.Instance.roleBloods.Count);
        if (CreatSkeleton.Instance.roleBloods.Count == 0) {
            beginFllow = false;
            //TODO失败动画 音乐播放 及其它处理
            AudioManager.Instance.SendMsg (AudioMsgDetail.GetInstance.ChangeInfo ((ushort) AudioId.cancel));
            Debug.Log ("过关失败!");

            ADDUIBattle.instance.FinishGame ("过关失败,请重新开始!!!", true);
            ADDUIBattle.instance.WinButtonClick ();

        }
    }

    //匹配距离,展示动画
    void JudgeDistanceShowAni (GameObject obj, bool attackIsRole = true) {
        SkeletonAnimation attacker;

        if (attackIsRole) {
            if (CreatSkeleton.Instance.skeletonsBattleAniDic.ContainsKey (roleconWhole.id)) {
                if (CreatSkeleton.Instance.skeletonsBattleAniDic.TryGetValue (roleconWhole.id, out attacker)) {
                    //RoleMove.instance.mainCamera.GetComponent<ConstrainCamera>().target = attacker.transform;
                    StartCoroutine (ChangePos (obj, attacker, attackIsRole));
                } else
                    Debug.Log ("没有找到人物攻击者");
            } else {
                //if (attackIsRole)
                //    StartCoroutine(DelayTime(attackIsRole));
            }
        } else {
            if (CreatSkeleton.Instance.skeletonsMonsterBattleAniDic.ContainsKey (roleconWhole.iddif)) {
                if (CreatSkeleton.Instance.skeletonsMonsterBattleAniDic.TryGetValue (roleconWhole.iddif, out attacker)) {
                    //RoleMove.instance.mainCamera.GetComponent<ConstrainCamera>().target = attacker.transform;
                    StartCoroutine (ChangePos (obj, attacker, attackIsRole));
                } else
                    Debug.Log ("没有找到NPC攻击者");
            } else {
                //if (!attackIsRole)
                //    StartCoroutine(DelayTime(!attackIsRole));
            }
        }
    }

    /// <summary>
    /// 改变位置
    /// </summary>
    /// <param name="obj"></param>  目标
    /// <param name="attacker"></param>  攻击者
    /// <param name="attackIsRole"></param>
    /// <returns></returns>
    IEnumerator ChangePos (GameObject obj, SkeletonAnimation attacker, bool attackIsRole) {
        attackerOldPosAll = attacker.transform.position;
        attackerAll = attacker;
        attackIsRoleAll = attackIsRole;
        SkillEffect.Instance.attack_roleIns = attacker.GetComponent<RoleConfig> ();
        if (!attackIsRole)
            yield return new WaitForSeconds (animationDurationTime);
        beginFllow = true;
        float role_x = target_roleIns.transform.position.x - 150f;
        float monster_x = target_roleIns.transform.position.x + 150f;
        float y = target_roleIns.transform.position.y;
        float z = target_roleIns.transform.position.z;
        Vector3 targetPos;
        Vector3 attackerOldPos = attacker.transform.position;
        if (recordSkillName == "skill3a") {
            RoleSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) RoleSpineAniId.attack, (ushort) roleconWhole.m_RoleInfo.roleid, recordSkillName));
            yield return new WaitForSeconds (animationDurationTime);
        }

        if (attackIsRole) {
            targetPos = new Vector3 (role_x, y, z);
            DOTween.To (() => attacker.transform.position, a => attacker.transform.position = a, targetPos, 0.5f);
            RoleSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) RoleSpineAniId.jumpforward, (ushort) roleconWhole.m_RoleInfo.roleid));
            if (recordSkillName == "skill3a") {
                RoleSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) RoleSpineAniId.attack, (ushort) roleconWhole.m_RoleInfo.roleid, "skill3b"));
            }
        } else {
            targetPos = new Vector3 (monster_x, y, z);
            DOTween.To (() => attacker.transform.position, a => attacker.transform.position = a, targetPos, 0.5f);
            MonsterSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) MonsterSpineAniId.jumpforward, roleconWhole.iddif));
            if (recordSkillName == "skill3a") {
                MonsterSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) MonsterSpineAniId.attack, roleconWhole.iddif, "skill3b"));
            }
        }
        yield return new WaitForSeconds (0.5f);

        if (Vector3.Distance (attacker.transform.position, target_roleIns.transform.position) < 151f) {
            if (attackIsRole) {
                RoleSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) RoleSpineAniId.attack, (ushort) roleconWhole.m_RoleInfo.roleid, recordSkillName));
                if (recordSkillName == "skill3a") {
                    Debug.LogError ("等待时间2：--" + animationDurationTime);
                    // yield return new WaitForSeconds (animationDurationTime - 0.1f);
                    RoleSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) RoleSpineAniId.attack, (ushort) roleconWhole.m_RoleInfo.roleid, "skill3c"));
                }
                PassSkiiTargetSendAttacked (attackIsRole);
                // RoleSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) MonsterSpineAniId.attacked, target_roleIns.iddif));

            } else {
                FindWhichSkill ();
                MonsterSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) MonsterSpineAniId.attack, roleconWhole.iddif, recordSkillName));
                if (recordSkillName == "skill3a") {
                    Debug.LogError ("等待时间2：--" + animationDurationTime);
                    yield return new WaitForSeconds (animationDurationTime - 0.1f);
                    MonsterSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) MonsterSpineAniId.attack, roleconWhole.iddif, "skill3c"));
                }
                PassSkiiTargetSendAttacked (attackIsRole);
                // MonsterSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) RoleSpineAniId.attacked, target_roleIns.id));
            }
        }

        Debug.Log ((ushort) roleconWhole.m_RoleInfo.roleid);
        Debug.Log (roleconWhole.name);
        Debug.Log (roleconWhole.iddif);
        Debug.LogError (animationDurationTime);
        yield return new WaitForSeconds (animationDurationTime);
        Debug.LogError (animationDurationTime + "..............................................................");
        if (attackIsRole) {
            RoleToMonsterDamageCalculate ();
        } else {
            MonsterToRoleDamageCalculate ();
        }

        DOTween.To (() => attacker.transform.position, a => attacker.transform.position = a,
            attackerOldPos, 0.7f);
        if (attackIsRole) {
            RoleSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) RoleSpineAniId.jumpback, (ushort) roleconWhole.m_RoleInfo.roleid));
            yield return new WaitForSeconds (animationDurationTime);
            RoleSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) RoleSpineAniId.idle, (ushort) roleconWhole.m_RoleInfo.roleid, "normal", false));
            PassSkiiTargetSendIdle (attackIsRole);
            // MonsterSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) MonsterSpineAniId.idle, target_roleIns.iddif, "normal", false, true));
        } else {
            MonsterSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) MonsterSpineAniId.jumpback, roleconWhole.iddif));
            yield return new WaitForSeconds (animationDurationTime);
            MonsterSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) MonsterSpineAniId.idle, roleconWhole.iddif, "normal", false));
            PassSkiiTargetSendIdle (attackIsRole);
            // RoleSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) RoleSpineAniId.idle, (ushort) roleconWhole.m_RoleInfo.roleid, "normal", false));
        }
        RoleMove.instance.mainCamera.GetComponent<ConstrainCamera> ().target = ADDUIBattle.instance.PlayerContrBattle.transform;

        //TODO 展示眩晕状态
        SkillEffect.Instance.CheckIsNeedChangeDizzinessState ();

        if (SkillEffect.Instance.canStrikeBack) {
            lastAttacker = null;
            SkillEffect.Instance.canStrikeBack = false;
            SkillEffect.Instance.strikeBackOver = true;
        }
        AgainAttack ();

    }

    //特殊
    //NPC友方加血技能 single and all
    IEnumerator MonsterNotChangePos (RoleConfig target_roleIns, bool isSingle = true) {
        SkeletonAnimation attacker = null;
        if (CreatSkeleton.Instance.skeletonsMonsterBattleAniDic.ContainsKey (roleconWhole.iddif)) {
            if (CreatSkeleton.Instance.skeletonsMonsterBattleAniDic.TryGetValue (roleconWhole.iddif, out attacker)) { } else
                Debug.Log ("没有找到NPC攻击者");
        }

        attackerOldPosAll = attacker.transform.position;
        attackerAll = attacker;
        SkillEffect.Instance.attack_roleIns = attacker.GetComponent<RoleConfig> ();
        Debug.Log ("攻击者：-- " + SkillEffect.Instance.attack_roleIns.name);
        Debug.Log ("-- ：-- " + target_roleIns.transform.name);
        Debug.Log (attacker.name);
        Debug.Log ("加血模式下的等待时间：-----------------------------------------------" + animationDurationTime);
        yield return new WaitForSeconds (animationDurationTime);
        MonsterSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) MonsterSpineAniId.attack, roleconWhole.iddif, recordSkillName));
        Debug.Log ((ushort) roleconWhole.m_RoleInfo.roleid);
        Debug.Log (roleconWhole.name);
        Debug.Log (roleconWhole.iddif);
        Debug.LogError (animationDurationTime);
        yield return new WaitForSeconds (animationDurationTime);

        if (recordSkillName == "skill3a") {
            RoleSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) RoleSpineAniId.attack, (ushort) roleconWhole.m_RoleInfo.roleid, "skill3b"));
            yield return new WaitForSeconds (animationDurationTime);
            RoleSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) RoleSpineAniId.attack, (ushort) roleconWhole.m_RoleInfo.roleid, "skill3c"));
            yield return new WaitForSeconds (animationDurationTime);
        }

        Debug.LogError (animationDurationTime);
        // MonsterToRoleDamageCalculate();
        SkillEffect.Instance.BasicRoleSkillDamageNor (whichSkill.gameObject);

        if (!isSingle) {
            AllAddBlood ();
        } else {
            SingleMonsterAddBloodToEnd (target_roleIns);
            isAdd = false;
        }
        // if(isSingle)
        //     MonsterBloodCalculate(target_roleIns);
        // MonsterSpineAniManager.Instance.SendMsg(ButtonMsg.GetInstance.ChangeInfo((ushort)MonsterSpineAniId.idle, roleconWhole.iddif));
        // RoleMove.instance.mainCamera.GetComponent<ConstrainCamera>().target = ADDUIBattle.instance.PlayerContrBattle.transform;
        // AgainAttack();

    }

    void MonsterBloodCalculate (RoleConfig target_roleIns, bool isAll = false) {
        Text showText;
        GameObject moveImageTest;
        target_life = ADDUIBattle.instance.deal.ChangeBloodTime (target_roleIns);
        target_roleIns.m_RoleInfo.life = target_life;
        BloodChangedShow (target_roleIns, 0, isAll);
        ColorDebug.Instance.DicDebug<int, Text> (CreatSkeleton.Instance.BloodsDic, false);
        int testCount = 0;
        testCount++;
        if (CreatSkeleton.Instance.BloodsDic.ContainsKey (target_roleIns.iddif)) {
            CreatSkeleton.Instance.BloodsDic.TryGetValue (target_roleIns.iddif, out showText);
            CreatSkeleton.Instance.BloodsObjDic.TryGetValue (target_roleIns.iddif, out moveImageTest);
            showText.text = ADDUIBattle.instance.deal.BloodShow (target_life, target_roleIns.id);
            moveImage = moveImageTest.transform.Find ("moveImage").GetComponent<Image> ();
            moveImage.fillAmount = ADDUIBattle.instance.deal.BloodShowPrecent (target_life, target_roleIns.id);
        } else {
            Debug.Log ("血量没有减少！");
        }
    }

    //特殊
    //Hero友方加血技能  single and all
    IEnumerator RoleNotChangePos (RoleConfig target_roleIns, bool isSingle = true) {
        SkeletonAnimation attacker = null;
        if (CreatSkeleton.Instance.skeletonsBattleAniDic.ContainsKey (roleconWhole.id)) {
            if (CreatSkeleton.Instance.skeletonsBattleAniDic.TryGetValue (roleconWhole.id, out attacker)) { } else
                Debug.Log ("没有找到Hero攻击者");
        }

        attackerOldPosAll = attacker.transform.position;
        attackerAll = attacker;
        SkillEffect.Instance.attack_roleIns = attacker.GetComponent<RoleConfig> ();
        Debug.Log ("攻击者：-- " + SkillEffect.Instance.attack_roleIns.name);
        Debug.Log ("-- ：-- " + target_roleIns.transform.name);
        // yield return new WaitForSeconds(animationDurationTime);
        RoleSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) RoleSpineAniId.attack, roleconWhole.id, recordSkillName));
        Debug.LogError (animationDurationTime);

        yield return new WaitForSeconds (animationDurationTime);
        if (recordSkillName == "skill3a") {
            RoleSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) RoleSpineAniId.attack, (ushort) roleconWhole.m_RoleInfo.roleid, "skill3b"));
            yield return new WaitForSeconds (animationDurationTime);
            RoleSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) RoleSpineAniId.attack, (ushort) roleconWhole.m_RoleInfo.roleid, "skill3c"));
            yield return new WaitForSeconds (animationDurationTime);
        }

        Debug.LogError (animationDurationTime);
        // MonsterToRoleDamageCalculate();
        SkillEffect.Instance.BasicRoleSkillDamageNor (whichSkill.gameObject);
        if (!isSingle)
            AllAddBlood ();
        else {
            SingleRoleAddBloodToEnd (target_roleIns);
            isAdd = false;
        }
        // if(isSingle)
        //     RoleBloodCalculate(target_roleIns);
        // RoleSpineAniManager.Instance.SendMsg(ButtonMsg.GetInstance.ChangeInfo((ushort)RoleSpineAniId.idle, roleconWhole.id));
        // RoleMove.instance.mainCamera.GetComponent<ConstrainCamera>().target = ADDUIBattle.instance.PlayerContrBattle.transform;
        // AgainAttack();
    }

    void RoleBloodCalculate (RoleConfig target_roleIns, bool isAll = false) {
        Text showText;
        GameObject moveImageTest;
        target_life = ADDUIBattle.instance.deal.ChangeBloodTime (target_roleIns);
        target_roleIns.m_RoleInfo.life = target_life;
        BloodChangedShow (target_roleIns, 0, isAll);
        ColorDebug.Instance.DicDebug<int, Text> (CreatSkeleton.Instance.BloodsDic, false);

        if (CreatSkeleton.Instance.BloodsDic.ContainsKey (target_roleIns.id)) {
            CreatSkeleton.Instance.BloodsDic.TryGetValue (target_roleIns.id, out showText);
            CreatSkeleton.Instance.BloodsObjDic.TryGetValue (target_roleIns.id, out moveImageTest);
            showText.text = ADDUIBattle.instance.deal.BloodShow (target_life, target_roleIns.id);
            moveImage = moveImageTest.transform.Find ("moveImage").GetComponent<Image> ();
            moveImage.fillAmount = ADDUIBattle.instance.deal.BloodShowPrecent (target_life, target_roleIns.id);
        } else {
            Debug.Log ("血量没有减少！");
        }
    }

    void AgainAttack (bool attackIsRole = false) {

        if (!SkillEffect.Instance.strikeBackOver) {
            //TODO 计算技能CD
            CalculateSkillCDSingle ();
            //TOTO 计算buffCD
            SkillEffect.Instance.CalaulateBuffCD ();
            SkillEffect.Instance.RefreshBuff ();
            //角色防御计算公式
            CalculateDefense ();
            //速度计算
            CalculateSpeed ();
        } else {
            SkillEffect.Instance.attack_beginbuff.Clear ();
            SkillEffect.Instance.attack_beginbuffList.Clear ();
            SkillEffect.Instance.attack_currentnewbuffList.Clear ();
        }

        //进行反击处理
        RoleConfig roleSke;
        RoleConfig roleImage = null;
        // SkillEffect.Instance.canStrikeBack = false;//ceshi
        roleSke = SkillEffect.Instance.CheckIsCanStrikeBack ();
        if (SkillEffect.Instance.canStrikeBack) {
            StrikeBack();
            Debug.LogError ("开始反击。。。。。。。。。。。。。。。");
            lastAttacker = SkillEffect.Instance.attack_roleIns;
            whichSkill = skill1.gameObject;
            target_roleIns = lastAttacker;

            selectedOutAttackers.Clear ();
            selectedOutAttackersDic.Clear ();
            selectedOutDistances.Clear ();
            System.GC.Collect ();
            roleconWhole.transform.localPosition = new Vector3 (roleconWhole.transform.localPosition.x, startpoint.localPosition.y, roleconWhole.transform.localPosition.z);
            roleconWhole.transform.SetAsFirstSibling ();

            if (roleSke == null) return;
            for (int i = 0; i < ADDUIBattle.instance.speedImages.Count; i++) {
                if (ADDUIBattle.instance.speedImages[i].name == roleSke.name) {
                    roleImage = ADDUIBattle.instance.speedImages[i];
                }
            }
            DealHeadImage (roleImage);
            RoleAttackLogic (roleImage);
            JudgeDistanceShowAni (lastAttacker.gameObject);
            // lastAttacker = null;

            // SkillEffect.Instance.canStrikeBack = false;
            // SkillEffect.Instance.strikeBackOver = true;
            return;
        }

        if (SkillEffect.isAgainAttack) {
            SkillEffect.isAgainAttack = false;
            DealHeadImage (selectedOutAttackers[0]);
            TheSameArriveDeal ();
            return;
        }
        // roleconWhole.transform.localPosition = new Vector3 (roleconWhole.transform.localPosition.x, startpoint.localPosition.y, roleconWhole.transform.localPosition.z);
        // roleconWhole.transform.SetAsFirstSibling ();

        selectedOutAttackers.Clear ();
        selectedOutAttackersDic.Clear ();
        selectedOutDistances.Clear ();
        System.GC.Collect ();
        Debug.Log ("---------------------------------清理");
        if (!attackIsRole && !isAlreadyWin) {
            isAlreadyWin = false;
            StartCoroutine (DelayTime (!attackIsRole));
        }
    }

    void CalculateSkillCDSingle () {
        string str = whichSkill.name.Substring (whichSkill.name.Length - 1);
        bool isRole = false;
        Debug.Log (str);
        //ToDo 计算技能CD数值  用于下面得刷新CD
        if (SkillEffect.Instance.attack_roleIns.iddif == 0)
            isRole = true;
        else
            isRole = false;
        if (ReturnSkill2FillAmount (SkillEffect.Instance.attack_roleIns) < 1) {
            SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillCd2--;
        }
        if (ReturnSkill3FillAmount (SkillEffect.Instance.attack_roleIns, isRole) < 1) {
            SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillCd3--;
        }

        // if(int.Parse(whichSkill.name) == SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillid1) return;
        if (int.Parse (whichSkill.name) == SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillid2) SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillCd2 -= 1;
        if (int.Parse (whichSkill.name) == SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillid3) SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillCd3 -= 1;
        if (SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillid2 < 0) SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillid2 = 0;
        if (SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillid3 < 0) SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillid3 = 0;

        if (ReturnSkill2FillAmount (SkillEffect.Instance.attack_roleIns) < 1) {
            SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillCd2Count++;
        }
        Debug.Log ("二技能数值：----" + ReturnSkill2FillAmount (SkillEffect.Instance.attack_roleIns));
        Debug.Log ("三技能数值：----" + ReturnSkill3FillAmount (SkillEffect.Instance.attack_roleIns, isRole));
        if (ReturnSkill3FillAmount (SkillEffect.Instance.attack_roleIns, isRole) < 1) {
            SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillCd3Count++;
        }
    }

    void RefreshSkillCD (bool attackIsRole, GameObject obj, bool select = false) {
        SkeletonAnimation attacker = null;
        int skill2CdValue = 0;
        int skill3CdValue = 0;
        if (attackIsRole) {
            if (CreatSkeleton.Instance.skeletonsBattleAniDic.ContainsKey (roleconWhole.id)) {
                if (CreatSkeleton.Instance.skeletonsBattleAniDic.TryGetValue (roleconWhole.id, out attacker)) {
                    if (select)
                        StartCoroutine (ChangePos (obj, attacker, attackIsRole));
                } else
                    Debug.Log ("没有找到人物攻击者");
            } else { }
        } else {
            if (CreatSkeleton.Instance.skeletonsMonsterBattleAniDic.ContainsKey (roleconWhole.iddif)) {
                if (CreatSkeleton.Instance.skeletonsMonsterBattleAniDic.TryGetValue (roleconWhole.iddif, out attacker)) {
                    if (select)
                        StartCoroutine (ChangePos (obj, attacker, attackIsRole));
                } else
                    Debug.Log ("没有找到NPC攻击者");
            } else { }
        }
        SkillEffect.Instance.attack_roleIns = attacker.GetComponent<RoleConfig> ();
        JudgeIsNeedToRecordSkillCD ();
        if (SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillid3 != 0)
            isSkillId3 = true;
        else
            isSkillId3 = false;

        skillShade[1].fillAmount = (float) (SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillCd2) / ReadJsonfiles.Instance.skillDics[SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillid2].skillcd;
        continueBoutText[1].text = SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillCd2 + "回合";

        if (isSkillId3) {
            skillShade[2].fillAmount = (float) (SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillCd3) / ReadJsonfiles.Instance.skillDics[SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillid3].skillcd;
            continueBoutText[2].text = SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillCd3 + "回合";
        }
        if (ReturnSkill2FillAmount (SkillEffect.Instance.attack_roleIns) == 1 || SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillCd2 == 0) {
            skillShade[1].fillAmount = 0;
            continueBoutText[1].text = "";
        }
        if (ReturnSkill3FillAmount (SkillEffect.Instance.attack_roleIns, attackIsRole) == 1 || SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillCd3 == 0) {
            skillShade[2].fillAmount = 0;
            continueBoutText[2].text = "";
        }

    }

    void JudgeIsNeedToRecordSkillCD () {
        if (SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillCd2 == 0) {
            SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillCd2 = SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillCd2Count;
            SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillCd2Count = 0;
            continueBoutText[1].text = "";
        }
        if (SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillCd3 == 0) {
            SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillCd3 = SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillCd3Count;
            SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillCd3Count = 0;
            continueBoutText[2].text = "";
        }
    }

    float ReturnSkill2FillAmount (RoleConfig role) {
        return (float) (SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillCd2) / ReadJsonfiles.Instance.skillDics[SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillid2].skillcd;
    }

    float ReturnSkill3FillAmount (RoleConfig role, bool attackIsRole) {
        if (attackIsRole && isSkillId3)
            return (float) (SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillCd3) / ReadJsonfiles.Instance.skillDics[SkillEffect.Instance.attack_roleIns.m_RoleInfo.skillid3].skillcd;
        else
            return 2;
    }

    public void ColculateBoolds (RoleConfig target_roleIns, float target_life = 0f) {
        // damage = 0;
        Text showText;
        GameObject moveImageTest;
        int id;
        if (target_roleIns.tag == "Hero")
            id = target_roleIns.id;
        else {
            id = target_roleIns.iddif;
        }

        target_life = ADDUIBattle.instance.deal.ChangeBloodTime (target_roleIns);
        target_life -= damage;
        //血量变化展示
        BloodChangedShow (target_roleIns, damage);

        if (target_life <= 0) {
            target_life = 0;
            removeMonsterElement = true;
        }

        target_roleIns.m_RoleInfo.life = target_life;
        ColorDebug.Instance.DicDebug<int, Text> (CreatSkeleton.Instance.BloodsDic, false);
        Debug.LogError ("zhegeshi :-- " + target_roleIns);
        if (CreatSkeleton.Instance.BloodsDic.ContainsKey (id)) {
            CreatSkeleton.Instance.BloodsDic.TryGetValue (id, out showText);
            CreatSkeleton.Instance.BloodsObjDic.TryGetValue (id, out moveImageTest);
            showText.text = ADDUIBattle.instance.deal.BloodShow (target_life, target_roleIns.id);
            moveImage = moveImageTest.transform.Find ("moveImage").GetComponent<Image> ();
            moveImage.fillAmount = ADDUIBattle.instance.deal.BloodShowPrecent (target_life, target_roleIns.id);
        } else {
            Debug.Log ("血量没有减少！");
        }

        if (removeMonsterElement && target_roleIns.tag != "Hero") {
            RemoveTheDeadMonster (SkillEffect.Instance.target_roleIns);
            removeMonsterElement = false;
        } else if (removeMonsterElement) {
            ReMoveTheDeadRole ();
            removeMonsterElement = false;
        }
    }
    
}