using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;
public enum RoleSpineAniId {
    idle = ManagerID.RoleSpineAniManager + 1,
    run,
    action,
    attack,
    attacked,
    dizziness,
    jumpforward,
    jumpback
}

public enum RoleId {
    Role1 = 10001,
    Role2,
    Role3,
    Role4,
    Role5
}

public class RoleSpineAni : MsgBase {
    public string name;
    //public ushort id;
    public string des;

    public RoleSpineAni () { }
    public void ChangeInfo (string tmp_name, string tmp_des, ushort tmp_id) {
        this.name = tmp_name;
        this.des = tmp_des;
        //this.id = tmp_id;
        msgID = tmp_id; //最重要
    }

}

public class RoleSpineAniMsgBack : MsgBase {
    public string name;
    public ushort id;
    public string des;
}

public class RoleSpineAniMsg : RoleSpineAniBase {

    private string roleStateInfo = "";

    private void Awake () {
        msgIDs = new ushort[] {
            (ushort) RoleSpineAniId.idle,
            (ushort) RoleSpineAniId.run,
            (ushort) RoleSpineAniId.action,
            (ushort) RoleSpineAniId.attack,
            (ushort) RoleSpineAniId.attacked,
            (ushort) RoleSpineAniId.dizziness,
            (ushort) RoleSpineAniId.jumpback,
            (ushort) RoleSpineAniId.jumpforward,
        };
        RegistSelf (this, msgIDs);
    }

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {

    }

    public override void ProcessEvent (MsgBase tmpMsg) {
        switch (tmpMsg.msgID) {
            case (ushort) RoleSpineAniId.idle:
                ShowAni (tmpMsg.msgID, tmpMsg.attackName, tmpMsg.allAction, tmpMsg.roleID, isLoop : tmpMsg.isLoop);
                // ShowAni (tmpMsg.msgID);
                // ShowAni (tmpMsg.msgID,isLoop:tmpMsg.isLoop);
                Debug.Log ("Role待机动画");
                break;
            case (ushort) RoleSpineAniId.run:
                ShowAni (tmpMsg.msgID, tmpMsg.attackName, tmpMsg.allAction, tmpMsg.roleID, tmpMsg.isLoop);
                Debug.Log ("Role跑步动画");
                break;
            case (ushort) RoleSpineAniId.action:
                ShowAni (tmpMsg.msgID, "", false, tmpMsg.roleID);
                // ShowAni (tmpMsg.msgID);
                Debug.Log ("Role行动动画");
                break;
            case (ushort) RoleSpineAniId.attack:
                Debug.LogError ("攻击的技能是：--" + tmpMsg.attackName);
                ShowAni (tmpMsg.msgID, tmpMsg.attackName, false, tmpMsg.roleID);
                Debug.Log ("Role攻击动画");
                // StartCoroutine(BattleLogic.instance.te());
                break;
            case (ushort) RoleSpineAniId.attacked:
                ShowAni (tmpMsg.msgID, tmpMsg.attackName, tmpMsg.allAction, tmpMsg.roleID);
                Debug.Log ("Role受击动画");
                break;
            case (ushort) RoleSpineAniId.dizziness:
                ShowAni (tmpMsg.msgID, "", false, tmpMsg.roleID);
                Debug.Log ("Role眩晕动画");
                break;
            case (ushort) RoleSpineAniId.jumpforward:
                ShowAni (tmpMsg.msgID, "", false, tmpMsg.roleID);
                Debug.Log ("Role前跳动画");
                break;
            case (ushort) RoleSpineAniId.jumpback:
                ShowAni (tmpMsg.msgID, "", false, tmpMsg.roleID);
                Debug.Log ("Role后跳动画");
                break;
        }
    }

