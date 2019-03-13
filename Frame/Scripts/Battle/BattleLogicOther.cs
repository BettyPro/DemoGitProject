using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;
using WinterTools;

namespace Demo
{


    public partial class BattleLogic : MonoBehaviour
    {

        ResourcesLoadBattleSceneInfos resourcesLoadBattleSceneInfosInstance = new ResourcesLoadBattleSceneInfos();
        RectTransform currenBlood = null;
        Vector2 cubeV2Pos = Vector2.zero;
        private int recordWhickCount = 0; //记录群体加血第几个

        //集合
        List<float> monsterSelectLife = new List<float>();
        Dictionary<int, float> monsterSelectLifeDic = new Dictionary<int, float>();
        List<SkeletonAnimation> saveMonsterSkeletonsMonsterBattleAni = new List<SkeletonAnimation>(); //保存数据用
        List<SkeletonAnimation> saveRoleSkeletonsBattleAni = new List<SkeletonAnimation>(); //保存数据用

        //bool
        bool beginFllow = false;
        bool isAllDemage = false;
        public bool isAdd = false;
        private bool saveData = true;

        //用于SkillEffect交互
        public float addBloodValue;

        //Tween
        Tweener tween;

        //用于计算公式
        float attackAdd = 0f;
        float attackReduce = 0f;
        float defenseAdd = 0f;
        float defenseReduce = 0f;
        float speedReduce = 0f;
        float passiveValue = 0f;

        //用于血量数字图片表示
        int ge, shi, bai, qian, wan, shiwan = 0;
        int[] values;

        //反击特效
        SkeletonAnimation strikeBackAni;

        //测试区
        float testValue = 10f;

        //血条跟随
        void BloodsFllow()
        {
//            beginFllow = true;//ceshi
            
            if (beginFllow)
            {
                if (SkillEffect.Instance.attack_roleIns.iddif == 0)
                {
                    currenBlood = CreatSkeleton.Instance.BloodsObjDic[SkillEffect.Instance.attack_roleIns.id]
                        .GetComponent<RectTransform>();
                    cubeV2Pos = RectTransformUtility.WorldToScreenPoint(Camera.main,
                        SkillEffect.Instance.attack_roleIns.transform.position);
                    currenBlood.position = cubeV2Pos + new Vector2(50, 200);
//                    currenBlood.anchoredPosition3D = cubeV2Pos;
                }
                else
                {
                    currenBlood = CreatSkeleton.Instance.BloodsObjDic[SkillEffect.Instance.attack_roleIns.iddif]
                        .GetComponent<RectTransform>();
                    cubeV2Pos = RectTransformUtility.WorldToScreenPoint(Camera.main,
                        SkillEffect.Instance.attack_roleIns.transform.position);
                    currenBlood.position = cubeV2Pos + new Vector2(-40, 240);
//                    currenBlood.anchoredPosition3D = cubeV2Pos;
                }
            }
        }

        void EffectFollow()
        {
        }

        //群体伤害 role and npc
        void AllDamage()
        {
            //TODO list赋值，存储当前的数量,防止群体伤害中死亡 造成的问题
            if (saveData)
            {
                // saveData = false;
                for (int i = 0; i < CreatSkeleton.Instance.monsterSkeletonsMonsterBattleAni.Count; i++)
                {
                    saveMonsterSkeletonsMonsterBattleAni.Add(CreatSkeleton.Instance
                        .monsterSkeletonsMonsterBattleAni[i]);
                }

                for (int i = 0; i < CreatSkeleton.Instance.roleSkeletonsBattleAni.Count; i++)
                {
                    saveRoleSkeletonsBattleAni.Add(CreatSkeleton.Instance.roleSkeletonsBattleAni[i]);
                }
            }

            if (SkillEffect.Instance.attack_roleIns.tag == "Hero" && isRole)
            {
                for (int i = 0; i < saveMonsterSkeletonsMonsterBattleAni.Count; i++)
                {
                    SkillEffect.Instance.target_roleIns =
                        saveMonsterSkeletonsMonsterBattleAni[i].GetComponent<RoleConfig>();
                    SingleDamage(SkillEffect.Instance.target_roleIns.m_RoleInfo.defense);
                    ColculateBoolds(SkillEffect.Instance.target_roleIns);
                }

                saveMonsterSkeletonsMonsterBattleAni.Clear();
            }
            else if (SkillEffect.Instance.attack_roleIns.tag != "Hero" && !isRole)
            {
                for (int i = 0; i < saveRoleSkeletonsBattleAni.Count; i++)
                {
                    SkillEffect.Instance.target_roleIns = saveRoleSkeletonsBattleAni[i].GetComponent<RoleConfig>();
                    SingleDamage(SkillEffect.Instance.target_roleIns.m_RoleInfo.defense);
                    ColculateBoolds(SkillEffect.Instance.target_roleIns);
                }

                saveRoleSkeletonsBattleAni.Clear();
            }

            // if (SkillEffect.Instance.attack_roleIns.tag == "Hero" && isRole) {
            //     for (int i = 0; i < CreatSkeleton.Instance.monsterSkeletonsMonsterBattleAni.Count; i++) {
            //         SkillEffect.Instance.target_roleIns = CreatSkeleton.Instance.monsterSkeletonsMonsterBattleAni[i].GetComponent<RoleConfig> ();
            //         SingleDamage (SkillEffect.Instance.target_roleIns.m_RoleInfo.defense);
            //         ColculateBoolds (SkillEffect.Instance.target_roleIns);
            //     }
            // } else if (SkillEffect.Instance.attack_roleIns.tag != "Hero" && !isRole) {
            //     for (int i = 0; i < CreatSkeleton.Instance.roleSkeletonsBattleAni.Count; i++) {
            //         SkillEffect.Instance.target_roleIns = CreatSkeleton.Instance.roleSkeletonsBattleAni[i].GetComponent<RoleConfig> ();
            //         SingleDamage (SkillEffect.Instance.target_roleIns.m_RoleInfo.defense);
            //         ColculateBoolds (SkillEffect.Instance.target_roleIns);
            //     }
            // }
        }

