using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IRC_Client
{
    /// <summary>
    /// Prompts the user for an irc channel to join.
    /// </summary>
    public partial class joinChannelForm : Form
    {
        /// <summary>
        /// The irc channel to join.
        /// </summary>
        public string channel = "#testChannel12345";

        /// <summary>
        /// The constructor for the join channel form.
        /// Sets the channel textbox keydown event.
        /// </summary>
        public joinChannelForm()
        {
            InitializeComponent();

            // Set the results of the dialog to cancle by default.
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;

            // Set the channel text box keydown event.
            channelBox.KeyDown += new KeyEventHandler(channelBox_KeyDown);
        }

        /// <summary>
        /// The keydown event for the channel box.  Check
        /// for the enter key and pressed the join button if it's pressed.
        /// </summary>
        /// <param name="sender">Not used.</param>
        /// <param name="e">The event arguments for key pressed</param>
        private void channelBox_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if the enter key is pressed.
            if (e.KeyData.Equals(Keys.Enter))
            {
                // If so, call the join button click method.
                joinButton_Click(null, EventArgs.Empty);
            }
            // Check for the escape key being pressed.
            if (e.KeyCode.Equals(Keys.Escape))
            {
                // Just close the form if so.
                this.Close();
            }
        }

        /// <summary>
        /// Sets the channel to join and closes the form.
        /// </summary>
        /// <param name="sender">Not used.</param>
        /// <param name="e">Not used.</param>
        private void joinButton_Click(object sender, EventArgs e)
        {
            // Set the channel.
            channel = channelBox.Text;
            // Set the results of the dialog to ok.
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            // Close the form.
            this.Close();
        }
    }
}
