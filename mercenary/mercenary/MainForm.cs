using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using BepInEx.Configuration;
using MyHsHelper;
using System.Text;

namespace Hearthstone
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

		private void autoButton_Click(object sender, EventArgs e)
		{
			bool enableAutoPlay = (MyHsHelper.MyHsHelper.autoRunCfg.Value = !MyHsHelper.MyHsHelper.enableAutoPlay);
			MyHsHelper.MyHsHelper.enableAutoPlay = enableAutoPlay;
			autoButton.Text = "自动佣兵：" + (MyHsHelper.MyHsHelper.enableAutoPlay ? "开" : "关");
		}

		private void modelButton_Click(object sender, EventArgs e)
		{
			bool isPVP = (MyHsHelper.MyHsHelper.pvpCfg.Value = !MyHsHelper.MyHsHelper.pvpMode);
			MyHsHelper.MyHsHelper.pvpMode = isPVP;
			modelButton.Text = (MyHsHelper.MyHsHelper.pvpMode ? "PVP" : "PVE");
			//groupBoxPVP.Visible = MyHsHelper.MyHsHelper.pvpMode;
			//groupBoxPVE.Visible = !MyHsHelper.MyHsHelper.pvpMode;
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			autoButton.Text = "自动佣兵：" + (MyHsHelper.MyHsHelper.enableAutoPlay ? "开" : "关");
			modelButton.Text = (MyHsHelper.MyHsHelper.pvpMode ? "PVP" : "PVE");
			//groupBoxPVP.Visible = MyHsHelper.MyHsHelper.pvpMode;
			//groupBoxPVE.Visible = !MyHsHelper.MyHsHelper.pvpMode;
			List<LettuceTeam> teams = CollectionManager.Get().GetTeams();
			if (teams.Count == 0)
			{
				SceneMgr.Get().SetNextMode(SceneMgr.Mode.LETTUCE_VILLAGE);
				UIStatus.Get().AddInfo("请先进入佣兵场景或者创建佣兵队伍");
				((Form)sender).Close();
			}
			foreach (LettuceTeam item in teams)
			{
				comboBoxTeamPVE.Items.Add(item.Name);
				comboBoxTeamPVP.Items.Add(item.Name);
			}
			comboBoxTeamPVE.SelectedIndex = comboBoxTeamPVE.Items.IndexOf(MyHsHelper.MyHsHelper.pveTeamName);
			comboBoxTeamPVP.SelectedIndex = comboBoxTeamPVP.Items.IndexOf(MyHsHelper.MyHsHelper.pvpTeamName);
			checkBoxonlypc.Checked = MyHsHelper.MyHsHelper.onlyPC;
			checkBoxautoConcede.Checked = MyHsHelper.MyHsHelper.defeat;
			ConcedeLine.Text = MyHsHelper.MyHsHelper.point.ToString();
			checkBoxSwitchPVE.Checked = MyHsHelper.MyHsHelper.autoSwitch;
			SwitchLine.Text = MyHsHelper.MyHsHelper.switchLine.ToString();
			comboBoxMode.SelectedIndex = comboBoxMode.Items.IndexOf(MyHsHelper.MyHsHelper.pveMode ? "任务模式" : "刷图模式");
			Label label = label5;
			bool visible = (comboBoxS.Visible = MyHsHelper.MyHsHelper.pveMode);
			label.Visible = visible;
			comboBoxS.SelectedItem = MyHsHelper.MyHsHelper.pveStep.ToString();
			for (int i = 57; i < 200; i++)
			{
				LettuceBountyDbfRecord record = GameDbf.LettuceBounty.GetRecord(i);
				if (record != null)
				{
					comboBoxMap.Items.Add(i + (record.Heroic ? " H" : " ") + record.BountySetRecord.Name.GetString() + " " + record.FinalBossCardRecord.Name.GetString());
					if (i == MyHsHelper.MyHsHelper.mapID)
					{
						comboBoxMap.SelectedIndex = comboBoxMap.Items.Count - 1;
					}
				}
			}
			FileInfo[] files = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).GetFiles("*.dll");
			foreach (FileInfo fileInfo in files)
			{
				if (!(Path.GetFileName(Assembly.GetExecutingAssembly().Location) == fileInfo.Name) && !(Path.GetFileName(Assembly.GetExecutingAssembly().Location) + "1" == fileInfo.Name))
				{
					comboBoxStrategyPVP.Items.Add(fileInfo.Name);
					comboBoxStrategyPVE.Items.Add(fileInfo.Name);
					if (fileInfo.Name == MyHsHelper.MyHsHelper.pvpStrategy)
					{
						comboBoxStrategyPVP.SelectedIndex = comboBoxStrategyPVP.Items.Count - 1;
					}
					if (fileInfo.Name == MyHsHelper.MyHsHelper.pveStrategy)
					{
						comboBoxStrategyPVE.SelectedIndex = comboBoxStrategyPVE.Items.Count - 1;
					}
				}
			}
			comboBoxStrategyPVP.Items.Add("");
			comboBoxStrategyPVE.Items.Add("");
			if (comboBoxStrategyPVP.Items.Count == 1)
			{
				ConfigEntry<string> strategyPVP = MyHsHelper.MyHsHelper.pvpPolicyCfg;
				string strategyPVE = (MyHsHelper.MyHsHelper.pvePolicyCfg.Value = null);
				string text3 = (MyHsHelper.MyHsHelper.pvpStrategy = (strategyPVP.Value = (MyHsHelper.MyHsHelper.pveStrategy = strategyPVE)));
			}
			Form1_LoadComplete();
		}

		private void Form1_LoadComplete()
		{
			comboBoxTeamPVP.SelectedIndexChanged += comboBoxTeamPVP_SelectedIndexChanged;
			ConcedeLine.KeyPress += Concedeline_KeyPress;
			checkBoxautoConcede.CheckedChanged += checkBoxautoConcede_CheckedChanged;
			checkBoxonlypc.CheckedChanged += checkBoxonlypc_CheckedChanged;
			comboBoxS.SelectedIndexChanged += comboBoxS_SelectedIndexChanged;
			comboBoxMap.SelectedIndexChanged += comboBoxMap_SelectedIndexChanged;
			comboBoxMode.SelectedIndexChanged += comboBoxMode_SelectedIndexChanged;
			comboBoxTeamPVE.SelectedIndexChanged += comboBoxTeamPVE_SelectedIndexChanged;
			comboBoxStrategyPVP.SelectedIndexChanged += comboBoxStrategyPVP_SelectedIndexChanged;
			comboBoxStrategyPVE.SelectedIndexChanged += comboBoxStrategyPVE_SelectedIndexChanged;
		}

		private void comboBoxMap_SelectedIndexChanged(object sender, EventArgs e)
		{
			string[] array = comboBoxMap.SelectedItem.ToString().Split(' ');
			if (array.Length != 0)
			{
				int num = Convert.ToInt32(array[0]);
				int num3 = (MyHsHelper.MyHsHelper.mapID = (MyHsHelper.MyHsHelper.mapIdCfg.Value = num));
			}
		}

		private void comboBoxTeamPVE_SelectedIndexChanged(object sender, EventArgs e)
		{
			MyHsHelper.MyHsHelper.pveTeamName = MyHsHelper.MyHsHelper.pveTeamCfg.Value = comboBoxTeamPVE.SelectedItem.ToString();
		}

		private void comboBoxMode_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBoxMode.SelectedItem.ToString() == "任务模式")
			{
				bool pVEMode = (MyHsHelper.MyHsHelper.pveModeCfg.Value = true);
				MyHsHelper.MyHsHelper.pveMode = pVEMode;
				Label label = label5;
				pVEMode = (comboBoxS.Visible = true);
				label.Visible = pVEMode;
			}
			else
			{
				bool pVEMode = (MyHsHelper.MyHsHelper.pveModeCfg.Value = false);
				MyHsHelper.MyHsHelper.pveMode = pVEMode;
				Label label2 = label5;
				pVEMode = (comboBoxS.Visible = false);
				label2.Visible = pVEMode;
			}
		}

		private void comboBoxTeamPVP_SelectedIndexChanged(object sender, EventArgs e)
		{
			MyHsHelper.MyHsHelper.pvpTeamName = MyHsHelper.MyHsHelper.pvpTeamCfg.Value = comboBoxTeamPVP.SelectedItem.ToString();
		}

		private void checkBoxonlypc_CheckedChanged(object sender, EventArgs e)
		{
			bool onlyPC = (MyHsHelper.MyHsHelper.onlyPcCfg.Value = checkBoxonlypc.Checked);
			MyHsHelper.MyHsHelper.onlyPC = onlyPC;
		}

		private void checkBoxautoConcede_CheckedChanged(object sender, EventArgs e)
		{
			bool 认输 = (MyHsHelper.MyHsHelper.autoConcedeCfg.Value = checkBoxautoConcede.Checked);
			MyHsHelper.MyHsHelper.defeat = 认输;
		}

		private void Concedeline_TextChanged(object sender, EventArgs e)
		{
			if (ConcedeLine.Text.Length > 0)
			{
				int num2 = (MyHsHelper.MyHsHelper.point = (MyHsHelper.MyHsHelper.concedelineCfg.Value = Convert.ToInt32(ConcedeLine.Text)));
			}
		}

		private void Concedeline_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsNumber(e.KeyChar) && e.KeyChar != '\b')
			{
				e.Handled = true;
			}
		}

		private void checkBoxSwitchPVE_CheckedChanged(object sender, EventArgs e)
		{
			bool 自动切换 = (MyHsHelper.MyHsHelper.autoSwitchCfg.Value = checkBoxSwitchPVE.Checked);
			MyHsHelper.MyHsHelper.autoSwitch = 自动切换;
		}

		private void SwitchLine_TextChanged(object sender, EventArgs e)
		{
			if (SwitchLine.Text.Length > 0)
			{
				int num2 = (MyHsHelper.MyHsHelper.switchLine = (MyHsHelper.MyHsHelper.switchLineCfg.Value = Convert.ToInt32(SwitchLine.Text)));
			}
		}

		private void SwitchLine_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsNumber(e.KeyChar) && e.KeyChar != '\b')
			{
				e.Handled = true;
			}
		}

		private void comboBoxS_SelectedIndexChanged(object sender, EventArgs e)
		{
			int num2 = (MyHsHelper.MyHsHelper.pveStep = (MyHsHelper.MyHsHelper.stepCfg.Value = Convert.ToInt32(comboBoxS.SelectedItem.ToString())));
		}

		private void comboBoxStrategyPVP_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (MyHsHelper.MyHsHelper.pvpStrategy != comboBoxStrategyPVP.SelectedItem.ToString())
			{
				string text2 = (MyHsHelper.MyHsHelper.pvpStrategy = (MyHsHelper.MyHsHelper.pvpPolicyCfg.Value = comboBoxStrategyPVP.SelectedItem.ToString()));
				MyHsHelper.MyHsHelper.LoadPolicy();
			}
		}

		private void comboBoxStrategyPVE_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (MyHsHelper.MyHsHelper.pveStrategy != comboBoxStrategyPVE.SelectedItem.ToString())
			{
				string text2 = (MyHsHelper.MyHsHelper.pveStrategy = (MyHsHelper.MyHsHelper.pvePolicyCfg.Value = comboBoxStrategyPVE.SelectedItem.ToString()));
				MyHsHelper.MyHsHelper.LoadPolicy();
			}
		}

		private void whiteListBox_TextChanged(object sender, EventArgs e)
		{
			if (whiteListBox.Text.Length > 0)
			{
				MyHsHelper.MyHsHelper.whitelist = MyHsHelper.MyHsHelper.whitelistCfg.Value = Convert.ToString(whiteListBox.Text);
			}
		}


		private void importButton_Click(object sender, EventArgs e)
        {
			string path;
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.ShowDialog();
			path = ofd.FileName;
			try
			{
			    FileStream fs = new FileStream(path, FileMode.Open);
				StreamReader sr = new StreamReader(fs, Encoding.Default);
				string json = sr.ReadToEnd();
				if (string.IsNullOrEmpty(json))
				{
					MessageBox.Show("文件内容为空");
					return;
				}

				string[] content = json.Split("\r\n".ToCharArray());
				Hashtable map = new System.Collections.Hashtable();
				foreach (string item in content)
				{
					if (string.IsNullOrEmpty(item))
					{
						continue;
					}

					string[] itemCfg = item.Replace("：", ":").Split(new char[] {':'});
					if (itemCfg.Length < 2)
					{
						continue;
					}
					map.Add(itemCfg[0], itemCfg[1]);

				}

				if (map.Count != 0)
				{
					MyHsHelper.MyHsHelper.teamList = MyHsHelper.MyHsHelper.teamListCfg.Value = teamListBox.Text = map["teamList"] as string;
					MyHsHelper.MyHsHelper.abilityList = MyHsHelper.MyHsHelper.abilityListCfg.Value = abilityListBox.Text = map["abilityList"] as string;
					MyHsHelper.MyHsHelper.firstTarget = MyHsHelper.MyHsHelper.firstTargetCfg.Value = firstTargetBox.Text = map["firstTarget"] as string;
					MyHsHelper.MyHsHelper.whitelist = MyHsHelper.MyHsHelper.whitelistCfg.Value = whiteListBox.Text = map["whiteList"] as string;
					MyHsHelper.MyHsHelper.blacklist = MyHsHelper.MyHsHelper.blacklistCfg.Value = blackListBox.Text = map["blackList"] as string;
				}

			}
			catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}

		}

        private void blackListBox_TextChanged(object sender, EventArgs e)
        {
			if (blackListBox.Text.Length > 0)
			{
				MyHsHelper.MyHsHelper.blacklist = MyHsHelper.MyHsHelper.blacklistCfg.Value = Convert.ToString(blackListBox.Text);
			}
		}

        private void teamListBox_TextChanged(object sender, EventArgs e)
        {
			if (teamListBox.Text.Length > 0)
			{
				MyHsHelper.MyHsHelper.teamList = MyHsHelper.MyHsHelper.teamListCfg.Value = Convert.ToString(teamListBox.Text);
				MessageBox.Show(Convert.ToString(teamListBox.Text));
			}
		}

        private void abilityListBox_TextChanged(object sender, EventArgs e)
        {
			if (abilityListBox.Text.Length > 0)
			{
				MyHsHelper.MyHsHelper.abilityList = MyHsHelper.MyHsHelper.abilityListCfg.Value = Convert.ToString(abilityListBox.Text);
			}
		}

        private void firstTargetBox_TextChanged(object sender, EventArgs e)
        {
			if (firstTargetBox.Text.Length > 0)
			{
				MyHsHelper.MyHsHelper.firstTarget = MyHsHelper.MyHsHelper.firstTargetCfg.Value = Convert.ToString(firstTargetBox.Text);
			}
		}

        private void battlePolicyCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
			MyHsHelper.MyHsHelper.battlePolicy = MyHsHelper.MyHsHelper.battlePolicyCfg.Value = battlePolicyCombo.SelectedItem.ToString();
        }
    }
}
