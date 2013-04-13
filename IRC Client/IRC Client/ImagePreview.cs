using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace IRC_Client
{
    public partial class ImagePreview : Form
    {
        private string imageLocation;
        public ImagePreview(string imageLocation)
        {
            InitializeComponent();

            WebClient client = new WebClient();
            Stream stream = client.OpenRead(this.imageLocation = imageLocation);
            this.pictureBox.Image = Image.FromStream(stream);
            stream.Flush();
            stream.Close();
        }

        private void ImagePreview_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void pictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.Close();
                System.Diagnostics.Process.Start(this.imageLocation);
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PNG Files (*.png)|*.png|BMP Files (*.bmp)|*.bmp|JPG Files (*.jpg)|*.jpg";
                DialogResult result = saveFileDialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    WebClient client = new WebClient();
                    Stream stream = client.OpenRead(this.imageLocation);
                    Image img = Image.FromStream(stream);
                    stream.Flush();
                    stream.Close();
                    img.Save(saveFileDialog.FileName);
                }
            }
        }
    }
}