        void SingleDamage(float target_defense)
        {
            //TODO 通过buff进行伤害计算

            #region new计算方式

            #endregion

            #region old计算方式

#if true
            // if (float.Parse(roleconWhole.m_RoleInfo.attack.ToString()) > target_defense)
            //     damage = float.Parse(roleconWhole.m_RoleInfo.attack.ToString()) - target_defense;
            if (float.Parse(SkillEffect.Instance.attack_roleIns.m_RoleInfo.attack.ToString()) > target_defense)
                damage = float.Parse(SkillEffect.Instance.attack_roleIns.m_RoleInfo.attack.ToString()) - target_defense;
            else
                damage = Random.Range(30, 61);
            Debug.Log("role damage：--" + damage);
            if (random <= int.Parse(roleconWhole.m_RoleInfo.critchance.Split('%')[0]))
            {
                Debug.LogError("随机值是：-- " + random);
                Debug.LogError("概率值是：-- " + int.Parse(roleconWhole.m_RoleInfo.critchance.Split('%')[0]));

                damage *= SkillEffect.Instance.skillDamage * SkillEffect.Instance.skillCoefficient;
                Debug.Log(int.Parse(roleconWhole.m_RoleInfo.critdamage.Split('%')[0]));
            }
            else
            {
                damage *= SkillEffect.Instance.skillDamage * SkillEffect.Instance.skillCoefficient;
                Debug.LogError("没有出现暴击");
            }

            //用于测试
            if (SkillEffect.Instance.attack_roleIns.iddif == 0)
                damage *= SetConfig.roleDamageCoefficient;
            else
                damage *= SetConfig.monsterDamageCoefficient;
            Debug.LogError(damage);
            damage = SkillEffect.Instance.ProjectTeammate_EffectType_Nineteen(damage);
#endif

            #endregion
        }

        //群体加血  role and npc
        void AllAddBlood()
        {
            if (SkillEffect.Instance.attack_roleIns.tag == "Hero")
            {
                for (int i = 0; i < CreatSkeleton.Instance.roleSkeletonsBattleAni.Count; i++)
                {
                    SkillEffect.Instance.target_roleIns =
                        CreatSkeleton.Instance.roleSkeletonsBattleAni[i].GetComponent<RoleConfig>();
                    Debug.Log(SkillEffect.Instance.target_roleIns.m_RoleInfo.life + "------这个时");
                    // StartCoroutine(RoleNotChangePos(SkillEffect.Instance.target_roleIns,false));
                    recordWhickCount = i;
                    SingleRoleAddBloodToEnd(SkillEffect.Instance.target_roleIns, false);
                }
            }
            else if (SkillEffect.Instance.attack_roleIns.tag != "Hero")
            {

                for (int i = 0; i < CreatSkeleton.Instance.monsterSkeletonsMonsterBattleAni.Count; i++)
                {
                    SkillEffect.Instance.target_roleIns = CreatSkeleton.Instance.monsterSkeletonsMonsterBattleAni[i]
                        .GetComponent<RoleConfig>();
                    // StartCoroutine(MonsterNotChangePos(SkillEffect.Instance.target_roleIns,false));
                    recordWhickCount = i;
                    SingleMonsterAddBloodToEnd(SkillEffect.Instance.target_roleIns, false);
                }
            }

            isAdd = false;
            AgainAttack();
        }

        void SingleRoleAddBloodToEnd(RoleConfig target_roleIns, bool isSingle = true)
        {
            RoleBloodCalculate(target_roleIns, !isSingle);
            RoleSpineAniManager.Instance.SendMsg(ButtonMsg.GetInstance.ChangeInfo((ushort) RoleSpineAniId.idle,
                roleconWhole.id, "normal"));
            RoleMove.instance.mainCamera.GetComponent<ConstrainCamera>().target =
                ADDUIBattle.instance.PlayerContrBattle.transform;
            if (isSingle)
                AgainAttack();
        }

        void SingleMonsterAddBloodToEnd(RoleConfig target_roleIns, bool isSingle = true)
        {
            MonsterBloodCalculate(target_roleIns, !isSingle);
            MonsterSpineAniManager.Instance.SendMsg(ButtonMsg.GetInstance.ChangeInfo((ushort) MonsterSpineAniId.idle,
                roleconWhole.iddif));
            RoleMove.instance.mainCamera.GetComponent<ConstrainCamera>().target =
                ADDUIBattle.instance.PlayerContrBattle.transform;
            if (isSingle)
                AgainAttack();
        }

