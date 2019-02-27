using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audios
{

    public int musicnNum;
    public string musicPath;
    public string musicDes;

    public override string ToString()
    {
        //return string.Format("音乐编号:--{0},音乐路径:--{1},音乐描述:--{2}",musicnNum,musicPath,musicDes);
        return string.Format("{0},{1},{2}",musicnNum,musicPath,musicDes);


    }

}
