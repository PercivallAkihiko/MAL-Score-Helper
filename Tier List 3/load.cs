using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tier_List_3
{
    public partial class load : Form
    {
        public load()
        {
            InitializeComponent();
            timer1.Start();
        }

        public MAL localMal = new MAL();
        public string authorization_code { get; set; }


        private void button2_Click(object sender, EventArgs e)
        {                        
            Clipboard.SetText(localMal._Url);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            localMal = new MAL();
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            using (var key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Classes\\" + Global.UriScheme))
            {
                authorization_code = (string)key.GetValue("authorization_code");
                if (authorization_code == "EMPTY") { return; }
            }
            timer1.Stop();
            MessageBox.Show(new Form { TopMost = true }, "Authorization completed.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (Global.Mal is not null)
            {
                DialogResult result;
                result = MessageBox.Show(new Form { TopMost = true }, "The data loaded on the current application will be overwritten and lost, make sure you have updated your current list.", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == System.Windows.Forms.DialogResult.No) { return; }
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