        RoleConfig MonsterSelectTarget(RoleConfig target_roleIns)
        {
            float temp = 0;
            int key = 0;
            for (int i = 0; i < CreatSkeleton.Instance.monsterSkeletonsMonsterBattleAni.Count; i++)
            {
                monsterSelectLife.Add(CreatSkeleton.Instance.monsterSkeletonsMonsterBattleAni[i]
                    .GetComponent<RoleConfig>().m_RoleInfo.life);
                monsterSelectLifeDic.Add(i,
                    CreatSkeleton.Instance.monsterSkeletonsMonsterBattleAni[i].GetComponent<RoleConfig>().m_RoleInfo
                        .life);
            }

            monsterSelectLife.Sort();
            // var keys = monsterSelectLifeDic.Where(q => q.Value == temp).Select(q => q.Key);  //get all keys
            // List<int> keyList = (from q in monsterSelectLifeDic 
            //                         where q.Value == temp
            //                             select q.Key).ToList<int>();
            // var firstKey = monsterSelectLifeDic.FirstOrDefault(q => q.Value == temp).Key;
            foreach (var item in monsterSelectLifeDic)
            {
                if (item.Value == monsterSelectLife[0])
                    key = item.Key;
            }

            target_roleIns = CreatSkeleton.Instance.monsterSkeletonsMonsterBattleAni[key].GetComponent<RoleConfig>();
            Debug.LogError("目标是：-- " + target_roleIns);
            monsterSelectLife.Clear();
            monsterSelectLifeDic.Clear();
            return target_roleIns;
        }

        void BloodChangedShow(RoleConfig target_roleIns, float damage, bool isAll = false)
        {
            GameObject blood;
            // Transform changedBlood;
            RectTransform changedBlood;
            Text changedBloodText;
            RectTransform managerValue;
            Image one;
            float addRealBloodValue;
            int id = 0;
            if (target_roleIns.tag == "Hero")
                id = target_roleIns.id;
            else
            {
                id = target_roleIns.iddif;
            }

            if (CreatSkeleton.Instance.BloodsDic.ContainsKey(id))
            {
                CreatSkeleton.Instance.BloodsObjDic.TryGetValue(id, out blood);
                changedBlood = blood.transform.Find("changedBlood").GetComponent<RectTransform>();
                changedBloodText = blood.transform.Find("changedBlood/changedBloodText").GetComponent<Text>();
                managerValue = blood.transform.Find("changedBlood/managerValue").GetComponent<RectTransform>();
                managerValue.gameObject.SetActive(true);
                managerValue.GetChild(0);
                one = blood.transform.Find("changedBlood/managerValue/one").GetComponent<Image>();
                if (isAdd)
                {
                    if (isAll)
                    {
                        if (SkillEffect.Instance.save_target_life_old.Count != 0)
                        {
                            SkillEffect.Instance.target_life_old =
                                SkillEffect.Instance.save_target_life_old[recordWhickCount];

                            if (recordWhickCount + 1 == SkillEffect.Instance.save_target_life_old.Count)
                                SkillEffect.Instance.save_target_life_old.Clear();

                            if (SkillEffect.Instance.target_life_old ==
                                ReadJsonfiles.Instance.roleDics[target_roleIns.id].life) return;
                            if (SkillEffect.Instance.target_life_old + addBloodValue <=
                                ReadJsonfiles.Instance.roleDics[target_roleIns.id].life)
                            {
                                // changedBloodText.text = "<color=green>+" + addBloodValue.ToString ("f0") + "</color>";
                                ValueDealToImage(managerValue, addBloodValue, isAdd);

                            }
                            else
                            {
                                addRealBloodValue = ReadJsonfiles.Instance.roleDics[target_roleIns.id].life -
                                                    SkillEffect.Instance.target_life_old;
                                // changedBloodText.text = "<color=green>+" + addRealBloodValue.ToString ("f0") + "</color>";
                                ValueDealToImage(managerValue, addRealBloodValue, isAdd);

                            }
                        }
                    }
                    else
                    {
                        if (SkillEffect.Instance.target_life_old ==
                            ReadJsonfiles.Instance.roleDics[target_roleIns.id].life) return;
                        if (SkillEffect.Instance.target_life_old + addBloodValue <=
                            ReadJsonfiles.Instance.roleDics[target_roleIns.id].life)
                        {
                            // changedBloodText.text = "<color=green>+" + addBloodValue.ToString ("f0") + "</color>";
                            ValueDealToImage(managerValue, addBloodValue);

                        }
                        else
                        {
                            addRealBloodValue = ReadJsonfiles.Instance.roleDics[target_roleIns.id].life -
                                                SkillEffect.Instance.target_life_old;
                            // changedBloodText.text = "<color=green>+" + addRealBloodValue.ToString ("f0") + "</color>";
                            ValueDealToImage(managerValue, addRealBloodValue);

                        }
                    }
                }
                else
                {
                    if (isMaxDamage)
                    {
                        // changedBloodText.text = "<color=blue>-" + damage.ToString ("f0") + "</color>";
                        ValueDealToImage(managerValue, damage);
                    }
                    else
                    {
                        // changedBloodText.text = "<color=red>-" + damage.ToString ("f0") + "</color>";
                        ValueDealToImage(managerValue, damage);

                    }
                }

                Debug.Log(changedBlood.position);
                Debug.Log(changedBlood.localPosition);
                Debug.Log(changedBlood.anchoredPosition);
                changedBlood.anchoredPosition = new Vector2(-10, 0);
                //伤害数字提示
                // Vector2 cubeV2Pos = RectTransformUtility.WorldToScreenPoint(Camera.main, blood.transform.position);
                Vector2 cubeV2Pos = Vector2.zero;
                if (SkillEffect.Instance.canStrikeBack)
                {
                    cubeV2Pos = RectTransformUtility.WorldToScreenPoint(Camera.main, lastAttacker.transform.position);
                }
                else
                {
                    cubeV2Pos = RectTransformUtility.WorldToScreenPoint(Camera.main,
                        SkillEffect.Instance.target_roleIns.transform.position);
                }

                Debug.Log(cubeV2Pos);
                // changedBlood.position = cubeV2Pos + new Vector2 (0, 35);
                changedBlood.position = cubeV2Pos + new Vector2(-20, 35);
                Debug.Log(changedBlood.position);

                // tween = changedBlood.DOLocalMove (changedBlood.anchoredPosition3D + new Vector3 (0f, 35f, 0), 0.2f);
                tween = changedBlood.DOLocalMove(changedBlood.anchoredPosition3D + new Vector3(0f, 205f, 0), 0.2f);
                tween.SetAutoKill(false);
                //tween.Pause();
                StartCoroutine(ADDUIBattle.instance.DelayBackTweenTwo(changedBlood, changedBloodText, managerValue));

            }
        }

