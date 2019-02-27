using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;


public class ReadJsonfiles : Singleton<ReadJsonfiles>
{
    //Role
    public Roles role = new Roles();
    public List<Roles> roleInfos = new List<Roles>();
    public Dictionary<int,string[]> roleShowInfosDic = new Dictionary<int,string[]>();
    public Dictionary<int, Roles> roleDics = new Dictionary<int, Roles>();
    public Dictionary<int, List<Roles>> roleDicsl = new Dictionary<int, List<Roles>>();

    public List<int> roleIds = new List<int>();
    public List<int> roleIdsRoleSceneUse = new List<int>();

    //All  role + monster
    public List<int> allSpeeds = new List<int>();


    //Skill
    public List<Skills> skillInfos = new List<Skills>();
    public Dictionary<int, Skills> skillDics = new Dictionary<int, Skills>();
    public Dictionary<int, List<Skills>> skillDicsl = new Dictionary<int, List<Skills>>();


    //BUFF
    public Dictionary<int, Buff> buffDics = new Dictionary<int, Buff>();


    //Monster
    public Dictionary<int, Roles> monsterDic = new Dictionary<int, Roles>();
    public Dictionary<int, Skills> monsterSkillDic = new Dictionary<int, Skills>();
    public Dictionary<int, Buff> monsterBuffDic = new Dictionary<int, Buff>();


    //Audio
    public Dictionary<int, Audios> audioDic = new Dictionary<int, Audios>();
    public List<string> audioPath = new List<string>();
    public List<int> audioNumOrder = new List<int>();

    //Level
    public Dictionary<string, LevelConfiguration> levelDics = new Dictionary<string, LevelConfiguration>();
    public List<LevelConfiguration> levels = new List<LevelConfiguration>();

    public void ReadRoleJson()
    {
        TextAsset tx = Resources.Load("Json/Data/角色表") as TextAsset;
        Debug.Log(tx);
        string data = tx.text;
        JsonData jd = JsonMapper.ToObject(data);
        //UnitySingleton<RoleConfig>.Instance.roleId = new List<int>();
        for (int i = 0; i < jd.Count; i++)
        {
            Roles role = new Roles();
            JsonData roleitems = jd[i];
            role.roleid = int.Parse(roleitems["角色ID"].ToString());
            role.nickname = (string)roleitems["昵称"];
            role.attribute = (string)roleitems["属性"];
            role.profession = (string)roleitems["职业"];
            role.constellation = (string)roleitems["星座"];
            role.starlevel = int.Parse(roleitems["星级"].ToString());
            role.numberawake = int.Parse(roleitems["觉醒次数"].ToString());
            role.degree = int.Parse(roleitems["亲密度"].ToString());
            role.degreedes = (string)roleitems["亲密度描述"];
            role.rank = (string)roleitems["等级"];
            role.intro = (string)roleitems["一句话介绍"];
            role.fight = (string)roleitems["战斗力"];
            role.attack = float.Parse(roleitems["攻击"].ToString());
            role.defense =  float.Parse(roleitems["防御"].ToString());
            role.life = float.Parse(roleitems["生命"].ToString());
            role.speed =  float.Parse(roleitems["速度"].ToString());
            role.critchance = (string)roleitems["暴击概率"];
            role.critdamage = (string)roleitems["暴击伤害"];
            role.effecthit = (string)roleitems["效果命中"];
            role.effectresist = (string)roleitems["效果抵抗"];
            role.attackrate = (string)roleitems["连携攻击率"];
            role.skillid1 = int.Parse(roleitems["技能1ID"].ToString());
            role.skillid2 = int.Parse(roleitems["技能2ID"].ToString());
            role.skillid3 = int.Parse(roleitems["技能3ID"].ToString());

            roleInfos.Add(role);
            roleDics.Add(role.roleid, role);
            roleDicsl.Add(role.roleid, roleInfos);
            string[] roleShowInfos =  {role.attack.ToString(), role.defense.ToString(), role.life.ToString(), role.speed.ToString(), role.critchance.ToString(), role.critdamage, role.effecthit, role.effectresist, role.attackrate };
            roleShowInfosDic.Add(role.roleid,roleShowInfos);

            roleIds.Add(role.roleid);
            roleIdsRoleSceneUse.Add(role.roleid);

            allSpeeds.Add(int.Parse(role.speed.ToString()));

            ToMonsterRoleValue(i,role.roleid);

        }

        for (int i = 0; i < roleInfos.Count; i++)
        {
           ColorDebug.Instance.RedDebug(roleInfos[i].ToString(), "角色:", false);
        }
    }

