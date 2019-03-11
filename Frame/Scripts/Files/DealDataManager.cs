using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class DealDataManager
    {
        private float hp;

        public float GetHp
        {
            get { return hp; }
            set { hp = value; }
        }


        public void ChangeValue()
        {
            Debug.Log(11);
        }

        /// <summary>
        /// 处理角色信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public string DetailedInfo(int roleId = 10001)
        {
            string space = "          ";
            //string space1 = "      ---     ";
            //string space2 = "       ";
            //string space3 = "      ";
            string[] infos = {"攻击力", "防御力", "生命力", "速度", "暴击率", "暴击伤害", "效果命中", "效果抵抗", "连携攻击率"};
            string str = "";
            Roles role = Singleton<ReadJsonfiles>.Instance.roleDics[roleId];
            for (int i = 0; i < infos.Length; i++)
            {
                infos[i] = infos[i].PadRight(6, '_');
                str += infos[i] + space + Singleton<ReadJsonfiles>.Instance.roleShowInfosDic[roleId][i] + "\r\n";
            }

            return str;
        }

        public string RankInfo(int roleId = 10001)
        {
            ReadJsonfiles role = Singleton<ReadJsonfiles>.Instance;
            string str = "";
            str = "<color=red><size=28>等级：--  " + role.roleDics[roleId].rank + "</size></color>";
            return str;
        }

        public string FightInfo(int roleId = 10001)
        {
            ReadJsonfiles role = Singleton<ReadJsonfiles>.Instance;
            string str = "";
            str = "战斗力：--  " + role.roleDics[roleId].fight;
            return str;
        }

        public string Info(int roleId = 10001)
        {
            ReadJsonfiles role = Singleton<ReadJsonfiles>.Instance;
            string str = "";
            str = role.roleDics[roleId].attribute + "   " + role.roleDics[roleId].profession + "   " +
                  role.roleDics[roleId].constellation;
            return str;
        }

        public string NickName(int roleId = 10001)
        {
            ReadJsonfiles role = Singleton<ReadJsonfiles>.Instance;
            string str = "";
            str = "<color=red>" + role.roleDics[roleId].nickname + "</color>";
            return str;
        }

        public string Degree(int roleId = 10001)
        {
            ReadJsonfiles role = Singleton<ReadJsonfiles>.Instance;
            string str = "";
            str = "<color=blue>" + role.roleDics[roleId].degree + "</color>";
            return str;
        }

        public string Intre(int roleId = 10001)
        {
            ReadJsonfiles role = Singleton<ReadJsonfiles>.Instance;
            string str = "";
            str = "<color=green>" + role.roleDics[roleId].intro + "</color>";
            return str;
        }

        public string Rank(int roleId = 10001, bool isRole = true)
        {
            ReadJsonfiles role = Singleton<ReadJsonfiles>.Instance;
            string str = "";
            if (isRole)
                str = "<color=green>" + role.roleDics[roleId].rank.Substring(0, 2) + "</color>";
            else
                str = "";

            return str;
        }

        public string Blood(int roleId = 10001)
        {
            ReadJsonfiles role = Singleton<ReadJsonfiles>.Instance;
            string str = "";
            hp = float.Parse(role.roleDics[roleId].life.ToString());
            str = "<color=green>" + hp + "/" + role.roleDics[roleId].life + "</color>";

            return str;
        }

        public float ChangeBloodTime(RoleConfig ro)
        {
            hp = float.Parse(ro.m_RoleInfo.life.ToString());
            return hp;
        }

        public string BloodShow(int roleId = 10001)
        {
            ReadJsonfiles role = Singleton<ReadJsonfiles>.Instance;
            string str = "";
            str = "<color=green>" + hp + "/" + role.roleDics[roleId].life + "</color>";
            Debug.Log(str);
            return str;
        }

        public string BloodShow(float hp, int roleId = 10001)
        {
            ReadJsonfiles role = Singleton<ReadJsonfiles>.Instance;
            string str = "";
            str = "<color=green>" + hp.ToString("f0") + "/" + role.roleDics[roleId].life + "</color>";
            //Debug.Log(str);
            return str;
        }

        public float BloodShowPrecent(float hp, int roleId = 10001)
        {
            ReadJsonfiles role = Singleton<ReadJsonfiles>.Instance;
            float str = 0;
            str = hp / int.Parse(role.roleDics[roleId].life.ToString());
            //Debug.Log(str);
            return str;
        }



        /// <summary>
        /// 处理技能信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public string SkillNameInfo(int skillId = 1000101)
        {
            ReadJsonfiles skill = Singleton<ReadJsonfiles>.Instance;
            string str = "技能名： ";
            str += skill.skillDics[skillId].skillname;
            return str;

        }

        public string HunNumInfo(int skillId = 1000101)
        {
            ReadJsonfiles skill = Singleton<ReadJsonfiles>.Instance;
            string str = "加魂数量： ";
            str += skill.skillDics[skillId].skilladdnum.ToString();
            return str;

        }

        public string SkillDesInfo(int skillId = 1000101)
        {
            ReadJsonfiles skill = Singleton<ReadJsonfiles>.Instance;
            string str = "技能描述： ";
            str += skill.skillDics[skillId].skilldes;
            return str;

        }

        public string StrengthenSkillInfo(int skillId = 1000101)
        {
            ReadJsonfiles skill = Singleton<ReadJsonfiles>.Instance;
            string str = "<color=red>强化技能：</color> \n";
            str += skill.skillDics[skillId].strengthen1 + "\n" + skill.skillDics[skillId].strengthen2 + "\n"
                   + skill.skillDics[skillId].strengthen3 + "\n"
                   + skill.skillDics[skillId].strengthen4 + "\n";
            return str;

        }

        /// <summary>
        /// 处理Battle界面速度条
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
    }
}