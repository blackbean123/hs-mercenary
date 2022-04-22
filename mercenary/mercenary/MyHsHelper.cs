using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Assets;
using BepInEx;
using BepInEx.Configuration;
using bgs;
using HarmonyLib;
using Hearthstone;
using Hearthstone.Progression;
using PegasusLettuce;
using PegasusShared;
using PegasusUtil;
using UnityEngine;

namespace MyHsHelper
{
	[BepInPlugin("plugins.MyHsHelper", "佣兵挂机插件", "2.0.0.0")]
	public class MyHsHelper : BaseUnityPlugin
	{
		private struct Reward
		{
			public int level;

			public int xp;

			public int xpNeeded;
		}

		public struct Battles
		{
			public Entity source;

			public Entity target;

			public Entity ability;

			public string subName;
		}

		public static bool enableAutoPlay;

		public static bool initialize;

		public static float sleeptime;

		public static int fakeClickDownCount;

		public static float idleTime;

		private static Queue clickqueue;

		public static bool hideGui;

		private static Reward reward;

		private string labelstr;

		private static int todayXp;

		private static float currentTime;

		private static int yesterdayXp;

		private static float yesterdayTime;

		public static ConfigEntry<bool> autoRunCfg;

		public static ConfigEntry<bool> onlyPcCfg;

		public static ConfigEntry<bool> pvpCfg;

		public static ConfigEntry<string> pvpTeamCfg;

		public static ConfigEntry<string> pveTeamCfg;

		public static ConfigEntry<bool> autoConcedeCfg;

		public static ConfigEntry<int> concedelineCfg;

		public static ConfigEntry<bool> autoSwitchCfg;

		public static ConfigEntry<int> switchLineCfg;

		public static ConfigEntry<int> mapIdCfg;

		public static ConfigEntry<bool> pveModeCfg;

		public static ConfigEntry<int> stepCfg;

		public static ConfigEntry<string> pvpPolicyCfg;

		public static ConfigEntry<string> pvePolicyCfg;

		public static ConfigEntry<long> startTimeCfg;

		public static ConfigEntry<bool> hideMainCfg;

		public static ConfigEntry<int> todayXpCfg;

		public static ConfigEntry<float> todayTimeCfg;

		public static ConfigEntry<int> yesterdayXpCfg;

		public static ConfigEntry<float> yesterdayTimeCfg;

		public static ConfigEntry<string> whitelistCfg;

		public static ConfigEntry<string> blacklistCfg;

		public static ConfigEntry<string> teamListCfg;

		public static ConfigEntry<string> abilityListCfg;

		public static ConfigEntry<string> firstTargetCfg;

		public static ConfigEntry<string> battlePolicyCfg;

		public static string teamList;

		public static string abilityList;

		public static string firstTarget;

		public static string battlePolicy;

		public static string whitelist;

		public static string blacklist;

		public static bool defeat;

		public static bool pvpMode;

		public static bool pveMode;

		public static int pveStep;

		public static string pvpStrategy;

		public static string pveStrategy;

		public static long startTime;

		public static bool onlyPC;

		public static string pvpTeamName;

		public static string pveTeamName;

		public static int mapID;

		public static int point;

		public static bool autoSwitch;

		public static int switchLine;

		private float labeltimer;

		private static List<LettuceMapNode> minNode;

		private static bool flag;

		private static bool isHaveRewardTask;

		private static List<string> taskMercenary;

		private static List<string> taskAbilityName;

		private static MainForm form;

		private static string path;

		private static int getHashCount;

		private static bool hashOK;

		private static object strategyInstance;

		private static MethodInfo entrance;

		private static MethodInfo battle;

		private static bool strategyOK;

		private static int phaseID;

		private static bool strategyRun;

		public static Queue<Entity> entranceQueue;

		public static Queue<Battles> battleQueue;

		private static Battles battles;

		private static bool handleQueueOK;

		private void Awake()
		{
		}

		private void OnGUI()
		{
			if (hideGui || !hashOK)
			{
				return;
			}
			if (initialize)
			{
				if (GUI.Button(new Rect(1f, 1f, 90f, 29f), new GUIContent("自动佣兵：" + (enableAutoPlay ? "开" : "关"))))
				{
					Resetidle();
					enableAutoPlay = !enableAutoPlay;
					autoRunCfg.Value = enableAutoPlay;
					flag = enableAutoPlay;
					strategyRun = false;
					handleQueueOK = true;
					entranceQueue.Clear();
					battleQueue.Clear();
					if (form != null && !form.IsDisposed)
					{
						form.autoButton.Text = "自动佣兵：" + (enableAutoPlay ? "开" : "关");
					}
				}
				if (GUI.Button(new Rect(92f, 1f, 90f, 29f), new GUIContent("设置")))
				{
					if (form == null)
					{
						form = new MainForm();
						form.Show();
					}
					else if (form.IsDisposed)
					{
						form = new MainForm();
						form.Show();
					}
					else
					{
						form.Activate();
					}
				}
			}
			GUIStyle gUIStyle = new GUIStyle();
			gUIStyle.normal.background = null;
			gUIStyle.normal.textColor = new Color(1f, 0f, 0f);
			gUIStyle.fontSize = 22;
			GUI.Label(new Rect(190f, 1f, 1800f, 38f), labelstr, gUIStyle);
		}

