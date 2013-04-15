using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace IRC_Client
{
    public partial class ServerTab : UserControl
    {
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
        /// The length of a buffer that can be recieved.
        /// </summary>
        private const int BUFFER_LENGTH = 32;

        /// <summary>
        /// The data received from the server as an array of bytes.
        /// </summary>
        private Byte[] dataBytes = new Byte[BUFFER_LENGTH];
        /// <summary>
        /// The data received from the server as a string.
        /// </summary>
        private StringBuilder dataString;

        /// <summary>
        /// The list of channels that the user is in currently.
        /// </summary>
        public SortedDictionary<string, UserControl> channels;

        /// <summary>
        /// The current channel that the user is viewing.
        /// </summary>
        public ChatTab currentChannel;

        /// <summary>
        /// The get data delegate.  Used for whenever data is recieved.
        /// </summary>
        /// <param name="receivedData">The data recieved from the server.</param>
        delegate void GetData(string receivedData);

        /// <summary>
        /// Manages the data recieved from the server.
        /// </summary>
        private event GetData manageData;

        /// <summary>
        /// The list of invalid character for a username.
        /// </summary>
        char[] usernameInvalidChars = { '+', '~', '@', '#', '%', '&' };

        /// <summary>
        /// The list of channel tabs that the user has.
        /// </summary>
        public TabControl channelTabs;

        public ServerTab()
        {
            InitializeComponent();

            // Initialize the data string.
            dataString = new StringBuilder();
            // Set the new size to the size of the recieved buffer data.
            dataBytes = new byte[BUFFER_LENGTH];

            // Create the user info form.
            userInfo = new UserInfoForm();

            // Setup the list of tabs.
            channels = new SortedDictionary<string, UserControl>();
        }

        /// <summary>
        /// Initializes the server connection with the set tabs to use.
        /// Requests for a username and servername, and then attempt to connect.
        /// </summary>
        /// <param name="channelTabs">The tabs to fill with channels.</param>
        /// <returns>Returns true if successful, false otherwise.</returns>
        public bool Initialize(TabControl channelTabs)
        {
            // Check if the user already setup a server with this control.
            if (userInfo != null)
            {
                // Dipose of and set the user info form to null.
                userInfo.Dispose();
                userInfo = null;
            }

            // Create a new user info form.
            userInfo = new UserInfoForm();

            // Check that the dialog's results were ok.
            if (userInfo.ShowDialog() != DialogResult.OK)
            {
                // If not, return false.
                return false;
            }

            // Set the channel tabs.
            this.channelTabs = channelTabs;

            // Setup the manage data method to handle any data recieved
            // from the server.
            manageData = new GetData(recievedData);
            // Setup the connection to the server's socket.
            mainSocket = connectToServer(userInfo.server, 6667);

            // Check that the socket was successfully created.
            if (mainSocket == null)
            {
                // If not, display an error message.
                MessageBox.Show("Error connecting to server!");
                // Then return false.
                return false;
            }
            else
            {
                // Set the callback method for when data is recieved.
                mainSocket.BeginReceive(dataBytes, 0, dataBytes.Length, SocketFlags.None, new AsyncCallback(dataRecieved), mainSocket);
            }

            // Return true if everything was successful.
            return true;
        }

        /// <summary>
        /// Used to join a channel on a server.
        /// </summary>
        /// <returns>Returns true if successful, false otherwise.</returns>
        public bool joinChannel()
        {
            // Check if the user is already in a channel.
            if (joinChannelForm != null)
            {
                // Dipose of and set the join channel form to null.
                joinChannelForm.Dispose();
                joinChannelForm = null;
            }

            // Create a new join channel form.
            joinChannelForm = new JoinChannelForm();

            // Then show the form so the user can type in the channel name and check the results.
            if (joinChannelForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Check that the tab does not already exist.
                if (channelTabs.TabPages.ContainsKey(joinChannelForm.channel.ToLower()))
                {
                    // If the channel already exists, set the selected tab.
                    channelTabs.SelectedTab = channelTabs.TabPages[joinChannelForm.channel.ToLower()];
                    // Also set the join channel form to null.
                    joinChannelForm = null;
                    // Return false since it was unsuccessful.
                    return false;
                }
                else
                {
                    // Create a new tab with the channel name as its text.
                    channelTabs.TabPages.Add(joinChannelForm.channel.ToLower(), joinChannelForm.channel);
                    // Create the new chat tab.
                    ChatTab newChatTab = new ChatTab();
                    // Set the tab dock style to fill the tab.
                    newChatTab.Dock = DockStyle.Fill;
                    // Set the socket, user info form and join channel form.
                    newChatTab.joinChannelForm = joinChannelForm;
                    newChatTab.userInfo = userInfo;
                    newChatTab.mainSocket = mainSocket;
                    // Clear the message recieved text box.
                    newChatTab.msgRecvBox.Text = "";

                    // Focus on the send message text box.
                    newChatTab.sendMsgBox.Focus();

                    // Add the tab to the list of chennels available.
                    channels.Add(joinChannelForm.channel.ToLower(), newChatTab);
                    // Set the current channel.
                    currentChannel = (ChatTab)(channels[joinChannelForm.channel.ToLower()]);
                    // Set the tab name for the channel.
                    channelTabs.TabPages[channelTabs.TabPages.IndexOfKey(joinChannelForm.channel)].Controls.Add(newChatTab);

                    // Select the newly created channel.
                    channelTabs.SelectTab(joinChannelForm.channel);

                    // Send the command to join the desired channel.
                    mainSocket.Send(Encoding.UTF8.GetBytes("JOIN " + joinChannelForm.channel + "\r\n"));
                }
            }
            else
            {
                // If joining the channel was cancelled, set the join channel form to null.
                joinChannelForm = null;
                // Return false since it was unsuccessful.
                return false;
            }

            // Return true since it was successful.
            return true;
        }

        /// <summary>
        /// Used to leave the current channel that the user is in.
        /// </summary>
        public void leaveCurrentChannel()
        {
            // Leave the previous channel by first sending the leave command.
            mainSocket.Send(Encoding.UTF8.GetBytes("PART " + currentChannel.joinChannelForm.channel + " leaving\r\n"));
            // Clear the message recieved text box.
            currentChannel.msgRecvBox.Text = "";
            // Set the tab name for the channel to be empty.
            this.Text = string.Empty;
            // Clear the user list text box.
            currentChannel.userListBox.Items.Clear();

            // Focus on the send message text box.
            currentChannel.sendMsgBox.Focus();

            // Remove the tab from the list of available tabs.
            channels.Remove(currentChannel.joinChannelForm.channel.ToLower());
            channelTabs.TabPages.RemoveByKey(currentChannel.joinChannelForm.channel);
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
                    // Set the server name by getting the exact host entry from
                    // the DNS server.
                    userInfo.server = Dns.GetHostEntry(address).HostName;

                    // Set the current channel to a new chat tab.
                    this.currentChannel = new ChatTab();
                    // Set the message box for the current channel to the server message box.
                    this.currentChannel.msgRecvBox = this.msgRecvBox;

                    // Set the first tab's text to the server name.
                    channelTabs.TabPages[0].Text = userInfo.server;

                    // Return the socket.
                    return socket;
                }
            }

            // Return null if the socket couldn't be connected to.
            return null;
        }

        /// <summary>
        /// Handles any data recieved form the server and responds accordingly
        /// to it.
        /// </summary>
        /// <param name="receivedData">The actual data recieved.</param>
        private void recievedData(string dataRecieved)
        {
            // Split and store the commands based on the '\n' character and remove the empty entries.
            string[] commands = dataRecieved.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // Loop through all of the commands.
            foreach (string cmd in commands)
            {
                // Store the command in a separate variable to be manipulated.
                string command = cmd;

                // Check if the first character is not a colon, which makes parsing the commands
                // different but easier.
                if (command[0] != ':')
                {
                    // Get the actual command (the first "word" before a space).
                    string actualCommand = command.Split(new char[] { ' ' })[0];
                    // Trim off the command from itself since we have that stored
                    // separately.
                    command = command.Substring(actualCommand.Length + 1);
                    // Everything after is a paremeter, and is stored as such.
                    string[] parameters = command.Split(new char[] { ' ' });
                    // Call the command on the current channel.
                    doCommand(currentChannel, actualCommand, parameters);
                }
                else
                {
                    // Otherwise, get the command after the colon.
                    command = command.Substring(1);

                    // Create a temporary channel name from the channel in the command.
                    string servername = command.Split(new char[] { ' ' })[0];
                    
                    // Check if that server name is the actual server.
                    if (userInfo.server.Equals(servername))
                    {
                        // Get the command after the servername.
                        command = command.Substring(servername.Length + 1);

                        // Get the actual command that is given.
                        string actualCommand = command.Split(new char[] { ' ' })[0];

                        // Go to the parameters after the command.
                        command = command.Substring(actualCommand.Length);
                        // Trims the start of any trailing spaces.
                        command = command.TrimStart(new char[] { ' ' });
                        // Trim the end of '\r' and '\n' characters.
                        command = command.TrimEnd(new char[] { '\r', '\n' });

                        // Get the rest of the parametsr (which may be separated by colons) and remove any empty entries.
                        string[] parameters = command.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

                        // Call the command on the current channel.
                        doCommand(currentChannel, actualCommand, parameters);
                    }
                    // Check that the servername doesn't contain the user's name.
                    else if (!servername.Split(new char[] { '!', '@' })[0].Equals(userInfo.username))
                    {
                        // Get the substring of the command after the servername.
                        command = command.Substring(servername.Length + 1);

                        // Get the actual command that is given.
                        string actualCommand = command.Split(new char[] { ' ' })[0];

                        // Check that the actual command is either a private message, a user joining, or quitting.
                        if (actualCommand.ToUpper().Equals("PRIVMSG") || actualCommand.ToUpper().Equals("JOIN")
                            || actualCommand.ToUpper().Equals("PART") || actualCommand.ToUpper().Equals("KILLED")
                            || actualCommand.ToUpper().Equals("QUIT"))
                        {
                            // Go to the parameters after the command.
                            command = command.Substring(actualCommand.Length);
                            // Trims the start of any trailing spaces.
                            command = command.TrimStart(new char[] { ' ' });
                            // Trim the end of '\r' and '\n' characters.
                            command = command.TrimEnd(new char[] { '\r', '\n' });

                            // Get the section of the command after the servername and username.
                            command += ":" + servername.Split(new char[] { '!', '@' })[0];

                            // Get the rest of the parametsr (which may be separated by colons) and remove any empty entries.
                            string[] parameters = command.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

                            // Check if the chosen channel exists.
                            if (channels.ContainsKey(parameters[0].Split(' ')[0].ToLower()))
                            {
                                // Call the command on the correct channel.
                                doCommand((ChatTab)channels[parameters[0].Split(' ')[0].ToLower()], actualCommand, parameters);
                            }
                            else
                            {
                                // Call the command on the current channel.
                                doCommand(currentChannel, actualCommand, parameters);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles when data is recieved.
        /// </summary>
        /// <param name="recieved">The results that were recieved.  Used for getting the original socket.</param>
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
                    string s = Encoding.UTF8.GetString(dataBytes, 0, bytesRecieved);
                    dataString.Append(Encoding.UTF8.GetString(dataBytes, 0, bytesRecieved));

                    if (dataString.ToString().EndsWith("\r\n"))
                    {
                        // Manage any data recieved with the manage data method variable.
                        Invoke(manageData, new string[] { dataString.ToString() });

                        // Reset the data as a string.
                        dataString = new StringBuilder();
                    }

                    // Set the new size to the size of the recieved buffer data.
                    dataBytes = new byte[BUFFER_LENGTH];
                    // Start recieving the next pieces of data.
                    mainSocket.BeginReceive(dataBytes, 0, dataBytes.Length, SocketFlags.None, new AsyncCallback(dataRecieved), mainSocket);
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
        /// Perform the actual commands in a specific chat tab.
        /// </summary>
        /// <param name="currentChannel">The channel to perform the command in.</param>
        /// <param name="command">The command to perform.</param>
        /// <param name="parameters">The parameters for the command</param>
        public void doCommand(ChatTab currentChannel, string command, string[] parameters)
        {
            // Check if the command was a ping command.
            if (command.ToUpper().Equals("PING"))
            {
                // Send the pong response as unicode bytes.
                mainSocket.Send(Encoding.UTF8.GetBytes("PONG :" + parameters + "\r\n"));
            }
            // Check if the command was a pong commands.
            else if (command.ToUpper().Equals("PONG"))
            {
                // Signal that a pong response was gotten.
                currentChannel.pingDone();
            }
            // Check if the command was a notice command.
            else if (command.ToUpper().Equals("NOTICE"))
            {
                // Print out the command with a space afterwards.
                this.msgRecvBox.AppendText(command + " ");

                // Loop through all of the parameters.
                foreach (string param in parameters)
                {
                    // Print out each parameter to the message box.
                    this.msgRecvBox.AppendText(param);

                    // Check if the parameters contains the strings "response".
                    if (param.Contains("response"))
                    {
                        // If the command contains the text "response", then the nickname and 
                        // username are sent over in that order.
                        mainSocket.Send(Encoding.UTF8.GetBytes("NICK " + userInfo.nickname + "\r\n"));
                        mainSocket.Send(Encoding.UTF8.GetBytes("USER " + userInfo.username + " 8 * :" + userInfo.realName + "\r\n"));
                    }
                }
                // End the message box with a newline.
                this.msgRecvBox.Text += "\n";
            }
            // Check if the command is 433, which signals a nickname conflict.
            else if (command.Equals("433"))
            {
                // Alert the user that the name is already taken.
                currentChannel.msgRecvBox.Text += "Name already taken!\n";
                // Recreate the user info form and show it.
                userInfo = new UserInfoForm();
                userInfo.ShowDialog();
                // Retry sending the username and nickname.
                mainSocket.Send(Encoding.UTF8.GetBytes("NICK " + userInfo.nickname + "\r\n"));
                mainSocket.Send(Encoding.UTF8.GetBytes("USER " + userInfo.username + " 8 * :" + userInfo.realName + "\r\n"));
            }
            // Check if command is 376, whichs signals the end of the motd command.
            else if (command.Equals("376"))
            {
                // Enable the join channel toolstrip item.
                //joinChannelFormToolStripMenuItem.Enabled = true;
                // Enable the connection image.
                currentChannel.connectionImage.Enabled = true;
#if DEBUG
                // Print out the command if debugging.
                currentChannel.msgRecvBox.AppendText(command + "\n");
#endif
            }
            // Check if the command is 332, which signals the channel topic command.
            else if (command.Equals("332"))
            {
                // Set the channel topic text to be empty.
                currentChannel.ChannelTopic.Text = String.Empty;
                // Loop through all of the parameters after the first one.
                for (int i = 1; i < parameters.Length; i += 1)
                {
                    // Append each parameter onto the channel topic's text.
                    currentChannel.ChannelTopic.Text += parameters[i] + ":";
                }
                // Trim the ending of any spaces, newlines, or line returns.
                currentChannel.ChannelTopic.Text.TrimEnd(new char[] { ':', ' ', '\n', '\r' });
            }
            // Check for commands 001, 002, 003, 004, 005, 250, 251, 252, 253, 254, 255, 265, 266, 372, and 375
            // all of which are information about the server.
            else if (command.Equals("001") || command.Equals("002") ||
                command.Equals("003") || command.Equals("004") ||
                command.Equals("005") || command.Equals("250") ||
                command.Equals("251") || command.Equals("252") ||
                command.Equals("253") || command.Equals("254") ||
                command.Equals("255") || command.Equals("265") ||
                command.Equals("266") || command.Equals("372") ||
                command.Equals("375"))
            {
                // Loop through all of the parameters after the first one.
                for (int i = 1; i < parameters.Length; i += 1)
                {
                    // Append each parameter onto the messsage box with a colon after since that
                    // is what separaters them.
                    this.msgRecvBox.AppendText(parameters[i] + ":");
                }
                // Trim the ending of the text of any colons that are still there.
                this.msgRecvBox.Text = this.msgRecvBox.Text.TrimEnd(new char[] { ':' });
                // Append a newline character to the end as well.
                this.msgRecvBox.AppendText("\n");

                // Check if the command is not 372.
                if (!command.Equals("372"))
                {
                    // If so, append another newline character for readability.
                    this.msgRecvBox.AppendText("\n");
                }
            }
            // Check if the command is 353, which signals the list of users in the channel.
            else if (command.Equals("353"))
            {
                // Create an old tab variable.
                ChatTab oldTab = currentChannel;
                // Check if the channel is on another tab.
                if (channels.ContainsKey(parameters[0].Split(' ')[3].ToLower()))
                {
                    // If so, set the current channel.
                    currentChannel = (ChatTab)channels[parameters[0].Split(' ')[3].ToLower()];
                }

                // Clear the old username list.
                currentChannel.userListBox.Items.Clear();
                // Split the list of users in the room.
                string[] userList = parameters[1].Split(' ');
                // Add the channel to the list of available chats.
                currentChannel.userListBox.Items.Add(currentChannel.joinChannelForm.channel);
                // Now loop through the list of users.
                foreach (string username in userList)
                {
                    // Check for any invalid characters at the beginning of
                    // the user's name.
                    if (usernameInvalidChars.Contains<char>(username.ToCharArray()[0]))
                    {
                        // Check that the user is not already in the list.
                        if (!currentChannel.userListBox.Items.Contains(username.Substring(1)))
                        {
                            // If there is an invalid character, ignore the first character
                            // and add the name to the username list.
                            currentChannel.userListBox.Items.Add(username.Substring(1));
                        }
                    }
                    else
                    {
                        // Otherwise, also check that the user's name isn't an empty string.
                        if (!username.Equals(string.Empty))
                        {
                            // Check that the user is not already in the list.
                            if (!currentChannel.userListBox.Items.Contains(username))
                            {
                                // Add the username to the list.
                                currentChannel.userListBox.Items.Add(username);
                            }
                        }
                    }
                }
#if DEBUG
                // Print out the command recieved to the screen if debugging.
                currentChannel.msgRecvBox.AppendText(command + "\n");
#else
                // If not debugging, nicely print out a message depending on the number of users
                // in the channel.
                switch (currentChannel.userListBox.Items.Count - 1)
                {
                    case 0:
                        currentChannel.msgRecvBox.Text += "No users in the current channel.";
                        break;
                    case 1:
                        currentChannel.msgRecvBox.Text += "Current user in channel: ";
                        break;
                    default:
                        currentChannel.msgRecvBox.Text += "Current users in channel: ";
                        break;
                }
                // Then print out each user that is in the channel.
                foreach (string user in currentChannel.userListBox.Items)
                {
                    if (!user.Equals(userInfo.username))
                    {
                        currentChannel.msgRecvBox.AppendText(user + ", ");
                    }
                }
                // Trim the ending of any invalid characters.
                currentChannel.msgRecvBox.Text = currentChannel.msgRecvBox.Text.TrimEnd(new char[] { ',', ' ' }) + "\n";
#endif
                // Enable the connection image.
                currentChannel.connectionImage.Enabled = true;
                // Set the current channel back to it's original channel.
                currentChannel = oldTab;
            }
            // Check if the command is a message (denoted by having PRIVMSG).
            else if (command.ToUpper().Equals("PRIVMSG"))
            {
                // Then check if the command contains the channel name.
                if (parameters[0].ToLower().Contains(currentChannel.joinChannelForm.channel.ToLower()))
                {
                    // If so, filter out the username of whoever sent it, and
                    // display the response in the message recieved text box.
                    currentChannel.msgRecvBox.AppendText(parameters[parameters.Length - 1] + ": ");

                    // Then loop through all of the paremeters.
                    for (int i = 1; i < parameters.Length - 1; i += 1)
                    {
                        // Append the message box with the parameter and a colon at the end
                        // since they are colon separated.
                        currentChannel.msgRecvBox.AppendText(parameters[i] + ":");
                    }
                    // Trim the ending of any leftover colons.
                    currentChannel.msgRecvBox.Text = currentChannel.msgRecvBox.Text.TrimEnd(new char[] { ':' });
                    // Append a newline to the end of it.
                    currentChannel.msgRecvBox.AppendText("\n");

                    // Check if the selected tab is the one that received the message.
                    if (!currentChannel.joinChannelForm.channel.ToLower().Equals(channelTabs.SelectedTab.Text.ToLower()))
                    {
                        // If not, set the chat notification.
                        currentChannel.notification = Chat_Notifications.Message;
                    }
                }
                // If not, then it was a private messgae most like.
                else
                {
                    // Display that it is a private message.
                    currentChannel.msgRecvBox.AppendText("Private Message from ");
                    // Display the user the message is from.
                    currentChannel.msgRecvBox.AppendText(parameters[parameters.Length - 1] + ": ");

                    // Then loop through all of the paremeters.
                    for (int i = 1; i < parameters.Length - 1; i += 1)
                    {
                        // Append the message box with the parameter and a colon at the end
                        // since they are colon separated.
                        currentChannel.msgRecvBox.AppendText(parameters[i] + ":");
                    }
                    // Trim the ending of any leftover colons.
                    currentChannel.msgRecvBox.Text = currentChannel.msgRecvBox.Text.TrimEnd(new char[] { ':' });
                    // Append a newline to the end of it.
                    currentChannel.msgRecvBox.AppendText("\n");
                }
            }
            // Check if the command is PART, KILLED, or QUIT.
            else if (command.ToUpper().Equals("PART") || command.ToUpper().Equals("KILLED") || command.ToUpper().Equals("QUIT"))
            {
                // If so, print out that the user has quit.
                currentChannel.msgRecvBox.AppendText(parameters[1] + " has quit!\n");

                // Then remove the user from the user list box.
                currentChannel.userListBox.Items.Remove(parameters[1]);

                // Check if the selected tab is the one that received the message.
                if (!currentChannel.joinChannelForm.channel.ToLower().Equals(channelTabs.SelectedTab.Text.ToLower()))
                {
                    // If not, set the chat notification.
                    currentChannel.notification = Chat_Notifications.UserLeft;
                }
            }
            // Check if the command is JOIN.
            else if (command.ToUpper().Equals("JOIN"))
            {
                // If so, print out that the user has joined.
                currentChannel.msgRecvBox.AppendText(parameters[1] + " has joined!\n");

                // Check that the user is not already in the list.
                if (!currentChannel.userListBox.Items.Contains(parameters[1]))
                {
                    // Then add the user to the user list box.
                    currentChannel.userListBox.Items.Add(parameters[1]);
                }

                // Check if the selected tab is the one that received the message.
                if (!currentChannel.joinChannelForm.channel.ToLower().Equals(channelTabs.SelectedTab.Text.ToLower()))
                {
                    // If not, set the chat notification.
                    currentChannel.notification = Chat_Notifications.UserJoined;
                }
            }
#if DEBUG
            else
            {
                // Otherwise, just print out the command if debugging and it is not a newline.
                currentChannel.msgRecvBox.AppendText(command + " ");
                foreach (string param in parameters)
                {
                    currentChannel.msgRecvBox.AppendText(param);
                }
                currentChannel.msgRecvBox.Text += "\n";
            }
#endif
            // Check if the text box length is greater than 0.
            if (currentChannel.msgRecvBox.Text.Length > 0)
            {
                // If so, set the selection starting position (the carret) and scroll to the
                // carret.
                currentChannel.msgRecvBox.SelectionStart = currentChannel.msgRecvBox.Text.Length - 1;
                currentChannel.msgRecvBox.ScrollToCaret();
            }
            else
            {
                // Otherwise, reset the selection starting position (the carret) and scroll to the
                // carret.
                currentChannel.msgRecvBox.SelectionStart = 0;
                currentChannel.msgRecvBox.ScrollToCaret();
            }

            // Refresh the channel tabs to display changes.
            channelTabs.Refresh();
        }
    }
}
