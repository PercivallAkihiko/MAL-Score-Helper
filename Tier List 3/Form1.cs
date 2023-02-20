using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using static System.Formats.Asn1.AsnWriter;

namespace Tier_List_3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            progressBar1.Hide();

            PanelList = new List<DragDropFlowLayoutPanel>()
            {
                notier, tier1, tier2, tier3, tier4, tier5, tier6, tier7, tier8, tier9, tier10
            };
        }

        private bool Max = false;
        private int HeightControl = 96;
        private int WidthControl = 65;

        private void notier_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private List<DragDropFlowLayoutPanel> PanelList;

        private async void button1_Click(object sender, EventArgs e)
        {
            using (var f = new load())
            {
                if (f.ShowDialog() == DialogResult.OK && f.authorization_code != "EMPTY")
                {
                    try
                    {
                        this.Focus();
                        UpdateList(f.localMal, f.authorization_code);                        
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(new Form { TopMost = true }, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        public async void UpdateList(MAL newMal, string authorization_code, bool reloading = false)
        {
            if (!reloading)
            {
                try
                {
                    await newMal.GetAccessToken(authorization_code);
                    await newMal.GetAnimeList();
                    Global.Mal = newMal;
                    Debug.WriteLine("Fatto!");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            tier1.Controls.Clear();
            tier2.Controls.Clear();
            tier3.Controls.Clear();
            tier4.Controls.Clear();
            tier5.Controls.Clear();
            tier6.Controls.Clear();
            tier7.Controls.Clear();
            tier8.Controls.Clear();
            tier9.Controls.Clear();
            tier10.Controls.Clear();
            notier.Controls.Clear();

            progressBar1.Maximum = Global.Mal.AnimeList.Count;
            progressBar1.Show();

            foreach (Anime anime in Global.Mal.AnimeList)
            {
                DragDropControl userControl = new DragDropControl(anime.Title, anime.MainPicture.Large, anime.MyListStatus.Score, anime.Id);
                progressBar1.PerformStep();
                switch (anime.MyListStatus.Score)
                {
                    case 1:
                        tier1.Controls.Add(userControl);
                        break;
                    case 2:
                        tier2.Controls.Add(userControl);
                        break;
                    case 3:
                        tier3.Controls.Add(userControl);
                        break;
                    case 4:
                        tier4.Controls.Add(userControl);
                        break;
                    case 5:
                        tier5.Controls.Add(userControl);
                        break;
                    case 6:
                        tier6.Controls.Add(userControl);
                        break;
                    case 7:
                        tier7.Controls.Add(userControl);
                        break;
                    case 8:
                        tier8.Controls.Add(userControl);
                        break;
                    case 9:
                        tier9.Controls.Add(userControl);
                        break;
                    case 10:
                        tier10.Controls.Add(userControl);
                        break;
                    default:
                        notier.Controls.Add(userControl);
                        break;
                }
            }
            progressBar1.Hide();
            progressBar1.Value = 0;
            MessageBox.Show(new Form { TopMost = true }, "List successfully downloaded.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button2_Click(object sender, EventArgs e)
        {            
            progressBar1.Show();
            progressBar1.Maximum = Global.Mal.AnimeList.Count;

            int score = 0;
            int error = 0;

            foreach(DragDropFlowLayoutPanel panel in PanelList)
            {
                foreach (DragDropControl userControl in panel.Controls)
                {
                    try
                    {
                        Global.Mal.UpdateAnimeScore(userControl.Id, score, userControl.Title);
                    }
                    catch(Exception ex) { error++; }
                    progressBar1.PerformStep();
                }
                score++;
            }

            progressBar1.Hide();
            progressBar1.Value = 0;
            MessageBox.Show(new Form { TopMost = true }, $"Your animelist has been updated with {error} error.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            UpdateList(Global.Mal, "", true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            progressBar1.Show();
            progressBar1.Maximum = tier1.Controls.Count +
                tier2.Controls.Count +
                tier3.Controls.Count +
                tier4.Controls.Count +
                tier5.Controls.Count +
                tier6.Controls.Count +
                tier7.Controls.Count +
                tier8.Controls.Count +
                tier9.Controls.Count +
                tier10.Controls.Count;
            foreach (DragDropFlowLayoutPanel panel in PanelList.Skip(1))
            {
                foreach(DragDropControl Anime in panel.Controls)
                {
                    notier.Controls.Add(Anime);
                    progressBar1.PerformStep();
                }               
                panel.Controls.Clear();
            }
            progressBar1.Hide();
            progressBar1.Value = 0;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!Max) {
                this.WindowState = FormWindowState.Normal;
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
                this.WindowState = FormWindowState.Normal;
            }
            Max = !Max;
        }

        private async void button6_Click(object sender, EventArgs e)
        {
            ChangeSize(sender, e, 73, 48);
        }

        private async void button7_Click(object sender, EventArgs e)
        {
            ChangeSize(sender, e, 96, 65);
        }

        private async void button8_Click(object sender, EventArgs e)
        {
            ChangeSize(sender, e, 145, 97);
        }

        private async void ChangeSize(object sender, EventArgs e, int height, int width)
        {
            if(height == HeightControl) { return; }

            HeightControl = height;
            WidthControl = width;
            progressBar1.Show();
            progressBar1.Maximum = tier1.Controls.Count +
                tier2.Controls.Count +
                tier3.Controls.Count +
                tier4.Controls.Count +
                tier5.Controls.Count +
                tier6.Controls.Count +
                tier7.Controls.Count +
                tier8.Controls.Count +
                tier9.Controls.Count +
                tier10.Controls.Count;
            foreach (DragDropFlowLayoutPanel panel in PanelList)
            {
                foreach (DragDropControl userControl in panel.Controls)
                {
                    userControl.Height = height;
                    userControl.Width = width;
                    progressBar1.PerformStep();
                }
            }
            progressBar1.Hide();
            progressBar1.Value = 0;
        }
    }
}