        void ValueDealToImage(RectTransform managerValue, float value, bool isAdd = false)
        {
            ge = (int) value % 10;
            shi = (int) value / 10 % 10;
            bai = (int) value / 100 % 10;
            qian = (int) value / 1000 % 10;
            wan = (int) value / 10000 % 10;
            shiwan = (int) value / 100000 % 10;
            values = new int[6] {shiwan, wan, qian, bai, shi, ge};

            if (value == 0)
            {
                return;
            }

            if (value < 10)
            {
                GetChildCount(managerValue, 1);
            }
            else if (value <= 100)
            {
                GetChildCount(managerValue, 2);
            }
            else if (value <= 1000)
            {
                GetChildCount(managerValue, 3);
            }
            else if (value <= 10000)
            {
                GetChildCount(managerValue, 4);
            }
            else if (value <= 100000)
            {
                GetChildCount(managerValue, 5);
            }
            else
            {
                GetChildCount(managerValue, 6);
            }
        }

        void GetChildCount(RectTransform managerValue, int length)
        {
            if (isAdd)
            {
                managerValue.GetChild(0).GetComponent<Image>().sprite =
                    ADDUIBattle.instance.ResourcesLoadBattleSceneInfos.damageValuesShow[10] as Sprite;
                ShowValueImage(managerValue, length,
                    ADDUIBattle.instance.ResourcesLoadBattleSceneInfos.bloodValuesShows);
            }
            else if (isMaxDamage)
            {
                managerValue.GetChild(0).GetComponent<Image>().sprite =
                    ADDUIBattle.instance.ResourcesLoadBattleSceneInfos.damageValuesShow[11] as Sprite;
                ShowValueImage(managerValue, length, ADDUIBattle.instance.ResourcesLoadBattleSceneInfos.critValuesShow);
            }
            else
            {
                managerValue.GetChild(0).GetComponent<Image>().sprite =
                    ADDUIBattle.instance.ResourcesLoadBattleSceneInfos.damageValuesShow[11] as Sprite;
                ShowValueImage(managerValue, length,
                    ADDUIBattle.instance.ResourcesLoadBattleSceneInfos.damageValuesShow);

            }
        }

        void ShowValueImage(RectTransform managerValue, int length, Object[] objs)
        {
            int k = 0;
            bool breakLoop = true;
            managerValue.GetChild(0).gameObject.SetActive(true);
            for (int i = 0; i < length; i++)
            {
                managerValue.GetChild(i + 1).gameObject.SetActive(true);
                if (breakLoop)
                {
                    for (int j = 0; j < values.Length; j++)
                    {
                        if (values[j] != 0)
                        {
                            k = j;
                            breakLoop = false;
                            break;
                        }
                    }
                }

                managerValue.GetChild(i + 1).GetComponent<Image>().sprite = objs[values[k + i]] as Sprite;
            }
        }

        public void HideValueImage(RectTransform managerValue)
        {
            for (int i = 0; i < 6; i++)
            {
                managerValue.GetChild(i).GetComponent<Image>().sprite = null;
                managerValue.GetChild(i).gameObject.SetActive(false);
            }

            managerValue.gameObject.SetActive(false);
        }

        public void DestoryAttackBuff(string buff)
        {
            string str = buff.Substring(2, buff.Length - 2);

            if (SkillEffect.Instance.attack_buffCkilds == null) return;
            if (SkillEffect.Instance.attack_buffCkilds.Length == 1) return;
            for (int i = 1; i < SkillEffect.Instance.attack_buffCkilds.Length; i++)
            {
                if (str.Equals(SkillEffect.Instance.attack_buffCkilds[i].name))
                {
                    Debug.LogError("去除buff---------------------------------------");
                    SkillEffect.Instance.attack_buffCkilds[i].gameObject.SetActive(false);
                    Destroy(SkillEffect.Instance.attack_buffCkilds[i].gameObject);
                }
            }
        }

        public void DestoryAttackBuff(string buff, Transform[] attack_buffCkilds)
        {
            string str = buff.Substring(2, buff.Length - 2);

            if (attack_buffCkilds == null) return;
            if (attack_buffCkilds.Length == 1) return;
            for (int i = 1; i < attack_buffCkilds.Length; i++)
            {
                if (str.Equals(attack_buffCkilds[i].name))
                {
                    Debug.LogError("去除buff---------------------------------------");
                    attack_buffCkilds[i].gameObject.SetActive(false);
                    Destroy(attack_buffCkilds[i].gameObject);
                }
            }
        }

