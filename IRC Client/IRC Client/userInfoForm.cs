using System;
using System.Windows.Forms;

namespace IRC_Client
{
    /// <summary>
    /// This a a form for the user's information.  This will ask for
    /// the user's username, the irc server to connect to, and the user's nickname.
    /// </summary>
    public partial class userInfoForm : Form
    {
        // The username and server for this IRC chat session.
        public string username = "testUser12345", realName = "testUser12345", nickname = "testUser12345", server = "irc.freenode.net";

        /// <summary>
        /// The constructor for the user info form.
        /// Sets the username textbox and server textbox keydown events.
        /// </summary>
        public userInfoForm()
        {
            InitializeComponent();

            // The username text box's key down function.
            usernameBox.KeyDown += new KeyEventHandler(textBox_KeyDown);
            // The server text box's key down function.
            serverBox.KeyDown += new KeyEventHandler(textBox_KeyDown);
        }

        /// <summary>
        /// Used for textbox key downs to detect keys.
        /// Checks if the enter key and clicks the continue
        /// button if it is.
        /// </summary>
        /// <param name="sender">Not used.</param>
        /// <param name="e">The event arguments for key pressed</param>
        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if the enter key was pressed.
            if (e.KeyData.Equals(Keys.Enter))
            {
                // If so, call the continue buttons clicked event.
                continueButton_Click(null, EventArgs.Empty);
            }
        }

        /// <summary>
        /// The method for the continue button.
        /// Closes the form and assigns the nickname, realname, username and 
        /// server variables.
        /// </summary>
        /// <param name="sender">Not used.</param>
        /// <param name="e">Not used.</param>
        private void continueButton_Click(object sender, EventArgs e)
        {
            // Set the nickname, real name, and username variables.
            nickname = realName = username = usernameBox.Text;
            // Set the server variable.
            server = serverBox.Text;
            // Close the form.
            this.Close();
        }
    }
}
