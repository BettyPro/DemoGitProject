using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WinterColorDebug;
using WinterDebug;

namespace Demo
{
    public partial class SkillEffect
    {

        public Dictionary<string, string> attack_beginbuff = new Dictionary<string, string>();
        public List<Transform> attack_beginbuffList = new List<Transform>();
        public List<string> attack_currentnewbuffList = new List<string>();

        /// <summary>
        /// buff处理
        /// 回合计算
        /// </summary>
        public void CalaulateBuffCD()
        {

            FindAttackChilds();
            CheckIsCurrenBuff();
            // if (attack_roleIns.m_RoleInfo.isForceAttackCount != 0) attack_roleIns.m_RoleInfo.isForceAttackCount -= 1;
            // if (attack_roleIns.m_RoleInfo.isMagicNailCount != 0) attack_roleIns.m_RoleInfo.isMagicNailCount -= 1;
            // if (attack_roleIns.m_RoleInfo.isDizzinessCount != 0) attack_roleIns.m_RoleInfo.isDizzinessCount -= 1;
            // if (attack_roleIns.m_RoleInfo.isSilienceCount != 0) attack_roleIns.m_RoleInfo.isSilienceCount -= 1;
            // if (attack_roleIns.m_RoleInfo.isAttackAddCount != 0) attack_roleIns.m_RoleInfo.isAttackAddCount -= 1;
            // if (attack_roleIns.m_RoleInfo.isAttackReduceCount != 0) attack_roleIns.m_RoleInfo.isAttackReduceCount -= 1;
            // if (attack_roleIns.m_RoleInfo.isDefenceAddCount != 0) attack_roleIns.m_RoleInfo.isDefenceAddCount -= 1;
            // if (attack_roleIns.m_RoleInfo.isDefenceReduceCount != 0) attack_roleIns.m_RoleInfo.isDefenceReduceCount -= 1;
            // if (attack_roleIns.m_RoleInfo.isSpeedReduceCount != 0) attack_roleIns.m_RoleInfo.isSpeedReduceCount -= 1;
        }

        public void RefreshBuff()
        {
            if (attack_roleIns.m_RoleInfo.isForceAttackCount == 0 && attack_roleIns.m_RoleInfo.isForceAttack)
            {
                attack_roleIns.m_RoleInfo.isForceAttack = false;
                // CheckBuff("isForceAttack");
                BattleLogic.instance.DestoryAttackBuff("isForceAttack");
            }

            if (attack_roleIns.m_RoleInfo.isMagicNailCount == 0 && attack_roleIns.m_RoleInfo.isMagicNail)
            {
                attack_roleIns.m_RoleInfo.isMagicNail = false;
                // CheckBuff("isMagicNail");
                BattleLogic.instance.DestoryAttackBuff("isMagicNail");
            }

            if (attack_roleIns.m_RoleInfo.isDizzinessCount == 0 && attack_roleIns.m_RoleInfo.isDizziness)
            {
                attack_roleIns.m_RoleInfo.isDizziness = false;
                // CheckBuff("isDizziness");
                BattleLogic.instance.DestoryAttackBuff("isDizziness");
                int id = ReturnId();
                MonsterSpineAniManager.Instance.SendMsg(
                    ButtonMsg.GetInstance.ChangeInfo((ushort) MonsterSpineAniId.idle, id));
            }

            if (attack_roleIns.m_RoleInfo.isSilienceCount == 0 && attack_roleIns.m_RoleInfo.isSilience)
            {
                attack_roleIns.m_RoleInfo.isSilience = false;
                // CheckBuff("isSilience");
                BattleLogic.instance.DestoryAttackBuff("isSilience");
            }

            if (attack_roleIns.m_RoleInfo.isAttackAddCount == 0 && attack_roleIns.m_RoleInfo.isAttackAdd)
            {
                attack_roleIns.m_RoleInfo.isAttackAdd = false;
                // CheckBuff("isAttackAdd");
                BattleLogic.instance.DestoryAttackBuff("isAttackAdd");
            }

            if (attack_roleIns.m_RoleInfo.isAttackReduceCount == 0 && attack_roleIns.m_RoleInfo.isAttackReduce)
            {
                attack_roleIns.m_RoleInfo.isAttackReduce = false;
                BattleLogic.instance.DestoryAttackBuff("isAttackReduce");
            }

            if (attack_roleIns.m_RoleInfo.isDefenceAddCount == 0 && attack_roleIns.m_RoleInfo.isDefenceAdd)
            {
                attack_roleIns.m_RoleInfo.isDefenceAdd = false;
                BattleLogic.instance.DestoryAttackBuff("isDefenceAdd");
            }

            if (attack_roleIns.m_RoleInfo.isDefenceReduceCount == 0 && attack_roleIns.m_RoleInfo.isDefenceReduce)
            {
                attack_roleIns.m_RoleInfo.isDefenceReduce = false;
                BattleLogic.instance.DestoryAttackBuff("isDefenceReduce");
            }

            if (attack_roleIns.m_RoleInfo.isSpeedReduceCount == 0 && attack_roleIns.m_RoleInfo.isSpeedReduce)
            {
                attack_roleIns.m_RoleInfo.isSpeedReduce = false;
                BattleLogic.instance.DestoryAttackBuff("isSpeedReduce");
            }

            if (attack_roleIns.id == 10004)
            {
                if (attack_roleIns.m_RoleInfo.isProjectTeammateCount == 0 &&
                    attack_roleIns.m_RoleInfo.isProjectTeammate)
                {
                    attack_roleIns.m_RoleInfo.isProjectTeammate = false;
                    BattleLogic.instance.DestoryProjectBuff("isProjectTeammate");
                }
            }

        }

