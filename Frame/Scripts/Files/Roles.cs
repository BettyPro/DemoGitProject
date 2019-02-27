using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SerializeField]
public class Roles
{
    public int roleid; //角色ID
    public string nickname;//昵称
    public string attribute; //属性
    public string profession; //职业
    public string constellation; //星座
    public int starlevel; //星级
    public int numberawake; //觉醒次数
    public int degree; //亲密度
    public string degreedes; //亲密度描述
    public string rank; //等级
    public string intro;//简介,一句话描述
    public string fight;//战斗力
    public float attack;//攻击力
    public float defense;//防御
    public float life;//生命值
    public float speed;//速度
    public string critchance;//暴击概率
    public string critdamage;//暴击伤害
    public string effecthit;//效果命中
    public string effectresist;//效果抵抗
    public string attackrate;//连携攻击率
    public int skillid1;//技能1ID
    public int skillid2;//技能2ID
    public int skillid3;//技能3D
    public List<string[]>  infos;

    public override string ToString()
    {
        return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23}", 
            roleid, nickname, attribute, profession, constellation, starlevel, numberawake, degree, degreedes, rank, intro, fight, attack,
            defense, life, speed, critchance, critdamage,effecthit, effectresist, attackrate, skillid1, skillid2, skillid3);
    }
}