    void ShowAni (ushort msgID, string attackName = "skill1", bool allAction = true, int roleID = 1000, bool isLoop = false) {
        switch (msgID) {
            case (ushort) RoleSpineAniId.idle:
                roleStateInfo = "idle";
                roleStateInfo = "normal";
                roleStateInfo = attackName;
                break;
            case (ushort) RoleSpineAniId.run:
                roleStateInfo = "run";
                break;
            case (ushort) RoleSpineAniId.action:
                roleStateInfo = "start";
                break;
            case (ushort) RoleSpineAniId.attack:
                roleStateInfo = "attack";
                roleStateInfo = attackName;
                break;
            case (ushort) RoleSpineAniId.attacked:
                roleStateInfo = "hit";
                break;
            case (ushort) RoleSpineAniId.dizziness:
                roleStateInfo = "stun";
                break;
            case (ushort) RoleSpineAniId.jumpforward:
                roleStateInfo = "jumpforward";
                break;
            case (ushort) RoleSpineAniId.jumpback:
                roleStateInfo = "jumpback";
                break;

        }

        if (allAction) {
            // Debug.Log ("检查长度");
            for (int i = 0; i < CreatSkeleton.Instance.skeletonsBattleAni.Count; i++) {
                CreatSkeleton.Instance.skeletonsBattleAni[i].loop = isLoop;
                CreatSkeleton.Instance.skeletonsBattleAni[i].AnimationName = roleStateInfo;
            }
        } else {
            SkeletonAnimation tempSkeleton;

            if (CreatSkeleton.Instance.skeletonsBattleAniDic.ContainsKey (roleID)) {
                CreatSkeleton.Instance.skeletonsBattleAniDic.TryGetValue (roleID, out tempSkeleton);
                // tempSkeleton.loop = isLoop;
                // tempSkeleton.AnimationName = roleStateInfo;
                tempSkeleton.state.SetAnimation (0, roleStateInfo, isLoop);
                // tempSkeleton.AnimationState.Start += delegate { Attack_Start (); };
                switch (roleStateInfo) {
                    case "hit":
                        // BattleLogic.instance.animationDurationTime = tempSkeleton.state.Data.SkeletonData.Animations.Items[0].duration;
                        break;
                    case "jumpback":
                        BattleLogic.instance.animationDurationTime = tempSkeleton.state.Data.SkeletonData.Animations.Items[1].duration;
                        break;
                    case "jumpforward":
                        BattleLogic.instance.animationDurationTime = tempSkeleton.state.Data.SkeletonData.Animations.Items[2].duration;
                        break;
                    case "normal":
                        BattleLogic.instance.animationDurationTime = tempSkeleton.state.Data.SkeletonData.Animations.Items[3].duration;
                        tempSkeleton.loop = true;
                        break;
                    case "run":
                        BattleLogic.instance.animationDurationTime = tempSkeleton.state.Data.SkeletonData.Animations.Items[4].duration;
                        tempSkeleton.loop = true;
                        break;
                    case "skill1":
                        BattleLogic.instance.animationDurationTime = tempSkeleton.state.Data.SkeletonData.Animations.Items[5].duration;
                        break;
                    case "skill2":
                        BattleLogic.instance.animationDurationTime = tempSkeleton.state.Data.SkeletonData.Animations.Items[6].duration;
                        break;
                    case "skill3a":
                        BattleLogic.instance.animationDurationTime = tempSkeleton.state.Data.SkeletonData.Animations.Items[7].duration;
                        break;
                    case "skill3b":
                        BattleLogic.instance.animationDurationTime = tempSkeleton.state.Data.SkeletonData.Animations.Items[8].duration;
                        break;
                    case "skill3c":
                        BattleLogic.instance.animationDurationTime = tempSkeleton.state.Data.SkeletonData.Animations.Items[9].duration;
                        break;
                    case "start":
                        BattleLogic.instance.animationDurationTime = tempSkeleton.state.Data.SkeletonData.Animations.Items[10].duration;
                        break;
                    case "stun":
                        BattleLogic.instance.animationDurationTime = tempSkeleton.state.Data.SkeletonData.Animations.Items[11].duration;
                        break;
                }

                //BattleLogic.instance.Animation_Complete(tempSkeleton);
                //tempSkeleton.AnimationState.Complete += AnimationState_Complete;
            }
        }

        //BattleLogic.intance.isSpineEnd = true;

    }

    void Attack_Start () {
       
    }
    private void AnimationState_Complete (Spine.TrackEntry trackEntry) {
        throw new System.NotImplementedException ();
    }
}