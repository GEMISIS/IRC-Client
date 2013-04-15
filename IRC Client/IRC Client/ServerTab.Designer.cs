namespace IRC_Client
{
    partial class ServerTab
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.chatSplitter = new System.Windows.Forms.SplitContainer();
            this.connectionImage = new System.Windows.Forms.PictureBox();
            this.ServerName = new System.Windows.Forms.Label();
            this.msgRecvBox = new System.Windows.Forms.RichTextBox();
            this.serverListBox = new System.Windows.Forms.ListBox();
#if !MONO
            ((System.ComponentModel.ISupportInitialize)(this.chatSplitter)).BeginInit();
#endif 
            this.chatSplitter.Panel1.SuspendLayout();
            this.chatSplitter.Panel2.SuspendLayout();
            this.chatSplitter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.connectionImage)).BeginInit();
            this.SuspendLayout();
            // 
            // chatSplitter
            // 
            this.chatSplitter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.chatSplitter.Location = new System.Drawing.Point(3, 3);
            this.chatSplitter.Name = "chatSplitter";
            // 
            // chatSplitter.Panel1
            // 
            this.chatSplitter.Panel1.Controls.Add(this.connectionImage);
            this.chatSplitter.Panel1.Controls.Add(this.ServerName);
            this.chatSplitter.Panel1.Controls.Add(this.msgRecvBox);
            // 
            // chatSplitter.Panel2
            // 
            this.chatSplitter.Panel2.Controls.Add(this.serverListBox);
            this.chatSplitter.Size = new System.Drawing.Size(723, 306);
            this.chatSplitter.SplitterDistance = 506;
            this.chatSplitter.TabIndex = 9;
            // 
            // connectionImage
            // 
            this.connectionImage.Enabled = false;
            this.connectionImage.Location = new System.Drawing.Point(479, 3);
            this.connectionImage.Name = "connectionImage";
            this.connectionImage.Size = new System.Drawing.Size(24, 24);
            this.connectionImage.TabIndex = 4;
            this.connectionImage.TabStop = false;
            this.connectionImage.Visible = false;
            // 
            // ServerName
            // 
            this.ServerName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.ServerName.AutoEllipsis = true;
            this.ServerName.AutoSize = true;
            this.ServerName.Font = new System.Drawing.Font("DejaVu Sans Mono", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ServerName.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ServerName.Location = new System.Drawing.Point(3, 3);
            this.ServerName.Name = "ServerName";
            this.ServerName.Size = new System.Drawing.Size(179, 18);
            this.ServerName.TabIndex = 3;
            this.ServerName.Text = "No Server Connected";
            this.ServerName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // msgRecvBox
            // 
            this.msgRecvBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.msgRecvBox.BackColor = System.Drawing.Color.White;
            this.msgRecvBox.Font = new System.Drawing.Font("DejaVu Sans Mono", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.msgRecvBox.Location = new System.Drawing.Point(3, 26);
            this.msgRecvBox.Name = "msgRecvBox";
            this.msgRecvBox.ReadOnly = true;
            this.msgRecvBox.Size = new System.Drawing.Size(500, 274);
            this.msgRecvBox.TabIndex = 2;
            this.msgRecvBox.TabStop = false;
            this.msgRecvBox.Text = "";
            // 
            // serverListBox
            // 
            this.serverListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.serverListBox.Font = new System.Drawing.Font("DejaVu Sans Mono", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverListBox.FormattingEnabled = true;
            this.serverListBox.ItemHeight = 19;
            this.serverListBox.Items.AddRange(new object[] {
            "Channel List"});
            this.serverListBox.Location = new System.Drawing.Point(3, 11);
            this.serverListBox.Name = "serverListBox";
            this.serverListBox.Size = new System.Drawing.Size(207, 289);
            this.serverListBox.Sorted = true;
            this.serverListBox.TabIndex = 4;
            // 
            // ServerTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chatSplitter);
            this.Name = "ServerTab";
            this.Size = new System.Drawing.Size(729, 316);
            this.chatSplitter.Panel1.ResumeLayout(false);
            this.chatSplitter.Panel1.PerformLayout();
            this.chatSplitter.Panel2.ResumeLayout(false);
#if !MONO
            ((System.ComponentModel.ISupportInitialize)(this.chatSplitter)).EndInit();
#endif
            this.chatSplitter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.connectionImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.SplitContainer chatSplitter;
        public System.Windows.Forms.PictureBox connectionImage;
        public System.Windows.Forms.Label ServerName;
        public System.Windows.Forms.RichTextBox msgRecvBox;
        public System.Windows.Forms.ListBox serverListBox;


    }
}