        //承伤buff特殊处理
        public void DestoryProjectBuff(string buff)
        {
            string str = buff.Substring(2, buff.Length - 2);
            string oldStr = str;

            GameObject buffParents;
            RoleConfig attack_roleIns;
            Transform[] attack_buffCkilds = null;
            ColorDebug.Instance.DicDebug<string, Sprite>(
                ADDUIBattle.instance.ResourcesLoadBattleSceneInfos.roleBuffImagesDics, false);

            for (int i = 0; i < CreatSkeleton.Instance.roleSkeletonsBattleAni.Count; i++)
            {
                attack_roleIns = CreatSkeleton.Instance.roleSkeletonsBattleAni[i].GetComponent<RoleConfig>();
                if (attack_roleIns.id == 10004) str = "Main" + str;
                else str = oldStr;
                if (CreatSkeleton.Instance.BloodsDic.ContainsKey(attack_roleIns.id))
                {
                    CreatSkeleton.Instance.BloodsObjDic.TryGetValue(attack_roleIns.id, out buffParents);
                    buffParents = buffParents.transform.Find("buffParents").gameObject;
                    attack_buffCkilds = buffParents.GetComponentsInChildren<Transform>();
                }

                if (attack_buffCkilds == null) return;
                if (attack_buffCkilds.Length == 1) return;
                for (int j = 1; j < attack_buffCkilds.Length; j++)
                {
                    if (str.Equals(attack_buffCkilds[j].name))
                    {
                        Debug.LogError("去除buff---------------------------------------");
                        attack_buffCkilds[j].gameObject.SetActive(false);
                        Destroy(attack_buffCkilds[j].gameObject);
                    }
                }
            }

        }

        //一波胜利后所有buff debuff回合-1
        public void AllBuffReduceOne()
        {
            GameObject buffParents;
            RoleConfig attack_roleIns;
            Transform[] attack_buffCkilds = null;
            ColorDebug.Instance.DicDebug<string, Sprite>(
                ADDUIBattle.instance.ResourcesLoadBattleSceneInfos.roleBuffImagesDics, false);

            for (int i = 0; i < CreatSkeleton.Instance.roleSkeletonsBattleAni.Count; i++)
            {
                attack_roleIns = CreatSkeleton.Instance.roleSkeletonsBattleAni[i].GetComponent<RoleConfig>();

                if (CreatSkeleton.Instance.BloodsDic.ContainsKey(attack_roleIns.id))
                {
                    CreatSkeleton.Instance.BloodsObjDic.TryGetValue(attack_roleIns.id, out buffParents);
                    buffParents = buffParents.transform.Find("buffParents").gameObject;
                    attack_buffCkilds = buffParents.GetComponentsInChildren<Transform>();
                }

                ColorDebug.Instance.ArrayDebug<Transform>(attack_buffCkilds, true);
                if (attack_buffCkilds == null) continue;
                if (attack_buffCkilds.Length == 1) continue;
                for (int j = 1; j < attack_buffCkilds.Length; j++)
                {
                    SkillEffect.Instance.CompareCalculate(attack_buffCkilds[j].name, 1, attack_roleIns);
                }

                SkillEffect.Instance.RefreshBuff(attack_roleIns, attack_buffCkilds);
            }
        }

        public void DestoryTargetDeBuff()
        {
            // string str = buff.Substring(2,buff.Length-2);

            if (SkillEffect.Instance.target_buffCkilds == null) return;
            if (SkillEffect.Instance.target_buffCkilds.Length == 1) return;
            for (int i = 1; i < SkillEffect.Instance.target_buffCkilds.Length; i++)
            {
                if (SkillEffect.Instance.target_buffCkilds[i].name.Equals("SpeedReduce") ||
                    SkillEffect.Instance.target_buffCkilds[i].name.Equals("MagicNail") ||
                    SkillEffect.Instance.target_buffCkilds[i].name.Equals("Dizziness") ||
                    SkillEffect.Instance.target_buffCkilds[i].name.Equals("ForceAttack") ||
                    SkillEffect.Instance.target_buffCkilds[i].name.Equals("Silience") ||
                    SkillEffect.Instance.target_buffCkilds[i].name.Equals("DefenceReduce") ||
                    SkillEffect.Instance.target_buffCkilds[i].name.Equals("AttackReduce"))
                {
                    Debug.LogError("去除buff---------------------------------------");
                    SkillEffect.Instance.target_buffCkilds[i].gameObject.SetActive(false);
                    Destroy(SkillEffect.Instance.target_buffCkilds[i].gameObject);
                }
            }
        }

        void CheckBeginBuff()
        {
            Transform[] trans = SkillEffect.Instance.FindAttackChilds();
            if (trans == null) return;
            if (trans.Length == 1) return;
            ColorDebug.Instance.ArrayDebug<Transform>(trans, true);
            for (int i = 1; i < trans.Length; i++)
            {
                if (trans[i].name.Equals("continueBoutText")) continue;
                SkillEffect.Instance.attack_beginbuff.Add(trans[i].name, trans[i].name);
                SkillEffect.Instance.attack_beginbuffList.Add(trans[i]);
            }

            CalculateAfterBuffDamage();
            ColorDebug.Instance.DicDebug<string, string>(SkillEffect.Instance.attack_beginbuff, true);
        }

        void CalculateAfterBuffDamage()
        {
            RoleConfig.RoleInfo m_RoleInfo = SkillEffect.Instance.attack_roleIns.m_RoleInfo;
            m_RoleInfo.attack = ReadJsonfiles.Instance.roleDics[m_RoleInfo.roleid].attack;

            //被动技能检测
            passiveValue = SkillEffect.Instance.CheckIsThisSkillToAddAttack_EffectType_Eleven();
            Debug.LogError("被动数值是：--" + passiveValue);
            //角色攻击力计算公式
            if (m_RoleInfo.isAttackAdd) attackAdd = SetConfig.isAttackAdd;
            if (m_RoleInfo.isAttackReduce) attackReduce = SetConfig.isAttackReduce;
            m_RoleInfo.attack = m_RoleInfo.attack * (passiveValue + attackAdd + attackReduce + 1);
            attackAdd = 0f;
            attackReduce = 0f;

            // if(m_RoleInfo.isAttackAdd)m_RoleInfo.attack += m_RoleInfo.attack*SetConfig.isAttackAdd;
            // if(m_RoleInfo.isAttackReduce)m_RoleInfo.attack += m_RoleInfo.attack*SetConfig.isAttackReduce;
            // if(m_RoleInfo.isDefenceAdd)m_RoleInfo.defense += m_RoleInfo.defense*SetConfig.isDefenceAdd;
            // if(m_RoleInfo.isDefenceReduce)m_RoleInfo.defense += m_RoleInfo.defense*SetConfig.isDefenceReduce;
            // if(m_RoleInfo.isSpeedReduce)m_RoleInfo.speed += m_RoleInfo.speed*SetConfig.isSpeedReduce;
        }