		private void Start()
		{
			path = System.Windows.Forms.Application.StartupPath + "\\BepInEx\\idleTime.log";
			ConfigEntry<float> configEntry = base.Config.Bind(DateTime.Now.AddDays(1.0).ToString("MM-dd"), "时间", 0f);
			ConfigEntry<int> configEntry2 = base.Config.Bind(DateTime.Now.AddDays(1.0).ToString("MM-dd"), "经验", 0);
			base.Config.Clear();
			configEntry.Value = 0f;
			configEntry2.Value = 0;
			for (int i = 3; i < 30; i++)
			{
				configEntry = base.Config.Bind(DateTime.Now.AddDays(-i).ToString("MM-dd"), "时间", 0f);
				ConfigEntry<int> configEntry3 = base.Config.Bind(DateTime.Now.AddDays(-i).ToString("MM-dd"), "经验", 0);
				base.Config.Clear();
				configEntry.Value = 0f;
				configEntry3.Value = 0;
			}
			base.Config.Clear();
			autoRunCfg = base.Config.Bind("配置", "AutoRun", defaultValue: false, "是否自动佣兵挂机");
			enableAutoPlay = autoRunCfg.Value;
			pvpCfg = base.Config.Bind("配置", "PVP", defaultValue: false, "PVP或者PVE");
			pvpMode = pvpCfg.Value;
			onlyPcCfg = base.Config.Bind("配置", "onlyPC", defaultValue: false, "PVP只打电脑");
			onlyPC = onlyPcCfg.Value;
			autoConcedeCfg = base.Config.Bind("配置", "投降", defaultValue: false, "是否自动认输");
			defeat = autoConcedeCfg.Value;
			concedelineCfg = base.Config.Bind("配置", "分数线", 6000, "自动认输分数线，高于此分数自动认输");
			point = concedelineCfg.Value;
			autoSwitchCfg = base.Config.Bind("配置", "切换", defaultValue: false, "是否切换至PVE");
			autoSwitch = autoSwitchCfg.Value;
			switchLineCfg = base.Config.Bind("配置", "切换线", 14000, "达到此分数时切换至PVE");
			switchLine = switchLineCfg.Value;
			pveModeCfg = base.Config.Bind("配置", "PVE模式", defaultValue: true, "true任务模式 false刷图模式");
			pveMode = pveModeCfg.Value;
			stepCfg = base.Config.Bind("配置", "步数", 2, "距离神秘选项怪物数，超过则重开地图。");
			pveStep = stepCfg.Value;
			mapIdCfg = base.Config.Bind("配置", "地图ID", 92, "地图ID");
			mapID = mapIdCfg.Value;
			pvpTeamCfg = base.Config.Bind("配置", "PVPteam", "", "PVP队伍名字");
			pvpTeamName = pvpTeamCfg.Value;
			pveTeamCfg = base.Config.Bind("配置", "PVEteam", "", "PVE队伍名字");
			pveTeamName = pveTeamCfg.Value;
			pvpPolicyCfg = base.Config.Bind("配置", "PVP策略", "", "PVP策略文件名");
			pvpStrategy = pvpPolicyCfg.Value;
			pvePolicyCfg = base.Config.Bind("配置", "PVE策略", "", "PVE策略文件名");
			pveStrategy = pvePolicyCfg.Value;
			whitelistCfg = base.Config.Bind("配置", "优先宝藏", "", "优先宝藏");
			whitelist = whitelistCfg.Value;
			blacklistCfg = base.Config.Bind("配置", "宝藏黑名单", "", "宝藏黑名单");
			blacklist = blacklistCfg.Value;
			teamListCfg = base.Config.Bind("配置", "登场顺序", "", "登场顺序");
			teamList = teamListCfg.Value;
			firstTargetCfg = base.Config.Bind("配置", "优先击杀目标", "", "优先击杀目标");
			firstTarget = firstTargetCfg.Value;
			abilityListCfg = base.Config.Bind("配置", "优先技能", "", "优先技能");
			abilityList = abilityListCfg.Value;
			battlePolicyCfg = base.Config.Bind("配置", "战斗策略", "", "战斗策略");
			battlePolicy = battlePolicyCfg.Value;
			startTimeCfg = base.Config.Bind("配置", "开始时间", 0L, "对局开始时间");
			startTime = startTimeCfg.Value;
			hideMainCfg = base.Config.Bind("配置", "隐藏", defaultValue: false, "是否隐藏GUI");
			hideGui = hideMainCfg.Value;
			todayXpCfg = base.Config.Bind(DateTime.Now.ToString("MM-dd"), "经验", 0, "当日获得的通行证经验值");
			todayXp = todayXpCfg.Value;
			todayTimeCfg = base.Config.Bind(DateTime.Now.ToString("MM-dd"), "时间", 0f, "当日挂机总时间");
			currentTime = todayTimeCfg.Value;
			yesterdayXpCfg = base.Config.Bind(DateTime.Now.AddDays(-1.0).ToString("MM-dd"), "经验", 0, "当日获得的通行证经验值");
			yesterdayXp = yesterdayXpCfg.Value;
			yesterdayTimeCfg = base.Config.Bind(DateTime.Now.AddDays(-1.0).ToString("MM-dd"), "时间", 0f, "当日挂机总时间(分钟)");
			yesterdayTime = yesterdayTimeCfg.Value;
			GetHash();
			LoadPolicy();
			Harmony harmony = new Harmony("MyHsHelper.patch");
			harmony.PatchAll();
			MethodInfo method = typeof(RewardPopups).GetMethod("ShowMercenariesRewards");
			MethodInfo method2 = typeof(MyHsHelper).GetMethod("AutoGetRewards");
			harmony.Patch(method, new HarmonyMethod(method2));
			method = typeof(MercenariesSeasonRewardsDialog).GetMethod("ShowWhenReady", BindingFlags.Instance | BindingFlags.NonPublic);
			method2 = typeof(MyHsHelper).GetMethod("SeasonRewardsDialog");
			harmony.Patch(method, new HarmonyMethod(method2));
			method = typeof(RewardBoxesDisplay).GetMethod("OnDoneButtonShown", BindingFlags.Instance | BindingFlags.NonPublic);
			method2 = typeof(MyHsHelper).GetMethod("BoxConfirm");
			harmony.Patch(method, null, new HarmonyMethod(method2));
			method = typeof(RewardBoxesDisplay).GetMethod("RewardPackageOnComplete", BindingFlags.Instance | BindingFlags.NonPublic);
			method2 = typeof(MyHsHelper).GetMethod("openBox");
			harmony.Patch(method, null, new HarmonyMethod(method2));
			method = typeof(LettuceMap).GetMethod("CreateMapFromProto", BindingFlags.Instance | BindingFlags.Public);
			method2 = typeof(MyHsHelper).GetMethod("onGameEnd");
			harmony.Patch(method, null, new HarmonyMethod(method2));
			method = typeof(LettuceMapDisplay).GetMethod("TryAutoNextSelectCoin", BindingFlags.Instance | BindingFlags.NonPublic);
			method2 = typeof(MyHsHelper).GetMethod("autoSelectNextCoin");
			harmony.Patch(method, null, new HarmonyMethod(method2));
			method = typeof(LettuceMapDisplay).GetMethod("DisplayNewlyGrantedAnomalyCards", BindingFlags.Instance | BindingFlags.NonPublic);
			method2 = typeof(MyHsHelper).GetMethod("ClickNewlyCards");
			harmony.Patch(method, new HarmonyMethod(method2));
			method = typeof(LettuceMapDisplay).GetMethod("ShouldShowVisitorSelection", BindingFlags.Instance | BindingFlags.NonPublic);
			method2 = typeof(MyHsHelper).GetMethod("AutoVisitor");
			harmony.Patch(method, null, new HarmonyMethod(method2));
			method = typeof(LettuceMapDisplay).GetMethod("OnVisitorSelectionResponseReceived", BindingFlags.Instance | BindingFlags.NonPublic);
			method2 = typeof(MyHsHelper).GetMethod("visitorSelectResponse");
			harmony.Patch(method, new HarmonyMethod(method2));
			method = typeof(HearthstoneApplication).GetMethod("OnApplicationFocus", BindingFlags.Instance | BindingFlags.NonPublic);
			method2 = typeof(MyHsHelper).GetMethod("SetApplicationFocus");
			harmony.Patch(method, new HarmonyMethod(method2));
			method = typeof(AlertPopup).GetMethod("Show");
			method2 = typeof(MyHsHelper).GetMethod("show");
			harmony.Patch(method, new HarmonyMethod(method2));
			method = typeof(GraphicsResolution).GetMethod("IsAspectRatioWithinLimit");
			method2 = typeof(MyHsHelper).GetMethod("setAspectRatio");
			harmony.Patch(method, new HarmonyMethod(method2));
			method = typeof(DialogManager).GetMethod("ShowReconnectHelperDialog");
			method2 = typeof(MyHsHelper).GetMethod("reconnectHelperDialog");
			harmony.Patch(method, new HarmonyMethod(method2));
			method = typeof(Network).GetMethod("OnFatalBnetError");
			method2 = typeof(MyHsHelper).GetMethod("netError");
			harmony.Patch(method, new HarmonyMethod(method2));
			method = typeof(ReconnectHelperDialog).GetMethod("Show");
			method2 = typeof(MyHsHelper).GetMethod("reconnectHelperDialog");
			harmony.Patch(method, new HarmonyMethod(method2));
			method = typeof(LettuceMissionEntity).GetMethod("ShiftPlayZoneForGamePhase", BindingFlags.Instance | BindingFlags.NonPublic);
			method2 = typeof(MyHsHelper).GetMethod("Phase");
			harmony.Patch(method, null, new HarmonyMethod(method2));
			method = typeof(SplashScreen).GetMethod("GetRatingsScreenRegion", BindingFlags.Instance | BindingFlags.NonPublic);
			method2 = typeof(MyHsHelper).GetMethod("skip");
			harmony.Patch(method, new HarmonyMethod(method2));
			method = typeof(QuestPopups).GetMethod("ShowNextQuestNotification");
			method2 = typeof(MyHsHelper).GetMethod("skip");
			harmony.Patch(method, new HarmonyMethod(method2));
			method = typeof(EndGameScreen).GetMethod("ShowMercenariesExperienceRewards", BindingFlags.Instance | BindingFlags.NonPublic);
			method2 = typeof(MyHsHelper).GetMethod("show");
			harmony.Patch(method, new HarmonyMethod(method2));
			method = typeof(RewardTrackManager).GetMethod("UpdateStatus", BindingFlags.Instance | BindingFlags.NonPublic);
			method2 = typeof(MyHsHelper).GetMethod("updateStatus");
			harmony.Patch(method, new HarmonyMethod(method2));
			method = typeof(EnemyEmoteHandler).GetMethod("IsSquelched");
			method2 = typeof(MyHsHelper).GetMethod("skip");
			harmony.Patch(method, new HarmonyMethod(method2));
		}

