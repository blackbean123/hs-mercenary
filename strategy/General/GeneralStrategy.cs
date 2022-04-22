using System.Collections.Generic;
using UnityEngine;

//此处不能有namespace 调用处没用namespace
public class GeneralStrategy
{
    //两个简单的例子,剩下的等大佬来解决。
    //最好把项目文件夹放到 .\Hearthstone\BepInEx\ 目录，这样不用再次指定引用路径。
    //编译之后把Dll放到插件同一目录即可


    private Dictionary<TAG_ROLE, TAG_ROLE> GetRestraintRoleDic()
    {
        Dictionary<TAG_ROLE, TAG_ROLE> restraintRoleDic = new Dictionary<TAG_ROLE, TAG_ROLE>();
        restraintRoleDic.Add(TAG_ROLE.TANK, TAG_ROLE.FIGHTER);
        restraintRoleDic.Add(TAG_ROLE.FIGHTER, TAG_ROLE.CASTER);
        restraintRoleDic.Add(TAG_ROLE.CASTER, TAG_ROLE.TANK);
        return restraintRoleDic;
    }

    public void Nomination()          //登场处理
    {
        //MessageBox.Show("登场处理");
        ZoneHand zoneHand = ZoneMgr.Get().FindZoneOfType<ZoneHand>(global::Player.Side.FRIENDLY);
        if (zoneHand != null)
        {

            List<string> fire = getTeamList();

            if (fire.Count == 0)
            {
                return;
            }

            foreach (string name in fire)
            {
                foreach (Card card in zoneHand.GetCards())
                {
                    if (name == card.GetEntity().GetName())
                    {
                        MyHsHelper.MyHsHelper.entranceQueue.Enqueue(card.GetEntity());
                        break;
                    }
                }
                if (MyHsHelper.MyHsHelper.entranceQueue.Count >= 6) { break; }
            }
        }
    }

    // 战斗处理
    public void Combat()
    {
        Debug.Log("start battle");
        ZonePlay zonePlay = ZoneMgr.Get().FindZoneOfType<ZonePlay>(global::Player.Side.FRIENDLY);
        ZonePlay enemyPlayZone = ZoneMgr.Get().FindZoneOfType<ZonePlay>(global::Player.Side.OPPOSING);
        List<string> fire = getTeamList();
        List<string> abilityNameList = getAbilityNameList();
        Debug.Log("team count" + fire.Count);
        Debug.Log("abilityNameList count" + abilityNameList.Count);

        if (fire.Count == 0 || abilityNameList.Count == 0)
        {
            return;
        }
        foreach (string name in fire)
        {
            foreach (Card card in zonePlay.GetCards())
            {
                MyHsHelper.MyHsHelper.Battles battles = new MyHsHelper.MyHsHelper.Battles();

                // 设置目标
                Entity firstTarget = GetFirstTarget(enemyPlayZone.GetCards());
                if (firstTarget != null)
                {
                    battles.target = firstTarget;
                }
                else
                {
                    string battlePolicy = MyHsHelper.MyHsHelper.battlePolicy;
                    if (battlePolicy == "颜色克制")
                    {
                        TAG_ROLE self = card.GetEntity().GetMercenaryRole();
                        battles.target = enemyPlayZone.GetCards()[0].GetEntity();
                        Dictionary<TAG_ROLE, TAG_ROLE> restraintRoleDic = GetRestraintRoleDic();
                        if (restraintRoleDic.ContainsKey(self))
                        {
                            foreach (Card enemyCard in enemyPlayZone.GetCards())
                            {
                                if (enemyCard.GetEntity().GetMercenaryRole() == restraintRoleDic[self])
                                {
                                    battles.target = enemyCard.GetEntity();
                                    break;
                                }
                            }
                        }
                    }
                    if (battlePolicy == "集火第一个")
                    {
                        battles.target = enemyPlayZone.GetCards()[0].GetEntity();
                    }
                    if (battlePolicy == "血量最少")
                    {
                        battles.target = HandleCards(enemyPlayZone.GetCards(), true, false, false);
                    }
                    if (battlePolicy == "血量最多")
                    {
                        battles.target = HandleCards(enemyPlayZone.GetCards(), false, true, false);
                    }
                }


                // 设置技能
                if (name == card.GetEntity().GetName())
                {

                    //battles.source = card.GetEntity();
                    List<Entity> abilityEntitys = GetLettuceAbilityEntitys(card.GetEntity());

                    foreach (string abilityName in abilityNameList)
                    {
                        foreach (Entity abilityEntity in abilityEntitys)
                        {
                            string s = abilityEntity.GetName();
                            s = s.Substring(0, s.Length - 1);
                            if (abilityName == s && GameState.Get().HasResponse(abilityEntity, new bool?(false)))
                            {
                                battles.ability = abilityEntity;
                                break;
                            }
                        }
                        if (battles.ability != null) { break; }
                    }
                }
            }
        }
    }

