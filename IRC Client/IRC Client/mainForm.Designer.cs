namespace IRC_Client
{
    partial class mainForm
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
            this.channelTabs = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.chatTab1 = new IRC_Client.ChatTab();
            this.commandBox = new System.Windows.Forms.TextBox();
            this.joinChannelButton = new System.Windows.Forms.Button();
            this.leaveButton = new System.Windows.Forms.Button();
            this.chatTabStart = new IRC_Client.ChatTab();
            this.channelTabs.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // channelTabs
            // 
            this.channelTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.channelTabs.Controls.Add(this.tabPage1);
            this.channelTabs.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.channelTabs.Location = new System.Drawing.Point(3, 5);
            this.channelTabs.Name = "channelTabs";
            this.channelTabs.SelectedIndex = 0;
            this.channelTabs.Size = new System.Drawing.Size(738, 396);
            this.channelTabs.TabIndex = 6;
            this.channelTabs.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.channelTabs_DrawItem);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.chatTab1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(730, 370);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Server";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // chatTab1
            // 
            this.chatTab1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chatTab1.Location = new System.Drawing.Point(3, 3);
            this.chatTab1.Name = "chatTab1";
            this.chatTab1.Size = new System.Drawing.Size(724, 364);
            this.chatTab1.TabIndex = 14;
            // 
            // commandBox
            // 
            this.commandBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.commandBox.Location = new System.Drawing.Point(271, 412);
            this.commandBox.Name = "commandBox";
            this.commandBox.Size = new System.Drawing.Size(465, 20);
            this.commandBox.TabIndex = 13;
            // 
            // joinChannelButton
            // 
            this.joinChannelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.joinChannelButton.Enabled = false;
            this.joinChannelButton.Font = new System.Drawing.Font("DejaVu Sans Mono", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.joinChannelButton.Location = new System.Drawing.Point(7, 403);
            this.joinChannelButton.Name = "joinChannelButton";
            this.joinChannelButton.Size = new System.Drawing.Size(126, 37);
            this.joinChannelButton.TabIndex = 12;
            this.joinChannelButton.Text = "Join Channel";
            this.joinChannelButton.UseVisualStyleBackColor = true;
            this.joinChannelButton.Click += new System.EventHandler(this.joinChannelButton_Click);
            // 
            // leaveButton
            // 
            this.leaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.leaveButton.Enabled = false;
            this.leaveButton.Font = new System.Drawing.Font("DejaVu Sans Mono", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.leaveButton.Location = new System.Drawing.Point(139, 403);
            this.leaveButton.Name = "leaveButton";
            this.leaveButton.Size = new System.Drawing.Size(126, 37);
            this.leaveButton.TabIndex = 14;
            this.leaveButton.Text = "Leave";
            this.leaveButton.UseVisualStyleBackColor = true;
            this.leaveButton.Click += new System.EventHandler(this.leaveButton_Click);
            // 
            // chatTabStart
            // 
            this.chatTabStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chatTabStart.Location = new System.Drawing.Point(7, 7);
            this.chatTabStart.Name = "chatTabStart";
            this.chatTabStart.Size = new System.Drawing.Size(729, 358);
            this.chatTabStart.TabIndex = 0;
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 452);
            this.Controls.Add(this.leaveButton);
            this.Controls.Add(this.commandBox);
            this.Controls.Add(this.joinChannelButton);
            this.Controls.Add(this.channelTabs);
            this.Name = "mainForm";
            this.Text = "IRC Client";
            this.channelTabs.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl channelTabs;
        private ChatTab chatTabStart;
        public System.Windows.Forms.Button joinChannelButton;
        public System.Windows.Forms.TextBox commandBox;
        private System.Windows.Forms.TabPage tabPage1;
        private ChatTab chatTab1;
        public System.Windows.Forms.Button leaveButton;
    }
}

