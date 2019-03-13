using Spine.Unity;
using UnityEngine;
using WinterTools;

namespace Demo
{


    public enum MonsterSpineAniId
    {
        idle = ManagerID.MonsterSpineAniManager + 1,
        run,
        action,
        attack,
        attacked,
        dizziness,
        jumpforward,
        jumpback
    }

    public enum MonsterId
    {
        NPC1 = 20001,
        NPC2,
        NPC3,
        NPC4

    }

    public class MonsterSpineAniMsgBack : MsgBase
    {
        public string name;
        public ushort id;
        public string des;
    }

    public class MonsterSpineAniMsg : MonsterSpineAniBase
    {
        private string monsterStateInfo = "";

        private void Awake()
        {
            msgIDs = new ushort[]
            {
                (ushort) MonsterSpineAniId.idle,
                (ushort) MonsterSpineAniId.run,
                (ushort) MonsterSpineAniId.action,
                (ushort) MonsterSpineAniId.attack,
                (ushort) MonsterSpineAniId.attacked,
                (ushort) MonsterSpineAniId.dizziness,
                (ushort) MonsterSpineAniId.jumpback,
                (ushort) MonsterSpineAniId.jumpforward,
            };
            RegistSelf(this, msgIDs);
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void ProcessEvent(MsgBase tmpMsg)
        {
            switch (tmpMsg.msgID)
            {
                case (ushort) MonsterSpineAniId.idle:
                    ShowAni(tmpMsg.msgID, "", tmpMsg.allAction, tmpMsg.roleID, isLoop: tmpMsg.isLoop);
                    // ShowAni (tmpMsg.msgID);
                    Debug.Log(tmpMsg.roleID);
                    Debug.Log("Monster待机动画");
                    break;
                case (ushort) MonsterSpineAniId.run:
                    ShowAni(tmpMsg.msgID);
                    Debug.Log("Monster跑步动画");
                    break;
                case (ushort) MonsterSpineAniId.action:
                    ShowAni(tmpMsg.msgID, "", false, tmpMsg.roleID);
                    Debug.Log("Monster行动动画");
                    break;
                case (ushort) MonsterSpineAniId.attack:
                    Debug.LogError("怪物的攻击技能是：" + tmpMsg.attackName);
                    ShowAni(tmpMsg.msgID, tmpMsg.attackName, false, tmpMsg.roleID);
                    Debug.Log("Monster攻击动画");
                    break;
                case (ushort) MonsterSpineAniId.attacked:
                    ShowAni(tmpMsg.msgID, tmpMsg.attackName, tmpMsg.allAction, tmpMsg.roleID, tmpMsg.isLoop);
                    Debug.Log("Monster受击动画");
                    break;
                case (ushort) MonsterSpineAniId.dizziness:
                    ShowAni(tmpMsg.msgID, "", false, tmpMsg.roleID, tmpMsg.isLoop);
                    Debug.Log("Monster眩晕动画");
                    break;
                case (ushort) MonsterSpineAniId.jumpforward:
                    ShowAni(tmpMsg.msgID, "", false, tmpMsg.roleID);
                    Debug.Log("Monster前跳动画");
                    break;
                case (ushort) MonsterSpineAniId.jumpback:
                    Debug.Log("播放后跳--------------------------------------");
                    ShowAni(tmpMsg.msgID, "", false, tmpMsg.roleID);
                    Debug.Log("Monster后跳动画");
                    break;
            }
        }

        void ShowAni(ushort msgID, string attackName = "skill1", bool allAction = true, int monsterID = 1000,
            bool isLoop = false)
        {
            switch (msgID)
            {
                case (ushort) MonsterSpineAniId.idle:
                    monsterStateInfo = "idle";
                    monsterStateInfo = "normal";
                    break;
                case (ushort) MonsterSpineAniId.run:
                    monsterStateInfo = "run";
                    break;
                case (ushort) MonsterSpineAniId.action:
                    monsterStateInfo = "start";
                    break;
                case (ushort) MonsterSpineAniId.attack:
                    monsterStateInfo = "attack";
                    monsterStateInfo = attackName;
                    break;
                case (ushort) MonsterSpineAniId.attacked:
                    monsterStateInfo = "hit";
                    monsterStateInfo = attackName;
                    break;
                case (ushort) MonsterSpineAniId.dizziness:
                    monsterStateInfo = "stun";
                    break;
                case (ushort) MonsterSpineAniId.jumpforward:
                    monsterStateInfo = "jumpforward";
                    break;
                case (ushort) MonsterSpineAniId.jumpback:
                    monsterStateInfo = "jumpback";
                    break;
            }

            if (allAction)
            {
                for (int i = 0; i < CreatSkeleton.Instance.skeletonsMonsterBattleAni.Count; i++)
                {
                    // CreatSkeleton.Instance.skeletonsMonsterBattleAni[i].loop = isLoop;
                    // CreatSkeleton.Instance.skeletonsMonsterBattleAni[i].AnimationName = monsterStateInfo;
                    CreatSkeleton.Instance.skeletonsMonsterBattleAni[i].state.SetAnimation(0, monsterStateInfo, isLoop);
                }

                Debug.Log("单个的ID:-- " + monsterID);
                Debug.Log("all个的action:-- " + monsterStateInfo);
            }
            else
            {
                SkeletonAnimation tempSkeleton;

                ColorDebug.Instance.DicDebug<int, SkeletonAnimation>(
                    CreatSkeleton.Instance.skeletonsMonsterBattleAniDic, false);
                if (CreatSkeleton.Instance.skeletonsMonsterBattleAniDic.ContainsKey(monsterID))
                {
                    CreatSkeleton.Instance.skeletonsMonsterBattleAniDic.TryGetValue(monsterID, out tempSkeleton);
                    // tempSkeleton.loop = isLoop;
                    // tempSkeleton.AnimationName = monsterStateInfo;
                    tempSkeleton.state.SetAnimation(0, monsterStateInfo, isLoop);
                    Debug.Log("zhege 个的action:-- " + monsterStateInfo);

                    switch (monsterStateInfo)
                    {
                        case "hit":
                            BattleLogic.instance.animationDurationTime =
                                tempSkeleton.state.Data.SkeletonData.Animations.Items[0].duration;
                            break;
                        case "jumpback":
                            BattleLogic.instance.animationDurationTime =
                                tempSkeleton.state.Data.SkeletonData.Animations.Items[1].duration;
                            break;
                        case "jumpforward":
                            BattleLogic.instance.animationDurationTime =
                                tempSkeleton.state.Data.SkeletonData.Animations.Items[2].duration;
                            break;
                        case "normal":
                            BattleLogic.instance.animationDurationTime =
                                tempSkeleton.state.Data.SkeletonData.Animations.Items[3].duration;
                            tempSkeleton.loop = true;
                            break;
                        case "run":
                            BattleLogic.instance.animationDurationTime =
                                tempSkeleton.state.Data.SkeletonData.Animations.Items[4].duration;
                            tempSkeleton.loop = true;
                            break;
                        case "skill1":
                            BattleLogic.instance.animationDurationTime =
                                tempSkeleton.state.Data.SkeletonData.Animations.Items[5].duration;
                            break;
                        case "skill2":
                            BattleLogic.instance.animationDurationTime =
                                tempSkeleton.state.Data.SkeletonData.Animations.Items[6].duration;
                            break;
                        case "skill3a":
                            BattleLogic.instance.animationDurationTime =
                                tempSkeleton.state.Data.SkeletonData.Animations.Items[7].duration;
                            break;
                        case "skill3b":
                            BattleLogic.instance.animationDurationTime =
                                tempSkeleton.state.Data.SkeletonData.Animations.Items[8].duration;
                            break;
                        case "skill3c":
                            BattleLogic.instance.animationDurationTime =
                                tempSkeleton.state.Data.SkeletonData.Animations.Items[9].duration;
                            break;
                        case "start":
                            BattleLogic.instance.animationDurationTime =
                                tempSkeleton.state.Data.SkeletonData.Animations.Items[10].duration;
                            tempSkeleton.loop = false;
                            break;
                        case "stun":
                            tempSkeleton.loop = true;
                            BattleLogic.instance.animationDurationTime =
                                tempSkeleton.state.Data.SkeletonData.Animations.Items[11].duration;
                            break;
                    }

                    Debug.Log(monsterID);
                    Debug.Log(tempSkeleton.name);
                }
                else
                {
                    Debug.Log(monsterID);
                    Debug.Log("不包含");

                }
            }
        }
    }
}