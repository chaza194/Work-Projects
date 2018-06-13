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
using System.Windows.Shapes;

namespace DataMapper
{
    /// <summary>
    /// Interaction logic for AddNewSQLNodeWindow.xaml
    /// </summary>
    public partial class AddNewSQLNodeWindow : Window
    {
        public AddNewSQLNodeWindowViewModel ViewModel;
        public AddNewSQLNodeWindow()
        {
            InitializeComponent();
            ViewModel = new AddNewSQLNodeWindowViewModel();
            this.DataContext = ViewModel;
        }
    }

    public class AddNewSQLNodeWindowViewModel
    {
        public string HeaderName { get { return _HeaderName; } set { _HeaderName = value; } }
        public string SQLText { get { return _SQLText; } set { _SQLText = value; } }
        public string KeyFields { get { return _KeyFields; } set { _KeyFields = value; } }
        public string SchemaSQL { get { return _SchemaSQL; } set { _SchemaSQL = value; } }

        private string _HeaderName;
        private string _SQLText;
        private string _KeyFields;
        private string _SchemaSQL;
    }
}