        public void RefreshBuff(RoleConfig attack_roleIns, Transform[] attack_buffCkilds)
        {
            if (attack_roleIns.m_RoleInfo.isForceAttackCount == 0 && attack_roleIns.m_RoleInfo.isForceAttack)
            {
                attack_roleIns.m_RoleInfo.isForceAttack = false;
                // CheckBuff("isForceAttack");
                BattleLogic.instance.DestoryAttackBuff("isForceAttack", attack_buffCkilds);
            }

            if (attack_roleIns.m_RoleInfo.isMagicNailCount == 0 && attack_roleIns.m_RoleInfo.isMagicNail)
            {
                attack_roleIns.m_RoleInfo.isMagicNail = false;
                // CheckBuff("isMagicNail");
                BattleLogic.instance.DestoryAttackBuff("isMagicNail", attack_buffCkilds);
            }

            if (attack_roleIns.m_RoleInfo.isDizzinessCount == 0 && attack_roleIns.m_RoleInfo.isDizziness)
            {
                attack_roleIns.m_RoleInfo.isDizziness = false;
                // CheckBuff("isDizziness");
                BattleLogic.instance.DestoryAttackBuff("isDizziness", attack_buffCkilds);
                int id = ReturnId();
                MonsterSpineAniManager.Instance.SendMsg(
                    ButtonMsg.GetInstance.ChangeInfo((ushort) MonsterSpineAniId.idle, id));
            }

            if (attack_roleIns.m_RoleInfo.isSilienceCount == 0 && attack_roleIns.m_RoleInfo.isSilience)
            {
                attack_roleIns.m_RoleInfo.isSilience = false;
                // CheckBuff("isSilience");
                BattleLogic.instance.DestoryAttackBuff("isSilience", attack_buffCkilds);
            }

            if (attack_roleIns.m_RoleInfo.isAttackAddCount == 0 && attack_roleIns.m_RoleInfo.isAttackAdd)
            {
                attack_roleIns.m_RoleInfo.isAttackAdd = false;
                // CheckBuff("isAttackAdd");
                BattleLogic.instance.DestoryAttackBuff("isAttackAdd", attack_buffCkilds);
            }

            if (attack_roleIns.m_RoleInfo.isAttackReduceCount == 0 && attack_roleIns.m_RoleInfo.isAttackReduce)
            {
                attack_roleIns.m_RoleInfo.isAttackReduce = false;
                BattleLogic.instance.DestoryAttackBuff("isAttackReduce", attack_buffCkilds);
            }

            if (attack_roleIns.m_RoleInfo.isDefenceAddCount == 0 && attack_roleIns.m_RoleInfo.isDefenceAdd)
            {
                attack_roleIns.m_RoleInfo.isDefenceAdd = false;
                BattleLogic.instance.DestoryAttackBuff("isDefenceAdd", attack_buffCkilds);
            }

            if (attack_roleIns.m_RoleInfo.isDefenceReduceCount == 0 && attack_roleIns.m_RoleInfo.isDefenceReduce)
            {
                attack_roleIns.m_RoleInfo.isDefenceReduce = false;
                BattleLogic.instance.DestoryAttackBuff("isDefenceReduce", attack_buffCkilds);
            }

            if (attack_roleIns.m_RoleInfo.isSpeedReduceCount == 0 && attack_roleIns.m_RoleInfo.isSpeedReduce)
            {
                attack_roleIns.m_RoleInfo.isSpeedReduce = false;
                BattleLogic.instance.DestoryAttackBuff("isSpeedReduce", attack_buffCkilds);
            }

            if (attack_roleIns.id == 10004)
            {
                if (attack_roleIns.m_RoleInfo.isProjectTeammateCount == 0 &&
                    attack_roleIns.m_RoleInfo.isProjectTeammate)
                {
                    attack_roleIns.m_RoleInfo.isProjectTeammate = false;
                    BattleLogic.instance.DestoryProjectBuff("isProjectTeammate");
                }
            }

        }

