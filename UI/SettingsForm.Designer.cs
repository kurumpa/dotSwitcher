namespace dotSwitcher.UI
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.buttonCancelSettings = new System.Windows.Forms.Button();
            this.buttonSaveSettings = new System.Windows.Forms.Button();
            this.buttonExit = new System.Windows.Forms.Button();
            this.checkBoxTrayIcon = new System.Windows.Forms.CheckBox();
            this.checkBoxAutorun = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxDelay = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonGithub = new System.Windows.Forms.Button();
            this.textBoxSwitchLayoutHotkey = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.textBoxConvertHotkey = new System.Windows.Forms.TextBox();
            this.textBoxSwitchHotkey = new System.Windows.Forms.TextBox();
            this.hotKeyBox1 = new dotSwitcher.UI.HotKeyBox();
            this.checkBoxSmartSelection = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCancelSettings
            // 
            this.buttonCancelSettings.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancelSettings.Location = new System.Drawing.Point(523, 330);
            this.buttonCancelSettings.Margin = new System.Windows.Forms.Padding(6);
            this.buttonCancelSettings.Name = "buttonCancelSettings";
            this.buttonCancelSettings.Size = new System.Drawing.Size(138, 42);
            this.buttonCancelSettings.TabIndex = 17;
            this.buttonCancelSettings.Text = "Cancel";
            this.buttonCancelSettings.UseVisualStyleBackColor = true;
            this.buttonCancelSettings.Click += new System.EventHandler(this.buttonCancelSettings_Click);
            // 
            // buttonSaveSettings
            // 
            this.buttonSaveSettings.Location = new System.Drawing.Point(374, 330);
            this.buttonSaveSettings.Margin = new System.Windows.Forms.Padding(6);
            this.buttonSaveSettings.Name = "buttonSaveSettings";
            this.buttonSaveSettings.Size = new System.Drawing.Size(138, 42);
            this.buttonSaveSettings.TabIndex = 15;
            this.buttonSaveSettings.Text = "Apply";
            this.buttonSaveSettings.UseVisualStyleBackColor = true;
            this.buttonSaveSettings.Click += new System.EventHandler(this.buttonSaveSettings_Click);
            // 
            // buttonExit
            // 
            this.buttonExit.Location = new System.Drawing.Point(61, 330);
            this.buttonExit.Margin = new System.Windows.Forms.Padding(6);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(138, 42);
            this.buttonExit.TabIndex = 18;
            this.buttonExit.Text = "Exit program";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // checkBoxTrayIcon
            // 
            this.checkBoxTrayIcon.AutoSize = true;
            this.checkBoxTrayIcon.Location = new System.Drawing.Point(22, 83);
            this.checkBoxTrayIcon.Margin = new System.Windows.Forms.Padding(6);
            this.checkBoxTrayIcon.Name = "checkBoxTrayIcon";
            this.checkBoxTrayIcon.Size = new System.Drawing.Size(166, 29);
            this.checkBoxTrayIcon.TabIndex = 20;
            this.checkBoxTrayIcon.Text = "Show tray icon";
            this.checkBoxTrayIcon.UseVisualStyleBackColor = true;
            this.checkBoxTrayIcon.CheckedChanged += new System.EventHandler(this.checkBoxTrayIcon_CheckedChanged);
            // 
            // checkBoxAutorun
            // 
            this.checkBoxAutorun.AutoSize = true;
            this.checkBoxAutorun.Location = new System.Drawing.Point(22, 41);
            this.checkBoxAutorun.Margin = new System.Windows.Forms.Padding(6);
            this.checkBoxAutorun.Name = "checkBoxAutorun";
            this.checkBoxAutorun.Size = new System.Drawing.Size(250, 29);
            this.checkBoxAutorun.TabIndex = 19;
            this.checkBoxAutorun.Text = "Start on windows startup";
            this.checkBoxAutorun.UseVisualStyleBackColor = true;
            this.checkBoxAutorun.CheckedChanged += new System.EventHandler(this.checkBoxAutorun_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 113);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(169, 25);
            this.label2.TabIndex = 23;
            this.label2.Text = "Convert selection:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 41);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(169, 25);
            this.label1.TabIndex = 22;
            this.label1.Text = "Convert last word:";
            // 
            // textBoxDelay
            // 
            this.textBoxDelay.Location = new System.Drawing.Point(22, 203);
            this.textBoxDelay.Margin = new System.Windows.Forms.Padding(6);
            this.textBoxDelay.Name = "textBoxDelay";
            this.textBoxDelay.Size = new System.Drawing.Size(83, 29);
            this.textBoxDelay.TabIndex = 25;
            this.textBoxDelay.TextChanged += new System.EventHandler(this.textBoxDelay_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 172);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(213, 25);
            this.label3.TabIndex = 26;
            this.label3.Text = "Delay before switching:";
            // 
            // buttonGithub
            // 
            this.buttonGithub.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonGithub.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonGithub.Image = global::dotSwitcher.Properties.Resources.github;
            this.buttonGithub.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonGithub.Location = new System.Drawing.Point(22, 253);
            this.buttonGithub.Margin = new System.Windows.Forms.Padding(6);
            this.buttonGithub.Name = "buttonGithub";
            this.buttonGithub.Size = new System.Drawing.Size(209, 42);
            this.buttonGithub.TabIndex = 28;
            this.buttonGithub.Text = "Report an issue";
            this.buttonGithub.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonGithub.UseVisualStyleBackColor = true;
            this.buttonGithub.Click += new System.EventHandler(this.buttonGithub_Click);
            // 
            // textBoxSwitchLayoutHotkey
            // 
            this.textBoxSwitchLayoutHotkey.Location = new System.Drawing.Point(981, 353);
            this.textBoxSwitchLayoutHotkey.Margin = new System.Windows.Forms.Padding(6);
            this.textBoxSwitchLayoutHotkey.Name = "textBoxSwitchLayoutHotkey";
            this.textBoxSwitchLayoutHotkey.Size = new System.Drawing.Size(307, 29);
            this.textBoxSwitchLayoutHotkey.TabIndex = 30;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 185);
            this.label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(219, 25);
            this.label4.TabIndex = 29;
            this.label4.Text = "Switch keyboard layout:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(326, 22);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox1.Size = new System.Drawing.Size(334, 273);
            this.groupBox1.TabIndex = 31;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Hotkeys";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(297, 185);
            this.label5.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 25);
            this.label5.TabIndex = 31;
            this.label5.Text = "?";
            this.label5.MouseLeave += new System.EventHandler(this.label5_MouseLeave);
            this.label5.MouseHover += new System.EventHandler(this.label5_MouseHover);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.InitialDelay = 100;
            this.toolTip1.ReshowDelay = 100;
            this.toolTip1.Popup += new System.Windows.Forms.PopupEventHandler(this.toolTip1_Popup);
            // 
            // textBoxConvertHotkey
            // 
            this.textBoxConvertHotkey.Location = new System.Drawing.Point(997, 194);
            this.textBoxConvertHotkey.Margin = new System.Windows.Forms.Padding(6);
            this.textBoxConvertHotkey.Name = "textBoxConvertHotkey";
            this.textBoxConvertHotkey.Size = new System.Drawing.Size(307, 29);
            this.textBoxConvertHotkey.TabIndex = 24;
            // 
            // textBoxSwitchHotkey
            // 
            this.textBoxSwitchHotkey.Location = new System.Drawing.Point(997, 83);
            this.textBoxSwitchHotkey.Margin = new System.Windows.Forms.Padding(6);
            this.textBoxSwitchHotkey.Name = "textBoxSwitchHotkey";
            this.textBoxSwitchHotkey.Size = new System.Drawing.Size(307, 29);
            this.textBoxSwitchHotkey.TabIndex = 21;
            // 
            // hotKeyBox1
            // 
            this.hotKeyBox1.HotKey = ((dotSwitcher.Data.KeyboardEventArgs)(resources.GetObject("hotKeyBox1.HotKey")));
            this.hotKeyBox1.Location = new System.Drawing.Point(711, 537);
            this.hotKeyBox1.Margin = new System.Windows.Forms.Padding(6);
            this.hotKeyBox1.Name = "hotKeyBox1";
            this.hotKeyBox1.Size = new System.Drawing.Size(417, 29);
            this.hotKeyBox1.TabIndex = 32;
            // 
            // checkBoxSmartSelection
            // 
            this.checkBoxSmartSelection.AutoSize = true;
            this.checkBoxSmartSelection.Checked = true;
            this.checkBoxSmartSelection.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSmartSelection.Location = new System.Drawing.Point(22, 131);
            this.checkBoxSmartSelection.Name = "checkBoxSmartSelection";
            this.checkBoxSmartSelection.Size = new System.Drawing.Size(283, 29);
            this.checkBoxSmartSelection.TabIndex = 33;
            this.checkBoxSmartSelection.Text = "Use smart convert selection";
            this.checkBoxSmartSelection.UseVisualStyleBackColor = true;
            this.checkBoxSmartSelection.CheckedChanged += new System.EventHandler(this.smartSelection_CheckedChanged);
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.buttonSaveSettings;
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancelSettings;
            this.ClientSize = new System.Drawing.Size(1397, 827);
            this.Controls.Add(this.checkBoxSmartSelection);
            this.Controls.Add(this.hotKeyBox1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonGithub);
            this.Controls.Add(this.textBoxSwitchLayoutHotkey);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxSwitchHotkey);
            this.Controls.Add(this.textBoxDelay);
            this.Controls.Add(this.textBoxConvertHotkey);
            this.Controls.Add(this.checkBoxTrayIcon);
            this.Controls.Add(this.checkBoxAutorun);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.buttonSaveSettings);
            this.Controls.Add(this.buttonCancelSettings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "SettingsForm";
            this.Text = "dotSwitcher Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsForm_FormClosing);
            this.Shown += new System.EventHandler(this.SettingsForm_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCancelSettings;
        private System.Windows.Forms.Button buttonSaveSettings;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.CheckBox checkBoxTrayIcon;
        private System.Windows.Forms.CheckBox checkBoxAutorun;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxDelay;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonGithub;
        private System.Windows.Forms.TextBox textBoxSwitchLayoutHotkey;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox textBoxSwitchHotkey;
        private System.Windows.Forms.TextBox textBoxConvertHotkey;
        private HotKeyBox hotKeyBox1;
        private System.Windows.Forms.CheckBox checkBoxSmartSelection;
    }
}