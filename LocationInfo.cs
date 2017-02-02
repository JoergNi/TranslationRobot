
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
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


        internal static TranslatedAddressEntity GetLocationFromGoogle(string address, TranslatorAccess translatorAccess)
        {
            string encodedAddress = HttpUtility.UrlEncode(address);
           
            string url ="https://maps.googleapis.com/maps/api/geocode/xml?address="+encodedAddress+"&key=" +GoogleMapsApiKey;
            string result = RequestHelper.DownloadString(url);
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(result);



            TranslatedAddressEntity location = GetLocationFromGoogleResult(xmlDocument);
            location.Input = address;
            location.LanguageCode = GetLanguageCode(location.CountryCode);
            location.OriginalAddress = xmlDocument.GetElementsByTagName("formatted_address").Item(0).InnerText;

            string localizedAddress;
            if (location.LanguageCode == "hi")
            {
                
                localizedAddress = GetLocalizedAddressFromBing(address, translatorAccess);
            }
            else
            {
                string localizedResult = RequestHelper.DownloadString(url + "&language=" + location.LanguageCode);
                XmlDocument localizedXmlDocument = new XmlDocument();
                localizedXmlDocument.LoadXml(localizedResult);
                localizedAddress = localizedXmlDocument.GetElementsByTagName("formatted_address").Item(0).InnerText;
            }
            location.Translation = localizedAddress;
        
            return location;
        }

        private static string GetLocalizedAddressFromBing(TranslatedAddressEntity location, string languageCode)
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

        private static TranslatedAddressEntity GetLocationFromGoogleResult(XmlDocument xmlDocument)
        {
            var location = new TranslatedAddressEntity();
            XmlElement xmlElement = (XmlElement) xmlDocument.GetElementsByTagName("location").Item(0);
            string lattitudeString = xmlElement.GetElementsByTagName("lat")[0].InnerText;

            location.Lattitude =double.Parse(lattitudeString, CultureInfo.InvariantCulture);

            string longitudeString = xmlElement.GetElementsByTagName("lng")[0].InnerText;
            location.Longitude = double.Parse(longitudeString, CultureInfo.InvariantCulture);
            location.SetCountryFromGoogleResult(xmlDocument);
            return location;
        }

        public static string GetLanguageCode(string countryCode)
        {
            string result;
            if (!CountryCodeToLanguageCode.TryGetValue(countryCode, out result))
            {
                //TODO better exception if country not found           
                string uri = string.Format("https://restcountries.eu/rest/v1/alpha/" + countryCode);

                WebRequest countryInfoWebRequest = WebRequest.Create(uri);

                CountryInfo countryInfo;
                using (WebResponse response = countryInfoWebRequest.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {

                        DataContractJsonSerializer dcs = new DataContractJsonSerializer(typeof(CountryInfo));
                        countryInfo = (CountryInfo)dcs.ReadObject(stream);

                    }
                }
                result = countryInfo.languages.First();
                if (countryCode.Equals("cn", StringComparison.InvariantCultureIgnoreCase)) result = "zh-CN";

                if (countryCode.Equals("hk", StringComparison.InvariantCultureIgnoreCase)) result = "zh-TW";
                CountryCodeToLanguageCode[countryCode] = result;
            }
            return result;
        }

        public static IDictionary<string, string> CountryCodeToLanguageCode = new Dictionary<string, string>();


        public static TranslatedAddressEntity GetLocationInfo(string address, TranslatorAccess translatorAccess)
        {
            return GetLocationFromGoogle(address, translatorAccess);
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
