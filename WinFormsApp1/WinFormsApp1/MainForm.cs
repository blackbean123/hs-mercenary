using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Collections;
using System.Text;
using Newtonsoft.Json;

namespace WinFormsApp1
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
			mockConfig = new MockConfig();
			mockConfig.enableAutoPlay = true;
			mockConfig.pvpMode = false;
			mockConfig.pveMode = true;


		}

		public MockConfig mockConfig;

		public class MockConfig
		{
			public bool enableAutoPlay;
			public bool pvpMode;
			public bool pveMode;


		}

		public class ImportCfg 
		{
			public string teamList;
			public string abilityList;
			public string firstTarget;
			public string battlePolicy;
			public string whiteList;
			public string blackList;


		}

		private void autoButton_Click(object sender, EventArgs e)
		{
			//bool enableAutoPlay = (MyHsHelper.MyHsHelper.autoRunCfg.Value = !MyHsHelper.MyHsHelper.enableAutoPlay);
			//MyHsHelper.MyHsHelper.enableAutoPlay = enableAutoPlay;
			autoButton.Text = "自动佣兵：" + (mockConfig.enableAutoPlay ? "开" : "关");
		}

		private void modelButton_Click(object sender, EventArgs e)
		{
			// bool isPVP = (MyHsHelper.MyHsHelper.pvpCfg.Value = !MyHsHelper.MyHsHelper.pvpMode);
			// MyHsHelper.MyHsHelper.pvpMode = isPVP;
			// modelButton.Text = (MyHsHelper.MyHsHelper.pvpMode ? "PVP" : "PVE");
			// groupBoxPVP.Visible = MyHsHelper.MyHsHelper.pvpMode;
			// groupBoxPVE.Visible = !MyHsHelper.MyHsHelper.pvpMode;
			mockConfig.pvpMode = !mockConfig.pvpMode;
			mockConfig.pveMode = !mockConfig.pveMode;
			modelButton.Text = (mockConfig.pvpMode ? "PVP" : "PVE");
			//groupBoxPVE.Visible = mockConfig.pveMode;
			//groupBoxPVP.Visible = mockConfig.pvpMode;

		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			autoButton.Text = "自动佣兵：" + (mockConfig.enableAutoPlay ? "开" : "关");
			modelButton.Text = mockConfig.pvpMode ? "PVP" : "PVE";
			//groupBoxPVP.Visible = mockConfig.pvpMode;
			//groupBoxPVE.Visible = !mockConfig.pvpMode;
			List<string> teams = new List<string>(){ "初始队伍", "队伍1"};
			if (teams.Count == 0)
			{
				((Form)sender).Close();
			}
			foreach (string item in teams)
			{
				comboBoxTeamPVE.Items.Add(item);
				comboBoxTeamPVP.Items.Add(item);
			}
			
			comboBoxMode.SelectedIndex = comboBoxMode.Items.IndexOf(mockConfig.pveMode ? "任务模式" : "刷图模式");
			Label label = label5;
			bool visible = (comboBoxS.Visible = mockConfig.pveMode);
			label.Visible = visible;

			comboBoxStrategyPVP.Items.Add("default");
			comboBoxStrategyPVE.Items.Add("PVE");
			comboBoxStrategyPVP.Items.Add("");
			comboBoxStrategyPVE.Items.Add("");
			
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
			
		}

		private void comboBoxTeamPVE_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

		private void comboBoxMode_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBoxMode.SelectedItem.ToString() == "任务模式")
			{
				bool pVEMode = mockConfig.pveMode;
				Label label = label5;
				pVEMode = (comboBoxS.Visible = true);
				label.Visible = pVEMode;
			}
			else
			{
				bool pVEMode = mockConfig.pveMode;
				Label label2 = label5;
				pVEMode = (comboBoxS.Visible = false);
				label2.Visible = pVEMode;
			}
		}

		private void comboBoxTeamPVP_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

		private void checkBoxonlypc_CheckedChanged(object sender, EventArgs e)
		{
			
		}

		private void checkBoxautoConcede_CheckedChanged(object sender, EventArgs e)
		{
			
		}

		private void Concedeline_TextChanged(object sender, EventArgs e)
		{
			if (ConcedeLine.Text.Length > 0)
			{
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
			
		}

		private void SwitchLine_TextChanged(object sender, EventArgs e)
		{
			if (SwitchLine.Text.Length > 0)
			{
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
		}

		private void comboBoxStrategyPVP_SelectedIndexChanged(object sender, EventArgs e)
		{
			
		}

		private void comboBoxStrategyPVE_SelectedIndexChanged(object sender, EventArgs e)
		{
			
		}

        private void whiteListBox_TextChanged(object sender, EventArgs e)
        {

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
				//var cfg = JsonConvert.DeserializeObject<dynamic>(json);
				if (string.IsNullOrEmpty(json)) 
				{
					MessageBox.Show("文件内容为空");
					return;
				}
				
				string[] content = json.Split("\r\n");
				Hashtable map = new System.Collections.Hashtable();
				foreach (string item in content)
				{
					if (string.IsNullOrEmpty(item)) 
					{
						continue;
					}
					
					string[] itemCfg = item.Replace("：", ":").Split(":");
					if (itemCfg.Length < 2)
					{
						continue;
					}
					map.Add(itemCfg[0], itemCfg[1]);
					
				}

				if (map.Count != 0) 
				{
					teamListBox.Text = map["teamList"] as string;
					abilityListBox.Text = map["abilityList"] as string;
					firstTargetBox.Text = map["firstTarget"] as string;
					whiteListBox.Text = map["whiteList"] as string;
					blackListBox.Text = map["blackLisy"] as string;
				}
				
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
        }
    }
}