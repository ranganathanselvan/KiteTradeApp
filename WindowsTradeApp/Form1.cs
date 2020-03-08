using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KiteConnect;

namespace WindowsTradeApp
{
    public partial class Form1 : Form
    {
        Kite kite;
        TradeUser tradeUser;
        public Form1()
        {
            InitializeComponent();
            tradeUser = TradeUser.GetInstance;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if(textBox1.Text.Length == 0 || textBox2.Text.Length == 0)
                {
                    MessageBox.Show("Please enter valid Api key and Api secret.");
                    return;
                }
                tradeUser.ApiKey = textBox1.Text.ToString();
                tradeUser.ApiSecret = textBox2.Text.ToString();
                kite = new Kite(tradeUser.ApiKey, Debug: true);
                webView1.Navigate("https://kite.trade/connect/login?v=3&api_key=px78bthdypvd6pj8");
                Log.WriteLog("trade-log.txt", "https://kite.trade/connect/login?v=3&api_key=px78bthdypvd6pj8");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void webView1_NavigationStarting(object sender, Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlNavigationStartingEventArgs e)
        {
            
        }

        private void webView1_NavigationCompleted(object sender, Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlNavigationCompletedEventArgs e)
        {
            try
            {
                var link = e.Uri.ToString();
                Log.WriteLog("trade-log.txt", "webView1_NavigationCompleted - " + e.Uri.ToString());
                btnLogin.Focus();
                if (link.Contains("request_token"))
                {
                    string[] strSplit = link.Split('?');
                    if(strSplit.Count() == 2)
                    {
                        string[] strSplit2 = strSplit[1].Split('&');
                        foreach(string s in strSplit2)
                        {
                            if (s.Contains("request_token"))
                            {
                                tradeUser.ApiReturnToken = s.Replace("request_token=", "");
                                break;
                            }
                        }
                    }
                    //tradeUser.ApiReturnToken = link.Replace("http://127.0.0.1:8080/?", "").Replace("&action=login&status=success", "").Replace("request_token=", "");

                    // Collect tokens and user details using the request token
                    User user = kite.GenerateSession(tradeUser.ApiReturnToken, tradeUser.ApiSecret);                    
                    
                    tradeUser.AccessToken = user.AccessToken;
                    tradeUser.PublicToken = user.PublicToken;
                    tradeUser.kiteUser = user;

                    // Initialize Kite APIs with access token
                    kite.SetAccessToken(tradeUser.AccessToken);
                    tradeUser.kite = kite;
                    HomeForm home = new HomeForm();
                    home.Show();
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("trade-log.txt", ex.ToString());
                throw ex;
            }            
        }
        
    }
}
