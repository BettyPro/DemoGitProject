using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace Demo
{
//public class CreatSkeleton : UnitySingleton<CreatSkeleton>
    public class CreatSkeleton : Singleton<CreatSkeleton>
    {
        //Roles roleUse;

        public List<SkeletonGraphic> skeletons = new List<SkeletonGraphic>(); //Role界面的展示人物Spine
        public List<SkeletonGraphic> skeletonsBattle = new List<SkeletonGraphic>(); //战斗界面不用3dSpine时的UISpine
        public List<SkeletonAnimation> skeletonsBattleAni = new List<SkeletonAnimation>(); //战斗界面的人员
        public List<SkeletonAnimation> roleSkeletonsBattleAni = new List<SkeletonAnimation>(); //战斗界面的人员 -- 用于技能伤害计算

        public Dictionary<int, SkeletonAnimation>
            skeletonsBattleAniDic = new Dictionary<int, SkeletonAnimation>(); //战斗界面的人员

        public Dictionary<int, SkeletonAnimation>
            roleSkeletonsBattleAniDic = new Dictionary<int, SkeletonAnimation>(); //战斗界面的人员 -- 用于技能伤害计算

        public List<SkeletonAnimation> skeletonsMonsterBattleAni = new List<SkeletonAnimation>(); //战斗界面的NPC

        public List<SkeletonAnimation>
            monsterSkeletonsMonsterBattleAni = new List<SkeletonAnimation>(); //战斗界面的NPC -- 用于技能伤害计算

        public Dictionary<int, SkeletonAnimation> skeletonsMonsterBattleAniDic =
            new Dictionary<int, SkeletonAnimation>(); //战斗界面的NPC

        public Dictionary<int, SkeletonAnimation> monsterSkeletonsMonsterBattleAniDic =
            new Dictionary<int, SkeletonAnimation>(); //战斗界面的NPC -- 用于技能伤害计算

        public List<GameObject> roleBloods = new List<GameObject>(); //战斗界面人物血条
        public List<GameObject> monsterBloods = new List<GameObject>(); //战斗界面NPC血条
        public Dictionary<int, GameObject> BloodsObjDic = new Dictionary<int, GameObject>(); //战斗界面血条(试用中)
        public Dictionary<int, Text> BloodsDic = new Dictionary<int, Text>(); //战斗界面血条(试用中)
        public Dictionary<int, Text> roleBloodsDic = new Dictionary<int, Text>(); //战斗界面血条
        public Dictionary<int, Text> monsterBloodsDic = new Dictionary<int, Text>(); //战斗界面血条

        public Image moveImage;
        //public int[] levels = { 2001, 2002, 2003, 2004 };

        //Role界面的展示人物Spine
        public void CreatSkeletonInfo(Transform trans, float x = -30f, float y = -140f, float z = 0f)
        {
            SkeletonGraphic[] skeleon = Resources.LoadAll<SkeletonGraphic>(SetConfig.roleSceneSpineAniShowUI);

            for (int i = 0; i < skeleon.Length; i++)
            {
                SkeletonGraphic obj = SkeletonGraphic.Instantiate(skeleon[i]);
                obj.transform.SetParent(trans);
                obj.name = "skeleon" + (i + 1).ToString();
                obj.transform.localPosition = new Vector3(x, y, z);
                obj.transform.localScale = Vector3.one;
                skeletons.Add(obj);
                obj.gameObject.SetActive(false);
                if (i > 1)
                    obj.transform.localScale = new Vector3(0.5f, 0.5f, 1);
            }
        }

        //战斗界面不用3dSpine时的UISpine
        public void CreatSkeletonInfo(Transform trans, string str, float x = -30f, float y = -140f, float z = 0f)
        {
            SkeletonGraphic[] skeleon = Resources.LoadAll<SkeletonGraphic>(SetConfig.roleSceneSpineAniShowUITest);
            SkeletonGraphic obj;
            for (int i = 0; i < skeleon.Length; i++)
            {
                obj = SkeletonGraphic.Instantiate(skeleon[i]);
                obj.transform.SetParent(trans);
                obj.name = "skeleon" + (i + 1).ToString();
                obj.transform.localPosition = new Vector3(x - 60 * i, y, z);
                obj.transform.localScale = new Vector3(0.5f, 0.5f, 1);
                //obj.gameObject.AddComponent<MoveConteralTest>();
                skeletonsBattle.Add(obj);
                obj.gameObject.SetActive(true);
                if (i > 1)
                    obj.transform.localScale = new Vector3(0.5f, 0.5f, 1);
                if (i == 4)
                {
                    obj.transform.localScale = new Vector3(0.25f, 0.25f, 1);

                    skeletonsBattle[0].gameObject.SetActive(true);
                    skeletonsBattle[0].startingLoop = false;
                    skeletonsBattle[0].startingAnimation = "idle";
                }

                if (i == 3)
                {
                    obj.transform.localScale = new Vector3(0.2f, 0.2f, 1);
                }

                if (i == 2)
                {
                    obj.transform.localScale = new Vector3(0.25f, 0.25f, 1);
                }

                if (i == 1)
                {
                    obj.transform.localScale = new Vector3(0.4f, 0.4f, 1);
                }
            }
        }

        //3d
        //角色spine展示
        public void CreatSkeletonAniInfo(Transform trans, float x = -30f, float y = -140f, float z = 0f)
        {
            Roles roleUse;
            SkeletonAnimation[] skeleon;
            if (SetConfig.GetInstance().isTest)
                skeleon = Resources.LoadAll<SkeletonAnimation>(SetConfig.roleSpineShow);
            else
                skeleon = Resources.LoadAll<SkeletonAnimation>(SetConfig.roleSceneSpineAniShow);
            for (int i = 0; i < skeleon.Length; i++)
            {
                SkeletonAnimation obj = SkeletonAnimation.Instantiate<SkeletonAnimation>(skeleon[i], trans);
                obj.transform.SetParent(trans);
                obj.name = "skeleon" + (i + 1).ToString();
                //obj.transform.localPosition = new Vector3(x-1*i, (y+(0.2f*i)), z);
                //obj.transform.localPosition = new Vector3(x - 50 * i, (y + (0.2f * i)), z + (0.2f * i));
                if (i == 0)
                {
                }
                else
                {
                    x = i % 2 != 0 ? 80 : -20;
                    //y = i % 2 != 0 ?100 : y + (20f * i);
                    //z = z + (0.2f * i);
                }

                if (i == 0)
                    obj.transform.localPosition = new Vector3(x, 0, 0);
                if (i == 1)
                    obj.transform.localPosition = new Vector3(x, 100, 0);//50
                if (i == 2)
                    obj.transform.localPosition = new Vector3(x, 50, 0);
                if (i == 3)
                    obj.transform.localPosition = new Vector3(x, -100, -0);//-50
                if (i == 4)
                    obj.transform.localPosition = new Vector3(-200, 0, 0);//50

                //obj.transform.localPosition = new Vector3(x,y,z);

                roleUse = ReadJsonfiles.Instance.roleInfos[i];
                RoleConfig re = obj.gameObject.AddComponent<RoleConfig>();
                //re.id = ReadJsonfiles.Instance.roleIds[i];
                re.id = ReadJsonfiles.Instance.roleIdsRoleSceneUse[i];
                re.UpdateInfo(roleUse);
                RefreshSkillCD(re);
                obj.name = re.id.ToString();

                skeletonsBattleAni.Add(obj);
                roleSkeletonsBattleAni.Add(obj);
                skeletonsBattleAniDic.Add(re.id, obj);
                roleSkeletonsBattleAniDic.Add(re.id, obj);
                ADDUIBattle.instance.battleCurrentAll.Add(obj);

                obj.transform.localScale = new Vector3(20, 20, 1);
                // obj.transform.localScale = new Vector3(50, 50, 1);
                //if (i == 1)
                //{
                //    obj.transform.localScale = new Vector3(10f, 10f, 1);
                //}

                //if (i == 0)
                //{
                //    obj.transform.localScale = new Vector3(50f, 50f, 1);
                //}
                RoleSpineAniManager.Instance.SendMsg(ButtonMsg.GetInstance.ChangeInfo((ushort) RoleSpineAniId.idle,
                    10001, "normal", true, true));
                BloodShow(obj.transform, re, re.id, re.id, roleBloods, true);
            }
        }

        void BloodShow(Transform trans, RoleConfig role, int nameId, int id, List<GameObject> obj, bool isRole = false)
        {
            Text allBlood;
            ADDUIBattle.instance.bloodParent =
                GameObject.Find("CanvasBattle/PlayerPanel/bloodParent").GetComponent<RectTransform>();
            GameObject blood = GameObject.Instantiate(ResourcesLoadInfos.instance.bloodImage.gameObject,
                ADDUIBattle.instance.bloodParent) as GameObject;
            blood.name = "blood" + nameId;
            blood.name = nameId.ToString();
            allBlood = GameObject.Find(blood.name + "/allBlood").GetComponent<Text>();
            allBlood.text = ADDUIBattle.instance.deal.Blood(id);
            moveImage = blood.transform.Find("moveImage").GetComponent<Image>();
            if (isRole)
            {
                Text rankText;
                rankText = GameObject.Find(blood.name + "/rank/rankText").GetComponent<Text>();
                rankText.text = ADDUIBattle.instance.deal.Rank(id, isRole);
                //3D坐标转换为2D坐标，WorldToScreenPoint:世界位置转换为屏幕位置
                Vector2 cubeV2Pos = RectTransformUtility.WorldToScreenPoint(Camera.main, trans.position);
                blood.GetComponent<RectTransform>().position = cubeV2Pos + new Vector2(50, 200);
                //blood.GetComponent<RectTransform>().localPosition = cubeV2Pos + new Vector2(-430, 10);
                //blood.GetComponent<RectTransform>().anchoredPosition = cubeV2Pos + new Vector2(-430, 10);
                BloodsDic.Add(role.id, allBlood);
                BloodsObjDic.Add(role.id, blood);
            }
            else
            {
                //3D坐标转换为2D坐标，WorldToScreenPoint:世界位置转换为屏幕位置
                Vector2 cubeV2Pos = RectTransformUtility.WorldToScreenPoint(Camera.main, trans.position);
                blood.GetComponent<RectTransform>().position = cubeV2Pos + new Vector2(-50, 240);
                blood.GetComponent<RectTransform>().position = cubeV2Pos + new Vector2(-120, 240);
                //blood.GetComponent<RectTransform>().anchoredPosition = cubeV2Pos + new Vector2(-50, 240);
                BloodsDic.Add(role.iddif, allBlood);
                BloodsObjDic.Add(role.iddif, blood);
            }

            obj.Add(blood);
        }

        public void ControlBloodShow(bool show = true, bool left = false)
        {
            float x = 0f;
            float y = 0f;
            float z = 0f;
            for (int i = 0; i < roleBloods.Count; i++)
            {
                roleBloods[i].SetActive(show);
                if (left)
                {
                    x = roleBloods[i].GetComponent<RectTransform>().localPosition.x + 400f;
                    y = roleBloods[i].GetComponent<RectTransform>().localPosition.y;
                    z = roleBloods[i].GetComponent<RectTransform>().localPosition.z;
                    //bloods[i].GetComponent<RectTransform>().localPosition = new Vector3(x, bloods[i].transform.localPosition.y, bloods[i].transform.localPosition.z);
                    roleBloods[i].GetComponent<RectTransform>().localPosition = new Vector3(x, y, z);
                    left = false;
                    x = 0f;
                    y = 0f;
                    z = 0f;
                }

                if (CreatSkeleton.Instance.skeletonsBattleAni[i].transform.localRotation.y < 0)
                    roleBloods[i].SetActive(left);
                //Debug.Log(CreatSkeleton.Instance.skeletonsBattleAni[2].transform.localRotation.y);
            }
        }

        //怪物spine展示
        public void CreatMonsterSkeletonAniInfo(Transform trans, float x = -30f, float y = -140f, float z = 0f)
        {
            SkeletonAnimation[] skeleon = Resources.LoadAll<SkeletonAnimation>(SetConfig.monsterSpineAniShow);
            for (int i = 0; i < skeleon.Length; i++)
            {
                SkeletonAnimation obj = SkeletonAnimation.Instantiate<SkeletonAnimation>(skeleon[i], trans);
                obj.transform.SetParent(trans);
                obj.name = "skeleon" + (i + 1).ToString();
                //obj.transform.localPosition = new Vector3(x-1*i, (y+(0.2f*i)), z);
                obj.transform.localPosition = new Vector3(x + 50 * i, (y + (2f * i)), z + (0.2f * i));
                obj.transform.localRotation = new Quaternion(0, 180, 0, 0);
                skeletonsMonsterBattleAni.Add(obj);

                obj.transform.localScale = new Vector3(20, 20, 1);
                if (i == 0)
                {
                    obj.transform.localScale = new Vector3(10f, 10f, 1);
                }
            }
        }

        //怪物spine展示(通过配置表进行)
        public void CreatMonsterSkeletonAniInfos(Vector3 battlePlace, Transform trans, float x = -30f, float y = -140f,
            float z = 0f)
        {
            if (battlePlace == SetConfig.firstPlaceCompare)
            {
                SelectMonsters(0, trans, x, y, z);
            }
            else if (battlePlace == SetConfig.secondPlaceCompare)
            {
                SelectMonsters(1, trans, x, y, z);
            }
            else
            {
                SelectMonsters(2, trans, x, y, z);
            }

            // MonsterSpineAniManager.Instance.SendMsg(ButtonMsg.GetInstance.ChangeInfo((ushort)MonsterSpineAniId.idle, (ushort)MonsterId.NPC1));
            MonsterSpineAniManager.Instance.SendMsg(ButtonMsg.GetInstance.ChangeInfo((ushort) MonsterSpineAniId.idle,
                true));
        }

        void SelectMonsters(int index, Transform trans, float x = -30f, float y = -140f, float z = 0f)
        {
            Roles roleUse;
            int num = 0;
            int count = 0;
            for (int i = 0; i < ResourcesLoadInfos.instance.skeleonDics.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        num = ReadJsonfiles.Instance.levels[index].NPCID1;
                        break;
                    case 1:
                        num = ReadJsonfiles.Instance.levels[index].NPCID2;
                        break;
                    case 2:
                        num = ReadJsonfiles.Instance.levels[index].NPCID3;
                        break;
                    case 3:
                        num = ReadJsonfiles.Instance.levels[index].NPCID4;
                        break;
                }

                if (ResourcesLoadInfos.instance.skeleonDics.ContainsKey(num))
                {
                    SkeletonAnimation outObj;
                    ResourcesLoadInfos.instance.skeleonDics.TryGetValue(num, out outObj);
                    SkeletonAnimation obj = SkeletonAnimation.Instantiate<SkeletonAnimation>(outObj);
                    obj.transform.SetParent(trans);
                    obj.name = "skeleon" + (i + 1).ToString();
                    if (i == 0)
                    {
                        x = ADDUIBattle.instance.PlayerContrBattle.transform.position.x + 450;
                        y = 80;
                        z = 0;
                    }

                    //obj.transform.localPosition = new Vector3(1300, 80, 0);
                    if (i == 1)
                    {
                        x = 1600;
                        x = ADDUIBattle.instance.PlayerContrBattle.transform.position.x + 850;
                        x = ADDUIBattle.instance.PlayerContrBattle.transform.position.x + 750;
                        y = 80;
//                        z = 250;
                        z = 0;
                    }

                    //obj.transform.localPosition = new Vector3(1600, 80, 250);
                    if (i == 2)
                    {
                        x = 1400;
                        x = ADDUIBattle.instance.PlayerContrBattle.transform.position.x + 650;

                        y = 10;
//                        z = 30;
                        z = 0;
                    }

                    //obj.transform.localPosition = new Vector3(1400, 10, 0);
                    if (i == 3)
                    {
                        x = 1600;
                        x = ADDUIBattle.instance.PlayerContrBattle.transform.position.x + 850;

                        y = 30;
                        z = 0;
                    }
                    //obj.transform.localPosition = new Vector3(1600, 30, 0);

                    obj.transform.localPosition = new Vector3(x, y, z);
                    obj.transform.localRotation = new Quaternion(0, 180, 0, 0);

                    if (ReadJsonfiles.Instance.roleDics.ContainsKey(num))
                    {
                        ReadJsonfiles.Instance.roleDics.TryGetValue(num, out roleUse);
                        RoleConfig re = obj.gameObject.AddComponent<RoleConfig>();
                        re.id = num;
                        re.iddif = int.Parse(num + "0" + count);
                        re.UpdateInfo(roleUse);
                        RefreshSkillCD(re);
                        count++;
                        skeletonsMonsterBattleAniDic.Add(re.iddif, obj);
                        monsterSkeletonsMonsterBattleAniDic.Add(re.iddif, obj);

                        BloodShow(obj.transform, re, re.iddif, re.id, monsterBloods, false);
                        obj.name = re.iddif.ToString();
                    }

                    skeletonsMonsterBattleAni.Add(obj);
                    monsterSkeletonsMonsterBattleAni.Add(obj);
                    ADDUIBattle.instance.battleCurrentAll.Add(obj);

                    obj.transform.localScale = new Vector3(20, 20, 1);
                    // obj.transform.localScale = new Vector3(50, 50, 1);
                    if (i == 10)
                    {
                        obj.transform.localScale = new Vector3(10f, 10f, 1);
                    }

                    obj.transform.localPosition = new Vector3(x + 150 * i, (y + (2f * i)), z + (0.2f * i));
                    //增加一个进场的动画(暂用)
                    //ADDUIBattle.instance.MonsterEnterBttleAni(obj.transform, new Vector3(x + 50 * i, (y + (2f * i)), z + (0.2f * i)), 0.5f);
                    ADDUIBattle.instance.MonsterEnterBttleAni(obj.transform, new Vector3(x, y, z), 0.5f);
                }
                else
                {
                    Debug.Log("字典中没有相应的key!");
                }
            }

            MonsterSpineAniManager.Instance.SendMsg(ButtonMsg.GetInstance.ChangeInfo((ushort) MonsterSpineAniId.idle));
        }

        void RefreshSkillCD(RoleConfig rolecon)
        {
            Skills skills;
            if (ReadJsonfiles.Instance.skillDics.ContainsKey(rolecon.m_RoleInfo.skillid1))
            {
                ReadJsonfiles.Instance.skillDics.TryGetValue(rolecon.m_RoleInfo.skillid1, out skills);
                rolecon.m_RoleInfo.skillCd1 = skills.skillcd;
                // rolecon.m_RoleInfo.skillCd1Count = skills.skillcd;   
                // rolecon.m_RoleInfo.skillCd2Count = skills.skillcd;   
                // rolecon.m_RoleInfo.skillCd3Count = skills.skillcd;   
            }

            if (ReadJsonfiles.Instance.skillDics.ContainsKey(rolecon.m_RoleInfo.skillid2))
            {
                ReadJsonfiles.Instance.skillDics.TryGetValue(rolecon.m_RoleInfo.skillid2, out skills);
                rolecon.m_RoleInfo.skillCd2 = skills.skillcd;
            }

            if (ReadJsonfiles.Instance.skillDics.ContainsKey(rolecon.m_RoleInfo.skillid3))
            {
                ReadJsonfiles.Instance.skillDics.TryGetValue(rolecon.m_RoleInfo.skillid3, out skills);
                rolecon.m_RoleInfo.skillCd3 = skills.skillcd;
            }
        }

        IEnumerator MoveToNormalStation(GameObject obj, int i)
        {
            yield return new WaitForSeconds(0.6f);
            if (i == 0)
                obj.transform.localPosition = new Vector3(1300, 80, 0);
            if (i == 1)
                obj.transform.localPosition = new Vector3(1600, 80, 250);
            if (i == 2)
                obj.transform.localPosition = new Vector3(1400, 10, 0);
            if (i == 3)
                obj.transform.localPosition = new Vector3(1600, 30, 0);
        }

        /// <summary>
        /// 处理文本数据
        /// </summary>
        /// <param name="str"></param>
    }
}