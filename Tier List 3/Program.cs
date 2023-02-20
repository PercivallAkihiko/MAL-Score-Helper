using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Web;

namespace Tier_List_3
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            const string UriScheme = "mal";

            if (args.Length > 0)
            {
                Uri uri = new Uri(args[0]);
                string authorization_code = HttpUtility.ParseQueryString(uri.Query).Get("code");

                if (authorization_code != null)
                {
                    using (var key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Classes\\" + UriScheme))
                    {
                        key.SetValue("authorization_code", authorization_code);
                    }                    
                }
                else
                {
                    MessageBox.Show("Error while parsing the URI. Try again.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }
            Application.Run(new Form1());            
        }
    }
}