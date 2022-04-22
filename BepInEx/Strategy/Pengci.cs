using System.Collections.Generic;
using System.Windows.Forms;


public class Pengci      //如果编译多个策略请随机修改此类名，以免策略冲突
{
    //两个简单的例子,剩下的等大佬来解决。
    //最好把项目文件夹放到 .\Hearthstone\BepInEx\ 目录，这样不用再次指定引用路径。
    //编译之后把Dll放到插件同一目录即可

    private List<string> mercenary = new List<string>(new string[] { "瓦里安·乌瑞恩", "鞭笞者特里高雷", "珑心", "赤精", "安东尼达斯", "迦顿男爵", "拉格纳罗斯", "泽瑞拉", "剑圣萨穆罗" });
    private List<string> mercenaryAbilitys = new List<string>(new string[] { "反击", "反冲", "星火祝福", "天神之息", "火雨风暴", "烈焰之歌", "火球术", "地狱火", "死吧，虫子", "致盲之光", "二连击" });

    public void Nomination()          //登场处理
    {
        //MessageBox.Show("登场处理");
        ZoneHand zoneHand = ZoneMgr.Get().FindZoneOfType<ZoneHand>(global::Player.Side.FRIENDLY);
        if (zoneHand != null)
        {
            List<string> fire;
            MyHsHelper.MyHsHelper.State state = new MyHsHelper.MyHsHelper.State();
            if (state.IsPVP)
            {
                fire = mercenary;
            } else {
                fire = mercenary;
            }
            foreach (string name in fire)
            {
                foreach (Card card in zoneHand.GetCards())
                {
                    if (name == card.GetEntity().GetName())
                    {
                        MyHsHelper.MyHsHelper.EntranceQueue.Enqueue(card.GetEntity());
                        break;
                    }
                }
                if (MyHsHelper.MyHsHelper.EntranceQueue.Count >= 3) { break; }
            }
        }
    }

    public void Combat()        //战斗处理
    {
        //MessageBox.Show("战斗处理");
        ZonePlay zonePlay = ZoneMgr.Get().FindZoneOfType<ZonePlay>(global::Player.Side.FRIENDLY);
        ZonePlay enemyPlayZone = ZoneMgr.Get().FindZoneOfType<ZonePlay>(global::Player.Side.OPPOSING);
        List<string> fire = mercenary;
        List<string> AbilityNames = mercenaryAbilitys;
        foreach (string name in fire)
        {
            foreach (Card card in zonePlay.GetCards())
            {
                if (name == card.GetEntity().GetName())
                {
                    MyHsHelper.MyHsHelper.Battles battles = new MyHsHelper.MyHsHelper.Battles();
                    //battles.source = card.GetEntity();
                    List<Entity> AbilityEntitys = GetLettuceAbilityEntitys(card.GetEntity());

                    foreach (string AbilityName in AbilityNames)
                    {
                        foreach (Entity AbilityEntity in AbilityEntitys)
                        {
                            string s = AbilityEntity.GetName();
                            s = s.Substring(0, s.Length - 1);
                            if (AbilityName == s && GameState.Get().HasResponse(AbilityEntity, new bool?(false)))
                            {
                                battles.Ability = AbilityEntity;
                                break;
                            }
                        }
                        if (battles.Ability != null) { break; }
                    }

                    if (name == "凯瑞尔·罗姆" && card.GetEntity().HasTaunt())
                    {
                        foreach (Entity entity in GetLettuceAbilityEntitys(card.GetEntity()))
                        {
                            string s = entity.GetName();
                            s = s.Substring(0, s.Length - 1);
                            if (s == "远征军打击")
                            {
                                battles.Ability = entity;
                                battles.target = HandleCards(enemyPlayZone.GetCards(), true, false, true);
                                break;
                            }
                        }
                    }

                    if (new List<string>(new string[] { "玛法里奥·怒风", "古夫·符文图腾", "布鲁坎" }).Contains(name))
                    {
                        battles.target = HandleCards(enemyPlayZone.GetCards(), true, false, false, TAG_ROLE.TANK);
                    }

                    if (name == "安东尼达斯")
                    {
                        MyHsHelper.MyHsHelper.State state = new MyHsHelper.MyHsHelper.State();
                        battles.target = HandleCards(enemyPlayZone.GetCards(), state.IsPVP, !state.IsPVP);
                    }

                    if (new List<string>(new string[] { "泽瑞拉", "剑圣萨穆罗" }).Contains(name))
                    {
                        battles.target = HandleCards(enemyPlayZone.GetCards(),true,false, true, TAG_ROLE.CASTER);
                    }
                    if (name == "珑心")
                    {
                        battles.target = HandleCards(enemyPlayZone.GetCards(), true, false, true, TAG_ROLE.TANK);
                    }
                    MyHsHelper.MyHsHelper.BattleQueue.Enqueue(battles);
                }
            }
        }
    }

    /// <summary>
    /// 选择目标
    /// </summary>
    /// <param name="cards">目标列表</param>
    /// <param name="healthMin">攻击最低血量目标</param>
    /// <param name="healthMax">攻击最高血量目标</param>
    /// <param name="isTaunt">攻击嘲讽目标</param>
    /// <param name="tAG_ROLE">目标类型(护卫，斗士，施法者)</param>
    /// public enum TAG_ROLE
    ///{
    ///    INVALID,
    ///    CASTER,
    ///    FIGHTER,
    ///    TANK,
    ///    NEUTRAL
    ///} 
    ///#if false // 反编译日志
    /// <returns>返回目标Entity</returns>
    private Entity HandleCards(List<Card> cards,bool healthMin = false , bool healthMax = false ,bool isTaunt = false, TAG_ROLE tAG_ROLE = TAG_ROLE.INVALID)
    {
        foreach (Card card in cards)
        {
            if (card.GetEntity().GetMercenaryRole() == tAG_ROLE  && !card.GetEntity().IsStealthed() && isTaunt)
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





