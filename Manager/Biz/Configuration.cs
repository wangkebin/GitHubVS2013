using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Biz
{
    public static class Configuration
    {
        public static void LoadConfigurations()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            TableList = new List<string>(ConfigurationManager.AppSettings["TableName"].Split(new char[] { ';' ,','}));
        }
        public static List<String> TableList { get; private set; }
        public static String ConnectionString { get; private set; }
    }
}
