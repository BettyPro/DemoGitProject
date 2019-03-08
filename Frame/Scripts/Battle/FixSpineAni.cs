using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class FixSpineAni : Singleton<FixSpineAni> {

    /// <summary>
    /// fix mix
    /// </summary>
    public void FixAniMix (SkeletonAnimation attacker, SkeletonAnimation target) {
        //    attacker.state.MixDuration
        //    attacker.AnimationState.GetCurrent(0).
    }
    /// <summary>
    /// time
    /// </summary>
    public void FixSpineAniFormTime (Skills usingSkill, RoleConfig target_roleIns, string recordSkillName) {

        if (usingSkill.releaseobj == (int) TargetType.singleEnemy) {

            // SkillEffect.Instance.attack_roleInsAni.AnimationState.Start += delegate {
            //     MyTimerTool.Instance.wtime.AddFrameTask ((int tid) => {
            //         Debug.Log (SkillEffect.Instance.attack_roleInsAni.state.GetCurrent (0).AnimationTime + "=====================");
            //         Debug.Log (SkillEffect.Instance.attack_roleInsAni.AnimationName + "=====================");
            //         RoleSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) MonsterSpineAniId.attacked, target_roleIns.iddif, "hit", false, false));
            //         Debug.Log ("1-------------------------------1");
            //     }, 7, 1);
            //     return;
            // };

            MyTimerTool.Instance.wtime.AddFrameTask ((int tid) => {
                Debug.Log (SkillEffect.Instance.attack_roleInsAni.state.GetCurrent (0).AnimationTime + "=====================");
                Debug.Log (SkillEffect.Instance.attack_roleInsAni.AnimationName + "=====================");
                RoleSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) MonsterSpineAniId.attacked, target_roleIns.iddif, "hit", false, false));
                Debug.Log ("1-------------------------------1");
            }, 14, 1);
            MyTimerTool.Instance.wtime.AddTimeTask ((int tid) => {
                Debug.Log (SkillEffect.Instance.attack_roleInsAni.state.GetCurrent (0).AnimationTime + "=====================");
                Debug.Log (SkillEffect.Instance.attack_roleInsAni.AnimationName + "=====================");
                RoleSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) MonsterSpineAniId.attacked, target_roleIns.iddif, "hit", false, false));
                Debug.Log ("1-------------------------------1");
            }, 0.23324, WinterTimeUnit.Second, 1);
            MyTimerTool.Instance.wtime.AddFrameTask ((int tid) => {
                RoleSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) MonsterSpineAniId.attacked, target_roleIns.iddif, "hit", false, false));
                Debug.Log ("1-------------------------------2");

            }, 15, 1);
            MyTimerTool.Instance.wtime.AddFrameTask ((int tid) => {
                RoleSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) MonsterSpineAniId.attacked, target_roleIns.iddif, "hit", false, false));
                Debug.Log ("1-------------------------------3");

            }, 27, 1);

        } else if (usingSkill.releaseobj == (int) TargetType.allEnemy) {

            MyTimerTool.Instance.wtime.AddFrameTask ((int tid) => {
                RoleSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) MonsterSpineAniId.attacked, target_roleIns.iddif, "hit", true, false));
                Debug.Log ("1-------------------------------1");
            }, 7, 1);
            MyTimerTool.Instance.wtime.AddFrameTask ((int tid) => {
                RoleSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) MonsterSpineAniId.attacked, target_roleIns.iddif, "hit", true, false));
                Debug.Log ("1-------------------------------2");

            }, 15, 1);
            MyTimerTool.Instance.wtime.AddFrameTask ((int tid) => {
                RoleSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) MonsterSpineAniId.attacked, target_roleIns.iddif, "hit", true, false));
                Debug.Log ("1-------------------------------3");

            }, 27, 1);
        }
    }

    public void FixAniMonsToRoleTime (RoleConfig target_roleIns) {
        MyTimerTool.Instance.wtime.AddTimeTask ((int tid) => {
            Debug.Log (SkillEffect.Instance.attack_roleInsAni.state.GetCurrent (0).AnimationTime + "=====================");
            Debug.Log (SkillEffect.Instance.attack_roleInsAni.AnimationName + "=====================");
            RoleSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) MonsterSpineAniId.attacked, target_roleIns.iddif, "hit", false, false));
            Debug.Log ("1-------------------------------1");
        }, 0.23324, WinterTimeUnit.Second, 1);
    }

    public void FixAniRoleToMonsTime (RoleConfig target_roleIns) {
        MyTimerTool.Instance.wtime.AddTimeTask ((int tid) => {
            Debug.Log (SkillEffect.Instance.attack_roleInsAni.state.GetCurrent (0).AnimationTime + "=====================");
            Debug.Log (SkillEffect.Instance.attack_roleInsAni.AnimationName + "=====================");
            RoleSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) MonsterSpineAniId.attacked, target_roleIns.iddif, "hit", false, false));
            Debug.Log ("1-------------------------------1");
        }, 0.23324, WinterTimeUnit.Second, 1);
    }

    /// <summary>
    /// frame
    /// </summary>
    public void FixSpineAniFormFrame (Skills usingSkill, RoleConfig target_roleIns, string recordSkillName) {

        if (usingSkill.releaseobj == (int) TargetType.singleEnemy) {

            switch (recordSkillName) {
                case "skill1":
                    MyTimerTool.Instance.wtime.AddFrameTask ((int tid) => {
                        RoleSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) MonsterSpineAniId.attacked, target_roleIns.iddif, "hit", false, false));
                    }, 7, 1);
                    break;
                case "skill2":
                    MyTimerTool.Instance.wtime.AddFrameTask ((int tid) => {
                        RoleSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) MonsterSpineAniId.attacked, target_roleIns.iddif, "hit", false, false));
                    }, 7, 1);
                    MyTimerTool.Instance.wtime.AddFrameTask ((int tid) => {
                        RoleSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) MonsterSpineAniId.attacked, target_roleIns.iddif, "hit", false, false));
                    }, 15, 1);
                    MyTimerTool.Instance.wtime.AddFrameTask ((int tid) => {
                        RoleSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) MonsterSpineAniId.attacked, target_roleIns.iddif, "hit", false, false));
                    }, 27, 1);
                    break;
                case "skill3c":
                    MyTimerTool.Instance.wtime.AddFrameTask ((int tid) => {
                        RoleSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) MonsterSpineAniId.attacked, target_roleIns.iddif, "hit", false, false));
                    }, 27, 1);
                    MyTimerTool.Instance.wtime.AddFrameTask ((int tid) => {
                        RoleSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) MonsterSpineAniId.attacked, target_roleIns.iddif, "hit", false, false));
                    }, 31, 1);
                    MyTimerTool.Instance.wtime.AddFrameTask ((int tid) => {
                        RoleSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) MonsterSpineAniId.attacked, target_roleIns.iddif, "hit", false, false));
                    }, 35, 1);
                    MyTimerTool.Instance.wtime.AddFrameTask ((int tid) => {
                        RoleSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) MonsterSpineAniId.attacked, target_roleIns.iddif, "hit", false, false));
                    }, 39, 1);
                    MyTimerTool.Instance.wtime.AddFrameTask ((int tid) => {
                        RoleSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) MonsterSpineAniId.attacked, target_roleIns.iddif, "hit", false, false));
                    }, 43, 1);
                    break;
                default:
                    Debug.Log("null");
                    break;
                
            }

        } else if (usingSkill.releaseobj == (int) TargetType.allEnemy) {

            MyTimerTool.Instance.wtime.AddFrameTask ((int tid) => {
                RoleSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) MonsterSpineAniId.attacked, target_roleIns.iddif, "hit", true, false));
            }, 7, 1);
            MyTimerTool.Instance.wtime.AddFrameTask ((int tid) => {
                RoleSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) MonsterSpineAniId.attacked, target_roleIns.iddif, "hit", true, false));

            }, 15, 1);
            MyTimerTool.Instance.wtime.AddFrameTask ((int tid) => {
                RoleSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) MonsterSpineAniId.attacked, target_roleIns.iddif, "hit", true, false));
            }, 27, 1);
        }
    }

    public void FixAniMonsToRoleFrame (RoleConfig target_roleIns, int delayFrame = 2) {
        MyTimerTool.Instance.wtime.AddFrameTask ((int tid) => {
            MonsterSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) RoleSpineAniId.attack, target_roleIns.iddif, "hit", false, false));
        }, delayFrame, 1);
    }

    public void FixAniRoleToMonsFrame (RoleConfig target_roleIns, int delayFrame = 2) {
        MyTimerTool.Instance.wtime.AddFrameTask ((int tid) => {
            RoleSpineAniManager.Instance.SendMsg (ButtonMsg.GetInstance.ChangeInfo ((ushort) MonsterSpineAniId.attacked, target_roleIns.iddif, "hit", false, false));
        }, delayFrame, 1);
    }

}