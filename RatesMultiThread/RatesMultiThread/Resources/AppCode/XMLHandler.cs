using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Windows;

namespace RatesMultiThread.Resources.AppCode
{
    // Baisc XML Serialization in here
   public class XMLHandler
    {
        public string FileLocation;
        public string FileName = "WSWebServices.ini";

        public void CreateXMLFile(List<XMLRateUserControlData> services =null)
        {
            try
            {
                XmlSerializer mySerializer = new XmlSerializer(typeof(XMLRateUserContsols));
                TextWriter writer = new StreamWriter(FileName);
                XMLRateUserContsols newXMLObject = new XMLRateUserContsols();

                if (services != null)
                {
                    newXMLObject.WSRates = new List<XMLRateUserControlData>();
                    newXMLObject.WSRates.AddRange(services.AsEnumerable());
                }

                mySerializer.Serialize(writer, newXMLObject);

                writer.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Could Not Save / Create Data : " + e.Message);
            }
}

        public XMLRateUserContsols ReadXMLFile()
        {
            try
            {
                if (!File.Exists(FileName))
                {
                    CreateXMLFile();
                }

                XmlSerializer mySerializer = new XmlSerializer(typeof(XMLRateUserContsols));
                TextReader reader = new StreamReader(FileName);

                return (XMLRateUserContsols)mySerializer.Deserialize(reader);
            } 
            catch (Exception e)
            {
                MessageBox.Show("Could Not Read File : "+ e.Message);
                return null;
            }
        }

        public void SaveServices(List<XMLRateUserControlData> services)
        {
            CreateXMLFile(services);
        }
    }

    [XmlRoot]
    public class XMLRateUserContsols
    {
        [XmlArray]
        public List<XMLRateUserControlData> WSRates;
    }


    public class XMLRateUserControlData
    {
        public APIThread.WSRateType Type;
        public string CurrencyFrom;
        public string CurrencyTo;
        public string Symbol;
        public string RefreshTime;
        public string ID;
    }
}
