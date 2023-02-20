using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace Tier_List_3
{
    public partial class DragDropControl : UserControl
    {
        public DragDropControl(string title, string imageUrl, int score, int id)
        {
            InitializeComponent();
            AllowDrop = true;

            Title= title;
            ImageUrl= imageUrl;
            Score= score;   
            Id= id;
            toolTip1.SetToolTip(this, title);
            var request = HttpWebRequest.Create(imageUrl);

            using (var response = request.GetResponse())
            using (var stream = response.GetResponseStream())
            {
                this.BackgroundImage = Bitmap.FromStream(stream);
                this.BackgroundImageLayout = ImageLayout.Stretch;
            }
        }

        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public int Score{ get; set; }
        public int Id{ get; set; }
    }
}
