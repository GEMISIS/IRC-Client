namespace IRC_Client
{
    partial class JoinChannelForm
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
            this.channelBox = new System.Windows.Forms.TextBox();
            this.joinButton = new System.Windows.Forms.Button();
            this.channelLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // channelBox
            // 
            this.channelBox.Location = new System.Drawing.Point(12, 32);
            this.channelBox.Name = "channelBox";
            this.channelBox.Size = new System.Drawing.Size(174, 20);
            this.channelBox.TabIndex = 0;
            this.channelBox.Text = "#testChannel12345";
            // 
            // joinButton
            // 
            this.joinButton.Location = new System.Drawing.Point(63, 58);
            this.joinButton.Name = "joinButton";
            this.joinButton.Size = new System.Drawing.Size(75, 23);
            this.joinButton.TabIndex = 1;
            this.joinButton.Text = "Join";
            this.joinButton.UseVisualStyleBackColor = true;
            this.joinButton.Click += new System.EventHandler(this.joinButton_Click);
            // 
            // channelLabel
            // 
            this.channelLabel.AutoSize = true;
            this.channelLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.channelLabel.Location = new System.Drawing.Point(43, 9);
            this.channelLabel.Name = "channelLabel";
            this.channelLabel.Size = new System.Drawing.Size(114, 20);
            this.channelLabel.TabIndex = 2;
            this.channelLabel.Text = "Channel Name";
            // 
            // joinChannelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(198, 92);
            this.Controls.Add(this.channelLabel);
            this.Controls.Add(this.joinButton);
            this.Controls.Add(this.channelBox);
            this.Name = "joinChannelForm";
            this.Text = "Channel";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox channelBox;
        private System.Windows.Forms.Button joinButton;
        private System.Windows.Forms.Label channelLabel;
    }
}