    public void ReadSkillJson()
    {
        TextAsset tx = Resources.Load("Json/Data/技能表") as TextAsset;
        Debug.Log(tx);
        string data = tx.text;
        JsonData jd = JsonMapper.ToObject(data);

        for (int i = 0; i < jd.Count; i++)
        {
            Skills skill = new Skills();
            JsonData skillitems = jd[i];
            skill.skillid = int.Parse(skillitems["技能ID"].ToString());
            skill.skillname = (string)skillitems["技能名"];
            skill.releaseobj = int.Parse(skillitems["释放对象"].ToString());
            skill.skilladdnum = int.Parse(skillitems["技能加魂数量"].ToString());
            skill.skillcd = int.Parse(skillitems["技能CD"].ToString());
            skill.isattacknor = int.Parse(skillitems["是否普攻"].ToString());
            skill.isnpcskill = int.Parse(skillitems["是否NPC技能"].ToString());
            skill.ispassive = int.Parse(skillitems["是否被动"].ToString());
            skill.skilldes = (string)skillitems["技能描述"];
            skill.strengthen1 = (string)skillitems["强化+1"];
            skill.strengthen2 = (string)skillitems["强化+2"];
            skill.strengthen3 = (string)skillitems["强化+3"];
            skill.strengthen4 = (string)skillitems["强化+4"];
            skill.effectid1 = (string) skillitems["效果1ID"];
            skill.effectid2 = (string) skillitems["效果2ID"];
            skill.effectid3 = (string) skillitems["效果3ID"];
            skill.effectid4 = (string) skillitems["效果4ID"];
            //skill.des = (string)skillitems["开发用描述"];

            skillInfos.Add(skill);
            skillDics.Add(skill.skillid, skill);
            skillDicsl.Add(skill.skillid, skillInfos);
            
            ToMonsterSkillValue(i,skill.skillid);
        }

        for (int i = 0; i < skillInfos.Count; i++)
        {
            ColorDebug.Instance.RedDebug(skillInfos[i].ToString(), "技能：", false);
        }
    }

    public void ReadBuffJson()
    {
        TextAsset tx = Resources.Load("Json/Data/BUFF") as TextAsset;
        Debug.Log(tx);
        string data = tx.text;
        JsonData jd = JsonMapper.ToObject(data);

        for (int i = 0; i < jd.Count; i++)
        {
            Buff buff = new Buff();
            JsonData buffitems = jd[i];
            buff.effectId = int.Parse(buffitems["效果ID"].ToString());
            buff.target = int.Parse(buffitems["目标"].ToString());
            buff.effectType = (string)buffitems["效果类型"];
            buff.value1 = int.Parse(buffitems["数值1"].ToString());
            buff.value2 = int.Parse(buffitems["数值2"].ToString());
            buff.continueBout = int.Parse(buffitems["持续回合"].ToString());
            buff.onsetProb = (string)buffitems["发动概率"];

            buffDics.Add(buff.effectId,buff);

            ToMonsterBuffValue(i,buff.effectId);
        }

        for (int i = 0; i < skillInfos.Count; i++)
        {
            //ColorDebug.Instance.RedDebug(skillInfos[i].ToString(), "技能：", false);
        }
    }

    //赋值monster

    void ToMonsterRoleValue(int i,int monsterId)
    {
        if(i >= 5)
        {
            monsterDic.Add(monsterId, roleDics[monsterId]);
            
            foreach (KeyValuePair<int, Roles> pair in monsterDic)
            {
                //Debug.Log(pair.Key + "     " + pair.Value);
            }
        }
    }

    void ToMonsterSkillValue(int i, int monsterId)
    {
        if (i >= 15)
        {
            monsterSkillDic.Add(monsterId, skillDics[monsterId]);

            foreach (KeyValuePair<int, Skills> pair in monsterSkillDic)
            {
                //Debug.Log(pair.Key + "     " + pair.Value);
            }
        }
    }

    void ToMonsterBuffValue(int i, int monsterId)
    {
        if (i >= 24)
        {
            monsterBuffDic.Add(monsterId, buffDics[monsterId]);

            foreach (KeyValuePair<int, Buff> pair in monsterBuffDic)
            {
                //Debug.Log(pair.Key + "     " + pair.Value);
            }
        }
    }


    /// <summary>
    /// 音乐配置
    /// </summary>
    public void ReadAudioJson()
    {
        TextAsset tx = Resources.Load("Json/Data/音乐配置表") as TextAsset;
        Debug.Log(tx);
        string data = tx.text;
        JsonData jd = JsonMapper.ToObject(data);

        for (int i = 0; i < jd.Count; i++)
        {
            Audios audio = new Audios();
            JsonData audioitems = jd[i];
            audio.musicnNum = int.Parse(audioitems["Type"].ToString());
            audio.musicPath = (string)audioitems["FileName"];
            audio.musicDes = (string)audioitems["描述"];

            audioDic.Add(audio.musicnNum,audio);
            audioPath.Add(audio.musicPath);
            audioNumOrder.Add(audio.musicnNum);
        }


        foreach (KeyValuePair<int, Audios> pair in audioDic)
        {
            //Debug.Log(pair.Key + "     " + pair.Value);
        }
       
    }

    /// <summary>
    /// 关卡配置
    /// </summary>
    public void ReadLevelJson()
    {
        TextAsset tx = Resources.Load("Json/Data/关卡配置表") as TextAsset;
        Debug.Log(tx);
        string data = tx.text;
        JsonData jd = JsonMapper.ToObject(data);
        string strTitle;
        for (int i = 0; i < jd.Count; i++)
        {
            LevelConfiguration level = new LevelConfiguration();
            JsonData levelitems = jd[i];
            level.NPCID1 = int.Parse(levelitems["NPCID1"].ToString());
            level.NPCID2 = int.Parse(levelitems["NPCID2"].ToString());
            level.NPCID3 = int.Parse(levelitems["NPCID3"].ToString());
            level.NPCID4 = int.Parse(levelitems["NPCID4"].ToString());
            strTitle = "第" + (i + 1).ToString() + "波";

            levelDics.Add(strTitle,level);
            levels.Add(level);

        }

        foreach (KeyValuePair<string, LevelConfiguration> pair in levelDics)
        {
            //Debug.Log(pair.Key + "     " + pair.Value);
            //Debug.Log(levelDics.Count);
        }
    }

}
