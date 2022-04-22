using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Hearthstone
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


        public Button autoButton;

        private Button modelButton;

        private GroupBox groupBoxPVP;

        private TextBox ConcedeLine;

        private System.Windows.Forms.CheckBox checkBoxautoConcede;

        private System.Windows.Forms.CheckBox checkBoxonlypc;

        private GroupBox groupBoxPVE;

        private Label label1;

        private ComboBox comboBoxTeamPVP;

        private Label label4;

        private Label label3;

        private ComboBox comboBoxTeamPVE;

        private Label label2;

        private ComboBox comboBoxMode;

        private ComboBox comboBoxMap;

        private ComboBox comboBoxS;

        private Label label5;

        private ComboBox comboBoxStrategyPVP;

        private Label label7;

        private ComboBox comboBoxStrategyPVE;

        private Label label8;

        private TextBox SwitchLine;

        private System.Windows.Forms.CheckBox checkBoxSwitchPVE;

        private TextBox whiteListBox;

        private TextBox blackListBox;

        private Label whiteListLabel;

        private Label blackListLabel;

        private Label teamListLabel;

        private Label abilityListLabel;

        private TextBox teamListBox;

        private TextBox abilityListBox;

        private Label firstTargetLabel;

        private TextBox firstTargetBox;

        private Label battlePolicyLabel;

        private ComboBox battlePolicyCombo;

        private Button importButton;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.autoButton = new System.Windows.Forms.Button();
            this.modelButton = new System.Windows.Forms.Button();
            this.groupBoxPVP = new System.Windows.Forms.GroupBox();
            this.SwitchLine = new System.Windows.Forms.TextBox();
            this.checkBoxSwitchPVE = new System.Windows.Forms.CheckBox();
            this.comboBoxStrategyPVP = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBoxTeamPVP = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ConcedeLine = new System.Windows.Forms.TextBox();
            this.checkBoxautoConcede = new System.Windows.Forms.CheckBox();
            this.checkBoxonlypc = new System.Windows.Forms.CheckBox();
            this.groupBoxPVE = new System.Windows.Forms.GroupBox();
            this.comboBoxStrategyPVE = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.comboBoxS = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.blackListLabel = new System.Windows.Forms.Label();
            this.comboBoxMap = new System.Windows.Forms.ComboBox();
            this.comboBoxMode = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxTeamPVE = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.whiteListLabel = new System.Windows.Forms.Label();
            this.whiteListBox = new System.Windows.Forms.TextBox();
            this.blackListBox = new System.Windows.Forms.TextBox();
            this.teamListLabel = new System.Windows.Forms.Label();
            this.abilityListLabel = new System.Windows.Forms.Label();
            this.teamListBox = new System.Windows.Forms.TextBox();
            this.abilityListBox = new System.Windows.Forms.TextBox();
            this.firstTargetLabel = new System.Windows.Forms.Label();
            this.firstTargetBox = new System.Windows.Forms.TextBox();
            this.battlePolicyLabel = new System.Windows.Forms.Label();
            this.battlePolicyCombo = new System.Windows.Forms.ComboBox();
            this.importButton = new System.Windows.Forms.Button();
            this.groupBoxPVP.SuspendLayout();
            this.groupBoxPVE.SuspendLayout();
            this.SuspendLayout();
            // 
            // autoButton
            // 
            this.autoButton.Location = new System.Drawing.Point(8, 8);
            this.autoButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.autoButton.Name = "autoButton";
            this.autoButton.Size = new System.Drawing.Size(95, 31);
            this.autoButton.TabIndex = 0;
            this.autoButton.Text = "自动佣兵：开";
            this.autoButton.UseVisualStyleBackColor = true;
            this.autoButton.Click += new System.EventHandler(this.autoButton_Click);
            // 
            // modelButton
            // 
            this.modelButton.Location = new System.Drawing.Point(107, 8);
            this.modelButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.modelButton.Name = "modelButton";
            this.modelButton.Size = new System.Drawing.Size(94, 31);
            this.modelButton.TabIndex = 1;
            this.modelButton.Text = "PVP";
            this.modelButton.UseVisualStyleBackColor = true;
            this.modelButton.Click += new System.EventHandler(this.modelButton_Click);
            // 
            // groupBoxPVP
            // 
            this.groupBoxPVP.Controls.Add(this.SwitchLine);
            this.groupBoxPVP.Controls.Add(this.checkBoxSwitchPVE);
            this.groupBoxPVP.Controls.Add(this.comboBoxStrategyPVP);
            this.groupBoxPVP.Controls.Add(this.label7);
            this.groupBoxPVP.Controls.Add(this.comboBoxTeamPVP);
            this.groupBoxPVP.Controls.Add(this.label1);
            this.groupBoxPVP.Controls.Add(this.ConcedeLine);
            this.groupBoxPVP.Controls.Add(this.checkBoxautoConcede);
            this.groupBoxPVP.Controls.Add(this.checkBoxonlypc);
            this.groupBoxPVP.Location = new System.Drawing.Point(225, 133);
            this.groupBoxPVP.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBoxPVP.Name = "groupBoxPVP";
            this.groupBoxPVP.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBoxPVP.Size = new System.Drawing.Size(193, 149);
            this.groupBoxPVP.TabIndex = 2;
            this.groupBoxPVP.TabStop = false;
            this.groupBoxPVP.Text = "PVP";
            // 
            // SwitchLine
            // 
            this.SwitchLine.Location = new System.Drawing.Point(148, 87);
            this.SwitchLine.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.SwitchLine.MaxLength = 5;
            this.SwitchLine.Name = "SwitchLine";
            this.SwitchLine.Size = new System.Drawing.Size(36, 21);
            this.SwitchLine.TabIndex = 17;
            this.SwitchLine.Text = "14000";
            this.SwitchLine.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SwitchLine.TextChanged += new System.EventHandler(this.SwitchLine_TextChanged);
            this.SwitchLine.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SwitchLine_KeyPress);
            // 
            // checkBoxSwitchPVE
            // 
            this.checkBoxSwitchPVE.AutoSize = true;
            this.checkBoxSwitchPVE.Location = new System.Drawing.Point(11, 88);
            this.checkBoxSwitchPVE.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.checkBoxSwitchPVE.Name = "checkBoxSwitchPVE";
            this.checkBoxSwitchPVE.Size = new System.Drawing.Size(138, 16);
            this.checkBoxSwitchPVE.TabIndex = 16;
            this.checkBoxSwitchPVE.Text = "分数达到时切换至PVE";
            this.checkBoxSwitchPVE.UseVisualStyleBackColor = true;
            this.checkBoxSwitchPVE.CheckedChanged += new System.EventHandler(this.checkBoxSwitchPVE_CheckedChanged);
            // 
            // comboBoxStrategyPVP
            // 
            this.comboBoxStrategyPVP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStrategyPVP.DropDownWidth = 242;
            this.comboBoxStrategyPVP.FormattingEnabled = true;
            this.comboBoxStrategyPVP.Location = new System.Drawing.Point(73, 119);
            this.comboBoxStrategyPVP.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboBoxStrategyPVP.Name = "comboBoxStrategyPVP";
            this.comboBoxStrategyPVP.Size = new System.Drawing.Size(111, 20);
            this.comboBoxStrategyPVP.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 121);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 14;
            this.label7.Text = "PVP策略:";
            // 
            // comboBoxTeamPVP
            // 
            this.comboBoxTeamPVP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTeamPVP.FormattingEnabled = true;
            this.comboBoxTeamPVP.Location = new System.Drawing.Point(73, 17);
            this.comboBoxTeamPVP.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboBoxTeamPVP.Name = "comboBoxTeamPVP";
            this.comboBoxTeamPVP.Size = new System.Drawing.Size(96, 20);
            this.comboBoxTeamPVP.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 22);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "选择队伍:";
            // 
            // ConcedeLine
            // 
            this.ConcedeLine.Location = new System.Drawing.Point(148, 64);
            this.ConcedeLine.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ConcedeLine.MaxLength = 5;
            this.ConcedeLine.Name = "ConcedeLine";
            this.ConcedeLine.Size = new System.Drawing.Size(36, 21);
            this.ConcedeLine.TabIndex = 1;
            this.ConcedeLine.Text = "6000";
            this.ConcedeLine.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ConcedeLine.TextChanged += new System.EventHandler(this.Concedeline_TextChanged);
            // 
            // checkBoxautoConcede
            // 
            this.checkBoxautoConcede.AutoSize = true;
            this.checkBoxautoConcede.Location = new System.Drawing.Point(11, 65);
            this.checkBoxautoConcede.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.checkBoxautoConcede.Name = "checkBoxautoConcede";
            this.checkBoxautoConcede.Size = new System.Drawing.Size(144, 16);
            this.checkBoxautoConcede.TabIndex = 0;
            this.checkBoxautoConcede.Text = "自动投降：当分数高于";
            this.checkBoxautoConcede.UseVisualStyleBackColor = true;
            // 
            // checkBoxonlypc
            // 
            this.checkBoxonlypc.AutoSize = true;
            this.checkBoxonlypc.Location = new System.Drawing.Point(11, 43);
            this.checkBoxonlypc.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.checkBoxonlypc.Name = "checkBoxonlypc";
            this.checkBoxonlypc.Size = new System.Drawing.Size(72, 16);
            this.checkBoxonlypc.TabIndex = 0;
            this.checkBoxonlypc.Text = "只打电脑";
            this.checkBoxonlypc.UseVisualStyleBackColor = true;
            // 
            // groupBoxPVE
            // 
            this.groupBoxPVE.Controls.Add(this.comboBoxStrategyPVE);
            this.groupBoxPVE.Controls.Add(this.label8);
            this.groupBoxPVE.Controls.Add(this.comboBoxS);
            this.groupBoxPVE.Controls.Add(this.label5);
            this.groupBoxPVE.Controls.Add(this.blackListLabel);
            this.groupBoxPVE.Controls.Add(this.comboBoxMap);
            this.groupBoxPVE.Controls.Add(this.comboBoxMode);
            this.groupBoxPVE.Controls.Add(this.label4);
            this.groupBoxPVE.Controls.Add(this.label3);
            this.groupBoxPVE.Controls.Add(this.comboBoxTeamPVE);
            this.groupBoxPVE.Controls.Add(this.label2);
            this.groupBoxPVE.Controls.Add(this.whiteListLabel);
            this.groupBoxPVE.Controls.Add(this.whiteListBox);
            this.groupBoxPVE.Controls.Add(this.blackListBox);
            this.groupBoxPVE.Location = new System.Drawing.Point(7, 133);
            this.groupBoxPVE.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBoxPVE.Name = "groupBoxPVE";
            this.groupBoxPVE.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBoxPVE.Size = new System.Drawing.Size(195, 230);
            this.groupBoxPVE.TabIndex = 3;
            this.groupBoxPVE.TabStop = false;
            this.groupBoxPVE.Text = "PVE";
            // 
            // comboBoxStrategyPVE
            // 
            this.comboBoxStrategyPVE.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStrategyPVE.DropDownWidth = 242;
            this.comboBoxStrategyPVE.FormattingEnabled = true;
            this.comboBoxStrategyPVE.Location = new System.Drawing.Point(70, 202);
            this.comboBoxStrategyPVE.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboBoxStrategyPVE.Name = "comboBoxStrategyPVE";
            this.comboBoxStrategyPVE.Size = new System.Drawing.Size(111, 20);
            this.comboBoxStrategyPVE.TabIndex = 15;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 203);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 14;
            this.label8.Text = "PVE策略:";
            // 
            // comboBoxS
            // 
            this.comboBoxS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxS.FormattingEnabled = true;
            this.comboBoxS.Items.AddRange(new object[] {
            "2",
            "3",
            "4",
            "5"});
            this.comboBoxS.Location = new System.Drawing.Point(148, 152);
            this.comboBoxS.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboBoxS.Name = "comboBoxS";
            this.comboBoxS.Size = new System.Drawing.Size(33, 20);
            this.comboBoxS.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 154);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(149, 12);
            this.label5.TabIndex = 11;
            this.label5.Text = "神秘选项怪物节点不超过：";
            // 
            // blackListLabel
            // 
            this.blackListLabel.Location = new System.Drawing.Point(5, 61);
            this.blackListLabel.Name = "blackListLabel";
            this.blackListLabel.Size = new System.Drawing.Size(73, 16);
            this.blackListLabel.TabIndex = 17;
            this.blackListLabel.Text = "宝藏黑名单:";
            // 
            // comboBoxMap
            // 
            this.comboBoxMap.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMap.DropDownWidth = 242;
            this.comboBoxMap.FormattingEnabled = true;
            this.comboBoxMap.Location = new System.Drawing.Point(70, 174);
            this.comboBoxMap.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboBoxMap.Name = "comboBoxMap";
            this.comboBoxMap.Size = new System.Drawing.Size(111, 20);
            this.comboBoxMap.TabIndex = 10;
            // 
            // comboBoxMode
            // 
            this.comboBoxMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMode.FormattingEnabled = true;
            this.comboBoxMode.Items.AddRange(new object[] {
            "任务模式",
            "刷图模式"});
            this.comboBoxMode.Location = new System.Drawing.Point(70, 129);
            this.comboBoxMode.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboBoxMode.Name = "comboBoxMode";
            this.comboBoxMode.Size = new System.Drawing.Size(111, 20);
            this.comboBoxMode.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 133);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "选择模式:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 179);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "选择地图:";
            // 
            // comboBoxTeamPVE
            // 
            this.comboBoxTeamPVE.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTeamPVE.FormattingEnabled = true;
            this.comboBoxTeamPVE.Location = new System.Drawing.Point(70, 104);
            this.comboBoxTeamPVE.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboBoxTeamPVE.Name = "comboBoxTeamPVE";
            this.comboBoxTeamPVE.Size = new System.Drawing.Size(111, 20);
            this.comboBoxTeamPVE.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 106);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "选择队伍:";
            // 
            // whiteListLabel
            // 
            this.whiteListLabel.Location = new System.Drawing.Point(6, 16);
            this.whiteListLabel.Name = "whiteListLabel";
            this.whiteListLabel.Size = new System.Drawing.Size(173, 16);
            this.whiteListLabel.TabIndex = 16;
            this.whiteListLabel.Text = "优先宝藏：";
            // 
            // whiteListBox
            // 
            this.whiteListBox.Location = new System.Drawing.Point(7, 38);
            this.whiteListBox.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.whiteListBox.Name = "whiteListBox";
            this.whiteListBox.Size = new System.Drawing.Size(174, 21);
            this.whiteListBox.TabIndex = 5;
            this.whiteListBox.TextChanged += new System.EventHandler(this.whiteListBox_TextChanged);
            // 
            // blackListBox
            // 
            this.blackListBox.Location = new System.Drawing.Point(7, 79);
            this.blackListBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.blackListBox.Name = "blackListBox";
            this.blackListBox.Size = new System.Drawing.Size(174, 21);
            this.blackListBox.TabIndex = 6;
            this.blackListBox.TextChanged += new System.EventHandler(this.blackListBox_TextChanged);
            // 
            // teamListLabel
            // 
            this.teamListLabel.Location = new System.Drawing.Point(8, 41);
            this.teamListLabel.Name = "teamListLabel";
            this.teamListLabel.Size = new System.Drawing.Size(86, 16);
            this.teamListLabel.TabIndex = 2;
            this.teamListLabel.Text = "登场顺序：";
            // 
            // abilityListLabel
            // 
            this.abilityListLabel.Location = new System.Drawing.Point(8, 83);
            this.abilityListLabel.Name = "abilityListLabel";
            this.abilityListLabel.Size = new System.Drawing.Size(86, 16);
            this.abilityListLabel.TabIndex = 0;
            this.abilityListLabel.Text = "优先技能：";
            // 
            // teamListBox
            // 
            this.teamListBox.Location = new System.Drawing.Point(8, 59);
            this.teamListBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.teamListBox.Name = "teamListBox";
            this.teamListBox.Size = new System.Drawing.Size(194, 21);
            this.teamListBox.TabIndex = 3;
            this.teamListBox.TextChanged += new System.EventHandler(this.teamListBox_TextChanged);
            // 
            // abilityListBox
            // 
            this.abilityListBox.Location = new System.Drawing.Point(8, 102);
            this.abilityListBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.abilityListBox.Name = "abilityListBox";
            this.abilityListBox.Size = new System.Drawing.Size(194, 21);
            this.abilityListBox.TabIndex = 0;
            this.abilityListBox.TextChanged += new System.EventHandler(this.abilityListBox_TextChanged);
            // 
            // firstTargetLabel
            // 
            this.firstTargetLabel.Location = new System.Drawing.Point(225, 41);
            this.firstTargetLabel.Name = "firstTargetLabel";
            this.firstTargetLabel.Size = new System.Drawing.Size(102, 16);
            this.firstTargetLabel.TabIndex = 4;
            this.firstTargetLabel.Text = "优先击杀目标：";
            // 
            // firstTargetBox
            // 
            this.firstTargetBox.Location = new System.Drawing.Point(225, 59);
            this.firstTargetBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.firstTargetBox.Name = "firstTargetBox";
            this.firstTargetBox.Size = new System.Drawing.Size(194, 21);
            this.firstTargetBox.TabIndex = 5;
            this.firstTargetBox.TextChanged += new System.EventHandler(this.firstTargetBox_TextChanged);
            // 
            // battlePolicyLabel
            // 
            this.battlePolicyLabel.Location = new System.Drawing.Point(225, 83);
            this.battlePolicyLabel.Name = "battlePolicyLabel";
            this.battlePolicyLabel.Size = new System.Drawing.Size(86, 16);
            this.battlePolicyLabel.TabIndex = 6;
            this.battlePolicyLabel.Text = "战斗策略：";
            // 
            // battlePolicyCombo
            // 
            this.battlePolicyCombo.Items.AddRange(new object[] {
            "颜色克制",
            "集火第一个",
            "血量最少",
            "血量最多"});
            this.battlePolicyCombo.Location = new System.Drawing.Point(225, 100);
            this.battlePolicyCombo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.battlePolicyCombo.Name = "battlePolicyCombo";
            this.battlePolicyCombo.Size = new System.Drawing.Size(194, 20);
            this.battlePolicyCombo.TabIndex = 7;
            this.battlePolicyCombo.SelectedIndexChanged += new System.EventHandler(this.battlePolicyCombo_SelectedIndexChanged);
            // 
            // importButton
            // 
            this.importButton.Location = new System.Drawing.Point(225, 8);
            this.importButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.importButton.Name = "importButton";
            this.importButton.Size = new System.Drawing.Size(95, 31);
            this.importButton.TabIndex = 2;
            this.importButton.Text = "导入";
            this.importButton.UseVisualStyleBackColor = true;
            this.importButton.Click += new System.EventHandler(this.importButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(433, 374);
            this.Controls.Add(this.modelButton);
            this.Controls.Add(this.autoButton);
            this.Controls.Add(this.importButton);
            this.Controls.Add(this.teamListLabel);
            this.Controls.Add(this.teamListBox);
            this.Controls.Add(this.abilityListLabel);
            this.Controls.Add(this.abilityListBox);
            this.Controls.Add(this.firstTargetLabel);
            this.Controls.Add(this.firstTargetBox);
            this.Controls.Add(this.battlePolicyLabel);
            this.Controls.Add(this.battlePolicyCombo);
            this.Controls.Add(this.groupBoxPVP);
            this.Controls.Add(this.groupBoxPVE);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "自动佣兵2.0";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBoxPVP.ResumeLayout(false);
            this.groupBoxPVP.PerformLayout();
            this.groupBoxPVE.ResumeLayout(false);
            this.groupBoxPVE.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}