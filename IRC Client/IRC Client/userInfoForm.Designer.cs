namespace IRC_Client
{
    partial class userInfoForm
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
            this.usernameBox = new System.Windows.Forms.TextBox();
            this.usernameLabel = new System.Windows.Forms.Label();
            this.continueButton = new System.Windows.Forms.Button();
            this.serverLabel = new System.Windows.Forms.Label();
            this.serverBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // usernameBox
            // 
            this.usernameBox.Location = new System.Drawing.Point(154, 8);
            this.usernameBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.usernameBox.Name = "usernameBox";
            this.usernameBox.Size = new System.Drawing.Size(103, 26);
            this.usernameBox.TabIndex = 0;
            this.usernameBox.Text = "testUser12345";
            // 
            // usernameLabel
            // 
            this.usernameLabel.AutoSize = true;
            this.usernameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.usernameLabel.Location = new System.Drawing.Point(18, 14);
            this.usernameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.usernameLabel.Name = "usernameLabel";
            this.usernameLabel.Size = new System.Drawing.Size(128, 20);
            this.usernameLabel.TabIndex = 3;
            this.usernameLabel.Text = "Input Username:";
            // 
            // continueButton
            // 
            this.continueButton.Location = new System.Drawing.Point(81, 86);
            this.continueButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.continueButton.Name = "continueButton";
            this.continueButton.Size = new System.Drawing.Size(112, 35);
            this.continueButton.TabIndex = 2;
            this.continueButton.Text = "Continue";
            this.continueButton.UseVisualStyleBackColor = true;
            this.continueButton.Click += new System.EventHandler(this.continueButton_Click);
            // 
            // serverLabel
            // 
            this.serverLabel.AutoSize = true;
            this.serverLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverLabel.Location = new System.Drawing.Point(18, 45);
            this.serverLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.serverLabel.Name = "serverLabel";
            this.serverLabel.Size = new System.Drawing.Size(105, 20);
            this.serverLabel.TabIndex = 4;
            this.serverLabel.Text = "Server Name:";
            // 
            // serverBox
            // 
            this.serverBox.Location = new System.Drawing.Point(131, 39);
            this.serverBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.serverBox.Name = "serverBox";
            this.serverBox.Size = new System.Drawing.Size(126, 26);
            this.serverBox.TabIndex = 1;
            this.serverBox.Text = "irc.freenode.net";
            // 
            // userInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(263, 135);
            this.Controls.Add(this.serverBox);
            this.Controls.Add(this.serverLabel);
            this.Controls.Add(this.continueButton);
            this.Controls.Add(this.usernameLabel);
            this.Controls.Add(this.usernameBox);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "userInfoForm";
            this.Text = "User Information";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox usernameBox;
        private System.Windows.Forms.Label usernameLabel;
        private System.Windows.Forms.Button continueButton;
        private System.Windows.Forms.Label serverLabel;
        private System.Windows.Forms.TextBox serverBox;
    }
}