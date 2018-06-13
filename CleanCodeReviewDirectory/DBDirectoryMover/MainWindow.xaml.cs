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
using System.IO;
using System.Data.SqlClient;
using System.Data;
using Microsoft.VisualBasic.FileIO;
using System.ComponentModel;

namespace CleanCodeReviewDirectory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string[] files;
        public static string ConStr;
        public static string UserName;
        public static string DirFrom;
        public static string DirTo;
        public static int MaxCount;
        public static string CommandText;
        public static string KeyField;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnGo_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInputs())
            {
                RunCleanUp();
            }
        }

        private string ValidateFields()
        {
            string message = "";

            ConStr = txtConStr.Text;
            if (ConStr == "") { message = lblConStr.Content.ToString(); } 
            DirFrom = txtDirFrom.Text;
            if (DirFrom == "") { message = lblCopyFrom.Content.ToString(); }
            DirTo = txtDirTo.Text;
            if (DirFrom == "") { message = lblCopyTo.Content.ToString(); }

            string fieldValues = "";
            fieldValues = txtSelect.Text;
            if (fieldValues == "") { message = lblSelect.Content.ToString(); }
            fieldValues = txtFrom.Text;
            if (fieldValues == "") { message = lblFrom.Content.ToString(); }
            fieldValues = txtWhere.Text;
            if (fieldValues == "") { message = lblWhere.Content.ToString(); }
            KeyField = txtKeyField.Text.Trim();
            if (KeyField == "") { message = lblKeyField.Content.ToString(); }

            if(message != "")
                message = "Please populate field : " + message;

            if(((txtSelect.Text ?? "") != "") && ((txtKeyField.Text ?? "") != ""))
            {
                if (!txtSelect.Text.Contains(txtKeyField.Text))
                {
                    message = "Key field needs to be included in the select";
                }
            }

            return message;
        }

        private void RunCleanUp()
        {
            string erMsg = ValidateFields();
            files = ValidateDirectiories();
            if(files == null) { erMsg = "Could not find directories"; }

            if ((erMsg == "") || (files == null))
            {
                CommandText = BuildCommandText();

                ProgBar.Minimum = 0;
                ProgBar.Visibility = Visibility.Visible;


                btnGo.IsEnabled = false;
                BackgroundWorker worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.DoWork += CopyFilesAsync;
                worker.ProgressChanged += ProgressChanged;
                worker.RunWorkerAsync();
                worker.RunWorkerCompleted += WorkerCleanUp;
            }
            else
            {
                lblErrorMessage.Content = erMsg;
            }
        }

        void WorkerCleanUp(object sender, RunWorkerCompletedEventArgs e)
        {
            btnGo.IsEnabled = true;
        }

        private void CopyFilesAsync(Object sender, DoWorkEventArgs e)
        {
            if (files != null)
            {
                DataSet Dataset = GetData();
                if (Dataset != null)
                {
                    MaxCount = Dataset.Tables[0].Rows.Count;
                    int count = 0;
                    foreach (DataRow row in Dataset.Tables[0].Rows)
                    {
                        string file = files.Where(x => x.Contains(row[KeyField].ToString())).FirstOrDefault();
                        if (file != null)
                        {
                            string newFileFromDirectory = DirFrom + @"\" + file;
                            DirectoryInfo directoryCopyFromInfo = new DirectoryInfo(newFileFromDirectory);

                            string newFileToDirectory = DirTo + @"\" + file;
                            DirectoryInfo directoryCopyToInfo = new DirectoryInfo(newFileToDirectory);

                            if ((directoryCopyFromInfo.Exists == true) && (directoryCopyToInfo.Exists == false))
                            {
                                FileSystem.CopyDirectory(newFileFromDirectory, newFileToDirectory);
                            }
                        }
                        count++;
                        (sender as BackgroundWorker).ReportProgress(count);
                    }
                }
            }
        }

        void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgBar.Value = e.ProgressPercentage;
            ProgBar.Maximum = MaxCount;
            if (e.ProgressPercentage == 100)
            {
                btnGo.IsEnabled = true;
            }
        }

        private DataSet GetData()
        {
            try
            {
                SqlConnection MainConnection;
                MainConnection = new SqlConnection(ConStr);
                MainConnection.Open();
                if (MainConnection.State == System.Data.ConnectionState.Open)
                {
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(CommandText, MainConnection);

                    DataSet dataSet = new DataSet();
                    sqlDataAdapter.Fill(dataSet);
                    MainConnection.Close();
                    return dataSet;
                }
                else
                    return null;
            }
            catch(Exception e)
            {
                string message = e.Message;
                string caption = "SQL Error";
                MessageBox.Show(message, caption);

                return null;
            }
        }

        private string[] ValidateDirectiories()
        {
            bool copyFromExists = Directory.Exists(txtDirFrom.Text);
            bool copyToExists = Directory.Exists(txtDirTo.Text);

            if ((copyFromExists) && (copyToExists))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(txtDirFrom.Text);
                return directoryInfo.GetDirectories().Select(X => X.Name).ToArray<string>();
            }
            else
                return null;
        }

        private bool ValidateInputs()
        {
            return true;
        }

        private string BuildCommandText()
        {
            string result = "SELECT " + txtSelect.Text + "  FROM " + txtFrom.Text + " WHERE "+txtWhere.Text;
            return result;
        }
    }
}
