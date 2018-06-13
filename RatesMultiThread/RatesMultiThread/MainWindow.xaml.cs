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
using RatesMultiThread.Resources.AppCode;
using RatesMultiThread.Resources.UserControls;
using RatesMultiThread.Resources.ViewModels;
using RatesMultiThread.Resources;
using VeryHotKeys.WPF;

namespace RatesMultiThread
{
    public partial class MainWindow : Window
    {
        public static XMLHandler MainXMLHander;
        public static Dictionary<RateUserControl,XMLRateUserControlData> WebServices;
        public static StackPanel WSStack;

        public bool IsShowing = true;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            MainXMLHander = new XMLHandler();

            // Read XML file initialy to check / create files
            MainXMLHander.ReadXMLFile();

            WSStack = MainStack;

            WebServices = new Dictionary<RateUserControl, XMLRateUserControlData>();

            CreateRaters();
        }

        // Remove and Create User Controls From Main Stack
        public void CreateRaters()
        {
            try
            {
                MainStack.Children.RemoveRange(0, MainStack.Children.Count - 1);
                XMLRateUserContsols webservices = MainXMLHander.ReadXMLFile();
                if ((webservices != null) && (webservices.WSRates.Count > 0))
                {
                    for (int z = 0; z < webservices.WSRates.Count; z++)
                    {
                        RateUserControl newWSRateMain = new RateUserControl();
                        RateUserDataControlViewModel model = newWSRateMain.ViewModel.RateDataControl.ViewModel;
                        XMLRateUserControlData i = MainXMLHander.ReadXMLFile().WSRates[z];

                        newWSRateMain.VerticalAlignment = VerticalAlignment.Stretch ;
                        newWSRateMain.HorizontalAlignment = HorizontalAlignment.Stretch;
                        newWSRateMain.Height = Double.NaN;
                        newWSRateMain.Width = Double.NaN;

                        model.RateType = i.Type;
                        model.CurrencyFrom = i.CurrencyFrom;
                        model.CurrencyTo = i.CurrencyTo;
                        model.Symbol = i.Symbol;
                        model.SelectedRefreshTime = model.RefreshTimes.Where(x => x.Key == i.RefreshTime).FirstOrDefault();
                        model.StartRatingThread();
                        MainStack.Children.Insert(0, newWSRateMain);

                        WebServices.Add(newWSRateMain, i);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Could Not Create Controls : " + e.Message);
            }
        }

        // Do window alignment
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Width = desktopWorkingArea.Width * 0.30;
            this.Left = desktopWorkingArea.Right - this.Width;
            this.Top = desktopWorkingArea.Top;
            this.Height = desktopWorkingArea.Height;

            MainStack.Width = desktopWorkingArea.Width * 0.30;
            MainStack.Height = desktopWorkingArea.Height;
        }

        private void btnAddNewWS_Click(object sender, RoutedEventArgs e)
        {
            AddNewWS();
        }

        // Add a new UserControl to Stack
        public void AddNewWS()
        {
            RateUserControl newRateMain = new RateUserControl();
            newRateMain.ViewModel.ChangeView(true);
            MainStack.Children.Insert(0, newRateMain);
        }

        // Save new Controls to XML.
        public static void Save(RateUserControl control, XMLRateUserControlData newService)
        {
            WebServices[control] = newService;
            MainXMLHander.SaveServices(WebServices.ToList().Select(x => x.Value).ToList());
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }


        //create new hot key for hidding and showing the window
        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            HotKeyRegisterer hotKey = new HotKeyRegisterer(this, OnHotKeyDown, HotKeyMods.Control, ConsoleKey.NumPad0);
        }

        public void OnHotKeyDown()
        {
            if (IsShowing)
            {
                this.Hide();
                IsShowing = false;
            }
            else
            {
                this.Show();
                IsShowing = true;
            }
        }
    }
}