        int ReturnId()
        {
            if (attack_roleIns.iddif == 0)
                return attack_roleIns.id;
            else
                return attack_roleIns.iddif;
        }

        void FindAttackChilds()
        {
            int id;
            GameObject buffParents;
            ColorDebug.Instance.DicDebug<string, Sprite>(
                ADDUIBattle.instance.ResourcesLoadBattleSceneInfos.roleBuffImagesDics, false);

            if (attack_roleIns.iddif == 0)
                id = attack_roleIns.id;
            else
                id = attack_roleIns.iddif;
            if (CreatSkeleton.Instance.BloodsDic.ContainsKey(id))
            {
                CreatSkeleton.Instance.BloodsObjDic.TryGetValue(id, out buffParents);
                buffParents = buffParents.transform.Find("buffParents").gameObject;
                attack_buffCkilds = buffParents.GetComponentsInChildren<Transform>();
            }
        }

        public Transform[] FindAttackChilds(float a = 1)
        {
            int id;
            GameObject buffParents;
            ColorDebug.Instance.DicDebug<string, Sprite>(
                ADDUIBattle.instance.ResourcesLoadBattleSceneInfos.roleBuffImagesDics, false);

            if (attack_roleIns.iddif == 0)
                id = attack_roleIns.id;
            else
                id = attack_roleIns.iddif;
            if (CreatSkeleton.Instance.BloodsDic.ContainsKey(id))
            {
                CreatSkeleton.Instance.BloodsObjDic.TryGetValue(id, out buffParents);
                buffParents = buffParents.transform.Find("buffParents").gameObject;
                attack_buffCkilds = buffParents.GetComponentsInChildren<Transform>();
            }

            return attack_buffCkilds;
        }

        public Transform[] FindTargetChilds()
        {
            int id;
            GameObject buffParents;
            ColorDebug.Instance.DicDebug<string, Sprite>(
                ADDUIBattle.instance.ResourcesLoadBattleSceneInfos.roleBuffImagesDics, false);

            if (target_roleIns.iddif == 0)
                id = target_roleIns.id;
            else
                id = target_roleIns.iddif;
            if (CreatSkeleton.Instance.BloodsDic.ContainsKey(id))
            {
                CreatSkeleton.Instance.BloodsObjDic.TryGetValue(id, out buffParents);
                buffParents = buffParents.transform.Find("buffParents").gameObject;
                target_buffCkilds = buffParents.GetComponentsInChildren<Transform>();
            }

            return target_buffCkilds;
        }

