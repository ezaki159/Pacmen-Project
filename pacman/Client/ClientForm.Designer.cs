using System;

namespace Client {
    partial class ClientForm {
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
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.chatTB = new System.Windows.Forms.RichTextBox();
            this.chatMsgTB = new System.Windows.Forms.TextBox();
            this.serverAddressTB = new System.Windows.Forms.TextBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.portTB = new System.Windows.Forms.TextBox();
            this.nicknameTB = new System.Windows.Forms.TextBox();
            this.serverAddressLabel = new System.Windows.Forms.Label();
            this.portLabel = new System.Windows.Forms.Label();
            this.nicknameLabel = new System.Windows.Forms.Label();
            this.scoreLabel1 = new System.Windows.Forms.Label();
            this.scoreLabel2 = new System.Windows.Forms.Label();
            this.gamePanel = new System.Windows.Forms.Panel();
            this.tracefileCB = new System.Windows.Forms.CheckBox();
            this.tracefileTB = new System.Windows.Forms.TextBox();
            this.tracefileLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // chatTB
            // 
            this.chatTB.Enabled = false;
            this.chatTB.Location = new System.Drawing.Point(451, 147);
            this.chatTB.Name = "chatTB";
            this.chatTB.ReadOnly = true;
            this.chatTB.Size = new System.Drawing.Size(182, 198);
            this.chatTB.TabIndex = 0;
            this.chatTB.Text = "";
            this.chatTB.TextChanged += new System.EventHandler(this.chatTB_TextChanged);
            // 
            // chatMsgTB
            // 
            this.chatMsgTB.Enabled = false;
            this.chatMsgTB.Location = new System.Drawing.Point(451, 351);
            this.chatMsgTB.Name = "chatMsgTB";
            this.chatMsgTB.Size = new System.Drawing.Size(182, 20);
            this.chatMsgTB.TabIndex = 1;
            this.chatMsgTB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.chatMsgTB_KeyDown);
            // 
            // serverAddressTB
            // 
            this.serverAddressTB.Location = new System.Drawing.Point(533, 12);
            this.serverAddressTB.Name = "serverAddressTB";
            this.serverAddressTB.Size = new System.Drawing.Size(100, 20);
            this.serverAddressTB.TabIndex = 2;
            this.serverAddressTB.TextChanged += new System.EventHandler(this.UpdateConnectButton);
            // 
            // connectButton
            // 
            this.connectButton.Enabled = false;
            this.connectButton.Location = new System.Drawing.Point(558, 116);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(75, 23);
            this.connectButton.TabIndex = 3;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // portTB
            // 
            this.portTB.Location = new System.Drawing.Point(533, 38);
            this.portTB.Name = "portTB";
            this.portTB.Size = new System.Drawing.Size(100, 20);
            this.portTB.TabIndex = 4;
            this.portTB.TextChanged += new System.EventHandler(this.UpdateConnectButton);
            this.portTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPressNumericOnly);
            // 
            // nicknameTB
            // 
            this.nicknameTB.Location = new System.Drawing.Point(533, 64);
            this.nicknameTB.Name = "nicknameTB";
            this.nicknameTB.Size = new System.Drawing.Size(100, 20);
            this.nicknameTB.TabIndex = 5;
            this.nicknameTB.TextChanged += new System.EventHandler(this.UpdateConnectButton);
            // 
            // serverAddressLabel
            // 
            this.serverAddressLabel.AutoSize = true;
            this.serverAddressLabel.Location = new System.Drawing.Point(448, 15);
            this.serverAddressLabel.Name = "serverAddressLabel";
            this.serverAddressLabel.Size = new System.Drawing.Size(81, 13);
            this.serverAddressLabel.TabIndex = 6;
            this.serverAddressLabel.Text = "Server address:";
            // 
            // portLabel
            // 
            this.portLabel.AutoSize = true;
            this.portLabel.Location = new System.Drawing.Point(500, 41);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(29, 13);
            this.portLabel.TabIndex = 7;
            this.portLabel.Text = "Port:";
            // 
            // nicknameLabel
            // 
            this.nicknameLabel.AutoSize = true;
            this.nicknameLabel.Location = new System.Drawing.Point(471, 67);
            this.nicknameLabel.Name = "nicknameLabel";
            this.nicknameLabel.Size = new System.Drawing.Size(58, 13);
            this.nicknameLabel.TabIndex = 8;
            this.nicknameLabel.Text = "Nickname:";
            // 
            // scoreLabel1
            // 
            this.scoreLabel1.AutoSize = true;
            this.scoreLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scoreLabel1.Location = new System.Drawing.Point(12, 9);
            this.scoreLabel1.Name = "scoreLabel1";
            this.scoreLabel1.Size = new System.Drawing.Size(80, 25);
            this.scoreLabel1.TabIndex = 9;
            this.scoreLabel1.Text = "Score:";
            // 
            // scoreLabel2
            // 
            this.scoreLabel2.AutoSize = true;
            this.scoreLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scoreLabel2.Location = new System.Drawing.Point(84, 9);
            this.scoreLabel2.Name = "scoreLabel2";
            this.scoreLabel2.Size = new System.Drawing.Size(25, 25);
            this.scoreLabel2.TabIndex = 10;
            this.scoreLabel2.Text = "0";
            // 
            // gamePanel
            // 
            this.gamePanel.Location = new System.Drawing.Point(17, 41);
            this.gamePanel.Name = "gamePanel";
            this.gamePanel.Size = new System.Drawing.Size(416, 330);
            this.gamePanel.TabIndex = 15;
            // 
            // tracefileCB
            // 
            this.tracefileCB.AutoSize = true;
            this.tracefileCB.Location = new System.Drawing.Point(457, 93);
            this.tracefileCB.Name = "tracefileCB";
            this.tracefileCB.Size = new System.Drawing.Size(15, 14);
            this.tracefileCB.TabIndex = 16;
            this.tracefileCB.UseVisualStyleBackColor = true;
            this.tracefileCB.CheckedChanged += new System.EventHandler(this.tracefileCB_CheckedChanged);
            // 
            // tracefileTB
            // 
            this.tracefileTB.Enabled = false;
            this.tracefileTB.Location = new System.Drawing.Point(533, 90);
            this.tracefileTB.Name = "tracefileTB";
            this.tracefileTB.Size = new System.Drawing.Size(100, 20);
            this.tracefileTB.TabIndex = 17;
            this.tracefileTB.Text = "tracefile.txt";
            // 
            // tracefileLabel
            // 
            this.tracefileLabel.AutoSize = true;
            this.tracefileLabel.Location = new System.Drawing.Point(478, 93);
            this.tracefileLabel.Name = "tracefileLabel";
            this.tracefileLabel.Size = new System.Drawing.Size(51, 13);
            this.tracefileLabel.TabIndex = 18;
            this.tracefileLabel.Text = "Tracefile:";
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(645, 383);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.tracefileLabel);
            this.Controls.Add(this.tracefileTB);
            this.Controls.Add(this.tracefileCB);
            this.Controls.Add(this.gamePanel);
            this.Controls.Add(this.scoreLabel2);
            this.Controls.Add(this.scoreLabel1);
            this.Controls.Add(this.nicknameLabel);
            this.Controls.Add(this.portLabel);
            this.Controls.Add(this.serverAddressLabel);
            this.Controls.Add(this.nicknameTB);
            this.Controls.Add(this.portTB);
            this.Controls.Add(this.serverAddressTB);
            this.Controls.Add(this.chatMsgTB);
            this.Controls.Add(this.chatTB);
            this.Name = "ClientForm";
            this.Text = "Pacmen";
            this.Load += new System.EventHandler(this.ClientForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox chatTB;
        private System.Windows.Forms.TextBox chatMsgTB;
        private System.Windows.Forms.TextBox serverAddressTB;
        public System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.TextBox portTB;
        private System.Windows.Forms.TextBox nicknameTB;
        private System.Windows.Forms.Label serverAddressLabel;
        private System.Windows.Forms.Label portLabel;
        private System.Windows.Forms.Label nicknameLabel;
        private System.Windows.Forms.Label scoreLabel1;
        private System.Windows.Forms.Label scoreLabel2;
        private System.Windows.Forms.Panel gamePanel;
        private System.Windows.Forms.CheckBox tracefileCB;
        private System.Windows.Forms.TextBox tracefileTB;
        private System.Windows.Forms.Label tracefileLabel;
    }
}