        //防御力计算
        void CalculateDefense()
        {
            RoleConfig roleCon;
            for (int i = 0; i < ADDUIBattle.instance.battleCurrentAll.Count; i++)
            {
                roleCon = ADDUIBattle.instance.battleCurrentAll[i].GetComponent<RoleConfig>();
                if (roleCon.m_RoleInfo.isDefenceAdd) defenseAdd = SetConfig.isDefenceAdd;
                if (roleCon.m_RoleInfo.isDefenceReduce) defenseReduce = SetConfig.isDefenceReduce;
                roleCon.m_RoleInfo.defense = ReadJsonfiles.Instance.roleDics[roleCon.m_RoleInfo.roleid].defense;
                roleCon.m_RoleInfo.defense = roleCon.m_RoleInfo.defense * (defenseAdd + defenseReduce + 1);
                defenseAdd = 0f;
                defenseReduce = 0f;

                if (roleCon.m_RoleInfo.isSpeedReduce) speedReduce = SetConfig.isSpeedReduce;
                roleCon.m_RoleInfo.speed = ReadJsonfiles.Instance.roleDics[roleCon.m_RoleInfo.roleid].speed;
                roleCon.m_RoleInfo.speed = roleCon.m_RoleInfo.speed * (speedReduce + 1);
                speedReduce = 0f;
            }
        }

        //速度计算
        void CalculateSpeed()
        {
            RoleConfig roleCon;
            RoleConfig roleCon1;
            for (int i = 0; i < ADDUIBattle.instance.speedImages.Count; i++)
            {
                roleCon = ADDUIBattle.instance.battleCurrentAll[i].GetComponent<RoleConfig>();
                roleCon1 = ADDUIBattle.instance.speedImages[i].GetComponent<RoleConfig>();
                if (roleCon.m_RoleInfo.isSpeedReduce) speedReduce = SetConfig.isSpeedReduce;
                roleCon1.m_RoleInfo.speed = ReadJsonfiles.Instance.roleDics[roleCon1.m_RoleInfo.roleid].speed;
                roleCon1.m_RoleInfo.speed = roleCon1.m_RoleInfo.speed * (speedReduce + 1);
                speedReduce = 0f;
            }
        }

        /// <summary>
        /// 动画设置
        /// </summary>
        //通过技能目标 发送动画(预留，方便控制)
        void PassSkiiTargetSendSpineAni(bool attackIsRole, string spineName)
        {
            Skills usingSkill = ReadJsonfiles.Instance.skillDics[int.Parse(whichSkill.name)];

            if (attackIsRole)
            {
                if (usingSkill.releaseobj == (int) TargetType.singleEnemy)
                {
                    SelectRoleSpineAni(spineName, false, false);
                }
                else if (usingSkill.releaseobj == (int) TargetType.allEnemy)
                {
                    SelectRoleSpineAni(spineName, true, false);
                }
            }
            else
            {
                if (usingSkill.releaseobj == (int) TargetType.singleEnemy)
                {
                    SelectMonsterSpineAni(spineName, false, false);
                }
                else if (usingSkill.releaseobj == (int) TargetType.allEnemy)
                {
                    SelectMonsterSpineAni(spineName, true, false);
                }
            }
        }

        //Role -> Monster
        void SelectRoleSpineAni(string spineName, bool allAction = false, bool isLoop = false)
        {
            switch (spineName)
            {
                case "idle":
                    RoleSpineAniManager.Instance.SendMsg(ButtonMsg.GetInstance.ChangeInfo(
                        (ushort) MonsterSpineAniId.idle, target_roleIns.iddif, "normal", allAction, isLoop));
                    break;
                case "hit":
                    RoleSpineAniManager.Instance.SendMsg(ButtonMsg.GetInstance.ChangeInfo(
                        (ushort) MonsterSpineAniId.attacked, target_roleIns.iddif, "hit", allAction, isLoop));
                    break;
                // case "idle":
                // break;
                // case "idle":
                // break;
                // case "idle":
                // break;
                // case "idle":
                // break;
            }
        }

        //Monster -> Role
        void SelectMonsterSpineAni(string spineName, bool allAction = false, bool isLoop = false)
        {
            switch (spineName)
            {
                case "idle":
                    MonsterSpineAniManager.Instance.SendMsg(
                        ButtonMsg.GetInstance.ChangeInfo((ushort) RoleSpineAniId.idle, target_roleIns.id, "normal",
                            allAction, isLoop));
                    break;
                case "hit":
                    MonsterSpineAniManager.Instance.SendMsg(
                        ButtonMsg.GetInstance.ChangeInfo((ushort) RoleSpineAniId.attacked, target_roleIns.id, "hit",
                            allAction, isLoop));
                    break;
                // case "idle":
                // break;
                // case "idle":
                // break;
                // case "idle":
                // break;
                // case "idle":
                // break;
            }
        }