        void CheckIsCurrenBuff()
        {
            //TODO buff重复 则重置CD
            //TODO buff不包含 则为新buff 不参与计算
            Text continueBoutText;
            if (attack_beginbuff == null && attack_currentnewbuffList == null) return;
            for (int j = 0; j < attack_currentnewbuffList.Count; j++)
            {
                if (attack_beginbuff.ContainsKey(attack_currentnewbuffList[j]))
                {
                    continueBoutText = attack_beginbuffList[j].GetChild(0).GetComponent<Text>();
                    CompareCalculate(attack_currentnewbuffList[j], -1, continueBoutText);
                }
            }

            for (int i = 0; i < attack_beginbuffList.Count; i++)
            {
                if (attack_beginbuffList[i] != null)
                {
                    continueBoutText = attack_beginbuffList[i].GetChild(0).GetComponent<Text>();
                    Debug.LogError(continueBoutText.text + "------=-=-===============-");
                    CompareCalculate(attack_beginbuffList[i].name, 1, continueBoutText);
                }
            }

            attack_beginbuff.Clear();
            attack_beginbuffList.Clear();
            attack_currentnewbuffList.Clear();
        }

        //计算buff CD
        public void CompareCalculate(string buffName, int value, Text continueBoutText = null)
        {
            switch (buffName)
            {
                case ("ForceAttack"):
                    if (attack_roleIns.m_RoleInfo.isForceAttackCount != 0)
                    {
                        attack_roleIns.m_RoleInfo.isForceAttackCount -= value;
                        if (continueBoutText != null)
                            continueBoutText.text = attack_roleIns.m_RoleInfo.isForceAttackCount.ToString();
                    }

                    break;
                case ("MagicNail"):
                    if (attack_roleIns.m_RoleInfo.isMagicNailCount != 0)
                    {
                        attack_roleIns.m_RoleInfo.isMagicNailCount -= value;
                        if (continueBoutText != null)
                            continueBoutText.text = attack_roleIns.m_RoleInfo.isMagicNailCount.ToString();
                    }

                    break;
                case ("Dizziness"):
                    if (attack_roleIns.m_RoleInfo.isDizzinessCount != 0)
                    {
                        attack_roleIns.m_RoleInfo.isDizzinessCount -= value;
                        if (continueBoutText != null)
                            continueBoutText.text = attack_roleIns.m_RoleInfo.isDizzinessCount.ToString();
                    }

                    break;
                case ("Silience"):
                    if (attack_roleIns.m_RoleInfo.isSilienceCount != 0)
                    {
                        attack_roleIns.m_RoleInfo.isSilienceCount -= value;
                        if (continueBoutText != null)
                            continueBoutText.text = attack_roleIns.m_RoleInfo.isSilienceCount.ToString();
                    }

                    break;
                case ("AttackAdd"):
                    if (attack_roleIns.m_RoleInfo.isAttackAddCount != 0)
                    {
                        attack_roleIns.m_RoleInfo.isAttackAddCount -= value;
                        if (continueBoutText != null)
                            continueBoutText.text = attack_roleIns.m_RoleInfo.isAttackAddCount.ToString();
                    }

                    break;
                case ("AttackReduce"):
                    if (attack_roleIns.m_RoleInfo.isAttackReduceCount != 0)
                    {
                        attack_roleIns.m_RoleInfo.isAttackReduceCount -= value;
                        if (continueBoutText != null)
                            continueBoutText.text = attack_roleIns.m_RoleInfo.isAttackReduceCount.ToString();
                    }

                    break;
                case ("DefenceAdd"):
                    if (attack_roleIns.m_RoleInfo.isDefenceAddCount != 0)
                    {
                        attack_roleIns.m_RoleInfo.isDefenceAddCount -= value;
                        if (continueBoutText != null)
                            continueBoutText.text = attack_roleIns.m_RoleInfo.isDefenceAddCount.ToString();
                    }

                    break;
                case ("DefenceReduce"):
                    if (attack_roleIns.m_RoleInfo.isDefenceReduceCount != 0)
                    {
                        attack_roleIns.m_RoleInfo.isDefenceReduceCount -= value;
                        if (continueBoutText != null)
                            continueBoutText.text = attack_roleIns.m_RoleInfo.isDefenceReduceCount.ToString();
                    }

                    break;
                case ("SpeedReduce"):
                    if (attack_roleIns.m_RoleInfo.isSpeedReduceCount != 0)
                    {
                        attack_roleIns.m_RoleInfo.isSpeedReduceCount -= value;
                        Debug.LogError(attack_roleIns.m_RoleInfo.isSpeedReduceCount +
                                       "------------------------------------------------");
                        if (continueBoutText != null)
                            continueBoutText.text = attack_roleIns.m_RoleInfo.isSpeedReduceCount.ToString();
                    }

                    break;
                // case ("ProjectTeammate"):
                //     if (attack_roleIns.m_RoleInfo.isProjectTeammateCount != 0) attack_roleIns.m_RoleInfo.isProjectTeammateCount -= value;
                //     break;
            }

            if (attack_roleIns.id == 10004)
            {
                switch (buffName)
                {
                    case ("ProjectTeammate"):
                        if (attack_roleIns.m_RoleInfo.isProjectTeammateCount != 0)
                        {
                            attack_roleIns.m_RoleInfo.isProjectTeammateCount -= value;
                            if (continueBoutText != null)
                                continueBoutText.text = attack_roleIns.m_RoleInfo.isProjectTeammateCount.ToString();
                        }

                        break;
                    case ("MainProjectTeammate"):
                        if (attack_roleIns.m_RoleInfo.isProjectTeammateCount != 0)
                        {
                            attack_roleIns.m_RoleInfo.isProjectTeammateCount -= value;
                            if (continueBoutText != null)
                                continueBoutText.text = attack_roleIns.m_RoleInfo.isProjectTeammateCount.ToString();
                        }

                        break;
                }
            }
        }

