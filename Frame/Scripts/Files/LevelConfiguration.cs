using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SerializeField]
public class LevelConfiguration : MonoBehaviour
{

    public int NPCID1;
    public int NPCID2;
    public int NPCID3;
    public int NPCID4;

    public override string ToString()
    {
        return string.Format("{0},{1},{2},{3}",NPCID1,NPCID2,NPCID3,NPCID4);
    }
}
