using System;
using System.Windows.Forms;

namespace IRC_Client
{
    /// <summary>
    /// This a a form for the user's information.  This will ask for
    /// the user's username, the irc server to connect to, and the user's nickname.
    /// </summary>
    public partial class UserInfoForm : Form
    {
        /// <summary>
        /// The user's username for this IRC chat session.
        /// </summary>
        public string username = "testUser12345";
        /// <summary>
        /// The user's real name for this IRC chat session.
        /// </summary>
        public string realName = "testUser12345";
        /// <summary>
        /// The user's nickname for this IRC chat session.
        /// </summary>
        public string nickname = "testUser12345";
        /// <summary>
        /// The user's server to connect to for this IRC chat session.
        /// </summary>
        public string server = "irc.freenode.net";

        /// <summary>
        /// The constructor for the user info form.
        /// Sets the username textbox and server textbox keydown events, as well as the 
        /// default dialog results.
        /// </summary>
        public UserInfoForm()
        {
            InitializeComponent();

            // Set the results of the dialog to cancel by default.
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;

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
            // Check for the escape key being pressed.
            if (e.KeyCode.Equals(Keys.Escape))
            {
                // Just close the form if so.
                this.Close();
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
            // Set the dialog results to ok.
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            // Close the form.
            this.Close();
        }
    }
}
