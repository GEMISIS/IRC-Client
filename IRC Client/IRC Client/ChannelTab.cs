using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;

namespace IRC_Client
{
    /// <summary>
    /// Used for notifications about the channel.
    /// </summary>
    public enum Chat_Notifications
    {
        // None for no message, Message for a message from a channel, UserJoined for when
        // a user joins a channel, and UserLeft for when a user leaves a channel.
        None, Message, UserJoined, UserLeft
    }

    /// <summary>
    /// Channel tabs are used for creating a tab that connects to a specific channel
    /// on an IRC server.  They contain a variety of controls for interacting with
    /// the channel.
    /// </summary>
    public partial class ChannelTab : UserControl
    {
        /// <summary>
        /// The server that this tab is under.
        /// </summary>
        public ServerTab server;
        /// <summary>
        /// The form used to get information about the user
        /// for connecting. (Username, IRC, etc.)
        /// </summary>
        public UserInfoForm userInfo;
        /// <summary>
        /// The form used to join an IRC channel.
        /// </summary>
        public JoinChannelForm joinChannelForm = null;

        /// <summary>
        /// The main socket used to transfer data to and from the server.
        /// </summary>
        public Socket mainSocket;

        /// <summary>
        /// The list of channels that can be left.  This is changed when
        /// another user's name is clicked, as it creates a new private chat
        /// tab.
        /// </summary>
        public ToolStripMenuItem leaveChannelToolStripMenuItem;

        /// <summary>
        /// This is changed when a channel is left.  It is a button for leaving
        /// all of the channels on the server.
        /// </summary>
        public ToolStripMenuItem allToolStripMenuItem;

        /// <summary>
        /// The notification for the tab.  This gives information about the tab
        /// when the user is not on the tab.
        /// </summary>
        public Chat_Notifications notification = Chat_Notifications.None;

        /// <summary>
        /// A timer used to check the connection strength.  Updates every 5 seconds.
        /// </summary>
        private Timer checkConnectionTimer;
        /// <summary>
        /// A timer that counts how long it has been since a ping was sent.
        /// </summary>
        private Timer pingTimer;
        /// <summary>
        /// The time span for how long it has been since a ping was sent.
        /// </summary>
        private TimeSpan pingTimeSpan;
        /// <summary>
        /// This contains the time of one millisecond.  This
        /// is added to the ping time span each time the timer ticks.
        /// </summary>
        private TimeSpan millisecondSpan = new TimeSpan(0, 0, 0, 0, 1);

        /// <summary>
        /// The constructor for a channel tab.  Initializes the timers for the tab.
        /// </summary>
        public ChannelTab()
        {
            InitializeComponent();

            // Create the ping timer.
            pingTimer = new Timer();
            pingTimer.Interval = 1;
            pingTimer.Tick += new EventHandler(pingTimer_Tick);

            // Create the connection timer.
            checkConnectionTimer = new Timer();
            checkConnectionTimer.Interval = 60 * 1000;
            checkConnectionTimer.Tick += new EventHandler(checkConnectionTimer_Tick);
        }

        /// <summary>
        /// Used for checking the connection to the server.
        /// Starts the ping timer and sends a ping message to the server.
        /// </summary>
        /// <param name="sender">Not used.</param>
        /// <param name="e">Not used.</param>
        private void checkConnectionTimer_Tick(object sender, EventArgs e)
        {
            // Reset the ping time span.
            pingTimeSpan = new TimeSpan();
            // Start the ping timer first.
            pingTimer.Start();
            // Then send a ping to the server.
            mainSocket.Send(Encoding.UTF8.GetBytes("PING " + this.userInfo.server + "\r\n"));
        }

        /// <summary>
        /// Adds one millisecond to the ping time span on each
        /// pass through.
        /// </summary>
        /// <param name="sender">Not used.</param>
        /// <param name="e">Not used.</param>
        private void pingTimer_Tick(object sender, EventArgs e)
        {
            // Add the millisecond to the ping time span and set it.
            pingTimeSpan = pingTimeSpan.Add(millisecondSpan);
        }

