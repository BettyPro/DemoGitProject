using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class Buff
{
    public int effectId;//效果ID
    public int target;//目标
    public string effectType;//效果类型
    public int value1;//数值1
    public int value2;//数值2
    public int continueBout;//持续回合
    public string onsetProb;//发动概率

    public override string ToString()
    {
        return string.Format("_效果ID:{0},_目标:{1},_效果类型:{2},__数值1:{3},_数值2:{4},_持续回合:{5},_发动概率:{6}", effectId,
            target, effectType, value1, value2, continueBout, onsetProb);
    }
}
