using KiteConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsTradeApp
{
    public sealed class TradeUser
    {
        private static TradeUser instance = null;

        private TradeUser()
        {
        }

        public static TradeUser GetInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TradeUser();
                }
                return instance;
            }
        }

        public static void DestroyInstance()
        {
            instance = null;
        }

        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public string ApiReturnToken { get; set; }
        public string AccessToken { get; set; }
        public string PublicToken { get; set; }
        public string UserID { get; set; }

        public Kite kite { get; set; }
        public User kiteUser { get; set; }
    }
}