        /// <summary>
        /// Repaints the connection image.  Green indicates good strength, yellow is warning
        /// level, and red means a bad connection.
        /// </summary>
        /// <param name="sender">Not used.</param>
        /// <param name="e">Used to get the graphics to draw to.</param>
        private void connectionImage_Paint(object sender, PaintEventArgs e)
        {
            // The current color of the connection.  Set to good by default.
            Color connectionColor = Color.Green;

            // Check if the ping reponse took less than 100ms to receive.
            if (pingTimeSpan.Milliseconds < 100)
            {
                // If so, that means the user has a good connection.
                connectionColor = Color.Green;
            }
            // Check if the ping response took less than 500ms but more than 100ms to receive.
            else if (pingTimeSpan.Milliseconds < 500)
            {
                // If so, that means the user has an ok connection.
                connectionColor = Color.Yellow;
            }
            else
            {
                // Otherwise, the user has a bad connection, so it is set to red.
                connectionColor = Color.Red;
            }

            // Create a pen of that color.
            Pen pen = new Pen(connectionColor);
            // Then create a brush from that pen.
            Brush brush = pen.Brush;
            // Finally, draw a circle within the image of the color specified by the connection strength.
            e.Graphics.FillEllipse(brush, 0, 0, connectionImage.Size.Width - 1, connectionImage.Size.Height - 1);
        }

        /// <summary>
        /// Signals that the ping message was recieved.
        /// </summary>
        public void pingDone()
        {
            // Checks if the ping timer is enabled.
            if (pingTimer.Enabled)
            {
                // If so, stop the timer.
                pingTimer.Stop();
                // Then refresh the connection image to force it to redraw.
                connectionImage.Refresh();
            }
        }

        /// <summary>
        /// Manages key presses for the send message text box.
        /// </summary>
        /// <param name="sender">Not used.</param>
        /// <param name="e">The key event arguments used to get the keys pressed.</param>
        private void sendMsgBox_KeyDown(object sender, KeyEventArgs e)
        {
            // If the enter key is being pressed, send the message.
            if (e.KeyData.Equals(Keys.Enter))
            {
                sendMsgButton_Click(null, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Used for when the send message button is clicked.  This sends
        /// a message to the IRC channel that is currently joined.
        /// </summary>
        /// <param name="sender">Not used.</param>
        /// <param name="e">Not used.</param>
        private void sendMsgButton_Click(object sender, EventArgs e)
        {
            // Check if a user is selected.
            if (userListBox.SelectedIndex != -1)
            {
                // If there is a specific user selected, send them a private message.
                mainSocket.Send(Encoding.UTF8.GetBytes("PRIVMSG " + userListBox.Items[userListBox.SelectedIndex] + " :" + sendMsgBox.Text + "\r\n"));
            }
            else
            {
                // If not, send it to the current channel.
                mainSocket.Send(Encoding.UTF8.GetBytes("PRIVMSG " + joinChannelForm.channel + " :" + sendMsgBox.Text + "\r\n"));
            }

            // Post the message on the user's message text box.
            msgRecvBox.Text += userInfo.username + ": " + sendMsgBox.Text + "\n";
            // Set the carret's scroll position.
            msgRecvBox.SelectionStart = msgRecvBox.Text.Length - 1;
            // Then scroll to the carret.
            msgRecvBox.ScrollToCaret();

            // Clear the send message text box.
            sendMsgBox.Text = "";
            // Then set the send message text box to be the current focused component.
            sendMsgBox.Focus();
        }

        /// <summary>
        /// Used for when a link is clicked.
        /// </summary>
        /// <param name="sender">Note used.</param>
        /// <param name="e">Used for getting the link text.</param>
        private void msgRecvBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                // Create a web request to the link.
                System.Net.WebRequest request = System.Net.WebRequest.Create(e.LinkText);
                // Set the timeout to 5 seconds.
                request.Timeout = 5000;

                // Use a web response and store the response from the server.
                using (System.Net.WebResponse response = request.GetResponse())
                {
                    // Check that there is a response stream.
                    using (response.GetResponseStream())
                    {
                        // Check if the content type is an image.
                        if (response.ContentType.Contains("image"))
                        {
                            // If so, create a new image preview form using the image link.
                            ImagePreview prev = new ImagePreview(e.LinkText);
                            // Then show the new image preview form.
                            prev.Show();
                        }
                        else
                        {
                            // Otherwise, start the link in the user's default browser. (IE: start it as a process)
                            System.Diagnostics.Process.Start(e.LinkText);
                        }
                    }
                }
            }
            catch (Exception)
            {
                // If there is an exception, then it probably means that the link was not to an image, in
                // which case the link is opened in the user's default browser.
                System.Diagnostics.Process.Start(e.LinkText);
            }
        }

        /// <summary>
        /// Used for when the user selects another user in the user list box.
        /// </summary>
        /// <param name="sender">Not used.</param>
        /// <param name="e">Not used.</param>
        private void userListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Check that the selected index is not -1.
            if (this.userListBox.SelectedIndex > -1)
            {
                // Start a new private chat with the selected username.
                server.newPrivateChat(this.userListBox.SelectedItem.ToString());

                // Create a new tool strip item for the private chat.
                ToolStripItem newLeaveChannelItem = new ToolStripButton(this.userListBox.SelectedItem.ToString());
                // Update the click method for the item.
                newLeaveChannelItem.Click += new EventHandler(newLeaveChannelItem_Click);
                // Add the item to the leave channel dropdown menu.
                leaveChannelToolStripMenuItem.DropDownItems.Add(newLeaveChannelItem);
                
                // Set the selected index to -1.
                this.userListBox.SelectedIndex = -1;
            }
        }
 
