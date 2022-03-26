using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Roblox_Modifier
{
    public partial class Form1 : Form
    {

        bool mainpaged = false;
        bool requiresreview = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            webView21.Source = new Uri("https://www.roblox.com/login");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            webView21.Source = new Uri("https://www.roblox.com/signup");
        }

        private void MainPage()
        {
            RobloModifier modifier = new RobloModifier(webView21);
            modifier.Show();
            modifier.TopMost = true;
            modifier.FormClosing += Modifier_FormClosing;
            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;
            Text = "Just leave this in the background, if this gets closed your modifier will be aswell.";
            mainpaged = true;
        }

        private void Modifier_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            try
            {
                var cookies = await webView21.CoreWebView2.CookieManager.GetCookiesAsync("https://roblox.com/");
                string cookie = "";
                for (int i = 0; i < cookies.Count; i++)
                {
                    if (cookies[i].Name == ".ROBLOSECURITY")
                    {
                        cookie = cookies[i].Value;
                    }
                }
                if (!string.IsNullOrEmpty(cookie))
                {
                    if (!mainpaged)
                    {
                        modifier.Default.RobloSecurity = cookie;
                        modifier.Default.Save();
                        MainPage();
                    }
                }
            } catch { MessageBox.Show("Error."); }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(modifier.Default.RobloSecurity))
            {
                MainPage();
            }
        }

        private async void WebView21_NavigationStarting(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs e)
        {
            Console.WriteLine(e.Uri.ToLower());
            if (e.Uri.ToLower().Contains("roblox.com/login") || e.Uri.ToLower().Contains("roblox.com/newlogin"))
            {
                e.Cancel = false;
            } else
            {
                e.Cancel = true;
            }
            if (e.Uri.ToLower().Contains("roblox.com/home"))
            {
                try
                {
                    var cookies = await webView21.CoreWebView2.CookieManager.GetCookiesAsync("https://roblox.com/");
                    string cookie = "";
                    for (int i = 0; i < cookies.Count; i++)
                    {
                        if (cookies[i].Name == ".ROBLOSECURITY")
                        {
                            cookie = cookies[i].Value;
                        }
                    }
                    if (!string.IsNullOrEmpty(cookie))
                    {
                        if (!mainpaged)
                        {
                            modifier.Default.RobloSecurity = cookie;
                            modifier.Default.Save();
                            MainPage();
                        }
                    }
                }
                catch { MessageBox.Show("Error."); }
            }
            if (e.Uri.ToLower().Contains("roblox.com/not-approved"))
            {
                MessageBox.Show("Oof, your account was either warned or banned temporarily, please review your account.", "Temporary error");
                e.Cancel=false;
            }
        }

        private void webView21_CoreWebView2InitializationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs e)
        {
            webView21.CoreWebView2.Settings.AreDevToolsEnabled = false;
            webView21.NavigationStarting += WebView21_NavigationStarting;
        }
    }
}
