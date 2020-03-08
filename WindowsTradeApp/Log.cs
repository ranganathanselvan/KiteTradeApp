using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsTradeApp
{
    class Log
    {

        public static bool WriteLog(string strFileName, string strMessage)
        {
            try
            {
                string getDateTime = DateTime.Now.ToLongDateString();
                string currentDirectory = System.IO.Directory.GetCurrentDirectory();
                FileStream objFilestream = new FileStream(string.Format("{0}\\{1}", currentDirectory, strFileName), FileMode.Append, FileAccess.Write);
                StreamWriter objStreamWriter = new StreamWriter((Stream)objFilestream);
                objStreamWriter.WriteLine(getDateTime + " -->" + strMessage);
                objStreamWriter.Close();
                objFilestream.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