		public static bool updateStatus(int rewardTrackId, int level, RewardTrackManager.RewardStatus status, bool forPaidTrack, List<RewardItemOutput> rewardItemOutput)
		{
			if (!enableAutoPlay)
			{
				return true;
			}
			if (status == RewardTrackManager.RewardStatus.GRANTED)
			{
				RewardTrackManager.Get().AckRewardTrackReward(rewardTrackId, level, forPaidTrack);
				return false;
			}
			return true;
		}

		public static bool visitorSelectResponse()
		{
			if (!enableAutoPlay)
			{
				return true;
			}
			Network.Get().GetMercenariesMapVisitorSelectionResponse();
			return false;
		}

		public static bool skip()
		{
			return false;
		}

		public static void Phase(int phase)
		{
			phaseID = phase;
		}

		public static bool netError(BnetErrorInfo info)
		{
			if (!enableAutoPlay)
			{
				return true;
			}
			UnityEngine.Application.Quit();
			return false;
		}

		public static bool reconnectHelperDialog()
		{
			if (!enableAutoPlay)
			{
				return true;
			}
			UnityEngine.Application.Quit();
			return false;
		}

		public static bool setAspectRatio(ref bool __result, int width, int height, bool isWindowedMode)
		{
			__result = true;
			return false;
		}

		public static bool show()
		{
			if (!enableAutoPlay)
			{
				return true;
			}
			return false;
		}

		public static bool SetApplicationFocus(bool focus)
		{
			return false;
		}

		public static void AutoVisitor(PegasusLettuce.LettuceMap map, ref bool __result)
		{
			if (enableAutoPlay && map != null)
			{
				__result = false;
				if (map.HasPendingVisitorSelection && map.PendingVisitorSelection.VisitorOptions.Count > 0)
				{
					Network.Get().MakeMercenariesMapVisitorSelection(0);
				}
			}
		}

		public static bool ClickNewlyCards(PegasusLettuce.LettuceMap lettuceMap, int completedNodeId)
		{
			if (!enableAutoPlay)
			{
				return true;
			}
			return false;
		}

		public static void onGameEnd(PegasusLettuce.LettuceMap lettuceMap)
		{
			if (!enableAutoPlay || lettuceMap == null)
			{
				return;
			}
			NetCache.NetCacheMercenariesVillageVisitorInfo netObject = NetCache.Get().GetNetObject<NetCache.NetCacheMercenariesVillageVisitorInfo>();
			if (netObject != null)
			{
				for (int i = 0; i < netObject.VisitorStates.Count; i++)
				{
					if (netObject.VisitorStates[i].ActiveTaskState.Status_ == MercenariesTaskState.Status.COMPLETE)
					{
						Network.Get().ClaimMercenaryTask(netObject.VisitorStates[i].ActiveTaskState.TaskId);
					}
				}
			}
			foreach (LettuceMapNode node in lettuceMap.Nodes)
			{
				if (GameUtils.IsFinalBossNodeType((int)node.NodeTypeId) && node.NodeState_ == LettuceMapNode.NodeState.COMPLETE)
				{
					SceneMgr.Get().SetNextMode(SceneMgr.Mode.LETTUCE_BOUNTY_BOARD, SceneMgr.TransitionHandlerType.NEXT_SCENE);
					return;
				}
			}
			if (ShouldShowTreasureSelection(lettuceMap))
			{
				// 增加黑白宝藏配置项
				int selectOption = 0;
				int current = -1;
				foreach (int dbId in lettuceMap.PendingTreasureSelection.TreasureOptions)
				{
					++current;
					// 宝藏实体
					EntityDef treasure = DefLoader.Get().GetEntityDef(dbId, false);
					// 宝藏名称，可用于黑白名单
					string name = treasure.GetName();
					if (!string.IsNullOrEmpty(whitelist))
					{
						whitelist = whitelist.Replace(",", "，"); // 兼容英文逗号分割
						string[] wList = whitelist.Split('，');
						bool isDone = false;
						foreach (string w in wList)
						{
							if (name.Contains(w))
							{
								selectOption = current;
								// 用于跳出外部循环
								isDone = true;
								break;
							}
						}
						if (isDone) break;
					}

					// 黑名单判断
					if (!string.IsNullOrEmpty(blacklist))
					{
						blacklist = blacklist.Replace(",", "，");
						string[] bList = blacklist.Split('，');
						bool isSkip = false;
						foreach (string w in bList)
						{
							if (name.Contains(w))
							{ 
								// 用于跳过外部循环
								isSkip = true;
								break;
							}
						}
						if (isSkip) continue;
					}

				}

				// 选择宝藏
				Network.Get().MakeMercenariesMapTreasureSelection(selectOption);
			}
			if (ShouldShowVisitorSelection(lettuceMap))
			{
				Network.Get().MakeMercenariesMapVisitorSelection(0);
			}
		}

