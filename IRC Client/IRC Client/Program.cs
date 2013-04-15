using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace IRC_Client
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Checks if this is being compiled for mono. If so, make sure that the application
            // remains compatible by not calling these methods.
#if !MONO
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
#endif
            Application.Run(new mainForm());
        }
    }
}
