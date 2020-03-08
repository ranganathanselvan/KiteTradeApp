using KiteConnect;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.WebSockets;

namespace WindowsTradeApp
{
    public partial class HomeForm : Form
    {
        TradeUser tradeUser;
        Ticker ticker;
        List<Instrument> listIns;
        List<InstrumentEntitiy> listInsEntity = new List<InstrumentEntitiy>();
        static HttpClient client = new HttpClient();
        public HomeForm()
        {
            InitializeComponent();
            tradeUser = TradeUser.GetInstance;

            label1.Text = tradeUser.kiteUser.UserName;
            label2.Text = tradeUser.kiteUser.UserId;
            label3.Text = tradeUser.kiteUser.Email;
            WebSocket();
        }

        public void GetInstruments()
        {
            listIns = tradeUser.kite.GetInstruments();

            for (int i = 0; i < listIns.Count; i++)
            {
                InstrumentEntitiy ie = new InstrumentEntitiy();
                ie.Name = listIns[i].Name;
                ie.InstrumentToken = listIns[i].InstrumentToken;
                ie.ExchangeToken = listIns[i].ExchangeToken;
                ie.Type = listIns[i].InstrumentType;
                listInsEntity.Add(ie);

            }

            var n = listInsEntity.OrderBy(o => o.Name).Select(s => s.Name).Distinct().ToList();
            comboBox1.DataSource = n;
            //Log.WriteLog("trade-log.txt", stringBuilder.ToString());
        }

        public void WebSocket()
        {
            try
            {
                ticker = new Ticker(tradeUser.ApiKey, tradeUser.AccessToken);
                // Add handlers to events
                ticker.OnTick += onTicker;

                // Engage reconnection mechanism and connect to ticker
                ticker.EnableReconnect(Interval: 5, Retries: 50);
                ticker.Connect();

                // Subscribing to NIFTY50 and setting mode to LTP
                ticker.Subscribe(Tokens: new UInt32[] { 2113328 });
                ticker.SetMode(Tokens: new UInt32[] { 2113328 }, Mode: Constants.MODE_LTP);

                //var ws = new WebSocket('');  //("wss://ws.kite.trade?api_key=xxx&access_token=xxxx");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        // Example onTick handler
        private static void onTicker(Tick TickData)
        {

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("onTick - " + "\n");
            stringBuilder.Append("InstrumentToken : " + TickData.InstrumentToken.ToString() + "\n");
            stringBuilder.Append("Open : " + TickData.Open.ToString() + "\n");
            stringBuilder.Append("High : " + TickData.High.ToString() + "\n");
            stringBuilder.Append("Low : " + TickData.Low.ToString() + "\n");
            stringBuilder.Append("Close : " + TickData.Close.ToString() + "\n");

            Log.WriteLog("trade-log.txt", stringBuilder.ToString());
            Console.WriteLine("LTP: " + TickData.LastPrice);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (ticker != null)
                ticker.Close();
            TradeUser.DestroyInstance();
            Form1 form = new Form1();
            form.Show();

            this.Hide();

        }

        private void btnGetOrders_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show(tradeUser.kite.GetOrders().ToString());
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void btnGetHoldings_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show(tradeUser.kite.GetHoldings().ToString());
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void btnLoadInstruments_Click(object sender, EventArgs e)
        {
            try
            {
                GetInstruments();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var selectedName = comboBox1.SelectedItem;
                var n = listInsEntity.Where(w => w.Name == selectedName.ToString()).OrderBy(o => o.InstrumentToken).Select(s => s.InstrumentToken).ToList();
                comboBox2.DataSource = n;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var selectedToken = comboBox2.SelectedItem.ToString();
                /*var exetoken = listInsEntity.Where(w => w.Name == comboBox1.SelectedItem.ToString() && w.InstrumentToken == Convert.ToUInt32(selectedToken))
                    .Select(s => s.ExchangeToken).ToList();
                var list = getHistoricalData(exetoken[0].ToString(), "day");
                int noOfGain = 0, noOfLoss = 0, avgOfGain = 0, avgOfLoss = 0;
                foreach (Historical h in list)
                {
                    if (h.Close >= h.High)
                    {
                        noOfGain++;
                    }
                    if (h.Close <= h.Low)
                    {
                        noOfLoss++;
                    }
                }
                avgOfGain = (noOfGain / 14) * 100;
                avgOfLoss = (noOfLoss / 14) * 100;

                var rsi = 100 - (100 / (1 + (avgOfGain / avgOfLoss)));
                label4.Text = "First step - " + rsi;*/
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
