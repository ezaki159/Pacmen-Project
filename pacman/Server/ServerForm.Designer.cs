namespace Server {
    partial class ServerForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
            _gameServer?.StopServer();
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.nPlayersTB = new System.Windows.Forms.TextBox();
            this.inputTimeTB = new System.Windows.Forms.TextBox();
            this.startButton = new System.Windows.Forms.Button();
            this.playerListTB = new System.Windows.Forms.TextBox();
            this.nPlayersLabel = new System.Windows.Forms.Label();
            this.inputTimeLabel = new System.Windows.Forms.Label();
            this.playerListLabel = new System.Windows.Forms.Label();
            this.serverStatLabel = new System.Windows.Forms.Label();
            this.serverStatLabel2 = new System.Windows.Forms.Label();
            this.timeUnitLabel = new System.Windows.Forms.Label();
            this.serverLogTB = new System.Windows.Forms.RichTextBox();
            this.serverLogLabel = new System.Windows.Forms.Label();
            this.gameTypeCB = new System.Windows.Forms.ComboBox();
            this.gameTypeLabel = new System.Windows.Forms.Label();
            this.replicaCB = new System.Windows.Forms.CheckBox();
            this.primaryInfoLabel = new System.Windows.Forms.Label();
            this.primaryAddressLabel = new System.Windows.Forms.Label();
            this.primaryPortLabel = new System.Windows.Forms.Label();
            this.primaryAddressTB = new System.Windows.Forms.TextBox();
            this.primaryPortTB = new System.Windows.Forms.TextBox();
            this.replicaStartButton = new System.Windows.Forms.Button();
            this.primaryLabel = new System.Windows.Forms.Label();
            this.serverPortLabel = new System.Windows.Forms.Label();
            this.portTB = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // nPlayersTB
            // 
            this.nPlayersTB.Location = new System.Drawing.Point(470, 81);
            this.nPlayersTB.Name = "nPlayersTB";
            this.nPlayersTB.Size = new System.Drawing.Size(38, 20);
            this.nPlayersTB.TabIndex = 0;
            this.nPlayersTB.TextChanged += new System.EventHandler(this.UpdatePrimaryStartButton);
            this.nPlayersTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPressNumericOnly);
            // 
            // inputTimeTB
            // 
            this.inputTimeTB.Location = new System.Drawing.Point(470, 104);
            this.inputTimeTB.Name = "inputTimeTB";
            this.inputTimeTB.Size = new System.Drawing.Size(38, 20);
            this.inputTimeTB.TabIndex = 1;
            this.inputTimeTB.TextChanged += new System.EventHandler(this.UpdatePrimaryStartButton);
            this.inputTimeTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPressNumericOnly);
            // 
            // startButton
            // 
            this.startButton.Enabled = false;
            this.startButton.Location = new System.Drawing.Point(511, 130);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 2;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // playerListTB
            // 
            this.playerListTB.Location = new System.Drawing.Point(9, 22);
            this.playerListTB.Multiline = true;
            this.playerListTB.Name = "playerListTB";
            this.playerListTB.ReadOnly = true;
            this.playerListTB.Size = new System.Drawing.Size(170, 236);
            this.playerListTB.TabIndex = 3;
            // 
            // nPlayersLabel
            // 
            this.nPlayersLabel.AutoSize = true;
            this.nPlayersLabel.Location = new System.Drawing.Point(398, 84);
            this.nPlayersLabel.Name = "nPlayersLabel";
            this.nPlayersLabel.Size = new System.Drawing.Size(65, 13);
            this.nPlayersLabel.TabIndex = 4;
            this.nPlayersLabel.Text = "# of players:";
            // 
            // inputTimeLabel
            // 
            this.inputTimeLabel.AutoSize = true;
            this.inputTimeLabel.Location = new System.Drawing.Point(398, 107);
            this.inputTimeLabel.Name = "inputTimeLabel";
            this.inputTimeLabel.Size = new System.Drawing.Size(56, 13);
            this.inputTimeLabel.TabIndex = 5;
            this.inputTimeLabel.Text = "Input time:";
            // 
            // playerListLabel
            // 
            this.playerListLabel.AutoSize = true;
            this.playerListLabel.Location = new System.Drawing.Point(6, 6);
            this.playerListLabel.Name = "playerListLabel";
            this.playerListLabel.Size = new System.Drawing.Size(54, 13);
            this.playerListLabel.TabIndex = 6;
            this.playerListLabel.Text = "Player list:";
            // 
            // serverStatLabel
            // 
            this.serverStatLabel.AutoSize = true;
            this.serverStatLabel.Location = new System.Drawing.Point(6, 261);
            this.serverStatLabel.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.serverStatLabel.Name = "serverStatLabel";
            this.serverStatLabel.Size = new System.Drawing.Size(72, 13);
            this.serverStatLabel.TabIndex = 7;
            this.serverStatLabel.Text = "Server status:";
            // 
            // serverStatLabel2
            // 
            this.serverStatLabel2.AutoSize = true;
            this.serverStatLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverStatLabel2.ForeColor = System.Drawing.Color.Red;
            this.serverStatLabel2.Location = new System.Drawing.Point(74, 261);
            this.serverStatLabel2.Name = "serverStatLabel2";
            this.serverStatLabel2.Size = new System.Drawing.Size(95, 13);
            this.serverStatLabel2.TabIndex = 8;
            this.serverStatLabel2.Text = "NOT RUNNING";
            // 
            // timeUnitLabel
            // 
            this.timeUnitLabel.AutoSize = true;
            this.timeUnitLabel.Location = new System.Drawing.Point(514, 107);
            this.timeUnitLabel.Name = "timeUnitLabel";
            this.timeUnitLabel.Size = new System.Drawing.Size(20, 13);
            this.timeUnitLabel.TabIndex = 9;
            this.timeUnitLabel.Text = "ms";
            // 
            // serverLogTB
            // 
            this.serverLogTB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.serverLogTB.Location = new System.Drawing.Point(194, 22);
            this.serverLogTB.Name = "serverLogTB";
            this.serverLogTB.ReadOnly = true;
            this.serverLogTB.Size = new System.Drawing.Size(198, 236);
            this.serverLogTB.TabIndex = 10;
            this.serverLogTB.Text = "";
            this.serverLogTB.TextChanged += new System.EventHandler(this.richTextBox_TextChanged);
            // 
            // serverLogLabel
            // 
            this.serverLogLabel.AutoSize = true;
            this.serverLogLabel.Location = new System.Drawing.Point(191, 6);
            this.serverLogLabel.Name = "serverLogLabel";
            this.serverLogLabel.Size = new System.Drawing.Size(58, 13);
            this.serverLogLabel.TabIndex = 11;
            this.serverLogLabel.Text = "Server log:";
            // 
            // gameTypeCB
            // 
            this.gameTypeCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.gameTypeCB.FormattingEnabled = true;
            this.gameTypeCB.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.gameTypeCB.Items.AddRange(new object[] {
            "Pacman"});
            this.gameTypeCB.Location = new System.Drawing.Point(442, 32);
            this.gameTypeCB.Name = "gameTypeCB";
            this.gameTypeCB.Size = new System.Drawing.Size(147, 21);
            this.gameTypeCB.TabIndex = 12;
            this.gameTypeCB.SelectedIndexChanged += new System.EventHandler(this.UpdatePrimaryStartButton);
            this.gameTypeCB.SelectedIndexChanged += new System.EventHandler(this.UpdateReplicaStartButton);
            // 
            // gameTypeLabel
            // 
            this.gameTypeLabel.AutoSize = true;
            this.gameTypeLabel.Location = new System.Drawing.Point(398, 35);
            this.gameTypeLabel.Name = "gameTypeLabel";
            this.gameTypeLabel.Size = new System.Drawing.Size(38, 13);
            this.gameTypeLabel.TabIndex = 13;
            this.gameTypeLabel.Text = "Game:";
            // 
            // replicaCB
            // 
            this.replicaCB.AutoSize = true;
            this.replicaCB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.replicaCB.Location = new System.Drawing.Point(398, 158);
            this.replicaCB.Name = "replicaCB";
            this.replicaCB.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.replicaCB.Size = new System.Drawing.Size(110, 17);
            this.replicaCB.TabIndex = 14;
            this.replicaCB.Text = "Replica Server";
            this.replicaCB.UseVisualStyleBackColor = true;
            this.replicaCB.CheckedChanged += new System.EventHandler(this.replicaCB_CheckedChanged);
            this.replicaCB.CheckedChanged += new System.EventHandler(this.UpdatePrimaryStartButton);
            this.replicaCB.CheckedChanged += new System.EventHandler(this.UpdateReplicaStartButton);
            // 
            // primaryInfoLabel
            // 
            this.primaryInfoLabel.AutoSize = true;
            this.primaryInfoLabel.Location = new System.Drawing.Point(398, 184);
            this.primaryInfoLabel.Name = "primaryInfoLabel";
            this.primaryInfoLabel.Size = new System.Drawing.Size(132, 13);
            this.primaryInfoLabel.TabIndex = 15;
            this.primaryInfoLabel.Text = "Primary Server information:";
            // 
            // primaryAddressLabel
            // 
            this.primaryAddressLabel.AutoSize = true;
            this.primaryAddressLabel.Location = new System.Drawing.Point(398, 207);
            this.primaryAddressLabel.Name = "primaryAddressLabel";
            this.primaryAddressLabel.Size = new System.Drawing.Size(81, 13);
            this.primaryAddressLabel.TabIndex = 16;
            this.primaryAddressLabel.Text = "Server address:";
            // 
            // primaryPortLabel
            // 
            this.primaryPortLabel.AutoSize = true;
            this.primaryPortLabel.Location = new System.Drawing.Point(398, 226);
            this.primaryPortLabel.Name = "primaryPortLabel";
            this.primaryPortLabel.Size = new System.Drawing.Size(29, 13);
            this.primaryPortLabel.TabIndex = 17;
            this.primaryPortLabel.Text = "Port:";
            // 
            // primaryAddressTB
            // 
            this.primaryAddressTB.Enabled = false;
            this.primaryAddressTB.Location = new System.Drawing.Point(485, 204);
            this.primaryAddressTB.Name = "primaryAddressTB";
            this.primaryAddressTB.Size = new System.Drawing.Size(100, 20);
            this.primaryAddressTB.TabIndex = 18;
            this.primaryAddressTB.TextChanged += new System.EventHandler(this.UpdateReplicaStartButton);
            // 
            // primaryPortTB
            // 
            this.primaryPortTB.Enabled = false;
            this.primaryPortTB.Location = new System.Drawing.Point(485, 226);
            this.primaryPortTB.Name = "primaryPortTB";
            this.primaryPortTB.Size = new System.Drawing.Size(100, 20);
            this.primaryPortTB.TabIndex = 19;
            this.primaryPortTB.TextChanged += new System.EventHandler(this.UpdateReplicaStartButton);
            this.primaryPortTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPressNumericOnly);
            // 
            // replicaStartButton
            // 
            this.replicaStartButton.Enabled = false;
            this.replicaStartButton.Location = new System.Drawing.Point(511, 252);
            this.replicaStartButton.Name = "replicaStartButton";
            this.replicaStartButton.Size = new System.Drawing.Size(75, 23);
            this.replicaStartButton.TabIndex = 20;
            this.replicaStartButton.Text = "Start";
            this.replicaStartButton.UseVisualStyleBackColor = true;
            this.replicaStartButton.Click += new System.EventHandler(this.replicaStartButton_Click);
            // 
            // primaryLabel
            // 
            this.primaryLabel.AutoSize = true;
            this.primaryLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.primaryLabel.Location = new System.Drawing.Point(398, 61);
            this.primaryLabel.Name = "primaryLabel";
            this.primaryLabel.Size = new System.Drawing.Size(89, 13);
            this.primaryLabel.TabIndex = 21;
            this.primaryLabel.Text = "Primary Server";
            // 
            // serverPortLabel
            // 
            this.serverPortLabel.AutoSize = true;
            this.serverPortLabel.Location = new System.Drawing.Point(398, 12);
            this.serverPortLabel.Name = "serverPortLabel";
            this.serverPortLabel.Size = new System.Drawing.Size(29, 13);
            this.serverPortLabel.TabIndex = 22;
            this.serverPortLabel.Text = "Port:";
            // 
            // portTB
            // 
            this.portTB.Location = new System.Drawing.Point(442, 9);
            this.portTB.Name = "portTB";
            this.portTB.Size = new System.Drawing.Size(45, 20);
            this.portTB.TabIndex = 23;
            this.portTB.TextChanged += new System.EventHandler(this.UpdatePrimaryStartButton);
            this.portTB.TextChanged += new System.EventHandler(this.UpdateReplicaStartButton);
            this.portTB.TextChanged += new System.EventHandler(this.PortTextChanged);
            this.portTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPressNumericOnly);
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(601, 283);
            this.Controls.Add(this.portTB);
            this.Controls.Add(this.serverPortLabel);
            this.Controls.Add(this.primaryLabel);
            this.Controls.Add(this.replicaStartButton);
            this.Controls.Add(this.primaryPortTB);
            this.Controls.Add(this.primaryAddressTB);
            this.Controls.Add(this.primaryPortLabel);
            this.Controls.Add(this.primaryAddressLabel);
            this.Controls.Add(this.primaryInfoLabel);
            this.Controls.Add(this.replicaCB);
            this.Controls.Add(this.gameTypeLabel);
            this.Controls.Add(this.gameTypeCB);
            this.Controls.Add(this.serverLogLabel);
            this.Controls.Add(this.serverLogTB);
            this.Controls.Add(this.timeUnitLabel);
            this.Controls.Add(this.serverStatLabel2);
            this.Controls.Add(this.serverStatLabel);
            this.Controls.Add(this.playerListLabel);
            this.Controls.Add(this.inputTimeLabel);
            this.Controls.Add(this.nPlayersLabel);
            this.Controls.Add(this.playerListTB);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.inputTimeTB);
            this.Controls.Add(this.nPlayersTB);
            this.Name = "ServerForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.ServerForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox nPlayersTB;
        private System.Windows.Forms.TextBox inputTimeTB;
        internal System.Windows.Forms.Button startButton;
        private System.Windows.Forms.TextBox playerListTB;
        private System.Windows.Forms.Label nPlayersLabel;
        private System.Windows.Forms.Label inputTimeLabel;
        private System.Windows.Forms.Label playerListLabel;
        private System.Windows.Forms.Label serverStatLabel;
        private System.Windows.Forms.Label serverStatLabel2;
        private System.Windows.Forms.Label timeUnitLabel;
        private System.Windows.Forms.RichTextBox serverLogTB;
        private System.Windows.Forms.Label serverLogLabel;
        private System.Windows.Forms.ComboBox gameTypeCB;
        private System.Windows.Forms.Label gameTypeLabel;
        private System.Windows.Forms.CheckBox replicaCB;
        private System.Windows.Forms.Label primaryInfoLabel;
        private System.Windows.Forms.Label primaryAddressLabel;
        private System.Windows.Forms.Label primaryPortLabel;
        private System.Windows.Forms.TextBox primaryAddressTB;
        private System.Windows.Forms.TextBox primaryPortTB;
        private System.Windows.Forms.Button replicaStartButton;
        private System.Windows.Forms.Label primaryLabel;
        private System.Windows.Forms.Label serverPortLabel;
        private System.Windows.Forms.TextBox portTB;
    }
}

