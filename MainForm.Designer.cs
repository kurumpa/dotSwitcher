namespace dotSwitcher
{
    partial class MainForm
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lbl1lng = new System.Windows.Forms.Label();
            this.lbl2lng = new System.Windows.Forms.Label();
            this.cbLangOne = new System.Windows.Forms.ComboBox();
            this.cbLangTwo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbCLHK = new System.Windows.Forms.TextBox();
            this.tbCSHK = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TrayIconCheckBox = new System.Windows.Forms.CheckBox();
            this.btnHelp = new System.Windows.Forms.Button();
            this.cbAutorun = new System.Windows.Forms.CheckBox();
            this.cbSpaceBreak = new System.Windows.Forms.CheckBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btnOK.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnOK.Location = new System.Drawing.Point(99, 191);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnApply
            // 
            this.btnApply.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btnApply.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnApply.Location = new System.Drawing.Point(180, 191);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 1;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnCancel.Location = new System.Drawing.Point(261, 191);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lbl1lng
            // 
            this.lbl1lng.AutoSize = true;
            this.lbl1lng.BackColor = System.Drawing.Color.Transparent;
            this.lbl1lng.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl1lng.Location = new System.Drawing.Point(104, 141);
            this.lbl1lng.Name = "lbl1lng";
            this.lbl1lng.Size = new System.Drawing.Size(70, 13);
            this.lbl1lng.TabIndex = 3;
            this.lbl1lng.Text = "Language 1:";
            // 
            // lbl2lng
            // 
            this.lbl2lng.AutoSize = true;
            this.lbl2lng.BackColor = System.Drawing.Color.Transparent;
            this.lbl2lng.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl2lng.Location = new System.Drawing.Point(104, 167);
            this.lbl2lng.Name = "lbl2lng";
            this.lbl2lng.Size = new System.Drawing.Size(70, 13);
            this.lbl2lng.TabIndex = 4;
            this.lbl2lng.Text = "Language 2:";
            // 
            // cbLangOne
            // 
            this.cbLangOne.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbLangOne.FormattingEnabled = true;
            this.cbLangOne.Location = new System.Drawing.Point(185, 138);
            this.cbLangOne.Name = "cbLangOne";
            this.cbLangOne.Size = new System.Drawing.Size(151, 21);
            this.cbLangOne.TabIndex = 5;
            this.cbLangOne.SelectedIndexChanged += new System.EventHandler(this.cbLangOne_SelectedIndexChanged);
            // 
            // cbLangTwo
            // 
            this.cbLangTwo.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbLangTwo.FormattingEnabled = true;
            this.cbLangTwo.Location = new System.Drawing.Point(185, 164);
            this.cbLangTwo.Name = "cbLangTwo";
            this.cbLangTwo.Size = new System.Drawing.Size(151, 21);
            this.cbLangTwo.TabIndex = 6;
            this.cbLangTwo.SelectedIndexChanged += new System.EventHandler(this.cbLangTwo_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(8, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Convert last word hotkey:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(8, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(137, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Convert selection hotkey:";
            // 
            // tbCLHK
            // 
            this.tbCLHK.BackColor = System.Drawing.Color.White;
            this.tbCLHK.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbCLHK.Location = new System.Drawing.Point(154, 32);
            this.tbCLHK.Name = "tbCLHK";
            this.tbCLHK.ReadOnly = true;
            this.tbCLHK.Size = new System.Drawing.Size(182, 22);
            this.tbCLHK.TabIndex = 11;
            this.tbCLHK.Text = "Pause";
            this.tbCLHK.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbCLHK_KeyDown);
            // 
            // tbCSHK
            // 
            this.tbCSHK.BackColor = System.Drawing.Color.White;
            this.tbCSHK.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbCSHK.Location = new System.Drawing.Point(154, 58);
            this.tbCSHK.Name = "tbCSHK";
            this.tbCSHK.ReadOnly = true;
            this.tbCSHK.Size = new System.Drawing.Size(182, 22);
            this.tbCSHK.TabIndex = 12;
            this.tbCSHK.Text = "Scroll";
            this.tbCSHK.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbCSHK_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(175, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(149, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Switch between languages:";
            // 
            // TrayIconCheckBox
            // 
            this.TrayIconCheckBox.AutoSize = true;
            this.TrayIconCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.TrayIconCheckBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TrayIconCheckBox.Location = new System.Drawing.Point(12, 88);
            this.TrayIconCheckBox.Name = "TrayIconCheckBox";
            this.TrayIconCheckBox.Size = new System.Drawing.Size(110, 17);
            this.TrayIconCheckBox.TabIndex = 14;
            this.TrayIconCheckBox.Text = "Tray icon visible?";
            this.TrayIconCheckBox.UseVisualStyleBackColor = false;
            this.TrayIconCheckBox.CheckedChanged += new System.EventHandler(this.TrayIconCheckBox_CheckedChanged);
            // 
            // btnHelp
            // 
            this.btnHelp.Location = new System.Drawing.Point(12, 191);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(75, 23);
            this.btnHelp.TabIndex = 15;
            this.btnHelp.Text = "Help";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // cbAutorun
            // 
            this.cbAutorun.AutoSize = true;
            this.cbAutorun.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbAutorun.Location = new System.Drawing.Point(11, 7);
            this.cbAutorun.Name = "cbAutorun";
            this.cbAutorun.Size = new System.Drawing.Size(152, 17);
            this.cbAutorun.TabIndex = 17;
            this.cbAutorun.Text = "Autostart with Windows";
            this.cbAutorun.UseVisualStyleBackColor = true;
            this.cbAutorun.CheckedChanged += new System.EventHandler(this.cbAutorun_CheckedChanged);
            // 
            // cbSpaceBreak
            // 
            this.cbSpaceBreak.AutoSize = true;
            this.cbSpaceBreak.Location = new System.Drawing.Point(11, 114);
            this.cbSpaceBreak.Name = "cbSpaceBreak";
            this.cbSpaceBreak.Size = new System.Drawing.Size(158, 17);
            this.cbSpaceBreak.TabIndex = 18;
            this.cbSpaceBreak.Text = "Space will begin new word?";
            this.cbSpaceBreak.UseVisualStyleBackColor = true;
            this.cbSpaceBreak.CheckedChanged += new System.EventHandler(this.cbSpaceBreak_CheckedChanged);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.LinkColor = System.Drawing.Color.Red;
            this.linkLabel1.Location = new System.Drawing.Point(281, 9);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(60, 13);
            this.linkLabel1.TabIndex = 19;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "BladeMight";
            this.linkLabel1.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Location = new System.Drawing.Point(232, 9);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(49, 13);
            this.linkLabel2.TabIndex = 20;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "Kurumpa";
            this.linkLabel2.VisitedLinkColor = System.Drawing.Color.Aqua;
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(182, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 21;
            this.label4.Text = "Creators:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(348, 224);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.linkLabel2);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.cbSpaceBreak);
            this.Controls.Add(this.cbAutorun);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.TrayIconCheckBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbCSHK);
            this.Controls.Add(this.tbCLHK);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbLangTwo);
            this.Controls.Add(this.cbLangOne);
            this.Controls.Add(this.lbl2lng);
            this.Controls.Add(this.lbl1lng);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnOK);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = global::dotSwitcher.Properties.Resources.icon;
            this.Name = "MainForm";
            this.Text = "dotSwitcher";
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.VisibleChanged += new System.EventHandler(this.MainForm_VisibleChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lbl1lng;
        private System.Windows.Forms.Label lbl2lng;
        private System.Windows.Forms.ComboBox cbLangOne;
        private System.Windows.Forms.ComboBox cbLangTwo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbCLHK;
        private System.Windows.Forms.TextBox tbCSHK;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox TrayIconCheckBox;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.CheckBox cbAutorun;
        private System.Windows.Forms.CheckBox cbSpaceBreak;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.Label label4;
    }
}