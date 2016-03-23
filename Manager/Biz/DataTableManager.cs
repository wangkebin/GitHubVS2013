using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media.Animation;

namespace Manager.Biz
{
    //Tables are keyed by their name
    public class DataTableManager
    {
        private object tableLock = new object();

        private DataSet allDataTables;
        private List<string> tableNameList; 

        // List of updated datatable. Need to be saved into DB.
        private List<DataTable> tablesToSync;

        public DataTableManager()
        {
            if(allDataTables == null)
                allDataTables = new DataSet();
            tableNameList = new List<string>(); 
            tablesToSync = new List<DataTable>(); 
            allDataTables.EnforceConstraints = true;
        }

        public bool TryAddDataTable(DataTable dataTableToAdd)
        {
            lock (tableLock)
            {
                if (tableNameList.Contains(dataTableToAdd.TableName))
                    return false;
                tableNameList.Add(dataTableToAdd.TableName);
                allDataTables.Tables.Add(dataTableToAdd);
                return true;
            }
        }

        public void AddOrUpdateDataTable(DataTable dataTableToAdd)
        {
            lock (tableLock)
            {
                if ( !tableNameList.Contains(dataTableToAdd.TableName))
                {
                    tableNameList.Add(dataTableToAdd.TableName);
                    allDataTables.Tables.Add(dataTableToAdd);
                }
                else
                {
                    allDataTables.Tables.Remove(dataTableToAdd.TableName);
                    allDataTables.Tables.Add(dataTableToAdd);
                }
            }
        }

        public bool ContainsDataTable(string tableName)
        {
            return tableNameList.Contains(tableName);
        }

        public void ClearAll()
        {
            lock (tableLock)
            {
                allDataTables.Tables.Clear();
                tableNameList.Clear();
            }
        }

        public IList<string> TableNameList {
            get { return tableNameList; }
        }

        /// <summary>
        ///  Get datatable by name or null if not exist.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string tableName)
        {
            lock (tableLock)
            {
                if (tableNameList.Contains(tableName))
                    return allDataTables.Tables[tableName];
                return null;
            }
        }

        private bool SaveDataTableToDB()
        {
            return false;
        }

    }
}