        //通过技能目标 发送受击动画
        void PassSkiiTargetSendAttacked(bool attackIsRole)
        {
            Skills usingSkill = ReadJsonfiles.Instance.skillDics[int.Parse(whichSkill.name)];

            if (attackIsRole)
            {
                if (usingSkill.releaseobj == (int) TargetType.singleEnemy)
                {
                    // if(SkillEffect.Instance.target_roleIns.m_RoleInfo.isDizziness)return;
                    RoleSpineAniManager.Instance.SendMsg(ButtonMsg.GetInstance.ChangeInfo(
                        (ushort) MonsterSpineAniId.attacked, target_roleIns.iddif, "hit", false, false));
                }
                else if (usingSkill.releaseobj == (int) TargetType.allEnemy)
                {
                    RoleSpineAniManager.Instance.SendMsg(ButtonMsg.GetInstance.ChangeInfo(
                        (ushort) MonsterSpineAniId.attacked, target_roleIns.iddif, "hit", true, false));
                }
            }
            else
            {
                if (usingSkill.releaseobj == (int) TargetType.singleEnemy)
                {
                    MonsterSpineAniManager.Instance.SendMsg(
                        ButtonMsg.GetInstance.ChangeInfo((ushort) RoleSpineAniId.attacked, target_roleIns.id, "hit",
                            false, false));
                }
                else if (usingSkill.releaseobj == (int) TargetType.allEnemy)
                {
                    MonsterSpineAniManager.Instance.SendMsg(
                        ButtonMsg.GetInstance.ChangeInfo((ushort) RoleSpineAniId.attacked, target_roleIns.id, "hit",
                            true, false));
                }
            }
        }

        /// <summary>
        /// ceshi 播放受击
        /// </summary>
        /// <param name="attackIsRole"></param>
        void PassSkiiTargetSendAttackedFromExl(bool attackIsRole)
        {
            Skills usingSkill = ReadJsonfiles.Instance.skillDics[int.Parse(whichSkill.name)];

            if (attackIsRole)
            {
                FixSpineAni.Instance.FixSpineAniFormFrame(usingSkill, target_roleIns, recordSkillName, attackIsRole);
            }
            else
            {
                if (usingSkill.releaseobj == (int) TargetType.singleEnemy)
                {
                    MonsterSpineAniManager.Instance.SendMsg(
                        ButtonMsg.GetInstance.ChangeInfo((ushort) RoleSpineAniId.attacked, target_roleIns.id, "hit",
                            false, false));
                }
                else if (usingSkill.releaseobj == (int) TargetType.allEnemy)
                {
                    MonsterSpineAniManager.Instance.SendMsg(
                        ButtonMsg.GetInstance.ChangeInfo((ushort) RoleSpineAniId.attacked, target_roleIns.id, "hit",
                            true, false));
                }
            }
        }

        /// <summary>
        /// ceshi 播放受击1
        /// </summary>
        /// <param name="attackIsRole"></param>
        void PassSkiiTargetSendAttackedFromExl1(bool attackIsRole)
        {
            Skills usingSkill = ReadJsonfiles.Instance.skillDics[int.Parse(whichSkill.name)];

            if (attackIsRole)
            {
                if (usingSkill.releaseobj == (int) TargetType.singleEnemy)
                {
                    MyTimerTool.Instance.wtime.AddTimeTask((int tid) =>
                    {
                        RoleSpineAniManager.Instance.SendMsg(ButtonMsg.GetInstance.ChangeInfo(
                            (ushort) MonsterSpineAniId.attacked, target_roleIns.iddif, "hit", false, false));
                        Debug.Log("1-------------------------------1");
                    }, 0.28f, WinterTimeUnit.Second, 1);
                    MyTimerTool.Instance.wtime.AddTimeTask((int tid) =>
                    {
                        RoleSpineAniManager.Instance.SendMsg(ButtonMsg.GetInstance.ChangeInfo(
                            (ushort) MonsterSpineAniId.attacked, target_roleIns.iddif, "hit", false, false));
                        Debug.Log("1-------------------------------2");

                    }, 0.6f, WinterTimeUnit.Second, 1);
                    MyTimerTool.Instance.wtime.AddTimeTask((int tid) =>
                    {
                        RoleSpineAniManager.Instance.SendMsg(ButtonMsg.GetInstance.ChangeInfo(
                            (ushort) MonsterSpineAniId.attacked, target_roleIns.iddif, "hit", false, false));
                        Debug.Log("1-------------------------------3");

                    }, 1.08f, WinterTimeUnit.Second, 1);

                }
                else if (usingSkill.releaseobj == (int) TargetType.allEnemy)
                {
                    MyTimerTool.Instance.wtime.AddTimeTask((int tid) =>
                    {
                        RoleSpineAniManager.Instance.SendMsg(ButtonMsg.GetInstance.ChangeInfo(
                            (ushort) MonsterSpineAniId.attacked, target_roleIns.iddif, "hit", true, false));
                        Debug.Log("1-------------------------------1");
                    }, 0.28f, WinterTimeUnit.Second, 1);
                    MyTimerTool.Instance.wtime.AddTimeTask((int tid) =>
                    {
                        RoleSpineAniManager.Instance.SendMsg(ButtonMsg.GetInstance.ChangeInfo(
                            (ushort) MonsterSpineAniId.attacked, target_roleIns.iddif, "hit", true, false));
                        Debug.Log("1-------------------------------2");

                    }, 0.6f, WinterTimeUnit.Second, 1);
                    MyTimerTool.Instance.wtime.AddTimeTask((int tid) =>
                    {
                        RoleSpineAniManager.Instance.SendMsg(ButtonMsg.GetInstance.ChangeInfo(
                            (ushort) MonsterSpineAniId.attacked, target_roleIns.iddif, "hit", true, false));
                        Debug.Log("1-------------------------------3");

                    }, 1.08f, WinterTimeUnit.Second, 1);
                }
            }
            else
            {
                if (usingSkill.releaseobj == (int) TargetType.singleEnemy)
                {
                    MonsterSpineAniManager.Instance.SendMsg(
                        ButtonMsg.GetInstance.ChangeInfo((ushort) RoleSpineAniId.attacked, target_roleIns.id, "hit",
                            false, false));
                }
                else if (usingSkill.releaseobj == (int) TargetType.allEnemy)
                {
                    MonsterSpineAniManager.Instance.SendMsg(
                        ButtonMsg.GetInstance.ChangeInfo((ushort) RoleSpineAniId.attacked, target_roleIns.id, "hit",
                            true, false));
                }
            }
        }

