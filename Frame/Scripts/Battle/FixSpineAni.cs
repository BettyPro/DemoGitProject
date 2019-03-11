using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

namespace Demo
{
    public class FixSpineAni : Singleton<FixSpineAni>
    {
        /// <summary>
        /// fix mix
        /// </summary>
        public void FixAniMix(SkeletonAnimation attacker, SkeletonAnimation target)
        {
            //    attacker.state.MixDuration
            //    attacker.AnimationState.GetCurrent(0).
        }

        /// <summary>
        /// time
        /// </summary>
        public void FixSpineAniFormTime(Skills usingSkill, RoleConfig target_roleIns, string recordSkillName)
        {
            if (usingSkill.releaseobj == (int) TargetType.singleEnemy)
            {
                // SkillEffect.Instance.attack_roleInsAni.AnimationState.Start += delegate {
                //     MyTimerTool.Instance.wtime.AddFrameTask ((int tid) => {
                //         Debug.Log (SkillEffect.Instance.attack_roleInsAni.state.GetCurrent (0).AnimationTime + "=====================");
                //         Debug.Log (SkillEffect.Instance.attack_roleInsAni.AnimationName + "=====================");
                //         RoleSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) MonsterSpineAniId.attacked, target_roleIns.iddif, "hit2", false, false));
                //         Debug.Log ("1-------------------------------1");
                //     }, 7, 1);
                //     return;
                // };

                MyTimerTool.Instance.wtime.AddFrameTask((int tid) =>
                {
                    Debug.Log(SkillEffect.Instance.attack_roleInsAni.state.GetCurrent(0).AnimationTime +
                              "=====================");
                    Debug.Log(SkillEffect.Instance.attack_roleInsAni.AnimationName + "=====================");
                    RoleSpineAniManager.Instance.SendMsg(ButtonMsg.GetInstance.ChangeInfo(
                        (ushort) MonsterSpineAniId.attacked, target_roleIns.iddif, "hit2", false, false));
                    Debug.Log("1-------------------------------1");
                }, 14, 1);
                MyTimerTool.Instance.wtime.AddTimeTask((int tid) =>
                {
                    Debug.Log(SkillEffect.Instance.attack_roleInsAni.state.GetCurrent(0).AnimationTime +
                              "=====================");
                    Debug.Log(SkillEffect.Instance.attack_roleInsAni.AnimationName + "=====================");
                    RoleSpineAniManager.Instance.SendMsg(ButtonMsg.GetInstance.ChangeInfo(
                        (ushort) MonsterSpineAniId.attacked, target_roleIns.iddif, "hit2", false, false));
                    Debug.Log("1-------------------------------1");
                }, 0.23324, WinterTimeUnit.Second, 1);
                MyTimerTool.Instance.wtime.AddFrameTask((int tid) =>
                {
                    RoleSpineAniManager.Instance.SendMsg(ButtonMsg.GetInstance.ChangeInfo(
                        (ushort) MonsterSpineAniId.attacked, target_roleIns.iddif, "hit2", false, false));
                    Debug.Log("1-------------------------------2");
                }, 15, 1);
                MyTimerTool.Instance.wtime.AddFrameTask((int tid) =>
                {
                    RoleSpineAniManager.Instance.SendMsg(ButtonMsg.GetInstance.ChangeInfo(
                        (ushort) MonsterSpineAniId.attacked, target_roleIns.iddif, "hit2", false, false));
                    Debug.Log("1-------------------------------3");
                }, 27, 1);
            }
            else if (usingSkill.releaseobj == (int) TargetType.allEnemy)
            {
                MyTimerTool.Instance.wtime.AddFrameTask((int tid) =>
                {
                    RoleSpineAniManager.Instance.SendMsg(ButtonMsg.GetInstance.ChangeInfo(
                        (ushort) MonsterSpineAniId.attacked, target_roleIns.iddif, "hit2", true, false));
                    Debug.Log("1-------------------------------1");
                }, 7, 1);
                MyTimerTool.Instance.wtime.AddFrameTask((int tid) =>
                {
                    RoleSpineAniManager.Instance.SendMsg(ButtonMsg.GetInstance.ChangeInfo(
                        (ushort) MonsterSpineAniId.attacked, target_roleIns.iddif, "hit2", true, false));
                    Debug.Log("1-------------------------------2");
                }, 15, 1);
                MyTimerTool.Instance.wtime.AddFrameTask((int tid) =>
                {
                    RoleSpineAniManager.Instance.SendMsg(ButtonMsg.GetInstance.ChangeInfo(
                        (ushort) MonsterSpineAniId.attacked, target_roleIns.iddif, "hit2", true, false));
                    Debug.Log("1-------------------------------3");
                }, 27, 1);
            }
        }

        public void FixAniMonsToRoleTime(RoleConfig target_roleIns)
        {
            MyTimerTool.Instance.wtime.AddTimeTask((int tid) =>
            {
                Debug.Log(
                    SkillEffect.Instance.attack_roleInsAni.state.GetCurrent(0).AnimationTime + "=====================");
                Debug.Log(SkillEffect.Instance.attack_roleInsAni.AnimationName + "=====================");
                RoleSpineAniManager.Instance.SendMsg(ButtonMsg.GetInstance.ChangeInfo(
                    (ushort) MonsterSpineAniId.attacked,
                    target_roleIns.iddif, "hit2", false, false));
                Debug.Log("1-------------------------------1");
            }, 0.23324, WinterTimeUnit.Second, 1);
        }

