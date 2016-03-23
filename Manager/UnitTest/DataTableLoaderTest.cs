using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manager.Biz;
using Manager.Data;
using NUnit.Framework;

namespace Manager.UnitTest
{
    [TestFixture]
    class DataTableLoaderTest
    {
        private DataTableDBAdaptor loader;
        private DataSet allData = new DataSet("EzeConnect");
            
        [TestFixtureSetUp]
        public void init()
        {
            Configuration.LoadConfigurations();
            loader = new DataTableDBAdaptor();
        }
        [Test]
        public void LoadDataTableTest()
        {
            allData.EnforceConstraints = true;
            Configuration.TableList.ForEach(x => allData.Tables.Add(loader.LoadDataTable(x)));
        }
    }
}
