using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Constraints.Rules
{
    public class ConstraintByTableColumn<T> : ConstraintBase<T>
    {
        public DataTable TargetTable;
        public DataColumn TargetColumn;

        public ConstraintByTableColumn(DataTable constraintedDataTable, DataColumn constraintedDataColumn, 
            DataTable targetTable, DataColumn targetColumn)
        {
            this.ConstraintedDataTable = constraintedDataTable;
            this.ConstraintedDataColumn = constraintedDataColumn;
            this.TargetTable = targetTable;
            this.TargetColumn = targetColumn;
        }

        public override bool ValidateNewValue(T newvalue)
        {
            List<object> query = (from q in TargetTable.AsEnumerable()
                                  select q[TargetColumn.ColumnName]).ToList();

            return query.Contains(newvalue);
        }

    }
}
