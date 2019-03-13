using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;
using WinterTools;

namespace Demo
{
    public class FixDistance : Singleton<FixDistance>
    {

        /// <summary>
        /// time
        /// </summary>
        public void FixFromTime(SkeletonAnimation attacker, Vector3 attackerOldPos, float needTime,
            float delayTime = 0.2f)
        {
            MyTimerTool.Instance.wtime.AddTimeTask((int tid) =>
            {
                DOTween.To(() => attacker.transform.position, a => attacker.transform.position = a,
                    attackerOldPos, needTime);
            }, delayTime, WinterTimeUnit.Second, 1);
        }

        public void FixFromTimeOne(bool attackIsRole, SkeletonAnimation attacker, Vector3 attackerOldPos,
            float needTime, float delayTime = 0.2f)
        {
            MyTimerTool.Instance.wtime.AddTimeTask((int tid) =>
            {
                DOTween.To(() => attacker.transform.position, a => attacker.transform.position = a,
                    attackerOldPos, needTime);
            }, delayTime, WinterTimeUnit.Second, 1);

        }

        /// <summary>
        /// frame
        /// </summary>
        public void FixDistanceFromFrame(SkeletonAnimation attacker, Vector3 attackerOldPos, float needTime, int delayFrame = 2)
        {
            MyTimerTool.Instance.wtime.AddFrameTask((int tid) =>
            {
                DOTween.To(() => attacker.transform.position, a => attacker.transform.position = a,
                    attackerOldPos, needTime);
            }, delayFrame, 1);
        }

        public void FixFromFrameOne(string recordSkillName, RoleConfig roleconWhole, SkeletonAnimation attacker,
            Vector3 attackerOldPos, float needTime, int delayFrame = 2)
        {
            MyTimerTool.Instance.wtime.AddFrameTask((int tid) =>
            {
                DOTween.To(() => attacker.transform.position, a => attacker.transform.position = a,
                    attackerOldPos, needTime);
                if (recordSkillName == "skill3a")
                {
                    //todo 改变二技能释放位置
                    SkillEffect.Instance.target_roleIns.transform.position -= new Vector3(-10, 0, 0);

                    RoleSpineAniManager.Instance.SendMsg(ButtonMsg.GetInstance.ChangeInfo(
                        (ushort) RoleSpineAniId.attack, (ushort) roleconWhole.m_RoleInfo.roleid, "skill3b"));
                }
            }, delayFrame, 1);
        }
    }
}