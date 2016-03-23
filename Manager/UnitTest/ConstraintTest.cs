using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manager.Biz;
using NUnit.Framework;

namespace Manager.UnitTest
{
    [TestFixture]
    class ConstraintTest
    {
        private DataTable virtualConnectionDataTable;
        private DataTable physicalConnectionDataTable;
        private DataTable logicalConnectionDataTable;

        private SqlCommandBuilder cb;
        private SqlDataAdapter sda;
        private SqlConnection con;
        string CmdString = "select * from ";

        private List<string> typeNameList = new List<string>();



        [TestFixtureSetUp]
        public void LoadData()
        {
            Configuration.LoadConfigurations();
            foreach (var tableName in Configuration.TableList)
            {
                
            }
        }
    }
}
