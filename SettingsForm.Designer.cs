namespace dotSwitcher
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
            this.shortcutTextBox = new System.Windows.Forms.TextBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.switchKeyboardCheckox = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxKeyboarLayouts = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // shortcutTextBox
            // 
            this.shortcutTextBox.Location = new System.Drawing.Point(98, 24);
            this.shortcutTextBox.Name = "shortcutTextBox";
            this.shortcutTextBox.Size = new System.Drawing.Size(169, 20);
            this.shortcutTextBox.TabIndex = 6;
            this.shortcutTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.shortcutTextBox_KeyUp);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(137, 225);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 7;
            this.buttonSave.Text = "&Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(218, 225);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 8;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Convert last word";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBoxKeyboarLayouts);
            this.groupBox1.Controls.Add(this.switchKeyboardCheckox);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.shortcutTextBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(281, 142);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Hotkey settings";
            // 
            // switchKeyboardCheckox
            // 
            this.switchKeyboardCheckox.AutoSize = true;
            this.switchKeyboardCheckox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.switchKeyboardCheckox.Location = new System.Drawing.Point(6, 74);
            this.switchKeyboardCheckox.Name = "switchKeyboardCheckox";
            this.switchKeyboardCheckox.Size = new System.Drawing.Size(175, 17);
            this.switchKeyboardCheckox.TabIndex = 13;
            this.switchKeyboardCheckox.Text = "Switch keyboard layout by keys";
            this.switchKeyboardCheckox.UseVisualStyleBackColor = true;
            this.switchKeyboardCheckox.CheckedChanged += new System.EventHandler(this.switchKeyboard_CheckedChanged);
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(98, 50);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(169, 20);
            this.textBox1.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Enabled = false;
            this.label2.Location = new System.Drawing.Point(6, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Convert selection";
            // 
            // comboBoxKeyboarLayouts
            // 
            this.comboBoxKeyboarLayouts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxKeyboarLayouts.FormattingEnabled = true;
            this.comboBoxKeyboarLayouts.Location = new System.Drawing.Point(6, 97);
            this.comboBoxKeyboarLayouts.Name = "comboBoxKeyboarLayouts";
            this.comboBoxKeyboarLayouts.Size = new System.Drawing.Size(121, 21);
            this.comboBoxKeyboarLayouts.TabIndex = 11;
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.buttonSave;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(305, 260);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SettingsForm";
            this.Text = "dotSwitcher Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsForm_FormClosing);
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox shortcutTextBox;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox switchKeyboardCheckox;
        private System.Windows.Forms.ComboBox comboBoxKeyboarLayouts;
    }
}