        void PassSkiiTargetSendIdle(bool attackIsRole)
        {
            Skills usingSkill = ReadJsonfiles.Instance.skillDics[int.Parse(whichSkill.name)];
            if (usingSkill.releaseobj == (int) TargetType.singleEnemy)
                if (SkillEffect.Instance.target_roleIns.m_RoleInfo.isDizziness)
                    return;
            // if(usingSkill.releaseobj == (int) TargetType.allEnemy)
            // {
            //     if(attackIsRole)
            //     {
            //         for (int i = 0; i < CreatSkeleton.Instance.monsterSkeletonsMonsterBattleAni.Count; i++)
            //         {
            //             if(CreatSkeleton.Instance.monsterSkeletonsMonsterBattleAni[i].GetComponent<RoleConfig>().m_RoleInfo.isDizziness)
            //                 continue;
            //         }
            //     }
            //     else
            //     {

            //     }
            // }
            if (attackIsRole)
            {
                if (usingSkill.releaseobj == (int) TargetType.singleEnemy)
                {
                    RoleSpineAniManager.Instance.SendMsg(
                        ButtonMsg.GetInstance.ChangeInfo((ushort) MonsterSpineAniId.idle, target_roleIns.iddif,
                            "normal", false, true));
                }
                else if (usingSkill.releaseobj == (int) TargetType.allEnemy)
                {
                    RoleSpineAniManager.Instance.SendMsg(
                        ButtonMsg.GetInstance.ChangeInfo((ushort) MonsterSpineAniId.idle, target_roleIns.iddif,
                            "normal", true, false));
                }
            }
            else
            {
                if (usingSkill.releaseobj == (int) TargetType.singleEnemy)
                {
                    MonsterSpineAniManager.Instance.SendMsg(
                        ButtonMsg.GetInstance.ChangeInfo((ushort) RoleSpineAniId.idle, target_roleIns.id, "normal",
                            false, true));
                }
                else if (usingSkill.releaseobj == (int) TargetType.allEnemy)
                {
                    MonsterSpineAniManager.Instance.SendMsg(
                        ButtonMsg.GetInstance.ChangeInfo((ushort) RoleSpineAniId.idle, target_roleIns.id, "normal",
                            true, false));
                }
            }
        }

        public void ControlRoleSpineAni(string spineName, bool allAction = false, bool isLoop = false)
        {
            switch (spineName)
            {
                case "idle":
                    RoleSpineAniManager.Instance.SendMsg(ButtonMsg.GetInstance.ChangeInfo((ushort) RoleSpineAniId.idle,
                        target_roleIns.id, "normal", allAction, isLoop));
                    break;
                case "hit":
                    RoleSpineAniManager.Instance.SendMsg(ButtonMsg.GetInstance.ChangeInfo(
                        (ushort) RoleSpineAniId.attacked, target_roleIns.id, "hit", allAction, isLoop));
                    break;
                // case "idle":
                // break;
                // case "idle":
                // break;
                // case "idle":
                // break;
                // case "idle":
                // break;
            }
        }

        public void ControlMonsterSpineAni(string spineName, bool allAction = false, bool isLoop = false)
        {
            switch (spineName)
            {
                case "idle":
                    MonsterSpineAniManager.Instance.SendMsg(ButtonMsg.GetInstance.ChangeInfo(
                        (ushort) MonsterSpineAniId.idle, target_roleIns.iddif, "normal", allAction, isLoop));
                    break;
                case "hit":
                    MonsterSpineAniManager.Instance.SendMsg(ButtonMsg.GetInstance.ChangeInfo(
                        (ushort) MonsterSpineAniId.attacked, target_roleIns.iddif, "hit", allAction, isLoop));
                    break;
                // case "idle":
                // break;
                // case "idle":
                // break;
                // case "idle":
                // break;
                // case "idle":
                // break;
            }
        }

        void StrikeBack(RoleConfig roleImage)
        {
            LoadSpineAni();

        }

        void LoadSpineAni()
        {
            if (CreatSkeleton.Instance.roleSkeletonsBattleAniDic[10003].transform.childCount != 0)
            {
                strikeBackAni.AnimationName = "";
                strikeBackAni.AnimationName = "fanji";
                return;
            }

            strikeBackAni = GameObject.Instantiate(ADDUIBattle.instance.ResourcesLoadBattleSceneInfos.spineEffectsPre);

            strikeBackAni.transform.SetParent(CreatSkeleton.Instance.roleSkeletonsBattleAniDic[10003].transform);
            strikeBackAni.transform.localPosition = new Vector3(0, 5, 0);
            strikeBackAni.transform.localScale = new Vector3(5, 5, 1);
        }
    }
}