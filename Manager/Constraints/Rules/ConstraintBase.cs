using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Constraints
{
    public class ConstraintBase<T>
    {
        public DataTable ConstraintedDataTable;
        public DataColumn ConstraintedDataColumn;
        public string RuleName;

        public ConstraintBase()
        {
            this.ConstraintedDataColumn = null;
            this.ConstraintedDataTable = null;
        }

        public ConstraintBase(DataTable constraintedDataTable, DataColumn constraintedDataColumn)
        {
            this.ConstraintedDataTable = constraintedDataTable;
            this.ConstraintedDataColumn = constraintedDataColumn;
        }

        public virtual bool ValidateNewValue(T newvalue)
        {
            return true;
        }

    }
}
