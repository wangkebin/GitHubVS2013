using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Documents;
using NUnit.Framework;

namespace VirtualConnection
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private string connectionString;

        private DataTable tempDataTable;
        private DataTable virtualConnectionDataTable;
        private DataTable physicalConnectionDataTable;


        private SqlCommandBuilder cb;
        private SqlDataAdapter sda;
        private SqlConnection con;
        string CmdString = "select * from ";

        private List<string> typeNameList = new List<string>();
        public MainWindow()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            //string tableName = ConfigurationManager.AppSettings["TableName"];
            //tableName;

            FillDataTables();
            VirtualConnectionDataGrid.ItemsSource = virtualConnectionDataTable.DefaultView;

            Assert.IsTrue(ValueContainedInTargetColumnValues<string>(physicalConnectionDataTable, "TargetCompID", "QAFIX"));
            Assert.IsFalse(ValueContainedInTargetColumnValues<string>(physicalConnectionDataTable, "TargetCompID", "QAFX"));
            Assert.IsTrue(ValueContainedInTargetColumnValues<int>(physicalConnectionDataTable, "Side", 1));
            Assert.IsFalse(ValueContainedInTargetColumnValues<int>(physicalConnectionDataTable, "Side", 0));
        }

        private void FillDataTables()
        {
            GetDataType("fix_physical_connections");
            FillDataGrid(ref physicalConnectionDataTable, "fix_physical_connections");

            GetDataType("fix_virtual_connections");
            FillDataGrid(ref virtualConnectionDataTable, "fix_virtual_connections");
        }


        public bool ValueContainedInTargetColumnValues<T>(DataTable targetDataTable, string targetColumnName, T targetValue )
        {

            List<object> query = (from q in targetDataTable.AsEnumerable()
                select q[targetColumnName]).ToList();

            return query.Contains(targetValue);
        }

        public bool ValueContainedInTargetSet<T>(IEnumerable<T> targetSet, T targetValue)
        {
            return targetSet.Contains(targetValue);
        }



        #region help functions
        private void GetDataType(string tableName)
        {
            typeNameList.Clear();
            using (con = new SqlConnection(connectionString))
            {

                SqlCommand cmd = new SqlCommand(CmdString + tableName, con);
                
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
        }



        private void FillDataGrid(ref DataTable dataTable, string tableName)
        {
            string unknowTypes = string.Empty;
            tempDataTable = new DataTable();

            using (con = new SqlConnection(connectionString))
            {

                SqlCommand cmd = new SqlCommand(CmdString + tableName, con);
                sda = new SqlDataAdapter(cmd);

                tempDataTable = new DataTable("tempTable");




                sda.Fill(tempDataTable);
                sda.FillSchema(tempDataTable, SchemaType.Source);
            }

            dataTable = new DataTable(tableName);
            // Populate columns
            for (int i = 0; i < typeNameList.Count; i++)
            {
                switch (typeNameList[i])
                {
                    case "int":
                    case "smallint":
                    case "bigint":
                        dataTable.Columns.Add(tempDataTable.Columns[i].ColumnName, typeof(int));
                        break;
                    case "varchar":
                    case "char":
                    case "text":
                        dataTable.Columns.Add(tempDataTable.Columns[i].ColumnName, typeof(string));
                        break;
                    case "binary":
                        dataTable.Columns.Add(tempDataTable.Columns[i].ColumnName, typeof(string));
                        break;
                    case "datetime":
                    //case "timestamp":
                        dataTable.Columns.Add(tempDataTable.Columns[i].ColumnName, typeof(DateTime));
                        break;
                    case "decimal":
                        dataTable.Columns.Add(tempDataTable.Columns[i].ColumnName, typeof(Decimal));
                        break;
                    case "bit":
                        dataTable.Columns.Add(tempDataTable.Columns[i].ColumnName, typeof(Boolean));
                        break;
                    default:
                        dataTable.Columns.Add(tempDataTable.Columns[i].ColumnName, typeof(string));
                        unknowTypes += typeNameList[i] + "-" + tempDataTable.Columns[i].ColumnName + " ";
                        break;
                }
            }

            foreach (DataRow row in tempDataTable.Rows)
            {
                dataTable.ImportRow(row);
            }

           if(unknowTypes != string.Empty)
                MessageBox.Show(unknowTypes);
            
        }

        #endregion

        #region handlers
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DataTable dtCopy = virtualConnectionDataTable.Clone();
            try
            {
                con = new SqlConnection(connectionString);

                //sda = new SqlDataAdapter();
                cb = new SqlCommandBuilder(sda);
                sda.SelectCommand = new SqlCommand(CmdString, con);
                sda.UpdateCommand = cb.GetUpdateCommand();
                sda.Update(virtualConnectionDataTable);
            }
            catch (Exception ex)
            {
                virtualConnectionDataTable = dtCopy;
                Debug.WriteLine(ex + Environment.StackTrace);
            }
            finally
            {
                if(con != null)
                    con.Close();
            }
        }

        #endregion

    }
}
