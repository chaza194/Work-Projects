using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using Newtonsoft.Json.Converters;

namespace DataMapper.Resources.AppCode
{
    class JsonHandler
    {
        private SQLHandler _SQLHandler;

        public JsonHandler()
        {
            _SQLHandler = new SQLHandler();
        }

        public void ExportSQLTreeToJson(TreeView tree, string connectionStr, string[] globalParams)
        {
            List<object> mainList = new List<object>();

            // Loop through root nodes  
            foreach(SQLTreeItem item in tree.Items)
            {
                // Check that they are SQL nodes
                if(item.ItemType == SQLTreeItem.SQLType.SQL)
                {
                    //Execute SQL from SQL Nodes and dump results in ExpandoObject
                    DataTable table = _SQLHandler.GetDataFromSQLItem(item, connectionStr, globalParams);
                    string json = "{ "+  '"'+ item.Header + '"' + ":" + JsonConvert.SerializeObject(table)+"}";
                    ExpandoObject data = JsonConvert.DeserializeObject<ExpandoObject>(json, new ExpandoObjectConverter());

                    IDictionary<string, object> tableDictionary = data as IDictionary<string, object>;
                    //Loop through Schema nodes
                    foreach (SQLTreeItem childItem in item.Items)
                    {
                        // try and locate field in table definition
                        string fieldName = childItem.Header.ToString().ToLower();
                        foreach (object lookup in ((List<object>)tableDictionary.FirstOrDefault().Value))
                        {
                            IDictionary<string, object> dict = lookup as IDictionary<string, object>;
                            if (dict.ContainsKey(fieldName))
                            {
                                // If we have a Child SQL node to Exectute
                                if (childItem.Items.Count > 0)
                                {
                                    if (dict[fieldName] != null)
                                    {
                                        // Replace Value with SQL table Data
                                        KeyValuePair<string, object> newValue = GetSQLData(((SQLTreeItem)childItem.Items[0]), dict[fieldName].ToString(), connectionStr);
                                        dict[fieldName] = newValue;
                                    }
                                }
                            }
                        }
                    }
                    mainList.Add(tableDictionary);
                }
            }
            Dictionary<string, List<object>> keys = new Dictionary<string, List<object>>();
            keys.Add("Root", mainList);
            string finalJson = JsonConvert.SerializeObject(keys);
        }

        //Recursive function . For each Tree node :
        /* - Run EXEC SQL from node.
         * - Put Results into dynamic object.
         * - Call self agiain until all SQL Nodes have been run
         * - Add results from child calls to dynamic object made.*/

        public KeyValuePair<string, object> GetSQLData(SQLTreeItem sqlItem, string currentValue, string connectionStr) //SQLTreeItem item, string connectionStr, DataRow returnedRow, ExpandoObject prevObject
        {
            DataTable table = _SQLHandler.GetDataFromSQLItem(sqlItem, connectionStr, GetMappedFieldsWithValues(sqlItem.KeyFields,currentValue));
            string json = "{ " + '"' + sqlItem.Header + '"' + ":" + JsonConvert.SerializeObject(table) + "}";
            ExpandoObject data = JsonConvert.DeserializeObject<ExpandoObject>(json, new ExpandoObjectConverter());

            IDictionary<string, object> tableDictionary = data as IDictionary<string, object>;
            //Loop through Schema nodes
            foreach (SQLTreeItem childItem in sqlItem.Items)
            {
                // try and locate field in table definition
                string fieldName = childItem.Header.ToString().ToLower();
                foreach (object lookup in ((List<object>)tableDictionary.FirstOrDefault().Value))
                {
                    IDictionary<string, object> dict = lookup as IDictionary<string, object>;
                    if (dict.ContainsKey(fieldName))
                    {
                        // If we have a Child SQL node to Exectute
                        if (childItem.Items.Count > 0)
                        {
                            // Replace Value with SQL table Data
                            tableDictionary[fieldName] = GetSQLData(((SQLTreeItem)childItem.Items[0]), dict[fieldName].ToString(), connectionStr);
                        }
                    }
                }
            }
            return ((IDictionary<string, object>)data).FirstOrDefault();
        }

        public string[] GetMappedFieldsWithValues (string[] KeyFields, string returnedRow)
        {
            List<string> dict = new List<string>();
            foreach (string str in KeyFields)
            {
                if (str != "")
                {
                    string[] tmp = str.Split('=');
                    dict.Add(tmp[0] + " = " + returnedRow);
                }
            }    
            return dict.ToArray();
        }
    }
}
