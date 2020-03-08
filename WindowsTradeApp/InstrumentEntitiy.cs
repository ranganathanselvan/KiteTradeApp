using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsTradeApp
{
    class InstrumentEntitiy
    {
        public string Name { get; set; }
        public uint InstrumentToken { get; set; }
        public uint ExchangeToken { get; set; }
        public string Type { get; set; }
    }
}
