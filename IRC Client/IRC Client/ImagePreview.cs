using System;
using System.Drawing;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace IRC_Client
{
    /// <summary>
    /// Creates a new preview image from a specific URL.  You can then save the image or open
    /// it in a user's default browser.
    /// </summary>
    public partial class ImagePreview : Form
    {
        /// <summary>
        /// The location of the image.  Must be a URL.
        /// </summary>
        private string imageLocation;

        /// <summary>
        /// Creates a new image preview form.
        /// </summary>
        /// <param name="imageLocation">The image's URL.</param>
        public ImagePreview(string imageLocation)
        {
            InitializeComponent();

            // Create a webclient temporarily for getting the image data.
            WebClient client = new WebClient();
            // Start reading the image stream.
            Stream imageStream = client.OpenRead(this.imageLocation = imageLocation);
            // Create the image from the image stream.
            this.pictureBox.Image = Image.FromStream(imageStream);
            // Flush any data leftover in the stream.
            imageStream.Flush();
            // Then close the image stream.
            imageStream.Close();
        }

        /// <summary>
        /// Manages key presses for the form.
        /// </summary>
        /// <param name="sender">Not used.</param>
        /// <param name="e">Get's the key presses for the form.</param>
        private void ImagePreview_KeyDown(object sender, KeyEventArgs e)
        {
            // Check for the escape key being pressed.
            if (e.KeyCode == Keys.Escape)
            {
                // If it is being pressed, close the form.
                this.Close();
            }

            // Check if the control key is being pressed.
            if (e.Modifiers == Keys.ControlKey)
            {
                // Check for the S key being pressed to save the image.
                if (e.KeyCode == Keys.S)
                {
                    // Then save the image if so.
                    this.saveImage();
                }
            }
        }

        /// <summary>
        /// Checks for a mouse click on the form.
        /// </summary>
        /// <param name="sender">Not used.</param>
        /// <param name="e">Used to check which mouse button was pressed.</param>
        private void pictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            // Check if the left mouse button was pressed.
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                // If so, open the image in the user's default browser.
                System.Diagnostics.Process.Start(this.imageLocation);
                // Then close this form afterwards since the image is open elsewhere.
                this.Close();
            }
            // Check if the right mouse button was pressed.
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                // If so, save the image.
                this.saveImage();
            }
        }

        /// <summary>
        /// Saves the image that is being previewed in this form.
        /// </summary>
        public void saveImage()
        {
            // Create a new save file dialog.
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            // Filter out the possible image types.
            saveFileDialog.Filter = "PNG Files (*.png)|*.png|BMP Files (*.bmp)|*.bmp|JPG Files (*.jpg)|*.jpg";
            // Show the dialog and store the results.
            DialogResult result = saveFileDialog.ShowDialog();
            // Check if the results were ok.
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                // If so, save the picture box's iamge with the chosen file name.
                pictureBox.Image.Save(saveFileDialog.FileName);
            }
        }
    }
}
