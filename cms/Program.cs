using System;
using System.Windows.Forms;

namespace cms
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Create and show login form
            using (Form2 loginForm = new Form2())
            {
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    // Login successful, show main form
                    Application.Run(new Form1());
                }
                else
                {
                    // Login failed or cancelled
                    Application.Exit();
                }
            }
        }
    }
}