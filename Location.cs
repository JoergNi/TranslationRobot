using System.Xml;

namespace TranslationRobot
{
    public class Location
    {
        public double Longitude { get; set; }
        public double Lattitude { get; set; }

        public string FormattedAddress { get; set; }

        public string  Country { get; set; }
        public string CountryCode { get; set; }

        public string LanguageCode { get; set; }
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
                    Country = xmlElement.GetElementsByTagName("short_name")[0].InnerText.ToLower();
                }
            }

        }
    
    }
}