namespace IRC_Client
{
    partial class ChatTab
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
            this.ChannelTopic = new System.Windows.Forms.Label();
            this.msgRecvBox = new System.Windows.Forms.RichTextBox();
            this.userListBox = new System.Windows.Forms.ListBox();
            this.sendMsgButton = new System.Windows.Forms.Button();
            this.sendMsgBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.chatSplitter)).BeginInit();
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
            this.chatSplitter.Location = new System.Drawing.Point(-58, -1);
            this.chatSplitter.Name = "chatSplitter";
            // 
            // chatSplitter.Panel1
            // 
            this.chatSplitter.Panel1.Controls.Add(this.connectionImage);
            this.chatSplitter.Panel1.Controls.Add(this.ChannelTopic);
            this.chatSplitter.Panel1.Controls.Add(this.msgRecvBox);
            // 
            // chatSplitter.Panel2
            // 
            this.chatSplitter.Panel2.Controls.Add(this.userListBox);
            this.chatSplitter.Size = new System.Drawing.Size(776, 330);
            this.chatSplitter.SplitterDistance = 545;
            this.chatSplitter.TabIndex = 6;
            // 
            // connectionImage
            // 
            this.connectionImage.Enabled = false;
            this.connectionImage.Location = new System.Drawing.Point(515, 6);
            this.connectionImage.Name = "connectionImage";
            this.connectionImage.Size = new System.Drawing.Size(27, 28);
            this.connectionImage.TabIndex = 4;
            this.connectionImage.TabStop = false;
            this.connectionImage.Visible = false;
            this.connectionImage.Paint += new System.Windows.Forms.PaintEventHandler(this.connectionImage_Paint);
            // 
            // ChannelTopic
            // 
            this.ChannelTopic.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.ChannelTopic.AutoEllipsis = true;
            this.ChannelTopic.AutoSize = true;
            this.ChannelTopic.Font = new System.Drawing.Font("DejaVu Sans Mono", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChannelTopic.Location = new System.Drawing.Point(61, 15);
            this.ChannelTopic.Name = "ChannelTopic";
            this.ChannelTopic.Size = new System.Drawing.Size(169, 19);
            this.ChannelTopic.TabIndex = 3;
            this.ChannelTopic.Text = "No Channel Topic";
            this.ChannelTopic.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // msgRecvBox
            // 
            this.msgRecvBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.msgRecvBox.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.msgRecvBox.Font = new System.Drawing.Font("DejaVu Sans Mono", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.msgRecvBox.Location = new System.Drawing.Point(61, 37);
            this.msgRecvBox.Name = "msgRecvBox";
            this.msgRecvBox.ReadOnly = true;
            this.msgRecvBox.Size = new System.Drawing.Size(481, 294);
            this.msgRecvBox.TabIndex = 2;
            this.msgRecvBox.TabStop = false;
            this.msgRecvBox.Text = "";
            this.msgRecvBox.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.msgRecvBox_LinkClicked);
            // 
            // userListBox
            // 
            this.userListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.userListBox.Font = new System.Drawing.Font("DejaVu Sans Mono", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userListBox.FormattingEnabled = true;
            this.userListBox.ItemHeight = 19;
            this.userListBox.Location = new System.Drawing.Point(4, 23);
            this.userListBox.Name = "userListBox";
            this.userListBox.Size = new System.Drawing.Size(223, 308);
            this.userListBox.Sorted = true;
            this.userListBox.TabIndex = 4;
            // 
            // sendMsgButton
            // 
            this.sendMsgButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sendMsgButton.Font = new System.Drawing.Font("DejaVu Sans Mono", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sendMsgButton.Location = new System.Drawing.Point(657, 332);
            this.sendMsgButton.Name = "sendMsgButton";
            this.sendMsgButton.Size = new System.Drawing.Size(61, 26);
            this.sendMsgButton.TabIndex = 1;
            this.sendMsgButton.Text = "Send";
            this.sendMsgButton.UseVisualStyleBackColor = true;
            this.sendMsgButton.Click += new System.EventHandler(this.sendMsgButton_Click);
            // 
            // sendMsgBox
            // 
            this.sendMsgBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sendMsgBox.Font = new System.Drawing.Font("DejaVu Sans Mono", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sendMsgBox.Location = new System.Drawing.Point(3, 332);
            this.sendMsgBox.Name = "sendMsgBox";
            this.sendMsgBox.Size = new System.Drawing.Size(645, 26);
            this.sendMsgBox.TabIndex = 0;
            this.sendMsgBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.sendMsgBox_DragDrop);
            this.sendMsgBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.sendMsgBox_KeyDown);
            // 
            // ChatTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.sendMsgButton);
            this.Controls.Add(this.sendMsgBox);
            this.Controls.Add(this.chatSplitter);
            this.DoubleBuffered = true;
            this.Name = "ChatTab";
            this.Size = new System.Drawing.Size(729, 358);
            this.chatSplitter.Panel1.ResumeLayout(false);
            this.chatSplitter.Panel1.PerformLayout();
            this.chatSplitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chatSplitter)).EndInit();
            this.chatSplitter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.connectionImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.SplitContainer chatSplitter;
        public System.Windows.Forms.RichTextBox msgRecvBox;
        public System.Windows.Forms.ListBox userListBox;
        public System.Windows.Forms.Button sendMsgButton;
        public System.Windows.Forms.TextBox sendMsgBox;
        public System.Windows.Forms.Label ChannelTopic;
        public System.Windows.Forms.PictureBox connectionImage;
    }
}
