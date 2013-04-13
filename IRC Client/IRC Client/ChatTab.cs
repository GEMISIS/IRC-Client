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
    public enum Chat_Notifications
    {
        None, Message, UserJoined, UserLeft
    }
    public partial class ChatTab : UserControl
    {
        /// <summary>
        /// The form used to get information about the user
        /// for connecting. (Username, IRC, etc.)
        /// </summary>
        public userInfoForm userInfo;
        /// <summary>
        /// The form used to join an IRC channel.
        /// </summary>
        public joinChannelForm joinChannel = null;
        /// <summary>
        /// The main socket used to transfer data to and from the server.
        /// </summary>
        public Socket mainSocket;

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
                mainSocket.Send(Encoding.UTF8.GetBytes("PRIVMSG " + joinChannel.channel + " :" + sendMsgBox.Text + "\r\n"));
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

        private void sendMsgBox_DragDrop(object sender, DragEventArgs e)
        {
        }
    }
}
