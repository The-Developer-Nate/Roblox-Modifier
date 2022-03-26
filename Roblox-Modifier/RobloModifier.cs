using Microsoft.VisualBasic;
using Microsoft.Web.WebView2.WinForms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Roblox_Modifier
{
    public partial class RobloModifier : Form
    {
        WebView2 webView;
        bool ere = false;
        RobloxPlayer plr;
        Image head;
        Image bustimg;
        public RobloModifier(WebView2 wv)
        {
            InitializeComponent();
            webView = wv;
        }
        public Image RoundCorners(Image img)
        {
            Bitmap bmp = new Bitmap(img.Width, img.Height);
            using (GraphicsPath gp = new GraphicsPath())
            {
                gp.AddEllipse(0, 0, img.Width, img.Height);
                using (Graphics gr = Graphics.FromImage(bmp))
                {
                    gr.SetClip(gp);
                    gr.DrawImage(img, Point.Empty);
                }
            }
            return bmp;
        }

        public DirectoryInfo GetRobloxPlayerInstallFolder()
        {
            try
            {
                var robloxversions = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Roblox\\Versions\\";
                DirectoryInfo dir = new DirectoryInfo(robloxversions);
                DirectoryInfo[] dirs = dir.GetDirectories();
                foreach (DirectoryInfo d in dirs)
                {
                    if (d.GetFiles("RobloxPlayerBeta.exe").ToList().Count != 0)
                    {
                        return d;
                    }
                }
            } catch
            {
                return null;
            }
            return null;
        }
        public Image GetHeadshot(string UID)
        {
            if (head == null)
            {
                WebClient wc = new WebClient();
                Stream rbxstr = wc.OpenRead($"https://www.roblox.com/headshot-thumbnail/image?userId={UID}&width=512&height=512&format=png");
                Image bmp = Bitmap.FromStream(rbxstr);
                head = bmp;
                return bmp;
            } else
            {
                return head;
            }
        }
        public Image GetBustThumbnail(string UID)
        {
            if (bustimg == null)
            {
                WebClient wc = new WebClient();
                Stream rbxstr = wc.OpenRead($"https://www.roblox.com/bust-thumbnail/image?userId={UID}&width=512&height=512&format=png");
                Image bmp = Bitmap.FromStream(rbxstr);
                bustimg = bmp;
                return bmp;
            } else
            {
                return bustimg;
            }
        }
        public byte[] ImageToByteArray(Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }
        public RobloxPlayer GetPlayer()
        {
            WebClient wc = new WebClient();
            wc.Headers.Add(HttpRequestHeader.Cookie, ".ROBLOSECURITY=" + modifier.Default.RobloSecurity);
            wc.Headers.Add(HttpRequestHeader.UserAgent, "RobloxModifier/4.0");
            string e = wc.DownloadString("https://users.roblox.com/v1/users/authenticated");
            return JsonConvert.DeserializeObject<RobloxPlayer>(e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DirectoryInfo loadingdir = new DirectoryInfo(GetRobloxPlayerInstallFolder().FullName + "\\content\\textures\\loading");
            FileInfo file = loadingdir.GetFiles("robloxTilt.png").FirstOrDefault();
            FileInfo file2 = loadingdir.GetFiles("loadingCircle.png").FirstOrDefault();
            Image head = GetBustThumbnail(plr.id.ToString());
            File.WriteAllBytes(file.FullName, ImageToByteArray(head));
            File.WriteAllBytes(file2.FullName, ImageToByteArray(head));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DirectoryInfo loadingdir = new DirectoryInfo(GetRobloxPlayerInstallFolder().FullName + "\\content\\textures\\loading");
            FileInfo file = loadingdir.GetFiles("robloxTilt.png").FirstOrDefault();
            FileInfo file2 = loadingdir.GetFiles("loadingCircle.png").FirstOrDefault();
            Image head = GetHeadshot(plr.id.ToString());
            File.WriteAllBytes(file.FullName, ImageToByteArray(head));
            File.WriteAllBytes(file2.FullName, ImageToByteArray(head));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DirectoryInfo loadingdir = new DirectoryInfo(GetRobloxPlayerInstallFolder().FullName + "\\content\\textures\\loading");
            FileInfo file = loadingdir.GetFiles("robloxTilt.png").FirstOrDefault();
            FileInfo file2 = loadingdir.GetFiles("loadingCircle.png").FirstOrDefault();
            File.WriteAllBytes(file.FullName, ImageToByteArray(Properties.Resources.robloxTilt));
            File.WriteAllBytes(file2.FullName, ImageToByteArray(Properties.Resources.loadingCircle));
        }

        private void RobloModifier_Load(object sender, EventArgs e)
        {
            plr = GetPlayer();
            pictureBox1.Image = GetHeadshot(plr.id.ToString());
            pictureBox1.ImageLocation = $"https://www.roblox.com/headshot-thumbnail/image?userId={plr.id.ToString()}&width=512&height=512&format=png";
            label1.Text = $"Welcome {plr.displayName}!";
            label2.Text = $"Your ID: {plr.id.ToString()}";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            webView.CoreWebView2.CookieManager.DeleteAllCookies();
            modifier.Default.RobloSecurity = null;
            modifier.Default.Save();
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (ere)
            {
                pictureBox1.Image = GetHeadshot(plr.id.ToString());
                pictureBox1.ImageLocation = $"https://www.roblox.com/headshot-thumbnail/image?userId={plr.id.ToString()}&width=512&height=512&format=png";
                ere = false;
            } else
            {
                pictureBox1.Image = GetBustThumbnail(plr.id.ToString());
                pictureBox1.ImageLocation = $"https://www.roblox.com/bust-thumbnail/image?userId={plr.id.ToString()}&width=512&height=512&format=png";
                ere = true;
            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            WebClient wc = new WebClient();
            Stream rbxstr = wc.OpenRead($"https://www.roblox.com/headshot-thumbnail/image?userId={plr.id.ToString()}&width=512&height=512&format=png");
            Image bmp = Bitmap.FromStream(rbxstr);
            head = bmp;
            pictureBox1.Image = GetHeadshot(plr.id.ToString());
            ere = false;
            Stream rbxstr2 = wc.OpenRead($"https://www.roblox.com/bust-thumbnail/image?userId={plr.id.ToString()}&width=512&height=512&format=png");
            Image bmp2 = Bitmap.FromStream(rbxstr2);
            bustimg = bmp2;
            pictureBox1.Image = bmp;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            head = null;
            bustimg = null;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(plr.id.ToString());
            MessageBox.Show("Copied ID!");
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(plr.displayName);
            MessageBox.Show("Copied DisplayName!");
        }

        private void copyImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetImage(pictureBox1.Image);
        }

        private void copyImagePathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(pictureBox1.ImageLocation.ToString());
        }

        private void button7_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.png";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                DirectoryInfo loadingdir = new DirectoryInfo(GetRobloxPlayerInstallFolder().FullName + "\\content\\textures\\loading");
                FileInfo file = loadingdir.GetFiles("robloxTilt.png").FirstOrDefault();
                FileInfo file2 = loadingdir.GetFiles("loadingCircle.png").FirstOrDefault();
                byte[] image;
                image = File.ReadAllBytes(ofd.FileName);
                File.WriteAllBytes(file.FullName, image);
                File.WriteAllBytes(file2.FullName, image);
            }
        }
    }
    public class RobloxPlayer
    {
        public int id { get; set; }
        public string name { get; set; }
        public string displayName { get; set; }
    }


}
