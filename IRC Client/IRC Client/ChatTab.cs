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
    public partial class ChatTab : UserControl
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
        public ToolStripMenuItem leaveChannelToolStripMenuItem, allToolStripMenuItem;

        public Chat_Notifications notification = Chat_Notifications.None;
        private Timer checkConnectionTimer;
        private Timer pingTimer;
        private TimeSpan pingTimeSpan;

        public ChatTab()
        {
            InitializeComponent();

            pingTimer = new Timer();
            pingTimer.Interval = 1;
            pingTimer.Tick += new EventHandler(pingTimer_Tick);

            checkConnectionTimer = new Timer();
            checkConnectionTimer.Interval = 60 * 1000;
            checkConnectionTimer.Tick += new EventHandler(checkConnectionTimer_Tick);
        }

        private void checkConnectionTimer_Tick(object sender, EventArgs e)
        {
            pingTimer.Start();
            mainSocket.Send(Encoding.UTF8.GetBytes("PING " + this.userInfo.server + "\r\n"));
        }

        private TimeSpan milliSpan = new TimeSpan(0, 0, 0, 0, 1);
        private void pingTimer_Tick(object sender, EventArgs e)
        {
            pingTimeSpan = pingTimeSpan.Add(milliSpan);
        }

        private void connectionImage_Paint(object sender, PaintEventArgs e)
        {
            Color connectionColor = Color.Green;
            if (pingTimeSpan.Milliseconds < 100)
            {
                connectionColor = Color.Green;
            }
            else if (pingTimeSpan.Milliseconds < 500)
            {
                connectionColor = Color.Yellow;
            }
            else
            {
                connectionColor = Color.Red;
            }
            Pen pencil = new Pen(connectionColor);
            Brush brush = pencil.Brush;
            e.Graphics.FillEllipse(brush, 0, 0, connectionImage.Size.Width - 1, connectionImage.Size.Height - 1);
        }

        public void pingDone()
        {
            if (pingTimer.Enabled)
            {
                pingTimer.Stop();
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
                System.Net.WebRequest request = System.Net.WebRequest.Create(e.LinkText);
                request.Timeout = 5000;
                using (System.Net.WebResponse response = request.GetResponse())
                {
                    using (response.GetResponseStream())
                    {
                        if (response.ContentType.Contains("image"))
                        {
                            ImagePreview prev = new ImagePreview(e.LinkText);
                            prev.Show();
                        }
                        else
                        {
                            System.Diagnostics.Process.Start(e.LinkText);
                        }
                    }
                }
            }
            catch (Exception)
            {
                System.Diagnostics.Process.Start(e.LinkText);
            }
        }

        private void userListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.userListBox.SelectedIndex > -1)
            {
                server.newPrivateChat(this.userListBox.SelectedItem.ToString());

                // If so, create a new tool strip item.
                ToolStripItem newLeaveChannelItem = new ToolStripButton(this.userListBox.SelectedItem.ToString());
                // Update the click method for the item.
                newLeaveChannelItem.Click += new EventHandler(newLeaveChannelItem_Click);
                // Add the item to the leave channel dropdown menu.
                leaveChannelToolStripMenuItem.DropDownItems.Add(newLeaveChannelItem);
                
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
