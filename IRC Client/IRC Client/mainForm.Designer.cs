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
            this.MenuBar = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeChatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serverOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.joinChannelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.leaveChannelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.currentServer = new IRC_Client.ServerTab();
            this.chatTabStart = new IRC_Client.ChatTab();
            this.channelTabs.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.MenuBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // channelTabs
            // 
            this.channelTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.channelTabs.Controls.Add(this.tabPage1);
            this.channelTabs.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.channelTabs.Location = new System.Drawing.Point(12, 32);
            this.channelTabs.Name = "channelTabs";
            this.channelTabs.Padding = new System.Drawing.Point(0, 0);
            this.channelTabs.SelectedIndex = 0;
            this.channelTabs.Size = new System.Drawing.Size(720, 408);
            this.channelTabs.TabIndex = 6;
            this.channelTabs.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.channelTabs_DrawItem);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.currentServer);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(712, 382);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Server";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // MenuBar
            // 
            this.MenuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.serverOptionsToolStripMenuItem});
            this.MenuBar.Location = new System.Drawing.Point(0, 0);
            this.MenuBar.Name = "MenuBar";
            this.MenuBar.Size = new System.Drawing.Size(744, 24);
            this.MenuBar.TabIndex = 15;
            this.MenuBar.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeChatToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // closeChatToolStripMenuItem
            // 
            this.closeChatToolStripMenuItem.Name = "closeChatToolStripMenuItem";
            this.closeChatToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.closeChatToolStripMenuItem.Text = "Exit";
            this.closeChatToolStripMenuItem.Click += new System.EventHandler(this.closeChatToolStripMenuItem_Click);
            // 
            // serverOptionsToolStripMenuItem
            // 
            this.serverOptionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.joinChannelToolStripMenuItem,
            this.leaveChannelToolStripMenuItem});
            this.serverOptionsToolStripMenuItem.Name = "serverOptionsToolStripMenuItem";
            this.serverOptionsToolStripMenuItem.Size = new System.Drawing.Size(96, 20);
            this.serverOptionsToolStripMenuItem.Text = "Server Options";
            // 
            // joinChannelToolStripMenuItem
            // 
            this.joinChannelToolStripMenuItem.Name = "joinChannelToolStripMenuItem";
            this.joinChannelToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.joinChannelToolStripMenuItem.Text = "Join Channel";
            this.joinChannelToolStripMenuItem.Click += new System.EventHandler(this.joinChannelFormToolStripMenuItem_Click);
            // 
            // leaveChannelToolStripMenuItem
            // 
            this.leaveChannelToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.allToolStripMenuItem});
            this.leaveChannelToolStripMenuItem.Name = "leaveChannelToolStripMenuItem";
            this.leaveChannelToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.leaveChannelToolStripMenuItem.Text = "Leave Channel";
            // 
            // allToolStripMenuItem
            // 
            this.allToolStripMenuItem.Enabled = false;
            this.allToolStripMenuItem.Name = "allToolStripMenuItem";
            this.allToolStripMenuItem.Size = new System.Drawing.Size(88, 22);
            this.allToolStripMenuItem.Text = "All";
            this.allToolStripMenuItem.Click += new System.EventHandler(this.allToolStripMenuItem_Click);
            // 
            // currentServer
            // 
            this.currentServer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.currentServer.Location = new System.Drawing.Point(7, 7);
            this.currentServer.Name = "currentServer";
            this.currentServer.Size = new System.Drawing.Size(699, 380);
            this.currentServer.TabIndex = 0;
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
            this.Controls.Add(this.channelTabs);
            this.Controls.Add(this.MenuBar);
            this.MainMenuStrip = this.MenuBar;
            this.Name = "mainForm";
            this.Text = "IRC Client";
            this.channelTabs.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.MenuBar.ResumeLayout(false);
            this.MenuBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl channelTabs;
        private ChatTab chatTabStart;
        private System.Windows.Forms.MenuStrip MenuBar;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeChatToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem serverOptionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem leaveChannelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem allToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem joinChannelToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPage1;
        private ServerTab currentServer;
    }
}

