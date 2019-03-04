using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class SkillEffect : Singleton<SkillEffect> {

    //bool
    public static bool isAgainAttack = false; //再动一回合
    public static bool isForceAttack = false; //被嘲讽
    public static bool isMagicNail = false; //魔法钉
    public static bool removeAllDebuff = false; //移除所有DeBuff
    public static bool isDizziness = false; //眩晕
    public static bool isSilience = false; //沉默
    public static bool isAttackAdd = false;
    public static bool isAttackReduce = false;
    public static bool isDefenceAdd = false;
    public static bool isDefenceReduce = false;
    public static bool isSpeedReduce = false;
    public static bool isProjectTeammate = false;

    bool CheckIsChildHaveOwn = true;
    public bool[] checkBools = {
        isForceAttack,
        isMagicNail,
        isDizziness,
        isSilience,
        isAttackAdd,
        isAttackReduce,
        isDefenceAdd,
        isDefenceReduce,
        isSpeedReduce,
        isProjectTeammate
    };
    string[] checkBoolsIn = {
        "isForceAttack",
        "isMagicNail",
        "isDizziness",
        "isSilience",
        "isAttackAdd",
        "isAttackReduce",
        "isDefenceAdd",
        "isDefenceReduce",
        "isSpeedReduce",
        "isProjectTeammate"
    };

    string[] checkNamesIn = {
        "ForceAttack",
        "MagicNail",
        "Dizziness",
        "Silience",
        "AttackAdd",
        "AttackReduce",
        "DefenceAdd",
        "DefenceReduce",
        "SpeedReduce"
    };

    public float skillDamage = 1; //暴击时物理伤害系数
    public float skillCoefficient = 1; //暴击时技能伤害系数
    private bool canExecute = false; //判断发动概率
    private bool canSecondDamage = false; //二次攻击
    public bool cheeckIsStrikeBack = false; //检查是否发动反击
    public bool canStrikeBack = false; //发动反击
    public bool strikeBackOver = false; //发动反击
    public string effectId = "";

    //目标
    public RoleConfig target_roleIns; //目标身上的
    public RoleConfig savetarget_roleIns; //目标身上的,预留区分使用
    public float target_life_old; //血量
    public bool isAll = false;
    public bool isAllAddBlood = false;
    public List<float> save_target_life_old = new List<float> ();

    //攻击者
    public RoleConfig attack_roleIns; //攻击者身上的
    public Transform[] buffCkilds;
    public Transform[] attack_buffCkilds;
    public Transform[] target_buffCkilds;

    //obj 攻击者技能，技能图片的
    //EffectType_One 攻击者 这个是速度条图片的
    public float BasicRoleSkillDamage (GameObject obj, RoleConfig EffectType_One) {
        effectId = "";
        if (ReadJsonfiles.Instance.skillDics.ContainsKey (int.Parse (obj.name))) {
            for (int i = 0; i < 4; i++) {
                switch (i) {
                    case 0:
                        effectId = ReadJsonfiles.Instance.skillDics[int.Parse (obj.name)].effectid1;
                        break;
                    case 1:
                        effectId = ReadJsonfiles.Instance.skillDics[int.Parse (obj.name)].effectid2;
                        break;
                    case 2:
                        effectId = ReadJsonfiles.Instance.skillDics[int.Parse (obj.name)].effectid3;
                        break;
                    case 3:
                        effectId = ReadJsonfiles.Instance.skillDics[int.Parse (obj.name)].effectid4;
                        break;
                    default:
                        break;
                }
            }
            if (effectId != null && ReadJsonfiles.Instance.buffDics.ContainsKey (int.Parse (effectId))) {
                if (ReadJsonfiles.Instance.buffDics[(int.Parse (ReadJsonfiles.Instance
                        .skillDics[int.Parse (obj.name)].effectid1))].effectType.Equals ("1")) {
                    skillDamage = ReadJsonfiles.Instance.buffDics[(int.Parse (ReadJsonfiles.Instance
                        .skillDics[int.Parse (obj.name)].effectid1))].value1 / 100;
                    Debug.LogError ("技能倍数：-- " + skillDamage);
                    Debug.LogError ("技能value1：-- " + ReadJsonfiles.Instance.buffDics[(int.Parse (ReadJsonfiles.Instance
                        .skillDics[int.Parse (obj.name)].effectid1))].value1);
                    Debug.LogError ("技能value1：-- " + (float) ReadJsonfiles.Instance.buffDics[(int.Parse (ReadJsonfiles.Instance
                        .skillDics[int.Parse (obj.name)].effectid1))].value1 / 100);
                }
            }

        }
        return skillDamage;
    }

    //obj  攻击技能图片的
    public void BasicRoleSkillDamageNor (GameObject obj) {
        //TODO 技能的效果判断  计算伤害
        effectId = "";
        if (ReadJsonfiles.Instance.skillDics.ContainsKey (int.Parse (obj.name))) {
            for (int i = 0; i < 4; i++) {
                switch (i) {
                    case 0:
                        effectId = ReadJsonfiles.Instance.skillDics[int.Parse (obj.name)].effectid1;
                        break;
                    case 1:
                        effectId = ReadJsonfiles.Instance.skillDics[int.Parse (obj.name)].effectid2;
                        break;
                    case 2:
                        effectId = ReadJsonfiles.Instance.skillDics[int.Parse (obj.name)].effectid3;
                        break;
                    case 3:
                        effectId = ReadJsonfiles.Instance.skillDics[int.Parse (obj.name)].effectid4;
                        break;
                    default:
                        break;
                }
                if (effectId == "")
                    break;
                if (effectId != null && ReadJsonfiles.Instance.buffDics.ContainsKey (int.Parse (effectId))) {
                    JudgeTargetType ();
                    // JudgeEffectType ();
                }
            }
            //TODO做血量计算
            // BattleLogic.instance.ColculateBoolds (target_roleIns,target_life);
        }
    }

    //whichSkill 攻击skill
    void JudgeEffectType () {
        // skillDamage = 1;
        // skillCoefficient = 1;

        JudgeOnSetProb ();
        if (canExecute) {
            JudgeEffectTypeFun ();
            canExecute = false;
        }
    }

    void JudgeEffectTypeFun () {
        string str = ReadJsonfiles.Instance.buffDics[(int.Parse (effectId))].effectType;
        switch (int.Parse (str)) {
            case (int) EffectType.one:
                EffectType_One (BattleLogic.instance.roleconWhole);
                break;
            case (int) EffectType.two:
                if (isAll) isAllAddBlood = true;
                EffectType_Two (BattleLogic.instance.roleconWhole);
                break;
            case (int) EffectType.three:
                EffectType_Three (BattleLogic.instance.roleconWhole);
                break;
            case (int) EffectType.four:
                EffectType_Four (BattleLogic.instance.roleconWhole);
                break;
            case (int) EffectType.five:
                EffectType_Five (BattleLogic.instance.roleconWhole);
                break;
            case (int) EffectType.six:
                EffectType_Six (BattleLogic.instance.roleconWhole);
                break;
            case (int) EffectType.seven:
                EffectType_Seven (BattleLogic.instance.roleconWhole);
                break;
            case (int) EffectType.eight:
                EffectType_Eight (BattleLogic.instance.roleconWhole);
                break;
            case (int) EffectType.nine:
                EffectType_Nine (BattleLogic.instance.roleconWhole);
                break;
            case (int) EffectType.ten:
                EffectType_Ten (BattleLogic.instance.roleconWhole);
                break;
            case (int) EffectType.eleven:
                EffectType_Eleven (BattleLogic.instance.roleconWhole);
                break;
            case (int) EffectType.twelve:
                EffectType_Twelve (BattleLogic.instance.roleconWhole);
                break;
            case (int) EffectType.thirteen:
                EffectType_Thirteen (BattleLogic.instance.roleconWhole);
                break;
            case (int) EffectType.fourteen:
                EffectType_Fourteen (BattleLogic.instance.roleconWhole);
                break;
            case (int) EffectType.fifteen:
                EffectType_Fifteen (BattleLogic.instance.roleconWhole);
                break;
            case (int) EffectType.sixteen:
                EffectType_Sixteen (BattleLogic.instance.roleconWhole);
                break;
            case (int) EffectType.seventeen:
                EffectType_Seventeen (BattleLogic.instance.roleconWhole);
                break;
            case (int) EffectType.eightteen:
                EffectType_Eightteen (BattleLogic.instance.roleconWhole);
                break;
            case (int) EffectType.nineteen:
                EffectType_Nineteen (BattleLogic.instance.roleconWhole);
                break;
            case (int) EffectType.twenty:
                EffectType_Twenty (BattleLogic.instance.roleconWhole);
                break;
            case (int) EffectType.twentyone:
                EffectType_Twentyone (BattleLogic.instance.roleconWhole);
                break;
            case (int) EffectType.twentytwo:
                EffectType_Twentytwo (BattleLogic.instance.roleconWhole);
                break;
            case (int) EffectType.twentythree:
                EffectType_Twentythree (BattleLogic.instance.roleconWhole);
                break;
            case (int) EffectType.twentyfour:
                EffectType_Twentyfour (BattleLogic.instance.roleconWhole);
                break;
            case (int) EffectType.twentyfive:
                EffectType_Twentyfive (BattleLogic.instance.roleconWhole);
                break;
            default:
                Debug.LogError ("没有找到匹配的类型");
                break;
        }
        BuffShow ();
    }
    void JudgeOnSetProb () {
        int ran;
        float prob = float.Parse (ReadJsonfiles.Instance.buffDics[(int.Parse (effectId))].onsetProb);
        if (prob == 100)
            canExecute = true;
        else {
            ran = Random.Range (1, 101);
            if (ran < prob) {
                canExecute = true;
                if (effectId == "200020203")
                    canSecondDamage = true;
            }

        }
    }

    public void JudgeTargetType () {
        // int targetType = ReadJsonfiles.Instance.buffDics[(int.Parse (ReadJsonfiles.Instance
        //     .skillDics[int.Parse (BattleLogic.instance.whichSkill.name)].effectid1))].target;
        target_roleIns = savetarget_roleIns;
        int targetType = ReadJsonfiles.Instance.buffDics[(int.Parse (effectId))].target;
        switch (targetType) {
            case (int) TargetSkillType.self:
                target_roleIns = attack_roleIns;
                JudgeEffectType ();
                // BuffShow();

                // BattleLogic.instance.ColculateBoolds (target_roleIns);
                break;
            case (int) TargetSkillType.singleFriend:
                JudgeEffectType ();
                // BuffShow();
                // BattleLogic.instance.ColculateBoolds (target_roleIns);
                break;
            case (int) TargetSkillType.allFriend:
                JudgeAllEffectType ();
                break;
            case (int) TargetSkillType.singleEnemy:
                JudgeEffectType ();
                // BuffShow();
                // BattleLogic.instance.ColculateBoolds (target_roleIns);
                break;
            case (int) TargetSkillType.allEnemy:
                JudgeAllEffectType (false);
                break;

        }
        Debug.LogError ("攻击对象是几：--" + targetType);
    }

    float ReturnValue1 (float targetAttribute) {
        return targetAttribute * (float) ReadJsonfiles.Instance.buffDics[(int.Parse (effectId))].value1 / 100;
    }

    float ReturnValue2 (float targetAttribute) {
        return targetAttribute * (float) ReadJsonfiles.Instance.buffDics[(int.Parse (effectId))].value2 / 100;
    }

    float ReturnFloatValue2 (int value = 200020301) {
        return (float) ReadJsonfiles.Instance.buffDics[200020301].value2 / 100;
    }

    float ReturnFloatValue1 (int value = 200020301) {
        return (float) ReadJsonfiles.Instance.buffDics[value].value1 / 100;
    }

    float ReturnOnSetProb () {
        return float.Parse (ReadJsonfiles.Instance.buffDics[100030201].onsetProb);
    }

    int ReturnValue2 () {
        return ReadJsonfiles.Instance.buffDics[(int.Parse (effectId))].value2;
    }

    void JudgeAllEffectType (bool isRole = true) {
        isAll = true;
        if (attack_roleIns.tag == "Hero" && isRole) {
            for (int i = 0; i < CreatSkeleton.Instance.roleSkeletonsBattleAni.Count; i++) {
                target_roleIns = CreatSkeleton.Instance.roleSkeletonsBattleAni[i].GetComponent<RoleConfig> ();
                JudgeEffectType ();
                // BuffShow();
                // BattleLogic.instance.ColculateBoolds (target_roleIns);
            }
        } else if (attack_roleIns.tag != "Hero" && !isRole) {
            for (int i = 0; i < CreatSkeleton.Instance.roleSkeletonsBattleAni.Count; i++) {
                target_roleIns = CreatSkeleton.Instance.roleSkeletonsBattleAni[i].GetComponent<RoleConfig> ();
                JudgeEffectType ();
                // BuffShow();
                // BattleLogic.instance.ColculateBoolds (target_roleIns);
            }
        } else {
            for (int i = 0; i < CreatSkeleton.Instance.monsterSkeletonsMonsterBattleAni.Count; i++) {
                target_roleIns = CreatSkeleton.Instance.monsterSkeletonsMonsterBattleAni[i].GetComponent<RoleConfig> ();
                JudgeEffectType ();
                // BuffShow();
                // BattleLogic.instance.ColculateBoolds (target_roleIns);
            }
        }
        isAll = false;
    }

    void JudgeAllFriendEffectType () {
        for (int i = 0; i < CreatSkeleton.Instance.roleSkeletonsBattleAni.Count; i++) {
            target_roleIns = CreatSkeleton.Instance.roleSkeletonsBattleAni[i].GetComponent<RoleConfig> ();
            JudgeEffectType ();
            BattleLogic.instance.ColculateBoolds (target_roleIns);
        }
    }

    void JudgeAllEnemyEffectType () {
        for (int i = 0; i < CreatSkeleton.Instance.monsterSkeletonsMonsterBattleAni.Count; i++) {
            target_roleIns = CreatSkeleton.Instance.monsterSkeletonsMonsterBattleAni[i].GetComponent<RoleConfig> ();
            JudgeEffectType ();
            BattleLogic.instance.ColculateBoolds (target_roleIns);
        }
    }

    //伤害 A%
    public void EffectType_One (RoleConfig roleconWhole) {

        skillDamage = BattleLogic.instance.isMaxDamage ? (float) int.Parse (roleconWhole.m_RoleInfo.critdamage.Split ('%') [0]) / 100 : 1;
        // skillCoefficient = (float) ReadJsonfiles.Instance.buffDics[(int.Parse (ReadJsonfiles.Instance
        //     .skillDics[int.Parse (BattleLogic.instance.whichSkill.name)].effectid1))].value1 / 100;
        skillCoefficient = (float) ReadJsonfiles.Instance.buffDics[(int.Parse (effectId))].value1 / 100;
    }
    //治疗，治疗量=A%自身生命上限
    public void EffectType_Two (RoleConfig roleconWhole) {
        BattleLogic.instance.isAdd = true;
        BattleLogic.instance.addBloodValue = ReturnValue1 (attack_roleIns.m_RoleInfo.life);
        target_life_old = 0;
        target_life_old = target_roleIns.m_RoleInfo.life;
        if (isAllAddBlood) {
            save_target_life_old.Add (target_life_old);
        }
        target_roleIns.m_RoleInfo.life += ReturnValue1 (attack_roleIns.m_RoleInfo.life);
        if (target_roleIns.m_RoleInfo.life >= ReadJsonfiles.Instance.roleDics[target_roleIns.id].life)
            target_roleIns.m_RoleInfo.life = ReadJsonfiles.Instance.roleDics[target_roleIns.id].life;
    }
    //给目标上BUFF 防御+A%
    public void EffectType_Three (RoleConfig roleconWhole) {
        // target_roleIns.m_RoleInfo.defense += ReturnValue1 (target_roleIns.m_RoleInfo.defense);
        isDefenceAdd = true;
        checkBools[6] = isDefenceAdd;

    }
    //给目标上BUFF 攻击+A%
    public void EffectType_Four (RoleConfig roleconWhole) {
        // target_roleIns.m_RoleInfo.attack += ReturnValue1 (target_roleIns.m_RoleInfo.attack);
        isAttackAdd = true;
        checkBools[4] = isAttackAdd;

    }
    //给目标上DEBUFF，速度-A%
    public void EffectType_Five (RoleConfig roleconWhole) {
        // target_roleIns.m_RoleInfo.speed -= ReturnValue1 (target_roleIns.m_RoleInfo.speed);
        isSpeedReduce = true;
        checkBools[8] = isSpeedReduce;

    }
    //给目标上DEBUFF，沉默
    public void EffectType_Six (RoleConfig roleconWhole) {
        isSilience = true;
        checkBools[3] = isSilience;

    }
    //给目标上DEBUFF 防御-A%
    public void EffectType_Seven (RoleConfig roleconWhole) {
        // target_roleIns.m_RoleInfo.defense -= ReturnValue1 (target_roleIns.m_RoleInfo.defense);
        isDefenceReduce = true;
        checkBools[7] = isDefenceReduce;

    }
    //给目标上DEBUFF 攻击-A%
    public void EffectType_Eight (RoleConfig roleconWhole) {
        // target_roleIns.m_RoleInfo.attack -= ReturnValue1 (target_roleIns.m_RoleInfo.attack);
        isAttackReduce = true;
        checkBools[5] = isAttackReduce;

    }
    //给目标上DEBUFF，【魔法钉】状态 9
    public void EffectType_Nine (RoleConfig roleconWhole) {
        Debug.LogError ("随机魔法钉概率：-- " + Random.Range (1, 101));
        // if (Random.Range (1, 101) < ReadJsonfiles.Instance.buffDics[(int.Parse (effectId))].value1) {
        isMagicNail = true;
        checkBools[1] = isMagicNail;
        // }

    }
    //给目标上DEBUFF，眩晕
    public void EffectType_Ten (RoleConfig roleconWhole) {

        isDizziness = true;
        checkBools[2] = isDizziness;
        if (attack_roleIns.iddif == 0) {
            RoleSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) MonsterSpineAniId.dizziness, target_roleIns.iddif,true));
        } else {
            MonsterSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) RoleSpineAniId.dizziness, target_roleIns.id,true));
        }

    }

    //眩晕状态
    public void CheckIsNeedChangeDizzinessState () {
        RoleConfig roleCon;
        for (int i = 0; i < ADDUIBattle.instance.battleCurrentAll.Count; i++) {
            roleCon = ADDUIBattle.instance.battleCurrentAll[i].GetComponent<RoleConfig> ();
            if (roleCon.m_RoleInfo.isDizziness && roleCon.iddif == 0) {
                MonsterSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) RoleSpineAniId.dizziness, roleCon.id, true));
            } else if (roleCon.m_RoleInfo.isDizziness) {
                RoleSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) MonsterSpineAniId.dizziness, roleCon.iddif, true));
            }
        }
    }

    //【被动技能】自身生命A%以下时，自身攻击力+B%
    public void EffectType_Eleven (RoleConfig roleconWhole) {
        if (float.Parse (attack_roleIns.m_RoleInfo.life.ToString ()) < ReadJsonfiles.Instance.buffDics[(int.Parse (effectId))].value1) {
            attack_roleIns.m_RoleInfo.attack += ReturnValue2 (target_roleIns.m_RoleInfo.attack);
        }
    }

    //【被动技能】自身生命A%以下时，自身攻击力+B%
    public float CheckIsThisSkillToAddAttack_EffectType_Eleven () {
        float value;
        if (attack_roleIns.id == 20002 &&
            float.Parse (attack_roleIns.m_RoleInfo.life.ToString ()) < ReadJsonfiles.Instance.buffDics[200020301].value1) {
            value = ReturnFloatValue2 ();
            return value;
        } else {
            return 0f;
        }
    }

    //【被动技能】受到攻击时发动反击，造成100%+A%伤害，自身损失A%生命 12
    public void EffectType_Twelve (RoleConfig roleconWhole) {

    }
    //嘲讽敌方全体角色，被嘲讽角色只能使用普通攻击 13
    public void EffectType_Thirteen (RoleConfig roleconWhole) {
        isForceAttack = true;
        checkBools[0] = isForceAttack;
        if (isForceAttack) {
            target_roleIns.m_RoleInfo.skillCd1 = 100;
            target_roleIns.m_RoleInfo.skillCd2 = 100;
            target_roleIns.m_RoleInfo.skillCd3 = 100;
        }

        //ceshi
        // target_roleIns.m_RoleInfo.isForceAttack = true;
        // target_roleIns.m_RoleInfo.isForceAttackCount = ReadJsonfiles.Instance.buffDics[(int.Parse (effectId))].continueBout;

    }
    //目标人数每增加1人，伤害+A% 14
    public void EffectType_Fourteen (RoleConfig roleconWhole) {
        skillCoefficient += (float) ReadJsonfiles.Instance.buffDics[(int.Parse (effectId))].value1 / 100;
    }
    //效果A发生暴击时伤害系数变更为B% 15
    public void EffectType_Fifteen (RoleConfig roleconWhole) {
        if (BattleLogic.instance.isMaxDamage) {
            skillCoefficient = (float) ReadJsonfiles.Instance.buffDics[(int.Parse (effectId))].value2 / 100;
        }
    }
    //效果A发生暴击时所有技能CD-B 16
    public void EffectType_Sixteen (RoleConfig roleconWhole) {
        if (BattleLogic.instance.isMaxDamage) {
            roleconWhole.m_RoleInfo.skillCd1 -= ReadJsonfiles.Instance.buffDics[(int.Parse (effectId))].value2;
            roleconWhole.m_RoleInfo.skillCd2 -= ReadJsonfiles.Instance.buffDics[(int.Parse (effectId))].value2;
            roleconWhole.m_RoleInfo.skillCd3 -= ReadJsonfiles.Instance.buffDics[(int.Parse (effectId))].value2;
            if (roleconWhole.m_RoleInfo.skillCd1 <= 0) roleconWhole.m_RoleInfo.skillCd1 = 0;
            if (roleconWhole.m_RoleInfo.skillCd2 <= 0) roleconWhole.m_RoleInfo.skillCd2 = 0;
            if (roleconWhole.m_RoleInfo.skillCd3 <= 0) roleconWhole.m_RoleInfo.skillCd3 = 0;
        }
    }
    //效果A发生暴击时再动1回合 17
    public void EffectType_Seventeen (RoleConfig roleconWhole) {
        if (BattleLogic.instance.isMaxDamage) {
            isAgainAttack = true;

        }
    }
    //效果A发生暴击时给目标上DEBUFF 防御-B% 18
    public void EffectType_Eightteen (RoleConfig roleconWhole) {
        if (BattleLogic.instance.isMaxDamage) {
            // target_roleIns.m_RoleInfo.defense -= ReadJsonfiles.Instance.buffDics[(int.Parse (effectId))].value2;
            isDefenceReduce = true;
            checkBools[7] = isDefenceReduce;

        }
    }
    //己方受到伤害A%转移到自身 19
    public void EffectType_Nineteen (RoleConfig roleconWhole) {
        isProjectTeammate = true;
        checkBools[9] = isProjectTeammate;
    }

    //己方受到伤害A%转移到自身 19
    public float ProjectTeammate_EffectType_Nineteen (float damage) {
        if (!CreatSkeleton.Instance.roleSkeletonsBattleAniDic.ContainsKey (10004)) return damage;
        else if (!isProjectTeammate) return damage;
        else if (attack_roleIns.iddif == 0) return damage;
        else {
            RoleConfig role10004 = CreatSkeleton.Instance.roleSkeletonsBattleAniDic[10004].GetComponent<RoleConfig> ();
            if (role10004.m_RoleInfo.isProjectTeammateCount == 0)
                isProjectTeammate = false;
            if (isProjectTeammate) {
                Debug.LogError ("原先得伤害值beifenbi：--- " + ReturnFloatValue1 (100040202));
                if (target_roleIns.id != 10004 && target_roleIns.iddif == 0) {
                    BattleLogic.instance.RoleBooldCalaulate (role10004, ReturnFloatValue1 (100040202) * damage);
                    return (1 - ReturnFloatValue1 (100040202)) * damage;
                } else {
                    return damage;
                }
            } else {
                return damage;
            }

        }
    }

    //移除目标身上的所有DEBUFFS 20
    public void EffectType_Twenty (RoleConfig roleconWhole) {
        removeAllDebuff = true;
        FindTargetChilds ();
        BattleLogic.instance.DestoryTargetDeBuff ();

    }
    //如果目标身上有【魔法钉】DEBUFF，效果A的伤害系数变更为B% 21
    public void EffectType_Twentyone (RoleConfig roleconWhole) {
        if (isMagicNail) {
            skillCoefficient = (float) ReadJsonfiles.Instance.buffDics[(int.Parse (effectId))].value2 / 100;
        }
    }
    //对目标造成伤害后，若目标生命值在A%以下，则造成目标眩晕DEBUFF 22
    public void EffectType_Twentytwo (RoleConfig roleconWhole) {
        if (target_roleIns.m_RoleInfo.life - BattleLogic.instance.damage * skillDamage * skillCoefficient < ReadJsonfiles.Instance.buffDics[(int.Parse (effectId))].value1) {
            isDizziness = true;
            checkBools[2] = isDizziness;
        }
    }
    //扣除目标A%生命值 23
    public void EffectType_Twentythree (RoleConfig roleconWhole) {
        target_roleIns.m_RoleInfo.life -= ReturnValue1 (target_roleIns.m_RoleInfo.life);
    }
    //如果技能效果A触发，则给目标上DEBUFF，攻击-B% 24
    public void EffectType_Twentyfour (RoleConfig roleconWhole) {
        if (canSecondDamage) {
            isAttackReduce = true;
            checkBools[5] = isAttackReduce;
            canSecondDamage = false;
        }
    }
    //【被动技能】任意友军受到攻击时发动反击，造成A%伤害（复数友军受击只反击1次）25
    public void EffectType_Twentyfive (RoleConfig roleconWhole) {

    }

    //【被动技能】任意友军受到攻击时发动反击，造成A%伤害（复数友军受击只反击1次）
    public RoleConfig CheckIsCanStrikeBack () {
        int ran;
        if (!CreatSkeleton.Instance.roleSkeletonsBattleAniDic.ContainsKey (10003)) return null;
        if (cheeckIsStrikeBack) {
            cheeckIsStrikeBack = false;
            ran = Random.Range (1, 101);
            ran = 10; //ceshi fanji
            if (ran < ReturnOnSetProb ()) {
                Debug.LogError ("进行反击");
                canStrikeBack = true;
                skillCoefficient = (float) ReadJsonfiles.Instance.buffDics[100030201].value1 / 100;
            } else {
                Debug.LogError ("没有进行反击");
            }
            return CreatSkeleton.Instance.roleSkeletonsBattleAniDic[10003].GetComponent<RoleConfig> ();
        } else {
            return null;
        }
    }

    public void BuffShow () {
        SelectBuffs ();
    }

    // bool[] checkBools = {isAgainAttack,isForceAttack,isMagicNail,removeAllDebuff,isDizziness,isSilience,isAttackAdd,isAttackReduce,isDefenceAdd,isDefenceReduce,isSpeedReduce};

    void SelectBuffs () {
        //checkBools[0] = isAgainAttack;
        //checkBools[1] = isForceAttack;
        //checkBools[2] = isMagicNail;
        //checkBools[3] = removeAllDebuff;
        //checkBools[4] = isDizziness;
        //checkBools[5] = isSilience;
        //checkBools[6] = isAttackAdd;
        //checkBools[7] = isAttackReduce;
        //checkBools[8] = isDefenceAdd;
        //checkBools[9] = isDefenceReduce;
        //checkBools[10] = isSpeedReduce;

        //TODO Buff记录
        // target_roleIns.ReverseUpdateInfo(m_RoleInfo: target_roleIns.m_RoleInfo);
        target_roleIns.UpdateInfo (checkBools, ReadJsonfiles.Instance.buffDics[(int.Parse (effectId))].continueBout);
        // FindAttackChilds();
        string str;
        for (int i = 0; i < checkBools.Length; i++) {
            if (checkBools[i]) {
                str = checkBoolsIn[i].ToString ().Substring (2, checkBoolsIn[i].Length - 2);
                checkBools[i] = false;
                ShowBuff (str);
            }
        }
    }

    void ShowBuff (string str) {
        Sprite objSprite;
        int id;
        GameObject buffParents;
        string oldStr = str;
        ColorDebug.Instance.DicDebug<string, Sprite> (ADDUIBattle.instance.ResourcesLoadBattleSceneInfos.roleBuffImagesDics, false);

        if (target_roleIns.iddif == 0)
            id = target_roleIns.id;
        else
            id = target_roleIns.iddif;
        if (CreatSkeleton.Instance.BloodsDic.ContainsKey (id)) {
            CreatSkeleton.Instance.BloodsObjDic.TryGetValue (id, out buffParents);
            buffParents = buffParents.transform.Find ("buffParents").gameObject;
            if (id == 10004 && str.Equals ("ProjectTeammate")) str = "MainProjectTeammate";
            else str = oldStr;
            CheckChildHaveOwn (buffParents, str);
            FindCurrentNewBuff (str);
            if (CheckIsChildHaveOwn) {
                GameObject obj = new GameObject (str);
                obj.transform.SetAsLastSibling ();
                Debug.LogError ("不存在同样的buff!!!");
                obj.AddComponent<RectTransform> ();
                obj.transform.parent = buffParents.transform;
                obj.GetComponent<RectTransform> ().sizeDelta = new Vector2 (30, 30);
                if (ADDUIBattle.instance.ResourcesLoadBattleSceneInfos.roleBuffImagesDics.ContainsKey (str)) {
                    objSprite = ADDUIBattle.instance.ResourcesLoadBattleSceneInfos.roleBuffImagesDics[str];
                    obj.AddComponent<Image> ().sprite = objSprite;
                    CreatContinueBoutText (obj.transform, ReadJsonfiles.Instance.buffDics[(int.Parse (effectId))].continueBout, str, id);
                }
                obj.GetComponent<RectTransform> ().position = target_roleIns.transform.position + new Vector3 (0, 20, 0);
                // //TOTO 找到刚添加的新buff
                // FindCurrentNewBuff(str);
                //TOTO 计算buff回合
                CountBuffTime ();
            } else {
                Debug.LogError ("已经存在了同样的buff!!!");
                CheckIsChildHaveOwn = true;
                //TODO 重新计算buff回合
                RecountBuffTime (str);
            }
        }
    }

    void CheckChildHaveOwn (GameObject tran, string str) {
        buffCkilds = tran.GetComponentsInChildren<Transform> ();
        // foreach (Transform temp in tran.GetComponentsInChildren<Transform> ()) {
        // }
        // if (trans.Length == 1) return;

        for (int i = 1; i < buffCkilds.Length; i++) {
            if (buffCkilds[i].name.Equals (str)) {
                CheckIsChildHaveOwn = false;
                break;
            }
        }
    }

    void FindCurrentNewBuff (string buffName) {
        if (target_roleIns.iddif == 0) {
            if (attack_roleIns.id == target_roleIns.id) {
                attack_currentnewbuffList.Add (buffName);
            }
        } else {
            if (attack_roleIns.iddif == target_roleIns.iddif) {
                attack_currentnewbuffList.Add (buffName);
            }
        }
    }

    void CreatContinueBoutText (Transform trans, int continueBout, string buffName = "", int id = 1) {

        RectTransform rectTrans;
        Text textTrans;
        GameObject obj = new GameObject ("continueBoutText");
        obj.transform.parent = trans;
        rectTrans = obj.AddComponent<RectTransform> ();
        rectTrans.sizeDelta = new Vector2 (30, 30);
        textTrans = obj.AddComponent<Text> ();
        textTrans.font = ResourcesLoadInfos.instance.fonts[0];
        textTrans.fontSize = 16;
        textTrans.fontStyle = FontStyle.Bold;
        if (buffName.Equals ("ProjectTeammate") && id != 10004)
            textTrans.text = "";
        else
            textTrans.text = continueBout.ToString ();

    }

    void CountBuffTime () {

    }

    void RecountBuffTime (string buffName) {
        Transform[] currentTarget = FindTargetChilds ();
        for (int i = 0; i < currentTarget.Length; i++) {
            if (buffName.Equals (currentTarget[i].name)) {
                currentTarget[i].GetChild (0).GetComponent<Text> ().text = ReadJsonfiles.Instance.buffDics[(int.Parse (effectId))].continueBout.ToString ();
            }
        }
    }

}