        public void CompareCalculate(string buffName, int value, RoleConfig attack_roleIns)
        {
            switch (buffName)
            {
                case ("ForceAttack"):
                    if (attack_roleIns.m_RoleInfo.isForceAttackCount != 0)
                        attack_roleIns.m_RoleInfo.isForceAttackCount -= value;
                    break;
                case ("MagicNail"):
                    if (attack_roleIns.m_RoleInfo.isMagicNailCount != 0)
                        attack_roleIns.m_RoleInfo.isMagicNailCount -= value;
                    break;
                case ("Dizziness"):
                    if (attack_roleIns.m_RoleInfo.isDizzinessCount != 0)
                        attack_roleIns.m_RoleInfo.isDizzinessCount -= value;
                    break;
                case ("Silience"):
                    if (attack_roleIns.m_RoleInfo.isSilienceCount != 0)
                        attack_roleIns.m_RoleInfo.isSilienceCount -= value;
                    break;
                case ("AttackAdd"):
                    if (attack_roleIns.m_RoleInfo.isAttackAddCount != 0)
                        attack_roleIns.m_RoleInfo.isAttackAddCount -= value;
                    break;
                case ("AttackReduce"):
                    if (attack_roleIns.m_RoleInfo.isAttackReduceCount != 0)
                        attack_roleIns.m_RoleInfo.isAttackReduceCount -= value;
                    break;
                case ("DefenceAdd"):
                    if (attack_roleIns.m_RoleInfo.isDefenceAddCount != 0)
                        attack_roleIns.m_RoleInfo.isDefenceAddCount -= value;
                    break;
                case ("DefenceReduce"):
                    if (attack_roleIns.m_RoleInfo.isDefenceReduceCount != 0)
                        attack_roleIns.m_RoleInfo.isDefenceReduceCount -= value;
                    break;
                case ("SpeedReduce"):
                    if (attack_roleIns.m_RoleInfo.isSpeedReduceCount != 0)
                        attack_roleIns.m_RoleInfo.isSpeedReduceCount -= value;
                    break;
                // case ("ProjectTeammate"):
                //     if (attack_roleIns.m_RoleInfo.isProjectTeammateCount != 0) attack_roleIns.m_RoleInfo.isProjectTeammateCount -= value;
                //     break;
            }

            if (attack_roleIns.id == 10004)
            {
                switch (buffName)
                {
                    case ("ProjectTeammate"):
                        if (attack_roleIns.m_RoleInfo.isProjectTeammateCount != 0)
                            attack_roleIns.m_RoleInfo.isProjectTeammateCount -= value;
                        break;
                    case ("MainProjectTeammate"):
                        if (attack_roleIns.m_RoleInfo.isProjectTeammateCount != 0)
                            attack_roleIns.m_RoleInfo.isProjectTeammateCount -= value;
                        break;
                }
            }
        }

        string ReturnStr(string str)
        {
            return str.Substring(2, str.Length - 2);
        }

        /// <summary>
        /// 恢复处理
        /// </summary>
        public void RecoverDatas()
        {

        }

        public void CheckBuffIsNeedToKill()
        {

        }
    }
}