		public static void HandleMap()
		{
			flag = false;
			minNode = getshenmi(out var step);
			if (step > pveStep || minNode[minNode.Count - 1].NodeState_ == LettuceMapNode.NodeState.COMPLETE)
			{
				if (step > pveStep)
				{
					UIStatus.Get().AddInfo("怪物节点数：" + step + "，重开地图。");
				}
				Network.Get().RetireLettuceMap();
				sleeptime += 2f;
				flag = true;
				return;
			}
			for (int i = 0; i < minNode.Count; i++)
			{
				if (minNode[i].NodeState_ != LettuceMapNode.NodeState.UNLOCKED)
				{
					continue;
				}
				if (Array.IndexOf(new uint[4]
				{
					1u,
					2u,
					3u,
					22u
				}, minNode[i].NodeTypeId) > -1)
				{
					GameMgr.Get().FindGame(GameType.GT_MERCENARIES_PVE, FormatType.FT_WILD, 3790, 0, 0L, null, null, restoreSavedGameState: false, null, (int)minNode[i].NodeId, 0L);
					flag = true;
					break;
				}
				Network.Get().ChooseLettuceMapNode(minNode[i].NodeId);
				minNode[i].NodeState_ = LettuceMapNode.NodeState.COMPLETE;
				if (i < minNode.Count - 1)
				{
					minNode[i + 1].NodeState_ = LettuceMapNode.NodeState.UNLOCKED;
				}
				if (minNode[i].NodeTypeId == 0)
				{
					for (int j = i + 1; j < minNode.Count - 1; j++)
					{
						minNode[j].NodeState_ = LettuceMapNode.NodeState.COMPLETE;
					}
				}
				if (i == minNode.Count - 1)
				{
					minNode[i].NodeState_ = LettuceMapNode.NodeState.COMPLETE;
					Network.Get().MakeMercenariesMapVisitorSelection(0);
				}
				flag = true;
				break;
			}
			flag = true;
		}

		public static void autoSelectNextCoin()
		{
			if (enableAutoPlay && flag)
			{
				Resetidle();
				if (!HaveRewardTask())
				{
					SceneMgr.Get().SetNextMode(SceneMgr.Mode.LETTUCE_VILLAGE);
					sleeptime += 5f;
				}
				else
				{
					HandleMap();
				}
			}
		}

		public static bool SeasonRewardsDialog(MercenariesSeasonRewardsDialog __instance)
		{
			if (!enableAutoPlay)
			{
				return true;
			}
			MercenariesSeasonRewardsDialog.Info info = (MercenariesSeasonRewardsDialog.Info)Traverse.Create(__instance).Field("m_info").GetValue();
			Network.Get().AckNotice(info.m_noticeId);
			return false;
		}

		public static bool AutoGetRewards(ref bool autoOpenChest, ref NetCache.ProfileNoticeMercenariesRewards rewardNotice, Action doneCallback = null)
		{
			if (!enableAutoPlay)
			{
				return true;
			}
			autoOpenChest = true;
			if (rewardNotice.RewardType == ProfileNoticeMercenariesRewards.RewardType.REWARD_TYPE_PVE_CONSOLATION)
			{
				Network.Get().AckNotice(rewardNotice.NoticeID);
				doneCallback?.Invoke();
				return false;
			}
			return true;
		}

		public static void openBox(RewardBoxesDisplay.RewardBoxData boxData)
		{
			if (enableAutoPlay)
			{
				sleeptime += 3f;
				boxData.m_RewardPackage.TriggerPress();
			}
		}

		public static void BoxConfirm(Spell spell, object userData)
		{
			if (enableAutoPlay)
			{
				sleeptime += 2f;
				RewardBoxesDisplay.Get().m_DoneButton.TriggerPress();
				RewardBoxesDisplay.Get().m_DoneButton.TriggerRelease();
			}
		}

		public static bool ShouldShowTreasureSelection(PegasusLettuce.LettuceMap map)
		{
			if (map.HasPendingTreasureSelection)
			{
				return map.PendingTreasureSelection.TreasureOptions.Count > 0;
			}
			return false;
		}

		public static bool ShouldShowVisitorSelection(PegasusLettuce.LettuceMap map)
		{
			if (map.HasPendingVisitorSelection)
			{
				return map.PendingVisitorSelection.VisitorOptions.Count > 0;
			}
			return false;
		}

