using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ManagerID
{
    GameManager = 0,
    UIManager = FrameTools.msgSpawn,
    AudioManager = FrameTools.msgSpawn * 2,
    NPCManager = FrameTools.msgSpawn * 3,
    CharactorManager = FrameTools.msgSpawn * 4,
    AssetsManager = FrameTools.msgSpawn * 5,
    NetManager = FrameTools.msgSpawn * 6,
    RoleSpineAniManager = FrameTools.msgSpawn * 7,
    MonsterSpineAniManager = FrameTools.msgSpawn * 8

}
public class FrameTools {
    public const int msgSpawn = 3000;
}
