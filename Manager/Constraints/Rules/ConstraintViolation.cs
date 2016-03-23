using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Constraints.Rules
{
    public enum ConstraintType
    {
        ByTableColumn,
        ByValueList,
        ByValueRange
    }

    public class ConstraintViolation
    {
        public ConstraintType ConstraintType;
        public string Violation;
    }
}
