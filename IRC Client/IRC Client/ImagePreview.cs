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
        public ImagePreview(string imageLocation)
        {
            InitializeComponent();

            WebClient client = new WebClient();
            Stream stream = client.OpenRead(imageLocation);
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
    }
}
