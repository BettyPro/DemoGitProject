using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class Skills {

    public int skillid;//技能ID
    public string skillname;//技能名称
    public int releaseobj;//释放对象
    public int skilladdnum;//技能加魂数量
    public int skillcd;//技能CD
    public int isattacknor;//是否普攻 
    public int isnpcskill;//是否NPC技能
    public int ispassive;//是否被动
    public string skilldes;//技能描述
    public string strengthen1;//技能强化+1
    public string strengthen2;//技能强化+2
    public string strengthen3;//技能强化+3
    public string strengthen4;//技能强化+4
    public string effectid1;//效果1ID
    public string effectid2;//效果2ID
    public string effectid3;//效果3ID
    public string effectid4;//效果4ID
    public string des;//开发描述

    
    public int skillid1;//技能1ID
    public int skillid2;//技能2ID
    public int skillid3;//技能3D

    public override string ToString()
    {
        return string.Format("{0},技能名称：_技能ID:{1},_技能名称:{2},_技能加魂数:{3},_技能CD:{4},_是否NPC技能:{5},_是否被动:{6},_技能描述:{7},_强化1:{8},_强化2:{9},_强化3:{10},_强化4:{11},_效果1:{12},_效果2:{13},_效果3:{14},_效果4:{15},_描述:{16}", skillid,skillname, skilladdnum, skillcd, isattacknor, isnpcskill, ispassive, skilldes,
            strengthen1, strengthen2, strengthen3, strengthen4,effectid1,effectid2,effectid3,effectid4,des);
    }
}
