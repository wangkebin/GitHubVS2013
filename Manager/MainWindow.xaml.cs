using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Configuration;
using System.Data.SqlClient;
using Manager.Biz;
using Manager.Data;
using Config = Manager.Biz.Configuration;

namespace ConnManager
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private string focusedTableName; 

        private string connectionString;
        private SqlCommandBuilder cb;
        private SqlDataAdapter sda;
        private SqlConnection con;
        string CmdString = "select * from ";

        DataTableManager dataTableManager = new DataTableManager();

        private List<string> typeNameList = new List<string>();
        public MainWindow()
        {
            InitializeComponent();
            InitializeData();
            
            //Assert.IsTrue(ValueContainedInTargetColumnValues<string>(physicalConnectionDataTable, "TargetCompID", "QAFIX"));
            //Assert.IsFalse(ValueContainedInTargetColumnValues<string>(physicalConnectionDataTable, "TargetCompID", "QAFX"));
            //Assert.IsTrue(ValueContainedInTargetColumnValues<int>(physicalConnectionDataTable, "Side", 1));
            //Assert.IsFalse(ValueContainedInTargetColumnValues<int>(physicalConnectionDataTable, "Side", 0));
        }

        private void InitializeData()
        {
            connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
           Config.LoadConfigurations();
            DataTableDBAdaptor loader = new DataTableDBAdaptor();
            foreach (string tableName in Config.TableList)
                dataTableManager.AddOrUpdateDataTable(loader.LoadDataTable(tableName));

            TableListView.Items.Clear();
            foreach (string tableName in Manager.Biz.Configuration.TableList)
                TableListView.Items.Add(tableName);

            focusedTableName = Config.TableList[0];

            LoadFocusedTableInGrid();
        }

        private void LoadFocusedTableInGrid()
        {
            ConnectionDataGrid.ItemsSource = dataTableManager.GetDataTable(focusedTableName).DefaultView;
        }

        //public bool ValueContainedInTargetColumnValues<T>(DataTable targetDataTable, string targetColumnName, T targetValue )
        //{

        //    List<object> query = (from q in targetDataTable.AsEnumerable()
        //        select q[targetColumnName]).ToList();

        //    return query.Contains(targetValue);
        //}

        //public bool ValueContainedInTargetSet<T>(IEnumerable<T> targetSet, T targetValue)
        //{
        //    return targetSet.Contains(targetValue);
        //}


        #region handlers
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                con = new SqlConnection(connectionString);

                sda = new SqlDataAdapter();
                cb = new SqlCommandBuilder(sda);
                sda.SelectCommand = new SqlCommand(CmdString + focusedTableName, con);
                sda.UpdateCommand = cb.GetUpdateCommand();
                //sda.InsertCommand = cb.GetInsertCommand();
                sda.Update(dataTableManager.GetDataTable(focusedTableName));
            }
            catch (Exception ex)
            {
                //virtualConnectionDataTable = dtCopy;
                Debug.WriteLine(ex + Environment.StackTrace);
            }
            finally
            {
                if(con != null)
                    con.Close();
            }
        }

        #endregion

        private void ConnectionDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void TableListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            focusedTableName = (sender as ListView).SelectedItem.ToString();
            LoadFocusedTableInGrid();
        }

    }
}