        /// <summary>
        /// Used for when leaving a specific channel.  This is applied to all of the
        /// leave channel buttons, as it uses the text from the button itself to leave
        /// from the correct tab.
        /// </summary>
        /// <param name="sender">Not used.</param>
        /// <param name="e">Not used.</param>
        void newLeaveChannelItem_Click(object sender, EventArgs e)
        {
            // Save the current tab index.
            int oldTabIndex = server.channelTabs.SelectedIndex;

            // Loop through all of the tabs after the server tab.
            for (int i = 1; i < server.channelTabs.TabCount; i += 1)
            {
                // Check if the tab contains the channel to leave.
                if (((ToolStripItem)sender).Text.ToLower().Equals(server.channelTabs.TabPages[i].Text.ToLower()))
                {
                    // If so, select that tab and break out of the loop.
                    server.channelTabs.SelectedIndex = i;
                    break;
                }
            }

            // Save the current tab index.
            int currentTabIndex = server.channelTabs.SelectedIndex;

            // Remove the item from the list of channels to leave.
            leaveChannelToolStripMenuItem.DropDownItems.Remove(((ToolStripItem)sender));
            // Then actually leave the channel.
            server.leaveCurrentChannel();

            // Check if the old tab is greater than or equal to the currently selected one.
            if (oldTabIndex >= currentTabIndex)
            {
                // Set the current tab to the tab prior to the recently closed one if so.
                server.channelTabs.SelectedIndex = oldTabIndex - 1;
            }
            else
            {
                // Otherwise, keep the same tab selected.
                server.channelTabs.SelectedIndex = oldTabIndex;
            }

            // Check if there is more than one tab (IE: if it has channel tabs).
            if (server.channelTabs.TabCount > 1)
            {
                // Enable the leave all channels button if so.
                allToolStripMenuItem.Enabled = true;
            }
            else
            {
                // Disable the leave all channels button otherwise.
                allToolStripMenuItem.Enabled = false;
            }
        }
    }
}
