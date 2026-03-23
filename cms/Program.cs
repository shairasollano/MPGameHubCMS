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

            try
            {
                Form2 loginForm = null;
                try
                {
                    loginForm = new Form2();
                    Application.Run(loginForm);
                }
                finally
                {
                    if (loginForm != null && !loginForm.IsDisposed)
                    {
                        loginForm.Close();
                        loginForm.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Application Error: {ex.Message}\n{ex.StackTrace}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}