using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace IRC_Client
{
    public partial class mainForm : Form
    {
        /// <summary>
        /// The constructor for the main form.
        /// </summary>
        public mainForm()
        {
            InitializeComponent();

            // Initialize the current server with the current channel tabs.
            this.currentServer.Initialize(this.channelTabs);

            // Set the event for when a tab is changed.
            channelTabs.Selecting += new TabControlCancelEventHandler(channelTabs_Selecting);
        }

        /// <summary>
        /// used for when one of the channel tabs is changed.
        /// </summary>
        /// <param name="sender">Not used.</param>
        /// <param name="e">Used to get the new index of the tab.</param>
        void channelTabs_Selecting(object sender, TabControlCancelEventArgs e)
        {
            // Check that the tab selected is not the server tab.
            if (e.TabPageIndex > 0)
            {
                // Set the current channel to the selected tab.
                currentServer.currentChannel = (ChannelTab)currentServer.channels[channelTabs.TabPages[e.TabPageIndex].Text.ToLower()];
                // Set the notifactions to none.
                currentServer.currentChannel.notification = Chat_Notifications.None;
                // Refresh the tabs to force them to redraw.
                channelTabs.Refresh();
            }
        }

        /// <summary>
        /// The method called when the main form is closing.
        /// </summary>
        /// <param name="sender">Not used.</param>
        /// <param name="e">Not used.</param>
        void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Send the quit message and disconnect form the main socket.
            currentServer.mainSocket.Send(Encoding.UTF8.GetBytes("QUIT\r\n"));
            currentServer.mainSocket.Disconnect(false);
        }

        /// <summary>
        /// Draws the tabs for the channels.
        /// </summary>
        /// <param name="sender">Not used.</param>
        /// <param name="e">Used to get what tab is being drawn.</param>
        private void channelTabs_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Create a temporary bounds value.
            Rectangle paddedBounds = e.Bounds;
            // Inflate the value by -2 to display the text within the tab properly.
            paddedBounds.Inflate(-2, -2);
            
            // Create a new pen that is black.
            Pen pen = new Pen(Color.Black);
            
            // Fill the tab's rectangle with the back color of the form.
            e.Graphics.FillRectangle(new SolidBrush(this.BackColor), e.Bounds);

            // Check that the selecated tab to draw is a channel tab.
            if (e.Index > 0)
            {
                // Loop through the notifications for the tab.
                switch (((ChannelTab)currentServer.channels[channelTabs.TabPages[e.Index].Text.ToLower()]).notification)
                {
                    // Set the pen color to blue for a message.
                    case Chat_Notifications.Message:
                        pen = new Pen(Color.Blue);
                        break;
                    // Set the pen color to green for a user who has joined.
                    case Chat_Notifications.UserJoined:
                        pen = new Pen(Color.Green);
                        break;
                    // Set the pen color to red for a user who has left.
                    case Chat_Notifications.UserLeft:
                        pen = new Pen(Color.Red);
                        break;
                    // Set the pen color to black by default.
                    default:
                        pen = new Pen(Color.Black);
                        break;
                }
            }

            // Draw the tab's text in the chosen color.
            e.Graphics.DrawString(channelTabs.TabPages[e.Index].Text.TrimEnd(new char[] { ' ' }), this.Font, pen.Brush, paddedBounds);
        }

        /// <summary>
        /// Used for leaving all channels in the current server.
        /// </summary>
        /// <param name="sender">Not used.</param>
        /// <param name="e">Not used.</param>
        private void allToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Got the last tab.
            channelTabs.SelectedIndex = channelTabs.TabCount - 1;

            // Loop while not on the first tab (the server tab).
            while (channelTabs.SelectedIndex > 0)
            {
                // Save the current tab index.
                int currentTabIndex = channelTabs.SelectedIndex;

                // Remove the channel from the list of channels that can be left.
                leaveChannelToolStripMenuItem.DropDownItems.RemoveByKey(currentServer.currentChannel.joinChannelForm.channel);
                // Then actually leave the channel.
                currentServer.leaveCurrentChannel();

                // Set the current tab index to the next tab.
                channelTabs.SelectedIndex = currentTabIndex - 1;
            }

            // Disable the leave all tabs button.
            allToolStripMenuItem.Enabled = false;
        }

        /// <summary>
        /// Joins a new channel on the server.
        /// </summary>
        /// <param name="sender">Not used.</param>
        /// <param name="e">Not used.</param>
        private void joinChannelFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Check if the user put in valid channel information.
            if (currentServer.joinChannel())
            {
                // If so, create a new tool strip item.
                ToolStripItem newLeaveChannelItem = new ToolStripButton(currentServer.joinChannelForm.channel);
                // Update the click method for the item.
                newLeaveChannelItem.Click += new EventHandler(newLeaveChannelItem_Click);
                // Add the item to the leave channel dropdown menu.
                leaveChannelToolStripMenuItem.DropDownItems.Add(newLeaveChannelItem);

                // Set the leave channel tool strip item for the current channel as well as for
                // the all tool strip item.
                currentServer.currentChannel.leaveChannelToolStripMenuItem = leaveChannelToolStripMenuItem;
                currentServer.currentChannel.allToolStripMenuItem = allToolStripMenuItem;

                // Enable the leave all channels button.
                allToolStripMenuItem.Enabled = true;
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
            int oldTabIndex = channelTabs.SelectedIndex;

            // Loop through all of the tabs after the server tab.
            for (int i = 1; i < channelTabs.TabCount; i += 1)
            {
                // Check if the tab contains the channel to leave.
                if (((ToolStripItem)sender).Text.ToLower().Equals(channelTabs.TabPages[i].Text.ToLower()))
                {
                    // If so, select that tab and break out of the loop.
                    channelTabs.SelectedIndex = i;
                    break;
                }
            }

            // Save the current tab index.
            int currentTabIndex = channelTabs.SelectedIndex;

            // Remove the item from the list of channels to leave.
            leaveChannelToolStripMenuItem.DropDownItems.Remove(((ToolStripItem)sender));
            // Then actually leave the channel.
            currentServer.leaveCurrentChannel();

            // Check if the old tab is greater than or equal to the currently selected one.
            if (oldTabIndex >= currentTabIndex)
            {
                // Set the current tab to the tab prior to the recently closed one if so.
                channelTabs.SelectedIndex = oldTabIndex - 1;
            }
            else
            {
                // Otherwise, keep the same tab selected.
                channelTabs.SelectedIndex = oldTabIndex;
            }

            // Check if there is more than one tab (IE: if it has channel tabs).
            if (channelTabs.TabCount > 1)
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

        /// <summary>
        /// Used for when the exit button is clicked.
        /// </summary>
        /// <param name="sender">Not used.</param>
        /// <param name="e">Not used.</param>
        private void closeChatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Simply close the form.
            this.Close();
        }
    }
}
