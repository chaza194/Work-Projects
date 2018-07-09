using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonInjector.Resources.UserControls;
using JsonInjector.Resources.ViewModels;
using JsonInjector.Resources.BusinessLogic;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows.Threading;

namespace JsonInjector.Resources.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public StackPanel MainStack;
        private DataHandler _DataHandler;

        public string LogText { get { return _LogText; } set { _LogText = value; OnPropertyChanged("LogText"); } }
        public string _LogText;

        public void AddObjectToStackPanel(UIElement obj)
        {
            if (MainStack != null)
            {
                MainStack.Children.Add(obj);
            }
        }

        public void RemoveObjectFromStackPanel(UIElement obj)
        {
            if (MainStack != null)
            {
                MainStack.Children.Remove(obj);
            }
        }

        public ucJsonDirectory CreateDirectoryObject(string directory, JsonDirectoryViewModel.Remove del = null)
        {
            ucJsonDirectory dirObject = new ucJsonDirectory();
            dirObject.ViewModel.Directory = directory;
            dirObject.ViewModel.RemoveDirectory += del;
            return dirObject;
        }

        public void ProcessJson(TextBox txtBox = null)
        {
            if (MainStack.Children.Count > 0)
            {
                if (ValidateFiles())
                {
                    _DataHandler = new DataHandler();
                    DataHandler.OnLogChange += UpdateLog;

                    List<ucJsonDirectory> elements = MainStack.Children.Cast<UIElement>().ToList().Select(x => ((ucJsonDirectory)x)).ToList();
                    _DataHandler.ProcessData(elements.Select(x => x.ViewModel.Directory).ToList());
                }
                else
                {
                    MessageBox.Show("Not all files are Json Files. Make sure all files end in '.JSON'");
                }
            }
            else
            {
                MessageBox.Show("There needs to be at least one File");
            }
        }

        public bool ValidateFiles()
        {
            List<UIElement> elements = MainStack.Children.Cast<UIElement>().ToList();
            int count = elements.Where(x => !((ucJsonDirectory)x).ViewModel.Directory.ToLower().Contains(".json")).Count();

            if(count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void UpdateLog(string txt)
        {
            MainStack.Dispatcher.Invoke(new Action(() => { LogText = txt; }));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
