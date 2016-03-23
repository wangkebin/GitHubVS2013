using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Manager.Data
{

    internal class DataTableDBAdaptor
    {
        private SqlCommandBuilder cb;
        private SqlDataAdapter sda;
        private SqlConnection con;
        private readonly string SQL_getDataTypesPrefix = "select top(0) * from ";
        private readonly string SQL_getDataPrefix = "select * from ";

        private readonly string connectionString =
            ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        public DataTable LoadDataTable(string tableName)
        {
            List<string> typeNameList;
            if (GetDataType(tableName, out typeNameList))
            {
                DataTable tempTable = GetData(tableName);
                DataTable dataTable = GenerateDataTableWithColumn(typeNameList, tempTable);
                FillDataGrid(dataTable, tempTable);
                return dataTable;
            }
            return null;

        }

        #region help functions

        private bool GetDataType(string tableName, out List<String> typeNameList)
        {
            typeNameList = new List<string>();
            using (con = new SqlConnection(connectionString))
            {

                SqlCommand cmd = new SqlCommand(SQL_getDataTypesPrefix + tableName, con);

                SqlDataReader SqlDr;
                con.Open();

                SqlDr = cmd.ExecuteReader();
                SqlDr.Read();
                int i = 0;
                while (i < SqlDr.FieldCount)
                {

                    //SqlDbType type = (SqlDbType)(int)SqlDr.GetSchemaTable().Rows[0]["ProviderType"];
                    //MessageBox.Show(SqlDr.GetDataTypeName(i));
                    typeNameList.Add(SqlDr.GetDataTypeName(i));
                    i++;
                }
            }
            return true;
        }

        /// <summary>
        /// return null of failed to generate
        /// </summary>
        /// <param name="typeNameList"></param>
        /// <param name="DataTableWithTargetColumn"></param>
        /// <returns></returns>
        private DataTable GenerateDataTableWithColumn(List<String> typeNameList, DataTable DataTableWithTargetColumn)
        {
            string unknowTypes = string.Empty;

            if(typeNameList == null || typeNameList.Count < 1)
                return null;
            //DataTable dataTable = new DataTable(DataTableWithTargetColumn.TableName);
            DataTable dataTable = DataTableWithTargetColumn.Clone();
            // Populate columns
            for (int i = 0; i < typeNameList.Count; i++)
            {
                switch (typeNameList[i])
                {
                    case "int":
                    case "smallint":
                    case "bigint":
                        //dataTable.Columns.Add(DataTableWithTargetColumn.Columns[i].ColumnName, typeof(int));
                        dataTable.Columns[i].DataType =  typeof(int);
                        dataTable.Columns[i].ReadOnly = true;
                        break;
                    case "varchar":
                    case "char":
                    case "text":
                        //dataTable.Columns.Add(DataTableWithTargetColumn.Columns[i].ColumnName, typeof(string));
                        dataTable.Columns[i].DataType = typeof(string);
                        break;
                    case "binary":
                        //dataTable.Columns.Add(DataTableWithTargetColumn.Columns[i].ColumnName, typeof(string));
                        dataTable.Columns[i].DataType = typeof(string);
                        break;
                    case "datetime":
                        //case "timestamp":
                        //dataTable.Columns.Add(DataTableWithTargetColumn.Columns[i].ColumnName, typeof(DateTime));
                        dataTable.Columns[i].DataType = typeof(DateTime);
                        break;
                    case "decimal":
                        //dataTable.Columns.Add(DataTableWithTargetColumn.Columns[i].ColumnName, typeof(Decimal));
                        dataTable.Columns[i].DataType = typeof(Decimal);
                        break;
                    case "bit":
                       // dataTable.Columns.Add(DataTableWithTargetColumn.Columns[i].ColumnName, typeof(Boolean));
                        dataTable.Columns[i].DataType = typeof(Boolean);
                        break;
                    default:
                        //dataTable.Columns.Add(DataTableWithTargetColumn.Columns[i].ColumnName, typeof(string));
                        dataTable.Columns[i].DataType = typeof(string);
                        unknowTypes += typeNameList[i] + "-" + DataTableWithTargetColumn.Columns[i].ColumnName + " ";
                        break;
                }
            }
            if (unknowTypes != string.Empty)
                MessageBox.Show(unknowTypes);
            return dataTable;
        }


        private DataTable GetData(string tableName)
        {
            DataTable tempDataTable;

            using (con = new SqlConnection(connectionString))
            {

                SqlCommand cmd = new SqlCommand(SQL_getDataPrefix + tableName, con);
                sda = new SqlDataAdapter(cmd);

                tempDataTable = new DataTable(tableName);

                sda.Fill(tempDataTable);
                sda.FillSchema(tempDataTable, SchemaType.Source);
            }
            return tempDataTable;
        }

        private void FillDataGrid(DataTable dataTable, DataTable tempDataTable)
        {

            foreach (DataRow row in tempDataTable.Rows)
            {
                dataTable.ImportRow(row);

            }

            return;
        }

        #endregion

    }
}