    private List<string> getTeamList()
    {
        List<string> teamList = new List<string>();
        string teamListStr = MyHsHelper.MyHsHelper.teamList;
        if (string.IsNullOrEmpty(teamListStr))
        {
            UIStatus.Get().AddInfo("佣兵队伍未配置");
            return teamList;
        }
        teamListStr = teamListStr.Replace(",", "，");
        string[] teamArray = teamListStr.Split(new char[] { '，' });
        teamList = new List<string>(teamArray);
        return teamList;
    }

    private List<string> getAbilityNameList()
    {
        List<string> abilityNameList = new List<string>();
        string abilityNameStr = MyHsHelper.MyHsHelper.abilityList;
        if (string.IsNullOrEmpty(abilityNameStr))
        {
            UIStatus.Get().AddInfo("技能未配置");
            return abilityNameList;
        }
        abilityNameStr = abilityNameStr.Replace(",", "，");
        string[] abilityArray = abilityNameStr.Split(new char[] { '，' });
        abilityNameList = new List<string>(abilityArray);
        return abilityNameList;
    }

    /// <summary>
    ///  选择优先攻击目标
    /// </summary>
    /// <param name="cards"></param>
    /// <returns></returns>
    private Entity GetFirstTarget(List<Card> cards)
    {
        string firstTargeLListStr = MyHsHelper.MyHsHelper.firstTarget;
        if (!string.IsNullOrEmpty(firstTargeLListStr))
        {
            firstTargeLListStr = firstTargeLListStr.Replace(",", "，");
            List<string> firstTargetList = new List<string>(firstTargeLListStr.Split(new char[] { '，' }));
            foreach (Card card in cards)
            {

                if (firstTargetList.Exists(t => t == card.GetEntity().GetName()))
                {
                    return card.GetEntity();
                }
            }
        }
        return null;
    }

    /// <summary>
    /// 选择目标
    /// </summary>
    /// <param name="cards">目标列表</param>
    /// <param name="healthMin">攻击最低血量目标</param>
    /// <param name="healthMax">攻击最高血量目标</param>
    /// <param name="isTaunt">攻击嘲讽目标</param>
    /// <param name="tAG_ROLE">目标类型(护卫，斗士，施法者)</param>
    /// INVALID, CASTER,FIGHTER,TANK,NEUTRAL
    /// <returns>返回目标Entity</returns>
    private Entity HandleCards(List<Card> cards, bool healthMin = false, bool healthMax = false, bool isTaunt = false, TAG_ROLE tAG_ROLE = TAG_ROLE.INVALID)
    {
        foreach (Card card in cards)
        {
            if (card.GetEntity().GetMercenaryRole() == tAG_ROLE && !card.GetEntity().IsStealthed() && isTaunt)
            {
                return card.GetEntity();
            }
        }
        if (isTaunt)
        {
            foreach (Card card in cards)
            {
                if (card.GetEntity().HasTaunt() && !card.GetEntity().IsStealthed())
                {
                    return card.GetEntity();
                }
            }
        }
        Entity target = cards[0].GetEntity();
        if (healthMin)
        {
            foreach (Card card in cards)
            {
                if (card.GetEntity().GetCurrentHealth() < target.GetCurrentHealth() && !card.GetEntity().IsStealthed())
                {
                    target = card.GetEntity();
                }
            }
        }
        if (healthMax)
        {
            foreach (Card card in cards)
            {
                if (card.GetEntity().GetCurrentHealth() > target.GetCurrentHealth() && !card.GetEntity().IsStealthed())
                {
                    target = card.GetEntity();
                }
            }
        }
        return target;
    }
    private List<Entity> GetLettuceAbilityEntitys(Entity entity)
    {
        List<Entity> m_displayedAbilityEntitys = new List<Entity>();
        foreach (int id in entity.GetLettuceAbilityEntityIDs())
        {
            Entity entity3 = GameState.Get().GetEntity(id);
            if (entity3 != null && !entity3.IsLettuceEquipment())
            {
                m_displayedAbilityEntitys.Add(entity3);
            }
        }
        return m_displayedAbilityEntitys;
    }

}