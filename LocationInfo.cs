
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace TranslationRobot
{
    public class LocationInfo
    {

        private XmlDocument _ipInfoXML;
        public LocationInfo(string ipAddress)
        {
            string url = "http://ip-api.com/xml/";
            string checkURL = url + ipAddress;
            string ipResponse = RequestHelper.DownloadString(checkURL);

            _ipInfoXML = new XmlDocument();
            _ipInfoXML.LoadXml(ipResponse);

        }


        public string GetCountryName()
        {
            XmlNodeList responseXML = _ipInfoXML.GetElementsByTagName("country");
            return responseXML.Item(0).InnerText;
        }


        public string GetCountryCode()
        {
            XmlNodeList responseXML = _ipInfoXML.GetElementsByTagName("countryCode");
            return responseXML.Item(0).InnerText;
        }

        private const string GoogleMapsApiKey = "AIzaSyBEaCk1QdMRA_jpJ3GJksA5Z6agwd9ps7Y";
        private const string BingMapsKey = "Ag2p3wHuXnmlaO-LffokUlisExjYT6n70Vo9t71n72V5qQ_ZqA6gmPT4cXuD1Ych";


        internal static string GetLocalizedAddressFromGoogle(string address, TranslatorAccess translatorAccess)
        {
            string encodedAddress = HttpUtility.UrlEncode(address);
           
            string url ="https://maps.googleapis.com/maps/api/geocode/xml?address="+encodedAddress+"&key=" +GoogleMapsApiKey;
            string result = RequestHelper.DownloadString(url);
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(result);
            
            string country = GetCountryFromGoogleResult(xmlDocument);

          
            string languageCode = translatorAccess.GetLanguageCode(country);
            string localizedAddress;
            if (languageCode == "hi")
            {
                Location location = GetLocationFromGoogleResult(xmlDocument);
                localizedAddress = GetLocalizedAddressFromBing(location, languageCode);
            }
            else
            {
                string localizedResult = RequestHelper.DownloadString(url + "&language=" + languageCode);
                XmlDocument localizedXmlDocument = new XmlDocument();
                localizedXmlDocument.LoadXml(localizedResult);
                localizedAddress = localizedXmlDocument.GetElementsByTagName("formatted_address").Item(0).InnerText;
            }

        
            return localizedAddress;
        }

        private static string GetLocalizedAddressFromBing(Location location, string languageCode)
        {
            http://dev.virtualearth.net/REST/v1/Locations/47.64054,-122.12934?o=xml&key=BingMapsKey
            string url = " http://dev.virtualearth.net/REST/v1/Locations/" + location.Lattitude.ToString(CultureInfo.InvariantCulture)+","+location.Longitude.ToString(CultureInfo.InvariantCulture) + "?o=xml&inclnb=1&key=" + BingMapsKey + "&c=" + languageCode;
            string result = RequestHelper.DownloadString(url);
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(result);
            int numberOfResults = int.Parse(xmlDocument.GetElementsByTagName("EstimatedTotal").Item(0).InnerText);
            if (numberOfResults == 0)
            {
                throw new AddressNotFoundException("Location could not be located: " + location);
            }
        
            string localizedAddress = xmlDocument.GetElementsByTagName("FormattedAddress").Item(0).InnerText;
            return localizedAddress;
        }

        private static Location GetLocationFromGoogleResult(XmlDocument xmlDocument)
        {
            var location = new Location();
            XmlElement xmlElement = (XmlElement) xmlDocument.GetElementsByTagName("location").Item(0);
            string lattitudeString = xmlElement.GetElementsByTagName("lat")[0].InnerText;

            location.Lattitude =double.Parse(lattitudeString, CultureInfo.InvariantCulture);

            string longitudeString = xmlElement.GetElementsByTagName("lng")[0].InnerText;
            location.Longitude = double.Parse(longitudeString, CultureInfo.InvariantCulture);
            return location;
        }

        private static string GetCountryFromGoogleResult(XmlDocument xmlDocument)
        {
            string country = null;
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
                    country = xmlElement.GetElementsByTagName("short_name")[0].InnerText.ToLower();
                }
            }
            return country;
        }


        public static string GetLocationInfo(string address, TranslatorAccess translatorAccess)
        {
            return GetLocalizedAddressFromGoogle(address, translatorAccess);
        }

        private static string GetLocalizedAddressFromBing(string address, TranslatorAccess translatorAccess)
        {
            string encodedAddress = HttpUtility.UrlEncode(address);
            string url = " http://dev.virtualearth.net/REST/v1/Locations/" + encodedAddress +
                         "?o=xml&incl=queryParse,ciso2&inclnb=1&key="+BingMapsKey;
            string result = RequestHelper.DownloadString(url);
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(result);
            int numberOfResults = int.Parse(xmlDocument.GetElementsByTagName("EstimatedTotal").Item(0).InnerText);
            if (numberOfResults == 0)
            {
                throw new AddressNotFoundException("Address could not be located: " + address);
            }
            string country = xmlDocument.GetElementsByTagName("CountryRegionIso2").Item(0).InnerText;
            string languageCode = translatorAccess.GetLanguageCode(country);
            string localizedResult = RequestHelper.DownloadString(url + "&c=" + languageCode);
            XmlDocument localizedXmlDocument = new XmlDocument();
            localizedXmlDocument.LoadXml(localizedResult);
            string localizedAddress = localizedXmlDocument.GetElementsByTagName("FormattedAddress").Item(0).InnerText;
            return localizedAddress;
        }
    } 
}
