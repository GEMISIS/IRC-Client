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
        /// The form used to get information about the user
        /// for connecting. (Username, IRC, etc.)
        /// </summary>
        private userInfoForm userInfo;
        /// <summary>
        /// The form used to join an IRC channel.
        /// </summary>
        private joinChannelForm joinChannel = null;

        /// <summary>
        /// The main socket used to transfer data to and from the server.
        /// </summary>
        private Socket mainSocket;
        const int BUFFER_LENGTH = 32;
        /// <summary>
        /// The recieved from the server.
        /// </summary>
        private Byte[] data = new Byte[BUFFER_LENGTH];

        private StringBuilder dataString;

        private SortedDictionary<string, ChatTab> tabs;
        ChatTab currentTab;

        /// <summary>
        /// THe get data delegate.  Used for whenever data is recieved.
        /// </summary>
        /// <param name="receivedData">The data recieved from the server.</param>
        delegate void GetData(string receivedData);
        /// <summary>
        /// Manages the data recieved from the server.
        /// </summary>
        private event GetData manageData;

        /// <summary>
        /// The list of invalide character for a username.
        /// </summary>
        char[] usernameInvalidChars = { '+', '~', '@', '#', '%', '&' };


        /// <summary>
        /// The constructor for the main form.
        /// </summary>
        public mainForm()
        {
            InitializeComponent();

            dataString = new StringBuilder();

            // Create the user info form and actually show it.
            userInfo = new userInfoForm();
            userInfo.ShowDialog();

            // The key down event handler for the command box.
            commandBox.KeyDown += new KeyEventHandler(commandBox_KeyDown);

            // Setup the list of tabs.
            tabs = new SortedDictionary<string, ChatTab>();

            // Set the event for when a tab is changed.
            channelTabs.Selecting += new TabControlCancelEventHandler(channelTabs_Selecting);

            // Setup the manage data method to handle any data recieved
            // from the server.
            manageData = new GetData(recievedData);
            // Setup the connection to the server's socket.
            mainSocket = connectToServer(userInfo.server, 6667);
            currentTab.mainSocket = mainSocket;
            // Set the new size to the size of the recieved buffer data.
            data = new byte[BUFFER_LENGTH];
            // Set the callback method for when data is recieved.
            mainSocket.BeginReceive(data, 0, data.Length, SocketFlags.None, new AsyncCallback(dataRecieved), mainSocket);

            // Set the form closing event.  This makes sure to disconnect all of the sockets.
            this.FormClosing += new FormClosingEventHandler(mainForm_FormClosing);

            channelTabs.DrawMode = TabDrawMode.OwnerDrawFixed;
        }

        void channelTabs_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (channelTabs.SelectedIndex > -1)
            {
                currentTab = tabs[channelTabs.TabPages[channelTabs.SelectedIndex].Text.ToLower()];
                currentTab.notification = Chat_Notifications.None;
            }
            if (channelTabs.SelectedIndex > 0)
            {
                leaveButton.Enabled = true;
            }
            else
            {
                leaveButton.Enabled = false;
            }
        }

        /// <summary>
        /// Used for when the command text box has the enter key pressed.
        /// </summary>
        /// <param name="sender">Not used.</param>
        /// <param name="e">Used to get the key arguments for what key is being pressed.</param>
        void commandBox_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if the enter key is being pressed.
            if (e.KeyData.Equals(Keys.Enter))
            {
                // If so, convert the command  in the text box to an array
                // of bytes, ending with \r\n, and send it over.
                mainSocket.Send(Encoding.UTF8.GetBytes("" + commandBox.Text + "\r\n"));
                // Set the command text box to be empty.
                commandBox.Text = string.Empty;
            }
            // Check if the control key is being held.
            if (e.Modifiers == Keys.Control)
            {
                // If the J key is also being pressed.
                if (e.KeyCode == Keys.J)
                {
                    // Check if the user can join a channel.
                    if (joinChannelButton.Enabled)
                    {
                        // If so, join a new IRC channel on the server.
                        joinChannelButton_Click(null, EventArgs.Empty);
                    }
                }
                // Check if the L key is also being pressed.
                if (e.KeyCode == Keys.L)
                {
                    // Check if the leave channel button is enabled.
                    if (leaveButton.Enabled)
                    {
                        // If so, leave the current IRC channel.
                        leaveButton_Click(null, EventArgs.Empty);
                    }
                }
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
            mainSocket.Send(Encoding.UTF8.GetBytes("QUIT\r\n"));
            mainSocket.Disconnect(false);
        }

        /// <summary>
        /// Handles any data recieved form the server and responds accordingly
        /// to it.
        /// </summary>
        /// <param name="receivedData">The actual data recieved.</param>
        void recievedData(string dataRecieved)
        {
            string[] commands = dataRecieved.Split(new char[] {'\n'}, StringSplitOptions.RemoveEmptyEntries);
            foreach (string cmd in commands)
            {
                string command = cmd;
                if (command[0] != ':')
                {
                    string actualCommand = cmd.Split(new char[] { ' ' })[0];
                    string[] parameters = cmd.Substring(actualCommand.Length + 1).Split(new char[] { ' ' });
                    doCommand(actualCommand, parameters);
                    channelTabs.Refresh();
                }
                else
                {
                    command = command.Substring(1);
                    // Create a temporary channel name from the channel in the command.
                    string servername = command.Split(new char[] {' '})[0];
                    if (userInfo.server.Equals(servername))
                    {
                        command = command.Substring(command.IndexOf(' ') + 1);

                        string actualCommand = command.Split(new char[] {' '})[0];

                        command = command.Substring(actualCommand.Length);
                        command = command.Trim(new char[] { '\r', '\n' });
                        command = command.TrimStart(new char[] { ' ' });

                        string[] parameters = command.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

                        // Call the command.
                        doCommand(actualCommand, parameters);
                        channelTabs.Refresh();
                    }
                    else if (!servername.Split(new char[] { '!', '@' })[0].Equals(userInfo.username))
                    {
                        command = command.Substring(command.IndexOf(' ') + 1);

                        string actualCommand = command.Split(new char[] {' '})[0];

                        if (actualCommand.ToUpper().Equals("PRIVMSG") || actualCommand.ToUpper().Equals("JOIN") || actualCommand.ToUpper().Equals("PART") || actualCommand.ToUpper().Equals("KILLED") || actualCommand.ToUpper().Equals("QUIT"))
                        {
                            command = command.Substring(actualCommand.Length);
                            command = command.Trim(new char[] { '\r', '\n' });
                            command = command.TrimStart(new char[] { ' ' });

                            command += ":" + servername.Split(new char[] { '!', '@' })[0];
                            string[] parameters = command.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

                            if (tabs.ContainsKey(parameters[0].Split(' ')[0].ToLower()))
                            {
                                ChatTab oldTab = currentTab;
                                currentTab = tabs[parameters[0].Split(' ')[0].ToLower()];
                                // Call the command.
                                doCommand(actualCommand, parameters);
                                currentTab = oldTab;
                            }
                            else
                            {
                                // Call the command.
                                doCommand(actualCommand, parameters);
                            }
                            channelTabs.Refresh();
                        }
                    }
                }
            }
        }

        public void doCommand(string command, string[] parameters)
        {
            if (command.ToUpper().Equals("PING"))
            {
                // If a ping command is recieved, then the response of PONG : XXX with the XXX
                // being whatever was pinged.
                mainSocket.Send(Encoding.UTF8.GetBytes("PONG :" + parameters + "\r\n"));
            }
            else if (command.ToUpper().Equals("PONG"))
            {
                currentTab.pingDone();
            }
            else if (command.ToUpper().Equals("NOTICE"))
            {
                foreach (string param in parameters)
                {
                    if (param.Contains("response"))
                    {
                        // If the command contains the text "response", then the nickname and 
                        // username are sent over in that order.
                        mainSocket.Send(Encoding.UTF8.GetBytes("NICK " + userInfo.nickname + "\r\n"));
                        mainSocket.Send(Encoding.UTF8.GetBytes("USER " + userInfo.username + " 8 * :" + userInfo.realName + "\r\n"));
                        break;
                    }
                }
                // Otherwise, just print out the command if debugging and it is not a newline.
                currentTab.msgRecvBox.AppendText(command + " ");
                foreach (string param in parameters)
                {
                    currentTab.msgRecvBox.AppendText(param);
                }
                currentTab.msgRecvBox.Text += "\n";
            }
            else if (command.Equals("433"))
            {
                currentTab.msgRecvBox.Text += "Name already taken!\n";
                string server = userInfo.server;
                // Create the user info form and actually show it.
                userInfo = new userInfoForm();
                userInfo.ShowDialog();
                userInfo.server = server;
                // If the command contains the text "response", then the nickname and 
                // username are sent over in that order.
                mainSocket.Send(Encoding.UTF8.GetBytes("NICK " + userInfo.nickname + "\r\n"));
                mainSocket.Send(Encoding.UTF8.GetBytes("USER " + userInfo.username + " 8 * :" + userInfo.realName + "\r\n"));
            }
            else if (command.Equals("376"))
            {
                // If the text contains "End of /MOTD command." then
                // the user has successfully connected to the server
                // and can join a channel, so the join button is enabled.
                joinChannelButton.Enabled = true;
                currentTab.connectionImage.Enabled = true;
#if DEBUG
                // Otherwise, just print out the command if debugging and it is not a newline.
                currentTab.msgRecvBox.AppendText(command + "\n");
#endif
            }
            else if (command.Equals("332"))
            {
                currentTab.ChannelTopic.Text = "";
                for (int i = 1; i < parameters.Length; i += 1)
                {
                    currentTab.ChannelTopic.Text += parameters[i];
                }
            }
            else if (command.Equals("001") || command.Equals("002") ||
                command.Equals("003") || command.Equals("004") ||
                command.Equals("005") || command.Equals("250") ||
                command.Equals("251") || command.Equals("252") ||
                command.Equals("253") || command.Equals("254") ||
                command.Equals("255") || command.Equals("265") ||
                command.Equals("266") || command.Equals("372") ||
                command.Equals("375"))
            {
                // If so, filter out the username of whoever sent it, and
                // display the response in the message recieved text box.
                for (int i = 1; i < parameters.Length; i += 1)
                {
                    currentTab.msgRecvBox.AppendText(parameters[i] + ":");
                }
                currentTab.msgRecvBox.Text = currentTab.msgRecvBox.Text.TrimEnd(new char[] { ':' });
                currentTab.msgRecvBox.AppendText("\n");
                if (!command.Equals("372"))
                {
                    currentTab.msgRecvBox.AppendText("\n");
                }
            }
            else if (command.Equals("353"))
            {
                ChatTab oldTab = currentTab;
                if (tabs.ContainsKey(parameters[0].Split(' ')[3].ToLower()))
                {
                    currentTab = tabs[parameters[0].Split(' ')[3].ToLower()];
                }
                // If the command contains 353 and the join channel form isn't null, then
                // the user just joined a channel and recieved the username list, thus
                // it must be updated.
                // Clear the old username list.
                currentTab.userListBox.Items.Clear();
                // Split the list of users in the room.
                string[] userList = parameters[1].Split(' ');
                // Add the channel to the list of available chats.
                currentTab.userListBox.Items.Add(currentTab.joinChannel.channel);
                // Now loop through the list of users.
                foreach (string username in userList)
                {
                    // Check for any invalid characters at the beginning of
                    // the user's name.
                    if (usernameInvalidChars.Contains<char>(username.ToCharArray()[0]))
                    {
                        // Check that the user is not already in the list.
                        if (!currentTab.userListBox.Items.Contains(username.Substring(1)))
                        {
                            // If there is an invalid character, ignore the first character
                            // and add the name to the username list.
                            currentTab.userListBox.Items.Add(username.Substring(1));
                        }
                    }
                    else
                    {
                        // Otherwise, also check that the user's name isn't an empty string.
                        if (!username.Equals(string.Empty))
                        {
                            // Check that the user is not already in the list.
                            if (!currentTab.userListBox.Items.Contains(username))
                            {
                                // Add the username to the list.
                                currentTab.userListBox.Items.Add(username);
                            }
                        }
                    }
                }
#if DEBUG
                // Print out the command recieved to the screen.
                currentTab.msgRecvBox.AppendText(command + "\n");
#else
                switch (currentTab.userListBox.Items.Count - 1)
                {
                    case 0:
                        currentTab.msgRecvBox.Text += "No users in the current channel.";
                        break;
                    case 1:
                        currentTab.msgRecvBox.Text += "Current user in channel: ";
                        break;
                    default:
                        currentTab.msgRecvBox.Text += "Current users in channel: ";
                        break;
                }
                foreach (string user in currentTab.userListBox.Items)
                {
                    if (!user.Equals(userInfo.username))
                    {
                        currentTab.msgRecvBox.AppendText(user + ", ");
                    }
                }
                currentTab.msgRecvBox.Text = currentTab.msgRecvBox.Text.TrimEnd(new char[] { ',', ' ' }) + "\n";
#endif
                currentTab.connectionImage.Enabled = true;
                currentTab = oldTab;
            }
            // Check if the command is a message (denoted by having PRIVMSG).
            else if (command.ToUpper().Equals("PRIVMSG"))
            {
                // If it is not null, check if the command contains the channel name.
                if (parameters[0].ToLower().Contains(currentTab.joinChannel.channel.ToLower()))
                {
                    // If so, filter out the username of whoever sent it, and
                    // display the response in the message recieved text box.
                    currentTab.msgRecvBox.AppendText(parameters[parameters.Length - 1] + ": ");
                    for (int i = 1; i < parameters.Length - 1; i += 1)
                    {
                        currentTab.msgRecvBox.AppendText(parameters[i] + ":");
                    }
                    currentTab.msgRecvBox.Text = currentTab.msgRecvBox.Text.TrimEnd(new char[] { ':' });
                    currentTab.msgRecvBox.AppendText("\n");
                    if (!currentTab.joinChannel.channel.ToLower().Equals(channelTabs.SelectedTab.Text.ToLower()))
                    {
                        currentTab.notification = Chat_Notifications.Message;
                    }
                }
                else
                {
                    // If it was not from a channel, then simply display the message from the
                    // specific user.
                    currentTab.msgRecvBox.AppendText("Private Message from ");
                    currentTab.msgRecvBox.AppendText(parameters[parameters.Length - 1] + ": ");
                    for (int i = 1; i < parameters.Length - 1; i += 1)
                    {
                        currentTab.msgRecvBox.AppendText(parameters[i] + ":");
                    }
                    currentTab.msgRecvBox.Text.TrimEnd(new char[] { ':' });
                    currentTab.msgRecvBox.AppendText("\n");
                }
            }
            else if (command.ToUpper().Equals("PART") || command.ToUpper().Equals("KILLED") || command.ToUpper().Equals("QUIT"))
            {
                // If so, print out that the user has quit.
                currentTab.msgRecvBox.AppendText(parameters[1] + " has quit!\n");

                // Then remove the user from the user list box.
                currentTab.userListBox.Items.Remove(parameters[1]);
                if (!currentTab.joinChannel.channel.ToLower().Equals(channelTabs.SelectedTab.Text.ToLower()))
                {
                    currentTab.notification = Chat_Notifications.UserLeft;
                }
            }
            else if (command.ToUpper().Equals("JOIN"))
            {
                // If so, print out that the user has joined.
                currentTab.msgRecvBox.AppendText(parameters[1] + " has joined!\n");

                // Check that the user is not already in the list.
                if (!currentTab.userListBox.Items.Contains(parameters[1]))
                {
                    // Then add the user to the user list box.
                    currentTab.userListBox.Items.Add(parameters[1]);
                }
                if (!currentTab.joinChannel.channel.ToLower().Equals(channelTabs.SelectedTab.Text.ToLower()))
                {
                    currentTab.notification = Chat_Notifications.UserJoined;
                }
            }
#if DEBUG
            else
            {
                // Otherwise, just print out the command if debugging and it is not a newline.
                currentTab.msgRecvBox.AppendText(command + " ");
                foreach (string param in parameters)
                {
                    currentTab.msgRecvBox.AppendText(param);
                }
                currentTab.msgRecvBox.Text += "\n";
            }
#endif
            // Check if the text box length is greater than 0.
            if (currentTab.msgRecvBox.Text.Length > 0)
            {
                // If so, set the selection starting position (the carret) and scroll to the
                // carret.
                currentTab.msgRecvBox.SelectionStart = currentTab.msgRecvBox.Text.Length - 1;
                currentTab.msgRecvBox.ScrollToCaret();
            }
            else
            {
                // Otherwise, reset the selection starting position (the carret) and scroll to the
                // carret.
                currentTab.msgRecvBox.SelectionStart = 0;
                currentTab.msgRecvBox.ScrollToCaret();
            }
        }

        /// <summary>
        /// Handles when data is recieved.
        /// </summary>
        /// <param name="recieved">The results that were recieved.</param>
        private void dataRecieved(IAsyncResult resultsRecieved)
        {
            // Create a new socket from the results.
            Socket resultsSocket = (Socket)resultsRecieved.AsyncState;

            // Try to get any data that was recieved.
            try
            {
                // Get the number of bytes recieved from the socket.
                int bytesRecieved = resultsSocket.EndReceive(resultsRecieved);
                // Check if the amount of bytes is greater than 0.
                if (bytesRecieved > 0)
                {
                    string s = Encoding.UTF8.GetString(data, 0, bytesRecieved);
                    dataString.Append(Encoding.UTF8.GetString(data, 0, bytesRecieved));

                    if (dataString.ToString().EndsWith("\r\n"))
                    {
                        // Manage any data recieved with the manage data method variable.
                        Invoke(manageData, new string[] { dataString.ToString() });

                        dataString = new StringBuilder();
                    }

                    // Set the new size to the size of the recieved buffer data.
                    data = new byte[BUFFER_LENGTH];
                    // Start recieving the next pieces of data.
                    mainSocket.BeginReceive(data, 0, data.Length, SocketFlags.None, new AsyncCallback(dataRecieved), mainSocket);
                }
                else
                {
                    // If no data was recieved, but this method was called, then the connection
                    // was probably severed, so the socket is shutdown and closed.
                    resultsSocket.Shutdown(SocketShutdown.Both);
                    resultsSocket.Close();
                }
            }
            catch (Exception ex)
            {
                // Show an error message in the console if debugging, and a dialog box if not.
#if DEBUG
                Console.WriteLine(ex.Message);
#else
                MessageBox.Show(this, ex.Message, "Unusual error druing Recieve!");
#endif
            }
        }

        /// <summary>
        /// Connects to an IRC server at the chosen addres and port.
        /// </summary>
        /// <param name="serverAdddress">The address of the server.</param>
        /// <param name="serverPort">The port to connect to the server on.</param>
        /// <returns>Returns the newly connected socket.</returns>
        private Socket connectToServer(string serverAdddress, int serverPort)
        {
            // Get a list of the ip address for the server address.
            IPHostEntry dnsHostEntry = Dns.GetHostEntry(serverAdddress);

            // Loop through all of the address.
            foreach (IPAddress address in dnsHostEntry.AddressList)
            {
                // Create the IP endpoint to connect to.
                IPEndPoint ipEndPoint = new IPEndPoint(address, serverPort);
                // Creates the socket to connect with.
                Socket socket = new Socket(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                // Actually connect to the IP endpoint.
                socket.Connect(ipEndPoint);

                // If the socket is connected, return it.
                if (socket.Connected)
                {
                    userInfo.server = Dns.GetHostEntry(address).HostName;

                    channelTabs.TabPages[0].Text = userInfo.server;
                    tabs.Add(userInfo.server.ToLower(), chatTab1);
                    currentTab = tabs[userInfo.server.ToLower()];
                    currentTab.ChannelTopic.Text = "Server: " + userInfo.server;
                    currentTab.userInfo = userInfo;
                    
                    return socket;
                }
            }
            // Return null if the socket couldn't be connected to.
            return null;
        }

        /// <summary>
        /// This handles the join channel button, which promts for and joins a new IRC channel
        /// on the server when clicked.
        /// </summary>
        /// <param name="sender">Not used.</param>
        /// <param name="e">Not used.</param>
        private void joinChannelButton_Click(object sender, EventArgs e)
        {
            // Check if the user is already in a channel.
            if (joinChannel != null)
            {
                // Dipose of and set the join channel form to null.
                joinChannel.Dispose();
                joinChannel = null;
            }

            // Create a new join channel form.
            joinChannel = new joinChannelForm();
            // Then show the form so the user can type in the channel name.
            if (joinChannel.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Create a new tab with the channel name as its text.
                channelTabs.TabPages.Add(joinChannel.channel, joinChannel.channel);
                // Create the new chat tab.
                ChatTab newTab = new ChatTab();
                newTab.Dock = DockStyle.Fill;
                // Set the socket, user info form and join channel form.
                newTab.joinChannel = joinChannel;
                newTab.userInfo = userInfo;
                newTab.mainSocket = mainSocket;
                // Clear the message recieved text box.
                newTab.msgRecvBox.Text = "";

                // Focus on the send message text box.
                newTab.sendMsgBox.Focus();

                tabs.Add(joinChannel.channel.ToLower(), newTab);
                currentTab = tabs[joinChannel.channel.ToLower()];
                currentTab.userInfo = userInfo;
                currentTab.mainSocket = mainSocket;
                // Set the tab name for the channel.
                channelTabs.TabPages[channelTabs.TabPages.IndexOfKey(joinChannel.channel)].Controls.Add(newTab);

                channelTabs.SelectTab(joinChannel.channel);

                // Send the command to join the desired channel.
                mainSocket.Send(Encoding.UTF8.GetBytes("JOIN " + joinChannel.channel + "\r\n"));
            }
            else
            {
                // If joining the channel was cancelled, set the join channel form to null.
                joinChannel = null;
            }
        }
        /// <summary>
        /// Leaves the current channel that the user is in.
        /// </summary>
        /// <param name="sender">Not used.</param>
        /// <param name="e">Not used.</param>
        private void leaveButton_Click(object sender, EventArgs e)
        {
            // If so, leave the previous channel by first sending the leave command.
            mainSocket.Send(Encoding.UTF8.GetBytes("PART " + currentTab.joinChannel.channel + " leaving\r\n"));
            // Clear the message recieved text box.
            currentTab.msgRecvBox.Text = "";
            // Set the tab name for the channel to be empty.
            this.Text = string.Empty;
            // Clear the user list text box.
            currentTab.userListBox.Items.Clear();

            // Focus on the send message text box.
            currentTab.sendMsgBox.Focus();

            // Save the current tab index.
            int currentTabIndex = channelTabs.SelectedIndex;
            // Remove the tab from the list of available tabs.
            tabs.Remove(currentTab.joinChannel.channel.ToLower());
            channelTabs.TabPages.RemoveByKey(currentTab.joinChannel.channel);

            // Set the current tab to the tab prior to the recently closed one.
            channelTabs.SelectedIndex = currentTabIndex - 1;
        }

        private void channelTabs_DrawItem(object sender, DrawItemEventArgs e)
        {
            Rectangle paddedBounds = e.Bounds;
            paddedBounds.Inflate(-2, -2);
            Pen pen = new Pen(Color.Black);
            e.Graphics.FillRectangle(new SolidBrush(this.BackColor), e.Bounds);
            switch (tabs[channelTabs.TabPages[e.Index].Text.ToLower()].notification)
            {
                case Chat_Notifications.Message:
                    pen = new Pen(Color.Blue);
                    break;
                case Chat_Notifications.UserJoined:
                    pen = new Pen(Color.Green);
                    break;
                case Chat_Notifications.UserLeft:
                    pen = new Pen(Color.Red);
                    break;
                default:
                    pen = new Pen(Color.Black);
                    break;
            }
            e.Graphics.DrawString(channelTabs.TabPages[e.Index].Text, this.Font, pen.Brush, paddedBounds);
        }
    }
}