        public void FixAniRoleToMonsTime(RoleConfig target_roleIns)
        {
            MyTimerTool.Instance.wtime.AddTimeTask((int tid) =>
            {
                Debug.Log(
                    SkillEffect.Instance.attack_roleInsAni.state.GetCurrent(0).AnimationTime + "=====================");
                Debug.Log(SkillEffect.Instance.attack_roleInsAni.AnimationName + "=====================");
                RoleSpineAniManager.Instance.SendMsg(ButtonMsg.GetInstance.ChangeInfo(
                    (ushort) MonsterSpineAniId.attacked,
                    target_roleIns.iddif, "hit2", false, false));
                Debug.Log("1-------------------------------1");
            }, 0.23324, WinterTimeUnit.Second, 1);
        }

        /// <summary>
        /// frame
        /// </summary>
        public void FixSpineAniFormFrame(Skills usingSkill, RoleConfig target_roleIns, string recordSkillName,
            bool attackIsRole = true)
        {
            if (usingSkill.releaseobj == (int) TargetType.singleEnemy)
            {
                JudgeAttackedFromSkill(target_roleIns, recordSkillName, attackIsRole);
            }
            else if (usingSkill.releaseobj == (int) TargetType.allEnemy)
            {
                JudgeAttackedFromSkill(target_roleIns, recordSkillName, attackIsRole, true);
            }
        }

        void JudgeAttackedFromSkill(RoleConfig target_roleIns, string recordSkillName, bool attackIsRole = true,
            bool isAllAction = false)
        {
            switch (recordSkillName)
            {
                case "skill1":
                    MyTimerTool.Instance.wtime.AddFrameTask(
                        (int tid) => { SendMesgToDiffTarget(target_roleIns, attackIsRole, isAllAction); }, 7, 1);
                    break;
                case "skill2":
                    MyTimerTool.Instance.wtime.AddFrameTask(
                        (int tid) => { SendMesgToDiffTarget(target_roleIns, attackIsRole, isAllAction); }, 7, 1);
                    MyTimerTool.Instance.wtime.AddFrameTask(
                        (int tid) => { SendMesgToDiffTarget(target_roleIns, attackIsRole, isAllAction); }, 15, 1);
                    MyTimerTool.Instance.wtime.AddFrameTask(
                        (int tid) => { SendMesgToDiffTarget(target_roleIns, attackIsRole, isAllAction); }, 27, 1);
                    break;
                case "skill3a":
                    MyTimerTool.Instance.wtime.AddFrameTask(
                        (int tid) => { SendMesgToDiffTarget(target_roleIns, attackIsRole, isAllAction); }, 7, 1);
                    MyTimerTool.Instance.wtime.AddFrameTask(
                        (int tid) => { SendMesgToDiffTarget(target_roleIns, attackIsRole, isAllAction); }, 11, 1);
                    MyTimerTool.Instance.wtime.AddFrameTask(
                        (int tid) => { SendMesgToDiffTarget(target_roleIns, attackIsRole, isAllAction); }, 15, 1);
                    MyTimerTool.Instance.wtime.AddFrameTask(
                        (int tid) => { SendMesgToDiffTarget(target_roleIns, attackIsRole, isAllAction); }, 19, 1);
                    MyTimerTool.Instance.wtime.AddFrameTask(
                        (int tid) => { SendMesgToDiffTarget(target_roleIns, attackIsRole, isAllAction); }, 23, 1);
                    break;
                default:
                    Debug.Log("null");
                    break;
            }
        }

        void SendMesgToDiffTarget(RoleConfig target_roleIns, bool attackIsRole = true, bool isAllAction = false)
        {
            if (attackIsRole)
            {
                RoleSpineAniManager.Instance.SendMsg(ButtonMsg.GetInstance.ChangeInfo(
                    (ushort) MonsterSpineAniId.attacked, target_roleIns.iddif, "hit2", isAllAction, false));
            }
            else
            {
                MonsterSpineAniManager.Instance.SendMsg(ButtonMsg.GetInstance.ChangeInfo(
                    (ushort) RoleSpineAniId.attacked,
                    target_roleIns.id, "hit2", isAllAction, false));
            }
        }

        public void FixAniMonsToRoleFrame(RoleConfig target_roleIns, int delayFrame = 2)
        {
            MyTimerTool.Instance.wtime.AddFrameTask(
                (int tid) =>
                {
                    MonsterSpineAniManager.Instance.SendMsg(ButtonMsg.GetInstance.ChangeInfo(
                        (ushort) RoleSpineAniId.attack,
                        target_roleIns.iddif, "hit2", false, false));
                }, delayFrame, 1);
        }

        public void FixAniRoleToMonsFrame(RoleConfig target_roleIns, int delayFrame = 2)
        {
            MyTimerTool.Instance.wtime.AddFrameTask(
                (int tid) =>
                {
                    RoleSpineAniManager.Instance.SendMsg(ButtonMsg.GetInstance.ChangeInfo(
                        (ushort) MonsterSpineAniId.attacked, target_roleIns.iddif, "hit2", true, false));
                }, delayFrame, 1);
        }
    }
}