using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Manager.Biz;

namespace Manager.UnitTest
{
    [TestFixture]
    class ConfigurationTest
    {
        [Test]
        public void LoadConfigurationsTest()
        {
            Configuration.LoadConfigurations();
            Assert.Contains("fix_physical_connections", Configuration.TableList);
            Assert.Contains("fix_logical_connections", Configuration.TableList);
            Assert.Contains("fix_virtual_connections", Configuration.TableList);
        }
    }
}
