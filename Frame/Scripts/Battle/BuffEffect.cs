using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//根据技能表配置执行
public enum TargetType
{
    possive = 0,
    self = 5,
    singleFriendButSelf = 6,
    singleFriend = 3,
    allFriend = 4,
    singleEnemy = 1,
    allEnemy = 2
}

//根据Buff(映射)表配置执行
public enum TargetSkillType
{
    self = 1,
    singleFriend,
    allFriend,
    singleEnemy,
    allEnemy
}

public enum EffectType
{
    one = 1,
    two, three, four, five, six, seven, eight, nine, ten,
    eleven, twelve, thirteen, fourteen, fifteen, sixteen,
    seventeen, eightteen, nineteen, twenty, twentyone,
    twentytwo, twentythree, twentyfour,twentyfive
}

public enum BuffEffectEnu
{
    attackAdd,
    attackReduce,
    defenceAdd,
    defenceReduce,
    speedAdd,
    speedReduce,
    dilence,//沉默
    dizziness,//眩晕
    magicNail,//魔法钉
    forceAttack//嘲讽
}

public class BuffEffect : Buff {

    


    //self




    //friend
    //friend -- #single



    //friend -- #all




    //enemy
    //enemy -- #single


    //enemy -- #all



    //passive




}
