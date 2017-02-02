using System.Xml;
using Microsoft.WindowsAzure.Storage.Table;

namespace TranslationRobot
{
    public class TranslatedAddressEntity:TableEntity
    {
        public const string DefaultPartitionKey = "Assorted";
        public const string TableName = "TranslatedAddress";

        public TranslatedAddressEntity()
        {
            PartitionKey = DefaultPartitionKey;
        }

        public TranslatedAddressEntity(string input)
        {
            RowKey = input;
            PartitionKey = DefaultPartitionKey;
        }

        public string Country { get; set; }

        public string Translation{ get; set; }

        public double Longitude { get; set; }
        public double Lattitude { get; set; }

        public string CountryCode { get; set; }

        public string LanguageCode { get; set; }

        public string Input
        {
            get
            {
                return RowKey;
            }
            set { RowKey = value; }
        }

        public string OriginalAddress { get; set; }

        public void SetCountryFromGoogleResult(XmlDocument xmlDocument)
        {
            foreach (XmlElement xmlElement in xmlDocument.GetElementsByTagName("address_component"))
            {
                XmlNodeList elementsByTagName = xmlElement.GetElementsByTagName("type");
                var isCountryNode = false;
                foreach (XmlNode node in elementsByTagName)
                {
                    if (node.InnerText == "country")
                    {
                        isCountryNode = true;
                    }
                }
                if (isCountryNode)
                {
                    CountryCode = xmlElement.GetElementsByTagName("short_name")[0].InnerText.ToLower();
                    Country = xmlElement.GetElementsByTagName("long_name")[0].InnerText.ToLower();
                }
            }

        }
    }
}