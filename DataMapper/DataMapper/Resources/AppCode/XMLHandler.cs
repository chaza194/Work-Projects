using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Serialization;
using DataMapper;
using System.IO;

namespace DataMapper.Resources.AppCode
{
    class XMLHandler
    {
        public const string FileDirectory = "TreeXMLData.ini";
        public void SaveTreeToXML(TreeView treeView)
        {
            string xml = TreeViewToXML(treeView);
            File.WriteAllText(FileDirectory, xml);
        }

        public void LoadTreeFromXML(TreeView treeView)
        {
            if (File.Exists(FileDirectory))
            {
                string xml = File.ReadAllText(FileDirectory);
                treeView.Items.Clear();

                foreach (SQLTreeItem i in XMLToTreeView(xml))
                {
                    treeView.Items.Add(i);
                }
            }
        }

        public List<SQLTreeItem> XMLToTreeView(string xml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(XMLTreeViews));
            TextReader reader = new StringReader(xml);
            XMLTreeViews view = xmlSerializer.Deserialize(reader) as XMLTreeViews;

            return GetSQLNodesFromXML(view.ChildNodes);

        }

        public List<SQLTreeItem> GetSQLNodesFromXML(List<XMLTreeView> items)
        {
            List<SQLTreeItem> result = new List<SQLTreeItem>();
            foreach (XMLTreeView i in items)
            {
                SQLTreeItem newItem = new SQLTreeItem()
                {
                    Header = i.Header,
                    SQL = i.SQL,
                    SchemaSQL = i.SchemaSQL,
                    KeyFields = i.KeyFields,
                    ItemType = i.ItemType
                };
                

                if(i.ChildNodes.Count > 0)
                {
                    foreach (SQLTreeItem x in GetSQLNodesFromXML(i.ChildNodes))
                    {
                        newItem.Items.Add(x);
                    }
                }
                result.Add(newItem);
            }
            return result;
        }

        public string TreeViewToXML(TreeView treeView)
        {
            XMLTreeViews views = new XMLTreeViews();
            List<SQLTreeItem> items = new List<SQLTreeItem>();

            items.AddRange(treeView.Items.OfType<SQLTreeItem>());

            views.ChildNodes = GetXMLFromSQLNodes(items);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(XMLTreeViews));
            TextWriter textWriter = new StringWriter();
            xmlSerializer.Serialize(textWriter, views);

            return textWriter.ToString();
        }

        public List<XMLTreeView> GetXMLFromSQLNodes(List<SQLTreeItem> items)
        {
            List<XMLTreeView> newList = new List<XMLTreeView>();

            foreach(SQLTreeItem i in items)
            {
                XMLTreeView newItem = new XMLTreeView();
                newItem.ChildNodes = new List<XMLTreeView>();
                newItem.Header = i.Header.ToString();
                newItem.SQL = i.SQL;
                newItem.SchemaSQL = i.SchemaSQL;
                newItem.KeyFields = i.KeyFields;
                newItem.ItemType = i.ItemType;

                if (i.Items.Count > 0)
                {
                    List<SQLTreeItem> childNodes = new List<SQLTreeItem>();
                    childNodes.AddRange(i.Items.OfType<SQLTreeItem>());

                    List<XMLTreeView> newXMLList = GetXMLFromSQLNodes(childNodes);
                    newItem.ChildNodes.AddRange(newXMLList);
                }
                newList.Add(newItem);
            }
            return newList;
        }

    }

    [Serializable]
    [XmlRoot]
    public class XMLTreeViews
    {
        [XmlArray]
        public List<XMLTreeView> ChildNodes;
    }

    [Serializable]
    public class XMLTreeView
    {
        public string Header;
        public string SQL;
        public string SchemaSQL;
        public string[] KeyFields;
        public SQLTreeItem.SQLType ItemType;
        [XmlArray]
        public List<XMLTreeView> ChildNodes;
    }
}
