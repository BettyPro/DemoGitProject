using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class RoleConfig : MonoBehaviour
    {

        public List<int> roleId = new List<int>();
        public int id;
        public int iddif;
        public float distance;

        //[SerializeField]
        [System.Serializable]
        public class RoleInfo
        {

            //roles
            public int roleid; //角色ID
            public string nickname; //昵称
            public string attribute; //属性
            public string profession; //职业
            public string constellation; //星座
            public int starlevel; //星级
            public int numberawake; //觉醒次数
            public int degree; //亲密度
            public string degreedes; //亲密度描述
            public string rank; //等级
            public string intro; //简介,一句话描述
            public string fight; //战斗力
            public float attack; //攻击力
            public float defense; //防御
            public float life; //生命值
            public float speed; //速度
            public string critchance; //暴击概率
            public string critdamage; //暴击伤害
            public string effecthit; //效果命中
            public string effectresist; //效果抵抗
            public string attackrate; //连携攻击率
            public int skillid1; //技能1ID
            public int skillid2; //技能2ID
            public int skillid3; //技能3ID

            //skills
            public int skillCd1;
            public int skillCd2;
            public int skillCd3;

            public int skillCd1Count = 0;
            public int skillCd2Count = 0;
            public int skillCd3Count = 0;

            //buffs
            public bool isAgainAttack = false; //再动一回合
            public bool isForceAttack = false; //被嘲讽
            public bool isMagicNail = false; //魔法钉
            public bool removeAllDebuff = false; //移除所有DeBuff
            public bool isDizziness = false; //眩晕
            public bool isSilience = false; //沉默
            public bool isAttackAdd = false;
            public bool isAttackReduce = false;
            public bool isDefenceAdd = false;
            public bool isDefenceReduce = false;
            public bool isSpeedReduce = false;
            public bool isProjectTeammate = false;


            //buffs count
            public float isForceAttackCount = 0; //被嘲讽
            public float isMagicNailCount = 0; //魔法钉
            public float isDizzinessCount = 0; //眩晕
            public float isSilienceCount = 0; //沉默
            public float isAttackAddCount = 0;
            public float isAttackReduceCount = 0;
            public float isDefenceAddCount = 0;
            public float isDefenceReduceCount = 0;
            public float isSpeedReduceCount = 0;
            public float isProjectTeammateCount = 0;

            //damage
            public float skillDamage = 1;
            public float skillCoefficient = 1;

        }

        public RoleInfo m_RoleInfo = new RoleInfo();

        public void UpdateInfo(Roles role)
        {
            m_RoleInfo.roleid = role.roleid;
            m_RoleInfo.nickname = role.nickname;
            m_RoleInfo.attribute = role.attribute;
            m_RoleInfo.profession = role.profession;
            m_RoleInfo.constellation = role.constellation;
            m_RoleInfo.starlevel = role.starlevel;
            m_RoleInfo.numberawake = role.numberawake;
            m_RoleInfo.degree = role.degree;
            m_RoleInfo.degreedes = role.degreedes;
            m_RoleInfo.rank = role.rank;
            m_RoleInfo.intro = role.intro;
            m_RoleInfo.fight = role.fight;
            m_RoleInfo.attack = role.attack;
            m_RoleInfo.defense = role.defense;
            m_RoleInfo.life = role.life;
            m_RoleInfo.speed = role.speed;
            m_RoleInfo.critchance = role.critchance;
            m_RoleInfo.critdamage = role.critdamage;
            m_RoleInfo.effecthit = role.effecthit;
            m_RoleInfo.effectresist = role.effectresist;
            m_RoleInfo.attackrate = role.attackrate;
            m_RoleInfo.skillid1 = role.skillid1;
            m_RoleInfo.skillid2 = role.skillid2;
            m_RoleInfo.skillid3 = role.skillid3;
        }

        public void UpdateInfo(RoleInfo role)
        {
            m_RoleInfo.roleid = role.roleid;
            m_RoleInfo.nickname = role.nickname;
            m_RoleInfo.attribute = role.attribute;
            m_RoleInfo.profession = role.profession;
            m_RoleInfo.constellation = role.constellation;
            m_RoleInfo.starlevel = role.starlevel;
            m_RoleInfo.numberawake = role.numberawake;
            m_RoleInfo.degree = role.degree;
            m_RoleInfo.degreedes = role.degreedes;
            m_RoleInfo.rank = role.rank;
            m_RoleInfo.intro = role.intro;
            m_RoleInfo.fight = role.fight;
            m_RoleInfo.attack = role.attack;
            m_RoleInfo.defense = role.defense;
            m_RoleInfo.life = role.life;
            m_RoleInfo.speed = role.speed;
            m_RoleInfo.critchance = role.critchance;
            m_RoleInfo.critdamage = role.critdamage;
            m_RoleInfo.effecthit = role.effecthit;
            m_RoleInfo.effectresist = role.effectresist;
            m_RoleInfo.attackrate = role.attackrate;
            m_RoleInfo.skillid1 = role.skillid1;
            m_RoleInfo.skillid2 = role.skillid2;
            m_RoleInfo.skillid3 = role.skillid3;
        }

        public void UpdateInfo(bool[] checkBool)
        {

            if (checkBool[0]) m_RoleInfo.isForceAttack = checkBool[0];
            if (checkBool[1]) m_RoleInfo.isMagicNail = checkBool[1];
            if (checkBool[2]) m_RoleInfo.isDizziness = checkBool[2];
            if (checkBool[3]) m_RoleInfo.isSilience = checkBool[3];
            if (checkBool[4]) m_RoleInfo.isAttackAdd = checkBool[4];
            if (checkBool[5]) m_RoleInfo.isAttackReduce = checkBool[5];
            if (checkBool[6]) m_RoleInfo.isDefenceAdd = checkBool[6];
            if (checkBool[7]) m_RoleInfo.isDefenceReduce = checkBool[7];
            if (checkBool[8]) m_RoleInfo.isSpeedReduce = checkBool[8];
            if (checkBool[9]) m_RoleInfo.isProjectTeammate = checkBool[9];

        }

        public void UpdateInfo(bool[] checkBool, int continueBout)
        {

            if (checkBool[0])
            {
                m_RoleInfo.isForceAttack = checkBool[0];
                m_RoleInfo.isForceAttackCount = continueBout;
            }

            if (checkBool[1])
            {
                m_RoleInfo.isMagicNail = checkBool[1];
                m_RoleInfo.isMagicNailCount = continueBout;
            }

            if (checkBool[2])
            {
                m_RoleInfo.isDizziness = checkBool[2];
                m_RoleInfo.isDizzinessCount = continueBout;
            }

            if (checkBool[3])
            {
                m_RoleInfo.isSilience = checkBool[3];
                m_RoleInfo.isSilienceCount = continueBout;
            }

            if (checkBool[4])
            {
                m_RoleInfo.isAttackAdd = checkBool[4];
                m_RoleInfo.isAttackAddCount = continueBout;
                Debug.LogError("zhegeeffectid是：--" + SkillEffect.Instance.effectId);
            }

            if (checkBool[5])
            {
                m_RoleInfo.isAttackReduce = checkBool[5];
                m_RoleInfo.isAttackReduceCount = continueBout;
            }

            if (checkBool[6])
            {
                m_RoleInfo.isDefenceAdd = checkBool[6];
                m_RoleInfo.isDefenceAddCount = continueBout;
            }

            if (checkBool[7])
            {
                m_RoleInfo.isDefenceReduce = checkBool[7];
                m_RoleInfo.isDefenceReduceCount = continueBout;
            }

            if (checkBool[8])
            {
                m_RoleInfo.isSpeedReduce = checkBool[8];
                m_RoleInfo.isSpeedReduceCount = continueBout;
            }

            if (checkBool[9])
            {
                m_RoleInfo.isProjectTeammate = checkBool[9];
                m_RoleInfo.isProjectTeammateCount = continueBout;
            }
        }

        public void ReverseUpdateInfo(RoleInfo m_RoleInfo)
        {
            if (m_RoleInfo.isAttackAdd) SkillEffect.Instance.checkBools[0] = m_RoleInfo.isForceAttack;
            if (m_RoleInfo.isMagicNail) SkillEffect.Instance.checkBools[1] = m_RoleInfo.isMagicNail;
            if (m_RoleInfo.isDizziness) SkillEffect.Instance.checkBools[2] = m_RoleInfo.isDizziness;
            if (m_RoleInfo.isSilience) SkillEffect.Instance.checkBools[3] = m_RoleInfo.isSilience;
            if (m_RoleInfo.isAttackReduce) SkillEffect.Instance.checkBools[4] = m_RoleInfo.isAttackAdd;
            if (m_RoleInfo.isDefenceAdd) SkillEffect.Instance.checkBools[6] = m_RoleInfo.isDefenceAdd;
            if (m_RoleInfo.isDefenceReduce) SkillEffect.Instance.checkBools[7] = m_RoleInfo.isDefenceReduce;
            if (!!m_RoleInfo.isSpeedReduce) SkillEffect.Instance.checkBools[8] = m_RoleInfo.isSpeedReduce;

            // SkillEffect.Instance.checkBools[0] = m_RoleInfo.isForceAttack;
            // SkillEffect.Instance.checkBools[1] = m_RoleInfo.isMagicNail;
            // SkillEffect.Instance.checkBools[2] = m_RoleInfo.isDizziness;
            // SkillEffect.Instance.checkBools[3] = m_RoleInfo.isSilience;
            // SkillEffect.Instance.checkBools[4] = m_RoleInfo.isAttackAdd;
            // SkillEffect.Instance.checkBools[5] = m_RoleInfo.isAttackReduce;
            // SkillEffect.Instance.checkBools[6] = m_RoleInfo.isDefenceAdd;
            // SkillEffect.Instance.checkBools[7] = m_RoleInfo.isDefenceReduce;
            // SkillEffect.Instance.checkBools[8] = m_RoleInfo.isSpeedReduce;

        }

    }
}