		private void Update()
		{
			if (!hashOK)
			{
				enableAutoPlay = false;
				return;
			}
			idleTime += Time.deltaTime;
			if (idleTime > 200f && enableAutoPlay)
			{
				UnityEngine.Application.Quit();
			}
			currentTime += Time.deltaTime;
			labeltimer -= Time.deltaTime;
			if (labeltimer <= 0f)
			{
				labeltimer = 20f;
				todayTimeCfg.Value = currentTime;
				todayXp += Getxp();
				todayXpCfg.Value = todayXp;
				labelstr = "通行证：" + reward.level + "  " + RewardTrackManager.Get().TrackDataModel.XpProgress + "   经验：" + todayXp + "  " + (int)((float)todayXp / (currentTime / 3600f)) + "/小时 (F9隐藏信息)";
			}
			if (Input.GetKeyUp(KeyCode.F9))
			{
				hideGui = !hideGui;
				hideMainCfg.Value = hideGui;
			}
			if (!(Time.realtimeSinceStartup - sleeptime > 1f))
			{
				return;
			}
			sleeptime = Time.realtimeSinceStartup;
			if (initialize)
			{
				if (!enableAutoPlay)
				{
					return;
				}
				GameMgr gameMgr = GameMgr.Get();
				GameType gameType = gameMgr.GetGameType();
				SceneMgr sceneMgr = SceneMgr.Get();
				SceneMgr.Mode mode = sceneMgr.GetMode();
				GameState gameState = GameState.Get();
				if (gameMgr.IsFindingGame())
				{
					sleeptime += 1f;
					handleQueueOK = true;
					entranceQueue.Clear();
					battleQueue.Clear();
					Resetidle();
					startTime = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000L) / 10000000;
					startTimeCfg.Value = startTime;
					return;
				}
				if (gameType == GameType.GT_UNKNOWN && (mode == SceneMgr.Mode.LETTUCE_VILLAGE || mode == SceneMgr.Mode.LETTUCE_PLAY) && gameState == null)
				{
					if (pvpMode)
					{
						if (autoSwitch && NetCache.Get().GetNetObject<NetCache.NetCacheMercenariesPlayerInfo>().PvpRating >= switchLine)
						{
							bool flag2 = (pvpCfg.Value = false);
							pvpMode = flag2;
							LoadPolicy();
							return;
						}
						List<LettuceTeam> teams = CollectionManager.Get().GetTeams();
						if (teams.Count == 0)
						{
							UIStatus.Get().AddInfo("请先创建队伍并在设置里选择队伍！");
							autoRunCfg.Value = (enableAutoPlay = false);
							return;
						}
						LettuceTeam lettuceTeam = null;
						foreach (LettuceTeam item in teams)
						{
							if (item.Name == pvpTeamName)
							{
								lettuceTeam = item;
								break;
							}
						}
						if (lettuceTeam == null)
						{
							UIStatus.Get().AddInfo("请先在设置里选择队伍！");
							autoRunCfg.Value = (enableAutoPlay = false);
						}
						else
						{
							GameMgr.Get().FindGame(GameType.GT_MERCENARIES_PVP, FormatType.FT_WILD, 3743, 0, 0L, null, null, restoreSavedGameState: false, null, null, lettuceTeam.ID);
						}
					}
					else
					{
						Resetidle();
						sleeptime += 3f;
						HaveRewardTask();
						SceneMgr.Get().SetNextMode(SceneMgr.Mode.LETTUCE_MAP, SceneMgr.TransitionHandlerType.NEXT_SCENE);
					}
					return;
				}
				if (gameType == GameType.GT_UNKNOWN && mode == SceneMgr.Mode.HUB && gameState == null)
				{
					sceneMgr.SetNextMode(SceneMgr.Mode.LETTUCE_VILLAGE);
					sleeptime += 5f;
					return;
				}
				if (gameType == GameType.GT_UNKNOWN && mode == SceneMgr.Mode.LETTUCE_BOUNTY_BOARD && gameState == null)
				{
					Resetidle();
					sleeptime += 3f;
					LettuceVillageDisplay.LettuceSceneTransitionPayload lettuceSceneTransitionPayload = new LettuceVillageDisplay.LettuceSceneTransitionPayload();
					LettuceBountyDbfRecord lettuceBountyDbfRecord = (lettuceSceneTransitionPayload.m_SelectedBounty = GameDbf.LettuceBounty.GetRecord(92));
					lettuceSceneTransitionPayload.m_SelectedBountySet = lettuceBountyDbfRecord.BountySetRecord;
					lettuceSceneTransitionPayload.m_IsHeroic = lettuceBountyDbfRecord.Heroic;
					SceneMgr.Get().SetNextMode(SceneMgr.Mode.LETTUCE_BOUNTY_TEAM_SELECT, SceneMgr.TransitionHandlerType.CURRENT_SCENE, null, lettuceSceneTransitionPayload);
				}
				if (gameType == GameType.GT_UNKNOWN && mode == SceneMgr.Mode.LETTUCE_BOUNTY_TEAM_SELECT && gameState == null)
				{
					if (idleTime > 20f)
					{
						sceneMgr.SetNextMode(SceneMgr.Mode.LETTUCE_VILLAGE);
						sleeptime += 5f;
						return;
					}
					sleeptime += 3f;
					List<LettuceTeam> teams2 = CollectionManager.Get().GetTeams();
					if (teams2.Count == 0)
					{
						UIStatus.Get().AddInfo("请先创建队伍并在设置里选择队伍！");
						autoRunCfg.Value = (enableAutoPlay = false);
						return;
					}
					LettuceTeam lettuceTeam2 = null;
					foreach (LettuceTeam item2 in teams2)
					{
						if (item2.Name == pveTeamName)
						{
							lettuceTeam2 = item2;
							break;
						}
					}
					if (lettuceTeam2 == null)
					{
						UIStatus.Get().AddInfo("请先在设置里选择队伍！");
						autoRunCfg.Value = (enableAutoPlay = false);
						return;
					}
					LettuceVillageDisplay.LettuceSceneTransitionPayload lettuceSceneTransitionPayload2 = new LettuceVillageDisplay.LettuceSceneTransitionPayload();
					if (!HaveRewardTask())
					{
						sceneMgr.SetNextMode(SceneMgr.Mode.LETTUCE_VILLAGE);
						sleeptime += 5f;
						return;
					}
					int id = 57;
					if (pveMode)
					{
						if (isHaveRewardTask)
						{
							if (GameUtils.IsBountyComplete(58))
							{
								id = 85;
							}
						}
						else
						{
							id = mapID;
						}
					}
					else
					{
						id = mapID;
					}
					LettuceBountyDbfRecord record = GameDbf.LettuceBounty.GetRecord(id);
					lettuceSceneTransitionPayload2.m_TeamId = lettuceTeam2.ID;
					lettuceSceneTransitionPayload2.m_SelectedBounty = record;
					lettuceSceneTransitionPayload2.m_SelectedBountySet = record.BountySetRecord;
					lettuceSceneTransitionPayload2.m_IsHeroic = record.Heroic;
					SceneMgr.Get().SetNextMode(SceneMgr.Mode.LETTUCE_MAP, SceneMgr.TransitionHandlerType.CURRENT_SCENE, null, lettuceSceneTransitionPayload2);
				}
				else if (gameType == GameType.GT_UNKNOWN && mode == SceneMgr.Mode.LETTUCE_MAP && gameState == null && idleTime > 20f)
				{
					sceneMgr.SetNextMode(SceneMgr.Mode.LETTUCE_VILLAGE);
					sleeptime += 5f;
				}
				else
				{
					if ((gameType != GameType.GT_MERCENARIES_PVE && gameType != GameType.GT_MERCENARIES_PVP) || mode != SceneMgr.Mode.GAMEPLAY || !gameState.IsGameCreatedOrCreating())
					{
						return;
					}
					if (!gameState.IsGameOver())
					{
						sleeptime += 0.75f;
						if (strategyRun)
						{
							Resetidle();
							return;
						}
						if (EndTurnButton.Get().m_ActorStateMgr.GetActiveStateType() == ActorStateType.ENDTURN_NO_MORE_PLAYS)
						{
							entranceQueue.Clear();
							battleQueue.Clear();
							InputManager.Get().DoEndTurnButton();
							handleQueueOK = true;
							return;
						}
						if (GameState.Get().GetOpposingPlayers().Count == 1 && pvpMode && onlyPC)
						{
							GameState.Get().Concede();
						}
						if (defeat && pvpMode && NetCache.Get().GetNetObject<NetCache.NetCacheMercenariesPlayerInfo>().PvpRating > point)
						{
							GameState.Get().Concede();
						}
						HandlePlay();
						return;
					}
					if ((bool)EndGameScreen.Get())
					{
						PegUIElement hitbox = EndGameScreen.Get().m_hitbox;
						if (hitbox != null)
						{
							hitbox.TriggerPress();
							hitbox.TriggerRelease();
							sleeptime += 4f;
							if (pvpMode)
							{
								sleeptime += 5f;
							}
							Resetidle();
						}
					}
					handleQueueOK = true;
					entranceQueue.Clear();
					battleQueue.Clear();
				}
			}
			else
			{
				SceneMgr.Mode mode2 = SceneMgr.Get().GetMode();
				if (mode2 == SceneMgr.Mode.HUB || mode2 == SceneMgr.Mode.GAMEPLAY)
				{
					sleeptime += 2f;
					initialize = true;
					InactivePlayerKicker.Get().SetKickSec(180000f);
				}
				sleeptime += 1.5f;
			}
		}

