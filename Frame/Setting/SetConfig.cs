using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using WinterTools;

public class SetConfig : Singleton<SetConfig>
{
    //Bool
    public bool IsDebug = true;
    public bool isSceneRole = false;
    public bool isSceneBattle = false;

    //Scene
    public const string sceneStartGame = "StartGame";

    public const string sceneRole = "Role";

    //public const string sceneRole = "RoleTest";
    public const string sceneBattle = "Battle";
    public const string sceneLogin = "Login";
    public const string sceneMain = "Main";

    //Resources Path
    public const string roleSceneSpineAniShow = "Prefabs/RoleAniPreShow"; //-Me-- Role && Battle界面角色展示
    public const string bloodShow = "Prefabs/blood"; //-Me-- Battle界面角色血条展示
    public const string damageValuesShow = "Prefabs/damageValuesShow"; //-Me-- Battle界面角色血条展示
    public const int damageValuesShowCount = 11; //-Me-- Battle界面角色血条展示

    public const string monsterSpineAniShow = "Prefabs/MonsterAniPreShow"; //-Me-- Battle界面怪物角色展示
    public const string monsterSpineAniShowTest = "Prefabs/MonsterAniPreShowTest"; //-Me-- Battle界面怪物角色展示
    public const string monsterSkeletonShow = "Prefabs/MonsterSkeletonDataAssets"; //-Me-- Battle界面怪物角色数据

    public const string roleSceneSpineAniShowUI = "Prefabs/RoleAniPreShowUI"; //-Me-- Role界面展示
    public const string roleSceneSpineAniShowUITest = "Prefabs/RoleAniPreShowUITest";
    public const string roleSceneRoleSingleShow = "Prefabs/rolepre"; //-Me-- Role界面展示 && Battle界面速度条

    public string saveInformationPath = Application.dataPath + "/Resources/saveInformation.txt";

    //Resources Path - SpineEffects
    public const string spineStrikeBack = "Assets/Resources/SpineEffects";
    public const string spineStrikeBack1 = "SpineEffects";
    public const string spineStrikeBackPre = "Prefabs/spineEffect";

    //Resources Path - Buff
    public const string skilllbuff = "Prefabs/buff"; //-Me-- Battle界面展示buff显示

    //Resources Path - Fonts
    public const string fonts = "Fonts";

    //Audio Path
    public const string audioPath = "Audios";
    public const string audioPrefab = "Prefabs/Audio/Audio";

    //Image
    public const string roleImagesPath = "Images/RoleImages";
    public const string allImagesPath = "Images/AllImages";
    public const string skillImagesPath = "Images/SkillImages";
    public const string actionImagesPath = "Images/ActionImages";
    public const string roleBuffImagesPath = "Images/BuffImages";
    public const string bloodValueImagesPath = "Assets/Resources/Images/Counts/BloodValue/BloodValue.png";
    public const string critValueImagesPath = "Assets/Resources/Images/Counts/CritValue/CritValue.png";
    public const string damageValueImagesPath = "Assets/Resources/Images/Counts/DamageValue/DamageValue.png";

    public const string bloodValueImagesPath1 = "Images/Counts/BloodValue";
    public const string critValueImagesPath1 = "Images/Counts/CritValue";
    public const string damageValueImagesPath1 = "Images/Counts/DamageValue";

    //BattlePlace
    public static Vector3 negativePlace = new Vector3(-1000f, 179f, 0);

    public static Vector3 firstPlace = new Vector3(800f, 179f, 0f);
    public static Vector3 secondPlace = new Vector3(1500f, 179f, 0f);
    public static Vector3 threePlace = new Vector3(2200f, 179f, 0f);
    public static Vector3 gameOverPlace = new Vector3(3000f, 179f, 0f);

    public static Vector3 firstPlaceCompare = new Vector3(800f, 179f, 0f);
    public static Vector3 secondPlaceCompare = new Vector3(1500f, 179f, 0f);
    public static Vector3 threePlaceCompare = new Vector3(2200f, 179f, 0f);

    //消息设置区
    public bool sendIdleMes = true;
    public bool sendRunMes = true;

    //战斗buff数值设置
    public static float isAttackAdd = 0.5f;
    public static float isAttackReduce = -0.5f;
    public static float isDefenceAdd = 0.6f;
    public static float isDefenceReduce = -0.7f;
    public static float isSpeedReduce = -0.3f;

    //测试区
    private static bool testBool = false;
    public static bool controleAndroidDebug = true;
    public bool isTest = true;
    public const string roleSpineShow = "Prefabs/Test/RoleSpineShow"; //-Me-- Role && Battle界面角色展示
    public const string monsterSpineShow = "Prefabs/Test/MonsterSpineShow"; //-Me-- Role && Battle界面角色展示
    public const int roleDamageCoefficient = 1;
    public const int monsterDamageCoefficient = 1;
    public const int changeCrit = 101; //修改暴击值

    public const float changeAndroidMoveSpeed = 2f;
    public const float changePCMoveSpeed = 50f;

    public static void TestVer(string sign,bool singleTestBool = false, float str = 1f, bool changBool = false,string debugInfo = "ceshi")
    {
        if (testBool)
        {
            if (singleTestBool)
            {
                str = 1f;
                changBool = true;
                Debug.Log("测试区的信息是：-----------------------------------"+debugInfo);
                
            }
        }
    }
}