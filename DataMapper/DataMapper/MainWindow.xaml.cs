using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DataMapper.Resources.AppCode;

namespace DataMapper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowViewModel ViewModel;
        
        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainWindowViewModel();
            DataContext = ViewModel;

            ViewModel.LoadTreeView(MainTree);

            if(MainTree.Items.Count == 0)
            {
                btnAddNew.Visibility = Visibility.Visible;
            }
        }

        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
            AddNewSQLNodeWindow addNewSQLNodeWindow = new AddNewSQLNodeWindow();
            addNewSQLNodeWindow.ShowDialog();
            ViewModel.AddNewSQLNode(MainTree, addNewSQLNodeWindow.ViewModel.HeaderName, addNewSQLNodeWindow.ViewModel.SQLText, addNewSQLNodeWindow.ViewModel.KeyFields, addNewSQLNodeWindow.ViewModel.SchemaSQL);
            ViewModel.SaveTreeView(MainTree);
        }

        private void btnDeSelectAll_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.DeselectAllInTreeView(MainTree);
        }

        private void btnRefreshSchema_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.RefreshSelectedSQLItemSchema(MainTree);
            ViewModel.SaveTreeView(MainTree);
        }

        private void MainTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue != null)
            {
                if (e.NewValue.GetType() == typeof(SQLTreeItem))
                {
                    if (((SQLTreeItem)e.NewValue).ItemType == SQLTreeItem.SQLType.SQL)
                    {
                        btnRefreshSchema.Visibility = Visibility.Visible;
                        btnAddNew.Visibility = Visibility.Hidden;
                        btnEdit.Visibility = Visibility.Visible;
                    }
                   else
                    {
                        btnRefreshSchema.Visibility = Visibility.Hidden;
                        btnAddNew.Visibility = Visibility.Visible;
                        btnEdit.Visibility = Visibility.Hidden;
                    }
                    btnDelete.Visibility = Visibility.Visible;
                }
                else
                {
                    btnRefreshSchema.Visibility = Visibility.Hidden;
                    btnAddNew.Visibility = Visibility.Hidden;
                    btnDelete.Visibility = Visibility.Hidden;
                    btnEdit.Visibility = Visibility.Hidden;
                }
            }
            else
            {
                btnRefreshSchema.Visibility = Visibility.Hidden;
                btnAddNew.Visibility = Visibility.Hidden;
                btnDelete.Visibility = Visibility.Hidden;
                btnEdit.Visibility = Visibility.Hidden;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            SQLTreeItem selected = ((SQLTreeItem)MainTree.SelectedItem);
            ViewModel.DeleteSelectedNode(MainTree.Items, selected);
            ViewModel.SaveTreeView(MainTree);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            AddNewSQLNodeWindow addNewSQLNodeWindow = new AddNewSQLNodeWindow();
            SQLTreeItem selected = ((SQLTreeItem)MainTree.SelectedItem);
            addNewSQLNodeWindow.ViewModel.HeaderName = selected.Header.ToString();
            addNewSQLNodeWindow.ViewModel.SQLText = selected.SQL;
            addNewSQLNodeWindow.ViewModel.SchemaSQL = selected.SchemaSQL;

            if (selected.KeyFields != null)
            {
                string fields = "";
                foreach (string str in selected.KeyFields)
                {
                    if (str.Trim() != "")
                    {
                        fields = fields + str + "; ";
                    }
                }
                addNewSQLNodeWindow.ViewModel.KeyFields = fields;
            }

            addNewSQLNodeWindow.ShowDialog();

            selected.Header = addNewSQLNodeWindow.ViewModel.HeaderName;
            selected.SQL = addNewSQLNodeWindow.ViewModel.SQLText;
            selected.KeyFields = addNewSQLNodeWindow.ViewModel.KeyFields.Split(';');
            selected.SchemaSQL = addNewSQLNodeWindow.ViewModel.SchemaSQL;
            ViewModel.SaveTreeView(MainTree);
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ExportTreeView(MainTree);
        }
    }

    public class MainWindowViewModel
    {
        private XMLHandler _XMLHandler;
        private SQLHandler _SQLHandler;
        private JsonHandler _JsonHandler;

        public string ConnectionStr { get { return _ConnectionStr; }set { _ConnectionStr = value; }}
        public string Params { get { return _Params; }set { _Params = value; }}

        private string _ConnectionStr;
        private string _Params;

        public MainWindowViewModel()
        {
            _XMLHandler = new XMLHandler();
            _SQLHandler = new SQLHandler();
            _JsonHandler = new JsonHandler();
        }

        public void AddNewSQLNode(TreeView treeView, string headerText, string sQLText, string keyFields, string schemaSQL)
        {
            if (((headerText ?? "") != "") && ((sQLText ?? "") != "") && ((schemaSQL ?? "") != ""))
            {
                SQLTreeItem newItem = new SQLTreeItem();
                newItem.Header = headerText;
                newItem.SQL = sQLText;
                newItem.KeyFields = (keyFields != null) ? keyFields.Split(';') : new string[0];
                newItem.SchemaSQL = schemaSQL;


                if (treeView.SelectedItem != null)
                {
                    if (((SQLTreeItem)treeView.SelectedItem).ItemType == SQLTreeItem.SQLType.SQL)
                    {
                        newItem.ItemType = SQLTreeItem.SQLType.Schema;
                    }
                    else if (((SQLTreeItem)treeView.SelectedItem).ItemType == SQLTreeItem.SQLType.Schema)
                    {
                        newItem.ItemType = SQLTreeItem.SQLType.SQL;
                    }
                    ((SQLTreeItem)treeView.SelectedItem).Items.Add(newItem);
                }
                else
                {
                    treeView.Items.Add(newItem);
                }
            }
        }

        public void ExportTreeView(TreeView t)
        {
            _JsonHandler.ExportSQLTreeToJson(t, ConnectionStr, (Params != null)? Params.Split(';'): new string[0]);
        }

        public void SaveTreeView(TreeView t)
        {
            _XMLHandler.SaveTreeToXML(t);
        }

        public void LoadTreeView(TreeView t)
        {
            _XMLHandler.LoadTreeFromXML(t);
        }

        public void DeselectAllInTreeView(TreeView treeView)
        {
            if (treeView.SelectedItem != null)
            {
                ((SQLTreeItem)treeView.SelectedItem).IsSelected = false;
            }
        }

        public void DeleteSelectedNode(ItemCollection collection, SQLTreeItem itemToDelete)
        {
            
            foreach(SQLTreeItem i in collection)
            {
                if (i.Items.Count > 0)
                {
                    if(i == itemToDelete)
                    {
                        collection.Remove(i);
                        break;
                    }

                    int index = i.Items.IndexOf(itemToDelete);
                    if (index > -1)
                    {
                        i.Items.RemoveAt(index);
                        break;
                    }
                    else
                    {
                        DeleteSelectedNode(i.Items, itemToDelete);
                    }
                }
            }
        }

        public void RefreshSelectedSQLItemSchema(TreeView treeView)
        {
            SQLTreeItem item = ((SQLTreeItem)treeView.SelectedItem);
            item.Items.Clear();

            List<string> columnSchemas = _SQLHandler.GetSQlSchemaFromSQLItem(item, ConnectionStr);

            foreach(string colname in columnSchemas)
            {
                item.Items.Add(new SQLTreeItem()
                {
                    Header = colname,
                    ItemType = SQLTreeItem.SQLType.Schema
                });
            }
        }
    }

    public class SQLTreeItem : TreeViewItem
    {
        public enum SQLType {SQL, Schema}

        public SQLType ItemType;
        public string[] KeyFields;
        public string SQL;
        public string SchemaSQL;
    }
}
