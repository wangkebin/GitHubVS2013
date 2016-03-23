using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Constraints.Data
{
    public static class EnumRules
    {
        public enum Fixvalues
        {
            [Description("4.0")]
            Fix40,
            [Description("4.2")]
            Fix42,
            [Description("4.4")]
            Fix44
        };

        public enum Side
        {
            [Description("0")]
            Side0,
            [Description("1")]
            Side1
        }
    }
}