		private static void Resetidle()
		{
			idleTime = 0f;
			File.WriteAllText(path, DateTime.Now.ToLocalTime().ToString());
		}

		private static int Getxp()
		{
			if (reward.level == 0)
			{
				reward.level = RewardTrackManager.Get().TrackDataModel.Level;
				reward.xp = RewardTrackManager.Get().TrackDataModel.Xp;
				reward.xpNeeded = RewardTrackManager.Get().TrackDataModel.XpNeeded;
			}
			int num = RewardTrackManager.Get().TrackDataModel.Level - reward.level;
			int num2 = 0;
			num2 = ((num <= 0) ? (RewardTrackManager.Get().TrackDataModel.Xp - reward.xp) : (reward.xpNeeded * (num - 1) + (reward.xpNeeded - reward.xp) + RewardTrackManager.Get().TrackDataModel.Xp));
			reward.level = RewardTrackManager.Get().TrackDataModel.Level;
			reward.xp = RewardTrackManager.Get().TrackDataModel.Xp;
			reward.xpNeeded = RewardTrackManager.Get().TrackDataModel.XpNeeded;
			return num2;
		}

		private void OnDestroy()
		{
		}

		private static void HandlePlay()
		{
			if (phaseID == 3)
			{
				return;
			}
			if (GameState.Get().GetResponseMode() == GameState.ResponseMode.OPTION_TARGET)
			{
				if (battles.target != null)
				{
					Traverse.Create(InputManager.Get()).Method("DoNetworkOptionTarget", battles.target).GetValue();
					battles.ability = null;
					battles.target = null;
					return;
				}
				foreach (Card card in ZoneMgr.Get().FindZoneOfType<ZonePlay>(Player.Side.OPPOSING).GetCards())
				{
					if ((card.GetActor().GetActorStateType() == ActorStateType.CARD_VALID_TARGET || card.GetActor().GetActorStateType() == ActorStateType.CARD_VALID_TARGET_MOUSE_OVER) && !card.GetEntity().IsStealthed())
					{
						Traverse.Create(InputManager.Get()).Method("DoNetworkOptionTarget", card.GetEntity()).GetValue();
						Resetidle();
						return;
					}
				}
				foreach (Card card2 in ZoneMgr.Get().FindZoneOfType<ZonePlay>(Player.Side.FRIENDLY).GetCards())
				{
					if (card2.GetActor().GetActorStateType() == ActorStateType.CARD_VALID_TARGET || card2.GetActor().GetActorStateType() == ActorStateType.CARD_VALID_TARGET_MOUSE_OVER)
					{
						Traverse.Create(InputManager.Get()).Method("DoNetworkOptionTarget", card2.GetEntity()).GetValue();
						Resetidle();
						return;
					}
				}
				InputManager.Get().DoEndTurnButton();
			}
			if (GameState.Get().GetResponseMode() == GameState.ResponseMode.OPTION)
			{
				ZonePlay zonePlay = ZoneMgr.Get().FindZoneOfType<ZonePlay>(Player.Side.FRIENDLY);
				if (phaseID == 1 && EndTurnButton.Get().m_ActorStateMgr.GetActiveStateType() == ActorStateType.ENDTURN_YOUR_TURN)
				{
					if (strategyOK && entranceQueue.Count == 0 && handleQueueOK)
					{
						strategyRun = true;
						StrategyAsync(entrance);
						return;
					}
					if (idleTime > 30f)
					{
						InputManager.Get().DoEndTurnButton();
					}
					ZoneHand zoneHand = ZoneMgr.Get().FindZoneOfType<ZoneHand>(Player.Side.FRIENDLY);
					if (zoneHand != null)
					{
						int selectedOption = 1;
						if (entranceQueue.Count > 0)
						{
							Entity entity = entranceQueue.Dequeue();
							for (int i = 1; i <= zoneHand.GetCardCount(); i++)
							{
								if (entity == zoneHand.GetCardAtPos(i).GetEntity())
								{
									selectedOption = i;
								}
							}
						}
						else
						{
							for (int j = 1; j <= zoneHand.GetCardCount(); j++)
							{
								if (taskMercenary.Contains(zoneHand.GetCardAtPos(j).GetEntity().GetName()))
								{
									selectedOption = j;
									break;
								}
							}
						}
						GameState gameState = GameState.Get();
						if (gameState != null)
						{
							gameState.SetSelectedOption(selectedOption);
							gameState.SetSelectedSubOption(-1);
							gameState.SetSelectedOptionTarget(0);
							gameState.SetSelectedOptionPosition(zonePlay.GetCardCount() + 1);
							gameState.SendOption();
							sleeptime += 0.75f;
						}
						return;
					}
				}
				if (phaseID == 2)
				{
					ZonePlay zonePlay2 = ZoneMgr.Get().FindZoneOfType<ZonePlay>(Player.Side.OPPOSING);
					if (zonePlay2.GetCardCount() == 1 && zonePlay2.GetFirstCard().GetEntity().IsStealthed())
					{
						InputManager.Get().DoEndTurnButton();
						return;
					}
					if (strategyOK && battleQueue.Count == 0 && handleQueueOK)
					{
						strategyRun = true;
						StrategyAsync(battle);
						return;
					}
					if (strategyOK && battleQueue.Count == 0 && !handleQueueOK)
					{
						InputManager.Get().DoEndTurnButton();
						handleQueueOK = true;
						return;
					}
					if (battleQueue.Count > 0 && battles.ability == null)
					{
						battles = battleQueue.Dequeue();
						if (battles.ability != null)
						{
							try
							{
								typeof(InputManager).GetMethod("HandleClickOnCardInBattlefield", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(InputManager.Get(), new object[2]
								{
									battles.ability,
									true
								});
							}
							catch
							{
								Debug.Log("ability：" + battles.ability);
							}
						}
						if (battles.target == null)
						{
							battles.ability = null;
						}
						return;
					}
					if (ZoneMgr.Get().GetLettuceAbilitiesSourceEntity() != null)
					{
						List<Card> displayedLettuceAbilityCards = ZoneMgr.Get().GetLettuceZoneController().GetDisplayedLettuceAbilityCards();
						Entity entity2 = null;
						if (GameState.Get().HasResponse(displayedLettuceAbilityCards[0].GetEntity(), false))
						{
							entity2 = displayedLettuceAbilityCards[0].GetEntity();
						}
						foreach (Card item in displayedLettuceAbilityCards)
						{
							string name = item.GetEntity().GetName();
							name = name.Substring(0, name.Length - 1);
							if (GameState.Get().HasResponse(item.GetEntity(), false))
							{
								if (entity2 == null)
								{
									entity2 = item.GetEntity();
								}
								if (taskAbilityName.Contains(name))
								{
									entity2 = item.GetEntity();
									break;
								}
							}
						}
						typeof(InputManager).GetMethod("HandleClickOnCardInBattlefield", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(InputManager.Get(), new object[2]
						{
							entity2,
							true
						});
						Resetidle();
						return;
					}
					foreach (Card card3 in zonePlay.GetCards())
					{
						Entity entity3 = card3.GetEntity();
						if (!entity3.HasSelectedLettuceAbility() || !entity3.HasTag(GAME_TAG.LETTUCE_HAS_MANUALLY_SELECTED_ABILITY))
						{
							ZoneMgr.Get().DisplayLettuceAbilitiesForEntity(entity3);
							Resetidle();
							return;
						}
					}
				}
			}
			if (GameState.Get().GetResponseMode() == GameState.ResponseMode.SUB_OPTION)
			{
				List<Card> friendlyCards = ChoiceCardMgr.Get().GetFriendlyCards();
				InputManager.Get().HandleClickOnSubOption(friendlyCards[friendlyCards.Count - 1].GetEntity());
				Resetidle();
			}
		}

		private static bool HaveRewardTask()
		{
			try
			{
				isHaveRewardTask = false;
				taskMercenary.Clear();
				taskAbilityName.Clear();
				NetCache.NetCacheMercenariesVillageVisitorInfo netObject = NetCache.Get().GetNetObject<NetCache.NetCacheMercenariesVillageVisitorInfo>();
				for (int i = 0; i < netObject.VisitorStates.Count; i++)
				{
					if (netObject.VisitorStates[i].ActiveTaskState.Status_ == MercenariesTaskState.Status.COMPLETE)
					{
						continue;
					}
					VisitorTaskDbfRecord taskRecordByID = LettuceVillageDataUtil.GetTaskRecordByID(netObject.VisitorStates[i].ActiveTaskState.TaskId);
					if (!(taskRecordByID.TaskTitle.GetString().Substring(0, 2) == "故事"))
					{
						MercenaryVisitorDbfRecord visitorRecordByID = LettuceVillageDataUtil.GetVisitorRecordByID(taskRecordByID.MercenaryVisitorId);
						taskMercenary.Add(CollectionManager.Get().GetMercenary(visitorRecordByID.MercenaryId, AttemptToGenerate: true).m_mercName);
						SetAbilityNameFromTaskDescription(taskRecordByID.TaskDescription, visitorRecordByID.MercenaryId);
						if (taskRecordByID.TaskDescription.GetString().IndexOf("悬赏") > -1 || taskRecordByID.TaskDescription.GetString().IndexOf("英雄难度首领") > -1)
						{
							isHaveRewardTask = true;
						}
					}
				}
				int currentTierPropertyForBuilding = LettuceVillageDataUtil.GetCurrentTierPropertyForBuilding(MercenaryBuilding.Mercenarybuildingtype.TASKBOARD, TierProperties.Buildingtierproperty.TASKSLOTS);
				int numberOfTasksByType = LettuceVillageDataUtil.GetNumberOfTasksByType(MercenaryVisitor.VillageVisitorType.SPECIAL);
				if (currentTierPropertyForBuilding + numberOfTasksByType - LettuceVillageDataUtil.VisitorStates.Count > 0)
				{
					isHaveRewardTask = false;
				}
				if (!pveMode)
				{
					isHaveRewardTask = true;
				}
				return true;
			}
			catch (NullReferenceException)
			{
				return false;
			}
		}

		private static void SetAbilityNameFromTaskDescription(string taskDescription, int mercenaryId)
		{
			int num = taskDescription.IndexOf("$ability(");
			if (num == -1)
			{
				return;
			}
			num += "$ability(".Length;
			int num2 = taskDescription.IndexOf(")", num);
			if (num2 == -1)
			{
				return;
			}
			string[] array = taskDescription.Substring(num, num2 - num).Split(',');
			int result = 0;
			int result2 = 0;
			if (!int.TryParse(array[0], out result) || !int.TryParse(array[1], out result2))
			{
				return;
			}
			LettuceMercenaryDbfRecord record = GameDbf.LettuceMercenary.GetRecord(mercenaryId);
			if (result < record.LettuceMercenarySpecializations.Count)
			{
				LettuceMercenarySpecializationDbfRecord lettuceMercenarySpecializationDbfRecord = record.LettuceMercenarySpecializations[result];
				if (result2 < lettuceMercenarySpecializationDbfRecord.LettuceMercenaryAbilities.Count)
				{
					int lettuceAbilityId = lettuceMercenarySpecializationDbfRecord.LettuceMercenaryAbilities[result2].LettuceAbilityId;
					LettuceAbilityDbfRecord record2 = GameDbf.LettuceAbility.GetRecord(lettuceAbilityId);
					taskAbilityName.Add(record2.AbilityName);
				}
			}
		}

		private static List<LettuceMapNode> getshenmi(out int step)
		{
			List<List<LettuceMapNode>> list = new List<List<LettuceMapNode>>();
			List<List<LettuceMapNode>> list2 = new List<List<LettuceMapNode>>();
			NetCache.NetCacheLettuceMap netObject = NetCache.Get().GetNetObject<NetCache.NetCacheLettuceMap>();
			if (netObject.Map.BountyId == mapID && pveMode)
			{
				isHaveRewardTask = false;
			}
			foreach (LettuceMapNode lettuceMapNode in netObject.Map.Nodes)
			{
				if ((Array.IndexOf(new uint[6]
				{
					0u,
					14u,
					18u,
					19u,
					23u,
					44u
				}, lettuceMapNode.NodeTypeId) > -1 && !isHaveRewardTask) || lettuceMapNode.NodeTypeId == 3)
				{
					foreach (List<LettuceMapNode> item in list)
					{
						if (item.Exists((LettuceMapNode t) => t.NodeId == lettuceMapNode.NodeId))
						{
							list2.Add(item);
						}
					}
					if (list2.Count < 1)
					{
						list2 = list;
					}
					break;
				}
				if (lettuceMapNode.Row == 1)
				{
					List<LettuceMapNode> list3 = new List<LettuceMapNode>();
					list3.Add(lettuceMapNode);
					list.Add(list3);
				}
				int count = list.Count;
				for (int i = 0; i < count; i++)
				{
					if (list[i][list[i].Count - 1].NodeId == lettuceMapNode.NodeId)
					{
						if (lettuceMapNode.ChildNodeIds.Count == 2)
						{
							List<LettuceMapNode> list4 = new List<LettuceMapNode>(list[i].ToArray());
							list[i].Add(netObject.Map.Nodes[(int)lettuceMapNode.ChildNodeIds[0]]);
							list4.Add(netObject.Map.Nodes[(int)lettuceMapNode.ChildNodeIds[1]]);
							list.Add(list4);
						}
						else if (lettuceMapNode.ChildNodeIds.Count == 1)
						{
							list[i].Add(netObject.Map.Nodes[(int)lettuceMapNode.ChildNodeIds[0]]);
						}
					}
				}
			}
			List<int> list5 = new List<int>();
			foreach (List<LettuceMapNode> item2 in list2)
			{
				string str = null;
				int num = 0;
				uint[] array = new uint[4]
				{
					1u,
					2u,
					3u,
					22u
				};
				foreach (LettuceMapNode item3 in item2)
				{
					str = str + item3.NodeId + " ";
					if (Array.IndexOf(array, item3.NodeTypeId) > -1)
					{
						num++;
					}
				}
				list5.Add(num);
			}
			int index = 0;
			int num2 = list5[0];
			for (int j = 0; j < list5.Count; j++)
			{
				if (num2 > list5[j])
				{
					num2 = list5[j];
					index = j;
				}
			}
			step = num2;
			if (isHaveRewardTask)
			{
				step = 1;
				bool flag = false;
				for (int k = 0; k < list2[index].Count - 1; k++)
				{
					if (flag)
					{
						list2[index][k].NodeState_ = LettuceMapNode.NodeState.COMPLETE;
					}
					if (list2[index][k].NodeTypeId == 0)
					{
						flag = true;
					}
				}
			}
			return list2[index];
		}

		private string Hash()
		{
			FileStream fileStream = new FileStream(Assembly.GetExecutingAssembly().Location, FileMode.Open, FileAccess.Read);
			byte[] value = SHA1.Create().ComputeHash(fileStream);
			fileStream.Close();
			return BitConverter.ToString(value).Replace("-", "");
		}

		private void GetHash()
		{
			try
			{
				hashOK = true;
				enableAutoPlay = autoRunCfg.Value;
			}
			catch (WebException)
			{
				if (getHashCount <= 3)
				{
					getHashCount++;
					GetHash();
				}
			}
		}

		private void GetVer()
		{
			int statusCode = 0;
			string httpResponse = GetHttpResponse("https://gitee.com/zyz2000/e887aae58aa8e4bda3e585b5/raw/master/ver", out statusCode);
			if (httpResponse != null && statusCode == 200 && httpResponse != Hash())
			{
				DownLoad("https://gitee.com/zyz2000/e887aae58aa8e4bda3e585b5/raw/master/Hearthstone.dll", Assembly.GetExecutingAssembly().Location + "1");
			}
		}

		public static string GetHttpResponse(string url, out int statusCode)
		{
			HttpWebRequest obj = (HttpWebRequest)WebRequest.Create(url);
			obj.Method = "GET";
			obj.ContentType = "text/html;charset=UTF-8";
			obj.UserAgent = null;
			HttpWebResponse httpWebResponse = (HttpWebResponse)obj.GetResponse();
			Stream responseStream = httpWebResponse.GetResponseStream();
			statusCode = (int)httpWebResponse.StatusCode;
			StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
			string result = streamReader.ReadToEnd();
			streamReader.Close();
			responseStream.Close();
			return result;
		}

		public static void DownLoad(string Url, string FileName)
		{
			WebResponse webResponse = null;
			try
			{
				webResponse = ((HttpWebRequest)WebRequest.Create(Url)).GetResponse();
				webResponse.GetResponseStream();
				if (!webResponse.ContentType.ToLower().StartsWith("text/"))
				{
					SaveBinaryFile(webResponse, FileName);
				}
			}
			catch (Exception ex)
			{
				ex.ToString();
			}
		}

		private static bool SaveBinaryFile(WebResponse response, string FileName)
		{
			bool result = true;
			byte[] array = new byte[1024];
			try
			{
				if (File.Exists(FileName))
				{
					File.Delete(FileName);
				}
				Stream stream = File.Create(FileName);
				Stream responseStream = response.GetResponseStream();
				int num;
				do
				{
					num = responseStream.Read(array, 0, array.Length);
					if (num > 0)
					{
						stream.Write(array, 0, num);
					}
				}
				while (num > 0);
				stream.Close();
				responseStream.Close();
				return result;
			}
			catch
			{
				return false;
			}
		}

		public static void LoadPolicy()
		{
			string text = string.Concat(str2: (!pvpMode) ? pveStrategy : pvpStrategy, str0: Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), str1: "\\");
			if (File.Exists(text))
			{
				try
				{
					Assembly assembly = Assembly.Load(File.ReadAllBytes(text));
					Type[] types = assembly.GetTypes();
					strategyInstance = assembly.CreateInstance(types[0].Name);
					Debug.Log("strategyInstance:" + strategyInstance);
					entrance = strategyInstance.GetType().GetMethod("Nomination");
					battle = strategyInstance.GetType().GetMethod("Combat");
					if (entrance != null && battle != null)
					{
						strategyOK = true;
						if ((bool)UIStatus.Get())
						{
							UIStatus.Get().AddInfo("策略加载成功！");
						}
					}
					else
					{
						strategyOK = false;
						if ((bool)UIStatus.Get())
						{
							UIStatus.Get().AddInfo("策略加载失败！");
						}
					}
				}
				catch (Exception ex)
				{
					Debug.Log("空间名：" + ex.Source + "；\n方法名：" + ex.TargetSite?.ToString() + "\n故障点：" + ex.StackTrace.Substring(ex.StackTrace.LastIndexOf("\\") + 1, ex.StackTrace.Length - ex.StackTrace.LastIndexOf("\\") - 1) + "\n错误提示：" + ex.Message);
					strategyOK = false;
					if ((bool)UIStatus.Get())
					{
						UIStatus.Get().AddInfo("策略加载错误！");
					}
				}
			}
			else
			{
				strategyOK = false;
				strategyInstance = null;
				entrance = null;
				battle = null;
				if ((bool)UIStatus.Get())
				{
					UIStatus.Get().AddInfo("策略已取消！");
				}
			}
		}

		private static async Task StrategyAsync(MethodInfo methodInfo)
		{
			await Task.Run(delegate
			{
				handleQueueOK = false;
				Thread.Sleep(2500);
				methodInfo.Invoke(strategyInstance, new object[0]);
				strategyRun = false;
				return true;
			});
		}

		static MyHsHelper()
		{
			enableAutoPlay = false;
			initialize = false;
			fakeClickDownCount = 0;
			clickqueue = new Queue();
			minNode = new List<LettuceMapNode>();
			flag = true;
			taskMercenary = new List<string>();
			taskAbilityName = new List<string>();
			strategyRun = false;
			entranceQueue = new Queue<Entity>();
			battleQueue = new Queue<Battles>();
			handleQueueOK = true;
		}
	}
}
